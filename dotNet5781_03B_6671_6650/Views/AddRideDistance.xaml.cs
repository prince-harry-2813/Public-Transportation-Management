using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DistanceOfRide_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (DistanceOfRide.Text != string.Empty && int.Parse(DistanceOfRide.Text)!=0 && this.bus.CanTakeRide(int.Parse(DistanceOfRide.Text)))
                {
                    
                    this.bus.UpdateRide(int.Parse(DistanceOfRide.Text));
                    Thread.Sleep(int.Parse(DistanceOfRide.Text)/ 50*1000*6);
                    MessageBox.Show("Ride confirm", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                  
                    this.Close();

                }
                else 
                {
                    MessageBox.Show("Cannot perform a ride","Error!",MessageBoxButton.OK,MessageBoxImage.Error);
                    DistanceOfRide.Text = "";
                    DistanceOfRide.Focus();
                }
            }
        }

        /// <summary>
        /// using Regular expression to validate input with only digits
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DistanceOfRide_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
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
    }
}
