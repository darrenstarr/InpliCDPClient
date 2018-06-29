namespace InpliCDPClient
{
    using System;
    using System.Runtime.InteropServices;
    using System.Text;

    /// <summary>
    /// C# P/Invoke representation of /usr/include/net/if.h ifreq
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal unsafe struct InterfaceRequestMac
    {
        [FieldOffset(0)]
        public fixed byte ifrn_name[16];

        [FieldOffset(16)]
        public SocketAddress ifru_hwaddr;

        /// <summary>
        /// Abstraction for getting/setting the interface name
        /// </summary>
        public string Name
        {
            get
            {
                fixed (InterfaceRequestMac* p = &this)
                {
                    var index = 0;
                    var buffer = new byte[16];

                    while (index < 16 && p->ifrn_name[index] != 0)
                    {
                        buffer[index] = p->ifrn_name[index];
                        index++;
                    }

                    return Encoding.ASCII.GetString(buffer, 0, index);
                }
            }
            
            set
            {
                var asBytes = Encoding.ASCII.GetBytes(value);
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
}
