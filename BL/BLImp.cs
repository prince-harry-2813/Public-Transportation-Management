using BL.BLApi;
using BL.BO;
using DalApi;
using System;
using System.Collections.Generic;
using System.Linq;


namespace BL
{
    internal class BLImp : IBL
    {
        private static IDAL iDal = DalApi.DalFactory.GetIDAL();

        #region IBL Bus Implementation

        /// <summary>
        /// Add New bus and sign unsigned properties appropriately 
        /// </summary>
        /// <param name="bus"></param>
        public void AddBus(Bus bus)
        {
            if (bus == null)
                throw new ArgumentNullException("Bus to Add is Null");

            if (bus.LicenseNum == 0 || bus.LicenseNum == null)
                throw new BadBusIdException("License number not initialized ", null);

            bus.FuelStatus = (bus.FuelStatus != null) ? bus.FuelStatus : 1200;
            bus.isActive = true;
            bus.LastTreatmentDate = (bus.LastTreatmentDate != DateTime.MinValue) ? bus.LastTreatmentDate : DateTime.Now;
            bus.TotalKM = (bus.TotalKM != null) ? bus.TotalKM : 0;
            bus.KmOnLastTreatment = (bus.KmOnLastTreatment != null) ? bus.KmOnLastTreatment : 0;
            bus.Status = (bus.FuelStatus != 0 && DateTime.Now.Subtract(bus.LastTreatmentDate).Days < 365 &&
                          bus.TotalKM - (int)bus.KmOnLastTreatment < 20000 && bus.isActive)
                ? BusStatusEnum.Ok
                : BusStatusEnum.Not_Available;



            try // checks if there is any bus return from DS by the license number 
            {

                DO.Bus busToAdd = new DO.Bus();
                bus.CopyPropertiesTo(busToAdd);
                iDal.AddBus(busToAdd);
            }
            catch (Exception)
            {
                throw new BadBusIdException("Bus With the same license number is already exist", null);
            }

        }

        /// <summary>
        /// Copy bus to DO property and send it do DAL to mark as not active
        /// </summary>
        /// <param name="bus"></param>
        public void DeleteBus(Bus bus)
        {
            if (bus == null)
            {
                throw new NullReferenceException("Bus to delete is Null");
            }

            DO.Bus busToDelete = new DO.Bus();
            bus.CopyPropertiesTo(busToDelete);
            iDal.DeleteBus(busToDelete.LicenseNum);
        }

        public IEnumerable<Bus> GetAllBuses()
        {
            // TODO :  check if this solution is good enough
            foreach (var variable in iDal.GetAllBuses())
            {
                //if (variable.IsActive)  //deleted because DAL does this check    // Ignore deleted bus  

                yield return (Bus)variable.CopyPropertiesToNew(typeof(BO.Bus));

            }

            #region Seconed Solution

            //List<Bus> busesToreturn = new List<Bus>();
            //IEnumerable<DO.Bus> busesToCopy = iDal.GetAllBuses();
            //foreach (var bus in busesToCopy)
            //{
            //    busesToreturn.Add((Bus) bus.CopyPropertiesToNew(typeof(BO.Bus)));
            //}

            //return busesToreturn;

            #endregion
        }

        public Bus GetBus(int licenseNum)
        {
            if (licenseNum == null || licenseNum == 0)
                throw new NullReferenceException("license number is null or not initialized");

            if (licenseNum < 0)
                throw new BadBusIdException("Bus license number can't be negative",
                    new ArgumentException("Bus license number can't be negative"));

            if (licenseNum <= 999999 || licenseNum >= 100000000)
                throw new BadBusIdException("Bus license number is too large or too small",
                    new ArgumentException("Bus license number is too large or too small"));

            var busToCopy = iDal.GetBus(licenseNum);
            return (Bus)busToCopy.CopyPropertiesToNew(typeof(Bus));
        }

