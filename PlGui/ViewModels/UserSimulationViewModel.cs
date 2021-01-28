﻿using BL.BLApi;
using PlGui.ViewModels.Bus;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using BL.BO;
using PlGui.ViewModels.Bus;
using PlGui.Views.Bus;
using Prism.Regions;

namespace PlGui.ViewModels
{
    public class UserSimulationViewModel : BindableBase
    {
        #region Service Decleration

        public IBL Bl { get; set; }
        private IRegionManager regionManager;

        #endregion

        #region Properties Decleration

        private int simulationHZ = 20;
        
        /// <summary>
        /// Simulation Dispatchering timer sec = Simulation time / Real sec 
        /// </summary>
        public int SimulationHZ
        {
            get => simulationHZ; set
            {
                SetProperty(ref simulationHZ, value);
            }
        }

        private bool isSimulationRuning;
        /// <summary>
        /// Determinds wether simulation started runing
        /// </summary>
        public bool IsSimulationRuning
        {
            get => isSimulationRuning;
            set
            {
                IsSimulationNotRuning = !value;
                SetProperty(ref isSimulationRuning, value);
            }
        }

        private bool isSimulationNotRuning = true;
        /// <summary>
        /// Determinds wether simulation started runing
        /// </summary>
        public bool IsSimulationNotRuning
        {
            get => isSimulationNotRuning;
            set
            {
                SetProperty(ref isSimulationNotRuning, value);
            }
        }

        private TimeSpan simulationStartTime = TimeSpan.Zero;
        /// <summary>
        /// Determinds wether simulation started runing
        /// </summary>
        public TimeSpan SimulationStartTime
        {
            get => simulationStartTime;
            set
            {
                SetProperty(ref simulationStartTime, value);
            }
        }

        private ObservableCollection<Station> stationCollection;
        public ObservableCollection<Station> StationCollection { get=> stationCollection;
            set
            {
                SetProperty(ref stationCollection, value);
            }
        }

        private Station station;

        public Station Station
        {
            get => station;
            set
            {
                SetProperty(ref station, value);
            }
        }

        public BusDetailsViewModel BusDetailsDataContext { get; set; }

        #region Private Memebers

        private BackgroundWorker clockWorker;
        private BackgroundWorker getStationsWorker;


        #endregion

        #endregion

        #region Command Decleration

        public ICommand StartStopToggelCommand { get; set; }

        #endregion

        public UserSimulationViewModel(IBL bl, IRegionManager manager)
        {
            #region Service Initialization

            Bl = bl;
            regionManager = manager;

            #endregion

            #region Properties Decleration

            StationCollection = new ObservableCollection<Station>();
            getStationsWorker = new BackgroundWorker();
            getStationsWorker.DoWork += (sender, args) =>
            {
                foreach (var item in Bl.GetAllStations())
                {
                    getStationsWorker.ReportProgress(0 , item);
                }
            };
            getStationsWorker.WorkerReportsProgress = true;
            getStationsWorker.ProgressChanged += (sender, args) =>
            {
                StationCollection.Add((Station)args.UserState);
            };
            getStationsWorker.RunWorkerAsync();

            Station = StationCollection?.FirstOrDefault();

            SimulationStartTime = DateTime.Now.TimeOfDay;

            // Call Navigate to bus details when region BusDetailsRegion added to the collection 
            regionManager.Regions.CollectionChanged += (sender, args) =>
            {
                if (regionManager.Regions.ContainsRegionWithName("BusDetailsRegion"))
                {
                    var param = new NavigationParameters()
                    {
                        {"InternalReadOnly", true},
                        {"ButtonsVisibility" , false },
                        {"MainLabelContent" , (object)"Last Arriving Bus" }
                    };
                    regionManager.RequestNavigate("BusDetailsRegion", "BusDetails", param);
                    var a = (UserControl)regionManager.Regions["BusDetailsRegion"].Views.FirstOrDefault();
                    //BusDetailsDataContext.InsertBusPropertiesToCollection(TTODO: ADD the dispaly properties object);
                }
            };

            #endregion

            #region Command Iplementation

            StartStopToggelCommand = new DelegateCommand<string>(StartStopToggel);

            #endregion
        }

        #region Command Implementation

        /// <summary>
        /// Start / Stop simulator and observe its watch
        /// </summary>
        /// <param name="parameter"></param>
        private void StartStopToggel(string parameter)
        {

            if (parameter.Equals("Stop") && clockWorker != null)
            {
                Bl.StopSimulator();
                clockWorker.CancelAsync();
                clockWorker.Dispose();
                IsSimulationRuning = false;
            }

            if (parameter.Equals("Start"))
            {
                clockWorker = new BackgroundWorker();
                clockWorker.WorkerReportsProgress = true;
                clockWorker.WorkerSupportsCancellation = true;
                clockWorker.DoWork += (sender, args) =>
                {
                         Bl.StartSimulator(SimulationStartTime , SimulationHZ , span => SimulationStartTime = span);
                };

                IsSimulationRuning = true;
                clockWorker.RunWorkerAsync();
            }
        }
        #endregion

        #region Public Methoed

        public void ComboBoxSelectionChanged()
        { 
            //Bl.SetStationPanel(Station.Code , );
            var param = new NavigationParameters()
            {
                {"InternalReadOnly", true},
                {"ButtonsVisibility" , false },
                {"MainLabelContent" , (object)"Last Arriving Bus" },
                {"BusStop" , null  } //TODO: Insert Last bus in station 
            };
            regionManager.RequestNavigate("BusDetailsRegion", "BusDetails", param);
        }

        #endregion
    }
}
