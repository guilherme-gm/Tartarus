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
using Common.Service;
using Common.Utils;
using Game.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Game.DataClasses
{
    public class Server
    {
        public static SocketService ClientSockets { get; private set; }
        public static SocketService AuthSocket { get; private set; }

        public static ServerInfo ServerInfo { get; private set; }

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

        private bool Reconnecting;

        private Dictionary<string, PendingUserInfo> PendingUsers;

        private Server()
        {
            Instance = this;
            this.PendingUsers = new Dictionary<string, PendingUserInfo>();
        }

        public void Start()
        {
            this.Reconnecting = false;

            ServerInfo = new ServerInfo()
            {
                AdultServer = false,
                Id = Settings.ServerIndex,
                Ip = Settings.ServerIp,
                Name = Settings.ServerName,
                Port = Settings.ServerPort,
                ScreenshotUrl = Settings.NoticeUrl,
                UserRatio = 0
            };

            AuthSocket =
                new SocketService(
                    Settings.ServerIp,
                    Settings.AuthPort,
                    false,
                    new AuthFactory(),
                    new ServerController()
                );
            AuthSocket.OnSocketDisconnect += AuthSocket_OnSocketDisconnect;
            AuthSocket.OnConnectionFailed += AuthSocket_OnConnectionFailed;
            AuthSocket.StartConnection();

            ClientSockets =
                new SocketService(
                    Settings.ServerIp,
                    Settings.ServerPort,
                    true,
                    new UserFactory(),
                    new ClientController()
                );
            ClientSockets.StartListening();

            ConsoleUtils.ShowStatus("Game Server initialized.");

            string input;
            do
            {
                input = Console.ReadLine();
            } while (input != "quit");
        }

        private async void AuthSocket_OnConnectionFailed()
        {
            ConsoleUtils.ShowError("Could not connect to Auth-Server, trying again in 2 seconds");
            await Task.Delay(2000);
            AuthSocket =
                new SocketService(
                    Settings.ServerIp,
                    Settings.AuthPort,
                    false,
                    new AuthFactory(),
                    new ServerController()
                );
            AuthSocket.OnSocketDisconnect += AuthSocket_OnSocketDisconnect;
            AuthSocket.OnConnectionFailed += AuthSocket_OnConnectionFailed;
            AuthSocket.StartConnection();
        }

        private async void AuthSocket_OnSocketDisconnect(Session session)
        {
            // TODO : Reconnection must send a list of connected users to Auth
            this.Reconnecting = true;

            ConsoleUtils.ShowError("Connection to Auth-Server lost, trying to reconnect in 2 seconds");
            await Task.Delay(2000);
            AuthSocket =
                new SocketService(
                    Settings.ServerIp,
                    Settings.AuthPort,
                    false,
                    new AuthFactory(),
                    new ServerController()
                );
            AuthSocket.OnSocketDisconnect += AuthSocket_OnSocketDisconnect;
            AuthSocket.OnConnectionFailed += AuthSocket_OnConnectionFailed;
            AuthSocket.StartConnection();
        }

        public void AddPendingUser(string userId, PendingUserInfo info)
        {
            if (this.PendingUsers.ContainsKey(userId))
            {
                this.PendingUsers[userId] = info;
            }
            else
            {
                this.PendingUsers.Add(userId, info);
            }
        }
    }
}

