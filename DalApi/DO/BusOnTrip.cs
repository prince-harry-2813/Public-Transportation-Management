using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    /// <summary>
    /// Bus vehicle on line route
    /// </summary>
    public class BusOnTrip
    {
        public int Id { get; set; }
        public int LicenseNum { get; set; }
        public int LineId { get; set; }
        /// <summary>
        /// What time that trip supposed to depart
        /// </summary>
        public DateTime PlannedTakeOff { get; set; }
        /// <summary>
        /// Time span from planned until trip was begun (if in time => value = 0)  
        /// </summary>
        public TimeSpan ActualTakeOff { get; set; }
        public int PrevStation { get; set; }
        /// <summary>
        /// Time span from previous station
        /// </summary>
        public TimeSpan PrevSatationAt { get; set; }
        public TimeSpan NextStationAt { get; set; }
        public bool isActive { get; set; }

    }
}
