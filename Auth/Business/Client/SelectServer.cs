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
using Common.Utils;
using CA = Auth.DataClasses.Network.ClientAuth;
using AG = Common.DataClasses.Network.AuthGame;

namespace Auth.Business.Client
{
    #region SelectServer
    public class SelectServer : ICommand
    {
        #region Execute Packet
        public void Execute(Session session, Packet message)
        {
            CA.SelectServer packet = (CA.SelectServer)message;

            GameServer server;
            if (DataClasses.Server.Instance.ServerList.TryGetValue(packet.ServerId, out server))
            {
                User user = (User)session._Client;
                AG.ClientLogin clientLogin = new AG.ClientLogin();
                clientLogin.UserId = user.UserId;
                clientLogin.AccountId = user.AccountId;
                clientLogin.Permission = user.Permission;

                DataClasses.Server.ServerSockets.SendPacket(server._Session, clientLogin);
            }
            else
            {
                ConsoleUtils.ShowWarning(
                    "User '{0}' is trying to join invalid server id '{1}'",
                    ((User)session._Client).UserId, packet.ServerId
                );
                // TODO : Is there an error code for this?
            }

		}
        #endregion
    }
    #endregion
}

