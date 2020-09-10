using System;
using System.Timers;


namespace MathQuizTimed
{
    // Class: Program
    // Authors: Professor David Schuh, Tyler Machanic
    // Purpose: PE9 Question 2
    // Restrictions: None
    class Program
    {
        // declare global class-scoped variables which need to be accessed by all members of the class

        // boolean to check if user timed out
        static bool bTimeOut = false;

        // declare timer variable
        static Timer timeOutTimer;

        // Method: Main
        // Purpose: Run math test
        //          Output: Questions and score
        // Restrictions: None
        static void Main()
        {
            // store user name
            string myName = "";

            // string and int of # of questions
            string sQuestions = "";
            int nQuestions = 0;

            // string and base value related to difficulty
            string sDifficulty = "";
            int nMaxRange = 0;

            // constant for setting difficulty with 1 variable
            const int MAX_BASE = 10;

            // question and # correct counters
            int nCntr = 0;
            int nCorrect = 0;

            // operator picker
            int nOp = 0;

            // operands and solution
            int val1 = 0;
            int val2 = 0;
            int nAnswer = 0;

            // string and int for the response
            string sResponse = "";
            Int32 nResponse = 0;

            // boolean for checking valid input
            bool bValid = false;

            // play again?
            string sAgain = "";

            // seed the random number generator
            Random rand = new Random();

            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine("Math Quiz!");
            Console.WriteLine();

            // fetch the user's name into myName
            while (true)
            {
                Console.Write("What is your name-> ");
                myName = Console.ReadLine();

                if (myName.Length > 0)
                {
                    break;
                }
            }

        // label to return to if they want to play again
        start:

            // initialize timeout flag
            bTimeOut = false;

            // initialize correct responses for each time around
            nCorrect = 0;

            Console.WriteLine();

            do
            {
                Console.Write("How many questions-> ");
                sQuestions = Console.ReadLine();

                try
                {
                    nQuestions = int.Parse(sQuestions);
                    bValid = true;
                }
                catch
                {
                    Console.WriteLine("Please enter an integer.");
                    bValid = false;
                }

            } while (!bValid);

            Console.WriteLine();

            do
            {
                Console.Write("Difficulty level (easy, medium, hard)-> ");
                sDifficulty = Console.ReadLine();
            } while (sDifficulty.ToLower() != "easy" &&
                     sDifficulty.ToLower() != "medium" &&
                     sDifficulty.ToLower() != "hard");

            Console.WriteLine();

            // if they choose easy, then set nMaxRange = MAX_BASE, unless myName == "David", then set difficulty to hard
            // if they choose medium, set nMaxRange = MAX_BASE * 2
            // if they choose hard, set nMaxRange = MAX_BASE * 3
            switch (sDifficulty.ToLower())
            {
                case "easy":
                    nMaxRange = MAX_BASE;
                    if (myName.ToLower() == "david")
                    {
                        goto case "hard";
                    }
                    break;

                case "medium":
                    nMaxRange = MAX_BASE * 2;
                    break;

                case "hard":
                    nMaxRange = MAX_BASE * 3;
                    break;
            }

            // ask each question
            for (nCntr = 0; nCntr < nQuestions; ++nCntr)
            {
                // generate a random number between 0 inclusive and 3 exclusive to get the operation
                nOp = rand.Next(0, 3);

                val1 = rand.Next(0, nMaxRange) + nMaxRange;
                val2 = rand.Next(0, nMaxRange);

                // if either argument is 0, pick new numbers
                if (val1 == 0 || val2 == 0)
                {
                    // decrement counter to try this one again (because it will be incremented at the top of the loop)
                    --nCntr;
                    continue;
                }

                // if nOp == 0, then addition
                // if nOp == 1, then subtraction
                // else multiplication
                if (nOp == 0)
                {
                    nAnswer = val1 + val2;
                    sQuestions = $"Question #{nCntr + 1}: {val1} + {val2} => ";
                }
                else if (nOp == 1)
                {
                    nAnswer = val1 - val2;
                    sQuestions = $"Question #{nCntr + 1}: {val1} - {val2} => ";
                }
                else
                {
                    nAnswer = val1 * val2;
                    sQuestions = $"Question #{nCntr + 1}: {val1} * {val2} => ";
                }

                // create timeOutTimer with an elapsed time of 5 seconds
                timeOutTimer = new Timer(5000);

                // declare a delegate variable
                ElapsedEventHandler timesUpMethod;

                // point delegate variable to TimesUp method
                timesUpMethod = new ElapsedEventHandler(TimesUp);

                // Add the TimesUp() delegate function to the timeOutTimer.Elapsed event handler
                timeOutTimer.Elapsed += timesUpMethod;

                // Start the timer
                timeOutTimer.Start();

                // display the question and prompt for the answer
                do
                {
                    Console.Write(sQuestions);
                    sResponse = Console.ReadLine();

                    // only run if user has not timed out
                    if (!bTimeOut)
                    {
                        try
                        {
                            nResponse = int.Parse(sResponse);
                            bValid = true;

                            // Stop the timer
                            timeOutTimer.Stop();
                        }
                        catch
                        {
                            Console.WriteLine("Please enter an integer.");
                            bValid = false;
                        }
                    }

                } while (!bValid);



                // if user timed out tell them the answer
                // else if user got answer right, output flashy reward and increment # correct
                // else output stark answer
                if (bTimeOut)
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("The answer is {1}", myName, nAnswer);

                    // reset bTimeOut to false
                    bTimeOut = false;
                }
                else if (nResponse == nAnswer)
                {
                    Console.BackgroundColor = ConsoleColor.Blue;
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine("Well done, {0}!!!", myName);

                    ++nCorrect;
                }
                else
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("I'm sorry {0}. The answer is {1}", myName, nAnswer);
                }

                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;

                Console.WriteLine();
                
            }

            Console.WriteLine();

            // output how many they got correct and their score
            Console.WriteLine("You got {0} correct out of {1}, which is a score of {2:P2}", nCorrect, nQuestions, Convert.ToDouble(nCorrect) / (double)nCntr);
            Console.WriteLine();

            do
            {
                // prompt if they want to play again
                Console.Write("Do you want to play again? ");

                sAgain = Console.ReadLine();

                if (sAgain.ToLower().StartsWith("y"))
                {
                    goto start;
                }
                else if (sAgain.ToLower().StartsWith("n"))
                {
                    break;
                }
            } while (true);
        }

        // Method: TimesUp
        // Purpose: Print times up message, set bTimeOut to true and stop timer
        //          Output: times up message
        // Restrictions: User needs to press enter after this happens
        static void TimesUp(object source, ElapsedEventArgs e)
        {
            // print times up message
            Console.WriteLine("\nYour time is up!");

            // set bTimeOut to true
            bTimeOut = true;

            // stop timer
            timeOutTimer.Stop();
        }
    }
}
