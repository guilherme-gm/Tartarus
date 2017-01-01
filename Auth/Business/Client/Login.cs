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
using Auth.DataRepository;
using Common.DataClasses;
using Common.DataClasses.Network;
using Common.Utils;
using CA = Auth.DataClasses.Network.ClientAuth;
using AC = Auth.DataClasses.Network.AuthClient;
using Auth.DataClasses.Network;
using System;

namespace Auth.Business.Client
{
    #region Login
    public class Login : ICommand
    {
        #region Authentication Method
        public const string DesKey = "MERONG";
        public static XDes DesCipher = new XDes(DesKey);
        #endregion

        #region Execute Packet
        public void Execute(Session session, Packet message)
        {
            AC.Result result = new AC.Result();
            result.RequestMessageId = (ushort)ClientAuthPackets.Account;

            AuthSession authSession = (AuthSession)session;
            string password;
            string username;

            if (authSession.UsesAes)
            {
                CA.AccountAes packet = (CA.AccountAes)message;
                username = packet.Username;

                byte[] key = new byte[16];
                byte[] iv = new byte[16];

                Buffer.BlockCopy(authSession.AesInfo, 0, key, 0, 16);
                Buffer.BlockCopy(authSession.AesInfo, 16, iv, 0, 16);

                password = AesUtils.Decrypt(packet.Password, key, iv);
            }
            else
            {
                CA.AccountDes packet = (CA.AccountDes)message;
                username = packet.Username;
                password = DesCipher.Decrypt(packet.Password).TrimEnd('\0');
            }

            if (Settings.LoginDebug)
                ConsoleUtils.ShowInfo("User '{0}' is trying to connect.", username);

            UserRepository repo = new UserRepository();
            User user = repo.GetUser(username, password);
            
            if (user != null)
            {
                session._Client = user;
                session._Client._Session = session;

                DataClasses.Server.Instance.AddUser(session);

                result.ResultCode = 0;
                if (Settings.LoginDebug)
                    ConsoleUtils.ShowInfo("User '{0}' is now connected (Permission: {1})", user.UserId, user.Permission);
            }
            else
            {
                result.ResultCode = 1;
                if (Settings.LoginDebug)
                    ConsoleUtils.ShowInfo("User '{0}' login failed (Invalid credentials)", username);
            }

            DataClasses.Server.ClientSockets.SendPacket(session, result);
        }
        #endregion

    }
    #endregion
}

