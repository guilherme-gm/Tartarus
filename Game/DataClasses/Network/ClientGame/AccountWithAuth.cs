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

namespace Game.DataClasses.Network.ClientGame
{
    /// <summary>
    /// Player Login to GameServer from AuthServer
    /// </summary>
    public class AccountWithAuth : Packet
    {
        public string Account { get; set; }
        public long OneTimeKey { get; set; }

        public enum ResultCode : ushort
        {
            Success = 0x0,
            // TODO : Find correct code
            Invalid = 0x1
        }

        public override void Read(byte[] data)
        {
            BinaryReader br = new BinaryReader(new MemoryStream(data));
            base.Read(br);

            this.Account = this.ReadString(br, 61);
            this.OneTimeKey = br.ReadInt64();
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }
    }
}
