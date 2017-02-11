
using System;
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
using Game.DataClasses.GameWorld;
using System.IO;

namespace Game.DataClasses.Network.ClientGame
{
    #region Chat Request
    public class ChatRequest : Packet
    {
        #region Get/Set
        public string Target { get; set; }
        public byte RequestId { get; set; }
        public byte MessageLength { get; set; }
        public ChatType Type { get; set; }
        public string Message { get; set; }
        #endregion

        #region Result Enum
        public enum Result : ushort
        {
            Success = 0x0
        }
        #endregion

        #region Constructor/Read/Write
        public override void Read(byte[] data)
        {
            BinaryReader br = new BinaryReader(new MemoryStream(data));
            base.Read(br);

            this.Target = this.ReadString(br, 21);
            this.RequestId = br.ReadByte();
            this.MessageLength = br.ReadByte();
            this.Type = (ChatType) br.ReadByte();
            this.Message = this.ReadString(br, this.MessageLength);
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
    #endregion
}
