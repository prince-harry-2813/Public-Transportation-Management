using DalApi;
using DO;
using System;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace DalXml
{
    public class DalXMLImp : IDAL
    {
        #region singleton
        static readonly DalXMLImp instance = new DalXMLImp();
        static DalXMLImp() { }
        DalXMLImp() { }
        public static DalXMLImp Instance { get => instance; }
        #endregion

        #region XML Files path
        static XElement configXml;

        static readonly string
            AdjacentStationsPath = "AdjacentStationsXml.xml",
            BusPath = "BusXml.xml",
            BusOnTripPath = "BusOnTripXMl.xml",
            LinePath = "LineXml.xml",
            LineStationPath = "LineStationXml.xml",
            LineTrip = "lineTripXml.xml",
            StationPath = "StatinXml.xml",
            Trip = "TripXml.xml",
            UserPath = "UserXml.xml";
        #endregion



        #region AdjacentStation CRUD Implementation 

        public IEnumerable<AdjacentStations> GetAllAdjacentStations()
        {
            XElement AdjacentStationsRootElement = XMLTools.LoadListFromXMLElement(AdjacentStationsPath);
            return (from s in AdjacentStationsRootElement.Elements()
                    where bool.Parse(s.Element("isActive").Value)
                    select new AdjacentStations()
                    {
                        PairId = Int32.Parse(s.Element("PairId").Value),
                        Station1 = Int32.Parse(s.Element("Station1").Value),
                        Station2 = Int32.Parse(s.Element("Station2").Value),
                        Distance = Double.Parse(s.Element("Distance").Value),
                        Time = TimeSpan.ParseExact(s.Element("Time").Value, "hh\\:mm\\:ss", CultureInfo.InvariantCulture)
                    }
                    );
        }

        public IEnumerable<AdjacentStations> GetAllAdjacentStationsBy(Predicate<AdjacentStations> predicate)
        {
            XElement AdjacentStationsRootElement = XMLTools.LoadListFromXMLElement(AdjacentStationsPath);
            return from s in AdjacentStationsRootElement.Elements()
                       //    where bool.Parse(s.Element("isActive").Value)
                   let s1 = new AdjacentStations()
                   {
                       PairId = Int32.Parse(s.Element("PairId").Value),
                       Station1 = Int32.Parse(s.Element("Station1").Value),
                       Station2 = Int32.Parse(s.Element("Station2").Value),
                       Distance = Double.Parse(s.Element("Distance").Value),
                       Time = TimeSpan.ParseExact(s.Element("Time").Value, "hh\\:mm\\:ss", CultureInfo.InvariantCulture)
                   }
                   where predicate(s1)
                   select s1;
        }

        public AdjacentStations GetAdjacentStations(int id)
        {
            XElement AdjacentStationsRootElement = XMLTools.LoadListFromXMLElement(AdjacentStationsPath);

            AdjacentStations adjacents = (from s in AdjacentStationsRootElement.Elements()
                                          where int.Parse(s.Element("PairId").Value) == id && bool.Parse(s.Element("isActive").Value)
                                          select new AdjacentStations()
                                          {
                                              PairId = Int32.Parse(s.Element("PairId").Value),
                                              Station1 = Int32.Parse(s.Element("Station1").Value),
                                              Station2 = Int32.Parse(s.Element("Station2").Value),
                                              Distance = Double.Parse(s.Element("Distance").Value),
                                              Time = TimeSpan.ParseExact(s.Element("Time").Value, "hh\\:mm\\:ss", CultureInfo.InvariantCulture)

                                          }).FirstOrDefault();

            if (adjacents == null)
            {
                throw new BadIdExeption(id, $"Those adjacent Stations doesn't exist {id}");
            }
            return adjacents;
        }

        public void AddAdjacentStations(AdjacentStations adjacentStations)
        {
            XElement AdjacentStationsRootElement = XMLTools.LoadListFromXMLElement(AdjacentStationsPath);

            XElement adjElem = (from s in AdjacentStationsRootElement.Elements()
                                where int.Parse(s.Element("PairId").Value) == adjacentStations.PairId
                                select s).FirstOrDefault();
            if (adjElem != null)
            {
                if (!bool.Parse(adjElem.Element("isActive").Value))
                    adjElem.Element("isActive").Value = adjacentStations.isActive.ToString();
                else
                    throw new DuplicateObjExeption(adjacentStations.PairId, $"Adjacent Stations already exist in system st'1:{adjacentStations.Station1} - st'2 {adjacentStations.Station2}");
            }
            else
            {
                XElement adjElem1 = new XElement("AdjacentStations",
                    new XElement("PairId", adjacentStations.PairId.ToString()),
                    new XElement("Station1", adjacentStations.Station1.ToString()),
                    new XElement("Station2", adjacentStations.Station2.ToString()),
                    new XElement("Distance", adjacentStations.Distance.ToString()),
                    new XElement("Time", adjacentStations.Time.ToString()),
                    new XElement("isActive", adjacentStations.isActive.ToString()));
                AdjacentStationsRootElement.Add(adjElem1);
            }
            XMLTools.SaveListToXMLElement(AdjacentStationsRootElement, AdjacentStationsPath);

        }

        public void UpdateAdjacentStations(AdjacentStations adjacentStations)
        {
            XElement AdjacentStationsRootElement = XMLTools.LoadListFromXMLElement(AdjacentStationsPath);

            XElement adjStat = (from s in AdjacentStationsRootElement.Elements()
                                where int.Parse(s.Element("PairId").Value) == adjacentStations.PairId && bool.Parse(s.Element("isActicve").Value)
                                select s).FirstOrDefault();
            if (adjStat != null)
            {
                adjStat.Element("PairId").Value = adjacentStations.PairId.ToString();
                adjStat.Element("Station1").Value = adjacentStations.Station1.ToString();
                adjStat.Element("Station2").Value = adjacentStations.Station2.ToString();
                adjStat.Element("Distance").Value = adjacentStations.Distance.ToString();
                adjStat.Element("Time").Value = adjacentStations.Time.ToString();

                XMLTools.SaveListToXMLElement(AdjacentStationsRootElement, AdjacentStationsPath);
            }
            else
                throw new BadIdExeption(adjacentStations.PairId, $"Those adjacent Stations doesn't exist");
        }

        public void UpdateAdjacentStations(int station1, int station2, Action<AdjacentStations> update)
        {
            throw new NotImplementedException();
        }

        public void DeleteAdjacentStations(int id)
        {
            XElement AdjacentStationsRootElement = XMLTools.LoadListFromXMLElement(AdjacentStationsPath);
            XElement adjStat = (from s in AdjacentStationsRootElement.Elements()
                                where int.Parse(s.Element("PairId").Value) == id && bool.Parse(s.Element("isActicve").Value)
                                select s).FirstOrDefault();
            if (adjStat != null)
            {
                adjStat.Element("isActive").Value = Boolean.FalseString;

                XMLTools.SaveListToXMLElement(AdjacentStationsRootElement, AdjacentStationsPath);
            }
            else
                throw new BadIdExeption(id, $"Those adjacent Stations doesn't exist ");
        }
        #endregion

        #region Bus CRUD Implementation

        public Bus GetBus(int id)
        {
            List<Bus> buses = XMLTools.LoadListFromXMLSerializer<Bus>(BusPath);

            Bus bus = buses.FirstOrDefault(b => b.LicenseNum == id);
            if (bus != null && bus.isActive)
                return bus;
            else
                throw new BadIdExeption(id, "Bad id, Bus doesn't exist or deleted");
        }

        public IEnumerable<Bus> GetAllBuses()
        {
            List<Bus> buses = XMLTools.LoadListFromXMLSerializer<Bus>(BusPath);
            return from b in buses
                   where b.isActive
                   select b;
        }

        public IEnumerable<Bus> GetAllBusesBy(Predicate<Bus> predicate)
        {
            List<Bus> buses = XMLTools.LoadListFromXMLSerializer<Bus>(BusPath);
            return from b in buses
                   where predicate(b)
                   select b;

        }

        public void AddBus(Bus bus)
        {
            List<Bus> buses = XMLTools.LoadListFromXMLSerializer<Bus>(BusPath);

            var busCheck = buses.FirstOrDefault(b => b.LicenseNum == bus.LicenseNum);

            if (busCheck != null)
            {
                if (!busCheck.isActive)
                    busCheck.isActive = true;
                else
                    throw new DuplicateObjExeption(bus.LicenseNum, "Bus already exist in system");
            }
            else
            {
                buses.Add(bus);
            }
            XMLTools.SaveListToXMLSerializer(buses, BusPath);
        }

        public void UpdateBus(Bus bus)
        {
            List<Bus> buses = XMLTools.LoadListFromXMLSerializer<Bus>(BusPath);
            var busCheck = buses.FirstOrDefault(b => b.LicenseNum == bus.LicenseNum);
            if (busCheck != null)
            {
                buses.Remove(busCheck);
                buses.Add(bus);
            }
            else
                throw new BadIdExeption(bus.LicenseNum, "Bus doesn't exist in system");
            XMLTools.SaveListToXMLSerializer<Bus>(buses, BusPath);

        }

        public void UpdateBus(int id, Action<Bus> update)
        {
            throw new NotImplementedException();
        }

        public void DeleteBus(int id)
        {
            List<Bus> buses = XMLTools.LoadListFromXMLSerializer<Bus>(BusPath);
            var busCheck = buses.FirstOrDefault(b => b.LicenseNum == id);

            if (busCheck != null)
            {
                busCheck.isActive = false;
            }
            else
                throw new BadIdExeption(id, "Bad id, Bus doesn't exist");
        }

        #endregion

        #region Bus On Trip CRUD Implementation 

        public BusOnTrip GetBusOnTrip(int id)
        {
            return null;
        }

        public IEnumerable<BusOnTrip> GetAllBusOnTrips()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DO.BusOnTrip> GetAllBusesOnTripsBy(Predicate<DO.BusOnTrip> predicate)
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

        public IEnumerable<LineStation> GetAllLinesStationBy(Predicate<LineStation> predicate)
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

        public IEnumerable<LineTrip> GetAllLinesTripBy(Predicate<LineTrip> predicate)
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
