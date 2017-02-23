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
using Common.Utils;
using Game.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Game.DataClasses
{
    public class Server
    {
        private const string RC4Key = "}h79q~B%al;k'y $E";

        public static SocketService ClientSockets { get; private set; }
        public static Common.Service.SocketService AuthSocket { get; private set; }

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

            #region Sets Server Info
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
            #endregion

            #region Load Databases
            Database.ItemBase.Load();
            Database.StatBase.Load(); // Must always load before Job
            Database.JobBase.Load();
            Database.JobLevelBonus.Load();
            Database.LocationBase.Load();
            #endregion

            InitGameWorld();

            #region Start Connection
            AuthSocket =
                new SocketService(
                    new ServerController(),
                    false
                );
            AuthSocket.OnSocketDisconnect += AuthSocket_OnSocketDisconnect;
            AuthSocket.OnConnectionFailed += AuthSocket_OnConnectionFailed;
            AuthSocket.OnConnectionSuccess += AuthSocket_OnConnectionSuccess;
            AuthSocket.StartConnection(Settings.AuthIp, Settings.AuthPort);

            ClientSockets =
                new SocketService(
                    new ClientController(),
                    true,
                    RC4Key
                );
            ClientSockets.StartListening(Settings.ServerPort, Settings.OpenExternal);

            ConsoleUtils.ShowStatus("Game Server initialized.");
            #endregion

            string input;
            do
            {
                input = Console.ReadLine();
            } while (input != "quit");
        }

        private void InitGameWorld()
        {
            Database.MapBase mapBase = new Database.MapBase();
            if (!mapBase.Load())
                return;

            GameWorld.Region.Init();
            GameWorld.WorldLocation.Init(mapBase);
        }

        #region Inter-Server Connection Events
        private async void AuthSocket_OnConnectionFailed()
        {
            ConsoleUtils.ShowError("Could not connect to Auth-Server, trying again in 2 seconds");
            await Task.Delay(2000);
            AuthSocket =
                new SocketService(
                    new ServerController(),
                    false
                );
            AuthSocket.OnSocketDisconnect += AuthSocket_OnSocketDisconnect;
            AuthSocket.OnConnectionFailed += AuthSocket_OnConnectionFailed;
            AuthSocket.OnConnectionSuccess += AuthSocket_OnConnectionSuccess;
            AuthSocket.StartConnection(Settings.AuthIp, Settings.AuthPort);
        }

        private async void AuthSocket_OnSocketDisconnect(Session session)
        {
            // TODO : Reconnection must send a list of connected users to Auth
            this.Reconnecting = true;

            ConsoleUtils.ShowError("Connection to Auth-Server lost, trying to reconnect in 2 seconds");
            await Task.Delay(2000);
            AuthSocket =
                new SocketService(
                    new ServerController(),
                    false
                );
            AuthSocket.OnSocketDisconnect += AuthSocket_OnSocketDisconnect;
            AuthSocket.OnConnectionFailed += AuthSocket_OnConnectionFailed;
            AuthSocket.OnConnectionSuccess += AuthSocket_OnConnectionSuccess;
            AuthSocket.StartConnection(Settings.AuthIp, Settings.AuthPort);
        }

        private void AuthSocket_OnConnectionSuccess(Session session)
        {
            Server.AuthSocket.SendPacket(
                session,
                new Common.DataClasses.Network.GameAuth.Login() { ServerInfo = Server.ServerInfo, Password = Settings.AuthPassword }
            );
        }
        #endregion

        #region Pending User
        /// <summary>
        /// Adds a PendingUserInfo to a given account name
        /// </summary>
        /// <param name="userId">account name</param>
        /// <param name="info">PendingUserInfo</param>
        internal void AddPendingUser(string userId, PendingUserInfo info)
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

        /// <summary>
        /// Retrieves PendingUserInfo for a given account name
        /// </summary>
        /// <param name="account">account name</param>
        /// <returns>PendingUserInfo for that account or null if not found</returns>
        internal PendingUserInfo RetrievePendingUser(string account)
        {
            PendingUserInfo info;
            if (!this.PendingUsers.TryGetValue(account, out info))
            {
                return null;
            }

            return info;
        }

        /// <summary>
        /// Removes PendingUserInfo of a given account name
        /// </summary>
        /// <param name="account">Accout name</param>
        internal void RemovePendingUser(string account)
        {
            this.PendingUsers.Remove(account);
        }
        #endregion
    }
}

