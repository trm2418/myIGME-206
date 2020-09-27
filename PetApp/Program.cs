using System;
using System.Collections.Generic;

namespace PetApp
{
    // Class: Program
    // Author: Tyler Machanic
    // Purpose: PE13
    // Restrictions: None
    class Program
    {
        // abstract Pet class
        public abstract class Pet
        {
            // private string obtained and set by Name accessor
            private string name;

            public int age;

            // accessor for name
            public string Name
            {
                get { return name; }
                set { name = value; }
            }

            // method signatures
            public abstract void Eat();

            public abstract void Play();

            public abstract void GotoVet();

            // constructor with 0 parameters
            public Pet() { }

            // constructor with 2 parameters
            public Pet(string name, int age)
            {
                this.name = name;
                this.age = age;
            }
        }

        // IDog interface
        public interface IDog
        {
            // method signatures
            void Eat();
            void Play();
            void Bark();
            void NeedWalk();
            void GotoVet();
        }

        // ICat interface
        public interface ICat
        {
            // method signatures
            void Eat();
            void Play();
            void Scratch();
            void Purr();
        }

        // Pets class used to store the list of pets
        public class Pets
        {

            // List of pets
            public List<Pet> petList = new List<Pet>();

            // indexer
            public Pet this[int nPetEl]
            {
                get
                {
                    // stores Pet that will be returned
                    Pet returnVal;
                    
                    try
                    {
                        returnVal = (Pet)petList[nPetEl];
                    }
                    // return null if there is nothing at that index
                    catch
                    {
                        returnVal = null;
                    }
                    return returnVal;
                }

                set
                {
                    // if the index is less than the number of list elements
                    if (nPetEl < petList.Count)
                    {
                        // update the existing value at that index
                        petList[nPetEl] = value;
                    }
                    else
                    {
                        // add the value to the list
                        petList.Add(value);
                    }
                }
            }

            // get the number of pets in PetList;
            public int Count
            {
                get { return petList.Count; }
            }

            // adds a pet to the list
            public void Add(Pet pet)
            {
                petList.Add(pet);
            }

            // removes a pet from the list
            public void Remove(Pet pet)
            {
                petList.Remove(pet);
            }

            // removes the pet at the specified index
            public void RemoveAt(int nPetEl)
            {
                petList.RemoveAt(nPetEl);
            }
            
        }

        // Cat class which inherits from Pet and ICat
        public class Cat : Pet, ICat
        {
            // prints eat message
            public override void Eat()
            {
                Console.WriteLine($"{Name} ate some cat food.");
            }

            // prints play message
            public override void Play()
            {
                Console.WriteLine($"{Name} played with some yarn.");
            }

            // prints purr message
            public void Purr()
            {
                Console.WriteLine($"{Name} purred questioningly.");
            }

            // prints scratch message
            public void Scratch()
            {
                Console.WriteLine($"{Name} scratched angrily.");
            }

            // prints go to vet message
            public override void GotoVet()
            {
                Console.WriteLine($"{Name} went to the vet. They were not happy about it.");
            }

            // constructor
            public Cat()
            {

            }
        }

        // Dog class which inherits from Pet and IDog
        public class Dog : Pet, IDog
        {
            public string license;

            // prints eat message
            public override void Eat()
            {
                Console.WriteLine($"{Name} ate some dog food.");
            }

            // prints play message
            public override void Play()
            {
                Console.WriteLine($"{Name} played with a chew toy.");
            }

            // prints bark message
            public void Bark()
            {
                Console.WriteLine($"{Name} barked loudly.");
            }

            // prints need walk message
            public void NeedWalk()
            {
                Console.WriteLine($"{Name} whined because they needed to walk.");
            }

            // prints go to vet message
            public override void GotoVet()
            {
                Console.WriteLine($"{Name} went to the vet. They were not happy about it.");
            }

            // constructor
            public Dog(string szLicense, string szName, int nAge) : base(szName, nAge) { }
        }

        // Method: Main
        // Purpose: Loop 50 times. Create a new pet 1/10 times and call a random method
        //          on an existing pet the other 9/10 times.
        // Restrictions: Age input needs to be int parseable 
        static void Main(string[] args)
        {
            // declare necessary objects
            Pet thisPet = null;
            Dog dog = null;
            Cat cat = null;
            IDog iDog = null;
            ICat iCat = null;

            // create pet list
            Pets pets = new Pets();

            // seed random
            Random rand = new Random();

            // loop 50 times
            for (int i = 0; i < 50; i++)
            {
                // 1 in 10 chance of adding an animal
                if (rand.Next(1, 11) == 1)
                {
                    // 50% chance of adding a dog
                    if (rand.Next(0, 2) == 0)
                    {
                        Console.WriteLine("You bought a dog!");

                        // declare constructor variables
                        string dogName;
                        int dogAge;
                        string dogLicense;

                        // get user input for constructor variables
                        Console.Write("Dog's Name = > ");
                        dogName = Console.ReadLine();
                        Console.Write("Age => ");
                        dogAge = int.Parse(Console.ReadLine());
                        Console.Write("License => ");
                        dogLicense = Console.ReadLine();

                        // create Dog object
                        dog = new Dog(dogLicense, dogName, dogAge);

                        // add dog to pet list
                        pets.Add(dog);
                    }
                    // else add a cat
                    else
                    {
                        Console.WriteLine("You bought a cat!");

                        // create Cat object
                        cat = new Cat();

                        // get user input to set fields
                        Console.Write("Cat's Name => ");
                        cat.Name = Console.ReadLine();
                        Console.Write("Age => ");
                        cat.age = int.Parse(Console.ReadLine());

                        // add cat to pet list
                        pets.Add(cat);
                    }
                }
                // the other 9/10 times
                else
                {
                    // set this pet to a random pet from pet list
                    thisPet = pets[rand.Next(0, pets.Count)];

                    // if pet list isn't empty
                    if (thisPet != null)
                    {
                        // check if pet is a cat
                        if (thisPet.GetType() == typeof(Cat))
                        {
                            // cast to ICat
                            iCat = (ICat)thisPet;

                            // call a random method from ICat
                            switch (rand.Next(1,5))
                            {
                                case 1:
                                    iCat.Eat();
                                    break;
                                case 2:
                                    iCat.Play();
                                    break;
                                case 3:
                                    iCat.Scratch();
                                    break;
                                case 4:
                                    iCat.Purr();
                                    break;
                            }
                        }
                        // if the pet is a dog
                        else
                        {
                            // cast to IDog
                            iDog = (IDog)thisPet;

                            // call a random method from IDog
                            switch (rand.Next(1,6))
                            {
                                case 1:
                                    iDog.Eat();
                                    break;
                                case 2:
                                    iDog.Play();
                                    break;
                                case 3:
                                    iDog.Bark();
                                    break;
                                case 4:
                                    iDog.NeedWalk();
                                    break;
                                case 5:
                                    iDog.GotoVet();
                                    break;
                            }
                        }
                    }
                }
            }
        }
    }
}
