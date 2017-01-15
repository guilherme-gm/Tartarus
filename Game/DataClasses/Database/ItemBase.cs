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
using System.IO;
using System.Text.RegularExpressions;

namespace Game.DataClasses.Database
{
    public class ItemBase
    {
        public static Dictionary<int, ItemBase> Db;

        #region Database Loading
        public static void Load()
        {
            if (Db != null)
            {
                ConsoleUtils.ShowFatalError("Trying to load already loaded Item Database. Load Aborted.");
                return;
            }

            Db = new Dictionary<int, ItemBase>();
            StringUtils.ReadDatabase("Database/ItemDatabase.csv", 12, ReadEntry, false, true);
        }

        private static void ReadEntry(string fileName, int lineNum, string[] columns, string[] values, bool allowReplace)
        {
            // Read Entry
            ItemBase item = new ItemBase();
            int j = 0;
            try
            {
                item.Code = int.Parse(values[j++]);
                j++; // Dummy name
                item.Type = int.Parse(values[j++]);
                item.Group = int.Parse(values[j++]);
                item.Class = int.Parse(values[j++]);
                item.Grade = byte.Parse(values[j++]);
                item.Rank = int.Parse(values[j++]);
                item.Level = int.Parse(values[j++]);
                item.Enhance = int.Parse(values[j++]);
                item.SocketCount = int.Parse(values[j++]);
                item.Weight = float.Parse(values[j++]);
                item.WearType = int.Parse(values[j++]);
            }
            catch (Exception)
            {
                ConsoleUtils.ShowError("Could not parse column '{0}' in '{1}' at line '{2}'. Skipping line.", columns[j - 1], fileName, lineNum);
                return;
            }

            // Inserts entry in Database
            if (Db.ContainsKey(item.Code))
            {
                if (!allowReplace)
                {
                    ConsoleUtils.ShowError("Duplicated code detected in '{0}' at line '{1}'. Skipping entry.", fileName, lineNum);
                    return;
                }
                Db[item.Code] = item;
            }
            else
            {
                Db.Add(item.Code, item);
            }
        }
        #endregion

        #region Properties
        public int Code { get; set; }
        public int Type { get; set; }
        public int Group { get; set; }
        public int Class { get; set; }
        public byte Grade { get; set; }
        public int Rank { get; set; }
        public int Level { get; set; }
        public int Enhance { get; set; }
        public int SocketCount { get; set; }
        public float Weight { get; set; }
        public int WearType { get; set; }
        #endregion
    }
}
