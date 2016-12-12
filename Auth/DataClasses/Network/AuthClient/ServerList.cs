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

namespace Auth.DataClasses.Network.AuthClient
{
	public class ServerList : Packet
	{
		public ushort LastLoginServerId { get; set; }

        public ushort Count { get; private set; }

        private ServerInfo[] _ServerInfo;

        public ServerInfo[] ServerInfo
        {
            get
            {
                return _ServerInfo;
            }

            set
            {
                _ServerInfo = value;
                this.Count = (ushort)_ServerInfo.Length;
            }
        }

        public ServerList()
        {
            this.Id = (ushort) AuthClientPackets.ServerList;
        }

        public override void Read(byte[] data)
        {
            throw new NotImplementedException();
        }

        public override byte[] Write()
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);

            // Writes a fake header
            writer.Write(new byte[HeaderSize]);

            // Write packet body
            writer.Write(this.LastLoginServerId);
            writer.Write(this.Count);
            foreach (ServerInfo info in this.ServerInfo)
            {
                writer.Write(info.Id);
                WriteString(writer, info.Name, 21);
                writer.Write(info.AdultServer);
                WriteString(writer, info.ScreenshotUrl, 256);
                WriteString(writer, info.Ip, 16);
                writer.Write(info.Port);
                writer.Write(info.UserRatio);
            }

            // finishes packet
            base.Write(writer);

            return stream.ToArray();
        }
    }

}

