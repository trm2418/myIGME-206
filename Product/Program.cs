using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace Product
{
    // Class: Program
    // Author: Tyler Machanic
    // Purpose: PE3 Question 5
    // Restrictions: None
    class Program
    {
        // Method: Main
        // Purpose: Prompt the user for four ints
        //          Output the product of the four ints
        // Restrictions: None
        static void Main(string[] args)
        {
            // declare product which will be multiplied by inputs
            int product = 1;

            // declare valid boolean which controls try catch block
            bool valid = false;

            // loop 4 times, one for each input
            for (int i = 1; i < 5; i++)
            {
                // repeat until valid input
                do
                {
                    // asks for input, catches if invalid
                    try
                    {
                        // prompts user for input
                        Console.Write($"Enter number {i}: ");

                        // multiplies product by input
                        product *= Convert.ToInt32(Console.ReadLine()); ;
                        valid = true;
                    }
                    catch
                    {
                        // tells user input was invalid
                        Console.WriteLine("Invalid input");
                    }

                } while (!valid);

                valid = false;
            }
            
            // prints final product
            Console.WriteLine(product);
        }
    }
}
