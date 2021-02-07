using BL.BO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
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
        private IBL bl;
        private BackgroundWorker getLineStaionworker = new BackgroundWorker();
        private TimeSpan simulationTime;
        List<BusOnTrip> busesOnTrips = new List<BusOnTrip>();
        event Action<TimeSpan> clockObserver = null;
        private DispatcherTimer simulationTimer = new DispatcherTimer();
        internal volatile bool Cancel;

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
                //rideOperation.UpdateSimualtionTime(simulatorTime);
                Debug.Print(simulatorTime.ToString());
            };
            simulationTimer.Start();

        }
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
                        LineTiming lineTiming = new LineTiming()
                        {
                            LastStation = (Station)idal.GetStation(idal.GetLine(item.LineId).LastStation)
                                .CopyPropertiesToNew(typeof(Station)),
                            LineCode = item.LineId,
                            
                        };
                        //Thread.CurrentThread.Name = $"LineTiming: {i} Line : {item.LineId} {item.}"
                    });

                    ////Thread.SpinWait((int)VARIABLE.Frequency.TotalSeconds);
                    //busesOnTrips.Add(new BusOnTrip()
                    //{
                    //    ActualTakeOff = simulationTime,
                    //    Id = i,
                    //    LineId = VARIABLE.LineId,
                    //    isActive = true,
                    //    LicenseNum = idal.GetAllBusesBy(bus => bus.Status == DO.BusStatusEnum.Ok).FirstOrDefault().LicenseNum,
                    //    //NextStationAt = idal.GetAdjacentStations()
                    //});

                    //if (simulationTime.Subtract(item.StartAt).TotalSeconds > 0)
                    //{
                    //    busesOnTrips.Add(new BusOnTrip()
                    //    {

                    //    });
                    //}

                    i++;
                }
            };

            getLineStaionworker.RunWorkerAsync();
        }


        public void StopSimulator()
        {
            Cancel = true;
        }


        //public void StartSimulation()
        //{
        //    foreach (var item in linesTrips)
        //    {
        //        //   for
        //        Task.Factory.StartNew(() =>
        //        {
        //            LineTiming lineTiming = new LineTiming()
        //            {
        //                // LastStation = (LineStation)idal.GetStation(idal.GetLineStation(item.LineId).LastStation)
        //                //   .CopyPropertiesToNew(typeof(Station))
        //                //,ArrivingTime = 
        //            };
        //        });
        //    }

        //}
    }
}
