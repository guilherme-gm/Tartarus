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
using FileHelpers;

namespace Game.DataClasses.Database.Records
{
    /// <summary>
    /// This class represents a record of JobLevelBonus on CSV files.
    /// </summary>
    [DelimitedRecord(","), IgnoreFirst(1)]
    public class JobLevelBonusRecord
    {
        #region Columns
        public int JobId;
        public string DummyName;
        public float Str1;
        public float Vit1;
        public float Dex1;
        public float Agi1;
        public float Int1;
        public float Wis1;
        public float Luk1;
        public float Str2;
        public float Vit2;
        public float Dex2;
        public float Agi2;
        public float Int2;
        public float Wis2;
        public float Luk2;
        public float Str3;
        public float Vit3;
        public float Dex3;
        public float Agi3;
        public float Int3;
        public float Wis3;
        public float Luk3;
        #endregion
    }
}
