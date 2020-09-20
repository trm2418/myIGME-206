using System;
using Vehicles;

namespace Traffic
{
    // Class: Program
    // Author: Tyler Machanic
    // Purpose: PE11 Question 6
    // Restrictions: None
    class Program
    {
        // Method: AddPassenger
        // Purpose: Calls LoadPassenger method from IPassengerCarrier
        //          interface and prints the object using ToString()
        // Restrictions: None
        static void AddPassenger(IPassengerCarrier vehicleObject)
        {
            vehicleObject.LoadPassenger();
            Console.WriteLine(vehicleObject.ToString());
        }

        // Method: Main
        // Purpose: Create a passengerTrain object and call AddPassenger method
        // Restrictions: None
        static void Main(string[] args)
        {
            PassengerTrain passengerTrain = new PassengerTrain();
            AddPassenger(passengerTrain);
            //FreightTrain freightTrain = new FreightTrain();
            //AddPassenger(freightTrain);
        }
    }
}
