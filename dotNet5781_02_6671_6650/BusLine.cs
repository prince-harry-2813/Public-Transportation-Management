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
    /// Represent a single Bus line  Identify bus line key and first and last stop
    /// </summary>
   internal class BusLine : IComparable
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="lineKey">Bus Service Line Number</param>
        /// <param name="firstStop">Mendetory</param>
        /// <param name="lastStop"></param>
        /// <param name="area">Service Area</param>
        public BusLine(int lineKey, int firstStop, int lastStop, Area area = Area.General)
        {
            LineKey = lineKey;
            FirstStation = new BusStop(firstStop);
        }
        #region Poroperties Declaration

        public int LineKey { get; set; }

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
        #endregion

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
        public double CalculateDistance(BusStop current , BusStop other)
        {
            var busLine = SubLine(current, other);
            double distance = 0.0;
            for (int i = 0; i < busLine.Count() - 1; i++)
            {
                distance = busLine[i].calculateDistance(busLine[i + 1]);
            }
            return distance;
        }
        /// <summary>
        /// Calculate time of traveling from station to other(Not necessarily close)
        /// Calculate for Half of the Avarage driving range ph in orban area - Traffic lightes ect 
        /// and plus 1 min per stop 
        /// see https://tri.net.technion.ac.il/files/2016/08/%D7%A1%D7%A7%D7%A8-%D7%90%D7%A8%D7%A6%D7%99-%D7%A9%D7%9C-%D7%9E%D7%94%D7%99%D7%A8%D7%95%D7%99%D7%95%D7%AA-%D7%A0%D7%A1%D7%99%D7%A2%D7%94-%D7%91%D7%99%D7%A9%D7%A8%D7%90%D7%9C-%D7%A1%D7%A7%D7%A8-%D7%9E%D7%94%D7%99%D7%A8%D7%95%D7%99%D7%95%D7%AA-2013.pdf
        /// IN the day light 55 kmh im using for 28 kmh 
        /// </summary>
        /// <param name="other"></param>
        /// <returns>Riding time as TimeSpan</returns>
        public TimeSpan calculateRideTime(BusStop current , BusStop other)
        {
            TimeSpan time = TimeSpan.Zero;
            double rideDistance = 0.0 + CalculateDistance(current, other) / 1000;
            time = TimeSpan.FromMinutes((rideDistance / 28.0d) * 60);
            var busStops = SubLine(current, other);
            time += TimeSpan.FromMinutes(busStops.Count());

            return time;
        }
        /// <summary>
        /// Return new line bus that have some of the current route stops
        /// </summary>
        /// <param name="first"></param>
        /// <param name="last"></param>
        /// <returns>New List of stations</returns>
        public List<BusStop> SubLine(BusStop first, BusStop last)
        {
            List<BusStop> subBusLines = new List<BusStop>();
            var enumerator = LineStations.Where((l) => l == first).GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (enumerator.Current.Equals(last))
                {
                    subBusLines.Add(enumerator.Current);
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
            BusLine other = obj as BusLine;

            if (LineStations.Count() < other.LineStations.Count())
            {
                return -1;
            }

            if (LineStations.Count() == other.LineStations.Count())
            {
                return 0;
            }

            return 1;   
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
