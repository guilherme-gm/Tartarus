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
    public struct WorldLocationPolygon
    {
        public WorldLocationPoint[] Points;
        public int Xmin, Xmax, Ymin, Ymax;

        public WorldLocationPolygon(WorldLocationPoint[] points, int xmin, int xmax, int ymin, int ymax)
        {
            this.Points = points;
            this.Xmin = xmin;
            this.Xmax = xmax;
            this.Ymin = ymin;
            this.Ymax = ymax;
        }

        public bool Contains(WorldLocationPoint point)
        {
            int i, j, nvert = Points.Length;
            bool contains = false;

            // Test for the boundary box, if it's not inside it, no need for more calcs
            if (point.X < Xmin || point.X > Xmax || point.Y < Ymin || point.Y > Ymax)
                return false;

            // Test for vertices (raycast)
            for (i = 0, j = nvert - 1; i < nvert; j = i++)
            {
                if (
                    ((Points[i].Y >= point.Y) != (Points[j].Y >= point.Y)) &&
                    (point.X <= (Points[j].X - Points[i].X) * (point.Y - Points[i].Y) / (Points[j].Y - Points[i].Y) + Points[i].X)
                  )
                    contains = !contains;
            }

            return contains;
        }
    }
}
