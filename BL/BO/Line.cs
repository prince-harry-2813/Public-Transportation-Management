namespace BL.BO
{
    /// <summary>
    /// Line service
    /// </summary>
    public class Line
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public Area Area { get; set; }
        public int FirstStation { get; set; }
        public int LastStation { get; set; }
        public bool IsActive { get; set; }

    }
}
