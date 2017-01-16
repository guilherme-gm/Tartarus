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
using Common.Utils;
using System;
using System.Collections.Generic;

namespace Game.DataClasses.Database
{
    public class JobLevelBonus
    {
        public static Dictionary<int, JobLevelBonus> Db;

        #region Database Loading
        public static void Load()
        {
            if (Db != null)
            {
                ConsoleUtils.ShowFatalError("Trying to load already loaded Job Level Bonus Database. Load Aborted.");
                return;
            }

            Db = new Dictionary<int, JobLevelBonus>();
            StringUtils.ReadDatabase("Database/JobLevelBonusDatabase.csv", 23, ReadEntry, false, true);
        }

        private static void ReadEntry(string fileName, int lineNum, string[] columns, string[] values, bool allowReplace)
        {
            // Read Entry
            JobLevelBonus bonus = new JobLevelBonus();
            int j = 0;
            try
            {
                bonus.JobId = int.Parse(values[j++]);
                j++; // DummyName
                bonus.Str1 = float.Parse(values[j++]);
                bonus.Vit1 = float.Parse(values[j++]);
                bonus.Dex1 = float.Parse(values[j++]);
                bonus.Agi1 = float.Parse(values[j++]);
                bonus.Int1 = float.Parse(values[j++]);
                bonus.Wis1 = float.Parse(values[j++]);
                bonus.Luk1 = float.Parse(values[j++]);
                bonus.Str2 = float.Parse(values[j++]);
                bonus.Vit2 = float.Parse(values[j++]);
                bonus.Dex2 = float.Parse(values[j++]);
                bonus.Agi2 = float.Parse(values[j++]);
                bonus.Int2 = float.Parse(values[j++]);
                bonus.Wis2 = float.Parse(values[j++]);
                bonus.Luk2 = float.Parse(values[j++]);
                bonus.Str3 = float.Parse(values[j++]);
                bonus.Vit3 = float.Parse(values[j++]);
                bonus.Dex3 = float.Parse(values[j++]);
                bonus.Agi3 = float.Parse(values[j++]);
                bonus.Int3 = float.Parse(values[j++]);
                bonus.Wis3 = float.Parse(values[j++]);
                bonus.Luk3 = float.Parse(values[j++]);
            }
            catch (Exception)
            {
                ConsoleUtils.ShowError("Could not parse column '{0}' in '{1}' at line '{2}'. Skipping line.", columns[j - 1], fileName, lineNum);
                return;
            }

            // Inserts entry in Database
            if (Db.ContainsKey(bonus.JobId))
            {
                if (!allowReplace)
                {
                    ConsoleUtils.ShowError("Duplicated code detected in '{0}' at line '{1}'. Skipping entry.", fileName, lineNum);
                    return;
                }
                Db[bonus.JobId] = bonus;
            }
            else
            {
                Db.Add(bonus.JobId, bonus);
            }
        }
        #endregion

        #region Database Commands
        public static JobLevelBonus Get(int jobId)
        {
            JobLevelBonus bonus;
            if (!Db.TryGetValue(jobId, out bonus))
            {
                return null;
            }
            return bonus;
        }
        #endregion

        #region Properties
        public int JobId { get; private set; }
        public float Str1 { get; private set; }
        public float Vit1 { get; private set; }
        public float Dex1 { get; private set; }
        public float Agi1 { get; private set; }
        public float Int1 { get; private set; }
        public float Wis1 { get; private set; }
        public float Luk1 { get; private set; }
        public float Str2 { get; private set; }
        public float Vit2 { get; private set; }
        public float Dex2 { get; private set; }
        public float Agi2 { get; private set; }
        public float Int2 { get; private set; }
        public float Wis2 { get; private set; }
        public float Luk2 { get; private set; }
        public float Str3 { get; private set; }
        public float Vit3 { get; private set; }
        public float Dex3 { get; private set; }
        public float Agi3 { get; private set; }
        public float Int3 { get; private set; }
        public float Wis3 { get; private set; }
        public float Luk3 { get; private set; }
        #endregion
    }
}
