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
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapCache.DataClasses
{
    public class Maps
    {
        public const string MapsFile = "Settings/Maps.txt";

        public const int TileLength = 42;
        public List<MapEntry> MapList { get; set; }

        public string FilesFolder { get; set; }
        public bool Load()
        {
            this.MapList = new List<MapEntry>();

            // Verify if maps file exists
            if (!File.Exists(MapsFile))
            {
                Console.WriteLine("Could not find maps file ({0}). Aborting.", MapsFile);
                return false;
            }

            int lineCount = 0;
            int failCount = 0;
            int okCount = 0;
            try
            {
                // Open MapsFile
                using (StreamReader sr = new StreamReader(MapsFile))
                {
                    string line;
                    // Read MapFile line by line
                    while ((line = sr.ReadLine()) != null)
                    {
                        ++lineCount;
                        // Skip comments and empty lines
                        if (line.Equals("") || line.StartsWith("//"))
                            continue;
                        
                        // Folder config
                        if (line.StartsWith("Folder:"))
                        {
                            this.FilesFolder = line.Substring(7).Trim();
                            if (!(this.FilesFolder.EndsWith("/") || this.FilesFolder.EndsWith("\\")))
                                this.FilesFolder += '/';
                            continue;
                        }

                        // Split line in infos
                        string[] infos = line.Split(',');

                        // Skip invalid entries
                        if (infos.Length < 3)
                        {
                            Console.WriteLine("Line {0} is too short. Skipping it.", lineCount);
                            ++failCount;
                            continue;
                        }

                        // Create a MapEntry
                        MapEntry entry = new MapEntry();
                        entry.X = short.Parse(infos[0]);
                        entry.Y = short.Parse(infos[1]);
                        entry.FileName = infos[2];
                        this.MapList.Add(entry);

                        ++okCount;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Map cache has failed at line {0}: {1}", lineCount, e.Message);
                return false;
            }

            Console.WriteLine("'{0}' loaded, {1} entries read ({2} success, {3} failed).", MapsFile, (failCount + okCount), okCount, failCount);
            return true;
        }
    }
}
