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
using Game.DataClasses.Objects;
using System;
using System.IO;

namespace Game.DataClasses.Network.GameClient
{
    #region Object Enter Info
    public abstract class Enter : Packet
    {
        #region Get/Set
        public ObjectType Type { get; set; }
        public uint GID { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public byte Layer { get; set; }
        public ObjectSubType SubType { get; set; }

        #endregion

        #region Constructor/Reader/Write
        public Enter()
        {
            this.Id = (ushort)GameClientPackets.Enter;
        }

        public override void Read(byte[] data)
        {
            throw new NotImplementedException();
        }

        protected void Write(BinaryWriter writer)
        {
            // Write packet body
            writer.Write((byte)this.Type);
            writer.Write(this.GID);
            writer.Write(this.X);
            writer.Write(this.Y);
            writer.Write(this.Z);
            writer.Write(this.Layer);
            writer.Write((byte)this.SubType);
        }
        #endregion
    }
    #endregion
}