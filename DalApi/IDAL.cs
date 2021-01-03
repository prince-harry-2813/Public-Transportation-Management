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
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEnumerable<DO.AdjacentStations> GetAllAdjacentStations();
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        IEnumerable<DO.AdjacentStations> GetAllAdjacentStationsBy(Predicate<DO.AdjacentStations> predicate);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="station1"></param>
        /// <param name="station2"></param>
        /// <returns></returns>
        DO.AdjacentStations GetAdjacentStations(int station1, int station2);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="adjacentStations"></param>
        void AddAdjacentStations(DO.AdjacentStations adjacentStations);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="adjacentStations"></param>
        void UpdateAdjacentStations(DO.AdjacentStations adjacentStations);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="station1"></param>
        /// <param name="station2"></param>
        /// <param name="update"></param>
        void UpdateAdjacentStations(int station1, int station2, Action<DO.AdjacentStations> update);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="station1"></param>
        /// <param name="station2"></param>
        void DeleteAdjacentStations(int station1, int station2);
        #endregion

        #region Bus
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        DO.Bus GetBus(int id);
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEnumerable<DO.Bus> GetAllBuses();
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bus"></param>
        void AddBus(DO.Bus bus);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bus"></param>
        void UpdateBus(DO.Bus bus);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="update"></param>
        void UpdateBus(int id, Action<DO.Bus> update); 
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        void DeleteBus(int id);
        
        #endregion

        #region Bus on trip
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        DO.BusOnTrip GetBusOnTrip(int id);
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEnumerable<DO.BusOnTrip> GetAllBusOnTrips();
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="busOnTrip"></param>
        void AddBusOnTrip(DO.BusOnTrip busOnTrip);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="busOnTrip"></param>
        void UpdateBusOnTrip(DO.BusOnTrip busOnTrip);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="update"></param>
        void UpdateBusOnTrip(int id, Action<DO.BusOnTrip> update);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        void DeleteBusOnTrip(int id);
        
        #endregion

        #region Line
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        DO.Line GetLine(int id);
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEnumerable<DO.Line> GetAllLines();
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="line"></param>
        void AddLine(DO.Line line);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="line"></param>
        void UpdateLine(DO.Line line);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="update"></param>
        void UpdateLine(int id, Action<DO.Line> update);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        void DeleteLine(int id);
        
        #endregion

        #region LineStation
        
        DO.LineStation GetLineStation(int lineId,int stationCode);
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEnumerable<DO.LineStation> GetAllLinesStation();
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="line"></param>
        void AddLine(DO.LineStation line);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="line"></param>
        void UpdateLineStation(DO.LineStation line);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lineId"></param>
        /// <param name="stationCode"></param>
        /// <param name="update"></param>
        void UpdateLineStation(int lineId, int stationCode, Action<DO.LineStation> update);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lineId"></param>
        /// <param name="stationCode"></param>
        void DeleteLineStation(int lineId, int stationCode);
        
        #endregion

        #region LineTrip
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        DO.LineTrip GetLineTrip(int id);
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEnumerable<DO.LineTrip> GetAllLinesTrip();
        
        /// <summary>
        /// 
        /// </summary>
        /// <
        /// param name="line"></param>
        void AddLineTrip(DO.LineTrip line);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="line"></param>
        void UpdateLineTrip(DO.LineTrip line);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="update"></param>
        void UpdateLineTrip(int id, Action<DO.LineTrip> update);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        void DeleteLineTrip(int id);
        
        #endregion

        #region Station
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        DO.Station station(int id);
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEnumerable<DO.Station> GetAllStation();
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="line"></param>
        void AddStation(DO.Station line);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="line"></param>
        void UpdateStation(DO.Station line);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="update"></param>
        void UpdateStation(int id, Action<DO.Station> update);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        void DeleteStation(int id);

        #endregion
    }

}
