using BL.BLApi;
using BL.BO;
using PlGui.StaticClasses;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Unity;

namespace PlGui.ViewModels.Lines
{
    public class AddLineViewModel : BindableBase
    {
        #region Service Deceleration

        private IRegionManager regionManager;
        private IUnityContainer unityContainer;

        #endregion

        #region Properties Declaration

        private BL.BO.Line line = new BL.BO.Line();
        /// <summary>
        /// Hold Bus data 
        /// </summary>
        public BL.BO.Line Line
        {
            get
            {
                return line;
            }
            set
            {
                SetProperty(ref line, value);
            }
        }

        private BackgroundWorker updaeteWorker;
        private BackgroundWorker addWorker;

        public BL.BLApi.IBL Bl { get; set; }

        #endregion

        #region Command Deceleration

        public ICommand EneterKeyCommand { get; set; }
        public ICommand AddLineButtonCommand { get; set; }
        public ICommand UpdateLineButtonCommand { get; set; }

        #endregion

        public AddLineViewModel(IRegionManager manager, IUnityContainer container , IBL bl)
        {
            #region Service Init

            Bl = bl;

            #endregion

            #region Properties Initialization

            //Line = new Line()
            //{
            //    Area = Area.Center,
            //    Code = 0,
            //    Id = Bl.GetLineBy(l => l.IsActive || !l.IsActive).Count()

            //};
            #endregion

            #region Command Initialization

            EneterKeyCommand = new DelegateCommand(EnterKey);
            AddLineButtonCommand = new DelegateCommand(AddLineButton);
            UpdateLineButtonCommand = new DelegateCommand(UpdateLineButton);

            #endregion
            #region Service Initialization

            regionManager = manager;
            unityContainer = container;

            #endregion
        }

        #region Command Implementation

        private void UpdateLineButton()
        {
            if (updaeteWorker != null)
            {
                updaeteWorker.CancelAsync();
            }
            updaeteWorker = new BackgroundWorker();
            updaeteWorker.WorkerSupportsCancellation = true;
            updaeteWorker.DoWork += (sender, args) =>
            {
                Bl.UpdateLine(Line);
            };
            updaeteWorker.RunWorkerAsync();
        }


        private void AddLineButton()
        {
            try
            {
                if (addWorker != null)
                {
                    addWorker.CancelAsync();
                }

                addWorker = new BackgroundWorker();
                addWorker.DoWork += (sender, args) =>
                {
                    Line.FirstStation.LineStationIndex = 0;
                    Line.LastStation.LineStationIndex = Line.Stations.Count();
                    Bl.AddLine(Line);
                };
                addWorker.RunWorkerAsync();

            }
            catch (Exception exception)
            {
                MessageBox.Show($"Couldn't Add line please check the new information \n {exception.Message}" );
            }
            finally
            {
                // GO Back to Bus Details Info 
                regionManager.RequestNavigate(StringNames.MainRegion,StringNames.LinesView);
            }
        }

        private void EnterKey()
        {
            AddLineButton();
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
            Line = (BL.BO.Line)navigationContext.Parameters.Where(pair => pair.Key == "Line").FirstOrDefault().Value;
        }
        #endregion
    }
}
