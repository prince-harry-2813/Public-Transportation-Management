using System;

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
        public int KmOnLastTreatment { get; set; }
        public DateTime LastTreatmentDate { get; set; }
    }
}
