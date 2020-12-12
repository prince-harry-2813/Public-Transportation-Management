using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace dotNet5781_03B_6671_6650.Views
{
    /// <summary>
    /// Interaction logic for AddRideDistance.xaml
    /// </summary>
    public partial class AddRideDistance : Window
    {
        Bus bus;
        public AddRideDistance(Bus bus)
        {
            InitializeComponent();
            this.bus = bus;
            DistanceOfRide.Text = "1";
            DistanceOfRide.SelectAll();
            DistanceOfRide.Focus();

        }

        /// <summary>
        /// Accept enter key press to proceed in ride operation.
        /// enter work only if there is valid number of KM to drive
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DistanceOfRide_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (DistanceOfRide.Text != string.Empty && int.Parse(DistanceOfRide.Text) != 0 && this.bus.CanTakeRide(int.Parse(DistanceOfRide.Text)))
                {

                    this.bus.UpdateRide(int.Parse(DistanceOfRide.Text));
                    MessageBox.Show("Ride confirm", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                    this.Close();

                }
                else
                {
                    MessageBox.Show("Cannot perform a ride", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                    DistanceOfRide.Text = "";
                    DistanceOfRide.Focus();
                }
            }
            if (e.Key == Key.Escape)
                this.Close();
        }

        /// <summary>
        /// using Regular expression to validate input with only digits 
        /// make sure that the number begin with larger than zero digit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DistanceOfRide_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (DistanceOfRide.Text.StartsWith("0"))
                DistanceOfRide.Text = DistanceOfRide.Text.Substring(1);

            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);


        }
        /// <summary>
        /// When this window closed show back main window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            App.Current.MainWindow.Show();
        }
        /// <summary>
        /// allow to drag the window 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
