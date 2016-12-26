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

namespace Auth.Business.Client
{
	public class Login : ICommand
    {
        public const string DesKey = "MERONG";
        public static XDes DesCipher = new XDes(DesKey);

        public void Execute(Session session, Packet message)
        {
            CA.Account packet = (CA.Account)message;
            AC.Result result = new AC.Result();
            result.RequestMessageId = (ushort)ClientAuthPackets.Account;

            string password = DesCipher.Decrypt(packet.Password).TrimEnd('\0');

            if (Settings.LoginDebug)
                ConsoleUtils.ShowInfo("User '{0}' is trying to connect.", packet.Username);

            UserRepository repo = new UserRepository();
            User user = repo.GetUser(packet.Username, password);
            
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
                    ConsoleUtils.ShowInfo("User '{0}' login failed (Invalid credentials)", packet.Username);
            }

            DataClasses.Server.ClientSockets.SendPacket(session, result);
        }

	}

}

