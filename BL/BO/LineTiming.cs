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

        public int LineCode { get; set; }

        /// <summary>
        /// Time on first station Acctual Take of 
        /// </summary>
        public TimeSpan StartedTime { get; set; }

        public LineStation LastStation { get; set; }

        /// <summary>
        /// Arriving to station time
        /// </summary>
        public TimeSpan ArrivingTime { get; set; }
    }
}
