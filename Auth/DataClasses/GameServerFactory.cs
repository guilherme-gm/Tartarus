﻿/**
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
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Common.DataClasses;

namespace Auth.DataClasses
{
    #region GameServerFactory
    public class GameServerFactory : SessionFactory
    {
        #region CreateSession
        public override Session CreateSession(Socket socket)
        {
            Session session = new Session();
            session._Client = new GameServer();
            session._NetworkData = new NetworkData(socket);
            session._Client._Session = session;

            return session;
        }
        #endregion
    }
    #endregion
}
