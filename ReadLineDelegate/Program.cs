using System;

namespace ReadLineDelegate
{
    // Class: Program
    // Author: Tyler Machanic
    // Purpose: PE9 Question 3
    // Restrictions: None
    class Program
    {
        // define delegate function data type
        delegate string readLineImpersonation();

        // Method: Main
        // Purpose: Declare and set delegate variable, ask for input and print
        //          Output: User input
        // Restrictions: None
        static void Main(string[] args)
        {
            // declare delegate variable
            readLineImpersonation readLine;

            // set delegate variable to Console.ReadLine function
            readLine = new readLineImpersonation(Console.ReadLine);

            // ask for input
            Console.Write("Input: ");

            // print output using delegate variable instead Console.ReadLine
            Console.WriteLine(readLine());
        }
    }
}
