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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.DataClasses;
using Common.Service;
using Common.DataClasses.Network;
using Game.DataClasses.GameWorld;
using Game.DataClasses.Objects;
using Common.DataClasses.Collections;

namespace Game.Services
{
    /// <summary>
    /// Extends Common SocketService by implementing send targets
    /// </summary>
    public class SocketService : Common.Service.SocketService
    {
        public SocketService(IController controller, bool isEncrypted, string cipherKey = "", ISessionFactory factory = null)
            : base(controller, isEncrypted, cipherKey, factory)
        { }

        public void SendSelf(Session session, Packet packet)
        {
            this.SendPacket(session, packet);
        }

        public void SendArea(Region tRegion, Packet packet)
        {
            List<Region> regions = Region.GetNearbyRegions(tRegion);
            foreach (Region region in regions)
            {
                lock (region)
                {
                    foreach (Player player in region.Players.AsReadOnly())
                    {
                        this.SendPacket(player.User._Session, packet);
                    }
                }
            }
        }

        public void SendAreaWithoutSelf(Session self, Region tRegion, Packet packet)
        {
            List<Region> regions = Region.GetNearbyRegions(tRegion);

            foreach (Region region in regions)
            {
                lock (region)
                {
                    foreach (Player player in region.Players.AsReadOnly())
                    {
                        Session session = player.User._Session;
                        if (self != session)
                            this.SendPacket(session, packet);
                    }
                }
            }
        }

        public void SendRegion(Region tRegion, Packet packet)
        {
            lock (tRegion)
            {
                foreach (Player player in tRegion.Players.AsReadOnly())
                {
                    this.SendPacket(player.User._Session, packet);
                }
            }
        }

        public void SendRegionWithoutSelf(Session self, Region tRegion, Packet packet)
        {
            lock (tRegion)
            {
                foreach (Player player in tRegion.Players.AsReadOnly())
                {
                    Session session = player.User._Session;
                    if (self != session)
                        this.SendPacket(session, packet);
                }
            }
        }

        public void SendAll(Packet packet)
        {
            foreach(Player player in Player.Players.AsReadOnly())
            {
                this.SendPacket(player.User._Session, packet);
            }
        }

        public void SendAllWithoutSelf(Session self, Packet packet)
        {
            foreach (Player player in Player.Players.AsReadOnly())
            {
                Session session = player.User._Session;
                if (session != self)
                    this.SendPacket(session, packet);
            }
        }

        //public void SendParty();
        //public void SendPartyWithoutSelf,
        //public void SendGuild,
        //public void SendGuildWithoutSelf,
        //public void SendTeam,
        //public void TeamWithoutSelf
    }
}
