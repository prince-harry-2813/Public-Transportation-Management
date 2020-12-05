using dotNet5781_03B_6671_6650.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_03B_6671_6650
{
    public class Bus : IComparable<Bus>
    {

        public StatusEnum  BusStaus { get; set; }

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
        public DateTime FirstRegistration { get;private set; }
        //
        //
        //
        /// <summary>
        /// Ctor that accept at least tow param's for license and date
        /// if there is no other arguments initial default values.
        /// </summary>
        /// <param name="licensNumber"></param>
        /// <param name="firstRegistration"></param>
        /// <param name="fuel"></param>
        /// <param name="maintenence"></param>
        public Bus(string licensNumber, DateTime firstRegistration, int fuel = 1200, int maintenence = 0, int totalKM = 0)
        {
            FirstRegistration = firstRegistration;
            SetLicenseNumber(licensNumber);
            Fuel = fuel;
            Maintenance = maintenence;
            TotalKM = totalKM;
            LastTreatment = firstRegistration;
            BusStaus = StatusEnum.Ok;
        }

        public Bus(Bus bus)
        {
            FirstRegistration = bus.FirstRegistration;
            SetLicenseNumber(bus.LicensNmuber);
            Fuel = bus.Fuel;
            Maintenance = bus.Maintenance;
            TotalKM = bus.TotalKM;
            LastTreatment = bus.FirstRegistration;
            BusStaus = bus.BusStaus;
        }

        public Bus() 
        { }
        
        public void SetFirstRegistration (DateTime date)
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
            SetTotalKM(km);
            Fuel -= km;
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
            Console.WriteLine("This bus can't perform the ride");
            return false;
        }


        /// <summary>
        /// Refuel gas bus gas tank 
        /// </summary>
        public void ReFuelBus()
        {
            this.Fuel = 1200;
        }

        /// <summary>
        /// Sets km since lest treatment to 0 and the date 
        /// </summary>
        public void MaintaineBus()
        {
            Maintenance = TotalKM;
            LastTreatment = DateTime.Now;
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
                System.Windows.MessageBox.Show("Car license doesn't match to year of registered","Date mismatch",System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Stop);
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
            return $"Bus License Number : {DisplayBusNumber()}";
        }
    }
}
