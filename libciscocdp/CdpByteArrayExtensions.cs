namespace libciscocdp
{
    using System.Linq;
    using System.Net;
    using System.Net.NetworkInformation;
    using System.Text;

    /// <summary>
    /// Internal methods to extend C# byte arrays for parsing CDP packets.
    /// </summary>
    internal static class CdpByteArrayExtensions
    {
        internal static bool Need(this byte [] buffer, int index, int bytesNeeded)
        {
            return (index + bytesNeeded) <= buffer.Length;
        }

        internal static int Get8(this byte[] buffer, ref int index, string operation)
        {
            if (!buffer.Need(index, 1))
                throw new CdpParserInputPastEndException(buffer, index, operation, "Input past end, needed 1 bytes");

            var result =
                buffer[index];

            index++;

            return result;
        }

        internal static int Get16(this byte[] buffer, ref int index, string operation)
        {
            if (!buffer.Need(index, 2))
                throw new CdpParserInputPastEndException(buffer, index, operation, "Input past end, needed 2 bytes");

            var result =
                buffer[index] << 8 |
                buffer[index + 1];

            index += 2;

            return result;
        }

        internal static int Get24(this byte[] buffer, ref int index, string operation)
        {
            if (!buffer.Need(index, 3))
                throw new CdpParserInputPastEndException(buffer, index, operation, "Input past end, needed 3 bytes");

            var result =
                buffer[index] << 16 |
                buffer[index + 1] << 8 |
                buffer[index + 2];

            index += 3;

            return result;
        }

        internal static int Get32(this byte[] buffer, ref int index, string operation)
        {
            if (!buffer.Need(index, 4))
                throw new CdpParserInputPastEndException(buffer, index, operation, "Input past end, needed 4 bytes");

            var result =
                buffer[index] << 24 |
                buffer[index + 1] << 16 |
                buffer[index + 2] << 8 |
                buffer[index + 3];

            index += 4;

            return result;
        }

        internal static uint Get32U(this byte[] buffer, ref int index, string operation)
        {
            if (!buffer.Need(index, 4))
                throw new CdpParserInputPastEndException(buffer, index, operation, "Input past end, needed 4 bytes");

            var result =
                (uint)buffer[index] << 24 |
                (uint)buffer[index + 1] << 16 |
                (uint)buffer[index + 2] << 8 |
                (uint)buffer[index + 3];

            index += 4;

            return result;
        }

        internal static int GetVariableLengthInt(this byte [] buffer, ref int index, int length, string operation)
        {
            switch(length)
            {
                case 1:
                    return Get8(buffer, ref index, operation);
                case 2:
                    return Get16(buffer, ref index, operation);
                case 3:
                    return Get24(buffer, ref index, operation);
                case 4:
                    return Get32(buffer, ref index, operation);
                default:
                    throw new CdpParserException(buffer, index, "Invalid protocol length specified");
            }
        }

        internal static string GetString(this byte[] buffer, ref int index, int length, string operation)
        {
            if (!buffer.Need(index, length))
                throw new CdpParserInputPastEndException(buffer, index, operation, "Input past end, needed " + length.ToString() + " bytes");

            var result = Encoding.UTF8.GetString(buffer, index, length);

            index += length;

            return result;
        }

        internal static byte [] GetSubbuffer(this byte[] buffer, ref int index, int length, string operation)
        {
            if (!buffer.Need(index, length))
                throw new CdpParserInputPastEndException(buffer, index, operation, "Input past end, needed " + length.ToString() + " bytes");

            var result = buffer.Skip(index).Take(length).ToArray();

            index += length;

            return result;
        }

        internal static ECdpAddressFamily GetProtocolFromSapSnap(this byte [] buffer, ref int index, int protocolLength, string operation)
        {
            var dsapAddress = buffer.Get8(ref index, "Reading DSAP address");
            if (dsapAddress != 0xAA)
                throw new CdpParserException(buffer, index - 1, "Header is not SNAP");

            var ssapAddress = buffer.Get8(ref index, "Reading SSAP address");
            if (ssapAddress != 0xAA)
                throw new CdpParserException(buffer, index - 1, "Header is not SNAP");

            var control = buffer.Get8(ref index, "Reading control field");

            var oui = buffer.Get24(ref index, "Reading SNAP OUI");
            if (oui != 0x000000)
                throw new CdpParserException(buffer, index - 3, "OUI is not generic");

            var ethertype = buffer.Get16(ref index, "Reading Ethertype/PID from SNAP");
            switch(ethertype)
            {
                case 0x0800:        // IPv4
                    return ECdpAddressFamily.IPv4;

                case 0x86DD:        // IPv6
                    return ECdpAddressFamily.IPv6;
            }

            return ECdpAddressFamily.Unknown;
        }

        internal static ECdpAddressFamily GetProtocolType(this byte [] buffer, ref int index, string operation)
        {
            var protocolType = Get8(buffer, ref index, "Reading protocol type");
            var protocolLength = Get8(buffer, ref index, "Reading protocol length");

            switch(protocolType)
            {
                case 0x01:      // NLPID
                    var protocolID = GetVariableLengthInt(buffer, ref index, protocolLength, "Reading NLPID protocol ID");
                    switch(protocolID)
                    {
                        case 0xCC:      // IPv4 (RFC6328)
                            return ECdpAddressFamily.IPv4;

                        default:
                            return ECdpAddressFamily.Unknown;
                    }

                case 0x02:      // 802.2
                    return buffer.GetProtocolFromSapSnap(ref index, protocolLength, "Reading protocol ID from SAP/SNAP 802.2");
            }

            
            return ECdpAddressFamily.Unknown; ;
        }

        internal static IPAddress GetAddress(this byte [] buffer, ref int index, string operation)
        {
            var protocolType = GetProtocolType(buffer, ref index, "Reading protocol type");
            var addressLength = Get16(buffer, ref index, "Reading address length");

            switch(protocolType)
            {
                case ECdpAddressFamily.IPv4:
                    return new IPAddress(buffer.GetSubbuffer(ref index, 4, "Reading IPv4 address"));

                case ECdpAddressFamily.IPv6:
                    return new IPAddress(buffer.GetSubbuffer(ref index, 16, "Reading IPv6 address"));

                default:
                    return null;
            }
        }

        internal static CdpHelloProtocol GetHelloProtocol(this byte [] buffer, ref int index, string operation)
        {
            var result = new CdpHelloProtocol();

            result.Oui = buffer.Get24(ref index, "Reading OUI");

            result.ProtocolId = buffer.Get16(ref index, "Protocol ID");
            if (result.ProtocolId != 0x0112)
                throw new CdpParserException(buffer, index - 2, "Unknown CDP hello protocol");

            result.ClusterMasterIP = new IPAddress(buffer.GetSubbuffer(ref index, 4, "Reading cluster master IP"));

            result.IP = new IPAddress(buffer.GetSubbuffer(ref index, 4, "Reading IP"));

            result.Version = buffer.Get8(ref index, "Reading version");
            if (result.Version != 1)
                throw new CdpParserException(buffer, index - 1, "Unknown CDP hello protocol version");

            result.Subversion = buffer.Get8(ref index, "Reading subversion");
            if (result.Subversion != 2)
                throw new CdpParserException(buffer, index - 1, "Unknown CDP hello protocol subversion");

            result.Status = buffer.Get8(ref index, "Getting status code");

            buffer.Get8(ref index, "Getting first unknown variable");

            result.ClusterCommanderMac = new PhysicalAddress(buffer.GetSubbuffer(ref index, 6, "Getting cluster commander MAC"));

            result.SwitchMac = new PhysicalAddress(buffer.GetSubbuffer(ref index, 6, "Getting switch MAC"));

            buffer.Get8(ref index, "Getting second unknown variable");

            result.ManagementVlan = buffer.Get16(ref index, "Getting Management VLAN");

            return result;
        }

        internal static CdpPowerAvailable GetPowerAvailable(this byte [] buffer, ref int index, string operation)
        {
            var result = new CdpPowerAvailable();
            result.RequestId = buffer.Get16(ref index, "Reading request ID");
            result.ManagementId = buffer.Get16(ref index, "Reading management ID");
            result.PowerAvailable = buffer.Get32U(ref index, "Reading power available");
            result.PowerAvailable2 = buffer.Get32U(ref index, "Reading second power available");

            return result;
        }

        internal static IPPrefix GetIPv4Prefix(this byte [] buffer, ref int index, string operation)
        {
            var result = new IPPrefix();
            result.Network = new IPAddress(buffer.GetSubbuffer(ref index, 4, "Reading the network of a prefix"));
            result.Length = buffer.Get8(ref index, "Reading the length of a prefix");

            return result;
        }
    }
}
