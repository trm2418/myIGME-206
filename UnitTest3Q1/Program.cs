using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest3Q1
{
    class Program
    {
        static void Main(string[] args)
        {
            // Prompt user for input
            Console.Write("enter a string: ");
            string input = Console.ReadLine();



            // Store number of letters as input gets looped through
            int letterCount = 0;

            // Loop through input
            foreach (char c in input)
            {
                // if char is a letter, increment letterCount
                if (char.IsLetter(c))
                {
                    letterCount++;
                }
            }

            Console.WriteLine("number of letters: " + letterCount);



            // Create empty string that will save the reversed version
            string reverse = "";

            // Loop through input backwards
            for (int i = input.Length - 1; i >= 0; i--)
            {
                // Add characters to reverse
                reverse += input[i];
            }

            // Print reversed string
            Console.WriteLine("reversed: " + reverse);



            // create strings to store the input with only letters
            string onlyLetters = "";
            string onlyLettersReversed = "";

            // loop through input and reverse
            for (int i = 0; i < input.Length; i++)
            {
                // if the char is a letter add to onlyLetters
                if (char.IsLetter(input[i]))
                {
                    onlyLetters += input[i];
                }

                // if the char is a letter add to onlyLettersReversed
                if (char.IsLetter(reverse[i]))
                {
                    onlyLettersReversed += reverse[i];
                }
            }

            // print if the input is a palindrome
            Console.WriteLine("palindrome: " + (onlyLetters.ToLower() == onlyLettersReversed.ToLower()).ToString().ToLower());

        }
    }
}
