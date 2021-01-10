using System;
using System.Collections.Generic;
using System.Linq;
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
            
            var busToAdd = new Bus();
            bus.CopyPropertiesTo(busToAdd);
        }

        public void DeleteBus(Bus bus)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Bus> GetAllBuses()
        {
            return null;
        }

        public Bus GetBus(int licenseNum)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Bus> GetBusBy(Predicate<Bus> predicate)
        {
            throw new NotImplementedException();
        }

        public void UpdateBus(Bus bus)
        {
            throw new NotImplementedException();
        } 
        #endregion
    }
}
