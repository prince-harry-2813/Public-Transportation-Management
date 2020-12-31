using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalApi
{
    /// <summary>
    /// this interface provide actions to manipulate on data object base on CRUD scheme
    /// </summary>
    public interface IDAL
    {
        #region AdjacentStations
        IEnumerable<DO.AdjacentStations> GetAllAdjacentStations();
        IEnumerable<DO.AdjacentStations> GetAllAdjacentStationsBy(Predicate<DO.AdjacentStations> predicate);
        DO.AdjacentStations GetAdjacentStations(int station1, int station2);
        void AddAdjacentStations(DO.AdjacentStations adjacentStations);
        void UpdateAdjacentStations(DO.AdjacentStations adjacentStations);
        void UpdateAdjacentStations(int station1, int station2, Action<DO.AdjacentStations> update);
        void DeleteAdjacentStations(int station1, int station2);
        #endregion

        #region Bus
        DO.Bus GetBus(int id);
        IEnumerable<DO.Bus> GetAllBuses();
        void AddBus(DO.Bus bus);
        void UpdateBus(DO.Bus bus);
        void UpdateBus(int id, Action<DO.Bus> update); 
        void DeleteBus(int id); 
        #endregion
    }
}
