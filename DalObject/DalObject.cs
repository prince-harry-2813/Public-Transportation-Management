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

        public AdjacentStations GetAdjacentStations(int id)
        {
            DO.AdjacentStations stations = DataSource.AdjacentStations.FirstOrDefault(adj => adj.PairId == id);
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

        public void DeleteAdjacentStations(int station1, int station2)
        {
            DO.AdjacentStations stations = DataSource.AdjacentStations.Find(adj => adj.Station1 == station1 && adj.Station2 == station2);
            if (stations != null)
                DataSource.AdjacentStations.Remove(stations);
            else
                throw new BadIdExeption(int.Parse(station1.ToString() + station2.ToString()), "there is no direct tripping between those stations");
        }
        #endregion

        #region Bus CRUD Implementation

        public Bus GetBus(int id)
        {
            DO.Bus bus = DataSource.Buses.Find(b => b.LicenseNum == id);
            if (bus != null)
                return bus;
            else
                throw new BadIdExeption(id);
        }

        public IEnumerable<Bus> GetAllBuses()
        {
            return from bus in DS.DataSource.Buses
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
            if (DataSource.Buses.FirstOrDefault(b => b.LicenseNum == bus.LicenseNum) != null)
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
            Bus busDlt = DataSource.Buses.Find(bus => bus.LicenseNum == id);
            if (busDlt != null)
                DataSource.Buses.Remove(busDlt);
            else
                throw new BadIdExeption(busDlt.LicenseNum, $"bus {busDlt.LicenseNum} not exist");

        }

        #endregion

        #region Bus On Trip CRUD Implementation 

        public BusOnTrip GetBusOnTrip(int id)
        {
            DO.BusOnTrip bus = DataSource.BusesOnTrips.Find(b => b.Id == id);
            if (bus != null)
                return bus;
            else
                throw new BadIdExeption(id);
        }

        public IEnumerable<BusOnTrip> GetAllBusOnTrips()
        {
            return from bus in DS.DataSource.BusesOnTrips
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
            throw new NotImplementedException();
        }

        #endregion

        #region Line CRUD Implementation 
        public Line GetLine(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Line> GetAllLines()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Line> GetAllLinesBy(Predicate<Line> predicate)
        {
            throw new NotImplementedException();
        }

        public void AddLine(Line line)
        {
            throw new NotImplementedException();
        }

        public void UpdateLine(Line line)
        {
            throw new NotImplementedException();
        }

        public void UpdateLine(int id, Action<Line> update)
        {
            throw new NotImplementedException();
        }

        public void DeleteLine(int id)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Line Station CRUD Implementation
        public LineStation GetLineStation(int lineId, int stationCode)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<LineStation> GetAllLinesStation()
        {
            throw new NotImplementedException();
        }

        public void AddLine(LineStation lineStation)
        {
            throw new NotImplementedException();
        }

        public void UpdateLineStation(LineStation lineStation)
        {
            throw new NotImplementedException();
        }

        public void UpdateLineStation(int lineId, int stationCode, Action<LineStation> update)
        {
            throw new NotImplementedException();
        }

        public void DeleteLineStation(int lineId, int stationCode)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region LineTrip CRUD Implementation 

        public LineTrip GetLineTrip(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<LineTrip> GetAllLinesTrip()
        {
            throw new NotImplementedException();
        }

        public void AddLineTrip(LineTrip lineTrip)
        {
            throw new NotImplementedException();
        }

        public void UpdateLineTrip(LineTrip lineTrip)
        {
            throw new NotImplementedException();
        }

        public void UpdateLineTrip(int id, Action<LineTrip> update)
        {
            throw new NotImplementedException();
        }

        public void DeleteLineTrip(int id)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Station CRUD Implementation

        public Station station(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Station> GetAllStation()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Station> GetAllStationsBy(Predicate<Station> predicate)
        {
            throw new NotImplementedException();
        }

        public void AddStation(Station station)
        {
            throw new NotImplementedException();
        }

        public void UpdateStation(Station station)
        {
            throw new NotImplementedException();
        }

        public void UpdateStation(int id, Action<Station> update)
        {
            throw new NotImplementedException();
        }

        public void DeleteStation(int id)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Trip CRUD Implementation 
        public Trip trip(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Trip> GetAllTrips()
        {
            throw new NotImplementedException();
        }

        public void AddTrip(Trip trip)
        {
            throw new NotImplementedException();
        }

        public void UpdateTrip(Trip user)
        {
            throw new NotImplementedException();
        }

        public void UpdateTrip(int id, Action<Trip> update)
        {
            throw new NotImplementedException();
        }

        public void DeleteTrip(int id)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region User CRUD Implementation

        public User user(int id)
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