        public IEnumerable<Bus> GetBusBy(Predicate<Bus> predicate)
        {
            return from bus in iDal.GetAllBusesBy(b => b.isActive || !b.isActive)
                   let BUS = new Bus().CopyPropertiesToNew(typeof(Bus))
                   where predicate((Bus)BUS)
                   orderby ((Bus)BUS).LicenseNum
                   select BUS as Bus;
        }
        public void UpdateBus(Bus bus)
        {
            if (bus == null)
                throw new NullReferenceException("Bus is Null ");

            var busToUpdate = new DO.Bus();
            bus.CopyPropertiesTo(busToUpdate);
            iDal.UpdateBus(busToUpdate);
        }

        #endregion

        #region Line Implementation

        /// <summary>
        /// Add new line to system, also creating new lines stations and connect them, and add basic information for all dependencies
        /// </summary>
        /// <param name="line">
        /// </param>
        public void AddLine(Line line)
        {
            if (line == null)
                throw new NullReferenceException("Line to add is Null please try again");

            if (line.FirstStation == null || line.FirstStation.Station.Code == 0)
                throw new BadBusStopIDException("First Station Id not added or not exist ", new ArgumentException());

            if (line.LastStation == null || line.LastStation.Station.Code == 0)
                throw new BadBusStopIDException("First Station Id not added or not exist ", new ArgumentException());

            var station = iDal.GetStation(line.FirstStation.Station.Code);
            if (station == null)
                throw new BadBusStopIDException("First Bus stop not exist in the system", null);

            var station2 = iDal.GetStation(line.LastStation.Station.Code);
            if (station2 == null)
                throw new BadBusStopIDException("Last Bus stop not exist in the system", null);

            var FirstStID = line.FirstStation.Station.Code; //1st station code to insert
            var LastStID = line.LastStation.Station.Code;//2nd station code to insert

            //adding new line-stations to system  
            try
            {
                iDal.AddLineStation(new DO.LineStation()
                {
                    LineId = line.Id,
                    LineStationIndex = 0,
                    isActive = true,
                    StationId = FirstStID,
                    NextStation = LastStID
                });
                iDal.AddLineStation(new DO.LineStation()
                {
                    LineId = line.Id,
                    LineStationIndex = 1,
                    isActive = true,
                    StationId = LastStID,
                    PrevStation = FirstStID
                });
                //check if there is already an adjacent stations
                var adjSta = iDal.GetAllAdjacentStationsBy(a => a.Station1 == FirstStID && a.Station2 == FirstStID);
                if (adjSta.FirstOrDefault() == null)
                {
                    iDal.AddAdjacentStations(new DO.AdjacentStations()
                    {
                        isActive = true,
                        Station1 = FirstStID,
                        Station2 = LastStID,
                        Distance = Utilities.CalculateDistance(line.FirstStation.Station, line.LastStation.Station),
                        PairId = iDal.GetAllAdjacentStationsBy(l => l.isActive || !l.isActive).Count() + 1,
                        Time = Utilities.CalculateTime(Utilities.CalculateDistance(line.FirstStation.Station, line.LastStation.Station))
                    });

                }
            }
            catch (Exception e)
            {
                throw new ArgumentException("Couldn't add First and last Stations, Check inner exception", e);
            }

            try
            {
                iDal.AddLine(new DO.Line()
                {
                    LastStation = LastStID,
                    Area = (DO.Area)line.Area,
                    Code = line.Code,
                    FirstStation = FirstStID,
                    Id = line.Id,
                    isActive = true
                });
            }
            catch (Exception e)
            {
                throw new BadLineIdException("Couldn't add Line to System, check inner exception", e);
            }
        }

