using System.Windows.Input;
using PlGui.Views.Lines;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Unity;

namespace PlGui.ViewModels.Lines
{
    public class LinesViewViewModel : BindableBase
    {
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
            unityContainer.RegisterType(typeof(object), typeof(AddLine), "AddLine");
            regionManager.RequestNavigate(StringNames.MainRegion ,"AddLine");
        }

        private void UpdateLineButton(string commandParameter)
        {
            //TODO: Navigate to Line Details wpf;
        }

        #endregion
    }
}
