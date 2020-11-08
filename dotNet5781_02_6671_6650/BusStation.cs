using System;

namespace dotNet5781_02_6671_6650
{
    public class BusStation
    {
        #region Properties Decleration

        public int StationCode { get; private set; } = 0;

        /// <summary>
        /// Bus station key uniqe key up to 6 digits 
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

        public BusStation(int _code, double _lat, double _long, string _adress = "")
        {
            SetStationCode(_code);
            SetLatitude(_lat);
            SetLongitude(_long);
            Address = _adress;
        }

        public override string ToString() => $"Bus Station Code: {StationCode}, {Latitude}°N {Longitude}°E ,{Address}\n";

        #region Seters
        /// <summary>
        /// Stes Bus Station Key and chaks its validity
        /// </summary>
        /// <param name="busStationKey">1 - 6 digits , Not 0 </param>
        /// <returns>true if value was assinged </returns>
        private bool SetStationCode(int stationCode)
        {
            if (stationCode >= 1000000 || stationCode == 0)
            {
                Console.Write("Bus Station key cant include more then 6 digits");
                return false;
            }
            StationCode = stationCode;
            return true;
        }

        /// <summary>
        /// Set Latitude value inside of Isarel Land 
        /// The function takes the minimum or maximum value respectivly in case latitude is out of range 
        /// </summary>
        /// <param name="latitude"> [31,33.3] בתחום רוחב קו</param>
        public void SetLatitude(double latitude)
        {
            if (latitude < 31)
            {
                latitude = 31;
            }

            if (latitude > 33.3)
            {
                latitude = 33.3;
            }

            Latitude = latitude;
        }

        /// <summary>
        /// Set Longitude value inside of Isarel Land 
        /// The function takes the minimum or maximum value respectivly in case latitude is out of range 
        /// </summary>
        /// <param name="longitude">[34.3, 35.5] בתחום אורך קו</param>
        public void SetLongitude(double longitude)
        {
            longitude = (longitude < 34.3) ? 34.3 : (longitude > 35.5) ? 35.5 : longitude;
            Longitude = longitude;
        }
        #endregion
    }
}