        /// <summary>
        /// update exist system line, adding or removing some line-stations, calculating the line trip duration 
        /// </summary>
        /// <param name="line"></param>
        public void UpdateLine(Line line)
        {
            if (line == null)

                throw new NullReferenceException("Line is Null ");
            //clean the system to update line-stations
            foreach (var item in iDal.GetAllLinesStationBy(l => l.isActive && l.LineId == line.Id))
            {
                iDal.DeleteLineStation(line.Id, item.StationId);
            }
            //update the line- stations
            foreach (var item in line.Stations)
            {
                var DOLineStation = new DO.LineStation()
                {
                    LineId = line.Id,
                    LineStationIndex = item.LineStationIndex,
                    isActive = true,
                    StationId = item.Station.Code,
                    NextStation = item.LineStationIndex == line.Stations.Count() - 1 ? 0 : line.Stations.ElementAt(item.LineStationIndex + 1).Station.Code,
                    PrevStation = item.LineStationIndex > 0 ? line.Stations.ElementAt(item.LineStationIndex - 1).Station.Code : 0,
                };
                try
                {
                    iDal.AddLineStation(DOLineStation);
                }
                catch (Exception e)
                {
                    throw new BadBusStopIDException("Couldn't update or add, check inner Exceptions details", e);
                }
            }
            var lineToUpdate = new DO.Line()
            {
                LastStation = iDal.GetAllLinesStationBy(ls => ls.LineId == line.Id).OrderBy(b => b.LineStationIndex).Last().StationId,
                Area = (DO.Area)line.Area,
                Code = line.Code,
                FirstStation = iDal.GetAllLinesStationBy(ls => ls.LineId == line.Id).OrderBy(o => o.LineStationIndex).First().StationId,
                Id = line.Id,
                isActive = true
            };
            try
            {
                iDal.UpdateLineStation(lineToUpdate.Id, lineToUpdate.FirstStation, b => { b.isActive = true; });
                iDal.UpdateLineStation(lineToUpdate.Id, lineToUpdate.LastStation, b => { b.isActive = true; });

                iDal.UpdateLine(lineToUpdate);
            }
            catch (Exception e)
            {
                throw new ArgumentException("Couldn't update line, Check inner Exception details", e);
            }
        }

        public void DeleteLine(Line line)
        {
            if (line == null)
            {
                throw new NullReferenceException("Line to delete is Null");
            }
            DO.Line lineToDelete = iDal.GetLine(line.Id);
            {

            };
            try
            {
                iDal.DeleteLine(lineToDelete.Id);
                //clean the system list of line stations 
                foreach (var item in iDal.GetAllLinesStationBy(l => l.isActive && l.LineId == line.Id))
                {
                    iDal.DeleteLineStation(line.Id, item.StationId);
                }
            }
            catch (Exception e)
            {
                throw new KeyNotFoundException("Couldn't complete operation, check inner exception", e);
            }
        }

        public Line GetLine(int lineId)
        {
            if (lineId == null || lineId == 0)
                throw new NullReferenceException("Line id is null or not initialized");

            if (lineId < 0)
                throw new BadBusIdException("Line id can't be negative",
                    new ArgumentException("Line id can't be negative"));

            var DalLine = iDal.GetLine(lineId);
            Line line = new Line()
            {
                FirstStation = new LineStation(),
                LastStation = new LineStation(),
                Stations = new List<LineStation>(),
                Area = (BO.Area)DalLine.Area,
                Code = DalLine.Code,
                Id = DalLine.Id,
                IsActive = true
            };
            var DOLineStations = iDal.GetAllLinesStationBy(l => l.isActive && l.LineId == lineId).OrderBy(l => l.LineStationIndex);
            foreach (var item in DOLineStations)
            {
                var BOLS = new LineStation()
                {
                    LineId = line.Id,
                    LineStationIndex = item.LineStationIndex,
                    Station = GetStation(item.StationId),
                    PrevStation = item.LineStationIndex - 1 < 0 ? 0 : DOLineStations.ElementAt(item.LineStationIndex - 1).StationId,
                    NextStation = item.LineStationIndex + 1 == DOLineStations.Count() ? 0 : DOLineStations.ElementAt(item.LineStationIndex + 1).StationId,
                    IsActive = true
                };
                if (BOLS.LineStationIndex == 0)
                    BOLS.CopyPropertiesTo(line.FirstStation);
                if (BOLS.LineStationIndex == DOLineStations.Count() - 1)
                    BOLS.CopyPropertiesTo(line.LastStation);
                line.Stations.Append(BOLS);

            }

            return line;
        }

