using System;

namespace Mandelbrot
{
    /// <summary>
    /// This class generates Mandelbrot sets in the console window!
    /// </summary>
    /// Author: Tyler Machanic
    /// Purpose: PE4 Question 6


    class Class1
    {
        /// <summary>
        /// This is the Main() method for Class1 -
        /// this is where we call the Mandelbrot generator!
        /// </summary>
        /// <param name="args">
        /// The args parameter is used to read in
        /// arguments passed from the console window
        /// </param>
        /// Output: Mandelbrot set in console window

        [STAThread]
        static void Main(string[] args)
        {
            double realCoord, imagCoord;
            double realTemp, imagTemp, realTemp2, arg;
            int iterations;
            bool valid = false;
            double imagCoordHigh = 0;
            double imagCoordLow = 0;
            double realCoordLow = 0;
            double realCoordHigh = 0;
            
            // prompt user for input 4 times
            for (int i = 0; i < 4; i++)
            {
                // repeat until valid input
                do
                {
                    // checks for valid input
                    try
                    {
                        // runs a different switch depending on the iteration
                        switch(i)
                        {
                            // high imaginary coordinate
                            case 0:
                                Console.Write("Enter high imaginary coordinate: ");
                                imagCoordHigh = Convert.ToDouble(Console.ReadLine());
                                valid = true;
                                break;

                            // low imaginary coordinate
                            case 1:
                                Console.Write("Enter low imaginary coordinate: ");
                                imagCoordLow = Convert.ToDouble(Console.ReadLine());

                                // validates input
                                if (imagCoordLow >= imagCoordHigh)
                                {
                                    Console.WriteLine("Low imaginary coordinate must be less than the high one");
                                    i--;
                                }
                                else
                                {
                                    valid = true;
                                }
                                break;

                            // low real coordinate
                            case 2:
                                Console.Write("Enter low real coordinate: ");
                                realCoordLow = Convert.ToDouble(Console.ReadLine());
                                valid = true;
                                break;

                            // high real coordinate
                            case 3:
                                Console.Write("Enter high real coordinate: ");
                                realCoordHigh = Convert.ToDouble(Console.ReadLine());
                                
                                // validates input
                                if (realCoordHigh <= realCoordLow)
                                {
                                    Console.WriteLine("High real coordinate must be greater than the low one.");
                                    i--;
                                }
                                else
                                {
                                    valid = true;
                                }
                                break;
                        }
                        
                    }
                    catch
                    {
                        Console.WriteLine("Invalid input");
                    }
                }
                while (!valid);

                valid = false;
            }

            // generate Mandelbrot
            for (imagCoord = imagCoordHigh; imagCoord >= imagCoordLow; imagCoord -= (imagCoordHigh-imagCoordLow)/48)
            {
                for (realCoord = realCoordLow; realCoord <= realCoordHigh; realCoord += (realCoordHigh-realCoordLow)/80)
                {
                    iterations = 0;
                    realTemp = realCoord;
                    imagTemp = imagCoord;
                    arg = (realCoord * realCoord) + (imagCoord * imagCoord);
                    while ((arg < 4) && (iterations < 40))
                    {
                        realTemp2 = (realTemp * realTemp) - (imagTemp * imagTemp)
                           - realCoord;
                        imagTemp = (2 * realTemp * imagTemp) - imagCoord;
                        realTemp = realTemp2;
                        arg = (realTemp * realTemp) + (imagTemp * imagTemp);
                        iterations += 1;
                    }
                    switch (iterations % 4)
                    {
                        case 0:
                            Console.Write(".");
                            break;
                        case 1:
                            Console.Write("o");
                            break;
                        case 2:
                            Console.Write("O");
                            break;
                        case 3:
                            Console.Write("@");
                            break;
                    }
                }
                Console.Write("\n");
            }

        }
    }
}
