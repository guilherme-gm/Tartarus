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
    #region Inventory
    public class Inventory : Packet
    {
        #region Get/Set
        public ushort Count { get; set; }
        public ItemInfo[] Items { get; set; }
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
            foreach (ItemInfo item in this.Items)
            {
                writer.Write(item.Base.Handle);
                writer.Write(item.Base.Code);
                writer.Write(item.Base.Uid);
                writer.Write(item.Base.Count);
                writer.Write(item.Base.EtherealDurability);
                writer.Write(item.Base.Endurance);
                writer.Write(item.Base.Enhance);
                writer.Write(item.Base.Level);
                writer.Write(item.Base.Flag);
                for (int i = 0; i < 4; i++)
                    writer.Write(item.Base.Socket[i]);
                writer.Write(item.Base.RemainTime);
                writer.Write(item.Base.ElementalEffectType);
                writer.Write(item.Base.ElementalEffectRemainTime);
                writer.Write(item.Base.ElementalEffectAttackPoint);
                writer.Write(item.Base.ElementalEffectMagicPoint);
                writer.Write(item.WearPosition);
                writer.Write(item.OwnSummonHandle);
                writer.Write(item.Index);
            }

            // finishes packet
            base.Write(writer);

            return stream.ToArray();
        }
        #endregion
    }
    #endregion
}