        public IEnumerable<Line> GetAllLines()
        {
            foreach (var variable in iDal.GetAllLines())
            {
                var line = new Line()
                {
                    Id = variable.Id,
                    Code = variable.Code,
                    Area = (Area)variable.Area,
                    IsActive = variable.isActive,
                    LastStation = GetLineStation(variable.Id, variable.LastStation),
                    FirstStation = GetLineStation(variable.Id, variable.FirstStation),

                    Stations = GetAllLinesStationBy(l => l.IsActive && l.LineId == variable.Id).OrderBy(l => l.LineStationIndex),

                };
                yield return line;
            }
        }

        public IEnumerable<Line> GetLinesBy(Predicate<BO.Line> predicate)
        {
            return from item in iDal.GetAllLinesBy(l => l.isActive || !l.isActive)
                   let stations = iDal.GetAllLinesStationBy(ls => ls.LineId == item.Id && ls.isActive)//get the line station 
                   let Bol = new Line()
                   {
                       LastStation = GetAllLinesStationBy(i => i.LineId == item.Id && i.Station.Code == item.LastStation).GetEnumerator().Current,
                       FirstStation = GetAllLinesStationBy(i => i.LineId == item.Id && i.Station.Code == item.FirstStation).GetEnumerator().Current,


                       Stations = from LS in GetAllLinesStationBy(l => l.IsActive && l.LineId == item.Id)
                                  orderby LS.LineStationIndex
                                  select LS,
                       Id = item.Id,
                       IsActive = item.isActive,
                       Area = (BO.Area)item.Area,
                       Code = item.Code

                   }
                   where predicate(Bol)
                   orderby Bol.Id
                   select Bol;




        }


        #endregion

        #region Station Implementation
        public void AddStation(Station station)
        {
            station.isActive = true;

            if (station.Name.Length == 0)
            {
                station.Name = "Example " + station.Code.ToString();
            }
            if (station.Code == 0)
            {
                throw new BadBusStopIDException("bus stop number can't be 0", new ArgumentException());
            }
            if (station.Longitude < 34.3 || station.Longitude > 35.5)
            {
                station.Longitude = double.Parse((new Random(DateTime.Now.Millisecond).NextDouble() * 1.2 + 34.3).ToString().Substring(0, 8));
            }
            if (station.Latitude <= 31 || station.Latitude >= 33.3)
            {
                station.Latitude = double.Parse((new Random(DateTime.Now.Millisecond).NextDouble() * 2.3 + 31).ToString().Substring(0, 8));
            }
            try
            {
                iDal.AddStation((DO.Station)station.CopyPropertiesToNew(typeof(DO.Station)));
            }
            catch (Exception e)
            {

                throw new ArgumentException("check details", e);
            }
            Console.WriteLine("wwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwww");
        }

        public void UpdateStation(Station station)
        {
            DO.Station DOstation = new DO.Station();
            station.CopyPropertiesTo(DOstation);
            iDal.UpdateStation(DOstation);
        }

        public void DeleteStation(Station station)
        {
            DO.Station DOstation = new DO.Station();
            station.CopyPropertiesTo(DOstation);
            iDal.DeleteStation(DOstation.Code);
        }

        public Station GetStation(int Id)
        {
            Station station = new Station();
            iDal.GetStation(Id).CopyPropertiesTo(station);
            return station;
        }

        public IEnumerable<Station> GetAllStations()
        {
            foreach (var variable in iDal.GetAllStation().OrderBy(station => station.Code))
            {
                var bostation = (Station)variable.CopyPropertiesToNew(typeof(Station));

                yield return bostation;
            }
        }

