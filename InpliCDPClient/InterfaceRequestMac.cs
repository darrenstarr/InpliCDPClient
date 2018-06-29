namespace InpliCDPClient
{
    using System;
    using System.Runtime.InteropServices;
    using System.Text;

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal unsafe struct InterfaceRequestMac
    {
        [FieldOffset(0)]
        public fixed byte ifrn_name[16];

        [FieldOffset(16)]
        public SocketAddress ifru_hwaddr;

        public void SetName(string name)
        {
            var asBytes = Encoding.ASCII.GetBytes(name);
            if (asBytes.Length > 15)
                throw new ArgumentException("Interface name is longer than 15 bytes", "name");

            fixed (InterfaceRequestMac* p = &this)
            {
                for (var i = 0; i < asBytes.Length; i++)
                    p->ifrn_name[i] = asBytes[i];

                p->ifrn_name[asBytes.Length] = 0;
            }
        }
    }
}
