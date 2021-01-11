using System;

namespace BL.BO
{

    /// <summary>
    /// Bus vehicle
    /// </summary>
    public  class Bus
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
        /// Bus Spidometer Total KM (Can only increaced )
        /// </summary>
        public int TotalKM { get; set; }
        
        /// <summary>
        /// Gas Tank Status (usually 1 liter per KM)
        /// </summary>
        public int FuelStatus { get; set; }
        
        /// <summary>
        /// Bus avalibility status 
        /// </summary>
        public BusStatusEnum Status { get; set; }
        
        /// <summary>
        /// Is Bus Active in the system 
        /// </summary>
        public bool isActive { get; set; }

        /// <summary>
        /// Bus Last Maintainance
        /// </summary>
        public DateTime LastTreatment { get; set; }

        /// <summary>
        ///  Km on last maintainance
        /// </summary>
        public uint LastTreatmentKm { get; set; }
    }
}