        public IEnumerable<Station> GetStationBy(Predicate<Station> predicate)
        {
            return from st in iDal.GetAllStationsBy(b => b.Code > 0)
                   let boSt = new Station()
                   {
                       Code = st.Code,
                       isActive = st.isActive,
                       Latitude = st.Latitude,
                       Longitude = st.Longitude,
                       Name = st.Name
                   }
                   where predicate(boSt)
                   orderby boSt.Code
                   select boSt;
        }
        #endregion

        #region Line Station

        public LineStation GetLineStation(int lineId, int stationCode)
        {
            LineStation station = new LineStation();
            iDal.GetLineStation(lineId, stationCode).CopyPropertiesTo(station);
            station.Station = (BO.Station)iDal.GetStation(stationCode).CopyPropertiesToNew(typeof(BO.Station));
            return station;
        }

        public IEnumerable<LineStation> GetAllLinesStation()
        {
            foreach (var item in iDal.GetAllLinesStation())
            {
                yield return LineStationAdapter(item);
            }
        }

        public IEnumerable<LineStation> GetAllLinesStationBy(Predicate<BO.LineStation> predicate)
        {
            foreach (var item in iDal.GetAllLinesStationBy(l => l.isActive || !l.isActive))
            {
                LineStation lineStation = new LineStation()
                {
                    LineId = item.LineId,
                    LineStationIndex = item.LineStationIndex,
                    IsActive = item.isActive,

                };

                lineStation.Station = new Station();
                lineStation.Station = GetStation(item.StationId);
                if (predicate(lineStation))
                {
                    yield return lineStation;
                }
            }
        }

        public void AddLineStation(LineStation lineStation)
        {

            var LineStations = GetAllLinesStationBy(ls => ls.LineId == lineStation.LineId && ls.IsActive);
            //expending the indexing of line Stations
            foreach (var item in LineStations.OrderBy(o => o.LineStationIndex))
            {
                if (item.LineStationIndex < lineStation.LineStationIndex)
                    continue;
                UpdateLineStation(lineStation.LineId, lineStation.Station.Code, l => l.LineStationIndex++);
            }
            //create DO obj to add in system
            var lStDO = new DO.LineStation()
            {
                isActive = true,
                LineId = lineStation.LineId,
                LineStationIndex = lineStation.LineStationIndex,
                NextStation = lineStation.NextStation,
                PrevStation = lineStation.PrevStation,
                StationId = lineStation.Station.Code
            };
            try
            {
                iDal.AddLineStation(lStDO);
            }
            catch (Exception e) { throw new ArgumentException("couldn't add Station to line", e); }

            //arrange the stations of line and their properties
            var stationsOfLine = iDal.GetAllLinesStationBy(ls => ls.isActive && ls.LineId == lineStation.LineId).OrderBy(o => o.LineStationIndex);
            int i = 0, j = stationsOfLine.Count();
            foreach (var item in stationsOfLine)
            {
                item.NextStation = 0;
                item.PrevStation = 0;
                if (i != 0)
                    item.PrevStation = stationsOfLine.ElementAt(i - 1).StationId;
                if (i < j - 1)
                    item.NextStation = stationsOfLine.ElementAt(i + 1).StationId;
                i++;
            }
            //create new adjacent stations for the new station
            foreach (var item in LineStations.OrderBy(o => o.LineStationIndex).Skip(1))
            {
                AdjacentStations adjacent = new AdjacentStations()
                {
                    isActive = true,
                    Station2 = item.Station,
                    Station1 = LineStations.FirstOrDefault(f => f.Station.Code == item.PrevStation).Station,

                    Time = TimeSpan.Zero
                };
                adjacent.Distance = Utilities.CalculateDistance(adjacent.Station1, adjacent.Station2);
                adjacent.Time = Utilities.CalculateTime(adjacent.Distance);
                try
                {
                    iDal.AddAdjacentStations((DO.AdjacentStations)adjacent.CopyPropertiesToNew(typeof(DO.AdjacentStations)));
                }
                catch (Exception e)
                {
                    continue;
                }

            }

        }

