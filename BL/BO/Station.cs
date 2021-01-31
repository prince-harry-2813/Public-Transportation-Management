using System.Collections.Generic;

namespace BL.BO
{
    /// <summary>
    /// Bus Station (can have several lines)
    /// </summary>
    public class Station
    {
        public int Code { get; set; }
        public string Name { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        /// <summary>
        /// Represent the lines that having a stop in this station, the required info from each line it his code and destination station.
        /// info should come from Line station entities
        /// </summary>
        public IEnumerable<Line> Lines { get; set; }
        
        public bool isActive { get; set; }
        public override string ToString()
        {
            return $"מספר הקו: {Code}";
        }


    }
}
