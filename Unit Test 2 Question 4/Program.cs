using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Unit_Test_2_Question_4
{
    // Phone abstract class
    public abstract class Phone
    {
        private string phoneNumber;
        public string PhoneNumber { get; set; }
        public string address;
        public abstract void Connect();
        public abstract void Disconnect();
    }

    // PhoneInterface interface
    public interface PhoneInterface
    {
        void Answer();
        void MakeCall();
        void HangUp();
    }

    // RotaryPhone class
    public class RotaryPhone : Phone, PhoneInterface
    {
        public void Answer() { }
        public void MakeCall() { }
        public void HangUp() { }
        public override void Connect() { }
        public override void Disconnect() { }
    }

    // PushButtonPhone class
    public class PushButtonPhone : Phone, PhoneInterface
    {
        public void Answer() { }
        public void MakeCall() { }
        public void HangUp() { }
        public override void Connect() { }
        public override void Disconnect() { }
    }

    // Tardis class
    public class Tardis : RotaryPhone
    {
        private bool sonicScrewdriver;
        private byte whichDrWho;
        public byte WhichDrWho { get; }
        private string femaleSideKick;
        public string FemaleSideKick { get; }
        public double exteriorSurfaceArea;
        public double interiorVolume;

        // overload operators. 10 is the best so 10 will always be greater than
        // anything besides itself
        public static bool operator <(Tardis tardis1, Tardis tardis2)
        {
            byte t1 = tardis1.whichDrWho;
            byte t2 = tardis2.whichDrWho;
            if (t1 == 10 && t2 == 10)
            {
                return false;
            }
            else if (t1 == 10)
            {
                return true;
            }
            else if (t2 == 10)
            {
                return false;
            }
            else
            {
                return (t1 < t2);
            }
        }

        public static bool operator >(Tardis tardis1, Tardis tardis2)
        {
            byte t1 = tardis1.whichDrWho;
            byte t2 = tardis2.whichDrWho;
            if (t1 == 10 && t2 == 10)
            {
                return false;
            }
            else if (t1 == 10)
            {
                return false;
            }
            else if (t2 == 10)
            {
                return true;
            }
            else
            {
                return (t1 > t2);
            }
        }

        public static bool operator <=(Tardis tardis1, Tardis tardis2)
        {
            byte t1 = tardis1.whichDrWho;
            byte t2 = tardis2.whichDrWho;
            if (t1 == 10 && t2 == 10)
            {
                return true;
            }
            else if (t1 == 10)
            {
                return true;
            }
            else if (t2 == 10)
            {
                return false;
            }
            else
            {
                return (t1 <= t2);
            }
        }
        public static bool operator >=(Tardis tardis1, Tardis tardis2)
        {
            byte t1 = tardis1.whichDrWho;
            byte t2 = tardis2.whichDrWho;
            if (t1 == 10 && t2 == 10)
            {
                return true;
            }
            else if (t1 == 10)
            {
                return false;
            }
            else if (t2 == 10)
            {
                return true;
            }
            else
            {
                return (t1 >= t2);
            }
        }

        public static bool operator ==(Tardis t1, Tardis t2)
        {
            return (t1.whichDrWho == t2.whichDrWho);
        }

        public static bool operator !=(Tardis t1, Tardis t2)
        {
            return (t1.whichDrWho != t2.whichDrWho);
        }


        public void TimeTravel() { }
    }

    // PhoneBooth class
    public class PhoneBooth : PushButtonPhone
    {
        private bool superMan;
        public double costPerCall;
        public bool phoneBook;
        public void OpenDoor() { }
        public void CloseDoor() { }
    }

    class Program
    {
        static void UsePhone(object obj)
        {
            // cast to PhoneInterface to call PhoneInterface methods
            ((PhoneInterface)obj).MakeCall();
            ((PhoneInterface)obj).HangUp();

            // if the object is a PhoneBooth
            if (obj is PhoneBooth)
            {
                // cast to PhoneBooth and call OpenDoor()
                ((PhoneBooth)obj).OpenDoor();
            }
            // else if the object is a Tardis
            else if (obj is Tardis)
            {
                // cast to Tardis and call TimeTravel()
                ((Tardis)obj).TimeTravel();
            }
            
        }

        static void Main(string[] args)
        {
            // create Tardis and PhoneBooth objects
            Tardis tardis = new Tardis();
            PhoneBooth phoneBooth = new PhoneBooth();

            // call UsePhone on both objects
            UsePhone(tardis);
            UsePhone(phoneBooth);
        }
    }
}
