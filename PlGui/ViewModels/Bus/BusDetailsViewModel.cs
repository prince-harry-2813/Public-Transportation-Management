using BL.BLApi;
using BL.BO;
using PlGui.StaticClasses;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Input;

namespace PlGui.ViewModels.Bus
{
    public class BusDetailsViewModel : BindableBase, INavigationAware
    {
        #region Properties Declaration

        private bool isInEditMode;
        public bool IsInEditMode
        {
            get => isInEditMode;
            set
            {
                SetProperty(ref isInEditMode, value);
            }
        }

        private bool internalReadOnly;
        /// <summary>
        /// Set all user control property to read only 
        /// </summary>
        public bool InternalReadOnly
        {
            get => internalReadOnly;
            set
            {
                SetProperty(ref internalReadOnly, value);
            }
        }

        private bool buttonsVisibility = true;
        /// <summary>
        /// Set editing buttons visibility for manager only 
        /// </summary>
        public bool ButtonsVisibility
        {
            get => buttonsVisibility;
            set
            {
                SetProperty(ref buttonsVisibility, value);
            }
        }


        private string mainLabelContent = "Bus Details";
        /// <summary>
        /// Set editing buttons visibility for manager only 
        /// </summary>
        public string MainLabelContent
        {
            get => mainLabelContent;
            set
            {
                SetProperty(ref mainLabelContent, value);
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




            if (SelectedItem != null)
            {


                BusValueIsReadOnly = (SelectedItem.PropertyName == "LicenseNum"
                                    //|| SelectedItem.PropertyName == "RegisDate"
                                    || SelectedItem.PropertyName == "TotalKM"
                                    || SelectedItem.PropertyName == "KmOnLastTreatment"
                                    || SelectedItem.PropertyName == "LastTreatmentDate" 
                                  //  || InternalReadOnly
                                    );
                
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

        #region Private Method

        public void InsertBusPropertiesToCollection(object bus)
        {
            LbItemSource = new ObservableCollection<PropertyDetails>();
            foreach (PropertyInfo VARIABLE in bus.GetType().GetProperties())
            {
                if (VARIABLE.Name.Equals("IsActive"))
                    continue;
                LbItemSource.Add(new PropertyDetails()
                {
                    PropertyType = VARIABLE.PropertyType,
                    PropertyName = VARIABLE.Name,
                    BusValueIsReadOnly = (SelectedItem?.PropertyName == "LicenseNum"
                    || SelectedItem?.PropertyName == "RegisDate"
                    || SelectedItem?.PropertyName == "TotalKM"
                    || SelectedItem?.PropertyName == "KmOnLastTreatment"
                    || SelectedItem?.PropertyName == "LastTreatmentDate" || InternalReadOnly),
                    PropertyValue = VARIABLE.GetValue(obj: bus, null).ToString()
                });
            }
        }

        private void InsertCollectionToBus()
        {
            foreach (var VARIABLE in Bus.GetType().GetProperties())
            {
                if (VARIABLE.Name == "IsActive")
                    continue;

                var property = (PropertyDetails)LbItemSource.Where(details => details.PropertyName == VARIABLE.Name).FirstOrDefault();

                dynamic result = property.PropertyValue;
                if (property.PropertyType == typeof(int))
                {
                    int.TryParse(property.PropertyValue, out int value);
                    result = value;
                }

                if (property.PropertyType == typeof(DateTime))
                {
                    DateTime.TryParse(property.PropertyValue, out DateTime value);
                    result = value;
                }

                if (property.PropertyType == typeof(BusStatusEnum))
                {
                    BusStatusEnum.TryParse(property.PropertyValue, out BusStatusEnum value);
                    result = value;
                }

                if (property.PropertyType == typeof(uint))
                {
                    uint.TryParse(property.PropertyValue, out uint value);
                    result = value;
                }

                VARIABLE.SetValue(Bus, result);
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
            var a = navigationContext.Parameters.Where(pair => pair.Key == "InternalReadOnly")
            .FirstOrDefault().Value;
            InternalReadOnly = (a != null) ? (bool)a : internalReadOnly;


            var tmp = navigationContext.Parameters.Where(pair => pair.Key == "MainLabelContent")
                .FirstOrDefault().Value;
            MainLabelContent = (tmp != null) ? (string)tmp : MainLabelContent;

            var tmp1 = navigationContext.Parameters.Where(pair => pair.Key == "ButtonsVisibility")
                .FirstOrDefault().Value;
            ButtonsVisibility = (tmp1 != null) ? (bool)tmp1 : ButtonsVisibility;

            var busStop = navigationContext.Parameters.Where(pair => pair.Key == "BusStop")
                .FirstOrDefault().Value;
            if (busStop != null)
            {
                InsertBusPropertiesToCollection(busStop);
            }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            ButtonsVisibility = true;
            MainLabelContent = "";
            InternalReadOnly = false;
            LbItemSource = null;
            SelectedItem = null;
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

        private bool busValueIsReadOnly;

        public bool BusValueIsReadOnly
        {
            get => busValueIsReadOnly;
            set
            {
                SetProperty(ref busValueIsReadOnly, value);
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