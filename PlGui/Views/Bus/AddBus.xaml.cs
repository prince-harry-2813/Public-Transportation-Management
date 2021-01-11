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
            
        }

        /// <summary>
        /// CheckLicense
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LicenseNumBox_OnLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
           var validNum = ViewModel.LicenseNumBoxLostFocus();
           if (!validNum)
           {
               licenseNumBox.Focus();
           }
        }

        /// <summary>
        /// Numbers Validation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LicenseNumBox_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            
            e.Handled = this.ViewModel.CheckLicenseInput(e.Text);
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

        }

        /// <summary>
        /// Validate input of text box to accept only numbers
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NumbersValidation(object sender, TextCompositionEventArgs e)
        {
        }
    }
}
