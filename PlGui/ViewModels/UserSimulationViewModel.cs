using BL.BLApi;
using BL.BO;
using PlGui.ViewModels.Bus;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace PlGui.ViewModels
{
    public class UserSimulationViewModel : BindableBase
    {
        #region Service Decleration

        public IBL Bl { get; set; }
        private IRegionManager regionManager;

        #endregion

        #region Properties Deceleration

        private int simulationHZ = 20;

        /// <summary>
        /// Simulation dispatching timer sec = Simulation time / Real sec 
        /// </summary>
        public int SimulationHZ
        {
            get => simulationHZ; set
            {
                SetProperty(ref simulationHZ, value);
            }
        }

        private ObservableCollection<LineTiming> lineTimings = new ObservableCollection<LineTiming>();

        /// <summary>
        /// Simulation dispatching timer sec = Simulation time / Real sec 
        /// </summary>
        public ObservableCollection<LineTiming> LineTimings
        {
            get => lineTimings; set
            {
                value.OrderBy(timing => timing.ArrivingTime);
                SetProperty(ref lineTimings, value);
            }
        }


        private bool isSimulationRuning;
        /// <summary>
        /// Determines whether simulation started to run
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
        /// Determines whether simulation started to run
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

        private ObservableCollection<Line> linesOfStation = new ObservableCollection<Line>();

        public ObservableCollection<Line> LinesOfStation
        {
            get => linesOfStation;
            set
            {
                SetProperty(ref linesOfStation, value);
            }
        }

        private ObservableCollection<Station> stationCollection;
        public ObservableCollection<Station> StationCollection
        {
            get => stationCollection;
            set
            {
                value.Distinct();
                SetProperty(ref stationCollection, value);
            }
        }

        private Station station = new Station();

        public Station Station
        {
            get => station;
            set
            {
                if (value != null)
                {
                    var a = Bl?.getLinesOfStation(value.Code) ?? null; 
                    OfStation.Lines = a.Lines;
                    LinesOfStation.Clear();
                    foreach (var VARIABLE in OfStation.Lines)
                    {
                        LinesOfStation.Add(VARIABLE);
                    }
                }
                SetProperty(ref station, value);
            }
        }

        public BusDetailsViewModel BusDetailsDataContext { get; set; }

        #region Private Memebers

        private BackgroundWorker clockWorker;
        private BackgroundWorker getStationsWorker;

        private LinesOfStation OfStation = new LinesOfStation();

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

            #region Get stations Worker
            StationCollection = new ObservableCollection<Station>();
            var listLS = Bl.GetAllLinesStation().ToList();
            getStationsWorker = new BackgroundWorker();
            getStationsWorker.DoWork += (sender, args) =>
            {
                foreach (var item in Bl.GetStationBy(s => listLS.Any(ls => ls.Station.Code == s.Code)))
                {
                    getStationsWorker.ReportProgress(0, item);
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
            #endregion

            #region Line timing list initialization 

            #endregion

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

            #region Stop Region

            if (parameter.Equals("Stop") && clockWorker != null)
            {
                Bl.StopSimulator();
                clockWorker.CancelAsync();
                clockWorker.Dispose();
                IsSimulationRuning = false;
            }

            #endregion

            #region Start 

            if (parameter.Equals("Start"))
            {
                if (Station is null)
                {
                    MessageBox.Show("Please choose station before statrting the simulator");
                    return;
                }
                clockWorker = new BackgroundWorker();
                clockWorker.WorkerReportsProgress = true;
                clockWorker.WorkerSupportsCancellation = true;
                clockWorker.DoWork += (sender, args) =>
                {
                    Bl.StartSimulator(SimulationStartTime, SimulationHZ, span => SimulationStartTime = span);
                    Bl.SetStationPanel(station.Code , timing =>
                    {
                        // get line timing instance from list 
                       var a =  (LineTiming)LineTimings.Where(lineTiming => lineTiming.LineID == timing.LineID);
                           // if therisnt add new instance 
                       if (a is null)
                       {
                           a = new LineTiming();
                           a = timing;
                           LineTimings.Add(a);
                        }
                       // update the item
                       else
                       {
                           a = timing;
                       }

                       // sort 
                       LineTimings.OrderBy(lineTiming => lineTiming.ArrivingTime);
                       // update view 
                       RaisePropertyChanged("LineTimings");
                       // update last bus 
                       BusDetailsDataContext.BusStop = LineTimings.LastOrDefault();
                    });
                };

                IsSimulationRuning = true;
                clockWorker.RunWorkerAsync();
            }
            
            #endregion
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

