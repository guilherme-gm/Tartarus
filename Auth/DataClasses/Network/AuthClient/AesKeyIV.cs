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
using Auth.DataClasses.Network;
using Common.DataClasses.Network;
using System;
using System.IO;

namespace Auth.DataClasses.Network.AuthClient
{
    #region AesKeyIV Packet
    public class AesKeyIV : Packet
    {
        #region Get/Set Packets
        public int DataSize { get; set; }

        public byte[] RSAEncryptedData { get; set; }
        #endregion

        #region Result/Read/Write Packets
        public AesKeyIV()
        {
            this.Id = (ushort)AuthClientPackets.AesKeyIV;
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
            writer.Write(this.DataSize);
            writer.Write(this.RSAEncryptedData);

            // finishes packet
            base.Complete(writer);

            return stream.ToArray();
        }
        #endregion
    }
    #endregion
}

