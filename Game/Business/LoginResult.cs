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
using Common.Utils;
using AG = Common.DataClasses.Network.AuthGame;

namespace Game.Business
{
    class LoginResult : ICommand
    {
        public void Execute(Session session, Packet message)
        {
            AG.GameLoginResult packet = (AG.GameLoginResult)message;

            switch (packet.Result)
            {
                case AG.GameLoginResult.ResultCodes.Success:
                    ConsoleUtils.ShowInfo("Successfully connected to Auth-Server.");
                    break;
                case AG.GameLoginResult.ResultCodes.DuplicatedId:
                    ConsoleUtils.ShowError("Could not connect to Auth-Server, Server ID is duplicated.");
                    break;
                default: // Should never happen
                    ConsoleUtils.ShowFatalError(
                        "Unknown Result ID {0}. (At {1})",
                        (ushort) packet.Result,
                        "Game.Business.LoginResult()"
                    );
                    break;
            }
        }
    }
}
