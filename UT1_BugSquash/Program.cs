using System;

namespace UT1_BugSquash
{
    class Program
    {
        // Calculate x^y for y > 0 using a recursive function
        static void Main(string[] args)
        {
            string sNumber;
            int nX;
            // compile time eror, missing semicolon
            //int nY
            int nY;
            int nAnswer;

            // compile time error, missing quotes
            //Console.WriteLine(This program calculates x ^ y.);
            Console.WriteLine("This program calculates x ^ y.");

            do
            {
                Console.Write("Enter a whole number for x: ");
                // compile time error, sNumber isn't assigned a value
                //Console.ReadLine();
                sNumber = Console.ReadLine();
            } while (!int.TryParse(sNumber, out nX));

            do
            {
                Console.Write("Enter a positive whole number for y: ");
                sNumber = Console.ReadLine();
            // logical error, should be ! and nY instead of nX
            //} while (int.TryParse(sNumber, out nX));
            } while (!int.TryParse(sNumber, out nY));

            // compute the factorial of the number using a recursive function
            nAnswer = Power(nX, nY);

            // logical error, doesn't print variables, needs $
            //Console.WriteLine("{nX}^{nY} = {nAnswer}");
            Console.WriteLine($"{nX}^{nY} = {nAnswer}");
        }

        // compile time error, Power method needs to be static
        //int Power(int nBase, int nExponent)
        static int Power(int nBase, int nExponent)
        {
            int returnVal = 0;
            int nextVal = 0;

            // the base case for exponents is 0 (x^0 = 1)
            if (nExponent == 0)
            {
                // return the base case and do not recurse
                // logical error, this will make the answer always be 0 so it needs to be 1 instead
                //returnVal = 0;
                returnVal = 1;

                // compile time error, missing return statement
                return returnVal;
            }
            else
            {
                // compute the subsequent values using nExponent-1 to eventually reach the base case
                // run time error, causes stack overflow because exponent increases forever instead of
                // decreasing and reaching the base case
                //nextVal = Power(nBase, nExponent + 1);
                nextVal = Power(nBase, nExponent - 1);

                // multiply the base with all subsequent values
                returnVal = nBase * nextVal;
            }

            // compile time error, needs to say return before
            //returnVal;
            return returnVal;
        }
    }
}
