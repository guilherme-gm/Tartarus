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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.DataClasses;
using Common.DataClasses.Network;
using AG = Common.DataClasses.Network.AuthGame;
using GA = Common.DataClasses.Network.GameAuth;
using System.Security.Cryptography;
using Game.DataClasses;

namespace Game.Business.Server
{
    public class ClientLogin : ICommand
    {
        public void Execute(Session session, Packet message)
        {
            AG.ClientLogin packet = (AG.ClientLogin)message;

            // Generates a join key
            long key;
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                byte[] data = new byte[8];
                rng.GetBytes(data);
                key = BitConverter.ToInt64(data, 0);
            }

            PendingUserInfo info = new PendingUserInfo();
            info.AccountId = packet.AccountId;
            info.Permission = packet.Permission;
            info.OneTimeKey = key;

            DataClasses.Server.Instance.AddPendingUser(packet.UserId, info);

            GA.ClientLogin clientLogin = new GA.ClientLogin();
            clientLogin.AccountId = info.AccountId;
            clientLogin.OneTimeKey = key;

            DataClasses.Server.AuthSocket.SendPacket(session, clientLogin);
        }
    }
}
