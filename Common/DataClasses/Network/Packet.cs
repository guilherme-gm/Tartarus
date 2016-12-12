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
using System.IO;

namespace Common.DataClasses.Network
{
	public abstract class Packet
	{
		public int Size { get; set; }

		public short Id { get; set; }

		public byte Checksum { get; set; }

        public abstract void Read(byte[] data);

        public abstract void Write();

		protected void Write(int stream)
		{

		}

		protected void Read(BinaryReader data)
		{
            this.Size = data.ReadInt32();
            this.Id = data.ReadInt16();
            this.Checksum = data.ReadByte();
		}

	}

}

