using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    // BuffDebuff class is the base class for individual attack/defense buffs and debuffs
    // these alter the affected battler's damage output and intake for a few turns
    public class BuffDebuff
    {
        public static Random rnd = new Random();

        // the type of buff/debuff, used to see if there is already an existing buff/debuff of the same type
        public string type;

        // the name of the buff or debuff i.e "+Attack III" or "-Defense V"
        // + at the beginning means it's a buff while - means it's a debuff
        public string name;

        // the console color that this prints in, blue is for buffs, red for debuffs
        public ConsoleColor color;

        // the tier of the buff/debuff
        public int tier;

        // the remaining turns until it goes away
        public int turnsLeft;

        // the attack or defense multiplier of the buff or debuff
        public Mod mult;

        // the battler that the buff or debuff is on
        public Battler battler;

        // if the battler applied a buff to itself this turn
        // this will prevent the buff from losing a turn immediately
        public bool buffedSelf = false;

        public void AddToModifiers()
        {
            if (type == "+Attack" || type == "-Attack")
            {
                battler.attackModifiers.Add(mult);
            }
            else
            {
                battler.defenseModifiers.Add(mult);
            }
        }
        public void RemoveFromModifiers()
        {
            if (type == "+Attack" || type == "-Attack")
            {
                battler.attackModifiers.Remove(mult);
            }
            else
            {
                battler.defenseModifiers.Remove(mult);
            }
        }
        public void ColorPrint(string str)
        {
            Console.ForegroundColor = color;
            Console.Write(str);
            Console.ResetColor();
        }

        // Apply method applies the buff or debuff to the target as long as there isn't a stronger version
        // of the same buff or debuff already applied
        public bool Apply()
        {
            // if the target has a buff or debuff of the same type
            if (battler.buffsDebuffs.ContainsKey(type))
            {
                // set the current buff/debuff to a new BuffDebuff object called current
                BuffDebuff current = battler.buffsDebuffs[type];

                // if it's the same tier with a shorter duration or a lower tier
                if ((current.tier == tier && current.turnsLeft < turnsLeft) || current.tier < tier)
                {
                    // print buff/debuff message
                    Console.Write($"{battler.name} got ");
                    ColorPrint(name);
                    Console.WriteLine($" for {turnsLeft} turns!");

                    // remove the old buff/debuff's multiplier from the user's modifier list
                    current.RemoveFromModifiers();

                    // replace the old buff/debuff with the new one
                    battler.buffsDebuffs[type] = this;

                    // add the new buff/debuff's multiplier to the user's modifier list
                    AddToModifiers();

                    return true;
                }
                else
                {
                    // print message saying the target has a stronger version of the buff/debuff already
                    Console.WriteLine($"{battler.name} already has a stronger {name.Split(' ')[0].ToLower()} buff!");

                    return false;
                }
            }
            // if the target doesn't have a buff or debuff of the same type
            else
            {
                // print buff/debuff message
                Console.Write($"{battler.name} got ");
                ColorPrint(name);
                Console.WriteLine($" for {turnsLeft} turns!");

                // add the new buff/debuff to the battler's buffsDebuffs dictionary
                battler.buffsDebuffs[type] = this;

                // add the new buff/debuff's multiplier to the user's modifier list
                AddToModifiers();

                return true;
            }
        }

        public void TurnEnd()
        {
            // decrement turnsLeft
            turnsLeft--;

            // print remaining turns
            Console.WriteLine($"{turnsLeft} turns remaining on {name}");

            // if no turns left
            if (turnsLeft == 0)
            {
                // print message saying the battler lost the buff/debuff
                Console.Write($"{battler.name} lost ");
                ColorPrint(name + "\n");

                // remove buff/debuff from battler's buffsDebuffs list
                battler.buffsDebuffs.Remove(type);

                // remove the buff's mod from the battler's modifier list
                RemoveFromModifiers();
            }
        }


        // constructor
        public BuffDebuff(int tier, Battler battler, string type)
        {
            this.type = type;
            this.tier = tier;
            this.battler = battler;
            name = type + " ";

            if (name[0].Equals('+'))
            {
                color = ConsoleColor.DarkBlue;
            }
            else
            {
                color = ConsoleColor.DarkRed;
            }

            // set the name with roman numerals according to the tier
            switch (tier)
            {
                case 4:
                    name += "IV";
                    break;
                case 5:
                    name += "V";
                    break;
                default:
                    // for loop for tiers 1-3
                    for (int i = 0; i < tier; i++)
                    {
                        name += "I";
                    }
                    break;
            }
        }
    }



    // Mod class is used for attack and defense modifiers to easily be able to access them from the battler's modifier lists
    public class Mod
    {
        // the name of the Mod i.e. "+Attack I" or "Stun Damage Increase"
        public string name;

        // the multiplier of the Mod
        public float mult;

        // contstructor
        public Mod(string name, float mult)
        {
            this.name = name;
            this.mult = mult;
        }
    }



    // AttackBuff subclass of BuffDebuff
    // increases the affected battler's damage output
    public class AttackBuff : BuffDebuff
    {
        // constructor
        public AttackBuff(int tier, Battler battler) : base(tier, battler, "+Attack")
        {
            // create the Mod and set the turnsLeft based on the tier of the buff
            switch (tier)
            {
                case 1:
                    // tier 1 lasts 2 turns and gives a 1.5x damage boost
                    mult = new Mod(name, 1.5f);
                    turnsLeft = 2;
                    break;
                case 2:
                    // tier 2 lasts 2-3 turns and gives a 1.75x damage boost
                    mult = new Mod(name, 1.75f);
                    turnsLeft = rnd.Next(2, 4);
                    break;
                case 3:
                    // tier 3 lasts 3 turns and gives a 2x damage boost
                    mult = new Mod(name, 2f);
                    turnsLeft = 3;
                    break;
                case 4:
                    // tier 4 lasts 3 turns and gives a 2.5x damage boost
                    mult = new Mod(name, 2.5f);
                    turnsLeft = 3;
                    break;
                case 5:
                    // tier 5 lasts 3-4 turns and gives a 3x damage boost
                    mult = new Mod(name, 3f);
                    turnsLeft = rnd.Next(3, 5);
                    break;
            }
        }
    }

    // AttackDebuff subclass of BuffDebuff
    // decreases the affected battler's damage output
    public class AttackDebuff : BuffDebuff
    {
        public AttackDebuff(int tier, Battler battler) : base(tier, battler, "-Attack")
        {
            switch (tier)
            {
                // these numbers when multiplied by their respective attack buff tier will equal 1
                // a battler with +Attack 5 and -Attack5 will do normal damage
                case 1:
                    // tier 1 lasts 2 turns and gives a 2/3 damage multiplier
                    mult = new Mod(name, 2 / 3f);
                    turnsLeft = 2;
                    break;
                case 2:
                    // tier 2 lasts 2-3 turns and gives a 4/7 damage multiplier
                    mult = new Mod(name, 4 / 7f);
                    turnsLeft = rnd.Next(2, 4);
                    break;
                case 3:
                    // tier 3 lasts 3 turns and gives a 1/2 damage multiplier
                    mult = new Mod(name, 0.5f);
                    turnsLeft = 3;
                    break;
                case 4:
                    // tier 4 lasts 3 turns and gives a 2/5 damage multiplier
                    mult = new Mod(name, 2 / 5f);
                    turnsLeft = 3;
                    break;
                case 5:
                    // tier 5 lasts 3-4 turns and gives a 1/3 damage multiplier
                    mult = new Mod(name, 1 / 3f);
                    turnsLeft = rnd.Next(3, 5);
                    break;
            }
        }
    }

    // DefenseBuff subclass of BuffDebuff
    // decreases the affected battler's damage intake
    public class DefenseBuff : BuffDebuff
    {
        public DefenseBuff(int tier, Battler battler) : base(tier, battler, "+Defense")
        {
            switch (tier)
            {
                // same stats as AttackBuff
                case 1:
                    mult = new Mod(name, 1.5f);
                    turnsLeft = 2;
                    break;
                case 2:
                    mult = new Mod(name, 1.75f);
                    turnsLeft = rnd.Next(2, 4);
                    break;
                case 3:
                    mult = new Mod(name, 2f);
                    turnsLeft = 3;
                    break;
                case 4:
                    mult = new Mod(name, 2.5f);
                    turnsLeft = 3;
                    break;
                case 5:
                    mult = new Mod(name, 3f);
                    turnsLeft = rnd.Next(3, 5);
                    break;
            }
        }
    }

    // DefenseDebuff subclass of BuffDebuff
    // increases the affected battler's damage intake
    public class DefenseDebuff : BuffDebuff
    {
        public DefenseDebuff(int tier, Battler battler) : base(tier, battler, "-Defense")
        {
            switch (tier)
            {
                // same stats as AttackDebuff
                case 1:
                    mult = new Mod(name, 2 / 3f);
                    turnsLeft = 2;
                    break;
                case 2:
                    mult = new Mod(name, 4 / 7f);
                    turnsLeft = rnd.Next(2, 4);
                    break;
                case 3:
                    mult = new Mod(name, 0.5f);
                    turnsLeft = 3;
                    break;
                case 4:
                    mult = new Mod(name, 2 / 5f);
                    turnsLeft = 3;
                    break;
                case 5:
                    mult = new Mod(name, 1 / 3f);
                    turnsLeft = rnd.Next(3, 5);
                    break;
            }
        }
    }
}
