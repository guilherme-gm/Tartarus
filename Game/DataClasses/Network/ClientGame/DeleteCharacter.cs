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

namespace Game.DataClasses.Network.ClientGame
{
    #region Delete Character Packet
    public class DeleteCharacter : Packet
    {
        #region Get/Set
        public string CharacterName { get; set; }
        public string SecurityNo { get; set; }

        public enum ResultCodes : ushort
        {
            Success = 0x0
        }
        #endregion

        #region Read/Write
        public override void Read(byte[] data)
        {
            BinaryReader br = new BinaryReader(new MemoryStream(data));
            base.Read(br);

            this.CharacterName = this.ReadString(br, 19);
            this.SecurityNo = this.ReadString(br, 19);
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
    #endregion
}
