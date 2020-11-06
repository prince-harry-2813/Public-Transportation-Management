using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace dotNet5781_02_6671_6650
{
    public enum Area
    {
        General,
        south,
        Jerusalem,
        Center,
        North
    }
   internal class BusLine : IComparable
    {
      internal  int lineKey { get; set; }
        BusStop firstStation { get; set; }
        BusStop lastStaion { get; set; }
        Area area { get; set; } = Area.General;
        public List<BusStop> lineStations = new List<BusStop>();
        /// <summary>
        /// Add stop to route of the line 
        /// </summary>
        /// <param name="stop"></param>
        public void addStop(BusStop stop)
        {
        }
        /// <summary>
        /// remove stop from the line route
        /// </summary>
        /// <param name="stop"></param>
        public void removeStop(BusStop stop)
        {

        }
        /// <summary>
        /// check if the stop in the route of this line
        /// </summary>
        /// <returns></returns>
        public bool isExist()
        {
            return false;
        }
        /// <summary>
        /// calculate the distance between tow stations (Not necessarily close)
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public double calculateDistance(BusStop other)
        {
            return 0.0;
        }
        /// <summary>
        /// Calculate time of traveling from station to other(Not necessarily close)
        /// </summary>
        /// <param name="other"></param>
        public TimeSpan calculateRideTime(BusStop other)
        {
            TimeSpan time = TimeSpan.Zero;
            return time;
        }
        /// <summary>
        /// return new line bus that have some of the current route stops
        /// </summary>
        /// <param name="first"></param>
        /// <param name="last"></param>
        /// <returns></returns>
        public BusLine subLine(BusStop first, BusStop last)
        {
            return this;
        }
        /// <summary>
        /// implementation of comparable to compare time travel of buses from current stop to traveler destination 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int CompareTo(object obj)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// overriding of to string method
        /// </summary>
    
        public override string ToString()
        {
            string stops = "";
            foreach (var item in lineStations)
                stops += item.stationCode.ToString() + "\n";

            return $"Line number :{lineKey}, activity area: {area:g}, \n list of station numbers:\n {stops}";

        }


    }
}
