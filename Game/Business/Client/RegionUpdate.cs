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
using Game.DataClasses.GameWorld;
using Game.DataClasses.Objects;
using CG = Game.DataClasses.Network.ClientGame;
using GC = Game.DataClasses.Network.GameClient;

namespace Game.Business.Client
{
    #region Region Update
    public class RegionUpdate : ICommand
    {
        #region Execute Packet
        public void Execute(Session session, Packet message)
        {
            CG.RegionUpdate packet = (CG.RegionUpdate)message;

            if (session._Client == null)
                return;
            Player player = ((User)session._Client).Character;
            if (player == null)
                return;

            Position pos = new Position()
            {
                X = packet.X,
                Y = packet.Y,
                Z = packet.Z
            };
            Region region = Region.FromPosition(pos);
            // TODO : Proper region updates
            //region.Enter(player);
            //region.ReceiveObjects(player, false);
            player.Region = region;

            if (packet.IsStopMessage)
            {
                player.PendingMovePositions.Clear();
                player.Position = pos;
            }

            GC.RegionAck regionAck = new GC.RegionAck();
            regionAck.RegionX = region.X;
            regionAck.RegionY = region.Y;

            DataClasses.Server.ClientSockets.SendSelf(session, regionAck);
        }
        #endregion
    }
    #endregion
}