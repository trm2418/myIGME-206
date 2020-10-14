using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHobby
{
    // ITricks interface,
    public interface ITricks
    {
        void Kickflip();
        void Ollie();
    }

    // IModify interface
    public interface IModify
    {
        void ChangeWheels();
    }

    // abstract Board class
    public abstract class Board
    {
        // controls whether the Board can go down hills
        private bool bigWheels;

        // accessor for bigWheels
        public bool BigWheels
        {
            get { return bigWheels; }
            set { bigWheels = value; }
        }
        
        // Skateboard and Longboard have different implementations for GoDownHill
        // so this is abstract
        public abstract void GoDownHill();

        // Skateboard has its own implementation for SlideBrake while Longboard
        // uses the default implementation so this is virtual
        public virtual void SlideBrake()
        {
            Console.WriteLine("You did a slide brake");
        }
    }

    // Skateboard class
    public class Skateboard : Board, ITricks, IModify
    {
        // if you changed wheels, you succesfully go down the hill, if not, you crash
        public override void GoDownHill()
        {
            if (BigWheels)
            {
                Console.WriteLine("You went down a steep hill and survived");
            }
            else
            {
                Console.WriteLine("You went down a steep hill and crashed because your wheels aren't big enough");
            }
        }

        // call Board class SlideBrake() method and then fall over
        public override void SlideBrake()
        {
            base.SlideBrake();
            Console.WriteLine("You fell over because you can't do slide brakes on skateboards");
        }
        
        // do a kickflip
        public void Kickflip()
        {
            Console.WriteLine("You did a kickflip.");
        }

        // do an ollie
        public void Ollie()
        {
            Console.WriteLine("You did an ollie.");
        }

        // change to big wheels
        public void ChangeWheels()
        {
            Console.WriteLine("You put big wheels on your skateboard");
            BigWheels = true;
        }

        // constructor sets BigWheels to false by default
        public Skateboard()
        {
            BigWheels = false;
        }
    }

    // Longboard class
    public class Longboard : Board, ITricks, IModify
    {
        // go down a hill successfully
        public override void GoDownHill()
        {
            Console.WriteLine("You went down a steep hill and survived");
        }

        // fail at doing a kickflip
        public void Kickflip()
        {
            Console.WriteLine("You tried to do a kickflip but couldn't get your longboard off the ground.");
        }

        // fail at doing an ollie
        public void Ollie()
        {
            Console.WriteLine("You tried to do an ollie but couldn't get your longboard off the ground.");
        }

        // replace your wheels
        public void ChangeWheels()
        {
            Console.WriteLine("You replaced your longboard's wheels");
        }

        // constructor sets BigWheels to true by default
        public Longboard()
        {
            BigWheels = true;
        }
    }
    class Program
    {
        // call all methods on the object that gets passed in
        static void UseBoard(object obj)
        {
            // Go down a hill, change wheels and try again
            ((Board)obj).GoDownHill();
            ((IModify)obj).ChangeWheels();
            ((Board)obj).GoDownHill();

            // Do a kickflip and an ollie
            ((ITricks)obj).Kickflip();
            ((ITricks)obj).Ollie();

            // cast to Longboard or Skateboard and try a slide brake
            if (obj is Longboard)
            {
                ((Longboard)obj).SlideBrake();
            }
            else if (obj is Skateboard)
            {
                ((Skateboard)obj).SlideBrake();
            }
        }

        static void Main(string[] args)
        {
            // create Longboard and Skateboard objects
            Longboard longboard = new Longboard();
            Skateboard skateboard = new Skateboard();

            // call UseBoard on the Longboard and Skateboard
            UseBoard(longboard);
            Console.WriteLine();
            UseBoard(skateboard);
        }
    }
}
