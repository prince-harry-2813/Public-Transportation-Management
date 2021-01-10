using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using BL.BLApi;
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

        #region Properties Declaration
        
        private BL.BO.Line line;
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
        
        public IBL Bl { get; set; }

        #endregion

        #region Command Decleration

        public ICommand AddLineButtonCommand { get; set; }

        #endregion

        public AddLineViewModel(IRegionManager manager , IUnityContainer container)
        {
            #region Properties Initiaization
            #endregion

            AddLineButtonCommand = new DelegateCommand(AddLineButton);

            #region Service Initalization

            regionManager = manager;
            unityContainer = container;

            #endregion
        }
        
        #region Command Implementation

        private void AddLineButton()
        {
            try
            {
                Bl.AddLine(Line);
            }
            catch (Exception exception)
            {
                MessageBox.Show("Couldn't Add line please check the new information");
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
            Bl = (IBL)navigationContext.Parameters.Where(pair => pair.Key == StringNames.BL).FirstOrDefault().Value;\
            Line = (BL.BO.Line)navigationContext.Parameters.Where(pair => pair.Key == "Line").FirstOrDefault().Value;
        }
        #endregion
    }
}
