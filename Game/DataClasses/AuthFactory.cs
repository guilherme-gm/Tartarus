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
using System.Net.Sockets;
using Common.DataClasses.Network.GameAuth;

namespace Game.DataClasses
{
    public class AuthFactory : SessionFactory
    {
        public override Session CreateSession(Socket socket)
        {
            Session session = new Session();
            session._Client = new Auth();
            session._NetworkData = new NetworkData(socket);;

            Server.AuthSocket.SendPacket(session, new Login() { ServerInfo = Server.ServerInfo });

            return session;
        }
    }
}
