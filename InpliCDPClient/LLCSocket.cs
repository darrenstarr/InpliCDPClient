namespace InpliCDPClient
{
    using System;
    using System.Linq;
    using System.Net.NetworkInformation;
    using System.Runtime.InteropServices;

    internal unsafe class LLCSocket : IDisposable
    {
        [DllImport("libc.so.6")]
        private static extern int socket(EAddressFamily domain, ESocketType socketType, EProtocol protocol);

        [DllImport("libc.so.6")]
        private static extern int close(int fd);

        [DllImport("libc.so.6")]
        private static extern int bind(int fd, ref SocketAddressLLC address, IntPtr addressLength);

        [DllImport("libc.so.6")]
        private static extern int ioctl(int d, int request, ref InterfaceRequestMac macRequest);

        [DllImport("libc.so.6")]
        private static extern IntPtr recvfrom(int sockfd, byte [] buf, IntPtr len, int flags, ref SocketAddressLLC src_addr, ref IntPtr addrlen);

        private int SocketHandle = -1;

        public LLCSocket(PhysicalAddress interfaceAddress)
        {
            SocketHandle = socket(EAddressFamily.LLC, ESocketType.Datagram, EProtocol.Null);
            Console.WriteLine("Socket called " + SocketHandle.ToString());

            var x = new SocketAddressLLC
            {
                sllc_family = EAddressFamily.LLC,
                sllc_arphrd = EAddressFamily.EthernetHardwareAddress,
                sllc_sap = (byte)ESapType.SNAP,
            };
            x.SetMac(interfaceAddress);

            var result = bind(SocketHandle, ref x, (IntPtr)16);
            Console.WriteLine("Bind called " + result.ToString());
        }

        public void RegisterMacOnInterface(string name, PhysicalAddress address)
        {
            var req = new InterfaceRequestMac();
            req.SetName(name);
            req.ifru_hwaddr.SetMac(address);

            var result = ioctl(SocketHandle, (int)ESocketIOCTL.ADDMULTI, ref req);

            Console.WriteLine("ioctl called " + result.ToString());
        }

        public byte [] ReceiveFrom(out PhysicalAddress remoteHost)
        {
            var buffer = new byte[2000];
            var destinationAddress = new SocketAddressLLC();
            IntPtr addressLength = (IntPtr) 16;
            var result = recvfrom(SocketHandle, buffer, (IntPtr)2000, 0, ref destinationAddress, ref addressLength);

            Console.WriteLine("recvfrom called " + result.ToString());

            if ((long)result <= 0)
            {
                remoteHost = null;
                return null;
            }

            // TODO : Use map instead
            remoteHost = destinationAddress.GetMac();
            return buffer.Take((int)result).ToArray();
        }

        public void Dispose()
        {
            if (SocketHandle != -1)
                close(SocketHandle);

            SocketHandle = -1;
        }
    }
}
