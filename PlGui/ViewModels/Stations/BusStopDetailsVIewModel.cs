using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using BL.BLApi;
using PlGui.StaticClasses;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace PlGui.ViewModels.Stations
{
    public class BusStopDetailsViewModel : BindableBase
    {

        #region Properties Declaraion

        private BL.BO.Station busStop;
        /// <summary>
        /// Hold Bus data 
        /// </summary>
        public BL.BO.Station BusStop
        {
            get
            {
                return busStop;
            }
            set
            {
                SetProperty(ref busStop, value);
            }
        }
        //return Bl.GetBus(1234456 /*TODO: Implement here bus licence Number from the user control sender */)

        private PropertyDetails selectedItem;
        public PropertyDetails SelectedItem
        {
            get => selectedItem;
            set
            {
                SetProperty(ref selectedItem, value);
            }
        }

        private ObservableCollection<PropertyDetails> lbItemSource;

        public ObservableCollection<PropertyDetails> LbItemSource
        {
            get => lbItemSource;
            set
            {
                SetProperty(ref lbItemSource, value);
            }
        }

        public int Id { get; set; }

        #region Private Members

        private int BusStopId;

        #endregion

        #endregion

        #region Service Decleration

        public IBL Bl { get; set; }

        #endregion

        #region Command decleration

        public ICommand BusDetailsButtonCommand { get; set; }

        #endregion

        public BusStopDetailsViewModel()
        {
            #region Command Implemetaion

            BusDetailsButtonCommand = new DelegateCommand<string>(LineDetailsButton);

            #endregion

            #region Properties Implementation

            InsertBusPropertiesToCollection(BusStop);
            SelectedItem = LbItemSource.FirstOrDefault();

            #endregion
        }

        #region Command Implementation

        private void LineDetailsButton(string commandParameter)
        {
            switch (commandParameter)
            {
                case "Edit":
                    Bl.UpdateStation(BusStop);
                    break;
                case "Remove":
                    Bl.DeleteStation(BusStop);
                    break;
            }
        }

        #endregion

        #region Interface Implementaion

        /// <summary>
        /// Return True If it Possible To Navigate 
        /// </summary>
        /// <param name="navigationContext"></param>
        /// <returns></returns>
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
            BusStop = (BL.BO.Station)navigationContext.Parameters.Where(pair => pair.Key == StringNames.SelectedBusStop).FirstOrDefault().Value;
        }

        #endregion

        #region Private Methoed

        private void InsertBusPropertiesToCollection(BL.BO.Station busStop)
        {
            LbItemSource.Clear();
            foreach (PropertyInfo VARIABLE in busStop.GetType().GetProperties())
            {
                LbItemSource.Add(new PropertyDetails()
                {
                    PropertyType = VARIABLE.PropertyType,
                    PropertyName = VARIABLE.Name,
                    Propertyvalue = VARIABLE.GetConstantValue().ToString()
                });
            }
        }

        private void InsertCollectionToBus()
        {
            foreach (var VARIABLE in busStop.GetType().GetProperties())
            {
                var property = LbItemSource.Where(details => details.PropertyName == VARIABLE.Name);

                VARIABLE.SetValue(busStop, property.GetEnumerator().Current.Propertyvalue);
            }
        }

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

        public string Propertyvalue
        {
            get => propertyValue;
            set
            {
                SetProperty(ref propertyValue, value);
            }
        }
    }
}
