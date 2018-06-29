namespace InpliCDPClient
{
    using System;
    using System.Linq;
    using System.Net.NetworkInformation;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Abstraction of the UNIX socket commands needed to perform Address Family LLC and SNAP communication
    /// </summary>
    internal class LLCSocket : IDisposable
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

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="interfaceAddress">The MAC address of the interface to receive the data on</param>
        public LLCSocket(PhysicalAddress interfaceAddress)
        {
            SocketHandle = socket(EAddressFamily.LLC, ESocketType.Datagram, EProtocol.Null);
            if (SocketHandle < 0)
                throw new Exception("Failed to create socket for the LLC protocol family. Either permissions or missing kernel module (llc2.o)");

            //Console.WriteLine("Socket called " + SocketHandle.ToString());

            var socketAddress = new SocketAddressLLC
            {
                sllc_family = EAddressFamily.LLC,
                sllc_arphrd = EAddressFamily.EthernetHardwareAddress,
                sllc_sap = (byte)ESapType.SNAP,
            };
            socketAddress.MacAddress = interfaceAddress;

            var result = bind(SocketHandle, ref socketAddress, (IntPtr)16);
            if (result < 0)
                throw new Exception("Error binding the socket to the LLC/SNAP protocol. Either permissions or missing module (llc2.o).");

            //Console.WriteLine("Bind called " + result.ToString());
        }

        /// <summary>
        /// Registers a multicast address as a receiver address on the given interface
        /// </summary>
        /// <param name="name">The name of the interface</param>
        /// <param name="address">The address to register with the interface</param>
        public void RegisterMacOnInterface(string name, PhysicalAddress address)
        {
            var req = new InterfaceRequestMac();
            req.Name = name;
            req.ifru_hwaddr.MacAddress = address;

            var result = ioctl(SocketHandle, (int)ESocketIOCTL.ADDMULTI, ref req);
            if (result < 0)
                throw new Exception("Error registering multicast MAC address to listen to on the interface " + name);

            //Console.WriteLine("ioctl called " + result.ToString());
        }

        /// <summary>
        /// Receive data from the remote host. This uses a fixed size buffer of 2000 bytes which should be safe for 802.2 packets.
        /// </summary>
        /// <param name="remoteHost">Return value of the remote host the the frame was received from</param>
        /// <returns>The buffer received from the remote host</returns>
        public byte [] ReceiveFrom(out PhysicalAddress remoteHost)
        {
            var buffer = new byte[2000];
            var destinationAddress = new SocketAddressLLC();
            IntPtr addressLength = (IntPtr) 16;
            var result = recvfrom(SocketHandle, buffer, (IntPtr)2000, 0, ref destinationAddress, ref addressLength);

            //Console.WriteLine("recvfrom called " + result.ToString());

            if ((long)result <= 0)
            {
                remoteHost = null;
                return null;
            }

            // TODO : Use map instead
            remoteHost = destinationAddress.MacAddress;

            return buffer.Take((int)result).ToArray();
        }

        /// <summary>
        /// For the IDisposableInterface
        /// </summary>
        public void Dispose()
        {
            if (SocketHandle != -1)
                close(SocketHandle);

            SocketHandle = -1;
        }
    }
}
