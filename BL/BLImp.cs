using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using BL.BLApi;
using BL.BO;
using DalApi;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;


namespace BL
{
    internal class BLImp : IBL
    {
        private IDAL iDal = DalApi.DalFactory.GetIDAL();

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
            bus.LastTreatment = (bus.LastTreatment != DateTime.MinValue) ? bus.LastTreatment : DateTime.Now;
            bus.TotalKM = (bus.TotalKM != null) ? bus.TotalKM : 0;
            bus.LastTreatmentKm = (bus.LastTreatmentKm != null) ? bus.LastTreatmentKm : 0;
            bus.Status = (bus.FuelStatus != 0 && DateTime.Now.Subtract(bus.LastTreatment).Days < 365 &&
                          bus.TotalKM - (int)bus.LastTreatmentKm < 20000 && bus.isActive)
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

        public IEnumerable<Bus> GetBusBy(Predicate<object> predicate)
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
        void IBL.AddLine(Line line)
        {
            if (line == null)
                throw new NullReferenceException("Line to add is Null please try again");

            if (line.FirstStation == null || line.FirstStation == 0)
                throw new BadBusStopIDException("First Station Id not added or not exist ", new ArgumentException());

            if (line.LastStation == null || line.LastStation == 0)
                throw new BadBusStopIDException("First Station Id not added or not exist ", new ArgumentException());

            var station = iDal.GetStation(line.FirstStation);
            if (station == null)
                throw new BadBusStopIDException("First Bus stop not exist in the system", null);

           var station2 = iDal.GetStation(line.LastStation);
            if (station2 == null)
                throw new BadBusStopIDException("Last Bus stop not exist in the system", null);

            try
            {
                iDal.AddLine((DO.Line)line.CopyPropertiesToNew(typeof(DO.Line)));
            }
            catch (Exception e)
            {
                throw new BadLineIdException("Line with the same id is already exist", new ArgumentException());
            }
        }

        void IBL.UpdateLine(Line line)
        {
            if (line == null)
                throw new NullReferenceException("Line is Null ");

            var lineToUpdate = new DO.Line();
            line.CopyPropertiesTo(lineToUpdate);
            iDal.UpdateLine(lineToUpdate);
        }

        void IBL.DeleteLine(Line line)
        {
            if (line == null)
            {
                throw new NullReferenceException("Line to delete is Null");
            }

            DO.Line lineToDelete = new DO.Line();
            line.CopyPropertiesTo(lineToDelete);
            iDal.DeleteLine(lineToDelete.Id);
        }

        Line IBL.GetLine(int lineId)
        {
            if (lineId == null || lineId == 0)
                throw new NullReferenceException("Line id is null or not initialized");

            if (lineId < 0)
                throw new BadBusIdException("Line id can't be negative",
                    new ArgumentException("Line id can't be negative"));

            var lineToCopy = iDal.GetLine(lineId);
            return (Line)lineToCopy.CopyPropertiesToNew(typeof(Line));
        }

        IEnumerable<Line> IBL.GetAllLines()
        {
            foreach (var VARIABLE in iDal.GetAllLines())
            {
                    yield return (Line)VARIABLE.CopyPropertiesToNew(typeof(Line));
            }
        }

        IEnumerable<Line> IBL.GetLineBy(Predicate<BO.Line> predicate)
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
        void IBL.AddBusStop(Station station)
        {
            station.isActive = true;

            if (station.Name.Length == 0) 
            {
                station.Name = "Exemple "+station.Code.ToString();
            }
            if (station.Code==0)
            {
                throw new BadBusStopIDException("bus stop number can't be 0", new ArgumentException());
            }
            if (station.Longitude<34.3||station.Longitude>35.5)
            {
                station.Longitude= double.Parse((new Random(DateTime.Now.Millisecond).NextDouble() * 1.2 + 34.3).ToString().Substring(0, 8));
            }
            if (station.Latitude <= 31 || station.Latitude >= 33.3)
            {
                station.Latitude = double.Parse((new Random(DateTime.Now.Millisecond).NextDouble() * 2.3 + 31).ToString().Substring(0,8));
            }
            try
            {
                iDal.AddStation((DO.Station)station.CopyPropertiesToNew(typeof(DO.Station)));
            }
            catch (Exception e)
            {

                throw new ArgumentException("check details",e);
            }
            Console.WriteLine("wwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwww");
        }

        void IBL.UpdateBusStop(Station station)
        {
            throw new NotImplementedException();
        }

        void IBL.DeleteBusStop(Station station)
        {
            throw new NotImplementedException();
        }

        Station IBL.GetBusStop(int lineId)
        {
            throw new NotImplementedException();
        }

        IEnumerable<Station> IBL.GetAllBusStops()
        {
            foreach (var VARIABLE in iDal.GetAllStation())
            {
                yield return (Station)VARIABLE.CopyPropertiesToNew(typeof(Station));
            }
        }

        IEnumerable<Station> IBL.GetBusStopsBy(Predicate<Station> predicate)
        {
            throw new NotImplementedException();
        }

        IEnumerable<Bus> IBL.GetBusBy(Predicate<Bus> predicate)
        {
            throw new NotImplementedException();
        }


        #endregion

        #region User Simulation

        event Action<TimeSpan> clockObserver = null;
        private DispatcherTimer simulationTimer = new DispatcherTimer();
        internal volatile bool Cancel ;

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
            TimeSpan simulatorTime = new TimeSpan(TimeSpan.FromSeconds(startTime.TotalSeconds).Days , 
                TimeSpan.FromSeconds(startTime.TotalSeconds).Hours ,
                TimeSpan.FromSeconds(startTime.TotalSeconds).Minutes 
                , TimeSpan.FromSeconds(startTime.TotalSeconds).Seconds
                , TimeSpan.FromSeconds(startTime.TotalSeconds).Milliseconds);
            {
                simulationTimer.Interval = new TimeSpan(0, 0, 0, 0, (1000 / (rate * (10 / 6)) ) );
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
                    Debug.Print(simulatorTime.ToString());
                };
                simulationTimer.Start();
            };
        }

        public void StopSimulator()
        {
            Cancel = true;
        }

        public void SetStationPanel(int station, Action<LineTiming> updateBus);

        #endregion
    }
}

