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
using Game.DataClasses.Database;
using Game.DataClasses.Objects;
using Game.DataRepository;
using System.Collections.Generic;
using CG = Game.DataClasses.Network.ClientGame;
using GC = Game.DataClasses.Network.GameClient;

namespace Game.Business.Client
{
    #region Create Character
    public class CreateCharacter : ICommand
    {
        #region Execute Packet
        public void Execute(Session session, Packet message)
        {
            CG.CreateCharacter packet = (CG.CreateCharacter)message;

            // Add Start Items
            List<ItemBase> startItems = new List<ItemBase>();
            int startX, startY;

            // IMPROVE : This probably should be configurable out of source
            switch (packet.Character.ModelInfo.Race)
            {
                case 3: // Gaia
                    // NOTE : Officially this is set by script on first login
                    packet.Character.Job = 100;
                    startX = 164474;
                    startY = 52932;

                    startItems.Add(ItemBase.Get(106100)); // Beginner's Mace
                    if (packet.Character.ModelInfo.WearInfo[2] == 601)
                        startItems.Add(ItemBase.Get(240100));
                    else
                        startItems.Add(ItemBase.Get(240109));
                    break;

                case 4: // Deva
                    // NOTE : Officially this is set by script on first login
                    packet.Character.Job = 200;
                    startX = 164335;
                    startY = 49510;
                    
                    startItems.Add(ItemBase.Get(112100)); // Trainee's Small Axe
                    if (packet.Character.ModelInfo.WearInfo[2] == 601)
                        startItems.Add(ItemBase.Get(220100));
                    else
                        startItems.Add(ItemBase.Get(220109));
                    break;

                case 5: // Asura
                    // NOTE : Officially this is set by script on first login
                    packet.Character.Job = 300;
                    startX = 168356;
                    startY = 55399;

                    startItems.Add(ItemBase.Get(103100)); // Beginner's Dirk
                    if (packet.Character.ModelInfo.WearInfo[2] == 601)
                        startItems.Add(ItemBase.Get(230100));
                    else
                        startItems.Add(ItemBase.Get(230109));
                    break;

                default:
                    // CHECK : Invalid Race, is there an error for that?
                    return;
            }

            // Create Character
            LobbyRepository repo = new LobbyRepository();
            repo.CreateCharacter(((User)session._Client).AccountId, packet.Character, startItems.ToArray(),startX, startY);

            // Prepare result packet
            GC.Result result = new GC.Result();
            result.RequestMessageId = packet.Id;
            result.ResultCode = (ushort)CG.CheckCharacterName.ResultCode.Success;

            // Send Result
            DataClasses.Server.ClientSockets.SendPacket(session, result);
        }
        #endregion
    }
    #endregion
}