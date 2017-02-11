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
using Game.DataClasses.GameWorld;
using System;
using System.IO;

namespace Game.DataClasses.Network.GameClient
{
    #region Chat
    public class Chat : Packet
    {
        #region Get/Set
        public string Sender { get; set; }
        public ChatType Type { get; set; }
        public string Message { get; set; }
        #endregion

        #region Constructor/Reader/Write
        public Chat()
        {
            this.Id = (ushort)GameClientPackets.Chat;
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
            this.WriteString(writer, this.Sender, 21);
            writer.Write((ushort)this.Message.Length + 1);
            writer.Write((byte)this.Type);
            this.WriteString(writer, this.Message, this.Message.Length);

            // finishes packet
            base.Complete(writer);

            return stream.ToArray();
        }
        #endregion
    }
    #endregion
}