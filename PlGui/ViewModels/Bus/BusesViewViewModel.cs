using BL.BLApi;
using PlGui.StaticClasses;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Unity;
using BusDetails = PlGui.Views.Bus.BusDetails;

namespace PlGui.ViewModels.Bus
{
    public class BusesViewViewModel : BindableBase, INavigationAware
    {
        #region Service Decleration

        private IRegionManager regionManager;
        private IUnityContainer unityContainer;
        public IBL Bl { get; set; }

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



        #region private Member

        private BackgroundWorker refuleBusWorker;

        #endregion
        #endregion

        public BusesViewViewModel(IRegionManager manager, IUnityContainer container, IBL bl)
        {
            #region Service Initialization

            Bl = bl;
            regionManager = manager;
            unityContainer = container;

            #endregion

            #region Properties Decleration

            LbItemSource = new ObservableCollection<BL.BO.Bus>();
            RefreshView();

            SelectedItem = lbItemSource.FirstOrDefault();

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
        /// Open Add Bus Window on Add clicked
        /// 
        /// </summary>
        private void MainBusButton(string commandParameter)
        {
            NavigationParameters param = new NavigationParameters(commandParameter);
            param.Add(StringNames.BL, Bl);
            param.Add(StringNames.SelectedBus, SelectedItem);

            if (commandParameter == "Add")
            {
                regionManager.RequestNavigate(StringNames.MainRegion, "AddBus", param);
                return;
            }

            var flag = unityContainer.IsRegistered(typeof(BusDetails), StringNames.BusDetails);
            Debug.Print(flag.ToString());
            regionManager.RequestNavigate(StringNames.MainRegion, StringNames.BusDetails, param);
        }

        /// <summary>
        /// On label button click that execute
        ///  Open Bus Details window 
        /// </summary>
        private void LicensNumberLabelClicked()
        {
            MainBusButton("Update");
        }

        /// <summary>
        /// Open Bus Details window 
        /// </summary>
        private void ChooseBusButton()
        {
            MainBusButton("Update");
        }

        /// <summary>
        /// Reful Bus to Maximum 1200 liter
        /// </summary>
        private void RefuleBusButton()
        {
            if (refuleBusWorker != null)
            {
                refuleBusWorker.CancelAsync();
            }

            SelectedItem.FuelStatus = 1200;
            refuleBusWorker = new BackgroundWorker();
            refuleBusWorker.WorkerSupportsCancellation = true;
            refuleBusWorker.DoWork += (sender, args) =>
            {
                Bl.UpdateBus(SelectedItem);
            };
            refuleBusWorker.RunWorkerCompleted += (sender, args) =>
            {
                MessageBox.Show("Fuel Added Succsefuly"
                    , "BRW", MessageBoxButton.OK
                    , MessageBoxImage.Information);
            };
            RefreshView();
        }

        #endregion

        #region External Methoeds

        private void RefreshView()
        {
            LbItemSource.Clear();
            foreach (var VARIABLE in Bl.GetAllBuses())
            {
                LbItemSource.Add(VARIABLE);
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
            Bl = (IBL)navigationContext.Parameters.Where(pair => pair.Key == StringNames.BL).FirstOrDefault().Value ?? Bl;
            RefreshView();
        }

        #endregion
    }
}
