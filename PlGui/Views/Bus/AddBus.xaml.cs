using PlGui.ViewModels.Bus;
using System.Windows.Controls;
using System.Windows.Input;

namespace PlGui.Views.Bus
{
    /// <summary>
    /// Interaction logic for AddBus.xaml
    /// </summary>
    public partial class AddBus : UserControl
    {
        private bool isClosed;

        private AddBusViewModel viewModel;
        public AddBus()
        {
            InitializeComponent();
            viewModel = (AddBusViewModel)this.DataContext;
            licenseNumBox.Focus();
        }

        /// <summary>
        /// CheckLicense
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LicenseNumBox_OnLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            var validNum = viewModel.DisplayDateFocus();
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

            e.Handled = viewModel.CheckLicenseInput(e.Text);
        }
    }
}
