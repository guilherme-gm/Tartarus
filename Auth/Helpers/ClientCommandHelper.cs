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
using Auth.DataClasses.Network;
using Common.DataClasses.Network;
using Common.Utils;
using System;

using CA = Auth.DataClasses.Network.ClientAuth;

namespace Auth.Helpers
{
    public static class ClientCommandHelper
    {
        public static ICommand GetCommand(byte[] packet, out Packet message)
        {
            ClientAuthPackets packetId = (ClientAuthPackets)BitConverter.ToUInt16(packet, 4);

            switch (packetId)
            {
                case ClientAuthPackets.Unknown:
                    message = null;
                    return null;
                case ClientAuthPackets.Version:
                    message = new CA.Version();
                    return new Business.Version();
                case ClientAuthPackets.Account:
                    message = new CA.Account();
                    return new Business.ClientLogin();
                case ClientAuthPackets.ImbcAccount:
                    message = new CA.ImbcAccount();
                    return new Business.ImbcLogin();
                case ClientAuthPackets.ServerList:
                    message = new CA.ServerList();
                    return new Business.ServerList();
                case ClientAuthPackets.SelectServer:
                    message = new CA.SelectServer();
                    return new Business.SelectServer();
                default:
                    ConsoleUtils.ShowFatalError("Invalid PacketId {0}. At {1}", packetId, "ClientCommandHelper.GetCommand()");
                    message = null;
                    return null;
            }
        }

    }

}

