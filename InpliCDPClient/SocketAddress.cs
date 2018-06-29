namespace InpliCDPClient
{
    using System.Net.NetworkInformation;
    using System.Runtime.InteropServices;

    /// <summary>
    /// A representation of the Linux sock_addr structure
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = 16, Pack = 1)]
    internal unsafe struct SocketAddress
    {
        [FieldOffset(0)]
        public EAddressFamily sa_family;

        [FieldOffset(2)]
        public fixed byte sa_data[14];

        // Converts the contents of the sa_data from a byte array to a .NET PhysicalAddress
        public PhysicalAddress MacAddress
        {
            get
            {
                var mac = new byte[6];

                fixed (SocketAddress* p = &this)
                {
                    for (var i = 0; i < 6; i++)
                        mac[i] = p->sa_data[i];
                }

                return new PhysicalAddress(mac);
            }

            set
            {
                var asBytes = value.GetAddressBytes();
                fixed (SocketAddress* p = &this)
                {
                    for (var i = 0; i < 6; i++)
                        p->sa_data[i] = asBytes[i];
                }
            }
        }
    }
}
