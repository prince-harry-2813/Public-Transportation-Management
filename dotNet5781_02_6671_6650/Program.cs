using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        private static void registerNewBus(int lineKey, int firstStop, int lastStop)

        {
            systemCollection.Add(new BusLine(lineKey, firstStop, lastStop));
        }

        static void Main(string[] args)
        {
            do
            {
                Console.WriteLine(@"Hello

Please choose an operation that you'll like to operate:

                1 : Register new line
                2 : Add stop to exist line
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
                        Console.WriteLine("Please enter the new line number");
                        int buskey;
                        int.TryParse(Console.ReadLine(),out buskey);
                        Console.WriteLine("Please enter first station Id");
                        //var newBus = newBus()
                        //systemCollection.Add()
                        break;
                    case 2:

                        break;
                    case 3:

                        break;
                    case 4:
                        break;
                    case 5:
                        break;
                    case 6:
                        break;
                    case 7:
                        break;
                    case 8:
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
