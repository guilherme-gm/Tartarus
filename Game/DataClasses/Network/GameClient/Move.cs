﻿/**
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

namespace Game.DataClasses.Network.GameClient
{
    #region Move
    public class Move : Packet
    {
        #region Sub classes
        public class MoveInfo
        {
            public float ToX { get; set; }
            public float ToY { get; set; }
        }
        #endregion

        #region Get/Set
        public uint StartTime { get; set; }
        public uint GID { get; set; }
        public byte ToLayer { get; set; }
        public byte Speed { get; set; }
        public MoveInfo[] Points { get; set; }
        #endregion

        #region Constructor/Reader/Write
        public Move()
        {
            this.Id = (ushort)GameClientPackets.Move;
        }

        public override void Read(byte[] data)
        {
            throw new NotImplementedException();
        }

        public override byte[] Write()
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);
            this.Speed /= 10;

            // Writes a fake header
            writer.Write(new byte[HeaderSize]);

            // Write packet body
            writer.Write(this.StartTime);
            writer.Write(this.GID);
            writer.Write(this.ToLayer);
            writer.Write(this.Speed);
            writer.Write((ushort)this.Points.Length);
            for (int i = 0; i < this.Points.Length; ++i)
            {
                writer.Write(this.Points[i].ToX);
                writer.Write(this.Points[i].ToY);
            }

            // finishes packet
            base.Complete(writer);

            return stream.ToArray();
        }
        #endregion
    }
    #endregion
}