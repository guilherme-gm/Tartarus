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
using GA = Common.DataClasses.Network.GameAuth;
using AG = Common.DataClasses.Network.AuthGame;

namespace Auth.Business.Server
{
	public class RegisterServer : ICommand
    {
        public void Execute(Session session, Packet message)
        {
            GA.Login packet = (GA.Login)message;
            AG.GameLoginResult result = new AG.GameLoginResult();

            GameServer server = new GameServer();
            session._Client = server;
            server._Session = session;
            server.ServerInfo = packet.ServerInfo;

            if (DataClasses.Server.Instance.AddServer(server))
            {
                result.Result = AG.GameLoginResult.ResultCodes.Success;
            }
            else
            {
                result.Result = AG.GameLoginResult.ResultCodes.DuplicatedId;
            }

            DataClasses.Server.ServerSockets.SendPacket(session, result);
        }

	}

}

