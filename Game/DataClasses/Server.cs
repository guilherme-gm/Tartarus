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

        private Server()
        {
            Instance = this;
        }

        public void Start()
        {
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
            AuthSocket.StartConnection();

            /*
            ClientSockets =
                new SocketService(
                    Settings.ServerIp,
                    Settings.ServerPort,
                    true,
                    new UserFactory(),
                    new ClientController()
                );
            ClientSockets.StartListening();
            */
            ConsoleUtils.ShowStatus("Game Server initialized.");

            string input;
            do
            {
                input = Console.ReadLine();
            } while (input != "quit");
        }

    }

}
