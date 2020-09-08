using System;

namespace PE8Question5
{
    // Class: Program
    // Author: Tyler Machanic
    // Purpose: PE8 Question 5
    // Restrictions: None
    class Program
    {
        // Method: Main
        // Purpose: Create 3 dimensional array that stores values for x, y and z
        // Restrictions: None
        static void Main(string[] args)
        {
            // declare 3D array that will store all x,y,z coordinates
            double[,,] values = new double[21, 31, 3];

            // declare int counters to keep track of position in the first 2 dimensions
            int xCounter = 0;
            int yCounter = 0;

            // declare doubles to store x, y and z
            double x;
            double y;
            double z;

            // loop from x = -1 to x = 1 with 0.1 increments
            for (x = -1; x <= 1; x += 0.1)
            {
                // round x to the nearest one decimal place
                x = Math.Round(x, 1);

                // loop from y = 1 to y = 4 with 0.1 increments
                for (y = 1; y <= 4; y += 0.1)
                {
                    // round y to the nearest one decimal place
                    y = Math.Round(y, 1);

                    // set z using the equation
                    z = Math.Round((3 * y * y) + (2 * x) - 1, 2);
                   
                    // set x, y and z in the #D array 
                    values[xCounter, yCounter, 0] = x;
                    values[xCounter, yCounter, 1] = y;
                    values[xCounter, yCounter, 2] = z;

                    // Print each coordinate, for testing only
                    //Console.WriteLine($"[{values[xCounter, yCounter, 0]}, {values[xCounter, yCounter, 1]}, {values[xCounter, yCounter, 2]}]");

                    // incremenet y counter
                    yCounter++;

                }
                // reset y counter
                yCounter = 0;
                
                // increment x counter
                xCounter++;
            }
        }
    }
}