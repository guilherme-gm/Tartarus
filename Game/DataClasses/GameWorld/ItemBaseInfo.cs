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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.DataClasses.GameWorld
{
    public class ItemBaseInfo
    {
        public uint Handle { get; set; }
        public int Code { get; set; }
        public long Uid { get; set; }
        public long Count { get; set; }
        public int EtherealDurability { get; set; }
        public uint Endurance { get; set; }
        public byte Enhance { get; set; }
        public byte Level { get; set; }
        public int Flag { get; set; }
        public int[] Socket { get; set; }
        public int RemainTime { get; set; }
        public byte ElementalEffectType { get; set; }
        public int ElementalEffectRemainTime { get; set; }
        public int ElementalEffectAttackPoint { get; set; }
        public int ElementalEffectMagicPoint { get; set; }

        public ItemBaseInfo()
        {
            this.Socket = new int[4];
        }
    }
}
