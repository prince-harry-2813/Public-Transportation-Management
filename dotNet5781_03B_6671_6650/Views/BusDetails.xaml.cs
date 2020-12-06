using dotNet5781_03B_6671_6650.Converters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
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
using dotNet5781_03B_6671_6650.Content;

namespace dotNet5781_03B_6671_6650.Views
{
    /// <summary>
    /// Interaction logic for BusDetails.xaml
    /// </summary>
    public partial class BusDetails : Window , INotifyPropertyChanged
    {
        private Bus _selectedBus;
        public Bus SelectedBus
        {
            get { return _selectedBus; }
            set {
                _selectedBus = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedBus")); }
        }
        ObservableCollection<BusPropertyInfo> busPropertyInfos { get; set; } = new ObservableCollection<BusPropertyInfo>();
        ObservableCollection<Bus> carsCollection = BusCarsCollection.BusesCollection;

        public BusDetails(Bus bus)
        {
            carsCollection.First(b => b.LicensNmuber == bus.LicensNmuber);
            InitializeComponent();
            ShowBusDetalis(carsCollection.First(b => b.LicensNmuber == bus.LicensNmuber));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        private void RefuleButton_Click(object sender, RoutedEventArgs e)
        {
            SelectedBus.ReFuelBus();
            SelectedBus.BusStaus = StatusEnum.NOk;
            ShowBusDetalis(SelectedBus);
            Thread thread = new Thread(() =>
            {
                Thread.Sleep(1200);
            });

            thread.Start();


            //Task.Factory.StartNew(() =>
            //{
            //    var obj = Dispatcher.BeginInvoke(new Action(() => Thread.Sleep(12000)));
            //    obj.Wait(); 

            //    this.Dispatcher.Invoke(new Action(() => SelectedBus.BusStaus = StatusEnum.NOk));
            //    Thread.Sleep(12000);
            //    Dispatcher.Invoke(new Action(() => SelectedBus.BusStaus = StatusEnum.Ok));
            //});
        }

        private void TreatmentButton_Click(object sender, RoutedEventArgs e)
        {
            SelectedBus.MaintaineBus();
            SelectedBus.BusStaus = StatusEnum.NOk;
            ShowBusDetalis(SelectedBus);

            Task.Factory.StartNew(() =>
            {
                this.Dispatcher.BeginInvoke(new Action(() => { SelectedBus.BusStaus = StatusEnum.NOk;
                    ShowBusDetalis(SelectedBus); })).Wait();
                Thread.Sleep(6 * 24 * 1000);
                //SelectedBus.BusStaus = StatusEnum.Ok;
            });
        }

        public void ShowBusDetalis(Bus bus)
        {
            busPropertyInfos.Clear();
            SelectedBus = bus;
            busPropertyInfos.Add(new BusPropertyInfo
            {
                PropertyKey = "Bus Status :"
                ,
                PropertyValue = SelectedBus.BusStaus.ToString(),

            });
            busPropertyInfos.Add(new BusPropertyInfo
            {
                PropertyKey = "First Registration :"
              ,
                PropertyValue = SelectedBus.FirstRegistration.ToString("dd/MM/yyyy"),

            }); busPropertyInfos.Add(new BusPropertyInfo
            {
                PropertyKey = "Fuel :"
               ,
                PropertyValue = SelectedBus.Fuel.ToString(),

            });
            busPropertyInfos.Add(new BusPropertyInfo
            {
                PropertyKey = "Last Treatment :"
               ,
                PropertyValue = SelectedBus.LastTreatment.ToString(),

            });
            busPropertyInfos.Add(new BusPropertyInfo
            {
                PropertyKey = "License Number :"
               ,
                PropertyValue = SelectedBus.LicensNmuber.ToString(),
            });

            busPropertyInfos.Add(new BusPropertyInfo
            {
                PropertyKey = "Maintenance :"
               ,
                PropertyValue = SelectedBus.Maintenance.ToString(),
            });

            busPropertyInfos.Add(new BusPropertyInfo
            {
                PropertyKey = "Total KM :"
               ,
                PropertyValue = SelectedBus.TotalKM.ToString(),
            });
            lbBusDetails.ItemsSource = busPropertyInfos;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            App.Current.MainWindow.Show();
        }
    }

    class BusPropertyInfo
    {
        public string PropertyKey { get; set; }
        public string PropertyValue { get; set; }
    }
}
