using dotNet5781_03B_6671_6650.Content;
using dotNet5781_03B_6671_6650.Converters;
using dotNet5781_03B_6671_6650.Views;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace dotNet5781_03B_6671_6650
{

    //delegate void RefuleAction();

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window 
    {
        // static RefuleAction refuleAction;
        public  ObservableCollection<Bus> BusesList = BusCarsCollection.Instance.BusesCollection;

        public MainWindow()
        {
            InitializeComponent();
            InsertBus("87654321", DateTime.Now, 1200);
            for (int i = 1; i < 10; i++)
            {
                int z = i + int.Parse("87654321");
                InsertBus(z.ToString(), DateTime.Now, 1200);
            }
            BusesList.Add(new Bus("1234567", new DateTime(2011, 02, 03), new DateTime(2009, 02, 03), 1200, 0, 0, StatusEnum.Not_Available));
            BusesList.Add(new Bus("1234568", new DateTime(2011, 02, 03), DateTime.Now, 1200,0, 19000, StatusEnum.Ok));
            BusesList.Add(new Bus("1234569", new DateTime(2011, 02, 03), DateTime.Now, 90, 0, 0, StatusEnum.Ok));
           
            LbBuses.DataContext = BusesList;
            LbBuses.SelectedItem = BusesList;
            
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
            if (BusesList.Any((b) => b.LicensNmuber == bus.LicensNmuber) || licensNumber == "")
            {
                MessageBox.Show("This license number already exist", "Registration not complete", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;

            }
            BusesList.Add(bus);
        }
        #endregion

        /// <summary>
        /// when double clicked on bus car label open new window of details 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Label_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Label bus = sender as Label;
            if (bus == null)
                return;
            BusDetails current = new BusDetails(bus.DataContext as Bus);
            current.Show();
            this.Hide();
        }

        /// <summary>
        /// open new window to add new bus to system
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            AddBus newer = new AddBus();
            newer.Show();
        }

        /// <summary>
        /// Button to operate ride. 
        /// when click open new window of ride details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChooseBusButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            Button button = sender as Button;
            Bus busToRide = button.DataContext as Bus;
            if ((int)busToRide.BusStatus != 1)
            {
                MessageBox.Show("this bus is current unavailable", "Cannot take a ride", MessageBoxButton.OK, MessageBoxImage.Warning);
                this.Show();
            }
            else
            {
                AddRideDistance addRide = new AddRideDistance(busToRide);
                addRide.Show();
            }
        }

        /// <summary>
        /// Refuel bus from main window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RefuleBusButton_Click(object sender, RoutedEventArgs e)
        {
            Button refuelBtn = sender as Button;
            Bus busToRefuel = refuelBtn.DataContext as Bus;
            if ((int)busToRefuel.BusStatus == 3 || busToRefuel.Fuel == 1200)
            {
                MessageBox.Show("This bus is already fueled", "Bus Fueled", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            // busToRefuel.BusStatus = Converters.StatusEnum.In_Refuling;
            // refuleAction = new RefuleAction(()=> busToRefuel.ReFuelBus());
            //BackgroundWorker backgroundWorker = new BackgroundWorker();
            //backgroundWorker.DoWork += ((s, e1) => { Thread.Sleep(12000); }
            //    );
            //busToRefuel.BusStatus = StatusEnum.In_Refuling;
            //busToRefuel.CountDown = 12;
            //busToRefuel.DispatcherTimerBus.Start();
            //busToRefuel.DispatcherTimerBus.Tick += DispatcherTimerBus_Tick;
            //backgroundWorker.RunWorkerAsync();
            //backgroundWorker.RunWorkerCompleted += ((s, e2) =>
            //{
            //    busToRefuel.BusStatus = StatusEnum.Ok;
            //    busToRefuel.ReFuelBus();
            //    LbBuses.ItemsSource = BusesList;

            //});
            busToRefuel.ReFuelBus();
        }

    }
}
