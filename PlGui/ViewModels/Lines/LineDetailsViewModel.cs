using BL.BLApi;
using PlGui.StaticClasses;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using BL.BO;

namespace PlGui.ViewModels.Lines
{
    public class LineDetailsViewModel : BindableBase , INavigationAware
    {

        #region Properties Declaraion

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

        #endregion

        public LineDetailsViewModel(IRegionManager manager, IBL bl)
        {
            #region Service Initialization

            regionManager = manager;
            Bl = bl;

            #endregion

            #region Command Implementation

            BusDetailsButtonCommand = new DelegateCommand<string>(LineDetailsButton);

            #endregion

            #region Properties Implementation

            #endregion
        }

        #region Command Implementation

        private void LineDetailsButton(string commandParameter)
        {
            switch (commandParameter)
            {
                case "Edit":
                    var param = new NavigationParameters();
                    param.Add("Line", Line);
                    regionManager.RequestNavigate(StringNames.MainRegion, StringNames.AddLine, param);
                    break;
                case "Remove":

                    break;
            }
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
            Bl = (IBL)navigationContext.Parameters.Where(pair => pair.Key == StringNames.BL).FirstOrDefault().Value;

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
        //    foreach (PropertyInfo VARIABLE in line.GetType().GetProperties())
        //    {
        //        if (VARIABLE.PropertyType is IEnumerable<Station>)
        //        {
        //            continue;
        //        }

        //        LbItemSource.Add(new PropertyDetails()
        //        {
        //            PropertyType = VARIABLE.PropertyType,
        //            PropertyName = VARIABLE.Name,
        //            PropertyValue = VARIABLE.GetValue(obj: line, null).ToString()
        //        });
        //    }
        //}

        //private void InsertCollectionToBus()
        //{
        //    foreach (var VARIABLE in Line.GetType().GetProperties())
        //    {
        //        var property = LbItemSource.Where(details => details.PropertyName == VARIABLE.Name);

        //        VARIABLE.SetValue(Line, property.GetEnumerator().Current.PropertyValue);
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

