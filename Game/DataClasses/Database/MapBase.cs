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
using System.Text;

namespace Game.DataClasses.Database
{
    public class MapBase
    {
        public const int MapSize = 16128;

        public const string MapsFile = "Settings/Maps.txt";

        public int MaxX { get; set; }
        public int MaxY { get; set; }

        public List<Map> Maps { get; set; }
        public class Map
        {
            public string Name { get; set; }
            public int X { get; set; }
            public int Y { get; set; }
            public MapData Data { get; set; }
        }

        public class MapData
        {
            public class LocalInfo
            {
                public class Polygon
                {
                    public class Point
                    {
                        public int X { get; set; }
                        public int Y { get; set; }
                    }

                    public Point[] Points { get; set; }

                    public int Xmin { get; set; }
                    public int Xmax { get; set; }
                    public int Ymin { get; set; }
                    public int Ymax { get; set; }
                }
                public int LocalId { get; set; }
                public Polygon[] Polygons { get; set; }
            }
            
            public LocalInfo[] LocalInfos { get; set; }
            
        }

        private bool LoadMapsInfo()
        {
            this.Maps = new List<Map>();

            // Verify if maps file exists
            if (!File.Exists(MapsFile))
            {
                ConsoleUtils.ShowError("Could not find maps file ({0}). Aborting.", MapsFile);
                return false;
            }

            this.MaxX = 0;
            this.MaxY = 0;

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
                        // Folder config, skip
                        else if (line.StartsWith("Folder:"))
                            continue;

                        // Split line in infos
                        string[] infos = line.Split(',');

                        // Skip invalid entries
                        if (infos.Length < 3)
                        {
                            ConsoleUtils.ShowError("Line {0} is too short. Skipping it.", lineCount);
                            ++failCount;
                            continue;
                        }

                        // Create a MapEntry
                        Map entry = new Map();
                        entry.X = short.Parse(infos[0]);
                        entry.Y = short.Parse(infos[1]);
                        entry.Name = infos[2];
                        this.Maps.Add(entry);

                        // Updates maxx and maxy
                        if (this.MaxX < entry.X) this.MaxX = entry.X;
                        if (this.MaxY < entry.Y) this.MaxY = entry.Y;

                        ++okCount;
                    }
                }
            }
            catch (Exception e)
            {
                ConsoleUtils.ShowError("Map loading has failed at Maps.txt line {0}: {1}", lineCount, e.Message);
                return false;
            }

            ConsoleUtils.ShowInfo("'{0}' loaded, {1} entries read ({2} success, {3} failed).", MapsFile, (failCount + okCount), okCount, failCount);
            return true;
        }
        
        public bool Load()
        {
            if (!this.LoadMapsInfo()) return false;
            
            using (BinaryReader br = new BinaryReader(File.Open("Database/MapCache.dat", FileMode.Open)))
            {
                int recordCount = br.ReadInt32();
                // Reading maps
                for (int i = 0; i < recordCount; ++i)
                {
                    string name = Encoding.UTF8.GetString(br.ReadBytes(24));
                    name = name.TrimEnd('\0');
                    MapData data = new MapData();
                    int mapLocalCount = br.ReadInt32();

                    data.LocalInfos = new MapData.LocalInfo[mapLocalCount];

                    // Reading locals
                    for (int j = 0; j < mapLocalCount; ++j)
                    {
                        MapData.LocalInfo info = new MapData.LocalInfo();
                        info.LocalId = br.ReadInt32();

                        int polyCount = br.ReadInt32();
                        info.Polygons = new MapData.LocalInfo.Polygon[polyCount];

                        // Reading Polygons
                        for (int k = 0; k < polyCount; ++k)
                        {
                            int pointCount = br.ReadInt32();
                            MapData.LocalInfo.Polygon polygon = new MapData.LocalInfo.Polygon();
                            polygon.Points = new MapData.LocalInfo.Polygon.Point[pointCount];

                            // Reading Points
                            for (int l = 0; l < pointCount; ++l)
                            {
                                MapData.LocalInfo.Polygon.Point point = new MapData.LocalInfo.Polygon.Point();
                                point.X = br.ReadInt32();
                                point.Y = br.ReadInt32();

                                // Min and Max of polygons
                                if (polygon.Xmax < point.X) polygon.Xmax = point.X;
                                if (polygon.Xmin > point.X) polygon.Xmin = point.X;
                                if (polygon.Ymax < point.Y) polygon.Ymax = point.Y;
                                if (polygon.Ymin > point.Y) polygon.Ymin = point.Y;

                                // Save Point to Polygon
                                polygon.Points[l] = point;
                            }

                            // Save Polygon to Local
                            info.Polygons[k] = polygon;
                        }

                        // Save Local to MapData
                        data.LocalInfos[j] = info;
                    }

                    // Save map data to map
                    for (int j = 0; j < Maps.Count; ++j)
                    {
                        if (Maps[j].Name == name)
                        {
                           
                            Maps[j].Data = data;
                            break;
                        }
                    }
                }

                int fails = 0;
                List<Map> toRemove = new List<Map>();
                foreach(Map map in Maps)
                {
                    if (map.Data == null)
                    {
                        ConsoleUtils.ShowInfo("Removing map '{0}'. Invalid data.", map.Name);
                        toRemove.Add(map);
                        ++fails;
                    }
                }
                foreach(Map map in toRemove)
                {
                    Maps.Remove(map);
                }

                ConsoleUtils.ShowInfo("{0} maps loaded from MapCache.", (recordCount - fails));
            }

            return true;
        }
    }
}
