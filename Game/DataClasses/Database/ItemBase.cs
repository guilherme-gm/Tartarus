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
            int i = ParseFile("Database/ItemDatabase.csv", false);
            if (i >= 0)
                ConsoleUtils.ShowInfo("'{0}' entries read from '{1}'.", i, "Database/ItemDatabase.csv");
        }

        private static int ParseFile(string fileName, bool allowReplace)
        {
            // File exists check
            if (!File.Exists(fileName))
            {
                ConsoleUtils.ShowFatalError("Could not find Item Database file '{0}'.", fileName);
                return -1;
            }

            // Loads up the file and pattern
            string pattern = "(.*?),(.*?),(.*?),(.*?),(.*?),(.*?),(.*?),(.*?),(.*?),(.*?),(.*?),(.*?)$";
            string[] lines = File.ReadAllLines(fileName);
            Regex regex = new Regex(pattern);

            // Load column names
            Match match = regex.Match(lines[0]);
            string[] columns = new string[match.Groups.Count - 1];
            for (int i = 1; i < match.Groups.Count; i++)
            {
                columns[i - 1] = match.Groups[i].ToString();
            }

            // Run through entries
            int count = 0;
            for (int i = 1; i < lines.Length; i++)
            {
                int j = 1;
                match = regex.Match(lines[i]);

                // Parse entry
                ItemBase item = new ItemBase();
                try
                {
                    item.Code = int.Parse(match.Groups[j++].ToString());
                    j++; // Dummy name
                    item.Type = int.Parse(match.Groups[j++].ToString());
                    item.Group = int.Parse(match.Groups[j++].ToString());
                    item.Class = int.Parse(match.Groups[j++].ToString());
                    item.Grade = byte.Parse(match.Groups[j++].ToString());
                    item.Rank = int.Parse(match.Groups[j++].ToString());
                    item.Level = int.Parse(match.Groups[j++].ToString());
                    item.Enhance = int.Parse(match.Groups[j++].ToString());
                    item.SocketCount = int.Parse(match.Groups[j++].ToString());
                    item.Weight = float.Parse(match.Groups[j++].ToString());
                    item.WearType = int.Parse(match.Groups[j++].ToString());
                }
                catch (Exception)
                {
                    ConsoleUtils.ShowError("Could not parse column '{0}' in '{1}' at line '{2}'. Skipping line.", columns[j-2], fileName, i);
                    continue;
                }

                // Inserts entry in Database
                if (Db.ContainsKey(item.Code))
                {
                    if (!allowReplace)
                    {
                        ConsoleUtils.ShowError("Duplicated code detected in '{0}' at line '{1}'. Skipping entry.", fileName, i);
                        continue;
                    }
                    Db[item.Code] = item;
                }
                else
                {
                    Db.Add(item.Code, item);
                }
                count++;
            }

            return count;
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
