using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;

namespace GreaterThan10
{
    // Class: Program
    // Author: Tyler Machanic
    // Purpose: PE4 Question 2
    // Restrictions: None
    class Program
    {
        // Method: Main
        // Purpose: Prompt the user for 2 ints
        //          Output The inputs and if one int is greater than 10
        // Restrictions: None
        static void Main(string[] args)
        {
            // declare ints that will be used later
            int var1 = 0;
            int var2 = 0;

            // declare valid boolean which controls try catch block
            bool valid = false;

            // repeat twice, once for each input
            for (int i = 1; i < 3; i++)
            {
                // repeat until valid input
                do
                {
                    // asks for input, catches if invalid
                    try
                    {
                        // prompts user for input
                        Console.Write($"enter number {i}: ");

                        // sets var1 and var2
                        if (i == 1)
                        {
                            // sets var 1 if input can be converted to int
                            var1 = Convert.ToInt32(Console.ReadLine());
                            valid = true;
                        }
                        else
                        {
                            // sets var 2 if input can be converted to int
                            var2 = Convert.ToInt32(Console.ReadLine());
                            
                            //checks if both inputs are greater than 10
                            if (var1 > 10 && var2 > 10)
                            {
                                // resets i to 1 so you will have to input both numbers again
                                i = 1;

                                Console.WriteLine("both numbers are > 10. try again.");
                            }
                            else
                            {
                                // otherwise continues with program
                                valid = true;
                            }
                        }
                    }
                    catch
                    {
                        // tells user input was invalid
                        Console.WriteLine("input could not be converted to int. try again.");
                    }
                } while (!valid);

                valid = false;
            }
            // print inputs
            Console.WriteLine(var1 + " and " + var2);

            // print true if one number is greater than 10, false otherwise
            Console.WriteLine(var1 > 10 || var2 > 10);
        }
    }
}
