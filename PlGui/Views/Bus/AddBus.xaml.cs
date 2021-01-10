using System;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using PlGui.ViewModels;
using PlGui.ViewModels.Bus;
using Prism.Mvvm;

namespace PlGui.Views.Bus
{
    /// <summary>
    /// Interaction logic for AddBus.xaml
    /// </summary>
    public partial class AddBus : UserControl
    { 
        //private PO.Bus temp = new PO.Bus();
        
        private bool isClosed;

        private AddBusViewModel ViewModel;
        public AddBus()
        {
            InitializeComponent();
            ViewModelLocationProvider.Register(typeof(AddBus).ToString(), typeof(AddBusViewModel));
            ViewModel = (AddBusViewModel) this.DataContext;
        }

        /// <summary>
        /// Enter key
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LicenseNumBox_OnKeyDown(object sender, KeyEventArgs e)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// CheckLicense
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LicenseNumBox_OnLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// NumbersValidation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LicenseNumBox_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            
        }

        /// <summary>
        /// When the text box lost focus check if the input length is more then 7 digits
        /// when input length is 7 digits the date picker configure to show dates between 2000 - 2018
        /// when input length is 8 digits the date picker configure to show dates between 2018 - now
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkLicense(object sender, RoutedEventArgs e)
        {

            if (licenseNumBox.Text.Length < 7 && !isClosed
                //&& !licenseNumBox.Focusable
                )
            {

                licenseNumBox.Text = "";
                MessageBox.Show("Please enter valid number, must contain at least 7 digits");
                licenseNumBox.Focus();
            }
            else if (licenseNumBox.Text.Length == 7)
            {
                RegistrationDate.DisplayDateEnd = new DateTime(2017, 12, 31);
                RegistrationDate.DisplayDateStart = new DateTime(2000, 1, 1);
            }
            else if (licenseNumBox.Text.Length == 8)
            {
                RegistrationDate.DisplayDateStart = new DateTime(2018, 01, 01);
                RegistrationDate.DisplayDateEnd = DateTime.Now;
            }
            licenseNumBox.Focus();

        }

        /// <summary>
        /// Validate input of text box to accept only numbers
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NumbersValidation(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

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
        //                ObservableCollection<PO.Bus> carsCollection = new ObservableCollection<PO.Bus>();

        //                foreach (PO.Bus bus in carsCollection)
        //                {
        //                    if (bus.LicensNmuber == licenseNumBox.Text)
        //                    {
        //                        MessageBox.Show("This License number already exist in system");
        //                        return;
        //                    }
        //                }
        //                MessageBox.Show("Added successfully", "Bus Added", MessageBoxButton.OK, MessageBoxImage.Information);
        //                carsCollection.Add(new PO.Bus(temp.LicenseNum, temp.RegisDate));
        //                this.Close();

        //            }
        //        }
        //    }
        //    if (e.Key == Key.Escape)
        //        this.Close();
        //}

    }
}
