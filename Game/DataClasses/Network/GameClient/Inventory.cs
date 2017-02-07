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
using Game.DataClasses.Objects;
using System;
using System.Collections.Generic;
using System.IO;

namespace Game.DataClasses.Network.GameClient
{
    #region Inventory
    public class Inventory : Packet
    {
        #region Get/Set
        public ushort Count { get; set; }
        public DataClasses.Inventory ItemList { get; set; }
        #endregion

        #region Constructor/Reader/Write
        public Inventory()
        {
            this.Id = (ushort)GameClientPackets.Inventory;
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
            writer.Write(this.Count);
            IEnumerator<Item> enumerator = this.ItemList.GetEnumerator();
            while(enumerator.MoveNext())
            {
                Item item = enumerator.Current;

                writer.Write(item.GID);
                writer.Write(item.Base.Id);
                writer.Write(item.Id);
                writer.Write(item.Amount);
                writer.Write(item.Durability);
                writer.Write(item.Endurance);
                writer.Write(item.Enhance);
                writer.Write(item.Level);
                writer.Write(item.Flag);
                for (int i = 0; i < Item.MaxSockets; i++)
                    writer.Write(item.Socket[i]);
                writer.Write(item.RemainTime);
                writer.Write(item.ElementalEffectType);
                writer.Write((int)0);// TODO :writer.Write(item.ElementalEffectRemainTime);
                writer.Write(item.ElementalEffectAttackPoint);
                writer.Write(item.ElementalMagicPoint);
                writer.Write((short)item.EquipPosition);
                writer.Write(item.OwnSummonHandle);
                writer.Write(item.Idx);
            }

            // finishes packet
            base.Complete(writer);

            return stream.ToArray();
        }
        #endregion
    }
    #endregion
}