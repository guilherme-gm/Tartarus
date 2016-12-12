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
using Auth.Business;
using Auth.Helpers;
using Common.DataClasses;
using Common.DataClasses.Network;
using Common.Service;
using System;

namespace Auth.Services
{
	public class ClientController : IController
	{
        public ClientController() { }

        public void ProcessRequest(Session session, byte[] data)
        {
            Packet message;

            ICommand command = ClientCommandHelper.GetCommand(data, out message);
            if (command == null)
            {
                return;
            }

            message.Read(data);
            command.Execute(session, message);
        }
    }

}

