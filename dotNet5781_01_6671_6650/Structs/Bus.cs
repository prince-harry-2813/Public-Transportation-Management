using System;
using System.Linq;
using System.Text;

namespace dotNet5781_01_6671_6650.Structs
{
    public class Bus
    {
        /// <summary>
        /// Date of last treatment
        /// enter dateTime.now
        /// </summary>
        public DateTime LastTreatment { get; private set; }
        /// <summary>
        /// Bus key
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
        /// KM since last tritment or travel
        /// between 0 - 20,000
        /// </summary>
        public int Maintenance { get; private set; } = 0;
        /// <summary>
        /// Sum of all KM since first travel
        /// can't bre reduce
        /// </summary>
        public int TotalKM { get; private set; } = 0;
        /// <summary>
        /// Date of the get in to service 
        /// </summary>
        public DateTime FirstRegistration { get; set; }
        //
        //
        //
        /// <summary>
        /// Ctor that accept at least tow params for licens and date
        /// if there is no other arguments initial default values.
        /// </summary>
        /// <param name="licensNumber"></param>
        /// <param name="firstRegistration"></param>
        /// <param name="fuel"></param>
        /// <param name="maintenence"></param>
        public Bus(string licensNumber, DateTime firstRegistration, int fuel = 0, int maintenence = 0, int totalKM = 0)
        {
            SetLicenseNumber(licensNumber);
            FirstRegistration = firstRegistration;
            Fuel = fuel;
            Maintenance = maintenence;
            TotalKM = totalKM;
        }

        public void updateRide(int km)
        {
            SetTotalKM(km);
            Fuel -= km;
            Maintenance += km;
        }
        /// <summary>
        /// Sets the bus toal KM and evoid decreasing its value 
        /// </summary>
        /// <param name="newKM"></param>
        public void SetTotalKM(int newKM)
        {
            if (newKM < 0)
                return;
            TotalKM += newKM;
        }


        /// <summary>
        /// In case There isn't enough Gas or range km since last treament return false
        /// </summary>
        /// <param name="rideRange"></param>
        /// <returns>bool type</returns>
        public bool CanTakeRide(int rideRange)
        {
            if (rideRange <= Fuel && (TotalKM - Maintenance + rideRange) < 20000)
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
        /// Sets bus licence number and checks if its between 7 - 8 digits 
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
                Console.WriteLine("Licence Number too long \n" +
                    "Licence can contain between 7 - 8 digits no more or no less", " ");
                return false;
            }

            if (number.Length < 7)
            {
                Console.WriteLine("Licence Number too short \n" +
                   "Licence can contain between 7 - 8 digits no more or no less");
                return false;
            }

            LicensNmuber = number.ToString();
            return true;
        }

        /// <summary>
        /// Gets th ebu snumber and display it with wite space and seperators
        /// </summary>
        /// <param name="licenceNumber"></param>
        public void DisplayBusNumber(int licenceNumber)
        {
            string number = licenceNumber.ToString();
            StringBuilder displayNumber = new StringBuilder(number.Length * 2);

            if (number.Length == 8)
            {
                for (int i = 0; i < number.Length; i++)
                {
                    displayNumber.Append(number[i]);

                    if (i == 2 || i == 4)
                    {
                        displayNumber.Append(" - ");
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
                        displayNumber.Append(" - ");
                    }
                }
            }

            Console.WriteLine(displayNumber);
        }
    }
}
