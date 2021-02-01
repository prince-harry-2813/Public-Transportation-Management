using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using BL.BLApi;
using BL.BO;
using PlGui.StaticClasses;
using PlGui.Views.Stations;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Unity;

namespace PlGui.ViewModels.Stations
{
    public class BusStopsViewViewModel : BindableBase
    {

        #region Properties Decleration

        private BL.BO.Station busStop;

        public BL.BO.Station BusStop
        {
            get => busStop;
            set
            {
                SetProperty(ref busStop, value);
            }
        }

        private ObservableCollection<Station> busStops = new ObservableCollection<Station>();
        public ObservableCollection<Station> BusStops
        {
            get => busStops;
            set
            {
                SetProperty(ref busStops, value);
            }
        }

        public IBL Bl { get; set; }
        #endregion

        #region Service Deceleration

        private IRegionManager regionManager;
        private IUnityContainer unityContainer;

        #endregion

        #region Command Declaration

        public ICommand AddBusStopButtonCommand { get; set; }
        public ICommand UpdateBusStopButtonCommand { get; set; }
        public ICommand DeleteStationButtonCommand { get; set; }
        #endregion

        /// <summary>
        /// Ctor 
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="container"></param>
        public BusStopsViewViewModel(IRegionManager manager, IUnityContainer container)
        {
            #region Properties Deceleration

            Bl = BLFactory.GetIBL();
            foreach (var item in Bl.GetAllStations())
            {
                BusStops.Add(item);
            }
            #endregion

            #region Service Initialization

            regionManager = manager;
            unityContainer = container;

            #endregion

            #region Command Initialization

            AddBusStopButtonCommand = new DelegateCommand(AddBusStopButton);
            UpdateBusStopButtonCommand = new DelegateCommand<string>(UpdateBusStopButton);
            DeleteStationButtonCommand = new DelegateCommand<string>(DeleteStationButton);
            #endregion
        }
        
        #region Command Implementation

        private void AddBusStopButton()
        {
            var parm = new NavigationParameters();
            parm.Add(StringNames.SelectedBusStop, BusStop);
            unityContainer.RegisterType(typeof(object), typeof(AddBusStop), "AddStation");
            regionManager.RequestNavigate(StringNames.MainRegion, "AddStation");
        }

        private void UpdateBusStopButton(string commandParameter)
        {
            if (BusStop == null)
            {
                return;
            }
            var parm = new NavigationParameters();
            parm.Add(StringNames.SelectedBusStop ,BusStop);
            unityContainer.RegisterType(typeof(object), typeof(BusStopDetails), "BusStopDetails");
            regionManager.RequestNavigate(StringNames.MainRegion, "BusStopDetails" , parm);
        }
        private void DeleteStationButton(string commandParameter)
        {
            if (BusStop == null )
            {
                    return;
            }
            Bl.DeleteStation(BusStop);
        }
        #endregion
    }
}
