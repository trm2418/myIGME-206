using System;

namespace UnitTest1Question13
{
    // Class: Program
    // Author: Tyler Machanic
    // Purpose: Unit Test 1 Question 13
    // Restrictions: None
    class Program
    {
        // employee struct that stores name and salary
        struct employee
        {
            public string sName;
            public double dSalary;
        }

        // Method: Main
        // Purpose: Prompt user for name, call GiveRaise, congratulate if you got a raise
        //          Output: Congratulations messaage and new salary if you got a raise
        // Restrictions: None
        static void Main(string[] args)
        {
            // create new employee object
            employee employee = new employee();

            // set salary to 30000
            employee.dSalary = 30000;

            // prompt for name
            Console.Write("Enter your name: ");

            // set name to user input
            employee.sName = Console.ReadLine();

            // call GiveRaise with a reference to employee so it can be changed
            if (GiveRaise(ref employee))
            {
                // if you got a raise, congratulate and print new salary
                Console.WriteLine($"Congratulations! Your new salary is ${employee.dSalary}");
            }
        }

        // Method: GiveRaise
        // Purpose: Give user raise if name is Tyler
        // Restrictions: User should not enter last name
        static bool GiveRaise(ref employee employee)
        {
            // if name is Tyler
            if (employee.sName.ToLower() == "tyler")
            {
                // raise salary
                employee.dSalary += 19999.99;

                // return true to run the if statement in main
                return true;
            }
            // return false and don't run the if statement in main
            return false;
        }
    }
}
