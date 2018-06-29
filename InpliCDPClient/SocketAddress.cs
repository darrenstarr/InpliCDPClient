namespace InpliCDPClient
{
    using System.Net.NetworkInformation;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Explicit, Size = 16, Pack = 1)]
    internal unsafe struct SocketAddress
    {
        [FieldOffset(0)]
        public EAddressFamily sa_family;

        [FieldOffset(2)]
        public fixed byte sa_data[6];

        public void SetMac(PhysicalAddress macAddress)
        {
            sa_family = EAddressFamily.Unspecified;
            var asBytes = macAddress.GetAddressBytes();
            fixed (SocketAddress* p = &this)
            {
                for (var i = 0; i < 6; i++)
                    p->sa_data[i] = asBytes[i];
            }
        }
    }
}
