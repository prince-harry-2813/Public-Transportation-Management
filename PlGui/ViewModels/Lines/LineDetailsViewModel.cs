using BL.BLApi;
using BL.BO;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using PlGui.StaticClasses;

namespace PlGui.ViewModels.Lines
{
    public class LineDetailsViewModel : BindableBase, INavigationAware
    {

        #region Properties Declaration

        private BL.BO.Line line = new Line();
        /// <summary>
        /// Hold Bus data 
        /// </summary>
        public BL.BO.Line Line
        {
            get
            {
                return line;
            }
            set
            {
                SetProperty(ref line, value);
            }
        }

        private uint maxIndex;
        /// <summary>
        /// Max Index in line 
        /// </summary>
        public uint MaxIndex
        {
            get
            {
                return maxIndex;
            }
            set
            {
                SetProperty(ref maxIndex, value);
            }
        }

        private uint index = 10000;
        /// <summary>
        /// Max Index in line 
        /// </summary>
        public uint Index
        {
            get
            {
                return index;
            }
            set
            {
                if (value > Line?.Stations.Count())
                {
                    value = (uint)Line?.Stations.Count();
                }
                SetProperty(ref index, value);
            }
        }

        private uint canAddStation;
        /// <summary>
        /// true only if index and station was choosen  
        /// </summary>
        public uint CanAddStation
        {
            get
            {
                return canAddStation;
            }
            set
            {
                SetProperty(ref canAddStation, value);
            }
        }



        private Station stationToAdd;
        public Station StationToAdd
        {
            get { return stationToAdd; }
            set
            {

                lineStToAdj = new LineStation()
                {
                    Station = value,
                    IsActive = true,
                    LineId = Line.Id,
                    LineStationIndex = 0,
                    NextStation = 0,
                    PrevStation = 0
                };
                SetProperty(ref stationToAdd, value);
            }
        }

        private ObservableCollection<Station> stations;
        public ObservableCollection<Station> Stations
        {
            get { return stations; }
            set { SetProperty(ref stations, value); }
        }


        private LineStation lineStToAdj;
        public LineStation LineStToAdj
        {
            get { return lineStToAdj; }
            set { SetProperty(ref lineStToAdj, value); }
        }

        private Area enumsArea;


        private string areaString;
        public string AreaString
        {
            get { return areaString; }
            set
            {
                bool sucseed = Enum.TryParse(value, out enumsArea);
                if (sucseed)
                {
                    Line.Area = enumsArea;
                    RaisePropertyChanged("Line");
                }
                SetProperty(ref areaString, value);
            }
        }

        private ObservableCollection<string> areas = new ObservableCollection<string>();

        public ObservableCollection<string> Areas
        {
            get => areas;
            set
            {

                SetProperty(ref areas, value);
            }
        }

        //public ObservableCollection<string> Areas = new ObservableCollection<string>(){ 

        //// "General",
        ////"South",
        ////"Jerusalem",
        ////"Center",
        ////"North"
        //};

        private bool isInEditMode;
        /// <summary>
        /// Hold Bus data 
        /// </summary>
        public bool IsInEditMode
        {
            get
            {
                return isInEditMode;
            }
            set
            {
                SetProperty(ref isInEditMode, value);
            }
        }


        #region Private Members

        private int lineId;
        //private BackgroundWorker insertingSecondListWorker;

        #endregion

        #endregion

        #region Service Deceleration

        private IRegionManager regionManager;

        public IBL Bl { get; set; }

        #endregion

        #region Command deceleration

        public ICommand BusDetailsButtonCommand { get; set; }
        public ICommand AddLineStationButtonCommand { get; set; }
        #endregion

        public LineDetailsViewModel(IRegionManager manager, IBL bl)
        {
            #region Service Initialization

            regionManager = manager;
            Bl = bl;

            #endregion

            #region Command Initialization

            BusDetailsButtonCommand = new DelegateCommand<string>(LineDetailsButton);
            AddLineStationButtonCommand = new DelegateCommand(AddLineStationButton);
            Stations = new ObservableCollection<Station>();

            foreach (var item in Bl.GetAllStations())
            {
                Stations.Add(item);
            }

            #endregion

            #region Properties Initialization

            foreach (var item in Enum.GetNames(typeof(Area)))
            {
                Areas.Add(item);
            }

            #endregion
        }

        #region Command Implementation

