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
            ReadDatabase("Database/JobDatabase.csv", true, false);
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
            var engine = new FileHelperAsyncEngine<JobBaseRecord>();
            engine.ErrorMode = ErrorMode.SaveAndContinue;
            using (engine.BeginReadFile(fileName))
            {
                // Read entry
                foreach (JobBaseRecord record in engine)
                {
                    JobBase job = RecordToEntry(record);

                    // Ensure stat exists
                    if (job.Stat == null)
                    {
                        ConsoleUtils.ShowError("Job '{0}' StatId '{1}' not found on Stat Database in '{2}' line '{3}'. Skipping line.", job.Id, record.StatId, fileName, engine.LineNumber);
                        continue;
                    }

                    // Inserts entry in Database
                    if (Db.ContainsKey(job.Id))
                    {
                        if (!allowReplace)
                        {
                            ConsoleUtils.ShowError("Duplicated JobId '{0}' detected in '{1}' at line '{2}'. Skipping entry.", job.Id, fileName, engine.LineNumber);
                            continue;
                        }
                        Db[job.Id] = job;
                    }
                    else
                    {
                        Db.Add(job.Id, job);
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

        private static JobBase RecordToEntry(JobBaseRecord record) {
            JobBase job = new JobBase();

            // Map
            job.Id = record.Id;
            job.Stat = StatBase.Get(record.StatId);
            job.Class = record.Class;
            job.Depth = record.Depth;
            job.MaxLevel = record.MaxLevel;
            job.MaxJLevel = record.MaxJLevel;
            job.NextJobs = record.NextJobs;

            return job;
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
