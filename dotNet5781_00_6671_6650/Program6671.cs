using System;

namespace dotNet5781_00_6671_6650
{
    partial class Program
    {
        static void Main(string[] args)
        {
            Welcome6671();
            Welcome6650();
            Console.ReadKey();

        }

        private static void Welcome6671()
        {
            Console.WriteLine("enter your name");
            var name = Console.ReadLine();
            Console.WriteLine("{0}, welcome to my first console application", name);
        }
        static partial void Welcome6650();
    }
}
