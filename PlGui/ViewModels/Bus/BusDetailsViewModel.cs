using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using BL.BLApi;
using PlGui.StaticClasses;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Unity;

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
            }}

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

        public BusDetailsViewModel(IRegionManager manager)
        {
            #region Service Initialization

            regionManager = manager;

            #endregion
            
            #region Command Implemetaion

            BusDetailsButtonCommand = new DelegateCommand<string>(BusDetailsButton);

            #endregion
            
            #region Properties Implementation

            InsertBusPropertiesToCollection(Bus);
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
            if(IsInEditMode)
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
                                    "Please try again", ex.Source, MessageBoxButton.OK
                        , MessageBoxImage.Error);
                    IsInEditMode = true;
                }
            }
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
            if (navigationContext == null) return;
            
            // Initialize Interface 
            Bl = (IBL)navigationContext.Parameters.Where(pair => pair.Key == StringNames.BL).FirstOrDefault().Value;

            // Initialize View object 
            Bus = (BL.BO.Bus)navigationContext.Parameters.Where(pair => pair.Key == StringNames.SelectedBus).FirstOrDefault().Value;
        }

        #endregion

        #region Private Methoed

        private void InsertBusPropertiesToCollection(BL.BO.Bus bus)
        {
            LbItemSource.Clear();
            foreach (PropertyInfo VARIABLE in bus.GetType().GetProperties())
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
            foreach (var VARIABLE in Bus.GetType().GetProperties())
            {
                var property = LbItemSource.Where(details => details.PropertyName == VARIABLE.Name);

                VARIABLE.SetValue(Bus, property.GetEnumerator().Current.Propertyvalue);
            }
        }

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
