namespace InpliCDPClient
{
    /// <summary>
    /// 802.2 Service Access Point values.
    /// </summary>
    internal enum ESapType
    {
        /// <summary>
        /// NULL SAP
        /// </summary>
        Null = 0x00,

        /// <summary>
        /// LLC Sublayer Management.
        /// </summary>
        LLC = 0x02,

        /// <summary>
        /// SNA Path Control.
        /// </summary>
        SNA = 0x04,

        /// <summary>
        /// Proway Network Management.
        /// </summary>
        PNM = 0x0E,

        /// <summary>
        /// TCP/IP.
        /// </summary>
        IP = 0x06,

        /// <summary>
        /// Bridge Spanning Tree Proto
        /// </summary>
        BSPAN = 0x42,

        /// <summary>
        /// Manufacturing Message Srv.
        /// </summary>
        MMS = 0x4E,

        /// <summary>
        /// ISO 8208
        /// </summary>
        Iso8208 = 0x7E,

        /// <summary>
        /// 3COM.
        /// </summary>
        Net3Com = 0x80,

        /// <summary>
        /// Proway Active Station List
        /// </summary>
        PRO = 0x8E,

        /// <summary>
        /// SNAP.
        /// </summary>
        SNAP = 0xAA,

        /// <summary>
        /// Banyan.
        /// </summary>
        Banyan = 0xBC,

        /// <summary>
        /// IPX/SPX.
        /// </summary>
        IPX = 0xE0,

        /// <summary>
        /// NetBEUI.
        /// </summary>
        NetBEUI = 0xF0,

        /// <summary>
        /// LanManager.
        /// </summary>
        LanManager = 0xF4,

        /// <summary>
        /// IMPL
        /// </summary>
        IMPL = 0xF8,

        /// <summary>
        /// Discovery
        /// </summary>
        Discovery = 0xFC,

        /// <summary>
        /// OSI Network Layers.
        /// </summary>
        OSI = 0xFE,

        /// <summary>
        /// LAN Address Resolution
        /// </summary>
        LAR = 0xDC,

        /// <summary>
        /// Resource Management
        /// </summary>
        RM = 0xD4,

        /// <summary>
        /// Global SAP.
        /// </summary>
        GlobalSAP = 0xFF,
    }
}
