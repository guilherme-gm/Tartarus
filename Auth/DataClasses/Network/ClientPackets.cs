namespace Auth.DataClasses.Network
{
    /// <summary>
    /// Packets sent by the client
    /// </summary>
    public enum ClientAuthPackets : ushort
    {
        Unknown = 0x270F,       //  9999
        Version = 0x2711,       // 10001
        Account = 0x271A,       // 10010
        ImbcAccount = 0x271C,   // 10012
        ServerList = 0x2725,    // 10021
        SelectServer = 0x2727,  // 10023
    }

    /// <summary>
    /// Packets sent by auth server
    /// </summary>
    public enum AuthClientPackets : ushort
    {
        Result = 0x2710,        // 10000
        ServerList = 0x2726,    // 10022
        SelectServer = 0x2728,  // 10024
    }
}
