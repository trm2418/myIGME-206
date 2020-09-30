using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace PE14_3
{
    // Class: Program
    // Author: Tyler Machanic
    // Purpose PE14 Question 3
    // Restrictions: None
    class Program
    {
        // seed random
        static Random rnd = new Random();

        // IMyInterface defines the PrintRandom method
        public interface IMyInterface
        {
            void PrintRandom();
        }

        // MyFirstClass inherits IMyInterface and implements the PrintRandom method
        public class MyFirstClass : IMyInterface
        {
            // print random int
            public void PrintRandom()
            {
                Console.WriteLine(rnd.Next(0, 100));
            }
        }

        // MySecondClass inherits IMyInterface and implements the PrintRandom method
        public class MySecondClass : IMyInterface
        {
            // print random char
            public void PrintRandom()
            {
                Console.WriteLine((char)rnd.Next(33, 127));
            }
        }

        // MyMethod calls PrintRandom on a IMyInterface object
        public static void MyMethod(IMyInterface myObject)
        {
            myObject.PrintRandom();
        }

        // Main creates a MyFirstClass and MySecondClass object and calls MyMethod on them
        static void Main(string[] args)
        {
            // create both objects
            MyFirstClass myFirstClass = new MyFirstClass();
            MySecondClass mySecondClass = new MySecondClass();

            // call MyMethod on each object
            MyMethod(myFirstClass);
            MyMethod(mySecondClass);
        }
    }
}
