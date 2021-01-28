using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.BO
{
    public class LineTiming
    {
        /// <summary>
        /// Unique
        /// </summary>
        public int LineID { get; set; }

        public int LineNumber { get; set; }
        /// <summary>
        /// Time on first station 
        /// </summary>
        public TimeSpan StartedTime { get; set; }

        public Station LastStation { get; set; }

        /// <summary>
        /// Arriving to station time
        /// </summary>
        public TimeSpan ArrivingTime { get; set; }
    }
}
