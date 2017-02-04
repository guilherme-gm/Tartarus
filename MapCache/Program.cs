
using MapCache.DataClasses;
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapCache
{
    class Program
    {
        static void Main(string[] args)
        {
            Maps maps = new Maps();
            if (!maps.Load())
            {
                Console.WriteLine("Press any key to exit.");
                Console.ReadKey();
                return;
            }
            Console.WriteLine("");

            Cache cache = new Cache();
            int okCount = 0;
            int failCount = 0;
            foreach(MapEntry entry in maps.MapList)
            {
                MapData data = new MapData();
                if (data.Load(maps.FilesFolder, entry))
                {
                    Console.WriteLine("Map '{0}' added to cache.", entry.FileName);
                    cache.MapsToCache.Add(data);
                    ++okCount;
                }
                else
                {
                    ++failCount;
                }
            }
            Console.WriteLine("{0} Maps loaded. {1} failed.", (okCount), failCount);
            Console.WriteLine("");


            Console.WriteLine("Generating MapCache");
            if (cache.Save())
            {
                Console.WriteLine("MapCache successfully generated.");
            }
            else
            {
                Console.WriteLine("An error ocurred while generating the MapCache. Fix it and run again!");
            }
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}
