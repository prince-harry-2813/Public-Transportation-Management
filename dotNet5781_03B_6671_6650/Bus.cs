using dotNet5781_03B_6671_6650.Converters;
using dotNet5781_03B_6671_6650.Views;
using System;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Threading;

namespace dotNet5781_03B_6671_6650
{
    public class Bus : IComparable<Bus>, INotifyPropertyChanged
    {
        private StatusEnum _busStatus;
        public StatusEnum BusStatus
        {
            get => _busStatus; set
            {
                _busStatus = value;
                OnPropertyChanged("BusStatus");
            }
        }

        /// <summary>
        /// Date of last treatment
        /// enter dateTime.now
        /// </summary>
        public DateTime LastTreatment { get; private set; }
        /// <summary>
        /// Bus key,
        /// tow option to registration
        /// 7 or 8 digits
        /// </summary>
        public string LicensNmuber { get; private set; } = "";
        /// <summary>
        /// Fuel status
        /// between 0 - 1200 
        /// </summary>
        public int Fuel { get; private set; } = 1200;
        /// <summary>
        /// KM since last treatment or travel
        /// between 0 - 20,000
        /// </summary>
        public int Maintenance { get; private set; } = 0;
        /// <summary>
        /// Sum of all KM since first travel
        /// can't re reduce
        /// </summary>
        public int TotalKM { get; private set; } = 0;
        /// <summary>
        /// Date of the get in to service 
        /// </summary>
        public DateTime FirstRegistration { get; private set; }

        public DispatcherTimer DispatcherTimerBus { get; set; } = new DispatcherTimer();

        private int _countDown = 0;
        public int CountDown
        {
            get => _countDown; set
            {
                _countDown = value;
                OnPropertyChanged("CountDown");
            }
        }

        /// <summary>
        /// Ctor with all explicit arguments to specify bus
        /// </summary>
        /// <param name="licensNumber"></param>
        /// <param name="firstRegistration"></param>
        /// <param name="lastTreatment"></param>
        /// <param name="fuel"></param>
        /// <param name="maintenence"></param>
        /// <param name="totalKM"></param>
        /// <param name="status"></param>
        public Bus(string licensNumber, DateTime firstRegistration, DateTime lastTreatment, int fuel = 1200, int maintenence = 0, int totalKM = 0, StatusEnum status = StatusEnum.Ok) : this()
        {
            FirstRegistration = firstRegistration;
            SetLicenseNumber(licensNumber);
            Fuel = fuel;
            Maintenance = maintenence;
            TotalKM = totalKM;
            LastTreatment = lastTreatment;
            BusStatus = status;
        }

