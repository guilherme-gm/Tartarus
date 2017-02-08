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
    public class PlayerEnter : CreatureEnter
    {
        #region Get/Set
        public byte Sex { get; set; }
        public int FaceId { get; set; }
        public int FaceTextureId { get; set; }
        public int HairId { get; set; }
        public string Name { get; set; }
        public ushort JobId { get; set; }
        public uint RideGID { get; set; }
        public int GuildId { get; set; }
        #endregion

        #region Constructor/Reader/Write
        public PlayerEnter() : base()
        {
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

            // Writes inherited structure
            base.Write(writer);

            // Write packet body
            writer.Write(this.Sex);
            writer.Write(this.FaceId);
            writer.Write(this.FaceTextureId);
            writer.Write(this.HairId);
            this.WriteString(writer, this.Name, 19);
            writer.Write(this.JobId);
            writer.Write(this.RideGID);
            writer.Write(this.GuildId);

            // Complete the packet
            this.Complete(writer);

            return stream.ToArray();
        }
        #endregion
    }
    #endregion
}