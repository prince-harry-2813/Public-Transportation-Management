﻿using System;
namespace dotNet5781_02_6671_6650
{

    /// <summary>
    /// Bus Stop hold all details of given bus stop: location, address etc.
    /// </summary>
    public class BusStop : BusStation
    {
        #region Properties Deceleration

        public double Distance { get; set; } = 0;
        public TimeSpan ArrivingTime { get; set; } = TimeSpan.Zero;

        #endregion

        /// <summary>
        /// Ctor with random parameters
        /// </summary>
        /// <param name="_code">station code</param>
        public BusStop(int _code) : base(_code)
        {

        }

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
        /// This uses the Haversine formula to calculate the short distance between tow coordinates on sphere surface  
        /// </summary>
        /// <param name="other"> previous or other station </param>
        /// <returns>Short distance in meters </returns>
        public double CalculateDistance(BusStop other)
        {
            double earthRadius = 6371e3;
            double l1 = this.Latitude * (Math.PI / 180);
            double l2 = other.Latitude * (Math.PI / 180);
            double l1_2 = (other.Latitude - this.Latitude) * (Math.PI / 180);
            double lo_1 = (other.Longitude - this.Longitude) * (Math.PI / 180);
            double a = (Math.Sin(l1_2 / 2) * Math.Sin(l1_2 / 2)) +
                Math.Cos(l1) * Math.Cos(l2) *
               (Math.Sin(lo_1 / 2) * Math.Sin(lo_1 / 2));
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            Distance = (earthRadius * c);
            CalculateTime();
            return Distance;
        }

        /// <summary>
        /// calculate the time of ride to this station from previous 
        /// one KM per minute
        /// </summary>
        public TimeSpan CalculateTime()
        {
            ArrivingTime = TimeSpan.Zero;
            return ArrivingTime += TimeSpan.FromMinutes(1 + (this.Distance / 28.0 * 60) / 1000);
        }



        /// <summary>
        /// Override to string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $" Bus Station Code: {StationCode},  {Latitude}˚N, {Longitude}˚E {Address} {ArrivingTime.ToString(@"\ h\:mm")}";
        }




    }
}
