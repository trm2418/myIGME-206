using System;

namespace ReplaceNoWithYes
{
    // Class: Program
    // Author: Tyler Machanic
    // Purpose: PE8 Question 8
    // Restrictions: None
    class Program
    {

        // Method: Main
        // Purpose: Have user input a string and replace all occurrences of no with yes
        //          Output: input with all "no" replaced with "yes"
        // Notes: 2 versions. One replaces no if its contained in another word, the other doesn't
        // Restrictions: None
        static void Main(string[] args)
        {
            // Prompt user for string and save it to input variable
            Console.Write("Enter a string: ");
            string input = Console.ReadLine();

            // Create empty string that will save the replaced version
            string replaced = "";

            // Asks if user wants to replace no if its contained in another word
            Console.Write("Type yes if you want to replace no if it's within another word: ");
            
            // Replaces all occurrences of no even if it's contained in another word
            if (Console.ReadLine() == "yes")
            {
                // Loop through each character in input
                for (int i = 0; i < input.Length; i++)
                {
                    // If the chararacter is n and it's not the last character
                    if (input[i] == 'n' && i < input.Length - 1)
                    {
                        // If the next character is o
                        if (input[i + 1] == 'o')
                        {
                            // Add yes to replaced string instead of no
                            replaced += "yes";

                            // Increment i because the o shouldn't be added
                            i++;
                        }
                    }
                    else
                    {
                        // Add the character to replaced string
                        replaced += input[i];
                    }
                }
            }
            // Doesn't replace occurrences of no contained in another word
            else
            {
                // Split input into each word
                string[] words = input.Split(' ');

                // Loop through each word
                foreach (string word in words)
                {
                    // If word is no
                    if (word == "no")
                    {
                        // Add yes to replaced string with a space
                        replaced += "yes ";
                    }
                    // If word isn't no
                    else
                    {
                        // Add word to replaced string with a space
                        replaced += word + " ";
                    }
                }
                // Remove extra space from replaced
                replaced = replaced.Substring(0, replaced.Length - 1);
            }

            // Print replaced string
            Console.WriteLine(replaced);
        }
    }
}
