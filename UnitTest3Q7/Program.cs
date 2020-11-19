using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest3Q7
{
    class Program
    {
        // Wizard class
        public class Wizard : IComparable<Wizard>
        {
            public string name;
            public int age;

            // constructor
            public Wizard(string name, int age)
            {
                this.name = name;
                this.age = age;
            }

            // compare the age
            public int CompareTo(Wizard other)
            {
                return age.CompareTo(other.age);
            }
        }

        static void Main(string[] args)
        {
            Random rnd = new Random();

            List<Wizard> wizardList = new List<Wizard>();

            // all wizard names
            string[] wizardNames = new string[] { "Dumbledore", "Gandalf", "Merlin", "Timothy", "Jimothy", "Bartholomew", "Morgana", "Griselda", "Gertrudis", "Beatrice" };

            // loop 10 times
            for (int i = 0; i < 10; i++)
            {
                // create a new wizard with a name and random age and add it to wizardList
                wizardList.Add(new Wizard(wizardNames[i], rnd.Next(75, 10000)));
            }

            // ask which way user wants to sort. defaults to normal
            Console.WriteLine("a. Normal b. Delegate c. Lambda");
            string input = Console.ReadLine().ToLower();

            // delegate
            if (input == "b")
            {
                wizardList = wizardList.OrderBy(delegate (Wizard w) { return w.age; }).ToList();
            }
            // lambda
            else if (input == "c")
            {
                wizardList = wizardList.OrderBy(w => w.age).ToList();
            }
            // sort
            else
            {
                wizardList.Sort();
            }

            // display sorted wizardList
            foreach(Wizard wiz in wizardList)
            {
                Console.WriteLine("Name: " + wiz.name + " Age: " + wiz.age);
            }
        }
    }
}
