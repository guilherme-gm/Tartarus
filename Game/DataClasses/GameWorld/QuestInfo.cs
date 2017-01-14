using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.DataClasses.GameWorld
{
    public class QuestInfo
    {
        public int Code { get; set; }
        public int StartID { get; set; }
        public int[] RandomValue { get; set; }
        public int[] Status { get; set; }
        public byte Progress { get; set; }
        public uint TimeLimit { get; set; }

        public QuestInfo()
        {
            this.RandomValue = new int[6];
            this.Status = new int[6];
        }
    }
}
