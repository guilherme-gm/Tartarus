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
using Auth.Business;
using Auth.DataClasses;
using Auth.DataClasses.Network;
using Common.DataClasses;
using Common.DataClasses.Network;
using Common.Utils;
using System;

using CA = Auth.DataClasses.Network.ClientAuth;

namespace Auth.Helpers
{
    #region ClientCommandHelper
    public static class ClientCommandHelper
    {
        #region GetCommand
        public static ICommand GetCommand(Session session, byte[] packet, out Packet message)
        {
            #region Packet Switch Case
            ClientAuthPackets packetId = (ClientAuthPackets)BitConverter.ToUInt16(packet, 4);

            switch (packetId)
            {
                case ClientAuthPackets.Unknown:
                    message = null;
                    return null;
                case ClientAuthPackets.RSAPublicKey:
                    message = new CA.RSAPublicKey();
                    return new Business.Client.RSAPublicKey();
                case ClientAuthPackets.Version:
                    message = new CA.Version();
                    return new Business.Client.Version();
                case ClientAuthPackets.Account:
                    if (((AuthSession)session).UsesAes)
                        message = new CA.AccountAes();
                    else
                        message = new CA.AccountDes();
                    return new Business.Client.Login();
                case ClientAuthPackets.ImbcAccount:
                    message = new CA.ImbcAccount();
                    return new Business.Client.ImbcLogin();
                case ClientAuthPackets.ServerList:
                    message = new CA.ServerList();
                    return new Business.Client.ServerList();
                case ClientAuthPackets.SelectServer:
                    message = new CA.SelectServer();
                    return new Business.Client.SelectServer();
                default:
                    ConsoleUtils.ShowFatalError("Invalid PacketId {0}. At {1}", packetId, "ClientCommandHelper.GetCommand()");
                    message = null;
                    return null;
            }
            #endregion
        }
        #endregion
    }
    #endregion
}

