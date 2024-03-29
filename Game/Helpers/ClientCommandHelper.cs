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
                case ClientGamePackets.Login:
                    message = new CG.Login();
                    return new Business.Client.Login();
                case ClientGamePackets.MoveRequest:
                    message = new CG.MoveRequest();
                    return new Business.Client.MoveRequest();
                case ClientGamePackets.RegionUpdate:
                    message = new CG.RegionUpdate();
                    return new Business.Client.RegionUpdate();
                case ClientGamePackets.ChatRequest:
                    message = new CG.ChatRequest();
                    return new Business.Client.ChatRequest();
                case ClientGamePackets.ReturnLobby:
                    message = new CG.ReturnLobby();
                    return new Business.Client.ReturnLobby();
                case ClientGamePackets.RequestReturnLobby:
                    message = new CG.RequestReturnLobby();
                    return new Business.Client.RequestReturnLobby();
                case ClientGamePackets.Version:
                    message = new CG.Version();
                    return new Business.Client.Version();
                case ClientGamePackets.ChangeLocation:
                    message = new CG.ChangeLocation();
                    return new Business.Client.ChangeLocation();
                case ClientGamePackets.CharacterList:
                    message = new CG.CharacterList();
                    return new Business.Client.CharacterList();
                case ClientGamePackets.CreateCharacter:
                    message = new CG.CreateCharacter();
                    return new Business.Client.CreateCharacter();
                case ClientGamePackets.DeleteCharacter:
                    message = new CG.DeleteCharacter();
                    return new Business.Client.DeleteCharacter();
                case ClientGamePackets.AccountWithAuth:
                    message = new CG.AccountWithAuth();
                    return new Business.Client.AccountWithAuth();
                case ClientGamePackets.CheckCharacterName:
                    message = new CG.CheckCharacterName();
                    return new Business.Client.CheckCharacterName();
                case ClientGamePackets.SystemSpecs:
                    message = null;
                    return null;
                case ClientGamePackets.SecurityNo:
                    message = new CG.SecurityNo();
                    return new Business.Client.SecurityNo();
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

