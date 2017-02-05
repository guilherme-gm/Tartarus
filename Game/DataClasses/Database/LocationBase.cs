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
    public class LocationBase
    {
        public static Dictionary<int, LocationBase> Db;

        #region Database Loading
        public static void Load()
        {
            if (Db != null)
            {
                ConsoleUtils.ShowFatalError("Trying to load already loaded Stat Database. Load Aborted.");
                return;
            }

            Db = new Dictionary<int, LocationBase>();
            ReadDatabase("Database/LocationDatabase.csv", true);
        }

        private static void ReadDatabase(string fileName, bool required)
        {
            int i = 0;

            // File exists check
            if (!File.Exists(fileName) && required)
            {
                ConsoleUtils.ShowFatalError("Could not find database file '{0}'.", fileName);
                return;
            }

            // Starts the engine
            var engine = new FileHelperAsyncEngine<LocationRecord>();
            engine.ErrorMode = ErrorMode.SaveAndContinue;
            using (engine.BeginReadFile(fileName))
            {
                // Read entry
                foreach (LocationRecord record in engine)
                {
                    LocationBase entry;

                    // Inserts entry in Database
                    if (!Db.TryGetValue(record.LocationId, out entry))
                    {
                        ++i;
                        entry = new LocationBase();
                        entry.Id = record.LocationId;
                        Db.Add(record.LocationId, entry);
                    }

                    entry.Type = record.LocationType;
                    entry.WeatherRatio[record.TimeId][record.WeatherId] = record.WeatherRatio;
                    entry.WeatherChangeTime = record.WeatherChangeTime;
                }

                // Show errors
                foreach (var err in engine.ErrorManager.Errors)
                {
                    ConsoleUtils.ShowError("Could not parse entry in '{0}' at line '{1}'. Skipping line.", fileName, err.LineNumber);
                }
            }

            ConsoleUtils.ShowInfo("'{0}' Locals read from '{1}'.", i, fileName);
        }
        #endregion

        #region Database Commands
        public static LocationBase Get(int localId)
        {
            LocationBase local;
            if (!Db.TryGetValue(localId, out local))
            {
                return null;
            }
            return local;
        }
        #endregion

        public LocationBase()
        {
            this.WeatherRatio = new int[4][];
            for (int i = 0; i < 4; i++)
                this.WeatherRatio[i] = new int[7];
        }

        #region Properties
        public int Id { get; private set; }
        public int Type { get; private set; }
        public int[][] WeatherRatio { get; private set; }
        public int WeatherChangeTime { get; private set; }
        #endregion
    }
}
