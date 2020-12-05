using dotNet5781_03B_6671_6650.Converters;
using dotNet5781_03B_6671_6650.Views;
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
using dotNet5781_03B_6671_6650.Content;
using System.Threading;
using System.Windows.Threading;

namespace dotNet5781_03B_6671_6650
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window 
    {
        public delegate void refuleAction();
       public static ObservableCollection<Bus> BusesList = BusCarsCollection.BusesCollection;

       
            public MainWindow()
        {
            InitializeComponent();
            InsertBus("87654321", DateTime.Now, 1200, DateTime.Now.Day - 10);
            for (int i = 1; i < 10; i++)
            {
                int z = i + int.Parse("87654321");
                InsertBus(z.ToString() ,  DateTime.Now , 1200, DateTime.Now.Day - 11 + i);
            }
            LbBuses.DataContext = BusesList;
            LbBuses.SelectedItem = BusesList;
        }


        private void ChooseBusButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RefuleBusButton_Click(object sender, RoutedEventArgs e)
        {
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
                MessageBox.Show("This license number already exist","Registration not complete",MessageBoxButton.OK,MessageBoxImage.Warning);
                return; 
                
            }
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
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            AddBus newer = new AddBus();
            newer.Show();
        }

        private void updateList(object sender, DependencyPropertyChangedEventArgs e)
        {
            

        }
    }
}
