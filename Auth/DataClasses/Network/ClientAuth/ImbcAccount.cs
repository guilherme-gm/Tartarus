
using System;
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
using Common.DataClasses.Network;
using System.IO;

namespace Auth.DataClasses.Network.ClientAuth
{
	public class ImbcAccount : Packet
	{
        public string Username { get; set; }

        public string Password { get; set; }

        public override void Read(byte[] data)
        {
            BinaryReader br = new BinaryReader(new MemoryStream(data));
            base.Read(br);

            this.Username = BitConverter.ToString(br.ReadBytes(61));
            this.Password = BitConverter.ToString(br.ReadBytes(48));
        }

        public override void Write()
        {
            throw new NotImplementedException();
        }
    }

}

