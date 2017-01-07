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
            List<Item> startItems = new List<Item>();
            int startX, startY;

            // TODO : This probably should be configurable out of source
            switch (packet.Character.ModelInfo.Race)
            {
                case 3: // Gaia
                    // TODO: Officially this is set by script on first login
                    packet.Character.Job = 100;
                    startX = 164474;
                    startY = 52932;

                    startItems.Add(new Item() { Code = 106100, Amount = 1, Position = 0 }); // Beginner's Mace
                    if (packet.Character.ModelInfo.WearInfo[2] == 601)
                        startItems.Add(new Item() { Code = 240100, Amount = 1, Position = 2 });
                    else
                        startItems.Add(new Item() { Code = 240109, Amount = 1, Position = 2 });
                    break;

                case 4: // Deva
                    // TODO: Officially this is set by script on first login
                    packet.Character.Job = 200;
                    startX = 164335;
                    startY = 49510;
                    
                    startItems.Add(new Item() { Code = 112100, Amount = 1, Position = 0 }); // Trainee's Small Axe
                    if (packet.Character.ModelInfo.WearInfo[2] == 601)
                        startItems.Add(new Item() { Code = 220100, Amount = 1, Position = 2 });
                    else
                        startItems.Add(new Item() { Code = 220109, Amount = 1, Position = 2 });
                    break;

                case 5: // Asura
                    // TODO: Officially this is set by script on first login
                    packet.Character.Job = 300;
                    startX = 168356;
                    startY = 55399;

                    startItems.Add(new Item() { Code = 103100, Amount = 1, Position = 0 }); // Beginner's Dirk
                    if (packet.Character.ModelInfo.WearInfo[2] == 601)
                        startItems.Add(new Item() { Code = 230100, Amount = 1, Position = 2 });
                    else
                        startItems.Add(new Item() { Code = 230109, Amount = 1, Position = 2 });
                    break;

                default:
                    // TODO : Invalid Race, is there an error for that?
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