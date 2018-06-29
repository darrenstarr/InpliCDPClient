namespace libciscocdp
{
    /// <summary>
    /// Representation of data reported by CDP about Power Over Ethernet
    /// </summary>
    public class CdpPowerAvailable
    {
        public int RequestId { get; internal set; }
        public int ManagementId { get; internal set; }
        public uint PowerAvailable { get; internal set; }
        public uint PowerAvailable2 { get; internal set; }
    }
}
