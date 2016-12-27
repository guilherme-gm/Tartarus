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
using Common.DataClasses.Network;
using GA = Common.DataClasses.Network.GameAuth;
using System;
using Common.Utils;

namespace Auth.Helpers
{
    #region ServerCommandHelper
    public class ServerCommandHelper
	{
        #region GetCommand
        public static ICommand GetCommand(byte[] packet, out Packet message)
        {
            #region Packet Switch Case
            GameAuthPackets packetId = (GameAuthPackets)BitConverter.ToUInt16(packet, 4);

            switch (packetId)
            {
                case GameAuthPackets.Login:
                    message = new GA.Login();
                    return new Business.Server.RegisterServer();
                case GameAuthPackets.ClientLogin:
                    message = new GA.ClientLogin();
                    return new Business.Server.ClientLogin();
                default:
                    ConsoleUtils.ShowFatalError("Invalid PacketId {0}. At {1}", packetId, "ServerCommandHelper.GetCommand()");
                    message = null;
                    return null;
            }
            #endregion
        }
        #endregion
    }
#endregion
}

