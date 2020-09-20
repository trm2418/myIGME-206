// File: Vehicle.cs
// Author: Tyler Machanic
// Purpose: define Vehicle family of objects for PE11 Question 5
// Restrictions: None

namespace Vehicles
{
    // abstract vehicle class
    public abstract class Vehicle
    {
        public virtual void LoadPassenger() { }
    }

    // abstract car class
    public abstract class Car : Vehicle
    {

    }

    // passenger carrier interface
    public interface IPassengerCarrier
    {
        void LoadPassenger();
    }

    // heavy load carrier interface
    public interface IHeavyLoadCarrier
    {

    }

    // abstract train class
    public abstract class Train : Vehicle
    {

    }

    // compact class
    public class Compact : Car, IPassengerCarrier
    {

    }

    // SUV class
    public class SUV : Car, IPassengerCarrier
    {

    }

    // pickup class
    public class Pickup : Car, IPassengerCarrier, IHeavyLoadCarrier
    {

    }

    // passenger train class
    public class PassengerTrain : Train, IPassengerCarrier
    {

    }

    // freight train class
    public class FreightTrain : Train, IHeavyLoadCarrier
    {

    }

    // 424 double bogey class
    public class _424DoubleBogey : Train, IHeavyLoadCarrier
    {

    }
}
