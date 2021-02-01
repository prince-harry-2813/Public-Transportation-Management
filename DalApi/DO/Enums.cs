namespace DO
{


    /// <summary>
    /// Current status of bus vehicle
    /// </summary>
    public enum BusStatusEnum
    {
        Ok = 1,
        In_Ride,
        In_Refueling,
        In_Maintainceing,
        Not_Available
    }

    /// <summary>
    /// Area of line service
    /// </summary>
    public enum Area
    {
        General,
        South,
        Jerusalem,
        Center,
        North
    }
}
