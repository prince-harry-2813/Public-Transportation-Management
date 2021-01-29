﻿using BL.BLApi;
using BL.BO;
using DalApi;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Threading;


namespace BL
{
    internal class BLImp : IBL
    {

        private static IDAL iDal = DalApi.DalFactory.GetIDAL();

        #region IBL Bus Implementation

        /// <summary>
        /// Add New bus and sign unsigned properties appropriately 
        /// </summary>
        /// <param name="bus"></param>
        public void AddBus(Bus bus)
        {
            if (bus == null)
                throw new ArgumentNullException("Bus to Add is Null");

            if (bus.LicenseNum == 0 || bus.LicenseNum == null)
                throw new BadBusIdException("License number not initialized ", null);

            bus.FuelStatus = (bus.FuelStatus != null) ? bus.FuelStatus : 1200;
            bus.isActive = true;
            bus.LastTreatmentDate = (bus.LastTreatmentDate != DateTime.MinValue) ? bus.LastTreatmentDate : DateTime.Now;
            bus.TotalKM = (bus.TotalKM != null) ? bus.TotalKM : 0;
            bus.KmOnLastTreatment = (bus.KmOnLastTreatment != null) ? bus.KmOnLastTreatment : 0;
            bus.Status = (bus.FuelStatus != 0 && DateTime.Now.Subtract(bus.LastTreatmentDate).Days < 365 &&
                          bus.TotalKM - (int)bus.KmOnLastTreatment < 20000 && bus.isActive)
                ? BusStatusEnum.Ok
                : BusStatusEnum.Not_Available;



            try // checks if there is any bus return from DS by the license number 
            {

                DO.Bus busToAdd = new DO.Bus();
                bus.CopyPropertiesTo(busToAdd);
                iDal.AddBus(busToAdd);
            }
            catch (Exception)
            {
                throw new BadBusIdException("Bus With the same license number is already exist", null);
            }

        }

        /// <summary>
        /// Copy bus to DO property and send it do DAL to mark as not active
        /// </summary>
        /// <param name="bus"></param>
        public void DeleteBus(Bus bus)
        {
            if (bus == null)
            {
                throw new NullReferenceException("Bus to delete is Null");
            }

            DO.Bus busToDelete = new DO.Bus();
            bus.CopyPropertiesTo(busToDelete);
            iDal.DeleteBus(busToDelete.LicenseNum);
        }

        public IEnumerable<Bus> GetAllBuses()
        {
            // TODO :  check if this solution is good enough
            foreach (var VARIABLE in iDal.GetAllBuses())
            {
                //if (VARIABLE.isActive)  //deleted because DAL does this check    // Ignore deleted bus  

                yield return (Bus)VARIABLE.CopyPropertiesToNew(typeof(BO.Bus));

            }

            #region Seconed Solution

            //List<Bus> busesToreturn = new List<Bus>();
            //IEnumerable<DO.Bus> busesToCopy = iDal.GetAllBuses();
            //foreach (var bus in busesToCopy)
            //{
            //    busesToreturn.Add((Bus) bus.CopyPropertiesToNew(typeof(BO.Bus)));
            //}

            //return busesToreturn;

            #endregion
        }

        public Bus GetBus(int licenseNum)
        {
            if (licenseNum == null || licenseNum == 0)
                throw new NullReferenceException("license number is null or not initialized");

            if (licenseNum < 0)
                throw new BadBusIdException("Bus license number can't be negative",
                    new ArgumentException("Bus license number can't be negative"));

            if (licenseNum <= 999999 || licenseNum >= 100000000)
                throw new BadBusIdException("Bus license number is too large or too small",
                    new ArgumentException("Bus license number is too large or too small"));

            var busToCopy = iDal.GetBus(licenseNum);
            return (Bus)busToCopy.CopyPropertiesToNew(typeof(Bus));
        }

        public IEnumerable<Bus> GetBusBy(Predicate<Bus> predicate)
        {
            return null;
        }

        public void UpdateBus(Bus bus)
        {
            if (bus == null)
                throw new NullReferenceException("Bus is Null ");

            var busToUpdate = new DO.Bus();
            bus.CopyPropertiesTo(busToUpdate);
            iDal.UpdateBus(busToUpdate);
        }

