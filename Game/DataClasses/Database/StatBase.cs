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
using System;
using System.Collections.Generic;
using System.IO;

namespace Game.DataClasses.Database
{
    [DelimitedRecord(","), IgnoreFirst(1)]
    public class StatBase
    {
        public static Dictionary<short, StatBase> Db;

        #region Database Loading
        public static void Load()
        {
            if (Db != null)
            {
                ConsoleUtils.ShowFatalError("Trying to load already loaded Stat Database. Load Aborted.");
                return;
            }

            Db = new Dictionary<short, StatBase>();
            ReadDatabase("Database/StatDatabase.csv", true, false);
        }

        private static void ReadDatabase(string fileName,  bool required, bool allowReplace)
        {
            int i = 0;

            // File exists check
            if (!File.Exists(fileName) && required)
            {
                ConsoleUtils.ShowFatalError("Could not find database file '{0}'.", fileName);
                return;
            }

            // Starts the engine
            var engine = new FileHelperAsyncEngine<StatBase>();
            engine.ErrorMode = ErrorMode.SaveAndContinue;
            using (engine.BeginReadFile(fileName))
            {
                // Read entry
                foreach (StatBase stat in engine)
                {
                    // Inserts entry in Database
                    if (Db.ContainsKey(stat.Id))
                    {
                        if (!allowReplace)
                        {
                            ConsoleUtils.ShowError("Duplicated id '{0}' detected in '{1}' at line '{2}'. Skipping entry.", stat.Id, fileName, engine.LineNumber);
                            continue;
                        }
                        Db[stat.Id] = stat;
                    }
                    else
                    {
                        Db.Add(stat.Id, stat);
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
        #endregion

        #region Database Commands
        public static StatBase Get(short statId)
        {
            StatBase stat;
            if (!Db.TryGetValue(statId, out stat))
            {
                return null;
            }
            return stat;
        }
        #endregion

        #region Properties
        public short Id { get; private set; }
        public short Strength { get; private set; }
        public short Vitality { get; private set; }
        public short Dexterity { get; private set; }
        public short Agility { get; private set; }
        public short Intelligence { get; private set; }
        public short Wisdom { get; private set; }
        public short Luck { get; private set; }
        #endregion
    }
}
