using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    /// <summary>
    /// Stop station at line route
    /// </summary>
    public class LineStation
    {
        public int LineId { get; set; }
        public int StationId { get; set; }
        public int LineStationIndex { get; set; }
        public int PrevStation { get; set; }
        public int NextStation { get; set; }
        public bool isActive { get; set; }

    }
}
