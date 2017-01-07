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
using Common.DataClasses;
using Common.DataClasses.Network;
using Game.DataClasses;
using Game.DataRepository;
using System.Text.RegularExpressions;
using CG = Game.DataClasses.Network.ClientGame;
using GC = Game.DataClasses.Network.GameClient;

namespace Game.Business.Client
{
    #region Delete Character
    public class DeleteCharacter : ICommand
    {
        #region Execute Packet
        public void Execute(Session session, Packet message)
        {
            CG.DeleteCharacter packet = (CG.DeleteCharacter)message;
            GC.Result result = new GC.Result();

            User user = (User)session._Client;
            if (user == null)
                return;

            LobbyRepository repo = new LobbyRepository();

            result.RequestMessageId = packet.Id;
            if (repo.DeleteCharacter(user.AccountId, packet.CharacterName))
                result.ResultCode = (ushort)CG.DeleteCharacter.ResultCodes.Success;
            
            // TODO : Is there an else for this?

            DataClasses.Server.ClientSockets.SendPacket(session, result);
        }
        #endregion
    }
    #endregion
}