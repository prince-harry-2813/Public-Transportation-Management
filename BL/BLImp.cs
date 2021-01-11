using System;
using System.Collections.Generic;
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

        private void SeconedToThirdLayerAdapter(object secondLayer , object thirdLayer)
        {

        }
        
        public void AddBus(Bus bus)
        {
            
            DO.Bus busToAdd = new DO.Bus();
            bus.CopyPropertiesTo(busToAdd);
            iDal.AddBus(busToAdd);
        }

        public void DeleteBus(Bus bus)
        {
            // TODO: add constarins
            DO.Bus busToAdd = new DO.Bus();
            bus.CopyPropertiesTo(busToAdd);
            iDal.DeleteBus(busToAdd.LicenseNum);
        }

        public IEnumerable<Bus> GetAllBuses()
        {
            foreach (var VARIABLE in iDal.GetAllBuses())
            {
                yield return (Bus)VARIABLE.CopyPropertiesToNew(typeof(BO.Bus));
            }
        }

        public Bus GetBus(int licenseNum)
        {
            //TODO: Add constarins 
            var busToCopy = iDal.GetBus(licenseNum);
            return (Bus)busToCopy.CopyPropertiesToNew(typeof(Bus));
        }

        public IEnumerable<Bus> GetBusBy(Predicate<Bus> predicate)
        {
            return null;
        }

        public void UpdateBus(Bus bus)
        {
            // TODO: Add Constarins
            var busToUpdate = new DO.Bus();
            bus.CopyPropertiesTo(busToUpdate);
            iDal.UpdateBus(busToUpdate);
        }

        #endregion

        #region Line Implementation

        void IBL.AddLine(Line line)
        {
            throw new NotImplementedException();
        }

        void IBL.UpdateLine(Line line)
        {
            throw new NotImplementedException();
        }

        void IBL.DeleteLine(Line line)
        {
            throw new NotImplementedException();
        }

        Bus IBL.GetLine(int lineId)
        {
            throw new NotImplementedException();
        }

        IEnumerable<Line> IBL.GetAllLines()
        {
            throw new NotImplementedException();
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
        #endregion
    }
}
