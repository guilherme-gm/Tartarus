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
using Game.DataClasses;
using Game.DataClasses.Database;
using System;
using System.Collections.Generic;

namespace Game.DataClasses.Objects
{
    public class Item : GameObject
    {
        public const int MaxSockets = 4;

        #region Object Creation
        private static uint LastUsedHandle = 0x0100; // TODO : Get correct Handle begin
        private static Stack<uint> HandlePool = new Stack<uint>();

        public static Item Create(int itemId)
        {
            ItemBase itemBase = ItemBase.Get(itemId);
            if (itemBase == null)
            {
                return null;
            }
            return Create(itemBase);
        }

        public static Item Create(ItemBase itemBase)
        {
            uint gid;

            // Locks this pool so other threads can't access it
            // during the handle retrieving
            lock (HandlePool)
            {
                if (HandlePool.Count > 0)
                {
                    gid = HandlePool.Pop();
                }
                else
                {
                    gid = LastUsedHandle++;
                }
            }
            
            Item item = new Item(itemBase, gid);
            return item;
        }
        #endregion

        public ItemBase Base { get; set; }
        public int Id { get; set; }
        public int Idx { get; set; }
        public int Code { get; set; }
        public int Amount { get; set; }
        public int Level { get; set; }
        public int Enhance { get; set; }
        public int Durability { get; set; }
        public int Endurance { get; set; }
        public int Flag { get; set; }
        public int EquipPosition { get; set; }
        public int[] Socket { get; set; }
        public int RemainTime { get; set; }
        public byte ElementalEffectType { get; set; }
        public DateTime ElementalEffectExpireTime { get; set; }
        public int ElementalEffectAttackPoint { get; set; }
        public int ElementalMagicPoint { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }

        public uint OwnSummonHandle { get; set; }

        private Item(ItemBase itemBase, uint gid) : base(gid)
        {
            this.Base = itemBase;
            this.Socket = new int[MaxSockets];
            this.ElementalEffectExpireTime = new DateTime();
            this.CreateTime = new DateTime();
            this.UpdateTime = new DateTime();
        }

	}

}

