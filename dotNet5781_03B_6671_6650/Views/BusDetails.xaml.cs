using dotNet5781_03B_6671_6650.Content;
using dotNet5781_03B_6671_6650.Converters;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows;

namespace dotNet5781_03B_6671_6650.Views
{
    /// <summary>
    /// Interaction logic for BusDetails.xaml
    /// </summary>
    public partial class BusDetails : Window, INotifyPropertyChanged
    {

        private Bus _selectedBus;

        ObservableCollection<BusPropertyInfo> busPropertyInfos { get; set; } = new ObservableCollection<BusPropertyInfo>();

        ObservableCollection<Bus> carsCollection = BusCarsCollection.Instance.BusesCollection;

        public Bus SelectedBus
        {
            get { return _selectedBus; }
            set
            {
                _selectedBus = value;
                if (value != _selectedBus && value.CountDown != _selectedBus.CountDown)
                    OnPropertyChanged(new PropertyChangedEventArgs("SelectedBus"));
            }
        }


        /// <summary>
        /// Ctor of window
        /// initialize bus to show info from global collection
        /// </summary>
        /// <param name="bus">Bus instance to show</param>
        public BusDetails(Bus bus)
        {
            InitializeComponent();
            _selectedBus = carsCollection.First(b => b.LicensNmuber == bus.LicensNmuber);
            ShowBusDetalis(SelectedBus);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// update view when property has changed
        /// </summary>
        /// <param name="e"></param>
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
            ShowBusDetalis(SelectedBus);

        }