        #endregion

        #region Line Implementation

        /// <summary>
        /// Add new line and add basic information
        /// </summary>
        /// <param name="line">
        /// </param>
        public void AddLine(Line line)
        {
            if (line == null)
                throw new NullReferenceException("Line to add is Null please try again");

            if (line.FirstStation == null || line.FirstStation.Station.Code == 0)
                throw new BadBusStopIDException("First Station Id not added or not exist ", new ArgumentException());

            if (line.LastStation == null || line.LastStation.Station.Code == 0)
                throw new BadBusStopIDException("First Station Id not added or not exist ", new ArgumentException());

            var station = iDal.GetStation(line.FirstStation.Station.Code);
            if (station == null)
                throw new BadBusStopIDException("First Bus stop not exist in the system", null);

            var station2 = iDal.GetStation(line.LastStation.Station.Code);
            if (station2 == null)
                throw new BadBusStopIDException("Last Bus stop not exist in the system", null);

            try
            {
                iDal.AddLineStation(new DO.LineStation()
                {
                    LineId = line.Id,
                    LineStationIndex = 0,
                    isActive = true,
                    StationId = line.FirstStation.Station.Code,
                    NextStation = line.LastStation.Station.Code
                });
                iDal.AddLineStation(new DO.LineStation()
                {
                    LineId = line.Id,
                    LineStationIndex = 1,
                    isActive = true,
                    StationId = line.LastStation.Station.Code,
                    PrevStation = line.FirstStation.Station.Code
                });
                var adjSta = iDal.GetAllAdjacentStationsBy(a => a.Station1 == line.FirstStation.Station.Code && a.Station2 == line.FirstStation.Station.Code);
                if (adjSta.Count() == 0)
                {
                    iDal.AddAdjacentStations(new DO.AdjacentStations()
                    {
                        isActive = true,
                        Station1 = line.FirstStation.Station.Code,
                        Station2 = line.LastStation.Station.Code,
                        Distance = CalculateDistance(line.FirstStation.Station, line.LastStation.Station),
                        PairId = iDal.GetAllAdjacentStationsBy(l => l.isActive||!l.isActive).Count()+1,
                        Time = CalculateTime(CalculateDistance(line.FirstStation.Station, line.LastStation.Station))

                    });

                }

                iDal.AddLine(new DO.Line()
                {
                    LastStation = line.LastStation.Station.Code,
                    Area = (DO.Area)line.Area,
                    Code = line.Code,
                    FirstStation = line.FirstStation.Station.Code,
                    Id = line.Id,
                    isActive = true

                });
            }
            catch (Exception e)
            {
                throw new ArgumentException("Check inner exception", e);
            }
        }

        public void UpdateLine(Line line)
        {
            if (line == null)
                throw new NullReferenceException("Line is Null ");

            var DOCollLinesStations = (from l in iDal.GetAllLinesStation()//to update from Updated Line Stations
                                       where l.LineId == line.Id
                                       select l);
            foreach (var item in line.Stations)
            {
                var DOLineStation = new DO.LineStation()
                {
                    LineId = line.Id,
                    LineStationIndex = item.LineStationIndex,
                    isActive = true,
                    StationId = item.Station.Code,
                    NextStation = (item.LineStationIndex < line.Stations.Count() - 1) ?
                        line.Stations.ElementAt(item.LineStationIndex + 1).Station.Code : 0,
                    PrevStation = (item.LineStationIndex > 0) ?
                        line.Stations.ElementAt(item.LineStationIndex - 1).Station.Code : 0,
                };
                try
                {
                    if (DOCollLinesStations.Any(l => l.StationId == item.Station.Code))//if Line station in Database ? update : add 
                        iDal.UpdateLineStation(DOLineStation);
                    else
                        iDal.AddLineStation(DOLineStation);
                }
                catch (Exception e)
                {
                    throw new BadBusStopIDException("Couldn't update or add, check details", e);
                }
            }
            DOCollLinesStations = iDal.GetAllLinesStationBy(l =>
                l.LineId == line.Id && !line.Stations.Any(s => s.Station.Code == l.StationId)); //to remove Unnecessary Line Station from data base
            foreach (var item in DOCollLinesStations) // line Stations that is in database but no in the updated line 
            {
                try
                {
                    iDal.DeleteLineStation(line.Id, item.StationId);
                }
                catch (Exception e)
                {
                    throw new BadBusStopIDException("Couldn't delete line station check details", e);
                }
            }

            var lineToUpdate = new DO.Line()
            {
                LastStation = line.LastStation.Station.Code,
                Area = (DO.Area)line.Area,
                Code = line.Code,
                FirstStation = line.FirstStation.Station.Code,
                Id = line.Id,
                isActive = true
            };
            try
            {
                iDal.UpdateLine(lineToUpdate);
            }
            catch (Exception e)
            {
                throw new ArgumentException("Couldn't update line, Check inner Exception details", e);
            }
        }

