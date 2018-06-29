namespace libciscocdp
{
    /// <summary>
    /// Enumeration identifying network protocols used when parsing CDP packets
    /// </summary>
    internal enum ECdpAddressFamily
    {
        /// <summary>
        /// Unknown
        /// </summary>
        Unknown,

        /// <summary>
        /// Address family IPv4
        /// </summary>
        IPv4,

        /// <summary>
        /// Address family IPv6
        /// </summary>
        IPv6,
    }
}
