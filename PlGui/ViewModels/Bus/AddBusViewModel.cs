using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using BL.BLApi;
using PlGui.StaticClasses;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace PlGui.ViewModels.Bus
{
    public class AddBusViewModel : BindableBase
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
        private DateTime registrationDate;

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
        
        private string licenseNum;
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

        public AddBusViewModel(IRegionManager manager)
        {
            #region Service Initialization

            regionManager = manager;

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
            return new Regex("[^0-9]+").IsMatch(text);
        }

        /// <summary>
        ///  Set date license registration
        ///  according to number of digits
        ///  When the text box lost focus check if the input length is more then 7 digits
        ///  when input length is 7 digits the date picker configure to show dates between 2000 - 2018
        ///  when input length is 8 digits the date picker configure to show dates between 2018 - now
        /// </summary>
        /// <returns></returns>
        public bool LicenseNumBoxLostFocus()
        {
            if (LicenseNum.Length < 7  /*//&& !licenseNumBox.Focusable*/)
            {
                LicenseNum = "";
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
                    MessageBox.Show("Please Insert License number and Registration Date", "Propertirs fault",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to add new Bus \n " +
                                "Please check license number and registration date");
            }
            finally
            {
                // GO Back to Bus Details Info 
                regionManager.RequestNavigate(StringNames.MainRegion, new Uri(StringNames.BusesView, UriKind.Absolute));
            }
        }

        #endregion

        ///// <summary>
        ///// BONUS! when enter key is pressed proceed 
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void enterKey(object sender, KeyEventArgs e)
        //{
        //    if (e.Key == Key.Enter)
        //    {
        //        if (licenseNumBox.Text.Length >= 7)
        //        {
        //            temp.SetFirstRegistration(RegistrationDate.DisplayDate);
        //            if (temp.SetLicenseNumber(licenseNumBox.Text))
        //            {
        //                ObservableCollection<Bus> carsCollection = BusCarsCollection.Instance.BusesCollection;

        //                foreach (Bus bus in carsCollection)
        //                {
        //                    if (bus.LicensNmuber == licenseNumBox.Text)
        //                    {
        //                        MessageBox.Show("This License number already exist in system");
        //                        return;
        //                    }
        //                }
        //                MessageBox.Show("Added successfully", "Bus Added", MessageBoxButton.OK, MessageBoxImage.Information);
        //                carsCollection.Add(new Bus(temp.LicensNmuber, temp.FirstRegistration));
        //                this.Close();

        //            }
        //        }
        //    }
        //    if (e.Key == Key.Escape)
        //        this.Close();
        //}

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
            Bl = (IBL)navigationContext.Parameters.Where(pair => pair.Key == StringNames.BL).FirstOrDefault().Value;
        }
        #endregion
    }
}
