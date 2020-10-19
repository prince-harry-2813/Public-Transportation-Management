using dotNet_01_6671_6650.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet_01_6671_6650
{
    class Program
    {
        /// <summary>
        /// User option selector
        /// </summary>
        private static int option = -1;

        /// <summary>
        /// 
        /// </summary>
        public static List<Bus> BusList { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public static bool Flag { get; set; } = true;


        /// <summary>
        /// Insert new sequence of a bus by licence number and first registration by mendetory and optinal properties
        /// </summary>
        /// <param name="licensNumber"></param>
        /// <param name="firstRegistration"></param>
        /// <param name="fuel"></param>
        /// <param name="maintenence"></param>
        /// <param name="totalKM"></param>
        public static void InsertBus(int licensNumber, DateTime firstRegistration, int fuel = 0, int maintenence = 0, int totalKM = 0)
        {
            Bus bus = new Bus(licensNumber, firstRegistration, fuel, maintenence, totalKM);
            BusList.Add(bus);
        }

        /// <summary>
        /// Return Instance of a bus incase its exsist and can take the ride 
        /// else return false
        /// </summary>
        /// <param name="licensNumber"></param>
        /// <param name="rideRange"></param>
        /// <returns></returns>
        public static Bus ChooseBus(int licensNumber ,  int rideRange)
        {
            return (Bus)BusList.Where((b) => b.LicensNmuber == licensNumber && b.CanTakeRide(rideRange));
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="licensNumber"></param>
        public static void RefuelAndMaintainBus(int licensNumber)
        {
            Bus bus = (Bus)BusList.Where((b) => b.LicensNmuber == licensNumber);
           
        }

        /// <summary>
        /// Write Method that willl display bus data licence numbe with range of km since last treatment
        /// </summary>
        public static void DispalyBusesInfo()
        {

        }

        /// <summary>
        /// Progarm entry point
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            do
            {
                Console.WriteLine(@"PLease Choose An Oparation :
                                     1 : Register new Bus
                                     2 : Choose a Bus for a ride 
                                     3 : Refule or mantainance
                                     4 : Display Buses KM range since last treatment
                                     
                                     0 : Exit");
                int.TryParse(Console.ReadLine(), out option);
                if (option == -1)
                {
                    Console.WriteLine("Wrong input, please try again");
                    continue;
                }


            } while (Flag);
        }


    }
}
