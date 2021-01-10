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
        public bool isActive { get; set; }

    }
}
