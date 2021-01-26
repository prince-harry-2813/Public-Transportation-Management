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
        /// Also name of the line, 
        /// </summary>
        public int Code { get; set; }
        public List<LineStation> Stations{get;set;}
        public Area Area { get; set; }
        public int FirstStation { get; set; }
        public int LastStation { get; set; }
        public bool IsActive { get; set; }
    }
}
