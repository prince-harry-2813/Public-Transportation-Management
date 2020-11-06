using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_02_6671_6650
{

   internal class BusStop : BusStation
    {
        double distance { get; set; }
        TimeSpan arrivingTime { get; set; }

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
            return distance;
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
    }
}
