using System;
using System.Windows.Controls;
using System.Windows.Input;
using PlGui.ViewModels;
using PlGui.ViewModels.Bus;

namespace PlGui.Views.Bus
{
    /// <summary>
    /// Interaction logic for AddBus.xaml
    /// </summary>
    public partial class AddBus : UserControl
    {
        private AddBusViewModel ViewModel;
        public AddBus()
        {
            InitializeComponent();
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
    }
}
