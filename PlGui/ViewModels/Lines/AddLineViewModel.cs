using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Unity;

namespace PlGui.ViewModels.Lines
{
    public class AddLineViewModel : BindableBase
    {
        #region Service Decleration

        private IRegionManager regionManager;
        private IUnityContainer unityContainer;

        #endregion

        
        public AddLineViewModel(IRegionManager manager , IUnityContainer container)
        {
            #region Service Initalization

            regionManager = manager;
            unityContainer = container;

            #endregion

         
        }

        
    }
}