        private void LineDetailsButton(string commandParameter)
        {
            if (Line.FirstStation == null)
            {
                Line.FirstStation = new LineStation();
                Line.FirstStation = Bl.GetAllLinesStationBy(ls => ls.LineId == Line.Id).OrderBy(o => o.LineStationIndex).First();
            }
            if (Line.LastStation == null)
            {
                Line.LastStation = new LineStation();
                Line.LastStation = Bl.GetAllLinesStationBy(ls => ls.LineId == Line.Id).OrderBy(o => o.LineStationIndex).Last();
            }
            switch (commandParameter)
            {
                case "Edit":
                    if (!IsInEditMode)
                    {
                        IsInEditMode = true;
                        break;
                    }
                    try
                    {
                        //  Line.Area = EnumsArea;
                        Bl.UpdateLine(Line);
                        break;
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.InnerException.Message + " couldn't update", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                        break;
                    }
                case "Delete":
                    try
                    {
                        Bl.DeleteLine(Line);
                        break;
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.InnerException.Message + " couldn't Delete", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                        break;
                    }
            }
        }

        private void AddLineStationButton()
        {
            if (Index == 10000 || StationToAdd == null||Line.Stations.Any(s=>s.Station.Code==LineStToAdj.Station.Code))
            {
                MessageBox.Show(".לא ניתן להוסיף תחנה זאת \n אנא בדוק הפרטים שהוזנו", "שגיאה בהוספת תחנה", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            LineStToAdj.LineStationIndex = (int)index;
            LineStToAdj.NextStation = Line.Stations.Count() == index ? 0 : Line.Stations.ElementAt((int)index).Station.Code;
            LineStToAdj.PrevStation = index == 0 ? 0 : Line.Stations.ElementAt((int)index-1).Station.Code;
            var stationsList = Line.Stations.ToList();
            stationsList.Insert((int)index, LineStToAdj);

            //updating the index number for each station in line trip
            foreach (var lStation in stationsList.Where(e=>e.Station.Code!=StationToAdd.Code))
            {
                if (lStation.LineStationIndex>=index)
                lStation.LineStationIndex+=1;
                if (lStation.LineStationIndex != 0)
                {
                    lStation.PrevStation = stationsList.ElementAt(lStation.LineStationIndex-1).Station.Code;
                }
                if (lStation.LineStationIndex < stationsList.Count() - 1 )
                {
                    lStation.NextStation = stationsList.ElementAt(lStation.LineStationIndex + 1).Station.Code;
                }
            }

            Line.Stations = stationsList;
            Bl.UpdateLine(Line);
            RaisePropertyChanged("Line");
        }

        #endregion

        #region Interface Implementation

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            regionManager.Regions[StringNames.MainRegion].Remove(regionManager.Regions[StringNames.MainRegion].ActiveViews.FirstOrDefault());
        }

        /// <summary>
        /// passing Parameters to the window 
        /// </summary>
        /// <param name="navigationContext"></param>
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            // Initialize Interface 
            // Bl = (IBL)navigationContext.Parameters.Where(pair => pair.Key == StringNames.BL).FirstOrDefault().Value;

            // Initialize View object 
            Line = (BL.BO.Line)navigationContext.Parameters.Where(pair => pair.Key == "Line").FirstOrDefault().Value;
            //  InsertBusPropertiesToCollection(Line);
        }

        #endregion

        #region Private Method

        //private void InsertBusStopCollection(Line line)
        //{
        ////    insertingSecondListWorker = new BackgroundWorker();
        ////    insertingSecondListWorker.WorkerSupportsCancellation = true;
        ////    insertingSecondListWorker.WorkerReportsProgress = true;
        ////    insertingSecondListWorker.DoWork += (sender, args) =>
        //        foreach (var item in line.Stations)
        //        {

        //        }
        //}

        //private void InsertBusPropertiesToCollection(BL.BO.Line line)
        //{
        //    if (line == null ) return;

        //    LbItemSource = new ObservableCollection<PropertyDetails>();
        //    foreach (PropertyInfo variable in line.GetType().GetProperties())
        //    {
        //        if (variable.PropertyType is IEnumerable<Station>)
        //        {
        //            continue;
        //        }

        //        LbItemSource.Add(new PropertyDetails()
        //        {
        //            PropertyType = variable.PropertyType,
        //            PropertyName = variable.Name,
        //            PropertyValue = variable.GetValue(obj: line, null).ToString()
        //        });
        //    }
        //}

        //private void InsertCollectionToBus()
        //{
        //    foreach (var variable in Line.GetType().GetProperties())
        //    {
        //        var property = LbItemSource.Where(details => details.PropertyName == variable.Name);

        //        variable.SetValue(Line, property.GetEnumerator().Current.PropertyValue);
        //    }
        //}

        #endregion
    }

    /// <summary>
    ///  Nested Class helper 
    /// </summary>
    public class PropertyDetails : BindableBase
    {
        public Type PropertyType { get; set; }

        private string propertyName;

        public string PropertyName
        {
            get => propertyName;
            set
            {
                SetProperty(ref propertyName, value);
            }
        }

        private string propertyValue;

        public string PropertyValue
        {
            get => propertyValue;
            set
            {
                SetProperty(ref propertyValue, value);
            }
        }
    }
}

