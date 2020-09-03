using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace NumberGuessingGame
{
    // Class: Program
    // Author: Tyler Machanic
    // Purpose: PE6 Number Guessing Game
    // Restrictions: None
    class Program
    {
        // Method: Main
        // Purpose: Controls game
        //          Output: Result of game, score, prompt to play again
        // Restrictions: None
        static void Main(string[] args)
        {
            // create a new Random object
            Random rand = new Random();

            // create boolean that will control whether the user wants to play again or not
            bool playAgain = false;

            // create boolean that will validate user input
            bool valid = false;

            // create score variable that will keep track of the user's score
            int score = 0;

            // runs the entire game, will continue if user wants to play again
            do
            {
                
                // creates maxNumber value which is the upper bound for the randomly generated number that the user will guess
                int maxNumber = 100;

                // creates difficulty variable that will control how many guesses the user gets
                int difficulty = 1;

                // prompts user to enter upper bound until a valid input is entered
                do
                {
                    try
                    {
                        // prompts user and sets max number to input
                        Console.Write("Select max number: ");
                        maxNumber = Convert.ToInt32(Console.ReadLine());

                        // lower bound will always be 0 so the upper bound must be at least 1
                        if (maxNumber < 1)
                        {
                            Console.WriteLine("Please enter an integer 1 or higher.");
                        }
                        else
                        {
                            valid = true;
                        }
                    }
                    catch
                    {
                        Console.WriteLine("Please enter an integer.");
                    }
                } while (!valid);

                // reset valid to false because it is used multiple times in this method
                valid = false;

                // prompts user for their desired difficulty until a valid input is entered
                do
                {
                    try
                    {
                        // prompts user and sets difficulty variable
                        Console.Write("Select difficulty\n1. Easy 2. Medium 3. Hard: ");
                        difficulty = Convert.ToInt32(Console.ReadLine());

                        // makes sure difficulty is 1, 2 or 3
                        if (difficulty > 3 || difficulty < 1)
                        {
                            Console.WriteLine("Please enter 1, 2 or 3.");
                        }
                        else
                        {
                            valid = true;
                        }
                    }
                    catch
                    {
                        Console.WriteLine("Please enter 1, 2 or 3.");
                    }
                } while (!valid);

                // generate the random number
                int randomNumber = rand.Next(0, maxNumber + 1);

                // set the total guesses using a logarithm
                int totalGuesses = Convert.ToInt32(Math.Log(maxNumber, 2)) + 1;

                // adjust total guesses with the difficulty
                if (difficulty == 1)
                {
                    // easy mode gives an extra guess
                    totalGuesses++;
                }
                else if (difficulty == 3)
                {
                    // hard mode gives one less guess
                    totalGuesses--;
                }

                // if total guesses are less than 1, set it to 1
                if (totalGuesses < 1)
                {
                    totalGuesses = 1;
                }

                // reset valid to false again
                valid = false;

                // declare int variable that will be used to store the user's guess
                int guess = 0;

                // create variable for for loop, declared outside the loop since it is used after the loop
                int i;

                // create an array to store all the guesses, used to check if the user has already guessed their inputted number
                int[] guesses = new int[totalGuesses];

                // fills guesses array with -1's since by default it's filled with 0's and 0 can be guessed
                for (int j = 0; j < guesses.Length; j++)
                {
                    guesses[j] = -1;
                }

                // stores the lowest guess the user has made
                int minGuess = 0;

                // stores the highest guess the user has made
                int maxGuess = maxNumber;

                // asks user if they want to reveal the number, for testing purposes only
                Console.Write("Type y to reveal number, anything else to continue: ");
                if (Console.ReadLine() == "y")
                {
                    Console.WriteLine(randomNumber);
                }

                // the main game loop, run once for each guess
                for (i = 1; i < totalGuesses + 1; i++)
                {
                    // print new line for formatting reasons
                    Console.WriteLine();

                    // prompts user for guess until a valid input is enterred
                    do
                    {
                        try
                        {
                            // prompt user for guess and store to guess variable
                            Console.Write($"Enter guess {i}/{totalGuesses}: ");
                            guess = Convert.ToInt32(Console.ReadLine());

                            // checks if guess is in the bounds of the highest and lowest guess
                            if (guess <= maxGuess && guess >= minGuess)
                            {
                                // checks if user has already guessed that number
                                if (guesses.Contains(guess))
                                {
                                    Console.WriteLine("You already guessed that");
                                }
                                // if not, continue
                                else
                                {
                                    guesses[i - 1] = guess;
                                    valid = true;
                                }
                            }
                            else
                            {
                                Console.WriteLine($"Please enter a number between {minGuess}-{maxGuess}.");
                            }
                        } catch
                        {
                            Console.WriteLine("Please enter an integer.");
                        }
                    } while (!valid);

                    // sets valid to false once again
                    valid = false;

                    // tell user if their guess was too high, too low or correct. also updates maxGuess and minGuess
                    if (guess > randomNumber)
                    {
                        Console.WriteLine("Too high!");
                        maxGuess = guess;
                    }
                    else if (guess < randomNumber)
                    {
                        Console.WriteLine("Too low!");
                        minGuess = guess;
                    }
                    else
                    {
                        Console.WriteLine("Correct!");

                        // print how many attempts the user got it in
                        Console.Write("You guessed it in " + i + " attempt");
                        if (i > 1)
                        {
                            Console.Write("s");
                        }
                        Console.WriteLine();

                        // increase score by the difficulty level and tell user
                        score += difficulty;
                        Console.WriteLine("You got " + difficulty + " points!");

                        // this is in case user guesses right on the last guess. the next if statement would run if this weren't here
                        i = totalGuesses + 2;
                    }
                }
                // if all guesses have been used
                if (i == totalGuesses + 1)
                {
                    // tell user the number
                    Console.WriteLine("You failed! The number was " + randomNumber + ".");

                    // decrease score depending on difficulty
                    switch (difficulty)
                    {
                        // -2 points if you lose on easy
                        case 1:
                            score -= 2;
                            Console.WriteLine("You lost 2 points!");
                            break;
                        // -1 point if you lose on medium
                        case 2:
                            score -= 1;
                            Console.WriteLine("You lost 1 point!");
                            break;
                        // no points lost if you lose on hard
                        case 3:
                            break;
                    }
                }
                // print current score and ask user if they want to play again
                Console.WriteLine("Your current score is " + score + ".");
                Console.WriteLine("Type y to play again");
                if (Console.ReadLine() == "y")
                {
                    playAgain = true;
                }
                else
                {
                    playAgain = false;
                }

            } while (playAgain);

            // once user quits, display final score
            Console.WriteLine("Your final score is " + score + ".");

        }
    }
}