        /// <summary>
        /// Ctor that accept at least tow param's for license and date
        /// if there is no other arguments initial default values.
        /// </summary>
        /// <param name="licensNumber"></param>
        /// <param name="firstRegistration"></param>
        /// <param name="fuel"></param>
        /// <param name="maintenence"></param>
        public Bus(string licensNumber, DateTime firstRegistration, int fuel = 1200, int maintenence = 0, int totalKM = 0) : this()
        {
            FirstRegistration = firstRegistration;
            SetLicenseNumber(licensNumber);
            Fuel = fuel;
            Maintenance = maintenence;
            TotalKM = totalKM;
            LastTreatment = firstRegistration;
            BusStatus = StatusEnum.Ok;
        }
        /// <summary>
        /// Copy Ctor 
        /// </summary>
        /// <param name="bus"></param>
        public Bus(Bus bus) : this()
        {
            FirstRegistration = bus.FirstRegistration;
            SetLicenseNumber(bus.LicensNmuber);
            Fuel = bus.Fuel;
            Maintenance = bus.Maintenance;
            TotalKM = bus.TotalKM;
            LastTreatment = bus.FirstRegistration;
            BusStatus = bus.BusStatus;

        }
        /// <summary>
        /// default ctor with no param's for adding new bus
        /// only initial interval for timer 
        /// </summary>
        public Bus()
        {
            this.DispatcherTimerBus.Interval = TimeSpan.FromSeconds(1);
            DispatcherTimerBus.Tick += DispatcherTimerBus_Tick;

        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Setter of First Registration 
        /// </summary>
        /// <param name="date"></param>
        public void SetFirstRegistration(DateTime date)
        {
            FirstRegistration = date;
            LastTreatment = date;
        }

        /// <summary>
        /// If bus can take a ride updating the data of the vehicle
        /// </summary>
        /// <param name="km">KM to ride</param>
        public void UpdateRide(int km)
        {
            this.BusStatus = StatusEnum.In_Ride;
            SetTotalKM(km);
            Fuel -= km;
            DispatcherTimerBus.Start();
            CountDown = km / 8; // 48 KM per Hour

        }


        /// <summary>
        /// Sets the bus total KM and avoid decreasing its value 
        /// </summary>
        /// <param name="newKM"></param>
        public void SetTotalKM(int newKM)
        {
            if (newKM < 0)
                return;
            TotalKM += newKM;
        }


        /// <summary>
        /// In case There isn't enough Gas or range km since last treatment return false
        /// </summary>
        /// <param name="rideRange"></param>
        /// <returns> </returns>
        public bool CanTakeRide(int rideRange)
        {
            TimeSpan span = DateTime.Now - this.LastTreatment;
            if (rideRange <= Fuel && (TotalKM - Maintenance + rideRange) < 20000 && span.Days < 365)
            {
                return true;
            }
            if ((TotalKM - Maintenance) > 20000 && span.Days > 365)
            {
                this.BusStatus = StatusEnum.Not_Available;
            }
            Console.WriteLine("This bus can't perform the ride");
            return false;
        }


        /// <summary>
        /// Refuel bus gas tank
        /// simulate real time by using thread to run in background with waiting 
        /// </summary>
        public void ReFuelBus()
        {

            BackgroundWorker backgroundWorker = new BackgroundWorker();
            CountDown = 12;
            DispatcherTimerBus.Start();
            backgroundWorker.DoWork += ((s, e1) => { Thread.Sleep(12000); }
                );
            BusStatus = StatusEnum.In_Refuling;
            backgroundWorker.RunWorkerAsync();
            backgroundWorker.RunWorkerCompleted += ((s, e2) =>
            {

                BusStatus = StatusEnum.Ok;
                this.Fuel = 1200;

            });
        }

        private void DispatcherTimerBus_Tick(object sender, EventArgs e)
        {
            if (CountDown < 1)
            {
                BusStatus = StatusEnum.Ok;
                DispatcherTimerBus.Stop();
                return;
            }
            CountDown = --CountDown;
        }


        /// <summary>
        /// Sets km since lest treatment to 0 and the date 
        /// </summary>
        public void MaintaineBus()
        {
            BackgroundWorker background = new BackgroundWorker();
            CountDown = 144;
            DispatcherTimerBus.Start();
            background.DoWork += ((s, e1) => { Thread.Sleep(144000); });
            BusStatus = StatusEnum.In_Maintainceing;
            background.RunWorkerAsync();
            background.RunWorkerCompleted += ((s, e2) =>
            {
                Maintenance = TotalKM;
                LastTreatment = DateTime.Now;
            });
        }



        /// <summary>
        /// Sets bus license number and checks if its between 7 - 8 digits 
        /// and return false if the number incorrect 
        /// </summary>
        public bool SetLicenseNumber(string licenseNumber)
        {
            var number = new StringBuilder(licenseNumber.Count());
            foreach (var item in licenseNumber)
            {
                if (char.IsDigit(item))
                {
                    number.Append(item);
                }
            }

            if (number.Length > 8)
            {
                Console.WriteLine("License Number too long \n" +
                    "License can contain between 7 - 8 digits no more or no less", " ");
                return false;
            }

            if (number.Length < 7)
            {
                Console.WriteLine("License Number too short \n" +
                   "License can contain between 7 - 8 digits no more or no less");
                return false;
            }
            if ((number.Length == 7 && this.FirstRegistration.Year < 2018) || (number.Length == 8 && this.FirstRegistration.Year >= 2018))
                LicensNmuber = number.ToString();
            else
            {
                System.Windows.MessageBox.Show("Car license doesn't match to year of registered", "Date mismatch", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Stop);
                Console.WriteLine("Car license doesn't match to year of registered");
                return false;

            }
            return true;

        }

        /// <summary>
        /// Gets the bus number and display it with separators
        /// </summary>
        /// <param name="licenceNumber"></param>
        public string DisplayBusNumber()
        {
            string number = this.LicensNmuber;
            StringBuilder displayNumber = new StringBuilder(number.Length * 2);

            if (number.Length == 8)
            {
                for (int i = 0; i < number.Length; i++)
                {
                    displayNumber.Append(number[i]);

                    if (i == 2 || i == 4)
                    {
                        displayNumber.Append("-");
                    }
                }
            }

            if (number.Length == 7)
            {
                for (int i = 0; i < number.Length; i++)
                {
                    displayNumber.Append(number[i]);

                    if (i == 1 || i == 4)
                    {
                        displayNumber.Append("-");
                    }
                }
            }

            return (displayNumber.ToString());
        }
        /// <summary>
        /// implement of IComparable for sort method
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(Bus other)
        {

            if (int.Parse(other.LicensNmuber) == int.Parse(this.LicensNmuber))
            {
                return 0;
            }
            else if
                (int.Parse(other.LicensNmuber) < int.Parse(this.LicensNmuber))
            {
                return 1;
            }
            else return -1;
        }

        public override string ToString()
        {
            return $"{DisplayBusNumber()}";
        }

        #region Interface Implementation

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            //App.Current.Windows.OfType<BusDetails>().FirstOrDefault()?.OnPropertyChanged("");
        }

        #endregion
    }
}
