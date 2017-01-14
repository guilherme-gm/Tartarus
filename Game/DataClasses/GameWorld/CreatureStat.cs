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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.DataClasses.GameWorld
{
    public class CreatureStat
    {
        public short StatId { get; set; }
        public short Strength { get; set; }
        public short Vitality { get; set; }
        public short Dexterity { get; set; }
        public short Agility { get; set; }
        public short Intelligence { get; set; }
        public short Mentality { get; set; }
        public short Luck { get; set; }
    }
}