        public void DeleteLine(Line line)
        {
            if (line == null)
            {
                throw new NullReferenceException("Line to delete is Null");
            }

            DO.Line lineToDelete = new DO.Line();
            line.CopyPropertiesTo(lineToDelete);
            iDal.DeleteLine(lineToDelete.Id);
        }

        public Line GetLine(int lineId)
        {
            if (lineId == null || lineId == 0)
                throw new NullReferenceException("Line id is null or not initialized");

            if (lineId < 0)
                throw new BadBusIdException("Line id can't be negative",
                    new ArgumentException("Line id can't be negative"));

            var lineToCopy = iDal.GetLine(lineId);
            return (Line)lineToCopy.CopyPropertiesToNew(typeof(Line));
        }

        public IEnumerable<Line> GetAllLines()
        {
            foreach (var VARIABLE in iDal.GetAllLines())
            {
                var line = new Line()
                {
                    Id = VARIABLE.Id,
                    Code = VARIABLE.Code,
                    Area = (Area)VARIABLE.Area,
                    IsActive = VARIABLE.isActive,
                    LastStation = GetLineStation(VARIABLE.Id, VARIABLE.LastStation),
                    FirstStation = GetLineStation(VARIABLE.Id, VARIABLE.FirstStation),

                    Stations = from LS in GetAllLinesStationBy(l => l.isActive && l.LineId == VARIABLE.Id)
                               orderby LS.LineStationIndex
                               select LS,

                };
                yield return line;
            }
        }

        public IEnumerable<Line> GetLinesBy(Predicate<BO.Line> predicate)
        {
            foreach (var item in iDal.GetAllLinesBy(l => l.isActive || !l.isActive))
            {

                BO.Line line = (Line)item.CopyPropertiesToNew(typeof(Line));
                if (predicate(line))
                    yield return line;

            }
        }


        #endregion

        #region Bus Stop Implementation
        public void AddStation(Station station)
        {
            station.isActive = true;

            if (station.Name.Length == 0)
            {
                station.Name = "Example " + station.Code.ToString();
            }
            if (station.Code == 0)
            {
                throw new BadBusStopIDException("bus stop number can't be 0", new ArgumentException());
            }
            if (station.Longitude < 34.3 || station.Longitude > 35.5)
            {
                station.Longitude = double.Parse((new Random(DateTime.Now.Millisecond).NextDouble() * 1.2 + 34.3).ToString().Substring(0, 8));
            }
            if (station.Latitude <= 31 || station.Latitude >= 33.3)
            {
                station.Latitude = double.Parse((new Random(DateTime.Now.Millisecond).NextDouble() * 2.3 + 31).ToString().Substring(0, 8));
            }
            try
            {
                iDal.AddStation((DO.Station)station.CopyPropertiesToNew(typeof(DO.Station)));
            }
            catch (Exception e)
            {

                throw new ArgumentException("check details", e);
            }
            Console.WriteLine("wwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwww");
        }

        public void UpdateStation(Station station)
        {
            DO.Station DOstation = new DO.Station();
            station.CopyPropertiesTo(DOstation);
            iDal.UpdateStation(DOstation);
        }