        /// <summary>
        /// when refuel button has pressed start refuel process
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RefuleButton_Click(object sender, RoutedEventArgs e)
        {
            if ((int)SelectedBus.BusStatus == 3 || SelectedBus.Fuel == 1200)
            {
                MessageBox.Show("This bus is already fueled", "Bus Fueled", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if ((int)SelectedBus.BusStatus != 1 && (int)SelectedBus.BusStatus != 5)
            {
                MessageBox.Show("This bus is busy right now", "Can't perform operation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            // busToRefuel.BusStatus = Converters.StatusEnum.In_Refuling;
            //// refuleAction = new RefuleAction(()=> busToRefuel.ReFuelBus());
            //new Thread(() =>
            //{
            //    SelectedBus.ReFuelBus();

            //    ShowBusDetalis(SelectedBus);

            //    //                refuleAction();
            //    // Thread.Sleep(12000);
            //}).Start();
            //SelectedBus.BusStatus = StatusEnum.Ok;
            //ShowBusDetalis(SelectedBus);

            //BackgroundWorker backgroundWorker = new BackgroundWorker();
            //SelectedBus.CountDown = 12;
            //SelectedBus.DispatcherTimerBus.Tick += DispatcherTimerBus_Tick;
            //SelectedBus.DispatcherTimerBus.Start();
            //backgroundWorker.DoWork += ((s, e1) => { Thread.Sleep(12000); }
            //    );
            //SelectedBus.BusStatus = StatusEnum.In_Refuling;
            //OnPropertyChanged(new PropertyChangedEventArgs("SelectedBus"));
            //ShowBusDetalis(SelectedBus);
            //backgroundWorker.RunWorkerAsync();
            //backgroundWorker.RunWorkerCompleted += ((s, e2) =>
            //{

            //    SelectedBus.BusStatus  = StatusEnum.Ok;
            SelectedBus.ReFuelBus();

            //    OnPropertyChanged(new PropertyChangedEventArgs("SelectedBus"));
            //    ShowBusDetalis(SelectedBus);
            //});


            //Task.Factory.StartNew(() =>
            //{
            //    var obj = Dispatcher.BeginInvoke(new Action(() => Thread.Sleep(12000)));
            //    obj.Wait(); 

            //    this.Dispatcher.Invoke(new Action(() => SelectedBus.BusStatus = StatusEnum.In_Ride));
            //    Thread.Sleep(12000);
            //    Dispatcher.Invoke(new Action(() => SelectedBus.BusStatus = StatusEnum.Ok));
            //});
        }
        /// <summary>
        /// when the timer has launched
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DispatcherTimerBus_Tick(object sender, EventArgs e)
        {
            if (SelectedBus.CountDown < 1)
            {
                SelectedBus.BusStatus = StatusEnum.Ok;
                SelectedBus.DispatcherTimerBus.Stop();
                return;
            }
            ShowBusDetalis(SelectedBus);
        }

        /// <summary>
        /// when button pressed treatment bus
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreatmentButton_Click(object sender, RoutedEventArgs e)
        {

            if ((int)SelectedBus.BusStatus != 1 && (int)SelectedBus.BusStatus != 5)
            {
                MessageBox.Show("This bus is busy right now", "Can't perform operation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            ///
            BackgroundWorker backgroundWorker = new BackgroundWorker();
            SelectedBus.CountDown = 144;
            SelectedBus.DispatcherTimerBus.Tick += DispatcherTimerBus_Tick;
            SelectedBus.DispatcherTimerBus.Start();
            backgroundWorker.DoWork += ((s, e1) => { Thread.Sleep(6 * 24 * 1000); }
                );
            SelectedBus.BusStatus = StatusEnum.In_Maintainceing;
            OnPropertyChanged(new PropertyChangedEventArgs("SelectedBus"));
            ShowBusDetalis(SelectedBus);
            backgroundWorker.RunWorkerAsync();
            backgroundWorker.RunWorkerCompleted += ((s, e2) =>
            {

                SelectedBus.BusStatus = StatusEnum.Ok;
                SelectedBus.MaintaineBus();

                OnPropertyChanged(new PropertyChangedEventArgs("SelectedBus"));
                ShowBusDetalis(SelectedBus);
            });
        }

        /// <summary>
        /// re initial bus info to show when property change
        /// </summary>
        /// <param name="bus"></param>
        public void ShowBusDetalis(Bus bus)
        {
            busPropertyInfos.Clear();
            busPropertyInfos.Add(new BusPropertyInfo
            {
                PropertyKey = "Bus Status :"
                ,
                PropertyValue = bus.BusStatus.ToString().Replace('_', ' '),

            });
            busPropertyInfos.Add(new BusPropertyInfo
            {
                PropertyKey = "First Registration :"
              ,
                PropertyValue = bus.FirstRegistration.ToString("dd/MM/yyyy"),

            }); busPropertyInfos.Add(new BusPropertyInfo
            {
                PropertyKey = "Fuel :"
               ,
                PropertyValue = bus.Fuel.ToString(),

            });
            busPropertyInfos.Add(new BusPropertyInfo
            {
                PropertyKey = "Last Treatment :"
               ,
                PropertyValue = bus.LastTreatment.ToString("dd/MM/yyyy"),

            });
            busPropertyInfos.Add(new BusPropertyInfo
            {
                PropertyKey = "License Number :"
               ,
                PropertyValue = bus.LicensNmuber.ToString(),
            });

            busPropertyInfos.Add(new BusPropertyInfo
            {
                PropertyKey = "Maintenance was at :"
               ,
                PropertyValue = bus.Maintenance.ToString() + " - KM",
            });

            busPropertyInfos.Add(new BusPropertyInfo
            {
                PropertyKey = "Total KM :"
               ,
                PropertyValue = bus.TotalKM.ToString() + " - KM",
            });
            if (SelectedBus.CountDown > 0)
            {
                busPropertyInfos.Add(new BusPropertyInfo
                {
                    PropertyKey = "Count-Down :"
                   ,
                    PropertyValue = bus.CountDown.ToString(),
                });
            }
            lbBusDetails.ItemsSource = busPropertyInfos;



        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            App.Current.MainWindow.Show();
        }
    }

    /// <summary>
    /// Represent of bus info property
    /// </summary>
    class BusPropertyInfo
    {
        public string PropertyKey { get; set; }
        public string PropertyValue { get; set; }


    }
}
