using System;

namespace dotNet5781_02_6671_6650
{
    
    public abstract class BusStation
    {
        public Random Random = new Random();
        #region Properties Deceleration

        public int StationCode { get; private set; } = 0;

        /// <summary>
        /// Bus station key unique key up to 6 digits 
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// store bus Latitude location 
        /// </summary>
        public double Latitude { get; private set; }
        /// <summary>
        /// Store bus Longitude location
        /// </summary>
        public double Longitude { get; set; }
        #endregion

        public BusStation (int _code)
        {
            SetStationCode(_code);
            SetLatitude(Random.Next(31,33)+Random.NextDouble());
            SetLongitude(Random.Next(34, 35) + Random.NextDouble());
            Address = "";
        }

        public BusStation(int _code, double _lat, double _long, string _adress = "")
        {

            SetStationCode(_code);
            SetLatitude(_lat);
            SetLongitude(_long);
            Address = _adress;
                  
        }

        public override string ToString() => $"Bus Station Code: {StationCode}, {Latitude}°N {Longitude}°E ,{Address}\n";

        #region Setters
        /// <summary>
        /// Sets Bus Station Key and checks its validity
        /// </summary>
        /// <param name="busStationKey">1 - 6 digits , Not 0 </param>
        /// <returns>true if value was assigned </returns>
        private bool SetStationCode(int stationCode)
        {
            if (stationCode >= 1000000 || stationCode <= 0)
            {

                throw new ArgumentException("Bus Station key must be positive and can't include more then 6 digits");
               
            }
            StationCode = stationCode;
            return true;
        }

        /// <summary>
        /// Set Latitude value inside of Israel Land 
        /// The function takes the minimum or maximum value respectively in case latitude is out of range 
        /// </summary>
        public void SetLatitude(double latitude)
        {
            if (latitude < 31 || latitude > 33.3)
            {
                throw new ArgumentException("Bus Station key must be positive and can't include more then 6 digits");
            }
        }

        /// <summary>
        /// Set Longitude value inside of Israel Land 
        /// The function takes the minimum or maximum value respectively in case latitude is out of range 
        /// </summary>
        /// <param name="longitude">[34.3, 35.5] </param>
        public void SetLongitude(double longitude)
        {
            longitude = (longitude < 34.3) ? 34.3 : (longitude > 35.5) ? 35.5 : longitude;
            Longitude = longitude;
        }
        #endregion
    }
}
