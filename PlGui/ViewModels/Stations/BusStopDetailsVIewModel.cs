using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using BL.BLApi;
using PlGui.StaticClasses;
using PlGui.ViewModels.Lines;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace PlGui.ViewModels.Stations
{
    public class BusStopDetailsViewModel : BindableBase , INavigationAware
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


        public int Id { get; set; }

        #region Private Members

        private int BusStopId;

        #endregion

        #endregion

        #region Service Decleration

        public IBL Bl { get; set; }

        #endregion

        #region Command decleration

        public ICommand BusStopDetailsButtonCommand { get; set; }

        #endregion

        public BusStopDetailsViewModel(IBL bl)
        {

            Bl = bl;

            #region Command Implemetaion

            BusStopDetailsButtonCommand = new DelegateCommand<string>(LineDetailsButton);

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
                   EditBusStop();
                    break;
                case "Remove":
                    Bl.DeleteStation(BusStop);
                    break;
            }
        }

        private void EditBusStop()
        {
            if (!IsInEditMode)
            {
                IsInEditMode = true;
            }
            else
            {
                if (BusStop == null)
                {
                    return;
                }
                Bl.UpdateStation(BusStop);
                IsInEditMode = false;
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
            Bl = (IBL)navigationContext.Parameters.Where(pair => pair.Key == StringNames.BL).FirstOrDefault().Value ?? Bl;

            // Initialize View object 
            BusStop = (BL.BO.Station)navigationContext.Parameters.Where(pair => pair.Key == StringNames.SelectedBusStop).FirstOrDefault().Value ?? BusStop;
        }

        #endregion

        #region Private Methoed

        #endregion
    }
}
