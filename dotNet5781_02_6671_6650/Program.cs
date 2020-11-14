using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Linq.Expressions;

namespace dotNet5781_02_6671_6650
{
    class Program
    {
        #region properties
        /// <summary>
        /// initial collection of bus line
        /// </summary>
        internal static LinesCollection systemCollection = new LinesCollection();

        /// <summary>
        /// Boolean to exit out of the program 
        /// </summary>
        public static bool Flag { get; set; } = true;

        /// <summary>
        /// User option selector
        /// </summary>
        private static int option = -1;
        #endregion

        #region operations
        /// <summary>
        /// Method to add stop to bus if th user want to add it with explicit parameters  
        /// </summary>
        /// <param name="line">Line code</param>
        /// <param name="stop">Station code</param>
        public static void addStopToLineExplicit(int line, int stop)
        {
            double[] location = new double[2];
            Console.WriteLine("Enter latitude");
            double.TryParse(Console.ReadLine(), out location[0]);
            Console.WriteLine("Enter longitude");
            double.TryParse(Console.ReadLine(), out location[1]);
            Console.WriteLine("Enter area");
            systemCollection[line].AddStop(new BusStop(stop, location[0], location[1], "" + Console.ReadLine()));
        }
        /// <summary>
        /// Use to register new line to main collection of line's
        /// </summary>
        private static void registerNewBus()
        {
            Console.WriteLine("Please enter the new line number");
            int[] input = new int[3];
            int.TryParse(Console.ReadLine(), out input[0]);
            Console.WriteLine("Please enter first station Id");
            int.TryParse(Console.ReadLine(), out input[1]);
            Console.WriteLine("Please enter last station Id");
            int.TryParse(Console.ReadLine(), out input[2]);
            var newBus = new BusLine(input[0],input[1],input[2]);
            systemCollection.Add(newBus);
        }

        /// <summary>
        /// method to adding stop to line.
        /// this method check if the new station is already exist, assign it (with the same details) to this line 
        /// </summary>
        private static void addStopToLine(int line)
        {
            int stopCode;
            Console.WriteLine($"Please enter the station number");
            int.TryParse(Console.ReadLine(), out stopCode);
            foreach (BusLine item in systemCollection)
            {
                if (item.IsExist(stopCode))
                {
                    systemCollection[line].AddStop(item.LineStations.FirstOrDefault(stop => stop.StationCode == (stopCode)));
                    return;
                }
            }
            Console.WriteLine("Do you want to assign the properties of this station? choose no if you want to generate it randomly (Y/N)");
            switch ("" + Console.ReadLine().ToUpper())
            {
                case "YES":
                case "Y":
                    addStopToLineExplicit(line, stopCode);
                    return;
                default:
                    break;
            }
            systemCollection[line].AddStop(new BusStop(stopCode));
        }
        /// <summary>
        /// 
        /// </summary>
        private static void removeLine()
        {
            Console.WriteLine("Please enter the line number");
            int[] input = new int[2];
            int.TryParse(Console.ReadLine(), out input[0]);
            systemCollection.Remove(systemCollection[input[0]]);
        }
        /// <summary>
        /// 
        /// </summary>
        private static void removeStopfromLine()
        {
            Console.WriteLine("Please enter the line number");
            int[] input = new int[2];
            int.TryParse(Console.ReadLine(), out input[0]);
            Console.WriteLine("Please enter station number");
            int.TryParse(Console.ReadLine(), out input[1]);
            systemCollection[input[0]].removeStop(input[1]);
        }
        /// <summary>
        /// 
        /// </summary>
        private static void searchLines()
        {
            Console.WriteLine("Please enter the station number");
            int input;
            int.TryParse(Console.ReadLine(), out input);
            foreach (BusLine item in systemCollection)
            {
                if (item.IsExist(input))
                {
                    Console.WriteLine($"Line number : {item.LineKey}.");
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private static void findRoute()
        {
            Console.WriteLine("Please enter your current station number");
            int[] stop = new int[2];
            int.TryParse(Console.ReadLine(), out stop[0]);
            Console.WriteLine("Please enter your destination station number");
            int.TryParse(Console.ReadLine(), out stop[1]);
            foreach (BusLine item in systemCollection)
            {
                if (item.IsExist(stop[0]) && item.IsExist(stop[1]))
                {

                }
            }

        }
        /// <summary>
        /// 
        /// </summary>
        private static void printLinesInfo()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        private static void printStaionInfo()
        {

        }
        #endregion

        static void Main(string[] args)
        {
            do
            {
                Console.WriteLine(@"Hello

Please choose an operation that you'll like to operate:

                1 : Register new line
                2 : Add stop with detailed or random location to exist line
                3 : Remove line from system
                4 : Remove stop from line route 
                5 : Search line by station code
                6 : Show route from a station to destination 
                7 : Info: print all bus line in system
                8 : Info: print all stations with their lines
 
                0 : Exit  ");
                int.TryParse(Console.ReadLine(), out option);
                switch (option)
                {
                    case 1:
                        registerNewBus();
                        break;
                    case 2:
                        Console.WriteLine("Please enter the line number");
                        addStopToLine(int.Parse(Console.ReadLine()));
                        break;
                    case 3:
                        removeLine();
                        break;
                    case 4:
                        removeStopfromLine();
                        break;
                    case 5:
                        searchLines();
                        break;
                    case 6:
                        findRoute();
                        break;
                    case 7:
                        printLinesInfo();
                        break;
                    case 8:
                        printStaionInfo();
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
