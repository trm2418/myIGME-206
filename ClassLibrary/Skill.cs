using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    // Skill class is the base class for each individual skill
    // Skills are defined in the individual Battler subclasses. Attack and Defend are defined in the Battler class
    // Skills are everything a battler can do including the default attack and defend options
    public class Skill
    {
        // seed the Random
        public static Random rnd = new Random();

        // name of skill
        public string name = "Skill";

        // mana required to use skill
        public int manaCost = 0;

        // base damage of the skill
        public int baseDamage = 0;

        // ranged or melee. ranged skills can target enemies in the back row
        public bool ranged = false;

        public bool triggersBleed = true;

        // some ally targeting skills may not be able to target the user so this controls whether they can
        public bool canTargetSelf = true;

        // physical or non contact
        public bool physical = true;

        // the effects that a skill applies. this is an array because there can be multiple effects on one skill
        // SkillEffect stores the effect name and its chance of occuring
        public SkillEffect[] effects = null;

        // does the skill target the user, a single target, or all targets in a party
        public string targeting = "single";

        // does the skill target the user's party or the other party
        public string targetParty;

        // does the skill require your weapon to be equiped to use (for throwing skills like Barbarian's Throw Weapon)
        public bool weaponRequired = false;

        // bonus pierce applied when using skill
        public float pierce = 0;

        // bonus lifesteal applied when using skill
        public float lifesteal = 0;

        // bonus crit chance applied when using skill
        public float critChance = 0;

        // skills that aren't the default attack do 2/3 crit damage.
        // critical attacks do 2x damage while a critical on a skill that does 6x damage will do 12x. this is way too powerful so the 1/3 reduction balances it out
        public float critDamageMult = 2 / 3f;

        // does the skill do fire damage or not. the Freeze status condition reacts differently to fire attacks which is why this exists
        public bool isFire = false;

        // the way the heal effect of the skill works
        public HealStyle healStyle = new HealStyle(0, 0, "");

        // many skills deal damage more than once so this stores the way that the extra hits get handled
        public HitAgainStyle hitAgainStyle = null;

        // this class determines the way skills that hit multiple times work
        public class HitAgainStyle
        {
            // the damage applied to additional hits
            public float damageMult;

            // the maximum number of hits
            public int maxHits;

            // the condition that additional hits will occur
            public string condition;

            // constructor
            public HitAgainStyle(float damageMult, int maxHits, string condition)
            {
                this.damageMult = damageMult;
                this.maxHits = maxHits;
                this.condition = condition;
            }
        }

        // this class determines the way the skill heals
        public class HealStyle
        {
            public int amount;
            public float percent;
            public string targeting;
            public string condition;

            public HealStyle(int amount, float percent, string targeting)
            {
                this.amount = amount;
                this.percent = percent;
                this.targeting = targeting;
                condition = null;
            }

        }

        // Effect method applies the effects of a skill to the target
        // user is the battler using the skill and target is the battler who the skill is being used on, i is the index of the effects array that this effect is in
        public void Effect(Battler user, Battler target, int i)
        {
            // see if the effect will be triggered based on its effect chance
            if (rnd.Next(0, 100) < effects[i].chance)
            {
                // split the effect string on spaces. effects are formatted like "Poison 2" so we get the effect here and use the tier (2) later
                string[] effectSplit = effects[i].name.Split(' ');
                string effect = effectSplit[0];
                int tier = 0;

                if (effectSplit.Length > 1)
                {
                    tier = int.Parse(effectSplit[1]);
                }



                // if the effect is Defend (from the default Defend skill)
                if (effect == "Defend")
                {
                    // set defending boolean in Battler to true. this will reduce damage by 2/3 until your next turn
                    target.defending = true;
                }



                // if the effect is a heal
                else if (effect == "Heal")
                {
                    double amount = 0;

                    if (healStyle.amount > 0)
                    {
                        amount = healStyle.amount;
                    }

                    // if the heal targets the user
                    if (healStyle.targeting == "self")
                    {
                        if (healStyle.percent > 0)
                        {
                            // set the amount of healing to the user's maxHp times the healPercent
                            amount = user.maxHp * healStyle.percent;
                        }

                        // call the user's Heal method with the amount
                        user.Heal(amount);
                    }
                    // if the heal targets an ally
                    else if (healStyle.targeting == "ally")
                    {
                        if (healStyle.percent > 0)
                        {
                            // set the amount of healing to the target's maxHp times the healPercent
                            amount = target.maxHp * healStyle.percent;
                        }
                        
                        // call the target's Heal method with the amount
                        target.Heal(amount);
                    }
                    // if the heal targets all members of a party (usually the user's party)
                    else if (healStyle.targeting == "all")
                    {
                        // loop through each battler in the target's party
                        foreach (Battler battler in target.party.members)
                        {
                            // if the battler is not null
                            if (battler != null)
                            {
                                if (healStyle.percent > 0)
                                {
                                    // set the amount of healing to the battler's maxHp times the healPercent
                                    amount = battler.maxHp * healStyle.percent;
                                }
                                

                                // call the Battler's Heal method with the amount
                                battler.Heal(amount);
                            }
                        }
                    }
                }



                // if the effect is to hit again
                else if (effect == "HitAgain")
                {
                    // multiHit is set to true after a multi hit skill's first hit
                    // HitAgain() only needs to be called once so this won't run for additional hits
                    if (!user.multiHit)
                    {
                        // call hit again with the user, target and skill's hit again style
                        HitAgain(user, target, hitAgainStyle);
                    }
                }



                // status effects
                else if (effect == "Burn")
                {
                    // create a new Burn and apply it
                    new Burn(tier, target).Apply();
                }
                else if (effect == "Poison")
                {
                    // create a new Poison and apply it
                    new Poison(tier, target).Apply();
                }
                else if (effect == "Curse")
                {
                    // create a new Curse and apply it
                    new Curse(tier, target).Apply();
                }
                else if (effect == "Bleed")
                {
                    // create a new Bleed and apply it
                    new Bleed(tier, target).Apply();
                }
                else if (effect == "Stun")
                {
                    // create a new Stun
                    Stun stun = new Stun(tier, target);

                    // if the stun was applied
                    if (stun.Apply())
                    {
                        // add stunDamageIncrease to battler's defense modifiers list
                        target.defenseModifiers.Add(stun.stunDamageIncrease);
                    }
                }
                else if (effect == "Freeze")
                {
                    // create a new Freeze
                    Freeze freeze = new Freeze(tier, target);

                    // if the freeze was applied
                    if (freeze.Apply())
                    {
                        // add freezeDamageDecrease to battler's defense modifiers list
                        target.defenseModifiers.Add(freeze.freezeDamageDecrease);
                    }
                }
                else if (effect == "Sleep")
                {
                    // create a new Sleep
                    Sleep sleep = new Sleep(tier, target);

                    // if the sleep was applied
                    if (sleep.Apply())
                    {
                        // add sleepDefenseDecrease to battler's defense modifiers list
                        target.defenseModifiers.Add(sleep.sleepDefenseDecrease);
                    }
                }
                else if (effect == "Shock")
                {
                    // create a new Shock
                    Shock shock = new Shock(tier, target);

                    // if the shock was applied
                    if (shock.Apply())
                    {
                        // add shockDefenseDecrease to battler's defense modifiers list
                        target.defenseModifiers.Add(shock.shockDefenseDecrease);
                    }
                }
                else if (effect == "Confuse")
                {
                    // create a new Confuse and apply it
                    new Confuse(tier, target).Apply();
                }



                // buffs and debuffs
                // attack buff
                else if (effect == "+Attack")
                {
                    // create a new AttackBuff object
                    AttackBuff buff = new AttackBuff(tier, target);

                    // Apply returns true when the buff/debuff will be added/replaced so this runs when the buff/debuff gets applied
                    // if the target already has a stronger tier (or same tier with longer duration) of the same buff or debuff,
                    // it will return false so this will not run.
                    if (buff.Apply())
                    {
                        // if the user buffed themselves set buffedSelf to true to give it an extra turn
                        if (user.Equals(target))
                        {
                            buff.buffedSelf = true;
                        }
                    }
                }
                // attack debuff
                else if (effect == "-Attack")
                {
                    // create a new AttackDebuff object
                    AttackDebuff debuff = new AttackDebuff(tier, target);

                    // if the debuff got applied
                    if (debuff.Apply())
                    {
                        // if the user debuffed themselves set buffedSelf to true to give it an extra turn
                        if (user.Equals(target))
                        {
                            debuff.buffedSelf = true;
                        }
                    }
                }
                // defense buff
                else if (effect == "+Defense")
                {
                    // create a new DefenseBuff object
                    DefenseBuff buff = new DefenseBuff(tier, target);

                    // if the buff got applied
                    if (buff.Apply())
                    {
                        // if the user buffed themselves set buffedSelf to true to give it an extra turn
                        if (user.Equals(target))
                        {
                            buff.buffedSelf = true;
                        }
                    }
                }
                // defense debuff
                else if (effect == "-Defense")
                {
                    // create a new DefenseDebuff object
                    DefenseDebuff debuff = new DefenseDebuff(tier, target);

                    // if the debuff got applied
                    if (debuff.Apply())
                    {
                        // if the user buffed themselves set buffedSelf to true to give it an extra turn
                        if (user.Equals(target))
                        {
                            debuff.buffedSelf = true;
                        }
                    }
                }



                // if the effect is to disarm the user's weapon (Barbarian's Throw Weapon skill and for heroes who equip a spear weapon (not implemented yet))
                else if (effect == "Disarm")
                {
                    // print disarm message
                    Console.WriteLine($"{user.name} was disarmed!");

                    // set the user's disarm field to 2. at the start of their turn this will decrease and when it becomes 0, they get their weapon back
                    user.disarmed = 2;
                }
            }
        }

        // Critical() is called whenever is damage is about to be dealt to see if the damage will become a critical hit
        // returns the multiplier of the critical hit damage (1 if no crit is applied)
        public float Critical(Battler user)
        {
            // if the random number between 0-99 is less than the user + skill crit chance
            if (rnd.Next(0, 100) < user.critChance + critChance)
            {
                // print crit message
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("A critical hit!");
                Console.ResetColor();

                // return 1 + the user's crit damage. Skills that aren't the default attack do 2/3 less crit damage which is what critDamageMult stores
                return 1 + (user.critDamage * critDamageMult);
            }
            // if no critical, return 1
            return 1;
        }

        // DealDamage method deals the damage from a skill to the target
        // user is the battler using the skill and target is the battler the skill is being used on
        public void DealDamage(Battler user, Battler target)
        {
            // set the attack modifier equal to 1
            float atkMod = 1;

            // loop through each of the user's attack modifiers in their attackModifier list and multiply them to the atkMod float
            foreach (Mod mod in user.attackModifiers)
            {

                // multiply the atkMod by the mod's multiplier
                atkMod *= mod.mult;
            }

            // multiply atkMod by 1 if no crit or crit damage if crit
            atkMod *= Critical(user);

            // set the piercing equal to the user's pierce and the bonus pierce from the skill
            float piercing = pierce + user.armorPierce;

            // if the piercing exceeds 100%, set it to 100%. going over 100% would increase base damage rather than reduce the effect
            // of the target's defense.
            if (piercing > 100)
            {
                piercing = 100;
            }

            // set piercing to a multiplier that will be applied to the target's defense in their TakeDamage method
            piercing = 1 - (piercing / 100);

            // set the damage equal to the base damage times atkMod times the user's attack stat
            int damage = (int)Math.Round(baseDamage * atkMod * Math.Pow(user.atk, 1 / 1.5));

            // the max damage is the user's current hp. this only matters for lifesteal since I intend on displaying overkill damage
            int maxDamage = target.hp;

            // call target's TakeDamage method with the damage calculated by the formula
            // this gets set to a new double that stores the actual damage dealt
            int actualDamage = target.TakeDamage(damage, piercing);

            // if the target is still alive and is frozen
            if (target.hp > 0 && target.statusConditions.ContainsKey("Freeze"))
            {
                // create a new Freeze object set to the target's Freeze
                Freeze freeze = (Freeze)target.statusConditions["Freeze"];

                // if the skill deals fire damage
                if (isFire)
                {
                    // if the Freeze instantly ends when fire damage is taken (Tier 1 and 2)
                    if (freeze.fireDamageInstantlyEnds)
                    {
                        // print thawed message
                        Console.WriteLine($"{target.name} was thawed!");

                        // remove the Freeze's freezeDamageDecrease from the target's defenseModifiers list
                        target.defenseModifiers.Remove(freeze.freezeDamageDecrease);

                        // call the Freeze's Remove method. false means the cure message won't be printed
                        freeze.Remove(false);
                    }
                    // if fire damage doesn't instantly end the freeze
                    else
                    {
                        // call the Freeze's Effect method which reduces the turnsLeft by 1
                        freeze.Effect();
                    }
                }
                // if the skill doesn't deal fire damage and the random number between 0-99 is less than the Freeze's chance to reduce turnsLeft when damage is taken
                else if (rnd.Next(0, 100) < freeze.takeDamageReduceTurnChance)
                {
                    // call the Freeze's Effect method which reduces turnsLeft by 1
                    freeze.Effect();
                }
            }

            // if the target is still alive and is asleep
            if (target.hp > 0 && target.statusConditions.ContainsKey("Sleep"))
            {
                // create a new Sleep object set to the target's Sleep
                Sleep sleep = (Sleep)target.statusConditions["Sleep"];

                // if a random number between 0-99 is less than the sleep's wake if damaged change, remove the sleep
                if (rnd.Next(0, 100) < sleep.wakeIfDamagedChance)
                {
                    // remove sleep
                    sleep.Remove(true);
                }
            }

            // if the actual damage exceeds the max damage, set the actual damage to the max damage
            // this only matters for lifesteal
            if (actualDamage > maxDamage)
            {
                actualDamage = maxDamage;
            }

            // if either the skill or user has any lifesteal
            if (lifesteal + user.lifesteal > 0)
            {
                // store the amount of health the user will heal. this is just the % of lifesteal * the actual damage done
                double amount = actualDamage * ((lifesteal + user.lifesteal) / 100);

                // call the user's heal method with the amount
                user.Heal(amount);
            }
        }

        public void HitAgain(Battler user, Battler target, HitAgainStyle style)
        {
            int baseDamagePlaceholder = baseDamage;
            List<Battler> exceptFor = new List<Battler>();
            int hits = 1;
            user.multiHit = true;

            if (style.condition == "killed target")
            {
                while (target.dead && !target.party.IsDefeated() && hits < style.maxHits)
                {
                    exceptFor.Add(target);
                    target = target.party.GetWeakest(exceptFor);
                    baseDamage = (int)Math.Round(baseDamage * style.damageMult);
                    DealDamage(user, target);
                    hits++;
                }
            }
            else if (style.condition == "behind")
            {
                if (target.GetBehind() != null)
                {
                    target = target.GetBehind();
                    baseDamage = (int)Math.Round(baseDamage * style.damageMult);
                    DealDamage(user, target);
                }
            }
            else if (style.condition == "next to")
            {
                baseDamage = (int)Math.Round(baseDamage * style.damageMult);
                foreach (Battler battler in target.GetNextTo())
                {
                    if (battler != null)
                    {
                        DealDamage(user, battler);
                    }
                }
            }
            else if (style.condition == "random")
            {
                baseDamage = (int)Math.Round(baseDamage * style.damageMult);

                while (hits < style.maxHits && hits < target.party.GetAllLiving().Length)
                {
                    exceptFor.Add(target);

                    do
                    {
                        target = target.party.RandomMember();
                    } while (exceptFor.Contains(target));

                    DealDamage(user, target);
                    hits++;
                }
            }

            baseDamage = baseDamagePlaceholder;
            user.multiHit = false;
        }

        public void TriggerEffects(Battler user, Battler target)
        {
            // if the skill has an effect
            if (effects != null)
            {
                for (int i = 0; i < effects.Length; i++)
                {
                    // if the target is still alive or the effect is Disarm
                    // Disarm gets applied to the user whereas most other effects get applied to the target
                    // the reason for the array is for when I implement other skills that apply effects to the user
                    if ((target.hp > 0 || new string[] { "Disarm", "HitAgain", "Heal" }.Contains(effects[i].name)) && healStyle.condition != "kill")
                    {
                        // the Sentinel has an ability to save it's allies from fatal blows so this will
                        // apply the effect to the Sentinel instead of the original target if this occurs
                        // sentinelSave is a boolean field in the Battler class that is set to true if the
                        // Sentinel saved that battler that turn, at the beginning of the turn it's set back to false
                        if (target.sentinelSave)
                        {
                            // apply the effect to the Sentinel
                            Effect(user, target.party.GetMemberByName("Sentinel"), i);
                        }
                        else
                        {
                            // apply the effec to the original target
                            Effect(user, target, i);
                        }
                    }
                    else if ((target.hp <= 0 && healStyle.condition == "kill") || (target.sentinelSave && !target.party.hasSentinel))
                    {
                        if (target.sentinelSave)
                        {
                            Effect(user, target.party.GetMemberByName("Sentinel"), i);
                        }
                        else
                        {
                            Effect(user, target, i);
                        }
                    }
                }

            }
        }

        // Use method calls the Effect and DealDamage methods on the target(s)
        // user is the battler using the skill and target is the battler the skill is being used on
        public void Use(Battler user, Battler target)
        {
            // if the user is using a melee skill and have a bleed status, activate it
            // bleed is triggered when using a melee skill as well as at the end of turn
            if (triggersBleed && targeting != "self" && user.statusConditions.ContainsKey("Bleed") && !user.multiHit)
            {
                // the false tells the method that this was not called at the end of turn so the turn's left does not need to be decreased
                user.statusConditions["Bleed"].Effect(false);
            }

            // if the user is still alive (this should only not run when the user dies from bleed damage since that triggers before their skill gets used)
            if (user.hp > 0)
            {
                // self targeting skills
                if (targeting == "self")
                {
                    // skill message
                    Console.WriteLine($"{user.name} used {name} on itself!");

                    TriggerEffects(user, target);
                }
                else if (user.statusConditions.ContainsKey("Confuse") && rnd.Next(0, 100) < user.statusConditions["Confuse"].confuseAttackRandomChance)
                {
                    do
                    {
                        target = GameHandler.heroParty.GetRandomMemberFromEitherParty(GameHandler.enemyParty);
                    } while (target == user || (target.party != user.party && target.guarded && !ranged));

                    Console.WriteLine(user.name + "'s confusion caused it to attack a random target!");
                }

                // if the user doesn't miss
                if (rnd.Next(0, 100) < user.accuracy && !(user.statusConditions.ContainsKey("Confuse") && user.party != target.party && rnd.Next(0, 100) < user.statusConditions["Confuse"].confuseMissEnemyChance))
                {
                    // single target skills
                    if (targeting == "single")
                    {
                        // skill message
                        Console.WriteLine($"{user.name} used {name} on {target.name}!");

                        // call DealDamage method if the skill deals damage
                        if (baseDamage > 0)
                        {
                            DealDamage(user, target);
                        }

                        TriggerEffects(user, target);
                    }

                    // hit all skills
                    else if (targeting == "all")
                    {
                        // skill message
                        Console.WriteLine($"{user.name} used {name}!");

                        // loop through each battler in the target's party
                        foreach (Battler battler in target.party.members)
                        {
                            // if the battler is not null
                            if (battler != null)
                            {
                                // call DealDamage for damaging skills
                                if (baseDamage > 0)
                                {
                                    DealDamage(user, battler);
                                }

                                TriggerEffects(user, battler);
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"{user.name} used {name} but it missed!");
                }

                
            }

            // subtract the skill's mana cost from the user's mana
            if (!user.multiHit)
            {
                user.mana -= manaCost;
            }

        }

        // constructor
        // most common fields are required, other rarer ones are specified in the individual skill constructors
        public Skill(string name, int manaCost, bool targetsEnemies, string targeting, int baseDamage, bool ranged, params SkillEffect[] effects)
        {
            this.name = name;
            this.manaCost = manaCost;
            if (targetsEnemies)
            {
                targetParty = "enemies";
            }
            else
            {
                targetParty = "allies";
            }
            this.targeting = targeting;
            this.baseDamage = baseDamage;
            this.ranged = ranged;
            if (ranged)
            {
                triggersBleed = false;
            }
            this.effects = effects;
            foreach (SkillEffect effect in effects)
            {
                if (effect.name == "Disarm")
                {
                    weaponRequired = true;
                    break;
                }
            }
        }
    }

    public struct SkillEffect
    {
        public string name;
        public int chance;

        public SkillEffect(string name, int chance)
        {
            this.name = name;
            this.chance = chance;
        }
    }
}
