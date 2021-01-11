using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using BL.BLApi;
using BL.BO;
using DalApi;

namespace BL
{
    internal class BLImp : IBL
    {
        private IDAL iDal = DalApi.DalFactory.GetIDAL();
        
        #region IBL Bus Implementation

        /// <summary>
        /// Add New bus and sing unsighn properties apporpiatly 
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
            bus.LastTreatment = (bus.LastTreatment != null) ?  bus.LastTreatment :DateTime.Now;
            bus.TotalKM = (bus.TotalKM != null) ? bus.TotalKM : 0;
            bus.LastTreatmentKm = (bus.LastTreatmentKm != null) ? bus.LastTreatmentKm : 0;
            bus.Status = (bus.FuelStatus != 0 && DateTime.Now.Subtract(bus.LastTreatment).Days < 365 &&
                          bus.TotalKM - (int) bus.LastTreatmentKm < 20000 && bus.isActive)
                ? BusStatusEnum.Ok
                : BusStatusEnum.Not_Available;

            var anotherBus = iDal.GetAllBusesBy(bus1 => bus1.LicenseNum == bus.LicenseNum && bus1.isActive);

            if (anotherBus != null) // checks if there is any bus return from DS by the license number 
            {
                DO.Bus busToAdd = new DO.Bus();
                bus.CopyPropertiesTo(busToAdd);
                iDal.AddBus(busToAdd);
            }
            else
                throw new BadBusIdException("Bus With the same license number is already exsist", null);
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
           
            DO.Bus busToAdd = new DO.Bus();
            bus.CopyPropertiesTo(busToAdd);
            iDal.DeleteBus(busToAdd.LicenseNum);
        }

        public IEnumerable<Bus> GetAllBuses()
        {
            // TODO :  check if this solution is good enough
            foreach (var VARIABLE in iDal.GetAllBuses())
            {
                if (VARIABLE.isActive)// Ignore deleted bus  
                {
                    yield return (Bus) VARIABLE.CopyPropertiesToNew(typeof(BO.Bus));
                }
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
            if (licenseNum == null || licenseNum == 0 )
                throw new NullReferenceException("license number is null or not initialized");

            if (licenseNum < 0)
                throw new BadBusIdException("Bus license number can't be negative" ,
                    new ArgumentException("Bus license number can't be negative"));

            if (licenseNum <= 999999 || licenseNum >= 100000000 )
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
                throw new BadBusStopIDException("First Station Id not added or not exsit ", new ArgumentException());
            
            if (line.LastStation == null || line.LastStation == 0)
                throw new BadBusStopIDException("First Station Id not added or not exsit ", new ArgumentException());

            var station = iDal.GetAllStationsBy(station1 => station1.Code == line.FirstStation);
            if (station == null)
                throw new BadBusStopIDException("First Bus stop not exsit in the system", null);

            station = iDal.GetAllStationsBy(station1 => station1.Code == line.LastStation);
            if (station == null)
                throw new BadBusStopIDException("Last Bus stop not exsit in the system", null);

            var anotherLineCode = iDal.GetAllLinesBy(line1 => line1.Id == line.Id);
            if (anotherLineCode == null)
                iDal.AddLine((DO.Line)line.CopyPropertiesToNew(typeof(DO.Line)));
            
            else 
                throw new BadLineIdException("Line with the same id is already exsist", new ArgumentException());
            
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

        IEnumerable<Line> IBL.GetLineBy(Predicate<Line> predicate)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
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
    }
}
