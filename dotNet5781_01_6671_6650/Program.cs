using dotNet5781_01_6671_6650.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_01_6671_6650
{
    class Program
    {
        /// <summary>
        /// User option selector
        /// </summary>
        private static int option = -1;

        public static Random random = new Random(DateTime.Now.Millisecond);
        /// <summary>
        /// buses list of main program
        /// </summary>
        public static List<Bus> BusList { get; set; } = new List<Bus>();

        /// <summary>
        /// Boolian to exit out of the program 
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
        public static void InsertBus(string licensNumber, DateTime firstRegistration, int fuel = 1200, int maintenence = 0, int totalKM = 0)
        {

            Bus bus = new Bus(licensNumber, firstRegistration, fuel, maintenence, totalKM);
            if (BusList.Exists((b) => b.LicensNmuber == bus.LicensNmuber))
                return;
            BusList.Add(bus);
        }

        /// <summary>
        /// Return Instance of a bus incase its exsist and can take the ride 
        /// else return false
        /// </summary>
        /// <param name="licensNumber"></param>
        /// <param name="rideRange"></param>
        /// <returns></returns>
        public static void ChooseBus(int licensNumber, int rideRange)
        {
            var a = (Bus)BusList.Where((b) => b.LicensNmuber == licensNumber);
            if (a == null)
            {
                Console.WriteLine("bus dosen't exsist");
            }
            else if (a.CanTakeRide(rideRange))
            {
                a.updateRide(rideRange);

            }
            else
                return;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="licensNumber"></param>
        public static void RefuelAndMaintainBus(int licensNumber)
        {
            Bus bus = (Bus)BusList.Where((b) => b.LicensNmuber == licensNumber);
            bus.ReFuelBus();
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





                switch (option)
                {
                    case 1:
                        Console.WriteLine("Please enter licens number for the new vehicle");
                        var newLicens = Console.ReadLine();
                        Console.WriteLine("Please enter date of registration (dd/mm/yyyy):");
                        var newDateReg = Console.ReadLine();
                        InsertBus(newLicens, DateTime.ParseExact(newDateReg, "dd/MM/yyyy", null));
                        break;
                    case 2:
                        Console.WriteLine("Please enter the licens number of the vehicle");
                        ChooseBus(int.Parse(Console.ReadLine()), random.Next(1, 1200));
                        break;
                    case 3:
                        break;
                    case 4:
                        break;
                    case 0:
                        Console.WriteLine("Good Bye");
                        Flag = false;
                        break;
                    default:
                        Flag = true;
                        Console.WriteLine("Wrong input, please try again");

                        break;
                }

            } while (Flag);
        }


    }
}
