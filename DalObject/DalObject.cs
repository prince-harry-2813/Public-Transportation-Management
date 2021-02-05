using DalApi;
using DO;
using DS;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DalObject
{
    sealed class DalObject : IDAL
    {
        #region Initial singleton : ID
        /// <summary>
        /// crate single instance for singleton class
        /// </summary>
        private static readonly DalObject instance = new DalObject();
        /// <summary>
        /// define static Ctor for singleton class
        /// </summary>
        static DalObject() { }
        DalObject() { }
        /// <summary>
        /// return instance
        /// </summary>
        public static DalObject Instance { get => instance; }
        #endregion

        #region AdjacentStation CRUD Implementation 

        public IEnumerable<AdjacentStations> GetAllAdjacentStations()
        {
            return from adjs in DataSource.AdjacentStations
                   select adjs;
        }

        public IEnumerable<AdjacentStations> GetAllAdjacentStationsBy(Predicate<AdjacentStations> predicate)
        {
            return from adjs in DataSource.AdjacentStations
                   where predicate(adjs)
                   select adjs;
        }

        public AdjacentStations GetAdjacentStations(int station1,int station2)
        {
            DO.AdjacentStations stations = DataSource.AdjacentStations.FirstOrDefault(adj => adj.Station1 == station1&&adj.Station2==station2);
            if (stations != null)
                return stations;
            else
                throw new BadIdExeption(id, $"there is no direct tripping between those stations");
        }

        public void AddAdjacentStations(AdjacentStations adjacentStations)
        {
            if (DataSource.AdjacentStations.FirstOrDefault(adjs => adjs.PairId == adjacentStations.PairId) != null)
                throw new DuplicateObjExeption(adjacentStations.PairId, "there is already an instance of this pair");
            else
                DataSource.AdjacentStations.Add(adjacentStations);
        }

        public void UpdateAdjacentStations(AdjacentStations adjacentStations)
        {
            DO.AdjacentStations stations = DataSource.AdjacentStations.Find(adj => adj.PairId == adjacentStations.PairId);
            if (stations != null)
            {
                DataSource.AdjacentStations.Remove(stations);
                DataSource.AdjacentStations.Add(adjacentStations);
            }
            else
                throw new BadIdExeption(adjacentStations.PairId, "there is no direct tripping between those stations");
        }

        public void UpdateAdjacentStations(int station1, int station2, Action<AdjacentStations> update)
        {
            throw new NotImplementedException();
        }

        public void DeleteAdjacentStations(int id)
        {
            DO.AdjacentStations stations = DataSource.AdjacentStations.Find(adj => adj.PairId == id);
            if (stations != null)
                DataSource.AdjacentStations.Remove(stations);
            else
                throw new BadIdExeption(id, "there is no direct tripping between those stations");
        }
        #endregion

        #region Bus CRUD Implementation

        public Bus GetBus(int id)
        {
            DO.Bus bus = DataSource.Buses.Find(b => b.LicenseNum == id);
            if (bus != null && bus.isActive)
                return bus;
            else
                throw new BadIdExeption(id);


        }

        public IEnumerable<Bus> GetAllBuses()
        {
            return from bus in DS.DataSource.Buses
                   where bus.isActive
                   select bus;
        }

        public IEnumerable<Bus> GetAllBusesBy(Predicate<Bus> predicate)
        {
            return from bus in DataSource.Buses
                   where predicate(bus)
                   select bus;


        }

        public void AddBus(Bus bus)
        {
            if (DataSource.Buses.FirstOrDefault(b => b.LicenseNum == bus.LicenseNum && b.isActive) != null)
                throw new DuplicateObjExeption(bus.LicenseNum, "This bus already exist");

            DataSource.Buses.Add(bus);
        }

        public void UpdateBus(Bus bus)
        {
            Bus busUp = DataSource.Buses.Find(b => b.LicenseNum == bus.LicenseNum);
            if (busUp != null)
            {
                DataSource.Buses.Remove(busUp);
                DataSource.Buses.Add(bus);
            }
            else
                throw new BadIdExeption(bus.LicenseNum, $"bus {bus.LicenseNum} not exist");
        }

        public void UpdateBus(int id, Action<Bus> update)
        {
            throw new NotImplementedException();
        }

        public void DeleteBus(int id)
        {
            Bus busDlt = DataSource.Buses.Find(bus => bus.LicenseNum == id && bus.isActive);
            if (busDlt != null)
                busDlt.isActive = false;
            else
                throw new BadIdExeption(busDlt.LicenseNum, $"bus {busDlt.LicenseNum} not exist");

        }

        #endregion

        #region Bus On Trip CRUD Implementation 

        public BusOnTrip GetBusOnTrip(int id)
        {
            DO.BusOnTrip bus = DataSource.BusesOnTrips.Find(b => b.Id == id);
            if (bus != null && bus.isActive)
                return bus;
            else
                throw new BadIdExeption(id);
        }

        public IEnumerable<BusOnTrip> GetAllBusOnTrips()
        {
            return from bus in DS.DataSource.BusesOnTrips
                   where bus.isActive
                   select bus;
        }

        public IEnumerable<DO.BusOnTrip> GetAllBusesOnTripsBy(Predicate<DO.BusOnTrip> predicate)
        {
            return from busTrip in DataSource.BusesOnTrips
                   where predicate(busTrip)
                   select busTrip;
        }


        public void AddBusOnTrip(BusOnTrip busOnTrip)
        {
            if (DataSource.BusesOnTrips.FirstOrDefault(b => b.Id == busOnTrip.Id) != null)
                throw new DuplicateObjExeption(busOnTrip.Id, "This bus already in drive");
            DataSource.BusesOnTrips.Add(busOnTrip);
        }

        public void UpdateBusOnTrip(BusOnTrip busOnTrip)
        {
            BusOnTrip busUp = DataSource.BusesOnTrips.Find(b => b.Id == busOnTrip.Id);
            if (busUp != null)
            {
                DataSource.BusesOnTrips.Remove(busUp);
                DataSource.BusesOnTrips.Add(busOnTrip);
            }
            else
                throw new BadIdExeption(busOnTrip.LicenseNum, $"bus {busOnTrip.Id} not exist");

        }

        public void UpdateBusOnTrip(int id, Action<BusOnTrip> update)
        {
            throw new NotImplementedException();
        }

        public void DeleteBusOnTrip(int id)
        {
            BusOnTrip busDlt = DataSource.BusesOnTrips.Find(bus => bus.Id == id);
            if (busDlt != null)
                DataSource.BusesOnTrips.Remove(busDlt);
            else
                throw new BadIdExeption(busDlt.LicenseNum, $"bus {busDlt.Id} not exist");
        }

        #endregion

        #region Line CRUD Implementation 
        public Line GetLine(int id)
        {
            DO.Line line = DataSource.Lines.Find(b => b.Id == id);
            if (line != null)
                return line;
            else
                throw new BadIdExeption(id);
        }

        public IEnumerable<Line> GetAllLines()
        {
            return from line in DS.DataSource.Lines
                   select line;
        }

        public IEnumerable<Line> GetAllLinesBy(Predicate<Line> predicate)
        {
            return from line in DataSource.Lines
                   where predicate(line)
                   select line;
        }

        public void AddLine(Line line)
        {
            if (DataSource.Lines.FirstOrDefault(b => b.Id == line.Id) != null)
                throw new DuplicateObjExeption(line.Id, "This line already signed");
            DataSource.Lines.Add(line);
        }

        public void UpdateLine(Line line)
        {
            Line lineUp = DataSource.Lines.Find(b => b.Id == line.Id);
            if (lineUp != null)
            {
                DataSource.Lines.Remove(lineUp);
                DataSource.Lines.Add(line);
            }
            else
                throw new BadIdExeption(line.Id, $"line {line.Id} not exist");

        }

        public void UpdateLine(int id, Action<Line> update)
        {
            throw new NotImplementedException();
        }

        public void DeleteLine(int id)
        {
            Line lineDlt = DataSource.Lines.Find(line => line.Id == id);
            if (lineDlt != null)
                DataSource.Lines.Remove(lineDlt);
            else
                throw new BadIdExeption(lineDlt.Id, $"line {lineDlt.Id} not exist");

        }

        #endregion

        #region Line Station CRUD Implementation
        public LineStation GetLineStation(int lineId, int stationCode)
        {
            DO.LineStation lineStation = DataSource.LineStations.Find(b => b.LineId == lineId && b.StationId == stationCode);
            if (lineStation != null)
                return lineStation;
            else
                throw new BadIdExeption(stationCode, $"Station number {stationCode} doesn't exist in line {lineId}");
        }

        public IEnumerable<LineStation> GetAllLinesStation()
        {
            return from lineSt in DataSource.LineStations
                   where lineSt.isActive
                   select lineSt;
        }

        public IEnumerable<LineStation> GetAllLinesStationBy(Predicate<LineStation> predicate)
        {
            return from lineSt in DataSource.LineStations
                   where predicate(lineSt)
                   select lineSt;

        }

        public void AddLineStation(LineStation lineStation)
        {
            if (DataSource.LineStations.FirstOrDefault(l => l.LineId == lineStation.LineId && l.StationId == lineStation.StationId) != null)
                throw new DuplicateObjExeption(lineStation.StationId, "This station already signed to line");
            DataSource.LineStations.Add(lineStation);
        }

        public void UpdateLineStation(LineStation lineStation)
        {
            LineStation lineUp = DataSource.LineStations.Find(l => l.LineId == lineStation.LineId && l.StationId == lineStation.StationId);
            if (lineUp != null)
            {
                DataSource.LineStations.Remove(lineUp);
                DataSource.LineStations.Add(lineStation);
            }
            else
                throw new BadIdExeption(lineStation.StationId, $"Station {lineStation.StationId} at line {lineStation.LineId} doesn't exist");

        }

        public void UpdateLineStation(int lineId, int stationCode, Action<LineStation> update)
        {
            throw new NotImplementedException();
        }

        public void DeleteLineStation(int lineId, int stationCode)
        {

            LineStation lineDlt = DataSource.LineStations.Find(line => line.LineId == lineId && line.StationId == stationCode);
            if (lineDlt != null)
                DataSource.LineStations.Remove(lineDlt);
            else
                throw new BadIdExeption(stationCode, $"line {lineId} not having station {stationCode}");
        }

        #endregion

        #region LineTrip CRUD Implementation 

        public LineTrip GetLineTrip(int id)
        {
            DO.LineTrip line = DataSource.LineTrips.Find(b => b.Id == id);
            if (line != null)
                return line;
            else
                throw new BadIdExeption(id);
        }

        public IEnumerable<LineTrip> GetAllLinesTrip()
        {
            return from lineTrip in DataSource.LineTrips
                   where lineTrip.isActive
                   select lineTrip;
        }

        public IEnumerable<LineTrip> GetAllLinesTripBy(Predicate<LineTrip> predicate)
        {
            return from lineTrip in DataSource.LineTrips
                   where predicate(lineTrip)
                   select lineTrip;
        }


        public void AddLineTrip(LineTrip lineTrip)
        {
            if (DataSource.LineTrips.FirstOrDefault(l => l.Id == lineTrip.Id) != null)
                throw new DuplicateObjExeption(lineTrip.Id, "This line already signed");
            DataSource.LineTrips.Add(lineTrip);
        }

        public void UpdateLineTrip(LineTrip lineTrip)
        {
            LineTrip lineUp = DataSource.LineTrips.Find(b => b.Id == lineTrip.Id);
            if (lineUp != null)
            {
                DataSource.LineTrips.Remove(lineUp);
                DataSource.LineTrips.Add(lineTrip);
            }
            else
                throw new BadIdExeption(lineTrip.Id, $"line trip {lineTrip.Id} not exist");

        }

        public void UpdateLineTrip(int id, Action<LineTrip> update)
        {
            throw new NotImplementedException();
        }

        public void DeleteLineTrip(int id)
        {
            LineTrip lineDlt = DataSource.LineTrips.Find(line => line.Id == id);
            if (lineDlt != null)
                DataSource.LineTrips.Remove(lineDlt);
            else
                throw new BadIdExeption(lineDlt.Id, $"line trip {lineDlt.Id} not exist");
        }

        #endregion

        #region Station CRUD Implementation

        public Station GetStation(int id)
        {
            DO.Station station = DataSource.Stations.Find(b => b.Code == id);
            if (station != null)
                return station;
            else
                throw new BadIdExeption(id);
        }

        public IEnumerable<Station> GetAllStation()
        {
            return from station in DataSource.Stations
                   where station.isActive
                   select station;
        }

        public IEnumerable<Station> GetAllStationsBy(Predicate<Station> predicate)
        {
            return from station in DataSource.Stations
                   where predicate(station)
                   select station;
        }

        public void AddStation(Station station)
        {
            if (DataSource.Stations.FirstOrDefault(l => l.Code == station.Code) != null)
                throw new DuplicateObjExeption(station.Code, "This station already exist");
            DataSource.Stations.Add(station);
        }

        public void UpdateStation(Station station)
        {
            Station stationUp = DataSource.Stations.Find(b => b.Code == station.Code);
            if (stationUp != null)
            {
                DataSource.Stations.Remove(stationUp);
                DataSource.Stations.Add(station);
            }
            else
                throw new BadIdExeption(station.Code, $"station {station.Code} not exist");
        }

        public void UpdateStation(int id, Action<Station> update)
        {
            throw new NotImplementedException();
        }

        public void DeleteStation(int id)
        {
            Station station = DataSource.Stations.Find(s => s.Code == id);
            if (station != null && station.isActive)
                station.isActive = true;
            else
                throw new BadIdExeption(id, $" station {id} not exist");
        }
        #endregion

        #region Trip CRUD Implementation 
        public Trip GetTrip(int id)
        {
            DO.Trip trip = DataSource.Trips.FirstOrDefault(t => t.Id == id && t.isActive);
            if (trip != null)
                return trip;
            else throw new BadIdExeption(id, "trip doesn't exist");
        }

        public IEnumerable<Trip> GetAllTrips()
        {
            return from trip in DataSource.Trips
                   where trip.isActive
                   select trip;
        }

        public IEnumerable<DO.Trip> GetAllTripsBy(Predicate<DO.Trip> predicate)
        {
            return from trip in DataSource.Trips
                   where predicate(trip)
                   select trip;
        }


        public void AddTrip(Trip trip)
        {
            var tripChek = DataSource.Trips.FirstOrDefault(t => t.Id == trip.Id);
            if (tripChek != null)
            {
                if (!tripChek.isActive)
                {
                    tripChek.isActive = true;
                }
                else
                    throw new DuplicateObjExeption(trip.Id, "Trip already in system");
            }
            else
            {
                DataSource.Trips.Add(trip);
            }
        }

        public void UpdateTrip(Trip trip)
        {
            var tripChek = DataSource.Trips.FirstOrDefault(t => t.Id == trip.Id);
            if (tripChek != null)
            {
                DataSource.Trips.Remove(tripChek);
                DataSource.Trips.Add(trip);
            }
            else
                throw new BadIdExeption(trip.Id, "trip doesn't exist to update");
        }

        public void UpdateTrip(int id, Action<Trip> update)
        {
            var tripChek = DataSource.Trips.FirstOrDefault(t => t.Id == id);
            if (tripChek != null)
            {
                update(tripChek);
            }
            else
                throw new BadIdExeption(id, "trip doesn't exist to update");

        }

        public void DeleteTrip(int id)
        {
            var tripChek = DataSource.Trips.FirstOrDefault(t => t.Id == id && t.isActive);
            if (tripChek != null)
                tripChek.isActive = false;
            else
                throw new BadIdExeption(id, $"trip doesn't exist or deleted");
        }
        #endregion

        #region User CRUD Implementation

        public User GetUser(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> GetAllUsers()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> GetAllUsersBy(Predicate<User> predicate)
        {
            throw new NotImplementedException();
        }

        public void AddUser(User user)
        {
            throw new NotImplementedException();
        }

        public void UpdateUser(User user)
        {
            throw new NotImplementedException();
        }

        public void UpdateUser(int id, Action<User> update)
        {
            throw new NotImplementedException();
        }

        public void DeleteUser(int id)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
