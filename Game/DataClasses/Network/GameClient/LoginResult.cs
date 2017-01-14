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

namespace Game.DataClasses.Network.GameClient
{
    #region Login Result
    public class LoginResult : Packet
    {
        #region Get/Set
        public bool IsAccepted { get; set; }
        public uint Handle { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public byte Layer { get; set; }
        public float FaceDirection { get; set; }
        public int RegionSize { get; set; }
        public int HP { get; set; }
        public short MP { get; set; }
        public int MaxHP { get; set; }
        public short MaxMP { get; set; }
        public int Havoc { get; set; }
        public int MaxHavoc { get; set; }
        public int Sex { get; set; }
        public int Race { get; set; }
        public uint SkinColor { get; set; }
        public int FaceId { get; set; }
        public int HairId { get; set; }
        public string Name { get; set; }
        public int CellSize { get; set; }
        public int GuildId { get; set; }
        #endregion

        #region Constructor/Reader/Write
        public LoginResult()
        {
            this.Id = (ushort)GameClientPackets.LoginResult;
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
            writer.Write(this.IsAccepted);
            writer.Write(this.Handle);
            writer.Write(this.X);
            writer.Write(this.Y);
            writer.Write(this.Z);
            writer.Write(this.Layer);
            writer.Write(this.FaceDirection);
            writer.Write(this.RegionSize);
            writer.Write(this.HP);
            writer.Write(this.MP);
            writer.Write(this.MaxHP);
            writer.Write(this.MaxMP);
            writer.Write(this.Havoc);
            writer.Write(this.MaxHavoc);
            writer.Write(this.Sex);
            writer.Write(this.Race);
            writer.Write(this.SkinColor);
            writer.Write(this.FaceId);
            writer.Write(this.HairId);
            this.WriteString(writer, this.Name, 19);
            writer.Write(this.CellSize);
            writer.Write(this.GuildId);

            // finishes packet
            base.Write(writer);

            return stream.ToArray();
        }
        #endregion
    }
    #endregion
}