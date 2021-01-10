using System.Collections.ObjectModel;
using System.Windows.Input;
using BL.BLApi;
using BL.BO;
using PlGui.Views.Lines;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Unity;

namespace PlGui.ViewModels.Lines
{
    public class LinesViewViewModel : BindableBase
    {
        #region Properties Decleration

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
        #region Service Decleration

        private IRegionManager regionManager;
        private IUnityContainer unityContainer;

        #endregion

        #region Command Declaration

        public ICommand AddLineButtonCommand { get; set; }
        public ICommand UpdateLineButtonCommand { get; set; }

        #endregion
        public LinesViewViewModel(IRegionManager manager, IUnityContainer container)
        {
            #region Properties Decleration

            Bl = BLFactory.GetIBL();
            LinesCollection = (ObservableCollection<Line>)Bl.GetAllLines();

            #endregion

            #region Service Initalization

            regionManager = manager;
            unityContainer = container;

            #endregion

            #region Command Initialization

            AddLineButtonCommand = new DelegateCommand(AddLineButton);
            UpdateLineButtonCommand = new DelegateCommand<string>(UpdateLineButton);

            #endregion
        }

        #region Command Implementation

        private void AddLineButton()
        {
            var parm = new NavigationParameters();
            parm.Add("Line", Line);
            unityContainer.RegisterType(typeof(object), typeof(AddLine), "AddLine");
            regionManager.RequestNavigate(StringNames.MainRegion, "AddLine");
        }

        private void UpdateLineButton(string commandParameter)
        {
            var parm = new NavigationParameters();
            parm.Add("Line", Line);
            unityContainer.RegisterType(typeof(object), typeof(LineDetails), "LineDetails");
            regionManager.RequestNavigate(StringNames.MainRegion, "LineDetails");
        }

        #endregion
    }
}
