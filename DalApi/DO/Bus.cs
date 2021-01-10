using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{

    /// <summary>
    /// Bus vehicle
    /// </summary>
    public class Bus
    {
        public int LicenseNum { get; set; }
        public DateTime RegisDate { get; set; } 
        public int TotalKM { get; set; }
        public int FuelStatus { get; set; }
        public BusStatusEnum Status { get; set; }
        public bool isActive { get; set; } = true;

    }
}
