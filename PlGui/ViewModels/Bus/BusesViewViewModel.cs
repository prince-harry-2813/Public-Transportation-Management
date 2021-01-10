using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using BL.BLApi;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Unity;
using BL;
using PlGui.StaticClasses;
using AddBus = PlGui.Views.Bus.AddBus;
using BusDetails = PlGui.Views.Bus.BusDetails;

namespace PlGui.ViewModels.Bus
{
    public class BusesViewViewModel : BindableBase
    {
        #region Service Decleration

        private IRegionManager regionManager;
        private IUnityContainer unityContainer;

        #endregion

        #region Command Decleration

        /// <summary>
        /// Add bus command 
        /// </summary>
        public ICommand MainBusButtonCommand { get; set; }
        public ICommand OnMouseDoubleClick { get; set; }
        public ICommand ChooseBusButtonCommand { get; set; }
        public ICommand RefuleBusButtonCommand { get; set; }

        #endregion

        #region Properties Decleration

        private BL.BO.Bus selectedItem;
        /// <summary>
        /// Selected Bus Form Buses list In list Box 
        /// </summary>
        public BL.BO.Bus SelectedItem
        {
            get
            {
                return selectedItem;
            }
            set
            {
                SetProperty(ref selectedItem, value);
            }
        }

        private ObservableCollection<BL.BO.Bus> lbItemSource;

        public ObservableCollection<BL.BO.Bus> LbItemSource
        {
            get
            {
                return lbItemSource;
            }
            set
            {
                SetProperty(ref lbItemSource, value);
            }
        }

        public IBL Bl { get; set; }

        #endregion

        public BusesViewViewModel(IRegionManager manager , IUnityContainer container)
        {
            #region Properties Decleration

            LbItemSource = (ObservableCollection<BL.BO.Bus>)Bl.GetAllBuses();
            SelectedItem = lbItemSource.FirstOrDefault();

            #endregion

            #region Service Initialization

            regionManager = manager;
            unityContainer = container;
            
            #endregion

            #region Command Initialization

            MainBusButtonCommand = new DelegateCommand<string>(MainBusButton);
            OnMouseDoubleClick = new DelegateCommand(LicensNumberLabelClicked);
            ChooseBusButtonCommand = new DelegateCommand(ChooseBusButton);
            RefuleBusButtonCommand = new DelegateCommand(RefuleBusButton);

            #endregion
        }

        #region Command Implementation

        /// <summary>
        /// Add new bus to the system
        /// </summary>
        private void MainBusButton(string commandParameter)
        {
            if (commandParameter ==  "Add")
            {
                unityContainer.RegisterType(typeof(object), typeof(AddBus), "AddBus");
                regionManager.RequestNavigate(StringNames.MainRegion, "AddBus");
                return;
            }
            NavigationParameters param = new NavigationParameters(commandParameter);
            param.Add(StringNames.BL , Bl);
            param.Add(StringNames.SelectedBus , SelectedItem);
            
            unityContainer.RegisterType(typeof(object) , typeof(BusDetails) , "BusDetails");
            regionManager.RequestNavigate(StringNames.MainRegion , "BusDetails" , param);
        }

        /// <summary>
        /// On label button click that excute 
        /// </summary>
        private void LicensNumberLabelClicked()
        {
            throw new NotImplementedException();
        }

        private void ChooseBusButton()
        {
            throw new NotImplementedException();
        }

        private void RefuleBusButton()
        {
            throw new NotImplementedException();
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
        }
        
        #endregion
    }
}
