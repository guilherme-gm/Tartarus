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
using System.IO;
using System.Text;

namespace Common.DataClasses.Network
{
	public abstract class Packet
	{
        public const int HeaderSize = 7;

		public int Size { get; set; }

		public ushort Id { get; set; }

		public byte Checksum { get; set; }

        public abstract void Read(byte[] data);

        public abstract byte[] Write();

		protected void Write(BinaryWriter writer)
		{
            this.Size = (int)writer.BaseStream.Position;
            writer.Seek(0, SeekOrigin.Begin);

            byte[] header = new byte[HeaderSize];

            Buffer.BlockCopy(BitConverter.GetBytes(this.Size), 0, header, 0, 4);
            Buffer.BlockCopy(BitConverter.GetBytes(this.Id), 0, header, 4, 2);

            byte checksum = 0;
            for (int i = 0; i < HeaderSize; i++)
            {
                unchecked { checksum += header[i]; }
            }

            header[HeaderSize - 1] = checksum;

            writer.Write(header);
		}

		protected void Read(BinaryReader data)
		{
            this.Size = data.ReadInt32();
            this.Id = data.ReadUInt16();
            this.Checksum = data.ReadByte();
		}

        protected void WriteString(BinaryWriter writer, string value, int length)
        {
            writer.Write(Encoding.UTF8.GetBytes(value));
            writer.Write(new byte[length - value.Length]);
        }

        protected string ReadString(BinaryReader reader, int length)
        {
            return Encoding.UTF8.GetString(reader.ReadBytes(length)).TrimEnd('\0');
        }
	}

}

