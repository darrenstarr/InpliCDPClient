namespace libciscocdp
{
    using System.Collections.Generic;
    using System.Net;

    public class CdpPacket
    {
        public int Version { get; internal set; }
        public int Ttl { get; internal set; }
        public string DeviceId { get; internal set; }
        public string SoftwareVersion { get; internal set; }
        public string Platform { get; internal set; }
        public List<IPAddress> Addresses { get; internal set; }
        public string PortId { get; internal set; }
        public bool HasCapabilities { get; set; }
        public bool CanRoute { get; internal set; }
        public bool CanTransparentBridge { get; internal set; }
        public bool CanSourceRouteBridge { get; internal set; }
        public bool CanSwitch { get; internal set; }
        public bool IsHost { get; internal set; }
        public bool IsIGMPCapable { get; internal set; }
        public bool IsRepeater { get; internal set; }
        public CdpHelloProtocol HelloProtocol { get; internal set; }
        public int? NativeVlan { get; internal set; }
        public string VtpManagementDomain { get; internal set; }
        public ECdpDuplex? Duplex { get; internal set; }
        public int? TrustBitmap { get; internal set; }
        public int? UntrustedPortCoS { get; internal set; }
        public List<IPAddress> ManagementAddresses { get; internal set; }
        public CdpPowerAvailable PowerAvailable { get; internal set; }
        public List<IPPrefix> OdrPrefixes { get; internal set; }
    }
}
