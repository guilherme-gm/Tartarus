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
using GA = Common.DataClasses.Network.GameAuth;

namespace Auth.Business.Server
{
	public class ClientLogin : ICommand
    {
        public void Execute(Session session, Packet message)
        {
            GA.ClientLogin packet = (GA.ClientLogin)message;

            User user;
            if (!DataClasses.Server.Instance.ConnectedUsers.TryGetValue(packet.AccountId, out user))
            {
                return;
            }

            AC.SelectServer selectServer = new AC.SelectServer();
            selectServer.Result = 0;
            selectServer.OneTimeKey = packet.OneTimeKey;
            selectServer.PendingTime = 10;

            user.Server = (GameServer)session._Client;
            user.LastServerId = user.Server.ServerInfo.Id;

            // TODO : Persist LastServerId

            DataClasses.Server.ClientSockets.SendPacket(user._Session, selectServer);
        }

	}

}
