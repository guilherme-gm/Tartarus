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
    #region Change Location Packet
    public class ChangeLocation : ICommand
    {
        #region Packet Execute
        public void Execute(Session session, Packet message)
        {
            CG.ChangeLocation packet = (CG.ChangeLocation)message;
            GC.ChangeLocation locationInfo = new GC.ChangeLocation();
            
            // Invalid request (client not logged)
            if (session._Client == null)
                return;

            Player player = ((User)session._Client).Character;
            if (player.Location != null)
                locationInfo.PrevLocationId = player.Location.Location.Id;
            else
                locationInfo.PrevLocationId = 0;

            // Get new location
            WorldLocation newLocal = WorldLocation.FromPosition(packet.X, packet.Y);
            player.Location = newLocal;
            locationInfo.CurLocationId = newLocal.Location.Id;

            // Send Packet
            DataClasses.Server.ClientSockets.SendSelf(session, locationInfo);
        }
        #endregion
    }
    #endregion
}
