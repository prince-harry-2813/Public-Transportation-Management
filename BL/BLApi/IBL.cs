using System;
using System.Collections.Generic;

namespace BL.BLApi
{
    public interface IBL
    {
        #region Bus

        void AddBus(BO.Bus bus);

        void UpdateBus(BO.Bus bus);

        void DeleteBus(BO.Bus bus);

        BO.Bus GetBus(int licenseNum);
        
        IEnumerable<BO.Bus> GetAllBuses();

        IEnumerable<BO.Bus> GetBusBy(Predicate<BO.Bus> predicate);
        

        #endregion
    }
}