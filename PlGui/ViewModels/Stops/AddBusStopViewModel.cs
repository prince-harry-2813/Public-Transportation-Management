using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using BL.BLApi;
using PlGui.StaticClasses;
using Prism.Regions;
using Unity;

namespace PlGui.ViewModels.Stops
{
    public class AddBusStpoViewModel : BindableBase
    {
        #region Service Decleration

        private IRegionManager regionManager;
        private IUnityContainer unityContainer;

        #endregion

        #region Properties Declaration

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

        public IBL Bl { get; set; }

        #endregion

        #region Command Decleration

        public ICommand AddBusStopButtonCommand { get; set; }

        #endregion

        public AddBusStpoViewModel(IRegionManager manager, IUnityContainer container)
        {
            #region Properties Initiaization
            #endregion

            AddBusStopButtonCommand = new DelegateCommand(AddBusStopButton);

            #region Service Initalization

            regionManager = manager;
            unityContainer = container;

            #endregion
        }

        #region Command Implementation

        private void AddBusStopButton()
        {
            try
            {
                Bl.AddBusStop(BusStop);
            }
            catch (Exception exception)
            {
                MessageBox.Show("Couldn't Add Bus Stop please check the new information");
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
