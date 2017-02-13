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
using System.IO;

namespace Game.DataClasses.Network.GameClient
{
    #region Wear Info
    public class WearInfo : Packet
    {
        #region Get/Set
        public uint Handle { get; set; }
        public int[] BaseModel { get; set; }
        public Item[] EquippedItems { get; set; }
        #endregion

        #region Constructor/Reader/Write
        public WearInfo()
        {
            this.Id = (ushort)GameClientPackets.WearInfo;
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

            int[] itemCode = new int[Player.MaxEquipPos];
            int[] itemEnhance = new int[Player.MaxEquipPos];
            int[] itemLevel = new int[Player.MaxEquipPos];
            byte[] elementalType = new byte[Player.MaxEquipPos];

            for (int i = 0; i < 24; ++i)
            {
                if (this.EquippedItems[i] != null)
                {
                    itemCode[i] = this.EquippedItems[i].Base.Id;
                    itemEnhance[i] = this.EquippedItems[i].Enhance;
                    itemLevel[i] = this.EquippedItems[i].Level;
                    elementalType[i] = this.EquippedItems[i].ElementalEffectType;
                }
            }

            // Hands and feet when without equipment must have its BaseModel ID
            if (itemCode[4] == 0) // Hands
                itemCode[4] = this.BaseModel[3];
            if (itemCode[5] == 0) // Feet
                itemCode[5] = this.BaseModel[4];

            // Write packet body
            writer.Write(this.Handle);
            for (int i = 0; i < 24; i++)
                writer.Write(itemCode[i]);
            for (int i = 0; i < 24; i++)
                writer.Write(itemEnhance[i]);
            for (int i = 0; i < 24; i++)
                writer.Write(itemLevel[i]);
            for (int i = 0; i < 24; i++)
                writer.Write(elementalType[i]);

            // finishes packet
            base.Complete(writer);

            return stream.ToArray();
        }
        #endregion
    }
    #endregion
}