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

            var station = iDal.GetAllStationsBy(station1 => station1.Code == line.FirstStation);
            if (station == null)
                throw new BadBusStopIDException("First Bus stop not exist in the system", null);

            station = iDal.GetAllStationsBy(station1 => station1.Code == line.LastStation);
            if (station == null)
                throw new BadBusStopIDException("Last Bus stop not exist in the system", null);

            var anotherLineCode = iDal.GetAllLinesBy(line1 => line1.Id == line.Id);
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
                if (VARIABLE.isActive)
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
            throw new NotImplementedException();
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
            foreach (var item in iDal.GetAllStation())
            {
                yield return (Station)item.CopyPropertiesToNew(typeof(Station));
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

        /// <summary>
        /// Start simulator stop watch and update it according 
        /// </summary>
        /// <param name="startTime">TIME TO START  </param>
        /// <param name="Rate"> Hz per minute</param>
        /// <param name="updateTime">Action</param>
        public void StartSimulator(TimeSpan startTime, int rate, Action<TimeSpan> updateTime)
        {
            //Timer
            //Nullable<TimeSpan> nullableTime = new TimeSpan(startTime.Hours, startTime.Minutes, startTime.Seconds);
            //Stopwatch stopwatch = new Stopwatch();
            //stopwatch.Restart();
            //SimulatorClock simulatorClock = new SimulatorClock(startTime , rate);

            //var lastMeaserTime = new TimeSpan(startTime.Ticks);
            //while (true)
            //{
            //                   clockObserver(new TimeSpan(simulatorClock.Time.Hours, simulatorClock.Time.Minutes, simulatorClock.Time.Seconds));
            //    updateTime = clockObserver;
            //    Thread.Sleep(1000 / rate);
            //}
        }

        //Stopwatch 
        //Clock 
        //simulatorClock = new Clock(simulatorStartTime + new TimeSpan(stopwatch.ElapsedTicks * simulatorRate));
        //clockObserver(new TimeSpan(simulatorClock.Time.Hours, simulatorClock.Time.Minutes, simulatorClock.Time.Seconds));
        //Thread.Sleep(your - sleep - time -in -msec);


        public void StopSimulator()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
