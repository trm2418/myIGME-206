using System;
using System.IO;

namespace MadLibs
{
    // Class: Program
    // Author: Tyler Machanic
    // Purpose: PE7 Mad Libs
    // Restrictions: None
    class Program
    {
        // Method: Main
        // Purpose: Controls game
        //          Output: mad lib story with user inputted words
        // Restrictions: None
        static void Main(string[] args)
        {
            // string that saves user's response for if they want to play
            string userChoice;

            // keeps track of how many invalid inputs the user has entered
            int invalidCounter = 0;

            // asks if the user wants to play
            Console.Write("Do you want to play Mad Libs?\nyes or no: ");

            // converts input to lower case
            userChoice = Console.ReadLine().ToLower();

            // keeps asking until you say yes or no
            while (userChoice != "yes" && userChoice!= "no")
            {
                invalidCounter++;

                // print increasingly disappointed messages
                switch (invalidCounter)
                {
                    case 1:
                    case 2:
                        Console.Write("Please type yes or no: ");
                        break;
                    case 3:
                        Console.Write("You got this! Type yes or no: ");
                        break;
                    case 4:
                        Console.Write("You can do it! Type yes or no: ");
                        break;
                    case 5:
                        Console.Write("Come on! Just type yes or no: ");
                        break;
                    case 6:
                        Console.Write("Please just type yes or no, it's not that hard: ");
                        break;
                    case 7:
                        Console.Write("You're just wasting your time at this point: ");
                        break;
                    case 8:
                        Console.Write("I give up: ");
                        break;
                    default:
                        Console.Write("yes or no: ");
                        break;
                }
                
                userChoice = Console.ReadLine();
            }

            // quit program if user says no
            if (userChoice == "no")
            {
                Console.WriteLine("Goodbye");
                System.Environment.Exit(0);
            }

            // create play again boolean that is used to keep playing
            bool playAgain = false;

            // repeat if the user wants to play again
            do
            {
                // create new StreamReader
                StreamReader input = null;

                // int that keeps track of the number of lines in the text file
                int lines = 0;

                // string array that holds all the stories
                string[] stories;

                // used to keep track of the line number while reading the text file
                int counter = 0;

                // used to store the current line being read
                string line;

                // used to make sure user inputs a valid story selection
                bool valid = false;

                // stores user's name
                string name;

                // stores user's story choice
                int storyChoice = 0;

                // stores final string that gets printed
                string resultString = "";

                // stores all words in a story once it's split
                string[] words;

                // opens file
                input = new StreamReader("c:\\templates\\MadLibsTemplate.txt");

                // reads file and increments lines
                while ((line = input.ReadLine()) != null)
                {
                    lines++;
                }

                // close file
                input.Close();

                // sets stories to an empty array equal to the amount of lines in the text file
                stories = new string[lines];

                // reopens the file
                input = new StreamReader("c:\\templates\\MadLibsTemplate.txt");

                // reads file and stores each line in the stories array
                while ((line = input.ReadLine()) != null)
                {
                    stories[counter] = line;
                    counter++;
                }

                // prompts user for their name
                Console.Write("Enter your name: ");
                name = Console.ReadLine();

                // asks user which story they want to do until they enter a valid number
                do
                {
                    try
                    {
                        Console.Write($"Which story would you like?\n1-{lines}: ");
                        storyChoice = Convert.ToInt32(Console.ReadLine()) - 1;
                        if (storyChoice < 0 || storyChoice > lines - 1)
                        {
                            Console.WriteLine($"Please enter a number between 1 and {lines}");
                        }
                        else
                        {
                            valid = true;
                        }
                    }
                    catch
                    {
                        Console.WriteLine("Please enter a valid number");
                    }
                } while (!valid);

                // split story into each word
                words = stories[storyChoice].Split(' ');

                // loop through each word
                foreach (string word in words)
                {
                    // if the word is a word the user needs to enter
                    if (word[0] == '{')
                    {
                        // due to the difference in formatting between commas and periods, this is necessary
                        if (word[word.Length - 1] != ',')
                        {
                            // prompt user for their input
                            Console.Write(word.Replace("_", " ").Substring(1, word.Length - 2) + ": ");
                            resultString += Console.ReadLine() + " ";
                        }
                        else
                        {
                            // prompt user for their input
                            Console.Write(word.Replace("_", " ").Substring(1, word.Length - 3) + ": ");
                            resultString += Console.ReadLine() + ", ";
                        }

                    }
                    else
                    {
                        // if new line
                        if (word == "\\n")
                        {
                            resultString += '\n';
                        }
                        // due to the weird formatting of periods in the text file this is necessary
                        else if (word == ".")
                        {
                            resultString = resultString.Substring(0, resultString.Length - 1) + ". ";
                        }
                        // for normal words
                        else
                        {
                            resultString += word + " ";
                        }
                    }
                }

                // print final string
                Console.WriteLine("\n" + resultString + "\n");

                // asks user if they want to play again
                Console.Write("Press enter to play again ");
                if (Console.ReadLine() == "")
                {
                    playAgain = true;
                }
                else
                {
                    playAgain = false;
                }
            } while (playAgain);

            
        }
    }
}
