using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet_01_6671_6650.Structs
{
    public class Bus
    {
        /// <summary>
        /// Date of last treatment
        /// enter dateTime.now
        /// </summary>
        public DateTime LastTreatment { get; set; }
        /// <summary>
        /// Bus key
        /// tow option to registration
        /// 7 or 8 digits
        /// </summary>
        public int LicensNmuber { get; set; } = 0;
        /// <summary>
        /// Fuel status
        /// between 0 - 1200 
        /// </summary>
        public int Fuel { get; set; } = 0;
        /// <summary>
        /// KM since last tritment
        /// between 0 - 20,000
        /// </summary>
        public int Maintenance { get; set; } = 0;
        /// <summary>
        /// Sum of all KM since first travel
        /// can't bre reduce
        /// </summary>
        public int TotalKM { get ;private set; } = 0;
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
        public Bus(int licensNumber, DateTime firstRegistration, int fuel = 0, int maintenence = 0,int totalKM=0)
        {
            LicensNmuber = licensNumber;
            FirstRegistration = firstRegistration;
            Fuel = fuel;
            Maintenance = maintenence;
            TotalKM = totalKM;
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
        /// <returns></returns>
        public bool CanTakeRide(int rideRange)
        {
            if (rideRange <= Fuel && Maintenance + rideRange < 20000)
            {
                return true;
            }
            return false;
        }
    }
}
