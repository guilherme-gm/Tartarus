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
using Common.DataClasses.Network;
using System.IO;

namespace Auth.DataClasses.Network.AuthClient
{
    #region SelectServer Packets
    public class SelectServer : Packet
	{
        #region Enums
        public enum ResultCodes : ushort
        {
            Success = 0x0,
            NotExist = 0x1,
            AccessDenied = 0x6,
            AlreadyExists = 0x9
        }
        #endregion

        #region Get/Set Packets
        public ushort Result { get; set; }

		public long OneTimeKey { get; set; }

		public uint PendingTime { get; set; }
        #endregion

        #region SelectServer/Read/Write Packets
        public SelectServer()
        {
            this.Id = (ushort)AuthClientPackets.SelectServer;
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
            writer.Write(this.Result);
            writer.Write(this.OneTimeKey);
            writer.Write(this.PendingTime);

            // finishes packet
            base.Write(writer);

            return stream.ToArray();
        }
        #endregion
    }
    #endregion
}

