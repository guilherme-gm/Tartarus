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
using System;
using System.IO;
using System.Text;

namespace Common.DataClasses.Network.AuthGame
{
	public class ClientLogin : Packet
	{
        public string UserId { get; set; }

        public int AccountId { get; set; }

        public int Permission { get; set; }

        public ClientLogin()
        {
            this.Id = (ushort)AuthGamePackets.ClientLogin;
        }

        public override void Read(byte[] data)
        {
            BinaryReader br = new BinaryReader(new MemoryStream(data));
            base.Read(br);

            this.UserId = Encoding.UTF8.GetString(br.ReadBytes(61)).TrimEnd('\0');
            this.AccountId = br.ReadInt32();
            this.Permission = br.ReadInt32();
        }

        public override byte[] Write()
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);

            // Writes a fake header
            writer.Write(new byte[HeaderSize]);

            // Write packet body
            this.WriteString(writer, this.UserId, 61);
            writer.Write(this.AccountId);
            writer.Write(this.Permission);

            // finishes packet
            base.Write(writer);

            return stream.ToArray();
        }
    }

}

