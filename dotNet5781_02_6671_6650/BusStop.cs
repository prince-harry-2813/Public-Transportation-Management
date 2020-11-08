using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_02_6671_6650
{

    /// <summary>
    /// Bus Stop Calculate all details of given bus stop location address ect'
    /// </summary>
    internal class BusStop : BusStation
    {
        #region Properties Decleration
        public double distance { get; set; }
        public TimeSpan arrivingTime { get; set; }
        #endregion

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="_code"> unique </param>
        /// <param name="_lat"></param>
        /// <param name="_long"></param>
        /// <param name="_adress"></param>
        public BusStop(int _code, double _lat, double _long, string _adress = "") : base(_code, _lat, _long, _adress)
        {

        }

        /// <summary>
        /// calculate the distance between previous station to current 
        /// </summary>
        /// <param name="other"> previous station </param>
        /// <returns></returns>
        public double calculateDistance(BusStop other)
        {
            return GetDistance(Longitude, Latitude, other.Longitude, other.Latitude);
        }

        /// <summary>
        /// calculate the time of ride to this station from previous 
        /// one KM per minute
        /// </summary>
        public TimeSpan calculateTime()
        {
            this.arrivingTime = TimeSpan.Zero;
            return arrivingTime += TimeSpan.FromMinutes(this.distance);
        }



        /// <summary>
        /// Override to string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $" Bus Station Code: {StationCode},  {Latitude}˚N, {Longitude}˚E {Address}";
        }

        /// <summary>
        /// calculate distance between to geo coordinates
        /// </summary>
        /// <param name="longitude"></param>
        /// <param name="latitude"></param>
        /// <param name="otherLongitude"></param>
        /// <param name="otherLatitude"></param>
        /// <returns>Distance as meters</returns>
        public double GetDistance(double longitude, double latitude, double otherLongitude, double otherLatitude)
        {
            var d1 = latitude * (Math.PI / 180.0);
            var num1 = longitude * (Math.PI / 180.0);
            var d2 = otherLatitude * (Math.PI / 180.0);
            var num2 = otherLongitude * (Math.PI / 180.0) - num1;
            var d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) + Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0);

            return 6376500.0 * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3)));
        }

        /// <summary>
        /// Calculate distance between two given bus stops
        /// </summary>
        /// <param name="other"></param>
        /// <returns>Left parmeter bus stop - right parameter bus stop in meters</returns>
        public double GetDistance(BusStop other)
        {
            double latitude = Latitude;
            double longitude = Longitude;
            double otherLatitude = other.Latitude;
            double otherLongitude = other.Longitude;

            return GetDistance(longitude, latitude, otherLongitude, otherLatitude);
        }
    }
}
