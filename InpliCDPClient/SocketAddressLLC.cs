namespace InpliCDPClient
{
    using System.Net.NetworkInformation;
    using System.Runtime.InteropServices;

    /// <summary>
    /// C# P/Invoke model of /usr/include/linux/llc.h sockaddr_llc
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = 16, Pack = 1)]
    internal unsafe struct SocketAddressLLC
    {
        [FieldOffset(0)]
        public EAddressFamily sllc_family;
        [FieldOffset(2)]
        public EAddressFamily sllc_arphrd;
        [FieldOffset(4)]
        public byte sllc_test;
        [FieldOffset(5)]
        public byte sllc_xid;
        [FieldOffset(6)]
        public byte sllc_ua;
        [FieldOffset(7)]
        public byte sllc_sap;
        [FieldOffset(8)]
        public fixed byte sllc_mac[6];

        // Converts the contents of the sllc_mac from a byte array to a .NET PhysicalAddress
        public PhysicalAddress MacAddress
        {
            get
            {
                var mac = new byte[6];

                fixed (SocketAddressLLC* p = &this)
                {
                    for (var i = 0; i < 6; i++)
                        mac[i] = p->sllc_mac[i];
                }

                return new PhysicalAddress(mac);
            }

            set
            {
                var asBytes = value.GetAddressBytes();
                fixed (SocketAddressLLC* p = &this)
                {
                    for (var i = 0; i < 6; i++)
                        p->sllc_mac[i] = asBytes[i];
                }
            }
        }
    }
}
