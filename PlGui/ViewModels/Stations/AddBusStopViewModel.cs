using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using BL.BLApi;
using PlGui.StaticClasses;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Unity;

namespace PlGui.ViewModels.Stations
{
    public class AddBusStopViewModel : BindableBase
    {
        #region Service Deceleration

        private IRegionManager regionManager;
        private IUnityContainer unityContainer;

        #endregion

        #region Properties Declaration

        private BL.BO.Station busStop = new BL.BO.Station();
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
                stamfunc();
                SetProperty(ref busStop, value);

            }
        }

        public IBL Bl { get; set; }

        #endregion

        #region Command Decleration

        public ICommand AddBusStopButtonCommand { get; set; }
        public void stamfunc()
        {
            Console.WriteLine("staaasaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaam");
        }
        #endregion

        public AddBusStopViewModel(IRegionManager manager, IUnityContainer container , IBL bl)
        {
            #region Properties Initiaization 

            Bl = bl;

            #endregion

            AddBusStopButtonCommand = new DelegateCommand(AddBusStopButton);

            #region Service Initalization

            regionManager = manager;
            unityContainer = container;

            #endregion
        }

        #region Command Implementation

        public void AddBusStopButton()
        {
            try
            {
                Bl.AddStation(BusStop);
            }
            catch (Exception exception)
            {
                MessageBox.Show("Couldn't Add Bus Stop please check the new information");
            }
            finally
            {
                // GO Back to Bus Details Info 
                regionManager.RequestNavigate(StringNames.MainRegion, StringNames.BusStopsView);
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
            Bl = (IBL)navigationContext.Parameters.Where(pair => pair.Key == StringNames.BL).FirstOrDefault().Value;
            BusStop = (BL.BO.Station)navigationContext.Parameters.Where(pair => pair.Key == StringNames.SelectedBusStop).FirstOrDefault().Value;
        }
        #endregion
    }
}
