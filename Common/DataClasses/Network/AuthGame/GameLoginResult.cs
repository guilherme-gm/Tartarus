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

namespace Common.DataClasses.Network.AuthGame
{
	public class GameLoginResult : Packet
	{
        public enum ResultCodes : ushort
        {
            Success = 0,
            DuplicatedId = 1
        }

        public ResultCodes Result { get; set; }

        public GameLoginResult()
        {
            this.Id = (ushort)AuthGamePackets.GameLoginResult;
        }

        public override void Read(byte[] data)
        {
            BinaryReader br = new BinaryReader(new MemoryStream(data));
            base.Read(br);

            this.Result = (ResultCodes) br.ReadUInt16();
        }

        public override byte[] Write()
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);

            // Writes a fake header
            writer.Write(new byte[HeaderSize]);

            // Write packet body
            writer.Write((ushort)this.Result);

            // finishes packet
            base.Write(writer);

            return stream.ToArray();
        }
    }

}

