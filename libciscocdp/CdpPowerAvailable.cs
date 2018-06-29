namespace libciscocdp
{
    public class CdpPowerAvailable
    {
        public int RequestId { get; internal set; }
        public int ManagementId { get; internal set; }
        public uint PowerAvailable { get; internal set; }
        public uint PowerAvailable2 { get; internal set; }
    }
}
