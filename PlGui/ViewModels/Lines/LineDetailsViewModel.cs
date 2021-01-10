using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using BL.BLApi;
using Prism.Regions;

namespace PlGui.ViewModels.Lines
{
    public class LineDetailsViewModel : BindableBase
    {

        #region Properties Declaraion

        private BL.BO.Line line;
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

        public int LicenseNumber { get; set; }
        #region Private Members

        private int lineId;

        #endregion

        #endregion

        #region Service Decleration

        public IBL Bl { get; set; }

        #endregion

        #region Command decleration

        public ICommand BusDetailsButtonCommand { get; set; }

        #endregion

        public LineDetailsViewModel()
        {
            #region Command Implemetaion

            BusDetailsButtonCommand = new DelegateCommand<string>(LineDetailsButton);

            #endregion
            #region Properties Implementation

            InsertBusPropertiesToCollection(Line);
            SelectedItem = LbItemSource.FirstOrDefault();

            #endregion
        }
        #region Command Implementation

        private void LineDetailsButton(string commandParameter)
        {
            switch (commandParameter)
            {
                case "Edit":
                    //TODO: Implement Edit Implementation
                    break;
                case "Remove":
                    //TODO: Add Remove Function 
                    break;
            }
        }

        #endregion
        #region Interface Implementaion

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
        }

        #endregion

        #region Private Methoed

        private void InsertBusPropertiesToCollection(BL.BO.Line line)
        {
            LbItemSource.Clear();
            foreach (PropertyInfo VARIABLE in line.GetType().GetProperties())
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
            foreach (var VARIABLE in Line.GetType().GetProperties())
            {
                var property = LbItemSource.Where(details => details.PropertyName == VARIABLE.Name);

                VARIABLE.SetValue(Line, property.GetEnumerator().Current.Propertyvalue);
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

