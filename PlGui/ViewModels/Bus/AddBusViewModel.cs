using System.Linq;
using BL.BLApi;
using Prism.Regions;

namespace PlGui.ViewModels.Bus
{
    public class AddBusViewModel
    {
                #region Properties Decleration

        public IBL Bl { get; set; }
        
        #endregion
        
        public AddBusViewModel()
        {
            #region MyRegion

            

            #endregion
        }

        ///// <summary>
        ///// When the text box lost focus check if the input length is more then 7 digits
        ///// when input length is 7 digits the date picker configure to show dates between 2000 - 2018
        ///// when input length is 8 digits the date picker configure to show dates between 2018 - now
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void checkLicense(object sender, RoutedEventArgs e)
        //{

        //    if (licenseNumBox.Text.Length < 7 && !isClosed
        //        //&& !licenseNumBox.Focusable
        //        )
        //    {

        //        licenseNumBox.Text = "";
        //        MessageBox.Show("Please enter valid number, must contain at least 7 digits");
        //        licenseNumBox.Focus();
        //    }
        //    else if (licenseNumBox.Text.Length == 7)
        //    {
        //        RegistrationDate.DisplayDateEnd = new DateTime(2017, 12, 31);
        //        RegistrationDate.DisplayDateStart = new DateTime(2000, 1, 1);
        //    }
        //    else if (licenseNumBox.Text.Length == 8)
        //    {
        //        RegistrationDate.DisplayDateStart = new DateTime(2018, 01, 01);
        //        RegistrationDate.DisplayDateEnd = DateTime.Now;
        //    }
        //    licenseNumBox.Focus();
        //}

        ///// <summary>
        ///// Validate input of text box to accept only numbers
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void NumbersValidation(object sender, TextCompositionEventArgs e)
        //{
        //    Regex regex = new Regex("[^0-9]+");
        //    e.Handled = regex.IsMatch(e.Text);
        //}

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
