using System.Collections.Generic;

namespace BL.BO
{
    /// <summary>
    /// Line service
    /// </summary>
    public class Line
    {
        /// <summary>
        /// serial number for line (Key property)
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Line number - Also name of the line. 
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// List of Stations in Line by order 
        /// info should come from Line station entities
        /// When initialize must have First and last Stations
        /// </summary>
        public List<LineStation> Stations { get; set; }
        public Area Area { get; set; }
        public LineStation FirstStation { get; set; }
        public LineStation LastStation { get; set; }
        public bool IsActive { get; set; }
    }
}
