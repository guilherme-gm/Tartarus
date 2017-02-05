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
using Game.DataClasses.Database;
using Game.DataClasses.Objects;
using System;
using System.Collections.Generic;

namespace Game.DataClasses.GameWorld
{
    public class WorldLocation
    {
        public static WorldLocation[][][] Locations { get; private set; }

        #region Initializing Methods
        public static void Init(MapBase maps)
        {
            // Init WorldLocation array
            Locations = new WorldLocation[maps.MaxX + 1][][];
            for (int i = 0; i < maps.MaxX + 1; ++i)
            {
                Locations[i] = new WorldLocation[maps.MaxY + 1][];
            }

            // Add location data
            for (int i = 0; i < maps.Maps.Count; ++i)
            {
                MapBase.Map map = maps.Maps[i];
                Locations[map.X][map.Y] = new WorldLocation[map.Data.LocalInfos.Length];

                for (int j = 0; j < map.Data.LocalInfos.Length; ++j)
                {
                    MapBase.MapData.LocalInfo info = map.Data.LocalInfos[j];

                    // Base Info
                    WorldLocation local = new WorldLocation();
                    local.Location = LocationBase.Get(info.LocalId);
                    local.LocationArea = new WorldLocationPolygon[info.Polygons.Length];

                    // Polygon info
                    for (int k = 0; k < info.Polygons.Length; ++k)
                    {
                        local.LocationArea[k] = GetLocationPolygon(info.Polygons[k]);
                    }

                    // Saves to array
                    Locations[map.X][map.Y][j] = local;
                }
            }
        }

        private static WorldLocationPolygon GetLocationPolygon(MapBase.MapData.LocalInfo.Polygon polygon)
        {
            WorldLocationPoint[] points = new WorldLocationPoint[polygon.Points.Length];
            for (int i = 0; i < polygon.Points.Length; ++i)
            {
                MapBase.MapData.LocalInfo.Polygon.Point mapPoint = polygon.Points[i];

                WorldLocationPoint point;
                point.X = mapPoint.X;
                point.Y = mapPoint.Y;

                points[i] = point;
            }

            return new WorldLocationPolygon(points, polygon.Xmin, polygon.Xmax, polygon.Ymin, polygon.Ymax);
        }
        #endregion

        #region Helper Methods
        public static WorldLocation FromPosition(Position pos)
        {
            return FromPosition(pos.X, pos.Y);
        }
        public static WorldLocation FromPosition(float x, float y)
        {
            uint rx = (uint)(x / MapBase.MapSize);
            uint ry = (uint)(y / MapBase.MapSize);

            // Ensure that given location exists.
            if (Locations.Length < rx)
                return null;
            if (Locations[rx] == null || Locations[rx].Length < ry)
                return null;
            if (Locations[rx][ry] == null || Locations[rx][ry].Length == 0)
                return null;

            // Get a better point inside the area
            WorldLocationPoint pos;
            pos.X = (RoundPoint(x) % MapBase.MapSize);
            pos.Y = (RoundPoint(y) % MapBase.MapSize);
            
            // Find the location
            WorldLocation[] mapLocals = Locations[rx][ry];
            WorldLocation local = null;
            int i = 0;
            // Search in each map locals, while not found
            while (local == null && i < mapLocals.Length)
            {
                // In each local, search in each polygon, whily not found
                int j = 0;
                while (local == null && j < mapLocals[i].LocationArea.Length)
                {
                    if (mapLocals[i].LocationArea[j].Contains(pos))
                    {
                        // Local found
                        local = mapLocals[i];
                    }
                    ++j;
                }
                ++i;
            }

            return local;
        }

        private static int RoundPoint(float value)
        {
            float decimalPart = (float)(value - Math.Truncate(value));
            if (decimalPart < 0.5)
                return (int)Math.Ceiling(value);
            else
                return (int)Math.Floor(value);
        }
        #endregion

        #region Properties
        public LocationBase Location { get; set; }
        public WorldLocationPolygon[] LocationArea { get; set; }
        public int LastChangedTime { get; set; }
        public ushort CurrenWeather { get; set; }
        public List<Player> Players { get; set; }
        #endregion

        public WorldLocation()
        {
            this.Players = new List<Player>();
        }
    }
}
