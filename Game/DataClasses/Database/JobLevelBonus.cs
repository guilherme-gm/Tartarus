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
using FileHelpers;
using Game.DataClasses.Database.Records;
using System.Collections.Generic;
using System.IO;

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
            ReadDatabase("Database/JobLevelBonusDatabase.csv", true, false);
        }

        private static void ReadDatabase(string fileName, bool required, bool allowReplace)
        {
            int i = 0;

            // File exists check
            if (!File.Exists(fileName) && required)
            {
                ConsoleUtils.ShowFatalError("Could not find database file '{0}'.", fileName);
                return;
            }

            // Starts the engine
            var engine = new FileHelperAsyncEngine<JobLevelBonusRecord>();
            engine.ErrorMode = ErrorMode.SaveAndContinue;
            using (engine.BeginReadFile(fileName))
            {
                // Read entry
                foreach (JobLevelBonusRecord record in engine)
                {
                    JobLevelBonus bonus = RecordToEntry(record);

                    // Inserts entry in Database
                    if (Db.ContainsKey(bonus.JobId))
                    {
                        if (!allowReplace)
                        {
                            ConsoleUtils.ShowError("Duplicated JobId '{0}' detected in '{1}' at line '{2}'. Skipping entry.", bonus.JobId, fileName, engine.LineNumber);
                            continue;
                        }
                        Db[bonus.JobId] = bonus;
                    }
                    else
                    {
                        Db.Add(bonus.JobId, bonus);
                    }
                    i++;
                }

                // Show errors
                foreach (var err in engine.ErrorManager.Errors)
                {
                    ConsoleUtils.ShowError("Could not parse entry in '{0}' at line '{1}'. Skipping line.", fileName, err.LineNumber);
                }
            }

            ConsoleUtils.ShowInfo("'{0}' entries read from '{1}'.", i, fileName);
        }

        private static JobLevelBonus RecordToEntry(JobLevelBonusRecord record)
        {
            JobLevelBonus bonus = new JobLevelBonus();

            // Map
            bonus.JobId = record.JobId;
            bonus.Str1 = record.Str1;
            bonus.Vit1 = record.Vit1;
            bonus.Dex1 = record.Dex1;
            bonus.Agi1 = record.Agi1;
            bonus.Int1 = record.Int1;
            bonus.Wis1 = record.Wis1;
            bonus.Luk1 = record.Luk1;
            bonus.Str2 = record.Str2;
            bonus.Vit2 = record.Vit2;
            bonus.Dex2 = record.Dex2;
            bonus.Agi2 = record.Agi2;
            bonus.Int2 = record.Int2;
            bonus.Wis2 = record.Wis2;
            bonus.Luk2 = record.Luk2;
            bonus.Str3 = record.Str3;
            bonus.Vit3 = record.Vit3;
            bonus.Dex3 = record.Dex3;
            bonus.Agi3 = record.Agi3;
            bonus.Int3 = record.Int3;
            bonus.Wis3 = record.Wis3;
            bonus.Luk3 = record.Luk3;

            return bonus;
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
