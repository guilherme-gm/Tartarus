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
using Auth.DataClasses;
using Common.DataClasses;
using Common.DataClasses.Network;
using AC = Auth.DataClasses.Network.AuthClient;

namespace Auth.Business.Client
{
    #region ServerList
    public class ServerList : ICommand
    {
        #region Execute packet
        public void Execute(Session session, Packet message)
        {
            AC.ServerList serverList = new AC.ServerList();

            serverList.LastLoginServerId = ((User)session._Client).LastServerId;
            serverList.ServerInfo = new ServerInfo[DataClasses.Server.Instance.ServerList.Count];

            int i = 0;
            foreach(GameServer server in DataClasses.Server.Instance.ServerList.Values)
            {
                serverList.ServerInfo[i] = server.ServerInfo;
                i++;
            }

            DataClasses.Server.ClientSockets.SendPacket(session, serverList);
        }
        #endregion
    }
    #endregion
}

