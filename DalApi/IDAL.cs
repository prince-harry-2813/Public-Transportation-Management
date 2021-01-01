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
        #region Adjacent Stations
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

        #region Bus on trip
        DO.BusOnTrip GetBusOnTrip(int id);
        IEnumerable<DO.BusOnTrip> GetAllBusOnTrips();
        void AddBusOnTrip(DO.BusOnTrip busOnTrip);
        void UpdateBusOnTrip(DO.BusOnTrip busOnTrip);
        void UpdateBusOnTrip(int id, Action<DO.BusOnTrip> update);
        void DeleteBusOnTrip(int id);
        #endregion

        #region Line
        DO.Line GetLine(int id);
        IEnumerable<DO.Line> GetAllLines();
        void AddLine(DO.Line line);
        void UpdateLine(DO.Line line);
        void UpdateLine(int id, Action<DO.Line> update);
        void DeleteLine(int id);
        #endregion

        #region LineStation
        DO.LineStation GetLineStation(int lineId,int stationCode);
        IEnumerable<DO.LineStation> GetAllLinesStation();
        void AddLine(DO.LineStation line);
        void UpdateLineStation(DO.LineStation line);
        void UpdateLineStation(int lineId, int stationCode, Action<DO.LineStation> update);
        void DeleteLineStation(int lineId, int stationCode);
        #endregion

        #region LineTrip
        DO.LineTrip GetLineTrip(int id);
        IEnumerable<DO.LineTrip> GetAllLinesTrip();
        void AddLineTrip(DO.LineTrip line);
        void UpdateLineTrip(DO.LineTrip line);
        void UpdateLineTrip(int id, Action<DO.LineTrip> update);
        void DeleteLineTrip(int id);
        #endregion

        #region Station
        DO.Station station(int id);
        IEnumerable<DO.Station> GetAllStation();
        void AddStation(DO.Station line);
        void UpdateStation(DO.Station line);
        void UpdateStation(int id, Action<DO.Station> update);
        void DeleteStation(int id);

        #endregion
    }

}
