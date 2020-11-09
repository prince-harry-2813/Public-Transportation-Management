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

    /// <summary>
    /// rREpresent a single Bus line  Identify bus line key and first and last stop
    /// </summary>
   internal class BusLine : IComparable
    {
        public BusLine(int lineKey, int firstStop, int lastStop, Area area = Area.General)
        {
            LineKey = lineKey;
            FirstStation = new BusStop(firstStop);
        }
        internal  int LineKey { get; set; }

        /// <summary>
        /// First station of bust route must open the station line
        /// </summary>
        public BusStop FirstStation { get; set; }

        /// <summary>
        /// Last station of bus route must be the 
        /// </summary>
        public BusStop LastStaion { get; set; }
      
        /// <summary>
        /// Enumeration of services area
        /// </summary>
        public Area Area { get; set; } = Area.General;


        /// <summary>
        /// Bus stops line first and last one must be respectively with first/last stop properties
        /// </summary>
        public List<BusStop> LineStations = new List<BusStop>();
        /// <summary>
        /// Add stop to route of the line 
        /// </summary>
        /// <param name="stop"></param>
        public void AddStop(BusStop stop)
        {
           
            LineStations.Add(stop);
        }
        /// <summary>
        /// remove stop from the line route
        /// </summary>
        /// <param name="stop"></param>
        public void removeStop(BusStop stop)
        {
            LineStations.Remove(stop);
        }
        /// <summary>
        /// check if the stop in the route of this line
        /// </summary>
        /// <returns>false if station doesn't exist in list</returns>
        public bool IsExist(int stationCode)
        {
            return LineStations.Any((l) => l.StationCode == stationCode);
        }
        /// <summary>
        /// calculate the distance between tow stations (Not necessarily close)
        /// </summary>
        /// <param name="current">Left stop to evaluate</param>
        /// <param name="other"> right stop to evaluate </param>
        /// <returns> left - right</returns>
        public double calculateDistance(BusStop current , BusStop other)
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
        public List<BusStop> SubLine(BusStop first, BusStop last)
        {
            List<BusStop> subBusLines = new List<BusStop>();
            var enumerator = LineStations.Where((l) => l == first).GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (enumerator.Current == last)
                {
                    break;
                }
                subBusLines.Add ( enumerator.Current);
            }

            return subBusLines;
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
            foreach (var item in LineStations)
                stops += item.StationCode.ToString() + "\n";

            return $"Line number :{LineKey}, activity Area: {Area:g}, \n list of station numbers:\n {stops}";

        }
    }
}
