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
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Common.Utils
{
    public static class StringUtils
    {
        /// <summary>
        /// Reads a Tartarus Database CSV file
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="columnsCount"></param>
        /// <param name="readFunc"></param>
        /// <param name="allowReplace"></param>
        /// <param name="required"></param>
        /// <returns></returns>
        public static int ReadDatabase(string fileName, int columnsCount, Action<string, int, string[], string[], bool> readFunc, bool allowReplace, bool required)
        {
            // File exists check
            if (!File.Exists(fileName) && required)
            {
                ConsoleUtils.ShowFatalError("Could not find database file '{0}'.", fileName);
                return -1;
            }

            // Creates database pattern
            string pattern;
            StringBuilder patternBuilder = new StringBuilder();
            for(int i = 0; i < columnsCount; i++)
            {
                patternBuilder.Append("(.*?),");
            }
            pattern = patternBuilder.ToString().TrimEnd(',');
            pattern = pattern + "$";
            Regex regex = new Regex(pattern);

            // Load up file and regex
            int lineNum = 0;
            int count = 0;
            string[] columns = new string[columnsCount];
            foreach (string line in File.ReadLines(fileName))
            {
                lineNum++;
                Match match = regex.Match(line);
                if (match.Groups.Count != columnsCount + 1)
                {
                    ConsoleUtils.ShowError("Invalid entry in '{0}' at line '{1}'. Skipping line.", fileName, lineNum);
                }
                else
                {
                    if (lineNum == 1)
                    {
                        // First row, header
                        for (int i = 1; i < match.Groups.Count; i++)
                        {
                            columns[i - 1] = match.Groups[i].ToString();
                        }
                    }
                    else
                    {
                        // Entry row
                        string[] values = new string[columnsCount];
                        int i = 0;
                        try
                        {
                            for (i = 0; i < columnsCount; i++)
                            {
                                values[i] = match.Groups[i + 1].ToString();
                            }

                            readFunc(fileName, lineNum, columns, values, allowReplace);
                        }
                        catch (Exception)
                        {
                            ConsoleUtils.ShowError("Could not parse column '{0}' in '{1}' at line '{2}'. Skipping line.", columns[i], fileName, lineNum);
                            continue;
                        }

                        count++;
                    }
                }
            }

            ConsoleUtils.ShowInfo("'{0}' entries read from '{1}'.", count, fileName);
            return count;
        }
    }
}
