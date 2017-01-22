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
using Game.DataClasses.GameWorld;

namespace Game.DataClasses
{
	public abstract class Creature : GameObject
	{
        public const int MaxPrevJobs = 3;

        public int Race { get; set; }

        public int Level { get; set; }

        public int MaxReachedLevel { get; set; }

        public long Exp { get; set; }

        public long LastDecreasedExp { get; set; }

        public int HP { get; set; }
        public int MaxHp { get; set; }

        public int MP { get; set; }

        public int MaxMP { get; set; }
        
        public int Stamina { get; set; }

        public int MaxStamina { get; set; }

        public int Havoc { get; set; }

        public int MaxHavoc { get; set; }

        public JobBase Job { get; set; }

        public int JobLevel { get; set; }

        public JobBase[] PrevJobs { get; set; }

        public int[] PrevJobLevel { get; set; }

        public int JP { get; set; }

        public int TotalJP { get; set; }

        public CreatureStat Stats { get; set; }
        public CreatureStat StatsByState { get; set; }


        public Creature(uint gid) : base(gid)
        {
            this.Stats = new CreatureStat();
            this.StatsByState = new CreatureStat();
            this.PrevJobs = new JobBase[MaxPrevJobs];
            this.PrevJobLevel = new int[MaxPrevJobs];
        }
    }

}

