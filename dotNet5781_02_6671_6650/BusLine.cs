﻿using System;
using System.Collections.Generic;
using System.Linq;

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
    public class BusLine : IComparable<BusLine>
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
        /// <summary>
        /// Ctor with only line key argument, for initialize example bus line in main program.
        /// </summary>
        /// <param name="lineKey"></param>
        public BusLine(int lineKey)
        {
            LineKey = lineKey;
        }
        /// <summary>
        /// Ctor that accept Bus- stop argument for first and last station
        /// </summary>
        /// <param name="lineKey"></param>
        /// <param name="first"></param>
        /// <param name="last"></param>
        public BusLine(int lineKey, BusStop first, BusStop last)
        {
            LineKey = lineKey;
            FirstStation = new BusStop(first.StationCode, first.Latitude, first.Longitude, first.Address);
            LastStaion = new BusStop(last.StationCode, last.Latitude, last.Longitude, last.Address);
        }

        #region Properties Declaration

        private int linekey;
        public int LineKey
        {
            get => linekey;
            set
            {
                if (value < 1 && value > 999)
                    throw new ArgumentOutOfRangeException("Line code must be positive and less than four digits");
                linekey = value;
            }
        }

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
        /// After updating the stops list recalculate the distance between the line stops 
        /// </summary>
        public void UpdatingDistance()
        {
            LineStations.First().Distance = 0;
            for (int index = 1; index < LineStations.Count; index++)
            {
                LineStations.ElementAt(index).CalculateDistance(LineStations.ElementAt(index - 1));
            }
        }
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
                UpdatingDistance();
                return;
            }
            Console.WriteLine($"in this line there is {LineStations.Count} stations choose number within the range of 0 - {LineStations.Count}");
            int.TryParse(Console.ReadLine(), out int index);
            LineStations.Insert(index, stop);
            UpdatingDistance();
        }

        /// <summary>
        /// remove stop from the line route
        /// </summary>
        /// <param name="stop"></param>
        public void RemoveStop(int stop)
        {
            if (!IsExist(stop))
                throw new ArgumentOutOfRangeException($"There is no station number {stop} in this line route");
            LineStations.Remove(LineStations.FirstOrDefault(station => station.StationCode == stop));
            UpdatingDistance();
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
                distance += bus.LineStations.ElementAt(i).CalculateDistance(bus.LineStations.ElementAt(i - 1));
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
        public TimeSpan CalculateRideTime(BusStop current, BusStop other)
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
            return TimeSpan.Compare(this.CalculateRideTime(FirstStation, LastStaion), other.CalculateRideTime(other.FirstStation, other.LastStaion));
        }
    }
}
