using dotNet5781_03B_6671_6650.Converters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace dotNet5781_03B_6671_6650
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window 
    {
        public List<Bus> BusesList { get; set; }
        public delegate void refuleAction();
        
        public MainWindow()
        {
            InitializeComponent();
            BusesList = new List<Bus>() ;
            InsertBus("87654321", DateTime.Now, 1200, DateTime.Now.Day - 10);
            for (int i = 0; i < 10; i++)
            {
                int z = i + int.Parse("87654321");
                InsertBus(z.ToString() ,  DateTime.Now , 1200, DateTime.Now.Day - 10 + i);
            }
            LbBuses.DataContext = BusesList;
            LbBuses.SelectedItem = BusesList[0];
        }

        private void ChooseBusButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RefuleBusButton_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine();  
        }

        private void LbBuses_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowBusline(4);
        }

        #region Initialization Methods

        /// <summary>
        /// Insert new instance of a bus by license number and first registration by mandatory and optional properties
        /// </summary>
        /// <param name="licensNumber"></param>
        /// <param name="firstRegistration"></param>
        /// <param name="fuel"></param>
        /// <param name="maintenence"></param>
        /// <param name="totalKM"></param>
        public void InsertBus(string licensNumber, DateTime firstRegistration, int fuel = 1200, int maintenence = 0, int totalKM = 0)
        {
            Bus bus = new Bus(licensNumber, firstRegistration, fuel, maintenence, totalKM);
            if (BusesList.Exists((b) => b.LicensNmuber == bus.LicensNmuber) || licensNumber == "")
                return;
            BusesList.Add(bus);
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lineKey"></param>
        private void ShowBusline(int lineKey)
        {

        }
    }
}
