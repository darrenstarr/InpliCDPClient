namespace InpliCDPClient
{
    using libciscocdp;
    using System;

    /// <summary>
    /// Message model transmitted via REST to the server
    /// </summary>
    public class CdpApiMessage
    {
        /// <summary>
        /// The interface name which the packet was received on
        /// </summary>
        public string ReceivedOn { get; set; }

        /// <summary>
        /// The date and time which the packet was received
        /// </summary>
        public DateTimeOffset ReceivedAt { get; set; }

        /// <summary>
        /// The transmitting device's source MAC address
        /// </summary>
        public string SourceMac { get; set; }

        /// <summary>
        /// The contents of the parsed packet
        /// </summary>
        public CdpPacket Packet { get; set; }
    }
}
