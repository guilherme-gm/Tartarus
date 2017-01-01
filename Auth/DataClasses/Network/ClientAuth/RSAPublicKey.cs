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
using System.Text;

namespace Auth.DataClasses.Network.ClientAuth
{
    #region Account Packets
    public class RSAPublicKey : Packet
    {
        #region Get/Set Packets
        public int KeySize { get; set; }

        public string Key { get; set; }
        #endregion

        #region Read/Write Packets
        public override void Read(byte[] data)
        {
            BinaryReader br = new BinaryReader(new MemoryStream(data));
            base.Read(br);

            this.KeySize = br.ReadInt32();
            this.Key = this.ReadString(br, this.KeySize);
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
    #endregion
}

