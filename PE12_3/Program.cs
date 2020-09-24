using System;

namespace PE12_3
{
    // Class: Program
    // Purpose: PE12 Question 3
    // Author: Tyler Machanic
    // Restrictions: None
    class Program
    {
        // MyClass base class
        public class MyClass
        {
            private string myString;
            
            // write only property to set myString
            public string MyString
            {
                set { myString = value; }
            }

            // returns myString
            public virtual string GetString()
            {
                return myString;
            }
        }

        // MyDerivedClass derived from MyClass
        public class MyDerivedClass : MyClass
        {
            // overriden GetString method
            public override string GetString()
            {
                return base.GetString() + " (output from the derived class)";
            }
        }

        // Method: Main
        // Purpose: Instantiate a MyDerivedClass object and call GetString()
        // Restrictions: None
        static void Main(string[] args)
        {
            // instantiate a MyDerivedClass object
            MyDerivedClass derived = new MyDerivedClass();

            // print overriden GetString method
            Console.WriteLine(derived.GetString());
        }
    }
}
