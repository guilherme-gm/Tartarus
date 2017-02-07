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
using Game.DataClasses.Lobby;
using Game.DataRepository;
using System;
using CG = Game.DataClasses.Network.ClientGame;
using GC = Game.DataClasses.Network.GameClient;

namespace Game.Business.Client
{
    #region Character List Packet
    public class CharacterList : ICommand
    {
        #region Packet Execute
        public void Execute(Session session, Packet message)
        {
            CG.CharacterList packet = (CG.CharacterList)message;
            GC.CharacterList characterList = new GC.CharacterList();

            // Invalid request (client not logged)
            if (session._Client == null)
                return;

            LobbyRepository repo = new LobbyRepository();
            LobbyCharacterInfo[] characters = repo.GetCharacterList(((User)session._Client).AccountId);

            // Find last logged character
            ushort lastSlot = 0;
            if (characters.Length > 0)
            {
                DateTime newestLogin = characters[0].LoginTime;
                lastSlot = 0;
                for (ushort i = 1; i < characters.Length; ++i)
                {
                    if (newestLogin < characters[i].LoginTime)
                    {
                        lastSlot = i;
                        newestLogin = characters[i].LoginTime;
                    }
                }
            }

            // Finish packet info
            characterList.Count = (ushort)characters.Length;
            characterList.Characters = characters;
            characterList.CurrentServerTime = 0; // Seems like this value is always 0 on 6.2
            characterList.LastLoginIndex = lastSlot;

            // Send Packet
            DataClasses.Server.ClientSockets.SendSelf(session, characterList);
        }
        #endregion
    }
    #endregion
}
