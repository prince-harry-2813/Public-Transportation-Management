using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using BL.BLApi;
using PlGui.StaticClasses;
using PlGui.Views;
using PlGui.Views.Lines;
using PlGui.Views.Stops;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Unity;
using BusesView = PlGui.Views.Bus.BusesView;

namespace PlGui.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        #region Service Decleration

        private IRegionManager regionManager;
        private IUnityContainer unityContainer;

        #endregion
        
        #region Properties Decleration

        private string _title = "Login Window";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private IBL Bl;
       
        #endregion

        #region Command Decleration

        public ICommand NavigateCommand { get; set; }
        public ICommand NavigationPanelCommand { get; set; }

        #endregion

        public MainWindowViewModel(IRegionManager manager , IUnityContainer container)
        {
            #region Service Initialization

            regionManager = manager;
            unityContainer = container;

            #endregion

            #region Property Initialization

            Bl = BLFactory.GetIBL();

            #endregion

            #region Command Initialization

            NavigateCommand = new DelegateCommand<string>(Navigate);
            NavigationPanelCommand = new DelegateCommand<string>(NavigationPanel);
            regionManager.RegisterViewWithRegion(StringNames.MainRegion, typeof(StartPage));

            #endregion

            #region Event Mapping

            regionManager.Regions.CollectionChanged += (sender, args) =>
            {
              
            };
                
            #endregion
        }

        #region Command Implementaion

        public void Navigate(string commandParameter)
        {
            Type viewType = typeof(string);/*= Assembly.GetExecutingAssembly().GetType(commandParameter);*/

            switch (commandParameter)
            {
                case "ManagerLogin":
                    viewType = typeof(ManagerLogin);
                    break;
                case "UserSimulation":
                    viewType = typeof(UserSimulation);
                    break;
                case "BusesView":
                    viewType = typeof(BusesView);
                    break;
                case "LinesView":
                    viewType = typeof(LinesView);
                    break;
                case "BusStopsView":
                    viewType = typeof(BusStopsView);
                    break;
            }

            var parm = new NavigationParameters();
            parm.Add("BL" , Bl);


            unityContainer.Resolve(viewType, commandParameter);
            regionManager.RequestNavigate(StringNames.MainRegion , commandParameter , parm);
            
            foreach (var view in regionManager.Regions[StringNames.MainRegion].Views)
            {
                Debug.Print(view.ToString());
            }
        }

        /// <summary>
        /// Navigation Panel
        /// </summary>
        /// <param name="obj"></param>
        private void NavigationPanel(string commandParameter)
        {
            switch (commandParameter)
            {
                case "Back":
                    regionManager.Regions[StringNames.MainRegion].NavigationService.Journal.GoBack();
                    break;
                case "Forward":
                    regionManager.Regions[StringNames.MainRegion].NavigationService.Journal.GoForward();
                    break;
                case "Home":
                    regionManager.RequestNavigate(StringNames.MainRegion , StringNames.ManagerLogin);
                    break;
                case "Clone":
                    regionManager.Regions[StringNames.MainRegion].NavigationService.Journal.GoBack();
                    break;
            }
        }

        #endregion
    }

   
}
