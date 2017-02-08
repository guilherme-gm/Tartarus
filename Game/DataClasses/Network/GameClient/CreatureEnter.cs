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

namespace Game.DataClasses.Network.GameClient
{
    #region Object Enter Info
    public abstract class CreatureEnter : Enter
    {
        #region Get/Set
        public uint Status { get; set; }
        public float FaceDirection { get; set; }
        public int Hp { get; set; }
        public int MaxHp { get; set; }
        public int Mp { get; set; }
        public int MaxMp { get; set; }
        public int Level { get; set; }
        public byte Race { get; set; }
        public uint SkinColor { get; set; }
        public bool IsFirstEnter { get; set; }
        public int Energy { get; set; }
        #endregion

        #region Constructor/Reader/Write
        public CreatureEnter() : base()
        {
        }

        public override void Read(byte[] data)
        {
            throw new NotImplementedException();
        }

        protected new void Write(BinaryWriter writer)
        {
            // Writes inherited structure
            base.Write(writer);

            // Write packet body
            writer.Write(this.Status);
            writer.Write(this.FaceDirection);
            writer.Write(this.Hp);
            writer.Write(this.MaxHp);
            writer.Write(this.Mp);
            writer.Write(this.MaxMp);
            writer.Write(this.Level);
            writer.Write(this.Race);
            writer.Write(this.SkinColor);
            writer.Write(this.IsFirstEnter);
            writer.Write(this.Energy);
        }
        #endregion
    }
    #endregion
}