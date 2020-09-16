using System;

namespace UnitTest1Question12
{
    // Class: Program
    // Author: Tyler Machanic
    // Purpose: Unit Test 1 Question 12
    // Restrictions: None
    class Program
    {
        // Method: Main
        // Purpose: Prompt user for name, call GiveRaise, congratulate if you got a raise
        //          Output: Congratulations messaage and new salary if you got a raise
        // Restrictions: None
        static void Main(string[] args)
        {
            // store user's name
            string sName;

            // store salary
            double dSalary = 30000;

            // prompt for name
            Console.Write("Enter your name: ");

            // set name variable to user input
            sName = Console.ReadLine();
            
            // call GiveRaise
            if (GiveRaise(sName, ref dSalary))
            {
                // if you got a raise, congratulate and print new salary
                Console.WriteLine($"Congratulations! Your new salary is ${dSalary}");
            }
        }

        // Method: GiveRaise
        // Purpose: Give user raise if name is Tyler
        // Restrictions: User should not enter last name
        static bool GiveRaise(string name, ref double salary)
        {
            // if name is Tyler
            if (name.ToLower() == "tyler")
            {
                // raise salary
                salary += 19999.99;

                // return true to run the if statement in main
                return true;
            }
            // return false and don't run the if statement in main
            return false;
        }
    }
}
