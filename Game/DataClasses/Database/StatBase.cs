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
            StringUtils.ReadDatabase("Database/StatDatabase.csv", 8, ReadEntry, false, true);
        }

        private static void ReadEntry(string fileName, int lineNum, string[] columns, string[] values, bool allowReplace)
        {
            // Read Entry
            StatBase stat = new StatBase();
            int j = 0;
            try
            {
                stat.Id = short.Parse(values[j++]);
                stat.Strength = short.Parse(values[j++]);
                stat.Vitality = short.Parse(values[j++]);
                stat.Dexterity = short.Parse(values[j++]);
                stat.Agility = short.Parse(values[j++]);
                stat.Intelligence = short.Parse(values[j++]);
                stat.Wisdom = short.Parse(values[j++]);
                stat.Luck = short.Parse(values[j++]);
            }
            catch (Exception)
            {
                ConsoleUtils.ShowError("Could not parse column '{0}' in '{1}' at line '{2}'. Skipping line.", columns[j - 1], fileName, lineNum);
                return;
            }

            // Inserts entry in Database
            if (Db.ContainsKey(stat.Id))
            {
                if (!allowReplace)
                {
                    ConsoleUtils.ShowError("Duplicated code detected in '{0}' at line '{1}'. Skipping entry.", fileName, lineNum);
                    return;
                }
                Db[stat.Id] = stat;
            }
            else
            {
                Db.Add(stat.Id, stat);
            }
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
