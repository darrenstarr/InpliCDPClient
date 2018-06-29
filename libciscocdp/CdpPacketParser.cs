namespace libciscocdp
{
    using System;
    using System.Collections.Generic;
    using System.Net;

    /// <summary>
    /// A Cisco Discovery Protocol Packet Parser
    /// </summary>
    public static class CdpPacketParser
    {
        /// <summary>
        /// Cisco's organizationally unique identifier as used in SNAP and elsewhere
        /// </summary>
        public static readonly int CiscoOUI = 0x00000C;

        /// <summary>
        /// The EtherType/PID for Cisco Discovery Protocol
        /// </summary>
        public static readonly int CdpProtocolId = 0x2000;

        /// <summary>
        /// Attempt to parse a packet provided as a buffer starting at the SNAP header
        /// </summary>
        /// <param name="buffer">The buffer to parse.</param>
        /// <returns>A parsed structure containing the data or null upon failure</returns>
        /// <exception cref="CdpParserException">For most general errors in parsing</exception>
        /// <exception cref="CdpParserInputPastEndException">When the packet appears malformed and the input runs past the end</exception>
        public static CdpPacket Parse(byte [] buffer)
        {
            int index = 0;

            var oui = buffer.Get24(ref index, "Reading organizationally unique identifier");
            if (oui != CiscoOUI)
                throw new CdpParserException(buffer, index - 3, "SNAP packet is not a Cisco packet");

            var protocolId = buffer.Get16(ref index, "Reading protocol ID/EtherType");
            if (protocolId != CdpProtocolId)
                throw new CdpParserException(buffer, index - 2, "SNAP packet is not a CDP packet");

            var version = buffer.Get8(ref index, "Reading version");
            if (version != 2)
                throw new CdpParserException(buffer, index - 1, "CDP version 2 is the only version supported");

            var ttl = buffer.Get8(ref index, "Reading time to live");

            var checksum = buffer.Get16(ref index, "Reading checksum");
            // TODO: Validate checksum?

            var result = new CdpPacket
            {
                Version = version,
                Ttl = ttl,
            };

            while(index < buffer.Length)
            {
                var tlvType = buffer.Get16(ref index, "Reading TLV type");
                var tlvLength = buffer.Get16(ref index, "Reading TLV length");

                var indexBeforeParsingTlv = index;

                switch ((ECdpTlv)tlvType)
                {
                    case ECdpTlv.DeviceId:
                        result.DeviceId = buffer.GetString(ref index, tlvLength - 4, "Reading device type");
                        break;

                    case ECdpTlv.Addresses:
                        var addressCount = buffer.Get32(ref index, "Reading number of addresses");
                        var addresses = new List<IPAddress>();
                        for (var i = 0; i < addressCount; i++)
                        {
                            var address = buffer.GetAddress(ref index, "Reading address");
                            if(address != null)
                                addresses.Add(address);
                        }

                        result.Addresses = addresses;
                        break;

                    case ECdpTlv.PortId:
                        result.PortId = buffer.GetString(ref index, tlvLength - 4, "Reading Port ID");
                        break;

                    case ECdpTlv.Capabilities:
                        var capabilities = buffer.Get32(ref index, "Reading capabilities");
                        result.HasCapabilities = true;
                        result.CanRoute = (capabilities & 0x1) != 0;
                        result.CanTransparentBridge = (capabilities & 0x2) != 0;
                        result.CanSourceRouteBridge = (capabilities & 0x4) != 0;
                        result.CanSwitch = (capabilities & 0x8) != 0;
                        result.IsHost = (capabilities & 0x10) != 0;
                        result.IsIGMPCapable = (capabilities & 0x20) != 0;
                        result.IsRepeater = (capabilities & 0x40) != 0;
                        break;

                    case ECdpTlv.SoftwareVersion:
                        result.SoftwareVersion = buffer.GetString(ref index, tlvLength - 4, "Reading software version");
                        break;

                    case ECdpTlv.Platform:
                        result.Platform = buffer.GetString(ref index, tlvLength - 4, "Reading platform");
                        break;

                    case ECdpTlv.OdrIPPrefixes:
                        var odrPrefixCount = (tlvLength - 4) / 5;
                        var odrPrefixes = new List<IPPrefix>();
                        for (var i = 0; i < odrPrefixCount; i++)
                        {
                            var prefix = buffer.GetIPv4Prefix(ref index, "Reading IPv4 prefix");
                            if (prefix != null)
                                odrPrefixes.Add(prefix);
                        }

                        result.OdrPrefixes = odrPrefixes;
                        break;

                    case ECdpTlv.ProtocolHello:
                        var indexBeforeHello = index;
                        var hello = buffer.GetHelloProtocol(ref index, "Reading Hello Protocol");
                        index = indexBeforeHello + tlvLength - 4;

                        result.HelloProtocol = hello;
                        break;

                    case ECdpTlv.VtpManagementDomain:
                        result.VtpManagementDomain = buffer.GetString(ref index, tlvLength - 4, "Reading VTP Management Domain");
                        break;

                    case ECdpTlv.NativeVlan:
                        result.NativeVlan = buffer.Get16(ref index, "Reading Native VLAN");
                        break;

                    case ECdpTlv.Duplex:
                        result.Duplex = (ECdpDuplex) buffer.Get8(ref index, "Reading duplex");
                        break;

                    case ECdpTlv.TrustBitmap:
                        result.TrustBitmap = buffer.Get8(ref index, "Reading trust bitmap");
                        break;

                    case ECdpTlv.UntrustedPortCoS:
                        result.UntrustedPortCoS = buffer.Get8(ref index, "Reading untrusted port CoS");
                        break;

                    case ECdpTlv.ManagementAddresses:
                        var managementAddressCount = buffer.Get32(ref index, "Reading number of management addresses");
                        var managementAddresses = new List<IPAddress>();
                        for (var i = 0; i < managementAddressCount; i++)
                        {
                            var address = buffer.GetAddress(ref index, "Reading address");
                            if (address != null)
                                managementAddresses.Add(address);
                        }

                        result.ManagementAddresses = managementAddresses;
                        break;

                    case ECdpTlv.PowerAvailable:
                        result.PowerAvailable = buffer.GetPowerAvailable(ref index, "Reading power available");
                        break;

                    default:        // Unknown tlv type
                        Console.WriteLine("Unknown TLV type 0x{0:X2}", tlvType);
                        break;
                }

                index = indexBeforeParsingTlv + tlvLength - 4;
            }

            return result;
        }
    }
}
