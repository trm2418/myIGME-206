using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    // StatusCondition is the base class for each individual status condition
    // Implemented: Burn, Poison, Curse, Bleed, Stun, Freeze, Sleep, Shock
    // Planned: Note: the numbers seperated by slashes correspond to their tier tier1/tier2/tier3
    //          Confuse: 2-3/2-4/3-4 turns, 40/60/80% chance to attack a random target (allies included), 30/45/60% chance to miss enemies
    //          Rage: 2-3/2-4/3-4 turns, forced to use damaging skills, deal 1.5/2/2.5x damage, 50/65/80% chance to hit self
    //          Fear: 2-4/2-5/3-5 turns, deal 30/50/70% less damage, 25/50/75% chance to run and hide in a corner each turn (can't move or be targeted by melee attacks until the fear ends)
    //          Silence: 2-3/3-4/3-5 turns, active abilities can't be used, at tier 3 passive abilities will also be nullified
    //          Beguile: 1-2/2-3/3-4 turns, help the enemy team for the duration of the beguile (you will be able to control enemies and will lose control of heroes)
    public abstract class StatusCondition
    {
        public static Random rnd = new Random();

        // the type of status condition, used to see if there is already an existing status of the same type
        public string type;

        // name of the status condition
        public string name;

        // tier of the status condition
        public int tier;

        // the turns left until the status condition ends
        public int turnsLeft;

        // the battler that the status condition is inflicted on
        public Battler battler;

        public Mod stunDamageIncrease;

        public Mod freezeDamageDecrease;

        public Mod sleepDefenseDecrease;

        public float shockUnableToMoveChance;

        public Mod shockDefenseDecrease;

        public float confuseMissEnemyChance;

        public int confuseAttackRandomChance;

        public Mod rageAttackIncrease;

        public Mod fearAttackDecrease;

        public ConsoleColor color = ConsoleColor.White;

        public void ColorPrint(string str)
        {
            Console.ForegroundColor = color;
            Console.Write(str);
            Console.ResetColor();
        }

        public abstract void Effect(bool endTurn);

        public void DealDamage(int damage)
        {
            damage = (int)Math.Round(damage * battler.statusDamageMultiplier);

            // subtract the damage from the battler's hp, nothing can reduce this damage which is why it doesn't call TakeDamage

            battler.ModifyHp(-damage);

            // print damage message
            Console.WriteLine($"{battler.name} took {damage} damage from {name}!");
        }

        public void ReduceTurns()
        {
            // decrement turnsLeft
            turnsLeft--;

            if (turnsLeft > 0)
            {
                // print remaining turns
                Console.WriteLine($"{turnsLeft} turns remaining on {name}");
            }

            // if no turns are left, remove from battler's status conditions
            if (turnsLeft == 0)
            {
                Remove(true);
            }
        }

        public void Remove(bool cureMessage)
        {
            // print cured message if true
            if (cureMessage)
            {
                Console.WriteLine($"{battler.name} was cured of {name}");
            }

            // remove from battler's status conditions
            battler.statusConditions.Remove(type);

            if (type == "Freeze")
            {
                battler.defenseModifiers.Remove(freezeDamageDecrease);
            }
            else if (type == "Stun")
            {
                battler.defenseModifiers.Remove(stunDamageIncrease);
            }
            else if (type == "Sleep")
            {
                battler.defenseModifiers.Remove(sleepDefenseDecrease);
            }
            else if (type == "Shock")
            {
                battler.defenseModifiers.Remove(shockDefenseDecrease);
            }
            else if (type == "Rage")
            {
                battler.attackModifiers.Remove(rageAttackIncrease);
            }
        }

        // Apply method applies the status condition to the target as long as there isn't a stronger version
        // of the same status condition already applied to that target
        public bool Apply()
        {

            if (battler.immunities.Contains(type))
            {
                Console.WriteLine($"{battler.name} is immune to {type}!");
                return false;
            }

            if (rnd.Next(0, 100) < battler.statusResist)
            {
                Console.WriteLine($"{battler.name} resisted {name}!");
                return false;
            }

            // if the target already has the same type of status condition
            if (battler.statusConditions.ContainsKey(type))
            {
                // set the current status to a new StatusCondition object called current
                StatusCondition status = battler.statusConditions[type];

                // if the existing one is the same tier with a shorter duration or a lower tier
                if ((status.tier == tier && status.turnsLeft < turnsLeft) || status.tier < tier)
                {
                    // inflicted message
                    Console.Write($"{battler.name} was inflicted with ");
                    ColorPrint(name);
                    Console.WriteLine($" for {turnsLeft} turns!");

                    status.Remove(false);

                    // replace the old StatusCondition with the new one
                    battler.statusConditions[type] = this;

                    return true;
                }
                // if the existing one is a higher tier
                else
                {
                    // print message saying they already have a stronger version
                    Console.WriteLine($"{battler.name} is already inflicted with a stronger {name.Split(' ')[0].ToLower()}!");

                    return false;
                }
            }
            // if the target doesn't have a status of the same type
            else
            {
                // inflicted message
                Console.Write($"{battler.name} was inflicted with ");
                ColorPrint(name);
                Console.WriteLine($" for {turnsLeft} turns!");

                // add the new status condition to the battler's StatusCondition dictionary
                battler.statusConditions[type] = this;

                return true;
            }

        }

        // constructor
        public StatusCondition(int tier, Battler battler, string type, ConsoleColor color)
        {
            this.type = type;
            this.tier = tier;
            this.battler = battler;
            name = type + " ";

            // set the name with roman numerals corresponding to the tier
            for (int i = 0; i < tier; i++)
            {
                name += "I";
            }

            this.color = color;
        }
    }

    // Burn does a set amount of damage at the end of each turn
    public class Burn : StatusCondition
    {
        // the damage done each turn
        public float damage;

        // Burn's Effect method is called at the end of each turn
        public override void Effect(bool endTurn = true)
        {
            // set a new int equal to the battler's hp * damage
            int damage = (int)Math.Round(battler.maxHp * this.damage);

            DealDamage(damage);

            // if battler didn't die
            if (!battler.DeathCheck())
            {
                ReduceTurns();
            }
        }

        // constructor
        public Burn(int tier, Battler battler) : base(tier, battler, "Burn", ConsoleColor.Red)
        {
            // set the damage and turnsLeft based on the tier
            switch (tier)
            {
                case 1:
                    // tier 1 does 10% damage for 2-3 turns
                    // 2 turns: 20%, 3 turns: 30%, avg: 25%
                    damage = 0.1f;
                    turnsLeft = rnd.Next(2, 4);
                    break;
                case 2:
                    // tier 2 does 13% damage for 3-4 turns
                    // 3 turns: 39%, 4 turns: 52%, avg: 45.5%
                    damage = 0.13f;
                    turnsLeft = rnd.Next(3, 5);
                    break;
                case 3:
                    // tier 3 does 16% damage for 4-5 turns
                    // 4 turns: 64%, 5 turns: 80%, avg: 72%
                    damage = 0.16f;
                    turnsLeft = rnd.Next(4, 6);
                    break;
            }
        }
    }

    // Poison deals increasing damage at the end of each turn
    public class Poison : StatusCondition
    {
        // the base damage done, gets multiplied by the turnNumber
        public float damage;

        // the amount of turns the battler has had the poison, used to calculate increasing damage
        public int turnNumber;

        // Poison's Effect method is called at the end of each turn
        public override void Effect(bool endTurn = true)
        {
            // set a new int equal to the base damage * turn number
            int damage = (int)Math.Round(battler.maxHp * (this.damage * turnNumber));

            DealDamage(damage);

            // if the battler didn't die
            if (!battler.DeathCheck())
            {
                ReduceTurns();

                // increment turnNumber
                turnNumber++;
            }
        }

        // constructor
        public Poison(int tier, Battler battler) : base(tier, battler, "Poison", ConsoleColor.Magenta)
        {
            // set the turnNumber to 1
            turnNumber = 1;

            // set damage and turnsLeft based on tier
            switch (tier)
            {
                case 1:
                    // tier 1 is 6% damage for 2-3 turns
                    // 6%, 12%, 18% (2 turns: 18%, 3 turns: 36%, avg: 27%)
                    damage = 0.06f;
                    turnsLeft = rnd.Next(2, 4);
                    break;
                case 2:
                    // tier 2 is 7.5% damage for 2-4 turns
                    // 7.5%, 15%, 22.5%, 30% (2 turns: 22.5%, 3 turns: 45%, 4 turns: 75%, avg: 47.5%)
                    damage = 0.075f;
                    turnsLeft = 3;//rnd.Next(2, 5);
                    break;
                case 3:
                    // tier 3 is 9% damage for 3-4 turns
                    // 9%, 18%, 27%, 36% (3 turns: 54%, 4 turns 90%, avg: 72%)
                    damage = 0.09f;
                    turnsLeft = rnd.Next(3, 5);
                    break;
            }
        }
    }

    // Curse does a random amount of damage at the end of each turn
    public class Curse : StatusCondition
    {
        // the minimum damage
        public int minDamage;

        // the maximum damage
        public int maxDamage;

        // the damage dealt
        public float damage;

        // Curse's Effect method is called at the end of each turn
        public override void Effect(bool endTurn = true)
        {
            // set damage to a random int between minDamage and maxDamage / 1000
            this.damage = (float)rnd.Next(minDamage, maxDamage + 1) / 1000;

            // set a new int equal to the battler's maxHp * the random damage
            int damage = (int)Math.Round(battler.maxHp * this.damage);

            DealDamage(damage);

            // if battler didn't die
            if (!battler.DeathCheck())
            {
                ReduceTurns();
            }
        }

        // constructor
        public Curse(int tier, Battler battler) : base(tier, battler, "Curse", ConsoleColor.DarkMagenta)
        {
            // set damage parameters and turnsLeft based on tier
            switch (tier)
            {
                case 1:
                    // tier 1 is 5%-20% damage for 2-3 turns
                    // min: 10%, max: 60%, avg: 31.25%
                    minDamage = 50;
                    maxDamage = 200;
                    turnsLeft = rnd.Next(2, 4);
                    break;
                case 2:
                    // tier 2 is 7.5-30% damage for 2-4 turns
                    // min: 15%, max: 120%, avg: 56.25%
                    minDamage = 75;
                    maxDamage = 300;
                    turnsLeft = rnd.Next(2, 5);
                    break;
                case 3:
                    // tier 3 is 10-40% damage for 3-4 turns
                    // min: 30%, max: 160%, avg: 87.5%
                    minDamage = 100;
                    maxDamage = 400;
                    turnsLeft = rnd.Next(3, 5);
                    break;
            }
        }
    }

    // Bleed deals a set amount of damage at the end of each turn as well as whenever the inflicted battler uses a melee skill
    // Bleed is meant to be a pretty weak status condition because of how common it will be (lots of enemies will be able to inflict it)
    public class Bleed : StatusCondition
    {
        // the damage done whenever it's triggered
        public float damage;

        // Bleed's Effect method is called at the end of each turn and when the inflicted battler uses a melee skill
        // the endTurn bool is whether or not this was called at the end of the turn
        public override void Effect(bool endTurn)
        {
            // set a new int equal to the battler's maxHp * damage
            int damage = (int)(battler.maxHp * this.damage);

            DealDamage(damage);

            // if battler survived and this was triggered from the end of turn
            if (!battler.DeathCheck() && endTurn)
            {
                ReduceTurns();
            }

            // nothing else happens if this was triggered from the battler using a melee skill
        }

        // constructor
        public Bleed(int tier, Battler battler) : base(tier, battler, "Bleed", ConsoleColor.DarkRed)
        {
            // set damage and turnsLeft based on tier
            switch (tier)
            {
                case 1:
                    // tier 1 is 5% damage for 2-3 turns
                    // 2 turns: 10%, 3 turns: 15%, avg: 12.5%
                    damage = 0.05f;
                    turnsLeft = rnd.Next(2, 4);
                    break;
                case 2:
                    // tier 2 is 7% damage for 2-4 turns
                    // 2 turns: 14%, 3 turns: 21%, 4 turns: 28%, avg: 21%
                    damage = 0.07f;
                    turnsLeft = rnd.Next(2, 5);
                    break;
                case 3:
                    // tier 3 is 10% damage for 3-4 turns
                    // 3 turns: 30%, 4 turns: 40%, avg: 35%
                    damage = 0.1f;
                    turnsLeft = rnd.Next(3, 5);
                    break;
            }
        }
    }

    // Stun prevents the inflicted battler from moving and increases the damage done to them
    // Stun is meant to be a pretty weak status condition due to how common it is
    public class Stun : StatusCondition
    {
        // Stun's Effect method is called at the end of each turn
        public override void Effect(bool endTurn = true)
        {
            ReduceTurns();
        }

        // constructor
        public Stun(int tier, Battler battler) : base(tier, battler, "Stun", ConsoleColor.Yellow)
        {
            // set stunDamageIncrease and turnsLeft based on tier
            switch (tier)
            {
                case 1:
                    // tier 1 lasts 1 turn and makes you take 1.5x damage
                    stunDamageIncrease = new Mod("Stun Damage Increase", 2 / 3f);
                    turnsLeft = 1;
                    break;
                case 2:
                    // tier 2 lasts 1 turn and makes you take 2x damage
                    stunDamageIncrease = new Mod("Stun Damage Increase", 0.5f);
                    turnsLeft = 1;
                    break;
                case 3:
                    // tier 3 lasts 2 turns and makes you take 2x damage
                    stunDamageIncrease = new Mod("Stun Damage Increase", 0.5f);
                    turnsLeft = 2;
                    break;
            }
        }
    }

    //Freeze: 1-3/2-3/2-4 turns, can't move, take 50/35/20% reduced damage, taking damage has a 100/50/0% chance to reduce turn count by 1, fire damage has a 100/100/100*% chance to instantly end (*tier 3 only gets turn count reducd by 1)
    public class Freeze : StatusCondition
    {
        public int takeDamageReduceTurnChance;
        public bool fireDamageInstantlyEnds = true;

        // Freeze's Effect method is called at the end of each turn and when damage taken reduces the turn count
        public override void Effect(bool endTurn = true)
        {
            ReduceTurns();
        }

        // constructor
        public Freeze(int tier, Battler battler) : base(tier, battler, "Freeze", ConsoleColor.Cyan)
        {
            // set stats and turnsLeft based on tier
            switch (tier)
            {
                case 1:
                    // tier 1 lasts 1-3 turns, decreases damage taken by 50%, 100% chance to reduce turn count when attacked, and instantly ends when fire damage is taken
                    freezeDamageDecrease = new Mod("Freeze Damage Decrease", 2f);
                    takeDamageReduceTurnChance = 100;
                    turnsLeft = rnd.Next(1, 4);
                    break;
                case 2:
                    // tier 2 lasts 2-3 turns, decreases damage taken by 35%, 50% chance to reduce turn count when attacked, and instantly ends when fire damage is taken
                    freezeDamageDecrease = new Mod("Freeze Damage Decrease", 20 / 13f);
                    takeDamageReduceTurnChance = 50;
                    turnsLeft = rnd.Next(2, 4);
                    break;
                case 3:
                    // tier 3 lasts 2-4 turns, decreases damage taken by 20%, 0% chance to reduce turn count when attacked, and reduces turn count by 1 when fire damage is taken
                    freezeDamageDecrease = new Mod("Freeze Damage Decrease", 1.25f);
                    takeDamageReduceTurnChance = 0;
                    fireDamageInstantlyEnds = false;
                    turnsLeft = rnd.Next(2, 5);
                    break;
            }
        }
    }

    public class Sleep : StatusCondition
    {
        public int wakeIfDamagedChance;

        // Sleep's Effect method is called at the end of each turn
        public override void Effect(bool endTurn = true)
        {
            ReduceTurns();
        }

        // constructor
        public Sleep(int tier, Battler battler) : base(tier, battler, "Sleep", ConsoleColor.DarkCyan)
        {
            // set sleepDefenseDecrease, wakeIfDamagedChance and turnsLeft based on tier
            switch (tier)
            {
                case 1:
                    // tier 1 lasts 1-3 turns, increases damage taken by 25% and has a 100% chance to wake if damaged
                    sleepDefenseDecrease = new Mod("Sleep Defense Decrease", 0.8f);
                    wakeIfDamagedChance = 100;
                    turnsLeft = rnd.Next(1, 4);
                    break;
                case 2:
                    // tier 2 lasts 2-4 turns, increases damage taken by 50% and has a 50% chance to wake if damaged
                    sleepDefenseDecrease = new Mod("Sleep Defense Decrease", 2 / 3f);
                    wakeIfDamagedChance = 50;
                    turnsLeft = rnd.Next(2, 5);
                    break;
                case 3:
                    // tier 3 lasts 2-5 turns, increases damage taken by 75% and has a 25% chance to wake if damaged
                    sleepDefenseDecrease = new Mod("Sleep Defense Decrease", 4 / 7f);
                    wakeIfDamagedChance = 25;
                    turnsLeft = rnd.Next(2, 6);
                    break;
            }
        }

    }

    public class Shock : StatusCondition
    {
        // Shock's Effect method is called at the end of each turn
        public override void Effect(bool endTurn = true)
        {
            ReduceTurns();
        }

        // constructor
        public Shock(int tier, Battler battler) : base(tier, battler, "Shock", ConsoleColor.DarkYellow)
        {
            // set shockDefenseDecrease, unableToMoveChance and turnsLeft based on tier
            switch (tier)
            {
                case 1:
                    // tier 1 lasts 3-5 turns, increases damage taken by 25% and has a 25% chance to be unable to move
                    shockDefenseDecrease = new Mod("Shock Defense Decrease", 0.8f);
                    shockUnableToMoveChance = 25;
                    turnsLeft = rnd.Next(3, 6);
                    break;
                case 2:
                    // tier 1 lasts 4-5 turns, increases damage taken by 35% and has a 35% chance to be unable to move
                    shockDefenseDecrease = new Mod("Shock Defense Decrease", 20 / 27f);
                    shockUnableToMoveChance = 35;
                    turnsLeft = rnd.Next(4, 6);
                    break;
                case 3:
                    // tier 1 lasts 4-6 turns, increases damage taken by 50% and has a 50% chance to be unable to move
                    shockDefenseDecrease = new Mod("Shock Defense Decrease", 2 / 3f);
                    shockUnableToMoveChance = 50;
                    turnsLeft = rnd.Next(4, 7);
                    break;
            }
        }
    }

    public class Confuse : StatusCondition
    {
        public override void Effect(bool endTurn = true)
        {
            ReduceTurns();
        }

        public Confuse(int tier, Battler battler) : base(tier, battler, "Confuse", ConsoleColor.Blue)
        {
            switch (tier)
            {
                case 1:
                    confuseAttackRandomChance = 40;
                    confuseMissEnemyChance = 30;
                    turnsLeft = rnd.Next(2, 4);
                    break;
                case 2:
                    confuseAttackRandomChance = 60;
                    confuseMissEnemyChance = 45;
                    turnsLeft = rnd.Next(2, 5);
                    break;
                case 3:
                    confuseAttackRandomChance = 80;
                    confuseMissEnemyChance = 60;
                    turnsLeft = rnd.Next(3, 5);
                    break;
            }
        }
    }

    public class Rage : StatusCondition
    {
        public int hitSelfChance;

        public override void Effect(bool endTurn = true)
        {
            ReduceTurns();
        }

        public Rage(int tier, Battler battler) : base(tier, battler, "Rage", ConsoleColor.DarkGreen)
        {
            switch (tier)
            {
                case 1:
                    rageAttackIncrease = new Mod("Rage Attack Increase", 1.5f);
                    hitSelfChance = 50;
                    turnsLeft = rnd.Next(2, 4);
                    break;
                case 2:
                    rageAttackIncrease = new Mod("Rage Attack Increase", 2);
                    hitSelfChance = 65;
                    turnsLeft = rnd.Next(2, 5);
                    break;
                case 3:
                    rageAttackIncrease = new Mod("Rage Attack Increase", 2.5f);
                    hitSelfChance = 80;
                    turnsLeft = rnd.Next(3, 5);
                    break;
            }
        }
    }
}
