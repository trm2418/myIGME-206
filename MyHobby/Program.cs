using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHobby
{
    public interface ITricks
    {
        void Kickflip();
        void Ollie();
    }

    public interface IModify
    {
        void ChangeWheels();
    }

    public abstract class Board
    {
        private bool bigWheels;
        public bool BigWheels
        {
            get { return bigWheels; }
            set { bigWheels = value; }
        }

        public abstract void GoDownHill();

        public virtual void SlideBrake()
        {
            Console.WriteLine("You did a slide brake");
        }
    }

    public class Skateboard : Board, ITricks, IModify
    {
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
        public override void SlideBrake()
        {
            base.SlideBrake();
            Console.WriteLine("You fell over because you can't do slide braktes on skateboards");
        }
        public void Kickflip()
        {
            Console.WriteLine("You did a kickflip.");
        }
        public void Ollie()
        {
            Console.WriteLine("You did an ollie.");
        }
        public void ChangeWheels()
        {
            Console.WriteLine("You put big wheels on your skateboard");
            BigWheels = true;
        }
        public Skateboard()
        {
            BigWheels = false;
        }
    }

    public class Longboard : Board, ITricks, IModify
    {
        public override void GoDownHill()
        {
            Console.WriteLine("You went down a steep hill and survived");
        }
        public void Kickflip()
        {
            Console.WriteLine("You tried to do a kickflip but couldn't get your longboard off the ground.");
        }
        public void Ollie()
        {
            Console.WriteLine("You tried to do an ollie but couldn't get your longboard off the ground.");
        }
        public void ChangeWheels()
        {
            Console.WriteLine("You replaced your longboard's wheels");
        }
        public Longboard()
        {
            BigWheels = true;
        }
    }
    class Program
    {
        static void myMethod(object obj)
        {
            ((Board)obj).GoDownHill();
            ((IModify)obj).ChangeWheels();
            ((Board)obj).GoDownHill();

            ((ITricks)obj).Kickflip();
            ((ITricks)obj).Ollie();

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
            Longboard longboard = new Longboard();
            Skateboard skateboard = new Skateboard();
            myMethod(longboard);
            Console.WriteLine();
            myMethod(skateboard);
        }
    }
}
