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
    public class CreatureAttribute
    {
        public short Critical { get; set; } // Crit Ratio
        public short CriticalPower { get; set; }
        public short AttackPointRight { get; set; }
        public short AttackPointLeft { get; set; }
        public short Defence { get; set; }
        public short BlockDefence { get; set; }
        public short MagicPoint { get; set; }
        public short MagicDefence { get; set; }
        public short AccuracyRight { get; set; }
        public short AccuracyLeft { get; set; }
        public short MagicAccuracy { get; set; }
        public short Avoid { get; set; }
        public short MagicAvoid { get; set; }
        public short BlockChance { get; set; }
        public short MoveSpeed { get; set; }
        public short AttackSpeed { get; set; }
        public short AttackRange { get; set; }
        public short MaxWeight { get; set; }
        public short CastingSpeed { get; set; }
        public short CoolTimeSpeed { get; set; }
        public short ItemChance { get; set; }
        public short HPRegenPercentage { get; set; }
        public short HPRegenPoint { get; set; }
        public short MPRegenPercentage { get; set; }
        public short MPRegenPoint { get; set; }
    }
}
