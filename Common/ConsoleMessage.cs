using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
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
