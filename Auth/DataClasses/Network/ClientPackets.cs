/**
* This file is part of Tartarus Emulator.
* 
* Tartarus is free software: you can redistribute it and/or modify
* it under the terms of the GNU General Public License as published by
* the Free Software Foundation, either version 3 of the License, or
* (at your option) any later version.
* 
* Tartarus is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
* GNU General Public License for more details.
* 
* You should have received a copy of the GNU General Public License
* along with Tartarus.  If not, see<http://www.gnu.org/licenses/>.
*/
namespace Auth.DataClasses.Network
{
    #region Client to Auth / Auth To Client - Packets

    #region Packets Sent by Client
    /// <summary>
    /// Packets sent by the client
    /// </summary>
    public enum ClientAuthPackets : ushort
    {
        RSAPublicKey = 0x0047,  //    71
        Unknown = 0x270F,       //  9999
        Version = 0x2711,       // 10001
        Account = 0x271A,       // 10010
        ImbcAccount = 0x271C,   // 10012
        ServerList = 0x2725,    // 10021
        SelectServer = 0x2727,  // 10023
    }
    #endregion

    #region Packets Sent by Server
    /// <summary>
    /// Packets sent by auth server
    /// </summary>
    public enum AuthClientPackets : ushort
    {
        AesKeyIV = 0x0048,      //    72
        Result = 0x2710,        // 10000
        ServerList = 0x2726,    // 10022
        SelectServer = 0x2728,  // 10024
    }
    #endregion

    #endregion
}