        public void DeleteStation(Station station)
        {
            DO.Station DOstation = new DO.Station();
            station.CopyPropertiesTo(DOstation);
            iDal.DeleteStation(DOstation.Code);
        }

        public Station GetStation(int lineId)
        {
            Station station = new Station();
            iDal.GetStation(lineId).CopyPropertiesTo(station);
            return station;
        }

        public IEnumerable<Station> GetAllStations()
        {
            foreach (var VARIABLE in iDal.GetAllStation())
            {
                yield return (Station)VARIABLE.CopyPropertiesToNew(typeof(Station));
            }
        }

        public IEnumerable<Station> GetStationBy(Predicate<Station> predicate)
        {
            throw new NotImplementedException();
        }



        #endregion

        #region User Simulation

        event Action<TimeSpan> clockObserver = null;
        private DispatcherTimer simulationTimer = new DispatcherTimer();
        internal volatile bool Cancel;

        /// <summary>
        /// Start simulator stop watch and update it according 
        /// </summary>
        /// <param name="startTime">TIME TO START  </param>
        /// <param name="Rate"> Hz per minute</param>
        /// <param name="updateTime">Action</param>
        public void StartSimulator(TimeSpan startTime, int rate, Action<TimeSpan> updateTime)
        {
            Cancel = false;
            clockObserver = updateTime;
            TimeSpan simulatorTime = new TimeSpan(TimeSpan.FromSeconds(startTime.TotalSeconds).Days,
                TimeSpan.FromSeconds(startTime.TotalSeconds).Hours,
                TimeSpan.FromSeconds(startTime.TotalSeconds).Minutes
                , TimeSpan.FromSeconds(startTime.TotalSeconds).Seconds
                , TimeSpan.FromSeconds(startTime.TotalSeconds).Milliseconds);

            simulationTimer.Interval = new TimeSpan(0, 0, 0, 0, (1000 / (rate * (10 / 6))));
            rideOperation.interval = simulationTimer.Interval.Milliseconds;
            simulationTimer.Tick += (sender, args) =>
            {
                if (Cancel)
                {
                    clockObserver = null;
                    simulationTimer.Stop();
                    return;
                }

                simulatorTime += TimeSpan.FromSeconds(1);
                updateTime.Invoke(simulatorTime);
                rideOperation.UpdateSimualtionTime(simulatorTime);
                Debug.Print(simulatorTime.ToString());
            };
            simulationTimer.Start();

        }





        #endregion

        #region Line Station

        public LineStation GetLineStation(int lineId, int stationCode)
        {
            LineStation station = new LineStation();
            iDal.GetLineStation(lineId, stationCode).CopyPropertiesTo(station);
            station.Station = (BO.Station)iDal.GetStation(stationCode).CopyPropertiesToNew(typeof(BO.Station));
            return station;
        }

        public IEnumerable<LineStation> GetAllLinesStation()
        {
            foreach (var item in iDal.GetAllLinesStation())
            {
                yield return (LineStation)item.CopyPropertiesToNew(typeof(BO.LineStation));
            }
        }

        public IEnumerable<LineStation> GetAllLinesStationBy(Predicate<BO.LineStation> predicate)
        {
            foreach (var item in iDal.GetAllLinesStationBy(l => l.isActive || !l.isActive))
            {
                LineStation lineStation = new LineStation();
                item.CopyPropertiesTo(lineStation);
                if (predicate(lineStation))
                {
                    yield return lineStation;
                }
            }
        }

        public void AddLineStation(LineStation lineStation)
        {
            iDal.AddLineStation((DO.LineStation)lineStation.CopyPropertiesToNew(typeof(DO.LineStation)));
        }

        public void UpdateLineStation(LineStation lineStation)
        {
            iDal.UpdateLineStation((DO.LineStation)lineStation.CopyPropertiesToNew(typeof(DO.LineStation)));
        }

        public void UpdateLineStation(int lineId, int stationCode, Action<LineStation> update)
        {
            var a = (DO.LineStation)iDal.GetAllLinesStationBy(station => station.LineId == lineId && station.StationId == stationCode).FirstOrDefault();
            if (!(a is null))
            {
                LineStation boLineStation = (LineStation)a.CopyPropertiesToNew(typeof(LineStation));
                update(boLineStation);
                boLineStation.CopyPropertiesTo(a);
                iDal.UpdateLineStation(a);
            }
        }

