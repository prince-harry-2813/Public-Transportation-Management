using System;

namespace BL.BO
{
    /// <summary>
    /// Details of line service time
    /// </summary>
    public class LineTrip
    {
        public int Id { get; set; }
        public int LineId { get; set; }
        public TimeSpan StartAt { get; set; }
        /// <summary>
        /// for bonus
        /// </summary>
        public TimeSpan Frequency { get; set; }
        public TimeSpan FinishAt { get; set; }
        public bool isActive { get; set; }

    }
}
