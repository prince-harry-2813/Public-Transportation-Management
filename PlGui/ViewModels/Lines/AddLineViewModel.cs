using BL.BLApi;
using BL.BO;
using PlGui.StaticClasses;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using PlGui.Views.Lines;
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
        

        private BL.BO.Line line = new BL.BO.Line()
        {
            LastStation = new LineStation()
            {
                Station = new Station()
            },
            FirstStation = new LineStation()
            {
                Station =  new Station()
            }
            ,Stations =  new List<LineStation>()
        };
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

        private List<Station> stationsList = new List<Station>();
        /// <summary>
        /// Combo box Choose sattion 
        /// </summary>
        public List<Station> StationsList
        {
            get => stationsList;
            set
            {
                SetProperty(ref stationsList, value);
            }
        }

        private Station firstStation = new Station();
        /// <summary>
        /// Combo box ChooseFirst station Selected Item
        /// </summary>
        public Station FirstStation
        {
            get => firstStation;
            set
            {
                Line.FirstStation.Station = FirstStation;
                SetProperty(ref firstStation, value);
            }
        }


        private Station lastStation = new Station();
        /// <summary>
        ///  Combo box ChooseFirst station Selected Item
        /// </summary>
        public Station LastStation
        {
            get => lastStation;
            set
            {
                Line.LastStation.Station = value;
                SetProperty(ref lastStation, value);
            }
        }

        private BackgroundWorker updaeteWorker;
        private BackgroundWorker addWorker;
        BackgroundWorker getStationsList ;

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
            Line.Id = bl.GetLinesBy(l => l.IsActive || !l.IsActive).Count()+1;
            #endregion

            #region Properties Initialization

            //Line = new Line()
            //{
            //    Area = Area.Center,
            //    Code = 0,
            //    Id = Bl.GetLineBy(l => l.IsActive || !l.IsActive).Count()

            //};
            #endregion

            #region WorkerInitialization

            getStationsList = new BackgroundWorker();
            getStationsList.WorkerSupportsCancellation = true;
            getStationsList.WorkerReportsProgress = true;
            getStationsList.DoWork += (sender, args) =>
            {
                foreach (var VARIABLE in Bl.GetAllStations())
                {
                    if (VARIABLE.isActive)
                    {
                        getStationsList.ReportProgress(0, VARIABLE);
                    }
                }
                int a = Bl.GetAllLinesStationBy(station => station.IsActive || !station.IsActive).Count() + 1; 
                getStationsList.ReportProgress(0 , a);
            };
            getStationsList.ProgressChanged += (sender, args) =>
            {
                if (args.UserState is int)
                {
                }
                else
                {
                    StationsList.Add(args.UserState as Station);
                }
            };

            getStationsList.RunWorkerCompleted += (sender, args) =>
            {
                StationsList.Sort((station, station1) => station.Code.CompareTo(station1.Code));
            };
            getStationsList.RunWorkerAsync();

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
                addWorker.WorkerSupportsCancellation = true;
                addWorker.DoWork += (sender, args) =>
                {
                    Line.FirstStation.Station = FirstStation;
                    Line.LastStation.Station = LastStation;
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
           regionManager.Regions[StringNames.MainRegion].Remove(regionManager.Regions[StringNames.MainRegion].ActiveViews.FirstOrDefault());
           addWorker.CancelAsync();
           updaeteWorker.CancelAsync();
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