        public void UpdateLineStation(LineStation lineStation)
        {
            iDal.UpdateLineStation((DO.LineStation)lineStation.CopyPropertiesToNew(typeof(DO.LineStation)));
        }

        public void UpdateLineStation(int lineId, int stationCode, Action<LineStation> update)
        {
            var a = (DO.LineStation)iDal.GetAllLinesStationBy(station => station.LineId == lineId && station.StationId == stationCode).FirstOrDefault();
            if (!(a == null))
            {
                LineStation boLineStation = GetLineStation(lineId, stationCode);
                update(boLineStation);
                boLineStation.CopyPropertiesTo(a);
                a.StationId = boLineStation.Station.Code;
                iDal.UpdateLineStation(a);
            }
        }

        public LineStation LineStationAdapter(DO.LineStation linestaDO)
        {
            LineStation lsta = new LineStation();
            DO.Station stationDO;
            int stationID = linestaDO.StationId;
            try
            {
                stationDO = iDal.GetStation(stationID);
            }
            catch (Exception e)
            {

                throw new BadBusStopIDException("check details", e);
            }
            stationDO.CopyPropertiesTo(lsta.Station);
            linestaDO.CopyPropertiesTo(lsta);
            return lsta;
        }

        #endregion




        #region Lines of Station
        /// <summary>
        /// return list of lines that have stop in some stations.
        /// TODO also return info for timing or another way....
        /// </summary>
        /// <param name="stationCode"></param>
        /// <returns></returns>
        public LinesOfStation getLinesOfStation(int stationCode)
        {
            LinesOfStation listOfLines = new LinesOfStation();

            var a = GetAllLines().Distinct();

            listOfLines.Lines = a.Where(line => line.Stations.Any(station => station.Station.Code == stationCode));

            return listOfLines;
        }


        #endregion
        #region Ride Operation

        public void StopSimulator()
        {
            // Cancel = true;
        }

        private RidesOperation rideOperation = RidesOperation.Instance;

        public void SetStationPanel(int station, Action<LineTiming> updateBus)
        {
            if (station == -1)
            {
                //TODO: Shut down
            }

            rideOperation.StartSimulation();
        }
        #endregion

        void IBL.StartSimulator(TimeSpan startTime, int Rate, Action<TimeSpan> updateTime)
        {
            throw new NotImplementedException();
        }


        #region User Simulation


        #endregion

    }


    internal static class Utilities
    {
        #region Utilities

        /// <summary>
        /// calculate the distance between previous station to current
        /// This uses the Haversine formula to calculate the short distance between tow coordinates on sphere surface  
        /// </summary>
        /// <param name="other"> previous or other station </param>
        /// <returns>Short distance in meters </returns>
        internal static double CalculateDistance(Station st1, Station st2)
        {
            double earthRadius = 6371e3;
            double l1 = st1.Latitude * (Math.PI / 180);
            double l2 = st2.Latitude * (Math.PI / 180);
            double l1_2 = (st2.Latitude - st1.Latitude) * (Math.PI / 180);
            double lo_1 = (st2.Longitude - st1.Longitude) * (Math.PI / 180);
            double a = (Math.Sin(l1_2 / 2) * Math.Sin(l1_2 / 2)) +
                Math.Cos(l1) * Math.Cos(l2) *
               (Math.Sin(lo_1 / 2) * Math.Sin(lo_1 / 2));
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var Distance = (earthRadius * c);
            return Distance;
        }



        /// <summary>
        /// calculate the time of ride to this station from previous 
        /// one KM per minute
        /// </summary>
        internal static TimeSpan CalculateTime(Double Distance)
        {
            TimeSpan time = TimeSpan.Zero;
            double rideDistance = 0.0 + Distance / 1000;
            time = TimeSpan.FromMinutes((rideDistance / 28.0d) * 60);
            time += TimeSpan.FromMinutes(2);
            return time;
        }

        #endregion

    }

    internal static class ImmaShcha
    {
        // imma shcha
    }


}


