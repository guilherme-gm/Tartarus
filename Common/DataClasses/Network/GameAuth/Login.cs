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
using Common.DataClasses;
using System;
using System.IO;
using System.Text;

namespace Common.DataClasses.Network.GameAuth
{
	public class Login : Packet
	{
        public ServerInfo ServerInfo { get; set; }
        public string Password { get; set; }

        public Login()
        {
            this.Id = (ushort) GameAuthPackets.Login;
        }

        public override void Read(byte[] data)
        {
            BinaryReader br = new BinaryReader(new MemoryStream(data));
            base.Read(br);

            this.ServerInfo = new ServerInfo()
            {
                Id = br.ReadUInt16(),
                Name = this.ReadString(br, 21),
                AdultServer = br.ReadBoolean(),
                ScreenshotUrl = this.ReadString(br, 256),
                Ip = this.ReadString(br, 16),
                Port = br.ReadInt32(),
                UserRatio = br.ReadUInt16()
            };
            this.Password = this.ReadString(br, 30);
        }

        public override byte[] Write()
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);

            // Writes a fake header
            writer.Write(new byte[HeaderSize]);

            // Write packet body
            writer.Write(this.ServerInfo.Id);
            WriteString(writer, this.ServerInfo.Name, 21);
            writer.Write(this.ServerInfo.AdultServer);
            WriteString(writer, this.ServerInfo.ScreenshotUrl, 256);
            WriteString(writer, this.ServerInfo.Ip, 16);
            writer.Write(this.ServerInfo.Port);
            writer.Write(this.ServerInfo.UserRatio);
            WriteString(writer, this.Password, 30);

            // finishes packet
            base.Complete(writer);

            return stream.ToArray();
        }
    }

}

