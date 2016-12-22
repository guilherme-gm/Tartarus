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
using Auth.Services;
using Common;
using Common.DataClasses;
using Common.Service;
using Common.Utils;
using System;
using System.Collections.Generic;

namespace Auth.DataClasses
{
	public class Server
	{
        public static SocketService ClientSockets { get; private set; }
        public static SocketService ServerSockets { get; private set; }

        private static Server _Instance;

        public static Server Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new Server();
                }
                return _Instance;
            }
            private set
            {
                _Instance = value;
            }
        }

		private List<Session> ConnectedUsers;
        private List<Session> ConnectedServers;

        public List<ServerInfo> ServerList { get; set; }

        private Server()
        {
            Instance = this;

            this.ConnectedUsers = new List<Session>();
            this.ConnectedServers = new List<Session>();
            this.ServerList = new List<ServerInfo>();
        }

		public void Start()
		{
            ClientSockets =
                new SocketService(
                    Settings.ServerIp,
                    Settings.ServerPort,
                    true,
                    new UserFactory(),
                    new ClientController()
                );
            ClientSockets.OnSocketDisconnect += ClientSockets_OnSocketDisconnect;
            ClientSockets.StartListening();

            ServerSockets =
                new SocketService(
                    Settings.ServerIp,
                    Settings.GameServerPort,
                    false,
                    new GameServerFactory(),
                    new ServerController()
                );
            ServerSockets.OnSocketDisconnect += ServerSockets_OnSocketDisconnect;
            ServerSockets.StartListening();
            
            ConsoleUtils.ShowStatus("Auth Server initialized.");

            string input;
            do
            {
                input = Console.ReadLine();
            } while (input != "quit");
		}

        public void AddUser(Session session)
        {
            this.ConnectedUsers.Add(session);
        }

        private void ClientSockets_OnSocketDisconnect(Session session)
        {
            this.ConnectedUsers.Remove(session);
            ConsoleUtils.ShowInfo("User {0} disconnected.", ((User)session._Client).UserId);
        }

        internal bool AddServer(GameServer gameServer)
        {
            if (!this.ServerList.Exists(server => server.Id == gameServer.ServerInfo.Id))
            {
                this.ServerList.Add(gameServer.ServerInfo);
                this.ConnectedServers.Add(gameServer._Session);
                ConsoleUtils.ShowInfo(
                    "Server {0} (Id: {1}) added to Server List.",
                    gameServer.ServerInfo.Name, gameServer.ServerInfo.Id
                );

                return true;
            }
            else
            {
                ConsoleUtils.ShowInfo(
                    "Server {0} (Id: {1}) connection refused, this ID is already in use.",
                    gameServer.ServerInfo.Name, gameServer.ServerInfo.Id
                );

                return false;
            }
        }

        private void ServerSockets_OnSocketDisconnect(Session session)
        {
            this.ServerList.Remove(((GameServer)session._Client).ServerInfo);
            this.ConnectedServers.Remove(session);
            ConsoleUtils.ShowInfo("GameServer {0} disconnected.", ((GameServer)session._Client).ServerInfo.Name);
        }

    }

}

