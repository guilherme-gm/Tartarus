
using Game.DataClasses.Objects;
using System.Collections.Generic;
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
namespace Game.DataClasses.GameWorld
{
    #region Region Class
    public class Region
    {
        public const int RegionSize = 180;
        public const int MapSize = 16128; // TODO : Maybe this const should be moved to another file?
        
        public static Region[][][] Regions;

        #region Initializing Methods
        public static void Init()
        {
            // TODO : These values must be determined by maps
            int mapWidth = 13;
            int mapHeight = 10;
            int channels = 1;
            
            int regionsX = (mapWidth * MapSize) / RegionSize;
            int regionsY = (mapHeight * MapSize) / RegionSize;
            
            Regions = new Region[regionsX][][];
            for (uint i = 0; i < regionsX; i++)
            {
                Regions[i] = new Region[regionsY][];
                for (uint j = 0; j < regionsY; j++)
                {
                    Regions[i][j] = new Region[channels];
                    for (uint k = 0; k < channels; k++)
                    {
                        // TODO : We might be able to improve that by not initializing a
                        //          region for invalid places.
                        Regions[i][j][k] = new Region(i, j);
                    }
                }
            }
        }
        #endregion

        #region Helper Methods
        public static Region FromPosition(Position pos)
        {
            return FromPosition(pos.X, pos.Y, pos.Layer);
        }
        public static Region FromPosition(float x, float y, byte layer)
        {
            uint rx = (uint)(x / RegionSize);
            uint ry = (uint)(y / RegionSize);
            layer = 0; // NOTE : Change this whne channels are supported.

            // Ensure that given region exists.
            if (Regions.Length < rx)
                return null;
            if (Regions[rx].Length < ry)
                return null;
            if (Regions[rx][ry].Length < layer)
                return null;

            return Regions[rx][ry][layer];
        }
        #endregion

        public uint X { get; set; }
        public uint Y { get; set; }
        public byte Layer { get; set; }
        //public List<StaticObject> StaticObjects { get; set; }
        public List<Player> Players { get; set; }

        public Region(uint rx, uint ry)
        {
            this.X = rx;
            this.Y = ry;
            this.Players = new List<Player>();
        }
	}
    #endregion
}

