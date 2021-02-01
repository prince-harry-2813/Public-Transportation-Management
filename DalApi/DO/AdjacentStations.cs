using System;

namespace DO
{
    /// <summary>
    /// Tow Adjacent Stations in line route
    /// </summary>
    public class AdjacentStations
    {
        public int PairId { get; set; }
        public int Station1 { get; set; }// key 1
        public int Station2 { get; set; }// key 2
        public double Distance { get; set; }
        public TimeSpan Time { get; set; }
        public bool isActive { get; set; }

    }
}
