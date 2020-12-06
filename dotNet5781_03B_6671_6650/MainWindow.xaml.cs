using dotNet5781_03B_6671_6650.Content;
using dotNet5781_03B_6671_6650.Views;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace dotNet5781_03B_6671_6650
{
    delegate void RefuleAction();

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        static RefuleAction refuleAction;

        public static ObservableCollection<Bus> BusesList = BusCarsCollection.BusesCollection;

        public event PropertyChangedEventHandler PropertyChanged;

        public MainWindow()
        {
            InitializeComponent();
            InsertBus("87654321", DateTime.Now, 1200);
            for (int i = 1; i < 10; i++)
            {
                int z = i + int.Parse("87654321");
                InsertBus(z.ToString(), DateTime.Now, 1200);
            }
            LbBuses.DataContext = BusesList;
            LbBuses.SelectedItem = BusesList;
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
            if ((int)busToRide.BusStaus != 1)
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

        private void RefuleBusButton_Click(object sender, RoutedEventArgs e)
        {
            Button refuelBtn = sender as Button;
            Bus busToRefuel = refuelBtn.DataContext as Bus;
            if ((int)busToRefuel.BusStaus == 3 || busToRefuel.Fuel == 1200)
            {
                MessageBox.Show("This bus is already fueled", "Bus Fueled", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            // busToRefuel.BusStaus = Converters.StatusEnum.InRefuling;
            // refuleAction = new RefuleAction(()=> busToRefuel.ReFuelBus());
            new Thread(() =>
            {
                busToRefuel.ReFuelBus();
                //                refuleAction();
                // Thread.Sleep(12000);
            }).Start();
            busToRefuel.BusStaus = Converters.StatusEnum.Ok;

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

        private void Label_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ////this.Hide();
            Label bus = sender as Label;
            //Thread thread = new Thread(() =>
            //{
            //    BusDetails current = new BusDetails(bus.DataContext as Bus);
            //    current.Show();
            //    SynchronizationContext.SetSynchronizationContext(
            //        new DispatcherSynchronizationContext(Dispatcher.CurrentDispatcher));
            //    Dispatcher.Run();

            //});
            //thread.SetApartmentState(ApartmentState.STA);
            //thread.Start();
            BusDetails current = new BusDetails(bus.DataContext as Bus);
            current.Show();
            this.Hide();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            AddBus newer = new AddBus();
            newer.Show();
        }


    }
}
