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
    /// <summary>
    /// Class that keeps maps to cache
    /// </summary>
    public class Cache
    {
        public List<MapData> MapsToCache { get; set; }

        public Cache()
        {
            this.MapsToCache = new List<MapData>();
        }

        public bool Save()
        {
            using (BinaryWriter bw = new BinaryWriter(File.Create("Database/MapCache.dat")))
            {
                try
                {
                    // Writes the number of records
                    bw.Write(MapsToCache.Count);

                    // Foreach map
                    foreach (MapData map in MapsToCache)
                    {
                        // Write map name (max le = 24)
                        byte[] mapName = Encoding.UTF8.GetBytes(map.MapEntry.FileName);
                        if (mapName.Length > 24)
                            Buffer.BlockCopy(mapName, 0, mapName, 0, 24);
                        bw.Write(mapName);
                        bw.Write(new byte[24 - mapName.Length]);

                        // Finds the lowest priority (higher Priority value means lower priority)
                        int minPrio = 0;
                        for (int i = 0; i < map.LocalInfo.Length; ++i)
                        {
                            if (minPrio < map.LocalInfo[i].Priority)
                                minPrio = map.LocalInfo[i].Priority;
                        }

                        // Write number of locals
                        bw.Write(map.LocalInfo.Length);

                        // Get locations from the highest priority to the lowest one
                        for (int pr = 1; pr <= minPrio; ++pr)
                        {
                            // Run through locations list
                            for (int i = 0; i < map.LocalInfo.Length; ++i)
                            {
                                // Priority Test
                                if (map.LocalInfo[i].Priority == pr)
                                {
                                    // Retrieves local ID from LUA script
                                    string localId = map.LocalInfo[i].Script;
                                    if (localId.Contains('('))
                                    {
                                        localId = localId.Split('(')[1];
                                        localId = localId.Split(')')[0];
                                        localId = localId.Trim();
                                    }
                                    else
                                    {
                                        localId = "0";
                                    }

                                    // Write location ID
                                    bw.Write(int.Parse(localId));

                                    // Write local number of polygons
                                    bw.Write(map.LocalInfo[i].Polygons.Length);

                                    // Run through local polygons
                                    for (int j = 0; j < map.LocalInfo[i].Polygons.Length; ++j)
                                    {
                                        // Write the number of points of the polygon
                                        bw.Write(map.LocalInfo[i].Polygons[j].Points.Length);

                                        // Write the points of the polygon, converted to game units
                                        for (int k = 0; k < map.LocalInfo[i].Polygons[j].Points.Length; ++k)
                                        {
                                            bw.Write((map.LocalInfo[i].Polygons[j].Points[k].X * (Maps.TileLength / 8)));
                                            bw.Write((map.LocalInfo[i].Polygons[j].Points[k].Y * (Maps.TileLength / 8)));
                                        }
                                    }
                                }
                            }
                        }
                    }

                    // close the mapcache
                    bw.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Failed to create MapCache. Error: {0}", e.Message);
                    return false;
                }
            }
            return true;
        }
    }
}
