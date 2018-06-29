namespace libciscocdp
{
    using System.Net;
    using System.Net.NetworkInformation;

    public class CdpHelloProtocol
    {
        public int Oui { get; internal set; }
        public int ProtocolId { get; internal set; }
        public IPAddress ClusterMasterIP { get; internal set; }
        public IPAddress IP { get; internal set; }
        public int Version { get; internal set; }
        public int Subversion { get; internal set; }
        public int Status { get; internal set; }
        public PhysicalAddress ClusterCommanderMac { get; internal set; }
        public PhysicalAddress SwitchMac { get; internal set; }
        public int ManagementVlan { get; internal set; }
    }
}
