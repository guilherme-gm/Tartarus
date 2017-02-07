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
using Game.DataClasses.GameWorld;

namespace Game.DataClasses
{
    public enum ObjectType
    {
        Static = 0x0,
        Movable = 0x1,
        Client = 0x2
    }

    public enum ObjectSubType
    {
        Player = 0x0,
        Npc = 0x1,
        Item = 0x2,
        Monster = 0x3,
        Summon = 0x4,
        SkillProp = 0x5,
        FieldProp = 0x6,
        Pet = 0x7
    }

    public abstract class GameObject
	{
		public bool InWorld { get; set; }

		public Region Region { get; set; }

		public Position Position { get; set; }

		public uint GID { get; set; }

        public ObjectType Type { get; set; }

        public ObjectSubType SubType { get; set; }


        public GameObject(uint gid)
        {
            this.GID = gid;
        }
	}

}

