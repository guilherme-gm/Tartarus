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
using Game.DataClasses.Objects;
using CG = Game.DataClasses.Network.ClientGame;
using GC = Game.DataClasses.Network.GameClient;

namespace Game.Business.Client
{
    #region Security No
    public class ReturnLobby : ICommand
    {
        #region Execute Packet
        public void Execute(Session session, Packet message)
        {
            CG.ReturnLobby packet = (CG.ReturnLobby)message;
            GC.Result result = new GC.Result();
            result.RequestMessageId = packet.Id;

            if (session._Client == null)
                return;
            Player player = ((User)session._Client).Character;
            if (player == null)
                return;

            // TODO : Ensure it's allowed to disconnect

            player.Disconnect();

            result.ResultCode = (ushort)CG.ReturnLobby.ResultCodes.Success;

            DataClasses.Server.ClientSockets.SendSelf(session, result);
        }
        #endregion
    }
    #endregion
}