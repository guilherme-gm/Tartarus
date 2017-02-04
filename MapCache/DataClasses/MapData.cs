
using MapCache.DataClasses.MapParts;
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
    public class MapData
    {
        public MapEntry MapEntry { get; set; }
        public LocationInfo[] LocalInfo { get; set; }

        internal bool Load(string dir, MapEntry entry)
        {
            this.MapEntry = entry;

            string filePath = dir + entry.FileName + ".nfc";
            if (!File.Exists(filePath))
            {
                Console.WriteLine("Map File '{0}' not found. Skipping it.", entry.FileName);
                return false;
            }

            try
            {
                using (BinaryReader br = new BinaryReader(File.Open(filePath, FileMode.Open)))
                {
                    int recordCount = br.ReadInt32();
                    LocalInfo = new LocationInfo[recordCount];

                    for (int i = 0; i < recordCount; ++i)
                    {
                        LocationInfo info = new LocationInfo();
                        info.Priority = br.ReadInt32();
                        info.CenterX = br.ReadSingle();
                        info.CenterY = br.ReadSingle();
                        info.CenterZ = br.ReadSingle();
                        info.Radius = br.ReadSingle();

                        int len = br.ReadInt32();
                        info.Description = Encoding.UTF8.GetString(br.ReadBytes(len));
                        len = br.ReadInt32();
                        info.Script = Encoding.UTF8.GetString(br.ReadBytes(len));

                        int polyCount = br.ReadInt32();
                        info.Polygons = new Polygon[polyCount];

                        for (int j = 0; j < polyCount; ++j)
                        {
                            int pointCount = br.ReadInt32();
                            Polygon poly = new Polygon();
                            poly.Points = new Point[pointCount];

                            for (int k = 0; k < pointCount; ++k)
                            {
                                Point p = new Point();
                                p.X = br.ReadInt32();
                                p.Y = br.ReadInt32();

                                poly.Points[k] = p;
                            }

                            info.Polygons[j] = poly;
                        }
                        LocalInfo[i] = info;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to read map '{0}'. Skipping it. Error: {1}", entry.FileName, e.Message);
                return false;
            }

            return true;
        }
    }
}
