using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    public enum StatusEnum
    {
        Ok = 1,
        In_Ride,
        In_Refuling,
        In_Maintainceing,
        Not_Available
    }
    class Bus
    {
        public int LicenseNum { get; set; }
        public DateTime RegisDate { get; set; }
        public int TotalKM { get; set; }
        public int FuelStatus { get; set; }
        public StatusEnum Status { get; set; }

    }
}
