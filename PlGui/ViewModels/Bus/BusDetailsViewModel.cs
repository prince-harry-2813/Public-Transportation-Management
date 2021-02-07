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

        private LineTiming busStop;
        /// <summary>
        /// Hold Bus data 
        /// </summary>
        public LineTiming BusStop
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

    

        private bool _busValueIsReadOnly = true;
        public bool BusValueIsReadOnly
        {
            get => _busValueIsReadOnly;
            set
            {
                SetProperty(ref _busValueIsReadOnly, value);
            }
        }

        private bool busDetaisVisibilty = false;
        public bool BusDetaisVisibilty
        {
            get => busDetaisVisibilty;
            set
            {
                SetProperty(ref busDetaisVisibilty, value);
            }
        }
        


        private bool busStopVisibilty = false;
        public bool BusStopVisibilty
        {
            get => busStopVisibilty;
            set
            {
                SetProperty(ref busStopVisibilty, value);
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

            if (Bus != null)
            {
                BusDetaisVisibilty = true;
                BusStopVisibilty = false;
            }

            if (BusStop != null)
            {
                BusDetaisVisibilty = false;
                BusStopVisibilty = true;
            }

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
            if (!IsInEditMode)
            {
                IsInEditMode = true;
            }
            else
            {
                IsInEditMode = false;
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
            }

            if (sucssesFlag)
            {
                regionManager.Regions[StringNames.MainRegion].NavigationService.Journal.GoBack();
            }
        }

        #endregion

        #region Private Method

        #region INavigation Aware Implementation

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            // Initialize Interface 
            Bl = (IBL)navigationContext.Parameters.Where(pair => pair.Key == StringNames.BL).FirstOrDefault().Value ?? Bl;

            // Initialize View object 
            Bus = (BL.BO.Bus)navigationContext.Parameters.Where(pair => pair.Key == StringNames.SelectedBus).FirstOrDefault().Value ?? Bus;
            if (Bus != null)
            {
                BusDetaisVisibilty = true;
            }
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
                BusStop = (LineTiming)busStop;
                BusStopVisibilty = true;
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
            regionManager.Regions[StringNames.MainRegion].Remove(regionManager.Regions[StringNames.MainRegion].ActiveViews.FirstOrDefault());

        }

        #endregion

        #endregion
    }
}