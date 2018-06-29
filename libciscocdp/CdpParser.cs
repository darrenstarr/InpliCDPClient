namespace libciscocdp
{
    using System;
    using System.Collections.Generic;
    using System.Net;

    public static class CdpParser
    {
        public static readonly int CiscoOUI = 0x00000C;
        public static readonly int CdpProtocolId = 0x2000;

        public static CdpPacket Parse(byte [] buffer)
        {
            int index = 0;

            var oui = buffer.Get24(ref index, "Reading organizationally unique identifier");
            if (oui != CiscoOUI)
                throw new ParserException(buffer, index - 3, "SNAP packet is not a Cisco packet");

            var protocolId = buffer.Get16(ref index, "Reading protocol ID/EtherType");
            if (protocolId != CdpProtocolId)
                throw new ParserException(buffer, index - 2, "SNAP packet is not a CDP packet");

            var version = buffer.Get8(ref index, "Reading version");
            if (version != 2)
                throw new ParserException(buffer, index - 1, "CDP version 2 is the only version supported");

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

                switch (tlvType)
                {
                    case 0x01:      // Device ID
                        result.DeviceId = buffer.GetString(ref index, tlvLength - 4, "Reading device type");
                        break;

                    case 0x02:      // Addresses
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

                    case 0x03:      // Port ID
                        result.PortId = buffer.GetString(ref index, tlvLength - 4, "Reading Port ID");
                        break;

                    case 0x04:      // Capabilities
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

                    case 0x05:      // Software Version
                        result.SoftwareVersion = buffer.GetString(ref index, tlvLength - 4, "Reading software version");
                        break;

                    case 0x06:      // Platform
                        result.Platform = buffer.GetString(ref index, tlvLength - 4, "Reading platform");
                        break;

                    case 0x07:      // ODR IP Prefixes
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

                    case 0x08:      // Protocol Hello
                        var indexBeforeHello = index;
                        var hello = buffer.GetHelloProtocol(ref index, "Reading Hello Protocol");
                        index = indexBeforeHello + tlvLength - 4;

                        result.HelloProtocol = hello;
                        break;

                    case 0x09:
                        result.VtpManagementDomain = buffer.GetString(ref index, tlvLength - 4, "Reading VTP Management Domain");
                        break;

                    case 0x0A:      // Native VLAN
                        result.NativeVlan = buffer.Get16(ref index, "Reading Native VLAN");
                        break;

                    case 0x0B:      // Duplex
                        result.Duplex = (ECdpDuplex) buffer.Get8(ref index, "Reading duplex");
                        break;

                    case 0x12:      // Trust Bitmap
                        result.TrustBitmap = buffer.Get8(ref index, "Reading trust bitmap");
                        break;

                    case 0x13:      // Untrusted port CoS
                        result.UntrustedPortCoS = buffer.Get8(ref index, "Reading untrusted port CoS");
                        break;

                    case 0x16:      // Management addresses
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

                    case 0x1A:      // Power available
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
