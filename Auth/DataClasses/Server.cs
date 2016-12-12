
using Auth.Services;
using Common;
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
using System;
using System.Collections.Generic;

namespace Auth.DataClasses
{
	public class Server
	{
		private List<Session> ConnectedUsers;

		private List<ServerInfo> ServerList;

		public void Start()
		{
            SocketService clientService =
                new SocketService("127.0.0.1", 8841, true, new UserFactory(), new ClientController());
            clientService.Start();
            
            ConsoleUtils.ShowInfo("Auth Server initialized.");

            string input;
            do
            {
                input = Console.ReadLine();
            } while (input != "quit");
		}

	}

}

