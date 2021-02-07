using BL.BO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using BL.BLApi;
using DalApi;

namespace BL
{
    class RidesOperation
    {
       
        #region Singalton 

        private static RidesOperation instance;

        public static RidesOperation Instance
        {
            get => instance ?? new RidesOperation();
            set
            {
                if (Instance == null)
                {
                    value = new RidesOperation();
                }
                instance = value;
            }
        }

        #endregion

        #region Properties Class 

        public int interval;
        private event EventHandler<LineTiming> updateBusPrivate;
        private int staionID;
        List<LineTrip> linesTrips = new List<LineTrip>();
        private List<LineTiming> lineTimes = new List<LineTiming>();
        private IDAL idal;
        private BackgroundWorker getLineStaionworker = new BackgroundWorker();

        /// <summary>
        /// private member that holds the global simulator time 
        /// </summary>
        private TimeSpan simulationTime;

        event Action<TimeSpan> clockObserver = null;

        private event Action<LineTiming> updateLineProgress = null; 
         
        /// <summary>
        /// 
        /// </summary>
        private DispatcherTimer simulationTimer = new DispatcherTimer();

        /// <summary>
        /// 
        /// </summary>
        internal volatile bool Cancel;

        /// <summary>
        /// 
        /// </summary>
        private int stationNumber;

        #endregion

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
            //rideOperation.interval = simulationTimer.Interval.Milliseconds;
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
                simulationTime = simulatorTime;
                //rideOperation.UpdateSimualtionTime(simulatorTime);
                Debug.Print(simulatorTime.ToString());
            };
            simulationTimer.Start();
        }

        /// <summary>
        /// ctor
        /// </summary>
        private RidesOperation()
        {
            idal = DalFactory.GetIDAL();

            if (getLineStaionworker.IsBusy)
            {
                getLineStaionworker.CancelAsync();
            }

            #region Rides Operation Worker Initialization 

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

            #endregion

            getLineStaionworker.RunWorkerCompleted += (sender, args) =>
            {
                linesTrips.Sort((trip, lineTrip) => trip.StartAt.CompareTo(lineTrip.StartAt));

                int i = 0;
                foreach (var item in linesTrips)
                {
                    
                    Task.Factory.StartNew(() =>
                    {
                        TimeSpan timeToArrive = item.StartAt;

                        #region Get All station Of line trip 

                        List<BO.LineStation> lineStationsOfLine = new List<LineStation>();
                        foreach (var stationItem in idal.GetAllLinesStationBy(station => station.LineId == item.LineId))
                        {
                            #region Adapter from DO to BO 
                            var tmpLineStation = (LineStation)stationItem.CopyPropertiesToNew(typeof(LineStation));
                            tmpLineStation.Station = new Station();
                            DO.Station stationDO = new DO.Station();
                            int stationID = stationItem.StationId;
                            try
                            {
                                stationDO = idal.GetStation(stationID);
                            }
                            catch (Exception e)
                            {
                                throw new BadBusStopIDException("check details", e);
                            }

                            stationDO.CopyPropertiesTo(tmpLineStation.Station);

                            #endregion

                            lineStationsOfLine.Add(tmpLineStation);
                        }

                        lineStationsOfLine.OrderBy(station => station.LineStationIndex);

                        #endregion

                        LineTiming lineTiming = new LineTiming()
                        {
                            LastStation = lineStationsOfLine.Last().Station,
                            LineCode = item.LineId,
                            LineID = item.LineId,
                            StartedTime = item.StartAt,
                        };

                        Thread.CurrentThread.Name =
                            $"{i} , {lineTiming.LineCode} , {lineTiming.LineID},{lineTiming.StartedTime}";

                        if (lineStationsOfLine.Count() == 0)
                            return;

                        foreach (var lineStation in lineStationsOfLine)
                        {
                            if (lineStation.NextStation == 0)
                            {
                                break;
                            }
                            timeToArrive += idal.GetAdjacentStations(lineStation.Station.Code, lineStation.NextStation).Time;
                        }

                        lineTiming.ArrivingTime = timeToArrive;

                        while (true)
                        {
                            Thread.SpinWait(interval);
                            updateLineProgress(lineTiming);
                        }
                    });


                
                    i++;
                }
            };

            getLineStaionworker.RunWorkerAsync();
        }

        /// <summary>
        /// cancel all simultor
        /// </summary>
        public void StopSimulator()
        {
            Cancel = true;
        }

        public void SetSimulationPanel(int station, Action<LineTiming> updateBus)
        {
            stationNumber = station;
            updateLineProgress = updateBus;
        }
    }
}
