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
using System;

namespace Game.DataClasses
{
    public class Item : GameObject
    {
        public const int MaxSockets = 4;

        //private ItemBase itemBase;
        public int ItemId { get; set; }
        public int Idx { get; set; }
        public int Code { get; set; }
        public int Amount { get; set; }
        public int Level { get; set; }
        public int Enhance { get; set; }
        public int Durability { get; set; }
        public int Endurance { get; set; }
        public int Flag { get; set; }
        public int Position { get; set; }
        public int[] Socket { get; set; }
        public int RemainTime { get; set; }
        public byte ElementalEffectType { get; set; }
        public DateTime ElementalEffectExpireTime { get; set; }
        public int ElementalEffectAttackPoint { get; set; }
        public int ElementalMagicPoint { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }

        public Item()
        {
            this.Socket = new int[MaxSockets];
            this.ElementalEffectExpireTime = new DateTime();
            this.CreateTime = new DateTime();
            this.UpdateTime = new DateTime();
        }

	}

}

