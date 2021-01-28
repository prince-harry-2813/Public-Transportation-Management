using System;

namespace BL.BO
{

    /// <summary>
    /// Bus vehicle
    /// </summary>
    public class Bus
    {
        /// <summary>
        /// Key property 
        /// </summary>
        public int LicenseNum { get; set; }

        /// <summary>
        /// Bus First Registration 
        /// </summary>
        public DateTime RegisDate { get; set; }

        /// <summary>
        /// Bus Speedometer Total KM (Can only increase )
        /// </summary>
        public int TotalKM { get; set; }

        /// <summary>
        /// Gas Tank Status (usually 1 liter per KM)
        /// </summary>
        public int FuelStatus { get; set; }

        /// <summary>
        /// Bus availability status 
        /// </summary>
        public BusStatusEnum Status { get; set; }

        /// <summary>
        /// Is Bus Active in the system 
        /// </summary>
        public bool isActive { get; set; }

        /// <summary>
        /// Bus Last Maintenance
        /// </summary>
        public DateTime LastTreatment { get; set; }

        /// <summary>
        ///  Km on last maintenance
        /// </summary>
        public uint LastTreatmentKm { get; set; }
    }
}
