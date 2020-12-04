using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using dotNet5781_03B_6671_6650;
using dotNet5781_03B_6671_6650.Content;

namespace dotNet5781_03B_6671_6650.Views
{
    /// <summary>
    /// Interaction logic for AddBus.xaml
    /// </summary>
    public partial class AddBus : Window
    {

        private Bus temp = new Bus();


        public AddBus()
        {
            InitializeComponent();
            licenseNumBox.Focus();

        }
        /// <summary>
        /// when the details window closed, open back the main window;
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            App.Current.MainWindow.Show();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkLicense(object sender, RoutedEventArgs e)
        {
            if (licenseNumBox.Text.Length < 7 && !licenseNumBox.Focusable)
            {

                licenseNumBox.Text = "";
                MessageBox.Show("Please enter valid number, must contain at least 7 digits");
                licenseNumBox.Focus();

            }

            licenseNumBox.Focus();

        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NumbersValidation(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        /// <summary>
        /// BONUS! when enter key is pressed proceed 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void enterKey(object sender, KeyEventArgs e)
        {
            if (e.Key==Key.Enter)
            {
                if (licenseNumBox.Text.Length>=7)
                {
                    temp.SetFirstRegistration(RegistrationDate.DisplayDate);
                  if (temp.SetLicenseNumber(licenseNumBox.Text)) 
                    {
                        MessageBox.Show("Added successfully", "Bus Added", MessageBoxButton.OK, MessageBoxImage.Information);
                        List<Bus> carsCollection = BusCarsCollection.BusesCollection;
                        carsCollection.Add(new Bus(temp.LicensNmuber, temp.FirstRegistration));
                        this.Close();

                    }
                }
            }
        }
    }
}