        #endregion

        #region Ride Operation

        public void StopSimulator()
        {
            Cancel = true;
        }

        private RideOperation rideOperation = new RideOperation((IDAL)iDal);

        public void SetStationPanel(int station, Action<LineTiming> updateBus)
        {
            if (station == -1)
            {
                //TODO: Shut down
            }

            rideOperation.StartSimulation();
        }
        class RideOperation
        {
            public int interval;
            private event EventHandler<LineTiming> updatebusPrivate;
            private int staionID;
            List<LineTrip> linesTrips = new List<LineTrip>();
            private IDAL idal;
            private IBL bl;
            private BackgroundWorker getLineStaionworker = new BackgroundWorker();
            private TimeSpan simulationTime;

            public RideOperation(IDAL dal)
            {
                idal = dal;
                this.bl = bl;

                if (getLineStaionworker.IsBusy)
                {
                    getLineStaionworker.CancelAsync();
                }
                getLineStaionworker.WorkerReportsProgress = true;
                getLineStaionworker.WorkerReportsProgress = true;
                getLineStaionworker.DoWork += (sender, args) =>
                {
                    int i = 0;
                    foreach (var item in idal.GetAllLinesTripBy(trip => trip.isActive))
                    {
                        if (getLineStaionworker.CancellationPending)
                            break;

                        getLineStaionworker.ReportProgress(i, item.CopyPropertiesToNew(typeof(LineTrip)));
                        i++;
                        if (i == 99)
                            i = 90;
                    }
                };
                getLineStaionworker.ProgressChanged += (sender, args) =>
                {
                    linesTrips.Add((LineTrip)args.UserState);
                };

                getLineStaionworker.RunWorkerCompleted += (sender, args) =>
                {
                    linesTrips.Sort((trip, lineTrip) => trip.StartAt.CompareTo(lineTrip.StartAt));
                };

                getLineStaionworker.RunWorkerAsync();
            }

            public void StartSimulation()
            {
                foreach (var item in linesTrips)
                {
                    //   for
                    Task.Factory.StartNew(() =>
                    {
                        LineTiming lineTiming = new LineTiming()
                        {
                            LastStation = (Station)idal.GetStation(idal.GetLine(item.LineId).LastStation)
                                .CopyPropertiesToNew(typeof(Station))
                            //,ArrivingTime = 
                        };
                    });
                }

            }

            public void UpdateSimualtionTime(TimeSpan time)
            {
                simulationTime = time;
            }

        }

        #endregion

        #region Utilities

        /// <summary>
        /// calculate the distance between previous station to current
        /// This uses the Haversine formula to calculate the short distance between tow coordinates on sphere surface  
        /// </summary>
        /// <param name="other"> previous or other station </param>
        /// <returns>Short distance in meters </returns>
        public double CalculateDistance(Station st1, Station st2)
        {
            double earthRadius = 6371e3;
            double l1 = st1.Latitude * (Math.PI / 180);
            double l2 = st2.Latitude * (Math.PI / 180);
            double l1_2 = (st2.Latitude - st1.Latitude) * (Math.PI / 180);
            double lo_1 = (st2.Longitude - st1.Longitude) * (Math.PI / 180);
            double a = (Math.Sin(l1_2 / 2) * Math.Sin(l1_2 / 2)) +
                Math.Cos(l1) * Math.Cos(l2) *
               (Math.Sin(lo_1 / 2) * Math.Sin(lo_1 / 2));
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var Distance = (earthRadius * c);
            return Distance;
        }



        private TimeSpan CalculateTime(Double Distance)
        {
            TimeSpan time = TimeSpan.Zero;
            double rideDistance = 0.0 + Distance / 1000;
            time = TimeSpan.FromMinutes((rideDistance / 28.0d) * 60);
            time += TimeSpan.FromMinutes(2);
            return time;
         }

        #endregion
    }
}

