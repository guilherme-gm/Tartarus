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
using System;
using System.IO;

namespace Game.DataClasses.Network.GameClient
{
    #region Quest List
    public class QuestList : Packet
    {
        #region Get/Set
        public QuestInfo[] ActiveQuests { get; set; }
        public PendingQuestInfo[] PendingQuests { get; set; }

        #endregion

        #region Constructor/Reader/Write
        public QuestList()
        {
            this.Id = (ushort)GameClientPackets.QuestList;
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
            writer.Write(this.ActiveQuests.Length);
            writer.Write(this.PendingQuests.Length);
            foreach (QuestInfo quest in this.ActiveQuests)
            {
                writer.Write(quest.Code);
                writer.Write(quest.StartID);
                for (int i = 0; i < 6; i++)
                    writer.Write(quest.RandomValue[i]);
                for (int i = 0; i < 6; i++)
                    writer.Write(quest.Status[i]);
                writer.Write(quest.Progress);
                writer.Write(quest.TimeLimit);
            }
            foreach (PendingQuestInfo quest in this.PendingQuests)
            {
                writer.Write(quest.Code);
                writer.Write(quest.StartID);
            }
                // finishes packet
                base.Complete(writer);

            return stream.ToArray();
        }
        #endregion
    }
    #endregion
}