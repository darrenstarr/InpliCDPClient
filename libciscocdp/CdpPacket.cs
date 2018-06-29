namespace libciscocdp
{
    using System.Collections.Generic;
    using System.Net;

    /// <summary>
    /// A Cisco Discovery Protocol packet model
    /// </summary>
    public class CdpPacket
    {
        /// <summary>
        /// The version of the CDP packet
        /// </summary>
        public int Version { get; internal set; }

        /// <summary>
        /// The TTL value for the packet in seconds
        /// </summary>
        public int Ttl { get; internal set; }

        /// <summary>
        /// The device ID as reported by the device.
        /// </summary>
        public string DeviceId { get; internal set; }

        /// <summary>
        /// The software version running on the device
        /// </summary>
        public string SoftwareVersion { get; internal set; }

        /// <summary>
        /// The platform reported by the device
        /// </summary>
        public string Platform { get; internal set; }

        /// <summary>
        /// The addresses configured on the device
        /// </summary>
        public List<IPAddress> Addresses { get; internal set; }

        /// <summary>
        /// The port on the device which the message was transmitted from
        /// </summary>
        public string PortId { get; internal set; }

        /// <summary>
        /// True if capabilities are provided by the transmitter (not part of the packet)
        /// </summary>
        public bool HasCapabilities { get; set; }

        /// <summary>
        /// Can the device operate as a Layer-3 router?
        /// </summary>
        public bool CanRoute { get; internal set; }

        /// <summary>
        /// Can the device operate as a Layer-2 transparent bridge (uncommon)
        /// </summary>
        public bool CanTransparentBridge { get; internal set; }

        /// <summary>
        /// Can the device operate as a source routing bridge (uncommon)
        /// </summary>
        public bool CanSourceRouteBridge { get; internal set; }

        /// <summary>
        /// Can the device operate as a layer-2 switch
        /// </summary>
        public bool CanSwitch { get; internal set; }

        /// <summary>
        /// Is the device a host or endpoint
        /// </summary>
        public bool IsHost { get; internal set; }

        /// <summary>
        /// Is the device capable of processing or forwarding IGMP packets
        /// </summary>
        public bool IsIGMPCapable { get; internal set; }

        /// <summary>
        /// Is the device a network repeater
        /// </summary>
        public bool IsRepeater { get; internal set; }

        /// <summary>
        /// The CDP Hello Protocol
        /// </summary>
        public CdpHelloProtocol HelloProtocol { get; internal set; }

        /// <summary>
        /// The native VLAN reported by the device for the link
        /// </summary>
        public int? NativeVlan { get; internal set; }

        /// <summary>
        /// The VTP management domain, this can be empty
        /// </summary>
        public string VtpManagementDomain { get; internal set; }

        /// <summary>
        /// The duplex the transmitting device is operating on for this link
        /// </summary>
        public ECdpDuplex? Duplex { get; internal set; }

        /// <summary>
        /// Trust bitmap (not sure what this is)
        /// </summary>
        public int? TrustBitmap { get; internal set; }

        /// <summary>
        /// Untrusted Port Class of Service (not sure what this is)
        /// </summary>
        public int? UntrustedPortCoS { get; internal set; }

        /// <summary>
        /// The management addresses of the device
        /// </summary>
        public List<IPAddress> ManagementAddresses { get; internal set; }

        /// <summary>
        /// Power of Ethernet availability information
        /// </summary>
        public CdpPowerAvailable PowerAvailable { get; internal set; }

        /// <summary>
        /// IPv4 Prefixes provided for IPv4 "On-Demand Routing"
        /// </summary>
        public List<IPPrefix> OdrPrefixes { get; internal set; }
    }
}
