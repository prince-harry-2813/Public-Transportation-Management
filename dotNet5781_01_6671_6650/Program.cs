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
        private static int Option = -1;

        public static Random Random = new Random(DateTime.Now.Millisecond);
        /// <summary>
        /// buses list of main program
        /// </summary>
        public static List<Bus> BusList { get; set; } = new List<Bus>();

        /// <summary>
        /// Boolean to exit out of the program 
        /// </summary>
        public static bool Flag { get; set; } = true;


        /// <summary>
        /// Insert new instance of a bus by license number and first registration by mandatory and optional properties
        /// </summary>
        /// <param name="licensNumber"></param>
        /// <param name="firstRegistration"></param>
        /// <param name="fuel"></param>
        /// <param name="maintenence"></param>
        /// <param name="totalKM"></param>
        public static void InsertBus(string licensNumber, DateTime firstRegistration, int fuel = 1200, int maintenence = 0, int totalKM = 0)
        {

            Bus bus = new Bus(licensNumber, firstRegistration, fuel, maintenence, totalKM);
            if (BusList.Exists((b) => b.LicensNmuber == bus.LicensNmuber)||licensNumber=="")
                return;
            BusList.Add(bus);
        }

        /// <summary>
        /// Return Instance of a bus in case its exist and can take the ride 
        /// else return false
        /// </summary>
        /// <param name="licensNumber"></param>
        /// <param name="rideRange"></param>
        /// <returns></returns>
        public static void ChooseBus(string licensNumber, int rideRange)
        {
            var a = (Bus)BusList.Where((b) => b.LicensNmuber == licensNumber).FirstOrDefault();
            if (a == null)
            {
                Console.WriteLine("bus doesn't exist");
            }
            else if (a.CanTakeRide(rideRange))
            {
                a.UpdateRide(rideRange);

            }
            else
                return;
        }
        

        /// <summary>
        /// Method to handle with refuel and maintenance request.
        /// </summary>
        /// <param name="licensNumber"></param>
        public static void RefuelAndMaintainBus(string licensNumber)
        {
            Bus bus = (Bus)BusList.Where((b) => b.LicensNmuber == licensNumber).FirstOrDefault();
            if (bus == null)
            {
                Console.WriteLine("bus doesn't exist");
            }
            else
            {
                Console.WriteLine("Choose 1 for refuel or 2 for main maintenance");
                switch (Console.ReadLine())
                {
                    case "1" :bus.ReFuelBus();
                        Console.WriteLine("Refuel complete");
                        break;
                    case "2": bus.MaintaineBus();
                        Console.WriteLine("maintenance complete");
                        break;
                     default:
                        Console.WriteLine("ERROR");
                        break;
                }
            }
                
        }

        /// <summary>
        /// Write Method that will display bus data license number with range of km since last treatment
        /// </summary>
        public static void DispalyBusesInfo()
        {
            if (BusList.Count() != 0)
            {
                BusList.Sort();
                foreach (var item in BusList)
                {
                    Console.WriteLine($"Bus number: {item.DisplayBusNumber()}. The last treatment was at {item.Maintenance}-KM" +
                        $", Since then the bus drove for {item.TotalKM - item.Maintenance}-KM");
                }
            }
            else
                Console.WriteLine("there is no registered buses");
        }

        /// <summary>
        /// Program entry point
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            do
            {
                Console.WriteLine(@"PLease Choose An Operation :
                                     1 : Register new Bus
                                     2 : Choose a Bus for a ride 
                                     3 : Refuel or maintenance
                                     4 : Display Buses KM range since last treatment
                                     
                                     0 : Exit");
                int.TryParse(Console.ReadLine(), out Option);





                switch (Option)
                {
                    case 1:
                        Console.WriteLine("Please enter license number for the new vehicle");
                        var newLicens = Console.ReadLine();
                        Console.WriteLine("Please enter date of registration (dd/mm/yyyy):");
                        var newDateReg = Console.ReadLine();
                        InsertBus(newLicens, DateTime.ParseExact(newDateReg, "dd/MM/yyyy", null));
                        break;
                    case 2:
                        Console.WriteLine("Please enter the license number of the vehicle");
                        ChooseBus(Console.ReadLine(), Random.Next(1, 1200));
                        break;
                    case 3:
                        Console.WriteLine("Please enter the license number of the vehicle");
                        RefuelAndMaintainBus(Console.ReadLine());
                        break;
                    case 4:
                        Console.WriteLine("View info:");
                        DispalyBusesInfo();
                        break;
                    case 0:
                        Console.WriteLine("Good Bye");
                        Console.ReadKey();
                        Flag = false;
                        break;
                    default:
                        
                        Console.WriteLine("Wrong input, please try again");

                        break;
                }

            } while (Flag);
        }


    }
}
