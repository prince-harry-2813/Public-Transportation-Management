using DO;
using System;
using System.Collections.Generic;

namespace DS
{
    /// <summary>
    /// this CLass hold data for all entities in program 
    /// </summary>
    public static class DataSource
    {

        #region Data Source Lists

        public static List<Bus> Buses;
        public static List<BusOnTrip> BusesOnTrips;
        public static List<Line> Lines;
        public static List<LineStation> LineStations;
        public static List<LineTrip> LineTrips;
        public static List<Station> Stations;
        public static List<Trip> Trips;
        public static List<User> Users;
        public static List<AdjacentStations> AdjacentStations;
        #endregion
        /// <summary>
        /// Static Ctor for Data object lists
        /// </summary>
        static DataSource()
        {
            InitialDS();
        }

        /// <summary>
        /// this method initial program with an initial information
        /// </summary>
        static void InitialDS()
        {
            Buses = new List<Bus> {
            new Bus
            {
                LicenseNum=12345678,
                RegisDate = new DateTime(2019,10,10),
                TotalKM =0,
                FuelStatus=1200,
                Status =BusStatusEnum.Ok,
                isActive =true
            }
            };
            BusesOnTrips = new List<BusOnTrip>();
            Lines = new List<Line>()
            {
                new Line()
                {
                    LastStation = 12345,
                    FirstStation = 3523,
                    isActive = true,
                    Area = Area.Center,
                    Code = 321,
                    Id = 43
                }
            };
            LineStations = new List<LineStation>();
            LineStations = new List<LineStation>();
            LineTrips = new List<LineTrip>();
            Trips = new List<Trip>();
            Users = new List<User>();
            AdjacentStations = new List<AdjacentStations>();
        }
    }

}
