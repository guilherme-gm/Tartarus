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
    #region Security No
    public class SecurityNo : ICommand
    {
        #region Execute Packet
        public void Execute(Session session, Packet message)
        {
            CG.SecurityNo packet = (CG.SecurityNo)message;
            GC.Result result = new GC.Result();
            result.RequestMessageId = packet.Id;

            User user = (User)session._Client;
            if (user == null)
                return;

            switch (packet.Mode)
            {
                case CG.SecurityNo.SecurityMode.None:
                    break;
                case CG.SecurityNo.SecurityMode.OpenStorage:
                    break;
                case CG.SecurityNo.SecurityMode.DeleteCharacterRepeat:
                case CG.SecurityNo.SecurityMode.DeleteCharacter:
                    {
                        if (user.DeleteCharacter.Equals(""))
                            return;

                        SecurityRepository secRepo = new SecurityRepository();

                        if (packet.SecurityCode.Equals(secRepo.GetCode(user.AccountId)))
                        {
                            LobbyRepository lobbyRepo = new LobbyRepository();
                            if (lobbyRepo.DeleteCharacter(user.AccountId, user.DeleteCharacter))
                            {
                                user.DeleteCharacter = "";
                                result.ResultCode = (ushort)CG.SecurityNo.ResultCodes.Success;
                            }
                            else
                            {
                                // CHECK : Is there an else for this?
                            }
                        }
                        else
                        {
                            result.ResultCode = (ushort)CG.SecurityNo.ResultCodes.PasswordMismatch;
                        }

                        DataClasses.Server.ClientSockets.SendSelf(session, result);
                    }
                    break;
                default:
                    break;
            }
        }
        #endregion
    }
    #endregion
}