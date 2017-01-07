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
using Game.DataClasses.Network;
using System;

using CG = Game.DataClasses.Network.ClientGame;

namespace Game.Helpers
{
	public static class ClientCommandHelper
	{
		public static ICommand GetCommand(byte[] packet, out Packet message)
		{
            ClientGamePackets packetId = (ClientGamePackets)BitConverter.ToUInt16(packet, 4);

            switch (packetId)
            {
                case ClientGamePackets.Version:
                    message = new CG.Version();
                    return new Business.Client.Version();
                case ClientGamePackets.CharacterList:
                    message = new CG.CharacterList();
                    return new Business.Client.CharacterList();
                case ClientGamePackets.CreateCharacter:
                    message = new CG.CreateCharacter();
                    return new Business.Client.CreateCharacter();
                case ClientGamePackets.AccountWithAuth:
                    message = new CG.AccountWithAuth();
                    return new Business.Client.AccountWithAuth();
                case ClientGamePackets.CheckCharacterName:
                    message = new CG.CheckCharacterName();
                    return new Business.Client.CheckCharacterName();
                case ClientGamePackets.SystemSpecs:
                    message = null;
                    return null;
                case ClientGamePackets.Unknown:
                    message = null;
                    return null;
                default:
                    ConsoleUtils.ShowFatalError("Invalid PacketId {0}. At {1}", packetId, "ClientCommandHelper.GetCommand()");
                    message = null;
                    return null;
            }
		}

	}

}

