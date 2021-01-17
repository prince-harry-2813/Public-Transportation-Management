using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using BL.BLApi;
using BL.BO;
using PlGui.StaticClasses;
using Prism.Regions;
using QuickConverter.Tokens;

namespace PlGui.ViewModels.Bus
{
    public class BusDetailsViewModel : BindableBase, INavigationAware
    {
        #region Properties Declaraion

        private bool isInEditMode;
        public bool IsInEditMode
        {
            get => isInEditMode;
            set
            {
                SetProperty(ref isInEditMode, value);
            }
        }

        private BL.BO.Bus bus;
        /// <summary>
        /// Hold Bus data 
        /// </summary>
        public BL.BO.Bus Bus
        {
            get
            {
                return bus;
            }
            set
            {
                SetProperty(ref bus, value);
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

        private bool _busValueIsReadOnly = true;
        public bool BusValueIsReadOnly
        {
            get => _busValueIsReadOnly;
            set
            {
                SetProperty(ref _busValueIsReadOnly, value);
            }
        }

        public int LicenseNumber { get; set; }

        #region Private Members

        private int licenseNum;

        #endregion

        #endregion

        #region Service Decleration

        private IRegionManager regionManager;

        public IBL Bl { get; set; }

        #endregion

        #region Command decleration

        public ICommand BusDetailsButtonCommand { get; set; }

        #endregion

        public BusDetailsViewModel(IRegionManager manager, IBL bl)
        {
            #region Service Initialization

            regionManager = manager;
            Bl = bl;

            #endregion

            #region Command Implemetaion

            BusDetailsButtonCommand = new DelegateCommand<string>(BusDetailsButton);

            #endregion

            #region Properties Implementation

            LbItemSource = new ObservableCollection<PropertyDetails>();
            if (Bus != null)
            {
                InsertBusPropertiesToCollection(Bus);
            }
            
            SelectedItem = LbItemSource.FirstOrDefault();
          

            #endregion
        }

        #region Command Implementation

        private void BusDetailsButton(string commandParameter)
        {
            switch (commandParameter)
            {
                case "Edit":
                    EditButtonClicked();
                    break;
                case "Remove":
                    RemoveBusButtomClicked();
                    break;
            }
        }

        private void EditButtonClicked()
        {
            if (IsInEditMode)
            {
                IsInEditMode = true;
            }
            else
            {
                IsInEditMode = false;
                InsertCollectionToBus();
                try
                {
                    if (Bus != null)
                    {
                        Bl.UpdateBus(Bus);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to update Bus \n" +
                                    "Please try again" + ex.Source, ex.Source, MessageBoxButton.OK
                        , MessageBoxImage.Error);
                    IsInEditMode = true;
                }
            }
        }

        public void ListBoxSelectionChanged()
        {
            if (SelectedItem.PropertyName == "LicenseNum" 
                || SelectedItem.PropertyName == "RegisDate" 
                || SelectedItem.PropertyName == "TotalKM"
                || SelectedItem.PropertyName == "KmOnLastTreatment"
                || SelectedItem.PropertyName == "LastTreatmentDate")
            {
                BusValueIsReadOnly = false;
                return;
            }

            BusValueIsReadOnly = false;
        }
        
        private void RemoveBusButtomClicked()
        {
            bool sucssesFlag = true;

            try
            {
                Bl.DeleteBus(Bus);
            }

            catch (Exception e)
            {
                sucssesFlag = false;
                MessageBox.Show("Failed to delete bus " +
                                "Please try again", e.Source, MessageBoxButton.OK, MessageBoxImage.Error);
                Console.WriteLine(e);
                throw;
            }

            if (sucssesFlag)
            {
                regionManager.Regions[StringNames.MainRegion].NavigationService.Journal.GoBack();
            }
        }

        #endregion


        #region Private Method

        private void InsertBusPropertiesToCollection(BL.BO.Bus bus)
        {
            LbItemSource.Clear();
            foreach (PropertyInfo VARIABLE in bus.GetType().GetProperties())
            {
                if (VARIABLE.Name.Equals("isActive"))
                    continue;
                LbItemSource.Add(new PropertyDetails()
                {
                    PropertyType = VARIABLE.PropertyType,
                    PropertyName = VARIABLE.Name,
                    PropertyValue = VARIABLE.GetValue(obj: bus , null ).ToString()
                });
            }
        }

        private void InsertCollectionToBus()
        {
            foreach (var VARIABLE in Bus.GetType().GetProperties())
            {
                if (VARIABLE.Name == "isActive")
                    continue;
                
                var property = (PropertyDetails)LbItemSource.Where(details => details.PropertyName == VARIABLE.Name).FirstOrDefault();

                dynamic result = property.PropertyValue;
                if (property.PropertyType == typeof(int))
                {
                    int.TryParse(property.PropertyValue ,out int value);
                    result = value;
                }

                if (property.PropertyType == typeof(DateTime))
                {
                    DateTime.TryParse(property.PropertyValue , out DateTime value);
                    result = value;
                }

                if (property.PropertyType == typeof(BusStatusEnum))
                {
                     BusStatusEnum.TryParse(property.PropertyValue , out BusStatusEnum  value);
                     result = value;
                }

                if (property.PropertyType == typeof(uint))
                {
                    uint.TryParse(property.PropertyValue , out uint value);
                    result = value;
                }

                VARIABLE.SetValue(Bus,result);
            }
        }

        #region INavigation Aware Implementation
        
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            // Initialize Interface 
            Bl = (IBL)navigationContext.Parameters.Where(pair => pair.Key == StringNames.BL).FirstOrDefault().Value ?? Bl;

            // Initialize View object 
            Bus = (BL.BO.Bus)navigationContext.Parameters.Where(pair => pair.Key == StringNames.SelectedBus).FirstOrDefault().Value ?? Bus;
            if (Bus != null)
                InsertBusPropertiesToCollection(Bus);
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            
        }

        #endregion

        #endregion
    }

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