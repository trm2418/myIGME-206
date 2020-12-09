using System;
using System.Collections.Generic;
using System.Linq;
using ClassLibrary;

namespace Final_Project
{
    class Program
    {
        // seed the Random
        static Random rnd = new Random();

        // Main class, creates the parties and calls the battle method
        class Main2
        {
            

            // Main method, creates parties and calls Battle method
            public static void Main(string[] args)
            {
                GameHandler.Main(args);
            }
        }
    }
}
