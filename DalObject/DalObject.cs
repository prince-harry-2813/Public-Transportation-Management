using System;
using System.Collections.Generic;
using System.Linq;
using DalApi;
using DO;
using DS;

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

        #region AdjacentStation CRUD Implemntation 

        public IEnumerable<AdjacentStations> GetAllAdjacentStations()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<AdjacentStations> GetAllAdjacentStationsBy(Predicate<AdjacentStations> predicate)
        {
            throw new NotImplementedException();
        }

        public AdjacentStations GetAdjacentStations(int station1, int station2)
        {
            throw new NotImplementedException();
        }

        public void AddAdjacentStations(AdjacentStations adjacentStations)
        {
            throw new NotImplementedException();
        }

        public void UpdateAdjacentStations(AdjacentStations adjacentStations)
        {
            throw new NotImplementedException();
        }

        public void UpdateAdjacentStations(int station1, int station2, Action<AdjacentStations> update)
        {
            throw new NotImplementedException();
        }

        public void DeleteAdjacentStations(int station1, int station2)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public IEnumerable<BusOnTrip> GetAllBusOnTrips()
        {
            throw new NotImplementedException();
        }

        public void AddBusOnTrip(BusOnTrip busOnTrip)
        {
            throw new NotImplementedException();
        }

        public void UpdateBusOnTrip(BusOnTrip busOnTrip)
        {
            throw new NotImplementedException();
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
