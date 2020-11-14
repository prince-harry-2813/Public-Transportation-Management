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
    class BusLine : IComparable<BusLine>
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="lineKey">Bus Service Line Number</param>
        /// <param name="firstStop"></param>
        /// <param name="lastStop"></param>
        /// <param name="area">Service Area</param>
        public BusLine(int lineKey, int firstStop, int lastStop, Area area = Area.General)
        {
            LineKey = lineKey;
            FirstStation = new BusStop(firstStop);
            LastStaion = new BusStop(lastStop);
        }
        public BusLine(int lineKey)
        {
            LineKey = lineKey;
        }
        internal int LineKey { get; set; }
        #region Properties Declaration

        /// <summary>
        /// First station of bust route must open the station line
        /// </summary>
        public BusStop FirstStation { get => this.LineStations.First(); set => LineStations.Insert(0, value); }

        /// <summary>
        /// Last station of bus route must be the 
        /// </summary>
        public BusStop LastStaion { get => LineStations.Last(); set => LineStations.Add(value); }

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
        /// if index not passed insert the stop to the last station
        /// </summary>
        /// <param name="stop"></param>
        public void AddStop(BusStop stop, int? pos = null)
        {
            foreach (BusStop item in LineStations)
            {
                if (stop.StationCode != item.StationCode)
                    continue;
                throw new ArgumentException("this stop already in the route of this bus");
            }
            if (pos != null)
            {
                LineStations.Insert((int)pos, stop);
                if (pos > 0)
                    LineStations.ElementAt((int)pos).calculateDistance(LineStations.ElementAt((int)pos - 1));
                if (pos < LineStations.Count - 1)
                    LineStations.ElementAt((int)pos + 1).calculateDistance(LineStations.ElementAt((int)pos));
                return;
            }
            Console.WriteLine($"in this line there is {LineStations.Count} stations choose number within the range of 0 - {LineStations.Count}");
            int.TryParse(Console.ReadLine(), out int index);
            LineStations.Insert(index, stop);
            if (index > 0)
                LineStations.ElementAt(index).calculateDistance(LineStations.ElementAt(index - 1));
            if (index < LineStations.Count - 1)
                LineStations.ElementAt(index + 1).calculateDistance(LineStations.ElementAt(index));

        }
        /// <summary>
        /// remove stop from the line route
        /// </summary>
        /// <param name="stop"></param>
        public void removeStop(int stop)
        {
            if (!IsExist(stop))
                throw new ArgumentOutOfRangeException($"There is no station number {stop} in this line route");
            LineStations.Remove(LineStations.FirstOrDefault(station => station.StationCode == stop));
        }
        /// <summary>
        /// check if the stop in the route of this line
        /// </summary>
        /// <returns>false if station doesn't exist in list</returns>
        public bool IsExist(int stationCode)
        {
            return LineStations.Any(BusStop => BusStop.StationCode == stationCode);
        }
        /// <summary>
        /// calculate the distance between tow stations (Not necessarily close)
        /// </summary>
        /// <param name="current">Left stop to evaluate</param>
        /// <param name="other"> right stop to evaluate </param>
        /// <returns> left - right</returns>
        public double CalculateDistance(BusStop current, BusStop other)
        {
            BusLine bus = SubLine(current, other);
            double distance = 0.0;
            for (int i = 1; i < bus.LineStations.Count; i++)
            {
                distance += bus.LineStations.ElementAt(i).calculateDistance(bus.LineStations.ElementAt(i - 1));
            }
            return distance;
        }
        /// <summary>
        /// Calculate time of traveling from station to other(Not necessarily close)
        /// Calculate for Half of the Average driving range PH in urban area - Traffic lights etc. 
        /// and plus 1 min per stop 
        /// see https://tri.net.technion.ac.il/files/2016/08/%D7%A1%D7%A7%D7%A8-%D7%90%D7%A8%D7%A6%D7%99-%D7%A9%D7%9C-%D7%9E%D7%94%D7%99%D7%A8%D7%95%D7%99%D7%95%D7%AA-%D7%A0%D7%A1%D7%99%D7%A2%D7%94-%D7%91%D7%99%D7%A9%D7%A8%D7%90%D7%9C-%D7%A1%D7%A7%D7%A8-%D7%9E%D7%94%D7%99%D7%A8%D7%95%D7%99%D7%95%D7%AA-2013.pdf
        /// IN the day light 55 km/h I'm using for 28 km/h 
        /// </summary>
        /// <param name="other"></param>
        /// <returns>Riding time as TimeSpan</returns>
        public TimeSpan calculateRideTime(BusStop current , BusStop other)
        {
            TimeSpan time = TimeSpan.Zero;
            double rideDistance = 0.0 + CalculateDistance(current, other) / 1000;
            time = TimeSpan.FromMinutes((rideDistance / 28.0d) * 60);
            time += TimeSpan.FromMinutes(LineStations.Count());

            return time;
        }
        /// <summary>
        /// Return new line bus that have some of the current route stops
        /// </summary>
        /// <param name="first"></param>
        /// <param name="last"></param>
        /// <returns>New List of stations</returns>
        public BusLine SubLine(BusStop first, BusStop last)
        {
            BusLine subBusLine = new BusLine(this.LineKey);
            var enumerator = LineStations.Where((l) => l == first).GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (enumerator.Current.Equals(last))
                {
                    subBusLine.LineStations.Add(enumerator.Current);
                    break;
                }
                subBusLine.LineStations.Add(enumerator.Current);
            }

            return subBusLine;
        }


        public override string ToString()
        {
            string stops = "";
            foreach (var item in LineStations)
                stops += item.StationCode.ToString() + "\n";

            return $"Line number :{LineKey}, activity Area: {Area:g}, \n list of station numbers:\n {stops}";

        }
        /// <summary>
        /// implementation of comparable to compare time travel of buses from current stop to traveler destination 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(BusLine other)
        {
            return TimeSpan.Compare(this.calculateRideTime(FirstStation, LastStaion), other.calculateRideTime(other.FirstStation, other.LastStaion));
        }
    }
}
