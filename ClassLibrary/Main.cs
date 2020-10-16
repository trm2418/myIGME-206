using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Media;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ClassLibrary
{
    


    // Game Handler class, creates the parties and calls the battle method
    public class GameHandler
    {
        public static Dungeon dungeon;

        public static Battler[] heroArray = new Battler[8];

        public static Party heroParty = new Party("heroes");

        public static Party enemyParty = new Party("enemies");

        public static List<Battler> availableHeroes = new List<Battler>() { new Player(), new Barbarian(), new Ranger(), new Sentinel(), new WitchDoctor(), new Paladin(), new Sorcerer(), new DeathKnight(), new Shadowalker() };

        public static Dictionary<Equipment, int> inventory = new Dictionary<Equipment, int>() { { new Equipment("Iron Sword", "weapon", 0, 100, 0), 1 } };

        public static List<Dungeon> dungeons = new List<Dungeon>() { new Forest() };

        public static SoundPlayer backgroundMusic = new SoundPlayer("../../music/background music.wav");
        public static SoundPlayer bossMusic = new SoundPlayer("../../music/boss music.wav");
        public static SoundPlayer menuMusic = new SoundPlayer("../../music/menu music.wav");

        public static long gold = 0;


        public static void Village()
        {
            string input;
            int spot = 0;
            int newSpot = 0;
            Battler selectedHero;

            foreach (Battler hero in heroArray)
            {
                if (hero != null)
                {
                    hero.Spawn();
                }
            }

            do
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine(string.Format("{0:n0}", gold) + "G");
                Console.ResetColor();
                Console.WriteLine("1. Manage Party 2. Enter Dungeon");
                input = Console.ReadLine();
            } while (!new string[] { "1", "2" }.Contains(input));

            if (input == "1")
            {
                do
                {
                    spot = 0;

                    // displays the hero array
                    for (int i = 0; i < 8; i += 2)
                    {
                        string name1 = "None";
                        string name2 = "None";

                        if (heroArray[i + 1] != null)
                        {
                            name1 = heroArray[i + 1].name;
                        }
                        if (heroArray[i] != null)
                        {
                            name2 = heroArray[i].name;
                        }

                        Console.Write($"{i + 2}. {name1}");

                        for (int j = name1.Length + 3; j < 15; j++)
                        {
                            Console.Write(" ");
                        }

                        Console.WriteLine($"{i + 1}. {name2}");
                    }

                    Console.WriteLine("9. Cancel");

                    do
                    {
                        try
                        {
                            Console.Write(">");
                            spot = int.Parse(Console.ReadLine());
                        }
                        catch { }
                    } while (spot < 1 || spot > 9);

                    if (spot == 9)
                    {
                        input = "";
                    }
                    else if (heroArray[spot - 1] == null)
                    {
                        selectedHero = null;

                        Console.WriteLine("Which hero would you like to place?");

                        for (int i = 0; i < availableHeroes.Count; i++)
                        {
                            if (!heroArray.Contains(availableHeroes[i]))
                            {
                                Console.Write($"{i + 1}. {availableHeroes[i].name} ");
                            }
                        }

                        Console.WriteLine();

                        do
                        {
                            try
                            {
                                Console.Write(">");

                                selectedHero = availableHeroes[int.Parse(Console.ReadLine()) - 1];
                            }
                            catch { }
                        } while (selectedHero == null || heroArray.Contains(selectedHero));
                        

                        heroArray[spot - 1] = selectedHero;
                        selectedHero.partySpot = spot - 1;
                    }
                    else
                    {
                        selectedHero = heroArray[spot - 1];

                        Console.WriteLine("What would you like to do with " + selectedHero.name + "?");
                        Console.WriteLine("1. Move 2. Remove 3. Details 4. Equipment");
                        do
                        {
                            Console.Write(">");
                            input = Console.ReadLine();
                        } while (!new string[] { "1", "2", "3", "4" }.Contains(input));

                        if (input == "1")
                        {
                            Console.WriteLine("Which spot?");
                            newSpot = 0;
                            do
                            {
                                try
                                {
                                    Console.Write(">");
                                    newSpot = int.Parse(Console.ReadLine());
                                }
                                catch { }
                            } while (newSpot < 1 || newSpot > 8);

                            if (heroArray[newSpot - 1] == null)
                            {
                                heroArray[newSpot - 1] = selectedHero;
                                selectedHero.partySpot = newSpot - 1;
                                heroArray[spot - 1] = null;
                            }
                            else
                            {
                                Battler temp = heroArray[newSpot - 1];
                                heroArray[newSpot - 1] = selectedHero;
                                selectedHero.partySpot = newSpot - 1;
                                heroArray[spot - 1] = temp;
                                temp.partySpot = spot - 1;
                            }
                        }
                        else if (input == "2")
                        {
                            heroArray[spot - 1] = null;
                            availableHeroes.Add(selectedHero);
                        }
                        else if (input == "3")
                        {
                            Console.WriteLine($"{selectedHero.name} lvl 1\n{selectedHero.maxHp}HP, {selectedHero.atk}atk, {selectedHero.def}def");
                        }
                        else if (input == "4")
                        {
                            string[] equipmentSlots = new string[] { "Weapon", "Shield", "Helmet", "Armor", "Boots", "Trinket" };
                            for (int i = 1; i < 7; i++)
                            {
                                Console.Write(i + ". " + equipmentSlots[i - 1] + ": ");
                                try
                                {
                                    Console.WriteLine(selectedHero.loadout[equipmentSlots[i - 1].ToLower()].name);
                                }
                                catch
                                {
                                    Console.WriteLine("None");
                                }
                            }

                            int selection = 0;
                            do
                            {
                                try
                                {
                                    Console.Write(">");
                                    selection = int.Parse(Console.ReadLine());
                                }
                                catch { }
                            } while (selection < 1 || selection > 6);

                            foreach (Equipment equipment in inventory.Keys)
                            {
                                Console.Write(equipment.name + " x" + inventory[equipment] + " ");
                            }

                            inventory.Keys.First().Equip(selectedHero);
                        }
                    }

                    for (int i = 0; i < heroArray.Length; i++)
                    {
                        if (heroArray[i] != null)
                        {
                            heroArray[i].Spawn();
                        }
                        else
                        {
                            heroParty.members[i] = null;
                        }
                    }


                } while (input != "");

            }
            else if (input == "2")
            {
                bool valid = false;

                foreach (Battler battler in heroArray)
                {
                    if (battler != null)
                    {
                        valid = true;
                    }
                }

                if (!valid)
                {
                    Console.WriteLine("You need at least one party member to enter the dungeon");
                }
                else
                {
                    foreach (Battler hero in heroParty.members)
                    {
                        if (hero != null)
                        {
                            hero.hp = hero.maxHp;
                            hero.statusConditions.Clear();
                            hero.buffsDebuffs.Clear();


                            // party hasHero bools
                            if (hero.name == "Sentinel")
                            {
                                heroParty.hasSentinel = true;
                            }
                            else if (hero.name == "Paladin")
                            {
                                heroParty.hasPaladin = true;
                            }
                        }
                    }
                    if (dungeons.Count == 1)
                    {
                        dungeon = dungeons[0];
                    }
                    dungeon.DungeonLoop();
                }
            }


        }

        // Main method, creates parties and calls Battle method
        public static void Main(string[] args)
        {
            menuMusic.PlayLooping();
            while (true)
            {
                Village();
            }
        }
    }
}
