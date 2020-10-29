using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    // Party class that stores the user's party of heroes and the random party of enemies on each floor
    public class Party
    {
        public static Random rnd = new Random();

        public string name;

        // Battler array that stores each member of the party and their position
        public Battler[] members = new Battler[8];

        // keeps track of which member gets to move next by
        public int whoseMove = 0;

        public bool hasSentinel = false;
        public bool hasPaladin = false;

        public void PrintAsTable(Party otherParty)
        {
            int maxChars = 24;

            Console.WriteLine("\n\n");
            for (int i = 0; i < 8; i += 2)
            {

                Battler hb = members[i + 1];
                Battler hf = members[i];
                Battler ef = otherParty.members[i];
                Battler eb = otherParty.members[i + 1];
                Battler[] battlers = new Battler[] { hb, hf, ef, eb };

                for (int j = 0; j < 4; j++)
                {
                    if (battlers[j] == null)
                    {
                        battlers[j] = new Battler("None", 0, 0, 0, false);
                    }
                }

                hb = battlers[0];
                hf = battlers[1];
                ef = battlers[2];
                eb = battlers[3];

                int nullCounter = 0;

                foreach (string str in new string[] { i + 2 + ". " + hb.name, i + 1 + ". " + hf.name, i + 1 + ". " + ef.name, i + 2 + ". " + eb.name })
                {
                    if (str.Contains("Shadowalker") && GetMemberByName("Shadowalker").stealth)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                    }
                    Console.Write(str);
                    Console.ResetColor();

                    for (int j = str.Length; j < maxChars; j++)
                    {
                        Console.Write(" ");
                    }
                    if (nullCounter == 1)
                    {
                        Console.Write("\t\t");
                    }
                    nullCounter++;
                }
                Console.WriteLine();

                string[,] hpMana = new string[4, 2];

                for (int n = 0; n < 4; n++)
                {
                    if (battlers[n].maxHp != 0)
                    {
                        hpMana[n, 0] = battlers[n].hp.ToString() + "/" + battlers[n].maxHp.ToString();
                        hpMana[n, 1] = battlers[n].mana.ToString();
                    }
                    else
                    {
                        hpMana[n, 0] = "";
                        hpMana[n, 1] = "";
                    }
                }

                for (int p = 0; p < 4; p++)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.Write(hpMana[p, 0]);
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write(" " + hpMana[p, 1]);

                    for (int j = hpMana[p, 0].Length + hpMana[p, 1].Length + 1; j < maxChars; j++)
                    {
                        Console.Write(" ");
                    }
                    if (p == 1)
                    {
                        Console.Write("\t\t");
                    }
                }
                Console.ResetColor();
                Console.WriteLine();

                int counter = 0;

                foreach (Battler battler in new Battler[] { hb, hf, ef, eb })
                {
                    foreach (KeyValuePair<string, StatusCondition> status in battler.statusConditions)
                    {
                        if (counter + 6 > maxChars)
                        {
                            Console.Write("...");
                            counter += 3;
                            break;
                        }
                        status.Value.ColorPrint(status.Value.name[0] + status.Value.tier.ToString() + " ");
                        counter += 3;
                    }

                    foreach (KeyValuePair<string, BuffDebuff> buffDebuff in battler.buffsDebuffs)
                    {
                        if (counter + 6 > maxChars)
                        {
                            Console.Write("...");
                            counter += 3;
                            break;
                        }
                        buffDebuff.Value.ColorPrint(buffDebuff.Value.name.Substring(0, 2) + buffDebuff.Value.tier.ToString() + " ");
                        counter += 4;
                    }

                    for (int j = counter; j < maxChars; j++)
                    {
                        Console.Write(" ");
                    }
                    if (battler.Equals(hf))
                    {
                        Console.Write("\t\t");
                    }
                    counter = 0;
                }

                Console.WriteLine("\n");
            }
        }

        public int GetFirstIndex()
        {
            for (int i = 0; i < 8; i++)
            {
                if (members[i] != null)
                {
                    return i;
                }
            }
            return 0;
        }

        public int GetLastIndex()
        {
            for (int i = 7; i >= 0; i--)
            {
                if (members[i] != null)
                {
                    return i;
                }
            }
            return 7;
        }

        // call the move function for whoever's move it is
        public void Move(Party otherParty)
        {
            Battler turn = members[whoseMove];

            if (whoseMove == 0 && name == "heroes")
            {
                for (int i = 0; i < 8; i++)
                {
                    if (members[i] != null)
                    {
                        members[i].ManaRegen();
                    }
                    if (otherParty.members[i] != null)
                    {
                        otherParty.members[i].ManaRegen();
                    }
                }

                if (hasSentinel)
                {
                    Sentinel sentinel = (Sentinel)GetMemberByName("Sentinel");
                    if (sentinel.saviorCooldown == 1)
                    {
                        sentinel.saviorCooldown--;
                        Console.WriteLine($"{sentinel.name} can save an ally again!");
                    }
                    else if (sentinel.saviorCooldown == 2)
                    {
                        sentinel.saviorCooldown--;
                    }
                }
                if (hasPaladin)
                {
                    Paladin paladin = (Paladin)GetMemberByName("Paladin");

                    foreach (Battler member in paladin.DivineBlessingTargets())
                    {
                        if (member != null)
                        {
                            member.Heal(paladin.divineBlessing);
                        }
                    }
                }
            }

            // if the member in position whoseMove isn't null
            // this will only run when there is a Battler in the spot
            // this is so that if there are an unven number of Battlers in each party
            // they will only get to move x times per round where x is the number of 
            // their remaining Battlers
            if (turn != null)
            {
                // call the Move method
                turn.Move(otherParty);

                turn.sentinelSave = false;

                // new line between hero and enemy turns
                Console.WriteLine();
            }

            // increment whoseMove if member is null or the member has used their move
            whoseMove++;

            // reset whoseMove to 0 if it reaches 8
            if (whoseMove == 8)
            {
                whoseMove = 0;

                // triple new line to indicate a new round
                Console.WriteLine("\n\n");
            }
        }

        // return a random Battler from the non null members
        public Battler RandomMember()
        {

            // get a random member
            Battler member = members[rnd.Next(0, members.Length)];

            // repeat until member is not null
            while (member == null)
            {
                member = members[rnd.Next(0, members.Length)];
            }

            // return the random member
            return member;
        }

        public Battler GetRandomMemberFromEitherParty(Party otherParty)
        {
            Battler[] allBattlers = new Battler[16];

            // copy both parties' members array to allBattlers
            members.CopyTo(allBattlers, 0);
            otherParty.members.CopyTo(allBattlers, 8);

            // get a random battler
            Battler battler = allBattlers[rnd.Next(0, 16)];

            // repeat until battler is not null
            while (battler == null)
            {
                battler = allBattlers[rnd.Next(0, members.Length)];
            }

            // return the random battler
            return battler;
        }

        // return the indexes of all non null members as an int array
        public int[] GetAllLiving()
        {

            // stores number of living members
            int counter = 0;

            // loop through members
            foreach (Battler member in members)
            {

                // increment count if member is not null
                if (member != null)
                {
                    counter++;
                }
            }

            // create new array of size count to store the indexes of living members
            int[] livingMembers = new int[counter];

            // reset counter to 0 since it will be used in the next loop
            counter = 0;

            // loop from 0 to 7
            for (int i = 0; i < 8; i++)
            {

                // if the member at the i value isn't null, store i in livingMembers
                if (members[i] != null)
                {
                    livingMembers[counter] = i;

                    // increment counter to move to the next index of livingMembers
                    counter++;
                }
            }

            // return int array of indexes of living members
            return livingMembers;
        }

        public Battler GetMemberByName(string name)
        {
            foreach (Battler member in members)
            {
                if (member != null)
                {
                    if (member.name == name)
                    {
                        return member;
                    }
                }
            }
            return null;
        }

        public Battler GetWeakest(List<Battler> exceptFor)
        {
            Battler current = null;

            foreach (Battler member in members)
            {
                if (member != null && !exceptFor.Contains(member))
                {
                    current = member;
                    break;
                }
            }

            foreach (Battler member in members)
            {
                if (member != null)
                {
                    if (member.hp < current.hp && !exceptFor.Contains(member))
                    {
                        current = member;
                    }
                }
            }

            return current;

        }

        // checks if party has no more living members
        public bool IsDefeated()
        {

            // loop through each member
            foreach (Battler member in members)
            {

                // if the member is not null, meaning there is a living member, return false to exit
                if (member != null)
                {
                    return false;
                }
            }

            // if no living members were found, return true
            return true;
        }

        public Party(string name)
        {
            this.name = name;

        }
    }
}
