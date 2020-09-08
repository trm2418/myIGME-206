using System;

namespace DoubleQuotes
{
    // Class: Program
    // Author: Tyler Machanic
    // Purpose: PE8 Question 9
    // Restrictions: None
    class Program
    {

        // Method: Main
        // Purpose: Have user input a string and put double quotes around each word
        //          Output: input with double quotes around each word
        // Restrictions: None
        static void Main(string[] args)
        {
            // Prompt user for string and save it to input variable
            Console.Write("Enter a string: ");
            string input = Console.ReadLine();

            // Create empty string that will save the replaced version
            string replaced = "";

            // Split input into each word
            string[] words = input.Split(' ');

            // Loop through each word
            foreach (string word in words)
            {
                // Add double quotes around each word and a space at the end
                replaced += '"' + word + '"' + " "; 
            }

            // Remove extra space from replaced
            replaced = replaced.Substring(0, replaced.Length - 1);
            
            // Print replaced string
            Console.WriteLine(replaced);
        }
    }
}
