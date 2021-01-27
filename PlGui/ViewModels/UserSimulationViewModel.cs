﻿using BL.BLApi;
using PlGui.ViewModels.Bus;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;

namespace PlGui.ViewModels
{
    public class UserSimulationViewModel : BindableBase
    {
        #region Service Decleration

        public IBL Bl { get; set; }
        private IRegionManager regionManager;

        #endregion

        #region Properties Decleration

        private int simulationHZ;

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

        public BusDetailsViewModel BusDetailsDataContext { get; set; }

        #region Private Memebers

        private BackgroundWorker clockWorker;

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
                clockWorker.CancelAsync();
                IsSimulationRuning = false;
            }

            if (parameter.Equals("Start"))
            {
                clockWorker = new BackgroundWorker();
                clockWorker.WorkerReportsProgress = true;
                clockWorker.WorkerSupportsCancellation = true;
                clockWorker.DoWork += (sender, args) =>
                {
                    Bl.StartSimulator(SimulationStartTime, SimulationHZ, UpdateTime);

                };

                clockWorker.ProgressChanged += (sender, args) =>
                {
                    SimulationStartTime = (TimeSpan)args.UserState;
                };

                IsSimulationRuning = true;
                clockWorker.RunWorkerAsync();
            }

        }

        private void UpdateTime(TimeSpan obj)
        {
            clockWorker.ReportProgress(0, obj);
        }

        #endregion
    }
}

