namespace InpliCDPClient
{
    /// <summary>
    /// Socket IOCTL values
    /// </summary>
    internal enum ESocketIOCTL
    {
        // #define SIOCINQ         FIONREAD
        // #define SIOCOUTQ        TIOCOUTQ        /* output queue size (not sent + not acked) */

        /* Routing table calls. */
        ADDRT = 0x890B,                 // add routing table entry
        DELRT = 0x890C,                 // delete routing table entry
        RTMSG = 0x890D,                 // call to routing system

        /* Socket configuration controls. */
        GIFNAME = 0x8910,               // get iface name
        SIFLINK = 0x8911,               // set iface channel
        GIFCONF = 0x8912,               // get iface list
        GIFFLAGS = 0x8913,              // get flags
        SIFFLAGS = 0x8914,              // set flags
        GIFADDR = 0x8915,               // get PA address
        SIFADDR = 0x8916,               // set PA address
        GIFDSTADDR = 0x8917,            // get remote PA address
        SIFDSTADDR = 0x8918,            // set remote PA address
        GIFBRDADDR = 0x8919,            // get broadcast PA address
        SIFBRDADDR = 0x891a,            // set broadcast PA address
        GIFNETMASK = 0x891b,            // get network PA mask
        SIFNETMASK = 0x891c,            // set network PA mask
        GIFMETRIC = 0x891d,             // get metric
        SIFMETRIC = 0x891e,             // set metric
        GIFMEM = 0x891f,                // get memory address (BSD)
        SIFMEM = 0x8920,                // set memory address (BSD)
        GIFMTU = 0x8921,                // get MTU size
        SIFMTU = 0x8922,                // set MTU size
        SIFNAME = 0x8923,               // set interface name
        SIFHWADDR = 0x8924,             // set hardware address
        GIFENCAP = 0x8925,              // get/set encapsulations
        SIFENCAP = 0x8926,              // 
        GIFHWADDR = 0x8927,             // Get hardware address
        GIFSLAVE = 0x8929,              // Driver slaving support
        SIFSLAVE = 0x8930,              // 
        ADDMULTI = 0x8931,              // Multicast address lists
        DELMULTI = 0x8932,              // 
        GIFINDEX = 0x8933,              // name -> if_index mapping
        SIFPFLAGS = 0x8934,             // set/get extended flags set
        GIFPFLAGS = 0x8935,             // 
        DIFADDR = 0x8936,               // delete PA address
        SIFHWBROADCAST = 0x8937,        // set hardware broadcast addr
        GIFCOUNT = 0x8938,              // get number of devices

        GIFBR = 0x8940,                 // Bridging support
        SIFBR = 0x8941,                 // Set bridging options

        GIFTXQLEN = 0x8942,             // Get the tx queue length
        SIFTXQLEN = 0x8943,             // Set the tx queue length

        /* SIOCGIFDIVERT was:   0x8944          Frame diversion support */
        /* SIOCSIFDIVERT was:   0x8945          Set frame diversion options */

        ETHTOOL = 0x8946,               // Ethtool interface

        GMIIPHY = 0x8947,               // Get address of MII PHY in use.
        GMIIREG = 0x8948,               // Read MII PHY register.
        SMIIREG = 0x8949,               // Write MII PHY register.

        WANDEV = 0x894A,                // get/set netdev parameters

        OUTQNSD = 0x894B,               // output queue size (not sent only)

        /* ARP cache control calls. */
        /*  0x8950 - 0x8952  * obsolete calls, don't re-use */
        DARP = 0x8953,                  // delete ARP table entry
        GARP = 0x8954,                  // get ARP table entry
        SARP = 0x8955,                  // set ARP table entry

        /* RARP cache control calls. */
        DRARP = 0x8960,                 // delete RARP table entry
        GRARP = 0x8961,                 // get RARP table entry
        SRARP = 0x8962,                 // set RARP table entry

        /* Driver configuration calls */
        GIFMAP = 0x8970,                // Get device parameters
        SIFMAP = 0x8971,                // Set device parameters

        /* DLCI configuration calls */
        ADDDLCI = 0x8980,               // Create new DLCI device
        DELDLCI = 0x8981,               // Delete DLCI device

        GIFVLAN = 0x8982,               // 802.1Q VLAN support
        SIFVLAN = 0x8983,               // Set 802.1Q VLAN options

        /* bonding calls */
        BONDENSLAVE = 0x8990,           // enslave a device to the bond
        BONDRELEASE = 0x8991,           // release a slave from the bond
        BONDSETHWADDR = 0x8992,         // set the hw addr of the bond
        BONDSLAVEINFOQUERY = 0x8993,    // rtn info about slave state
        BONDINFOQUERY = 0x8994,         // rtn info about bond state
        BONDCHANGEACTIVE = 0x8995,      // update to a new active slave

        /* bridge calls */
        BRADDBR = 0x89a0,               // create new bridge device
        BRDELBR = 0x89a1,               // remove bridge device
        BRADDIF = 0x89a2,               // add interface to bridge
        BRDELIF = 0x89a3,               // remove interface from bridge

        /* hardware time stamping: parameters in linux/net_tstamp.h */
        SHWTSTAMP = 0x89b0,             // set and get config
        GHWTSTAMP = 0x89b1,             // get config
    }
}
