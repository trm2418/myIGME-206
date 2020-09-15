using System;
using System.Linq;
using System.Timers;

namespace _3Questions
{
    // Class: Program
    // Author: Tyler Machanic
    // Purpose: Unit Test 1 Question 4
    // Restrictions: None
    class Program
    {
        // declare global class-scoped variables which need to be accessed by all members of the class

        // boolean to check if user timed out
        static bool bTimeOut = false;

        // string for correct answer
        static string sAnswer = "";

        // declare timer variable
        static Timer timeOutTimer;

        // Method: Main
        // Purpose: Run 3 questions program
        //          Output: Questions and message depending on if user got it right
        // Restrictions: Case sensitive
        public static void Main(string[] args)
        {
            // play again?
            string sAgain = "";

            // string for player response
            string sResponse = "";

            Console.WriteLine();
        // label to return to if user wants to play again or inputs an invalid number
        start:

            // prompt user for question choice
            Console.Write("Choose your question (1-3): ");
            sResponse = Console.ReadLine();

            // go to start if user enters anything besides 1, 2 or 3
            if (!new string[] { "1", "2", "3" }.Contains(sResponse))
            {
                goto start;
            }

            // initialize timeout flag
            bTimeOut = false;

            Console.WriteLine("You have 5 seconds to answer the following question:");

            // ask question and set sAnswer to the right answer
            switch (sResponse)
            {
                case "1":
                    Console.WriteLine("What is your favorite color?");
                    sAnswer = "black";
                    break;
                case "2":
                    Console.WriteLine("What is the answer to life, the universe and everything?");
                    sAnswer = "42";
                    break;
                case "3":
                    Console.WriteLine("What is the airspeed velocity of an unladen swallow?");
                    sAnswer = "What do you mean? African or European swallow?";
                    break;
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

            // Get answer from user
            sResponse = Console.ReadLine();
            
            // if user got answer right and hasn't timed out say well done
            // else if user got answer wrong and hasn't timed out say wrong and tell user the answer
            if (sResponse == sAnswer && !bTimeOut)
            {
                Console.WriteLine("Well done!");
            }
            else if (!bTimeOut)
            {
                Console.WriteLine("Wrong!  The answer is: {0}", sAnswer);
            }

            // only run if user has not timed out
            if (!bTimeOut)
            {
                timeOutTimer.Stop();
            }

            do
            {
                // prompt if they want to play again
                Console.Write("Play again? ");

                sAgain = Console.ReadLine();

                if (sAgain.ToLower().StartsWith("y"))
                {
                    Console.WriteLine();
                    goto start;
                }
                else if (sAgain.ToLower().StartsWith("n"))
                {
                    break;
                }
            } while (true);
        }

        // Method: TimesUp
        // Purpose: Print times up message and the answer, reset timeout flag and stop timer
        //          Output: times up message, the answer and press enter message
        // Restrictions: None
        static void TimesUp(object source, ElapsedEventArgs e)
        {
            // print times up message
            Console.WriteLine("Time's up!");
            
            // print answer
            Console.WriteLine("The answer is: {0}", sAnswer);

            Console.WriteLine("Please press enter.");

            // set bTimeOut to true
            bTimeOut = true;

            // stop timer
            timeOutTimer.Stop();
        }
    }
}
