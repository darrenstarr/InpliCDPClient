namespace libciscocdp
{
    /// <summary>
    /// Cisco Discovery Protocol TLV values currently known and decoded by this client
    /// </summary>
    internal enum ECdpTlv
    {
        DeviceId = 0x01,
        Addresses = 0x02,
        PortId = 0x03,
        Capabilities = 0x04,
        SoftwareVersion = 0x05,
        Platform = 0x06,
        OdrIPPrefixes = 0x07,
        ProtocolHello = 0x08,
        VtpManagementDomain = 0x09,
        NativeVlan = 0x0A,
        Duplex = 0x0B,
        TrustBitmap = 0x12,
        UntrustedPortCoS = 0x13,
        ManagementAddresses = 0x16,
        PowerAvailable = 0x1A,
    }
}
