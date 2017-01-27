

using System;
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
namespace Game.DataClasses.Lobby
{
    public class LobbyCharacterInfo
    {
        public ModelInfo ModelInfo { get; set; }
        public int Level { get; set; }
        public int Job { get; set; }
        public int JobLevel { get; set; }
        public int ExpPercentage { get; set; }
        public int Hp { get; set; }
        public int Mp { get; set; }
        public int Permission { get; set; }
        public bool Banned { get; set; }
        public string Name { get; set; } // 19
        public uint SkinColor { get; set; }
        public string CreateTime { get; set; }//30
        public string DeleteTime { get; set; } //30
        public int[] WearItemEnhanceInfo { get; set; } // 24
        public int[] WearItemLevelInfo { get; set; } // 24
        public byte[] WearItemElementalType { get; set; } //24
        public DateTime LoginTime { get; set; }

        public LobbyCharacterInfo()
        {
            this.ModelInfo = new ModelInfo();
            this.WearItemEnhanceInfo = new int[24];
            this.WearItemLevelInfo = new int[24];
            this.WearItemElementalType = new byte[24];
        }
    }
}
