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
    public class JobBase
    {
        public static Dictionary<int, JobBase> Db;

        #region Database Loading
        public static void Load()
        {
            if (Db != null)
            {
                ConsoleUtils.ShowFatalError("Trying to load already loaded Job Database. Load Aborted.");
                return;
            }

            Db = new Dictionary<int, JobBase>();
            StringUtils.ReadDatabase("Database/JobDatabase.csv", 10, ReadEntry, false, true);
        }

        private static void ReadEntry(string fileName, int lineNum, string[] columns, string[] values, bool allowReplace)
        {
            // Read Entry
            JobBase job = new JobBase();
            short statId;
            int j = 0;
            try
            {
                job.Id = int.Parse(values[j++]);
                j++; // Dummy name
                statId = short.Parse(values[j++]);
                job.Class = byte.Parse(values[j++]);
                job.Depth = byte.Parse(values[j++]);
                job.MaxLevel = short.Parse(values[j++]);
                job.MaxJLevel = short.Parse(values[j++]);
                job.NextJobs = new int[3];
                job.NextJobs[0] = int.Parse(values[j++]);
                job.NextJobs[1] = int.Parse(values[j++]);
                job.NextJobs[2] = int.Parse(values[j++]);
            }
            catch (Exception)
            {
                ConsoleUtils.ShowError("Could not parse column '{0}' in '{1}' at line '{2}'. Skipping line.", columns[j - 1], fileName, lineNum);
                return;
            }

            // Ensure stat exists
            job.Stat = StatBase.Get(statId);
            if (job.Stat == null)
            {
                ConsoleUtils.ShowError("Job '{0}' StatId '{1}' not found on Stat Database in '{2}' line '{3}'. Skipping line.", job.Id, statId, fileName, lineNum);
                return;
            }

            // Inserts entry in Database
            if (Db.ContainsKey(job.Id))
            {
                if (!allowReplace)
                {
                    ConsoleUtils.ShowError("Duplicated code detected in '{0}' at line '{1}'. Skipping entry.", fileName, lineNum);
                    return;
                }
                Db[job.Id] = job;
            }
            else
            {
                Db.Add(job.Id, job);
            }
        }
        #endregion

        #region Database Commands
        public static JobBase Get(int jobId)
        {
            JobBase job;
            if (!Db.TryGetValue(jobId, out job))
            {
                return null;
            }
            return job;
        }
        #endregion

        #region Properties
        public int Id { get; private set; }
        public StatBase Stat { get; private set; }
        public byte Class { get; private set; }
        public byte Depth { get; private set; }
        public short MaxLevel { get; private set; }
        public short MaxJLevel { get; private set; }
        public int[] NextJobs { get; private set; }
        #endregion
    }
}
