using BL.BLApi;
using BL.BO;
using PlGui.StaticClasses;
using PlGui.Views.Lines;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Unity;

namespace PlGui.ViewModels.Lines
{
    public class LinesViewViewModel : BindableBase , INavigationAware
    {
        #region Properties Deceleration

        private BL.BO.Line line;

        public BL.BO.Line Line
        {
            get => line;
            set
            {
                SetProperty(ref line, value);
            }
        }

        private ObservableCollection<Line> linesCollection;
        public ObservableCollection<Line> LinesCollection
        {
            get => linesCollection;
            set
            {
                SetProperty(ref linesCollection, value);
            }
        }

        public IBL Bl { get; set; }
        #endregion

        #region Service Deceleration

        private IRegionManager regionManager;
        private IUnityContainer unityContainer;

        #endregion

        #region Command Declaration

        public ICommand AddLineButtonCommand { get; set; }
        public ICommand UpdateLineButtonCommand { get; set; }
        public ICommand ChooseLineButtonCommand { get; set; }
        public ICommand DeleteLineButtonCommand { get; set; }

        #endregion
        public LinesViewViewModel(IRegionManager manager, IUnityContainer container)
        {
            #region Properties Deceleration

            Bl = BLFactory.GetIBL();
            LinesCollection = new ObservableCollection<Line>();
            RefreshView();

            #endregion

            #region Service Initialization

            regionManager = manager;
            unityContainer = container;

            #endregion

            #region Command Initialization

            AddLineButtonCommand = new DelegateCommand(AddLineButton);
            UpdateLineButtonCommand = new DelegateCommand<string>(UpdateLineButton);
            ChooseLineButtonCommand = new DelegateCommand(ChooseLine);
            DeleteLineButtonCommand = new DelegateCommand<string>(DeleteLineCommand);

            #endregion
        }


        #region Command Implementation


        private void ChooseLine()
        {
            UpdateLineButton(" ");
        }


        /// <summary>
        /// Navigate to Add line view 
        /// </summary>
        private void AddLineButton()
        {
            var parm = new NavigationParameters();
            parm.Add("Line", Line);
            unityContainer.RegisterType(typeof(object), typeof(AddLine), "AddLine");
            regionManager.RequestNavigate(StringNames.MainRegion, "AddLine" , parm);
        }

        /// <summary>
        /// Navigate to line Details 
        /// </summary>
        /// <param name="commandParameter"></param>
        private void UpdateLineButton(string commandParameter)
        {
            var parm = new NavigationParameters(commandParameter);
            parm.Add("Line", Line);
            unityContainer.RegisterType(typeof(object), typeof(LineDetails), "LineDetails");
            regionManager.RequestNavigate(StringNames.MainRegion, "LineDetails" , parm);
        }

        private void DeleteLineCommand(string parameter)
        {
            if (line == null)
            {
                return;
            }

            Bl.DeleteLine(Line);

            RefreshView();
        }

        #endregion

        #region Private Methods

        private void RefreshView()
        {
            LinesCollection = new ObservableCollection<Line>();
            LinesCollection.Clear();
            foreach (var variable in Bl.GetLinesBy(l=>l.IsActive||!l.IsActive))
            {
                LinesCollection.Add(variable);
               
            }
            
        }

        #region INavigation Aware Implementation 

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            RefreshView();
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return  true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            regionManager.Regions[StringNames.MainRegion]
                .Remove(regionManager.Regions[StringNames.MainRegion].ActiveViews.FirstOrDefault());
        }
        #endregion

        #endregion
    }
}
