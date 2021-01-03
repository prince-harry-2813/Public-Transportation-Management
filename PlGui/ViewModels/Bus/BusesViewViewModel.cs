using System;
using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Unity;
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

        public BusesViewViewModel(IRegionManager manager , IUnityContainer container)
        {
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
    }
}
