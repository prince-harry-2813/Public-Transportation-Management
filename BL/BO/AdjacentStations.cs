using System;

namespace BL.BO
{
    /// <summary>
    /// Tow Adjacent Stations in line route
    /// </summary>
    public class AdjacentStations
    {
        public Station Station1 { get; set; }// key 1
        public Station Station2 { get; set; }// key 2
        public double Distance { get; set; }
        public TimeSpan Time { get; set; }
        public bool isActive { get; set; }

    }
}
