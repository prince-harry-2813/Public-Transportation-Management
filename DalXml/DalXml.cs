using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;

namespace DalXml
{
    public class DalXml : IDAL
    {

        #region singleton
        static readonly DalXml instance = new DalXml();
        static DalXml() { }
        DalXml() { }
        public static DalXml Instance { get => instance; }
        #endregion

        #region XML Files path
        // static XElement configXml;

        static readonly string
            AdjacentStationsPath = "AdjacentStationsXml.xml",
            BusPath = "BusXml.xml",
            BusOnTripPath = "BusOnTripXMl.xml",
            LinePath = "LineXml.xml",
            LineStationPath = "LineStationXml.xml",
            LineTripPath = "LineTripXml.xml",
            StationPath = "StatoinXml.xml",
            TripPath = "TripXml.xml",
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
                        PairId = int.Parse(s.Element("PairId").Value),
                        Station1 = int.Parse(s.Element("Station1").Value),
                        Station2 = int.Parse(s.Element("Station2").Value),
                        Distance = double.Parse(s.Element("Distance").Value),
                        Time = TimeSpan.ParseExact(s.Element("Time").Value, "hh\\:mm\\:ss", CultureInfo.InvariantCulture)
                    }
                    );
        }

        public IEnumerable<AdjacentStations> GetAllAdjacentStationsBy(Predicate<AdjacentStations> predicate)
        {
            XElement AdjacentStationsRootElement = XMLTools.LoadListFromXMLElement(AdjacentStationsPath);
            return from s in AdjacentStationsRootElement.Elements()
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

        public AdjacentStations GetAdjacentStations(int station1, int staion2)
        {
            XElement AdjacentStationsRootElement = XMLTools.LoadListFromXMLElement(AdjacentStationsPath);

            AdjacentStations adjacents = (from s in AdjacentStationsRootElement.Elements()
                                          where int.Parse(s.Element("Station1").Value) == station1 && int.Parse(s.Element("Station2").Value) == staion2 && bool.Parse(s.Element("isActive").Value)
                                          select new AdjacentStations()
                                          {
                                              PairId = int.Parse(s.Element("PairId").Value),
                                              Station1 = int.Parse(s.Element("Station1").Value),
                                              Station2 = int.Parse(s.Element("Station2").Value),
                                              Distance = double.Parse(s.Element("Distance").Value),
                                              Time = TimeSpan.ParseExact(s.Element("Time").Value, "hh\\:mm\\:ss", CultureInfo.InvariantCulture)

                                          }).FirstOrDefault();

            if (adjacents == null)
            {
                throw new BadIdExeption(station1, $"Those adjacent Stations doesn't exist {station1}{staion2}");
            }
            return adjacents;
        }

        public void AddAdjacentStations(AdjacentStations adjacentStations)
        {
            XElement AdjacentStationsRootElement = XMLTools.LoadListFromXMLElement(AdjacentStationsPath);

            XElement adjElem = (from s in AdjacentStationsRootElement.Elements()
                                where int.Parse(s.Element("Station1").Value) == adjacentStations.Station1 && int.Parse(s.Element("Station2").Value) == adjacentStations.Station2
                                select s).FirstOrDefault();
            if (adjElem != null)
            {
                if (!bool.Parse(adjElem.Element("isActive").Value))
                    adjElem.Element("isActive").Value = adjacentStations.isActive.ToString();
                else
                    return;
            }
            else
            {
                XElement adjElem1 = new XElement("AdjacentStations",
                    new XElement("PairId", adjacentStations.PairId.ToString()),
                    new XElement("Station1", adjacentStations.Station1.ToString()),
                    new XElement("Station2", adjacentStations.Station2.ToString()),
                    new XElement("Distance", adjacentStations.Distance.ToString()),
                    new XElement("Time", adjacentStations.Time.ToString("hh\\:mm\\:ss", CultureInfo.InvariantCulture)),
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
                adjStat.Element("isActive").Value = bool.FalseString;

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
            var busCheck = buses.FirstOrDefault(b => b.isActive && b.LicenseNum == bus.LicenseNum);
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
            var busCheck = buses.FirstOrDefault(b => b.LicenseNum == id && b.isActive);

            if (busCheck != null)
            {
                busCheck.isActive = false;
            }
            else
                throw new BadIdExeption(id, "Bad id, Bus doesn't exist");
            XMLTools.SaveListToXMLSerializer(buses, BusPath);
        }

        #endregion

        #region Bus On Trip CRUD Implementation 

        public BusOnTrip GetBusOnTrip(int id)
        {
            List<BusOnTrip> buses = XMLTools.LoadListFromXMLSerializer<BusOnTrip>(BusOnTripPath);
            BusOnTrip bus = buses.FirstOrDefault(b => b.Id == id);
            if (bus != null && bus.isActive)
                return bus;
            throw new BadIdExeption(id, $"No such bus on trip:{id}");
        }

        public IEnumerable<BusOnTrip> GetAllBusOnTrips()
        {
            List<BusOnTrip> buses = XMLTools.LoadListFromXMLSerializer<BusOnTrip>(BusOnTripPath);
            return from bus in buses
                   where bus.isActive
                   select bus;
        }

        public IEnumerable<DO.BusOnTrip> GetAllBusesOnTripsBy(Predicate<DO.BusOnTrip> predicate)
        {
            List<BusOnTrip> buses = XMLTools.LoadListFromXMLSerializer<BusOnTrip>(BusOnTripPath);
            return from bus in buses
                   where predicate(bus)
                   select bus;
        }


        public void AddBusOnTrip(BusOnTrip busOnTrip)
        {
            XElement BusesOnTripRootElement = XMLTools.LoadListFromXMLElement(BusOnTripPath);

            XElement bus = (from s in BusesOnTripRootElement.Elements()
                            where int.Parse(s.Element("Id").Value) == busOnTrip.Id
                            select s).FirstOrDefault();
            if (bus != null)
            {
                if (!bool.Parse(bus.Element("isActive").Value))
                    bus.Element("isActive").Value = bool.TrueString;
                else
                    throw new DuplicateObjExeption(busOnTrip.Id, $"This bus is already on a trip  {busOnTrip.Id}");
            }
            else
            {
                XElement adjElem1 = new XElement("BusOnTrip",
                    new XElement("Id", busOnTrip.Id.ToString()),
                    new XElement("LicenseNum", busOnTrip.LicenseNum.ToString()),
                    new XElement("LineId", busOnTrip.LineId.ToString()),
                    new XElement("PlannedTakeOff", busOnTrip.PlannedTakeOff.ToString("g")),
                    new XElement("ActualTakeOff", busOnTrip.ActualTakeOff.ToString()),
                    new XElement("PrevStation", busOnTrip.PrevStation.ToString()),
                    new XElement("PrevSatationAt", busOnTrip.PrevSatationAt.ToString()),
                    new XElement("NextStationAt", busOnTrip.NextStationAt.ToString()),
                    new XElement("isActive", busOnTrip.isActive.ToString()));
                BusesOnTripRootElement.Add(adjElem1);
            }
            XMLTools.SaveListToXMLElement(BusesOnTripRootElement, BusOnTripPath);

        }

        public void UpdateBusOnTrip(BusOnTrip busOnTrip)
        {

            XElement BusesOnTripRootElement = XMLTools.LoadListFromXMLElement(BusOnTripPath);

            XElement bus = (from s in BusesOnTripRootElement.Elements()
                            where int.Parse(s.Element("Id").Value) == busOnTrip.Id && bool.Parse(s.Element("isActicve").Value)
                            select s).FirstOrDefault();
            if (bus != null)
            {
                bus.Element("Id").Value = busOnTrip.Id.ToString();
                bus.Element("LicenseNum").Value = busOnTrip.LicenseNum.ToString();
                bus.Element("LineId").Value = busOnTrip.LineId.ToString();
                bus.Element("PlannedTakeOff").Value = busOnTrip.PlannedTakeOff.ToString("g");
                bus.Element("ActualTakeOff").Value = busOnTrip.ActualTakeOff.ToString();
                bus.Element("PrevStation").Value = busOnTrip.PrevStation.ToString();
                bus.Element("PrevSatationAt").Value = busOnTrip.PrevSatationAt.ToString();
                bus.Element("NextStationAt").Value = busOnTrip.NextStationAt.ToString();


                XMLTools.SaveListToXMLElement(BusesOnTripRootElement, BusOnTripPath);
            }
            else
                throw new BadIdExeption(busOnTrip.Id, $"This Trip bus doesn't exist{busOnTrip.Id}");

        }

        public void UpdateBusOnTrip(int id, Action<BusOnTrip> update)
        {
            throw new NotImplementedException();
        }

        public void DeleteBusOnTrip(int id)
        {
            XElement BusesOnTripRootElement = XMLTools.LoadListFromXMLElement(BusOnTripPath);
            XElement bus = (from s in BusesOnTripRootElement.Elements()
                            where int.Parse(s.Element("Id").Value) == id && bool.Parse(s.Element("isActicve").Value)
                            select s).FirstOrDefault();
            if (bus != null)
            {
                bus.Element("isActive").Value = bool.FalseString;

                XMLTools.SaveListToXMLElement(BusesOnTripRootElement, BusOnTripPath);
            }
            else
                throw new BadIdExeption(id, $"This Bus Trip doesn't exist or Deleted ");

        }

        #endregion

        #region Line CRUD Implementation 
        public Line GetLine(int id)
        {
            List<Line> Lines = XMLTools.LoadListFromXMLSerializer<Line>(LinePath);

            Line line = Lines.FirstOrDefault(b => b.Id == id);
            if (line != null && line.isActive)
                return line;
            else
                throw new BadIdExeption(id, $"Bad id, Line {id} doesn't exist or deleted");

        }

        public IEnumerable<Line> GetAllLines()
        {
            List<Line> Lines = XMLTools.LoadListFromXMLSerializer<Line>(LinePath);
            return from l in Lines
                   where l.isActive
                   select l;
        }

        public IEnumerable<Line> GetAllLinesBy(Predicate<Line> predicate)
        {
            List<Line> Lines = XMLTools.LoadListFromXMLSerializer<Line>(LinePath);
            return from l in Lines
                   where predicate(l)
                   select l;
        }

        public void AddLine(Line line)
        {
            List<Line> Lines = XMLTools.LoadListFromXMLSerializer<Line>(LinePath);

            var lineCheck = Lines.FirstOrDefault(b => b.Id == line.Id);

            if (lineCheck != null)
            {
                if (!lineCheck.isActive)
                    lineCheck.isActive = true;
                else
                    throw new DuplicateObjExeption(line.Id, "Line already exist in system");
            }
            else
            {
                Lines.Add(line);
            }
            XMLTools.SaveListToXMLSerializer(Lines, LinePath);
        }

        public void UpdateLine(Line line)
        {
            List<Line> Lines = XMLTools.LoadListFromXMLSerializer<Line>(LinePath);
            var lineCheck = Lines.FirstOrDefault(b => b.Id == line.Id);
            if (lineCheck != null)
            {
                Lines.Remove(lineCheck);
                Lines.Add(line);
            }
            else
                throw new BadIdExeption(line.Id, "Line doesn't exist in system");
            XMLTools.SaveListToXMLSerializer<Line>(Lines, LinePath);

        }

        public void UpdateLine(int id, Action<Line> update)
        {
            throw new NotImplementedException();
        }

        public void DeleteLine(int id)
        {
            List<Line> Lines = XMLTools.LoadListFromXMLSerializer<Line>(LinePath);
            var lineCheck = Lines.FirstOrDefault(b => b.Id == id && b.isActive);
            if (lineCheck != null)
            {
                lineCheck.isActive = false;
            }
            else
                throw new BadIdExeption(id, "Line doesn't exist in system");
            XMLTools.SaveListToXMLSerializer<Line>(Lines, LinePath);

        }

        #endregion

        #region Line Station CRUD Implementation
        public LineStation GetLineStation(int lineId, int stationCode)
        {
            List<LineStation> Lines = XMLTools.LoadListFromXMLSerializer<LineStation>(LineStationPath);

            LineStation line = Lines.FirstOrDefault(b => b.LineId == lineId && b.StationId == stationCode);
            if (line != null && line.isActive)
                return line;
            else
                throw new BadIdExeption(int.Parse(lineId.ToString() + stationCode.ToString()), $"Bad id, Line {lineId} doesn't have a stop {stationCode} or deleted");

        }


        public IEnumerable<LineStation> GetAllLinesStation()
        {
            List<LineStation> Lines = XMLTools.LoadListFromXMLSerializer<LineStation>(LineStationPath);
            return from l in Lines
                   where l.isActive
                   select l;
        }

        public IEnumerable<LineStation> GetAllLinesStationBy(Predicate<LineStation> predicate)
        {
            List<LineStation> Lines = XMLTools.LoadListFromXMLSerializer<LineStation>(LineStationPath);
            return from l in Lines
                   where predicate(l)
                   orderby l.LineId
                   orderby l.LineStationIndex
                   select l;
        }

        public void AddLineStation(LineStation lineStation)
        {

            List<LineStation> Lines = XMLTools.LoadListFromXMLSerializer<LineStation>(LineStationPath);

            var lineCheck = Lines.FirstOrDefault(b => b.LineId == lineStation.LineId && b.StationId == lineStation.StationId);
            if (lineCheck != null)
            {
                if (!lineCheck.isActive)
                {
                    lineCheck.isActive = true;
                    UpdateLineStation(lineStation);
                }
                else
                    throw new DuplicateObjExeption(lineStation.StationId, "Line Station already exist in system");
            }
            else
            {
                Lines.Add(lineStation);
            }
            XMLTools.SaveListToXMLSerializer(Lines, LineStationPath);

        }

        public void UpdateLineStation(LineStation lineStation)
        {
            List<LineStation> Lines = XMLTools.LoadListFromXMLSerializer<LineStation>(LineStationPath);

            var lineCheck = Lines.FirstOrDefault(b => b.LineId == lineStation.LineId && b.StationId == lineStation.StationId);
            if (lineCheck != null)
            {
                Lines.Remove(lineCheck);
                Lines.Add(lineStation);
            }
            else
                throw new BadIdExeption(lineStation.StationId, "Line Station doesn't exist or deleted");
            XMLTools.SaveListToXMLSerializer(Lines, LineStationPath);

        }

        public void UpdateLineStation(int lineId, int stationCode, Action<LineStation> update)
        {
            List<LineStation> Lines = XMLTools.LoadListFromXMLSerializer<LineStation>(LineStationPath);
            var lineCheck = Lines.FirstOrDefault(b => b.LineId == lineId && b.StationId == stationCode);
            if (lineCheck != null)
            {
                var newLineSt = new LineStation()
                {
                    isActive = lineCheck.isActive,
                    StationId = lineCheck.StationId,
                    LineStationIndex = lineCheck.LineStationIndex,
                    LineId = lineCheck.LineId,
                    NextStation = lineCheck.NextStation,
                    PrevStation = lineCheck.PrevStation
                };
                update(newLineSt);
                Lines.Remove(lineCheck);
                Lines.Add(newLineSt);
            }
            else
                throw new BadIdExeption(stationCode, "Line station doesn't exist or deleted");
            XMLTools.SaveListToXMLSerializer(Lines, LineStationPath);
        }

        public void DeleteLineStation(int lineId, int stationCode)
        {
            List<LineStation> Lines = XMLTools.LoadListFromXMLSerializer<LineStation>(LineStationPath);
            var lineCheck = Lines.FirstOrDefault(b => b.LineId == lineId && b.StationId == stationCode && b.isActive);
            if (lineCheck != null)
            {
                lineCheck.isActive = false;
            }
            else
                throw new BadIdExeption(stationCode, "Line Station doesn't exist in system");
            XMLTools.SaveListToXMLSerializer<LineStation>(Lines, LineStationPath);
        }

        #endregion

        #region LineTrip CRUD Implementation 

        public LineTrip GetLineTrip(int id)
        {
            List<LineTrip> Lines = XMLTools.LoadListFromXMLSerializer<LineTrip>(LineTripPath);

            LineTrip line = Lines.FirstOrDefault(b => b.Id == id);
            if (line != null && line.isActive)
                return line;
            else
                throw new BadIdExeption(id, $"Bad id, Line trip {id} doesn't exist or deleted");

        }

        public IEnumerable<LineTrip> GetAllLinesTrip()
        {
            List<LineTrip> Lines = XMLTools.LoadListFromXMLSerializer<LineTrip>(LineTripPath);
            return from lt in Lines
                   where lt.isActive
                   select lt;
        }

        public IEnumerable<LineTrip> GetAllLinesTripBy(Predicate<LineTrip> predicate)
        {

            List<LineTrip> Lines = XMLTools.LoadListFromXMLSerializer<LineTrip>(LineTripPath);
            return from lt in Lines
                   where predicate(lt)
                   select lt;
        }


        public void AddLineTrip(LineTrip lineTrip)
        {

            List<LineTrip> Lines = XMLTools.LoadListFromXMLSerializer<LineTrip>(LineTripPath);

            var lineCheck = Lines.FirstOrDefault(b => b.Id == lineTrip.Id);
            if (lineCheck != null)
            {
                if (!lineCheck.isActive)
                    lineCheck.isActive = true;

                else
                    throw new DuplicateObjExeption(lineTrip.Id, "Line Trip already exist in system");
            }
            else
            {
                Lines.Add(lineTrip);
            }
            XMLTools.SaveListToXMLSerializer(Lines, LineTripPath);

        }

        public void UpdateLineTrip(LineTrip lineTrip)
        {
            List<LineTrip> Lines = XMLTools.LoadListFromXMLSerializer<LineTrip>(LineTripPath);
            var lineCheck = Lines.FirstOrDefault(b => b.isActive && b.Id == lineTrip.Id);
            if (lineCheck != null)
            {
                Lines.Remove(lineCheck);
                Lines.Add(lineTrip);
            }
            else
                throw new BadIdExeption(lineTrip.Id, "Line doesn't exist in system");
            XMLTools.SaveListToXMLSerializer<LineTrip>(Lines, LineTripPath);

        }

        public void UpdateLineTrip(int id, Action<LineTrip> update)
        {
            throw new NotImplementedException();
        }

        public void DeleteLineTrip(int id)
        {
            List<LineTrip> Lines = XMLTools.LoadListFromXMLSerializer<LineTrip>(LineTripPath);
            var lineCheck = Lines.FirstOrDefault(b => b.Id == id && b.isActive);
            if (lineCheck != null)
            {
                lineCheck.isActive = false;
            }
            else
                throw new BadIdExeption(id, "Line doesn't exist in system");
            XMLTools.SaveListToXMLSerializer(Lines, LineTripPath);

        }

        #endregion

        #region Station CRUD Implementation

        public Station GetStation(int id)
        {
            List<Station> Stations = XMLTools.LoadListFromXMLSerializer<Station>(StationPath);

            Station station = Stations.FirstOrDefault(b => b.Code == id);
            if (station != null && station.isActive)
                return station;
            else
                throw new BadIdExeption(id, $"Bad id, Station {id} doesn't exist or deleted");
        }

        public IEnumerable<Station> GetAllStation()
        {
            List<Station> Stations = XMLTools.LoadListFromXMLSerializer<Station>(StationPath);
            return from s in Stations
                   where s.isActive
                   select s;
        }

        public IEnumerable<Station> GetAllStationsBy(Predicate<Station> predicate)
        {
            List<Station> Stations = XMLTools.LoadListFromXMLSerializer<Station>(StationPath);
            return from s in Stations
                   where predicate(s)
                   select s;
        }

        public void AddStation(Station station)
        {
            List<Station> Stations = XMLTools.LoadListFromXMLSerializer<Station>(StationPath);
            Station staCheck = Stations.FirstOrDefault(s => s.Code == station.Code);
            if (staCheck != null)
            {
                if (!staCheck.isActive)
                {
                    staCheck.isActive = true;
                }
                else
                {
                    throw new DuplicateObjExeption(station.Code, "Station already exist");
                }
            }
            else
            {
                Stations.Add(station);
            }
            XMLTools.SaveListToXMLSerializer(Stations, StationPath);
        }

        public void UpdateStation(Station station)
        {
            List<Station> Stations = XMLTools.LoadListFromXMLSerializer<Station>(StationPath);
            Station staCheck = Stations.FirstOrDefault(s => s.Code == station.Code && s.isActive);
            if (staCheck != null)
            {
                Stations.Remove(staCheck);
                Stations.Add(station);
            }
            else
                throw new BadIdExeption(station.Code, $"Bad id, Station {station.Code} doesn't exist or deleted");

            XMLTools.SaveListToXMLSerializer(Stations, StationPath);

        }

        public void UpdateStation(int id, Action<Station> update)
        {
            throw new NotImplementedException();
        }

        public void DeleteStation(int id)
        {
            List<Station> Stations = XMLTools.LoadListFromXMLSerializer<Station>(StationPath);
            Station staCheck = Stations.FirstOrDefault(s => s.Code == id && s.isActive);
            if (staCheck != null)
            {
                staCheck.isActive = false;
            }
            else
                throw new BadIdExeption(id, $"Bad id, Station {id} doesn't exist or deleted");

            XMLTools.SaveListToXMLSerializer(Stations, StationPath);
        }
        #endregion

        #region Trip CRUD Implementation 
        public Trip GetTrip(int id)
        {
            List<Trip> Trips = XMLTools.LoadListFromXMLSerializer<Trip>(TripPath);
            Trip trip = Trips.FirstOrDefault(t => t.Id == id && t.isActive);
            if (trip != null)
            {
                return trip;
            }
            throw new BadIdExeption(id, $"Trip {id} Doesn't exist or deleted");
        }

        public IEnumerable<Trip> GetAllTrips()
        {
            throw new NotImplementedException();
        }
        public IEnumerable<Trip> GetAllTripsBy(Predicate<Trip> predicate)
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

