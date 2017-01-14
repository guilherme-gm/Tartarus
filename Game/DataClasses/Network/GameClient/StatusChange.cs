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
    #region Status Change
    public class StatusChange : Packet
    {
        #region Get/Set
        public uint Handle { get; set; }
        public uint Status { get; set; }
        #endregion

        #region Constructor/Reader/Write
        public StatusChange()
        {
            this.Id = (ushort)GameClientPackets.StatusChange;
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
            writer.Write(this.Handle);
            writer.Write(this.Status);

            // finishes packet
            base.Write(writer);

            return stream.ToArray();
        }
        #endregion
    }
    #endregion
}