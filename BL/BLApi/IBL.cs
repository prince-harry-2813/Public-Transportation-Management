﻿using System;
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

        #region Line

        void AddLine(BO.Line line);

        void UpdateLine(BO.Line line );

        void DeleteLine(BO.Line line);

        BO.Bus GetLine(int lineId);

        IEnumerable<BO.Line> GetAllLines();

        IEnumerable<BO.Line> GetLineBy(Predicate<BO.Line> predicate);


        #endregion

        #region Bus Stop

        void AddBusStop(BO.Station station);

        void UpdateBusStop(BO.Station station);

        void DeleteBusStop(BO.Station station);

        BO.Station GetBusStop(int lineId);

        IEnumerable<BO.Station> GetAllBusStops();

        IEnumerable<BO.Station> GetBusStopsBy(Predicate<BO.Station> predicate);


        #endregion
    }
}