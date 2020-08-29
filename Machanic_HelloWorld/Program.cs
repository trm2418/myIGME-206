using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machanic_HelloWorld
{
    // Class: Program
    // Author: Tyler Machanic
    // Purpose: Print Hello World! and Practice Basic C#
    // Restrictions: None
    class Program
    {
        // Method: Main
        // Purpose: Print Hello World!, my name, casting and while loop
        // Restrictions: None
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Console.WriteLine("Tyler Machanic");
            String word = "word";
            Console.WriteLine(word);
            String word2 = word;
            int num2 = 2;
            int num3 = num2;
            num3 += 1;
            Console.WriteLine(word);
            while (num2 < 5)
            {
                Console.WriteLine(num2);
                num2++;
            }
        }
    }
}
