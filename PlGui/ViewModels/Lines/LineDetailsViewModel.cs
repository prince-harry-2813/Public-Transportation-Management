using BL.BLApi;
using BL.BO;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

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

        private Station stationToAdd;
        public Station StationToAdd
        {
            get { return stationToAdd; }
            set { SetProperty(ref stationToAdd, value); }
        }

        private List<Station> stations;
        public List<Station> Stations
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

        //private Area enumsArea= new Area();
        //public Area EnumsArea
        //{
        //    get { return enumsArea; }
        //    set {  SetProperty(ref enumsArea, value); }
        //}

        //public ObservableCollection<string> Areas = new ObservableCollection<string>(){
        // "General",
        //"South",
        //"Jerusalem",
        //"Center",
        //"North"
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


        public int LicenseNumber { get; set; }
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

            #region Command Implementation

            BusDetailsButtonCommand = new DelegateCommand<string>(LineDetailsButton);
            AddLineStationButtonCommand = new DelegateCommand<string>(AddLineStationButton);
            #endregion

            #region Properties Implementation

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

        private void AddLineStationButton(string commandParameter) {
        
        }

        #endregion

        #region Interface Implementation

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
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

