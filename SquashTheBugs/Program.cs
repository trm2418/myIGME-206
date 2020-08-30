using System;

namespace SquashTheBugs
{
    // Class Program
    // Author: David Schuh
    // Purpose: Bug squashing exercise
    // Restrictions: None
    class Program
    {
        // Method: Main
        // Purpose: Loop through the numbers 1 through 10 
        //          Output N/(N-1) for all 10 numbers
        //          and list all numbers processed
        // Restrictions: None
        static void Main(string[] args)
        {
            // declare int counter
            // compile-time error: missing semicolon
            // int i = 0
            int i = 0;

            // declare string to hold all numbers
            string allNumbers = null;

            // loop through the numbers 1 through 10
            // logic error: doesn't include 10
            // for (i = 1; i < 10; ++i)
            for (i = 1; i <= 10; ++i)
            {
                // declare string to hold all numbers
                // run time error that causes compile-time error on line 50: variable needs to be declared outside of loop
                // string allNumbers = null;

                // output explanation of calculation
                // compile-time error: ints and Strings can't be concatenated with +
                // Console.Write(i + "/" + i - 1 + " = ");
                Console.Write($"{i}/{i-1} = ");

                // output the calculation based on the numbers
                // run-time error: when i is one it's divided by zero, solved by making it a float or a try catch
                // Console.WriteLine(i / (i - 1));
                try
                {
                    Console.WriteLine((float) i / (i - 1));
                } catch
                {
                    Console.WriteLine("Undefined");
                }

                // concatenate each number to allNumbers
                allNumbers += i + " ";

                // increment the counter
                // logic error: since it's a for loop, i is incremented by default, this makes it increment twice
                // i = i + 1;
            }

            // output all numbers which have been processed
            // compile-time error: allNumbers needs to be concatenated with +
            // Console.WriteLine("These numbers have been processed: " allNumbers);
            Console.WriteLine("These numbers have been processed: " + allNumbers);
        }
    }
}

