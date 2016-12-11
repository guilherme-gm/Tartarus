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

namespace Common.Utils
{
    /// <summary>
    /// Container class for console messages
    /// </summary>
    public class ConsoleMessage
    {
        /// <summary>
        /// Text to be displayed
        /// </summary>
        public string[] TextBlocks { get; set; }
        /// <summary>
        /// Color of the background of the text to be displayed
        /// </summary>
        public ConsoleColor BackColor { get; set; }
        /// <summary>
        /// Color of the text to be displayed
        /// </summary>
        public ConsoleColor ForeColor { get; set; }
        /// <summary>
        /// Determines if new lines should be appended to the final message
        /// </summary>
        public bool InsertNewLine { get; set;  }
        /// <summary>
        /// Determines how many new lines should be appended to the final message
        /// </summary>
        public int NewLineCount
        {
            get { if (_newLineCount == 0) { return 1; } else { return _newLineCount; } }
            set { _newLineCount = value; }
        }

        private int _newLineCount;
    }
}
