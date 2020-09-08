using System;

namespace ReverseString
{
    // Class: Program
    // Author: Tyler Machanic
    // Purpose: PE8 Question 7
    // Restrictions: None
    class Program
    {

        // Method: Main
        // Purpose: Have user input a string and outputs it in reverse order
        //          Output: user inputted string in reverse
        // Restrictions: None
        static void Main(string[] args)
        {
            // Prompt user for string and save it to input variable
            Console.Write("Enter a string: ");
            string input = Console.ReadLine();

            // Create empty string that will save the reversed version
            string reverse = "";

            // Loop through input backwards
            for (int i = input.Length - 1; i >= 0; i--)
            {
                // Add characters to reverse
                reverse += input[i];
            }

            // Print reversed string
            Console.WriteLine(reverse);
        }
    }
}
