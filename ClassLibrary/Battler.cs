using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    // Battler class contains stats and general methods
    public class Battler
    {
        public static Random rnd = new Random();

        /********************* hero and enemy shared fields *********************/

        // name of battler
        public string name;

        // max HP
        public int maxHp = 1;
        public int baseMaxHp = 1;

        // current HP
        public int hp = 1;

        // attack used to calculate damage dealt
        public int atk = 1;
        public int baseAtk = 1;

        // defense used to calculate damage taken
        public int def = 1;
        public int baseDef = 1;

        // current mana
        public int mana = 0;

        // mana regenerated at the start of each turn
        public float manaRegen = 10;

        // % chance of a critical hit
        public float critChance = 5;

        // critical hit damage multiplier
        public float critDamage = 1;

        // % of guaranteed damage reduction
        public float damageReduction = 0;

        // % of enemy defense ignored
        public float armorPierce = 0;

        // % chance that non self targeting skills will hit
        public float accuracy = 100;

        // % chance to dodge an attack
        public float dodgeChance = 0;

        // hp regenerated each turn
        public int hpRegen = 0;

        // % of damage taken that gets reflected back at the source of the damage
        public float damageReflect = 0;

        // % of damage dealt that gets converted into healing
        public float lifesteal = 0;

        // % chance to resist being inflicted with a status condition
        public float statusResist = 0;

        // if there is another battler in front of this battler
        // prevents this battler from being targeted by melee attacks if true
        public bool guarded = false;

        // a dictionary that stores the battler's current status conditions.
        // index is the name of the status: "Poison" "Burn" etc.
        public Dictionary<string, StatusCondition> statusConditions = new Dictionary<string, StatusCondition>();

        // a dictionary that stores the battler's current buffs and debuffs
        // index is the name of buff/debuff: "+Attack" "-Defense" etc.
        public Dictionary<string, BuffDebuff> buffsDebuffs = new Dictionary<string, BuffDebuff>();

        // the status conditions this battler is immune to
        public List<string> immunities = new List<string>();

        // is the battler dead or alive
        public bool dead = false;

        // the party that this Battler belongs to
        public Party party = GameHandler.enemyParty;

        // which spot in the party's members array this Battler is in
        public int partySpot;

        // is the battler undead
        public bool undead = false;

        // heals used on the battler get multiplied by this amount
        public float healMultiplier = 1;

        // does the battler currently have stealth (unable to be targeted)
        public bool stealth = false;

        // if this battler has already attacked this turn (used for multi hit skills)
        public bool multiHit = false;

        // list of the battler's skills
        public List<Skill> skills = new List<Skill>();

        // list of the battler's attack modifiers such as buffs, debuffs, Barbarian's inner fury passive, etc.
        public List<Mod> attackModifiers = new List<Mod>();

        // list of the battler's defense modifiers such as buffs, debuffs, increased damage from being stunned, etc.
        public List<Mod> defenseModifiers = new List<Mod>();

        // default attack skill that every hero and enemy gets
        public Skill Attack = new Skill("Attack", 0, true, "single", 100, false);





        /********************* hero only fields *********************/

        // default defend skill that every hero gets
        public static Skill Defend = new Skill("Defend", 0, false, "self", 0, false, Effect("Defend", 100));

        // % of mana carried over into the next battle
        public float manaRetention = 0;

        // is the battler defending or not
        public bool defending = false;

        // keeps track of how many turns until the battler gets their weapon back
        public int disarmed = 0;

        // did the Sentinel save the battler this turn or not
        public bool sentinelSave = false;

        // stores the hero's equipment
        public Dictionary<string, Equipment> loadout = new Dictionary<string, Equipment>() { { "weapon", null }, { "shield", null }, { "helmet", null }, { "armor", null }, { "boots", null }, { "trinket", null } };




        /********************* enemy only fields *********************/

        // used to have bosses take reduced damage from statuses
        public float statusDamageMultiplier = 1;

        // only for enemies. if the enemy is elite or not
        public bool elite = false;

        // is the battler a miniboss
        public bool miniBoss = false;

        // is the battler a boss
        public bool boss = false;

        // the gold that this enemy drops
        public int goldDrop = 10;

        

        // create a skill effect without having to write new every time
        // used in battler subclasses to define their skills
        public static SkillEffect Effect(string name, int chance)
        {
            return new SkillEffect(name, chance);
        }

        public void Info()
        {
            Console.WriteLine(GetInfo());

            Console.Write("Status Conditions: ");
            foreach (KeyValuePair<string, StatusCondition> status in statusConditions)
            {
                status.Value.ColorPrint(status.Value.name);
                Console.Write($" ({status.Value.turnsLeft} turns remaining) ");
            }

            Console.Write("\nBuffs and Debuffs: ");
            foreach (KeyValuePair<string, BuffDebuff> buffDebuff in buffsDebuffs)
            {
                buffDebuff.Value.ColorPrint(buffDebuff.Value.name);
                Console.Write($"({buffDebuff.Value.turnsLeft} turns remaining) ");
            }
            Console.WriteLine();
        }

        public virtual string GetInfo()
        {
            return "";
        }

        public void ManaRegen()
        {
            mana += (int)manaRegen;
            if (mana > 60)
            {
                mana = 60;
            }
        }

        public void TurnStart(Party otherParty)
        {

            if (disarmed == 1)
            {
                disarmed--;
                Console.WriteLine($"{name} is no longer disarmed!");
            }
            else if (disarmed == 2)
            {
                disarmed--;
            }


            if (party.name == "heroes")
            {
                if (defending)
                {
                    defending = false;
                }

                party.PrintAsTable(otherParty);

                // new line between enemy team HP message and your hero's turn message
                Console.WriteLine();

                // print hero's turn message
                Console.WriteLine(name + "'s turn.");

            }

        }

        public void TurnEnd()
        {
            Dictionary<string, BuffDebuff> buffsDebuffsCopy = new Dictionary<string, BuffDebuff>(buffsDebuffs);
            Dictionary<string, StatusCondition> statusConditionsCopy = new Dictionary<string, StatusCondition>(statusConditions);
            foreach (KeyValuePair<string, BuffDebuff> buffDebuff in buffsDebuffsCopy)
            {
                if (!buffDebuff.Value.buffedSelf)
                {
                    buffDebuff.Value.TurnEnd();
                }
                else
                {
                    buffDebuff.Value.buffedSelf = false;
                }
            }

            foreach (KeyValuePair<string, StatusCondition> status in statusConditionsCopy)
            {
                status.Value.Effect(true);
            }
        }

        public void Move(Party otherParty)
        {
            // call turn start before choosing a move
            TurnStart(otherParty);

            if (statusConditions.ContainsKey("Stun"))
            {
                Console.WriteLine($"{name} is stunned and can't move!");
            }
            else if (statusConditions.ContainsKey("Freeze"))
            {
                Console.WriteLine($"{name} is frozen and can't move!");
            }
            else if (statusConditions.ContainsKey("Sleep"))
            {
                Console.WriteLine($"{name} is asleep and can't move!");
            }
            else if (statusConditions.ContainsKey("Shock") && rnd.Next(0, 100) < statusConditions["Shock"].shockUnableToMoveChance)
            {
                Console.WriteLine($"{name} is shocked and can't move!");
            }
            else
            {
                // get user input for which move they want to use if the battler is a hero
                if (party.name == "heroes")
                {
                    // store input string
                    string input;

                    // store skill numbers
                    string[] skillNumbers = new string[skills.Count];

                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine($"{mana} Mana");
                    Console.ResetColor();

                    int i;

                ChooseMove:
                    for (i = 0; i < skills.Count; i++)
                    {
                        Console.Write($"{i + 1}. {skills[i].name} ");
                        if (skills[i].manaCost > 0)
                        {
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.Write($"({ skills[i].manaCost}) ");
                            Console.ResetColor();
                        }
                        skillNumbers[i] = (i + 1).ToString();
                    }

                    Console.WriteLine(i + 1 + ". Info");

                    // bool for input validation
                    bool valid = false;
                    Skill choice = null;


                    // repeat until user chooses a valid move
                    do
                    {

                        // get user input
                        Console.Write(">");
                        input = Console.ReadLine();

                        try
                        {
                            if (int.Parse(input) == i + 1)
                            {
                                Info();
                            }
                            choice = skills[int.Parse(input) - 1];
                            if (mana < choice.manaCost)
                            {
                                Console.WriteLine("Not enough mana!");
                                valid = false;
                            }
                            else if (disarmed != 0 && choice.weaponRequired)
                            {
                                Console.WriteLine("You are disarmed!");
                                valid = false;
                            }
                            else
                            {
                                valid = true;
                            }
                        }
                        catch { }

                    } while (!skillNumbers.Contains(input) || !valid);



                    if (choice.targeting == "self")
                    {
                        choice.Use(this, this);
                    }
                    else if (choice.targeting == "all")
                    {
                        if (choice.targetParty == "enemies")
                        {
                            choice.Use(this, otherParty.RandomMember());
                        }
                        else
                        {
                            choice.Use(this, party.RandomMember());
                        }
                    }
                    else if (choice.targetParty == "allies")
                    {
                        Battler target = GetTarget(party, true);
                        if (target == null)
                        {
                            goto ChooseMove;
                        }
                        else
                        {
                            choice.Use(this, target);
                        }
                    }
                    else
                    {
                        Battler target = GetTarget(otherParty, choice.ranged);
                        if (target == null)
                        {
                            goto ChooseMove;
                        }
                        else
                        {
                            choice.Use(this, target);
                        }
                    }
                }
                // if the battler is an enemy, use a random skill on a random target
                else
                {
                    // attack random target

                    Skill skill;
                    do
                    {
                        skill = skills[rnd.Next(0, skills.Count)];
                    } while (skill.manaCost > mana);

                    if (skill.name == "Attack")
                    {
                        do
                        {
                            skill = skills[rnd.Next(0, skills.Count)];
                        } while (skill.manaCost > mana);
                    }

                    Battler target = otherParty.RandomMember();

                    while ((target.guarded && !skill.ranged) || target.stealth)
                    {
                        target = otherParty.RandomMember();
                    }

                    skill.Use(this, target);
                }
            }

            // call TurnEnd as long as the battler whose move it is is still alive
            if (hp > 0)
            {
                TurnEnd();
            }
        }

        // have user choose their target
        public Battler GetTarget(Party targets, bool ranged)
        {

            // get all indexes of valid targets
            int[] validTargets = targets.GetAllLiving();

            // loop through each valid target and display index and name
            foreach (int validTarget in validTargets)
            {
                Console.Write(validTarget + 1 + ". " + targets.members[validTarget].name + " ");
            }

            Console.WriteLine(targets.GetLastIndex() + 2 + ". Cancel");

            // store input string
            string input;

            // initialize target int
            int target = -1;

            // initialize valid bool
            bool valid = false;

            // input validation. while the user picks a valid target and the input can be parsed
            do
            {
                Console.Write(">");
                input = Console.ReadLine();
                try
                {
                    target = int.Parse(input) - 1;

                    if (target == targets.GetLastIndex() + 1)
                    {
                        return null;
                    }

                    if (!targets.members[target].guarded || ranged)
                    {
                        valid = true;
                    }
                    else
                    {
                        Console.WriteLine($"{targets.members[target].name} is guarded by {targets.members[target - 1].name}");
                    }
                }
                catch
                {
                    Console.Write(">");
                    input = Console.ReadLine();
                }
            } while (!validTargets.Contains(target) || !valid);

            // extra space before attack message is printed
            Console.WriteLine();

            // return the chosen target
            return targets.members[target];
        }

        // take the amount of damage passed by the attacker's Attack method
        public int TakeDamage(int amount, float pierceMult)
        {
            // set the defense modifier equal to 1
            float defMod = 1;

            // loop through each of the target's defense modifiers in their defenseModifier list and multiply them to the defMod float
            foreach (Mod mod in defenseModifiers)
            {
                // multiply the defMod by the mod's multiplier
                defMod *= mod.mult;
            }

            /*********************** DAMAGE FORMULA ***********************/
        double damage = amount / (Math.Pow(def * pierceMult, 1 / 1.5) * defMod);
            /**************************************************************/

            damage = (int)Math.Round(damage);

            Sentinel sentinel = null;

            if (party.hasSentinel && name != "Sentinel")
            {
                sentinel = (Sentinel)party.GetMemberByName("Sentinel");
                if (sentinel.statusConditions.ContainsKey("Stun") || sentinel.statusConditions.ContainsKey("Freeze") || sentinel.statusConditions.ContainsKey("Sleep"))
                {
                    Console.WriteLine($"{sentinel.name} can't move and can't protect {name}!");
                    sentinel = null;
                }
                else if (sentinel.statusConditions.ContainsKey("Shock") && rnd.Next(0, 100) < sentinel.statusConditions["Shock"].shockUnableToMoveChance)
                {
                    Console.WriteLine($"{sentinel.name} is shocked and can't protect {name}!");
                    sentinel = null;
                }
            }

            int sentinelDamage = 0;

            if (sentinel != null && name != "Sentinel")
            {
                sentinelDamage = amount - (int)Math.Round(amount * (1 - sentinel.defender));
                damage *= 1 - sentinel.defender;
            }

            if (defending)
            {
                damage = (int)Math.Round(damage / 3f);
            }

            damage = Math.Round(damage * (1 - (damageReduction / 100)));

            if (hp - damage <= 0 && sentinel != null && sentinel.saviorCooldown == 0)
            {
                Console.WriteLine($"Sentinel blocked a fatal hit on {name}!");
                sentinel.saviorCooldown = 2;
                damage = sentinel.TakeDamage(amount, pierceMult);
                sentinelSave = true;
            }
            else
            {
                // took damage message
                Console.WriteLine(name + " took " + (int)damage + " damage!");

                // decrement hp
                ModifyHp(-(int)damage);

                // check if dead
                DeathCheck();

                if (party.hasSentinel && name != "Sentinel" && sentinel != null)
                {
                    sentinelDamage = party.GetMemberByName("Sentinel").TakeDamage(sentinelDamage, pierceMult);
                }
            }

            

            return (int)damage + sentinelDamage;
        }

        // check if battler is dead and remove from party if so
        public bool DeathCheck()
        {
            // if hp is 0 or lower
            if (hp <= 0)
            {

                // death message
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine(name + " died!");
                Console.ResetColor();

                if (party.name == "enemies")
                {
                    DropLoot();
                }

                if (partySpot % 2 == 0)
                {
                    if (party.members[partySpot + 1] != null)
                    {
                        party.members[partySpot + 1].guarded = false;
                    }
                }

                // remove from party
                party.members[partySpot] = null;

                dead = true;

                if (name == "Sentinel")
                {
                    party.hasSentinel = false;
                }
                else if (name == "Paladin")
                {
                    party.hasPaladin = false;
                }

                if (party.GetAllLiving().Length == 1 && party.members[party.GetFirstIndex()].stealth)
                {
                    party.members[party.GetFirstIndex()].stealth = false;
                }

                return true;
            }
            return false;
        }

        public void DropLoot()
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Got " + goldDrop + "G");
            Console.ResetColor();
            GameHandler.gold += goldDrop;
        }

        public void ModifyHp(int amount)
        {
            hp += amount;

            // update Barbarian's innerFury field
            if (name == "Barbarian")
            {
                Barbarian barbarian = (Barbarian)this;

                // set Barbarian's innerFury multiplier to 1 + .001 for every 1% of health missing
                barbarian.innerFury.mult = 1 + (1 - hp / (float)maxHp);
            }
        }

        public void Heal(double amount)
        {
            amount *= healMultiplier;

            // if the amount of healing would make the user exceed their max hp
            if (amount > (maxHp - hp))
            {
                // set the amount to the amount needed to reach max hp and not more
                amount = maxHp - hp;
            }

            // restores the user's hp by the amount
            amount = (int)Math.Round(amount);

            ModifyHp((int)amount);

            // print a message saying the user restored hp if they restored more than 0
            // this is so no message appears when at full hp
            if (amount > 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"{name} restored {amount} hp!");
                Console.ResetColor();
            }
        }

        public Battler GetBehind()
        {
            if (partySpot % 2 == 0)
            {
                return party.members[partySpot + 1];
            }
            else
            {
                return null;
            }
        }

        public Battler GetInfront()
        {
            if (partySpot % 2 == 1)
            {
                return party.members[partySpot - 1];
            }
            else
            {
                return null;
            }
        }

        public Battler[] GetDiagonal()
        {
            Battler[] diagonal = new Battler[4];

            if (partySpot % 2 == 0)
            {
                try
                {
                    diagonal[0] = party.members[partySpot - 1];
                } catch { }

                try
                {
                    diagonal[1] = party.members[partySpot + 3];
                } catch { }
            }
            else
            {
                try
                {
                    diagonal[2] = party.members[partySpot - 3];
                }
                catch { }

                try
                {
                    diagonal[3] = party.members[partySpot + 1];
                }
                catch { }
            }

            return diagonal;
        }

        public Battler[] GetOrthogonalAdjacent()
        {
            Battler[] adjacent = new Battler[4];
            Battler[] nextTo = GetNextTo();

            adjacent[0] = nextTo[0];
            adjacent[1] = GetBehind();
            adjacent[2] = nextTo[1];
            adjacent[3] = GetInfront();
            
            return adjacent;
        }

        public Battler[] GetAdjacent()
        {
            Battler[] adjacent = new Battler[8];
            Battler[] orthogonal = GetOrthogonalAdjacent();
            Battler[] diagonal = GetDiagonal();

            adjacent[0] = orthogonal[0];
            adjacent[1] = diagonal[2];
            adjacent[2] = GetInfront();
            adjacent[3] = diagonal[3];
            adjacent[4] = orthogonal[2];
            adjacent[5] = diagonal[1];
            adjacent[6] = GetBehind();
            adjacent[7] = diagonal[0];

            return adjacent;
        }

        public Battler[] GetNextTo()
        {
            Battler[] nextTo = new Battler[2];

            try
            {
                nextTo[0] = party.members[partySpot - 2];
            } catch { }

            try
            {
                nextTo[1] = party.members[partySpot + 2];
            } catch { }

            return nextTo;
        }

        public Battler DeepCopy()
        {
            Battler clone = (Battler)this.MemberwiseClone();
            clone.statusConditions = new Dictionary<string, StatusCondition>();
            clone.buffsDebuffs = new Dictionary<string, BuffDebuff>();
            clone.attackModifiers = new List<Mod>();
            clone.defenseModifiers = new List<Mod>();

            return clone;
        }

        public void Spawn()
        {
            if (GetBehind() != null && !stealth)
            {
                GetBehind().guarded = true;
            }
            else if (GetInfront() != null && !GetInfront().stealth)
            {
                guarded = true;
            }

            if (party.name == "enemies")
            {
                // scale enemy stats
                maxHp = (int)Math.Round(maxHp * GameHandler.dungeon.baseStatBoost[0] * GameHandler.dungeon.currentStatBoost[0]);
                atk = (int)Math.Round(atk * GameHandler.dungeon.baseStatBoost[1] * GameHandler.dungeon.currentStatBoost[1]);
                def = (int)Math.Round(def * GameHandler.dungeon.baseStatBoost[2] * GameHandler.dungeon.currentStatBoost[2]);
                goldDrop = (int)Math.Round(goldDrop * GameHandler.dungeon.baseStatBoost[3] * GameHandler.dungeon.currentStatBoost[3]);
                
                // see if enemy is elite and boost stats if so
                if (!boss && !miniBoss && rnd.Next(0, 100) < GameHandler.dungeon.eliteChance)
                {
                    elite = true;
                    maxHp = (int)(maxHp * 1.5);
                    atk = (int)(atk * 1.5);
                    def = (int)(def * 1.5);
                    goldDrop *= 2;
                    name = "*" + name + "*";
                }

                hp = maxHp;
            }

            party.members[partySpot] = this;
        }

        // constructor
        public Battler(string name, int maxHp, int atk, int def, bool hero, params Skill[] skills)
        {
            // set base stats
            this.name = name;
            this.maxHp = maxHp;
            baseMaxHp = maxHp;
            hp = maxHp;
            this.atk = atk;
            baseAtk = atk;
            this.def = def;
            baseDef = def;

            // default attacks do full crit damage so this replaces the default 0.5x critDamageMult
            Attack.critDamageMult = 1;

            // add Attack to skills list
            this.skills.Add(Attack);

            // add skills to skills list
            if (skills != null)
            {
                foreach (Skill skill in skills)
                {
                    this.skills.Add(skill);
                }
            }

            // add Defend to skills list if the Battler is a hero
            if (hero)
            {
                party = GameHandler.heroParty;
                this.skills.Add(Defend);
            }
            else
            {
                party = GameHandler.enemyParty;
            }

        }
    }
}
