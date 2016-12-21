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
using Common.DataClasses.Network;
using Common.Utils;
using Game.Business;
using AG = Common.DataClasses.Network.AuthGame;
using System;

namespace Game.Helpers
{
	public class ServerCommandHelper
	{
        public static ICommand GetCommand(byte[] packet, out Packet message)
        {
            AuthGamePackets packetId = (AuthGamePackets)BitConverter.ToUInt16(packet, 4);

            switch (packetId)
            {
                case AuthGamePackets.GameLoginResult:
                    message = new AG.GameLoginResult();
                    return new Business.LoginResult();
                default:
                    ConsoleUtils.ShowFatalError("Invalid PacketId {0}. At {1}", packetId, "ServerCommandHelper.GetCommand()");
                    message = null;
                    return null;
            }
        }

    }

}

