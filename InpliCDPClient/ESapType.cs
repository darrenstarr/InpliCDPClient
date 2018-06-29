namespace InpliCDPClient
{
    internal enum ESapType
    {
        Null = 0x00,          //  NULL SAP.
        LLC = 0x02,           //  LLC Sublayer Management.
        SNA = 0x04,           //  SNA Path Control.
        PNM = 0x0E,           //  Proway Network Management.
        IP = 0x06,            //  TCP/IP.
        BSPAN = 0x42,         //  Bridge Spanning Tree Proto
        MMS = 0x4E,           //  Manufacturing Message Srv.
        Iso8208 = 0x7E,       //  ISO 8208
        Net3Com = 0x80,       //  3COM.
        PRO = 0x8E,           //  Proway Active Station List
        SNAP = 0xAA,          //  SNAP.
        Banyan = 0xBC,        //  Banyan.
        IPX = 0xE0,           //  IPX/SPX.
        NetBEUI = 0xF0,       //  NetBEUI.
        LanManager = 0xF4,    //  LanManager.
        IMPL = 0xF8,          //  IMPL
        Discovery = 0xFC,     //  Discovery
        OSI = 0xFE,           //  OSI Network Layers.
        LAR = 0xDC,           //  LAN Address Resolution
        RM = 0xD4,            //  Resource Management
        GlobalSAP = 0xFF,     //  Global SAP.
    }
}
