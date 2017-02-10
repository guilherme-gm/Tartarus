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
    #region Move Request
    public class MoveRequest : ICommand
    {
        #region Execute Packet
        public void Execute(Session session, Packet message)
        {
            CG.MoveRequest packet = (CG.MoveRequest)message;

            if (session._Client == null)
                return;

            Player player = ((User)session._Client).Character;
            if (player == null)
                return;

            // TODO : This is probably used by summons and pets, we need to
            //         ensure that player is allowed to control given GID
            Region region = player.Region;
            if (region == null)
                return;

            GameObject go = GameObject.GIDToObject(packet.GID);
            if (go.Type != ObjectType.Static) //&& go.Type != ObjectType.Movable)
                return;

            if (go.Position == null)
                return;
            if (go.Position.X != packet.X || go.Position.Y != packet.Y)
            {
                // TODO : client position is not in sync with server
            }
            
            region.MoveRequest((Creature)go, packet.Points);
        }
        #endregion
    }
    #endregion
}