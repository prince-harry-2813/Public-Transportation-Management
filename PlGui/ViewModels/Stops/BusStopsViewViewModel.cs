using BL.BLApi;
using BL.BO;
using PlGui.StaticClasses;
using PlGui.Views.Stops;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Unity;

namespace PlGui.ViewModels.Stops
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

        private ObservableCollection<Station> busStopsCollection;
        public ObservableCollection<Station> BusStopsCollection
        {
            get => busStopsCollection;
            set
            {
                SetProperty(ref busStopsCollection, value);
            }
        }

        public IBL Bl { get; set; }
        #endregion
        #region Service Decleration

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
            foreach (var item in Bl.GetAllBusStops())
            {
                BusStopsCollection.Add(item);
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
            unityContainer.RegisterType(typeof(object), typeof(AddBusStop), "AddBusStop");
            regionManager.RequestNavigate(StringNames.MainRegion, "AddBusStop");
        }

        private void UpdateBusStopButton(string commandParameter)
        {
            var parm = new NavigationParameters();
            parm.Add(StringNames.SelectedBusStop, ""/*TODO: Insert Selected Bus Stop from collection*/);
            unityContainer.RegisterType(typeof(object), typeof(BusStopDetails), "BusStopDetails");
            regionManager.RequestNavigate(StringNames.MainRegion, "BusStopDetails");
        }
        private void DeleteStationButton(string commandParameter)
        {
            /*TODO: Insert Selected Bus Stop from collection and delete it*/
            throw new NotImplementedException();
        }
        #endregion
    }
}
