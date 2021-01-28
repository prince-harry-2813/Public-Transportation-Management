using BL.BO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

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

        #region Line

        void AddLine(BO.Line line);

        void UpdateLine(BO.Line line);

        void DeleteLine(BO.Line line);

        BO.Line GetLine(int lineId);

        IEnumerable<BO.Line> GetAllLines();

        IEnumerable<BO.Line> GetLinesBy(Predicate<BO.Line> predicate);


        #endregion

        #region Bus Stop

        void AddStation(BO.Station station);

        void UpdateStation(BO.Station station);

        void DeleteStation(BO.Station station);

        BO.Station GetStation(int lineId);

        IEnumerable<BO.Station> GetAllStations();

        IEnumerable<BO.Station> GetStationBy(Predicate<BO.Station> predicate);


        #endregion

        #region User simulation

        void StartSimulator(TimeSpan startTime, int Rate, Action<TimeSpan> updateTime);
        void StopSimulator();
        void SetStationPanel(int station, Action<LineTiming> updateBus);

        #endregion

        #region Line Station

        BO.LineStation GetLineStation(int lineId, int stationCode);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEnumerable<BO.LineStation> GetAllLinesStation();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        IEnumerable<BO.LineStation> GetAllLinesStationBy(Predicate<BO.LineStation> predicate);

        void AddLineStation(BO.LineStation lineStation);

        void UpdateLineStation(BO.LineStation lineStation);

        void UpdateLineStation(int lineId, int stationCode, Action<BO.LineStation> update);


        #endregion

    }
}