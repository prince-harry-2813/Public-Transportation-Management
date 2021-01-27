using BL.BLApi;
using PlGui.StaticClasses;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace PlGui.ViewModels.Bus
{
    public class AddBusViewModel : BindableBase, INavigationAware
    {
        #region Service Initialization

        private IRegionManager regionManager;

        #endregion

        #region Properties Deceleration

        private DateTime displayDateEnd;

        public DateTime DisplayDateEnd
        {
            get => displayDateEnd;
            set
            {
                SetProperty(ref displayDateEnd, value);
            }
        }

        private DateTime registrationDate = DateTime.Now;

        public DateTime RegistrationDate
        {
            get => registrationDate;
            set
            {
                SetProperty(ref registrationDate, value);
            }
        }
        private DateTime displayDateStart;

        public DateTime DisplayDateStart
        {
            get => displayDateStart;
            set
            {
                SetProperty(ref displayDateStart, value);
            }
        }

        private string licenseNum = "";
        public string LicenseNum
        {
            get => licenseNum;
            set
            {
                SetProperty(ref licenseNum, value);
            }
        }

        public IBL Bl { get; set; }

        #endregion

        #region Command Decleration

        public ICommand EneterKeyCommand { get; set; }

        #endregion

        public AddBusViewModel(IRegionManager manager, IBL bl)
        {
            #region Service Initialization

            regionManager = manager;
            Bl = bl;

            #endregion

            #region Command Initialization

            EneterKeyCommand = new DelegateCommand(EnterKey);

            #endregion
        }


        #region Command Implementation

        /// <summary>
        ///  Validate input of text box to accept only numbers
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public bool CheckLicenseInput(string text)
        {
            return ((new Regex("[^0-9]+").IsMatch(text) || text.First() == ' ') /*&& !string.IsNullOrWhiteSpace(text)*/);
        }

        /// <summary>
        ///  Set date license registration
        ///  according to number of digits
        ///  When the text box lost focus check if the input length is more then 7 digits
        ///  when input length is 7 digits the date picker configure to show dates between 2000 - 2018
        ///  when input length is 8 digits the date picker configure to show dates between 2018 - now
        /// </summary>
        /// <returns></returns>
        public bool DisplayDateFocus()
        {
            if (LicenseNum.Length < 7 || !new Regex("^[0-9]*$").IsMatch(licenseNum)  /*&& !licenseNumBox.Focusable*/)
            {
                LicenseNum = String.Empty;
                MessageBox.Show("Please enter valid number, must contain at least 7 digits");

                return false;
            }
            else if (LicenseNum.Length == 7)
            {
                DisplayDateEnd = new DateTime(2017, 12, 31);
                DisplayDateStart = new DateTime(2000, 1, 1);
                return true;
            }
            else if (LicenseNum.Length == 8)
            {
                DisplayDateStart = new DateTime(2018, 01, 01);
                DisplayDateEnd = DateTime.Now;
                return true;
            }

            return true;
        }

        /// <summary>
        /// When Enter key is pressed add bus to system
        /// </summary>
        private void EnterKey()
        {
            try
            {
                if (LicenseNum.Length > 6 && LicenseNum.Length < 9 && RegistrationDate != null)
                {
                    Bl.AddBus(new BL.BO.Bus()
                    {
                        LicenseNum = int.Parse(LicenseNum)
                        ,
                        RegisDate = RegistrationDate,
                        TotalKM = 0
                    });
                }
                else
                {
                    MessageBox.Show("Please Insert License number and Registration Date", "Properties fault",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (BL.BO.BadBusIdException ex)
            {
                MessageBox.Show("Failed to add new Bus \n " +
                                "Please check license number and registration date");
            }
            finally
            {
                // GO Back to Bus Details Info 
                regionManager.RequestNavigate(StringNames.MainRegion, StringNames.BusesView);
            }
        }

        #endregion

        #region Interface Implementation

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            LicenseNum = "";
            RegistrationDate = DateTime.Now;
        }

        /// <summary>
        /// passing Parameters to the window 
        /// </summary>
        /// <param name="navigationContext"></param>
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            Bl = (IBL)navigationContext.Parameters.Where(pair => pair.Key == StringNames.BL).FirstOrDefault().Value;
        }
        #endregion
    }
}
