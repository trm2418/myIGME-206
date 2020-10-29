using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public class Dungeon
    {
        public static Random rnd = new Random();

        public string name;
        public int eliteChance;
        public int currentLevel = 1;
        public int highestLevelReached = 1;
        public Battler[] enemies = { };
        public Battler[] minibosses = { };
        public Battler[] bosses = { };
        public Battler dungeonBoss;
        public float[] baseStatBoost = { 1, 1, 1, 1 };
        public float[] currentStatBoost = { 1, 1, 1, 1 };
        public float[] incrementalStatBoost10 = { 1, 1, 1, 1 };
        public float[] incrementalStatBoost20 = { 1, 1, 1, 1 };

        public void RandomEvent(Party heroParty)
        {
            Console.WriteLine("Random Event\n");
        }

        // Battle method, calls Party.Move method until one party is dead
        public bool Battle(Party heroParty, Party enemyParty)
        {
            heroParty.whoseMove = 0;
            enemyParty.whoseMove = 0;

            if (currentLevel % 10 == 0)
            {
                GameHandler.bossMusic.PlayLooping();
            }
            else if (currentLevel % 10 == 1)
            {
                GameHandler.backgroundMusic.PlayLooping();
            }

            // reset battler's mana at the beginning of each level
            foreach (Battler member in heroParty.members)
            {
                if (member != null)
                {
                    // multoply the member's mana by it's mana retention (usually 0)
                    // mana retention is a stat that lets you keep a percentage of your mana when starting the next battle
                    member.mana = (int)(member.manaRetention/100.0 * member.mana);

                    // update Barbarian's innerFury field
                    if (member.name == "Barbarian")
                    {
                        Barbarian barbarian = (Barbarian)member;

                        // set Barbarian's innerFury multiplier to 1 + .001 for every 1% of health missing
                        barbarian.innerFury.mult = 1 + (1 - barbarian.hp / (float)barbarian.maxHp);
                    }
                }
            }

            // while both parties are alive
            while (!heroParty.IsDefeated() && !enemyParty.IsDefeated())
            {

                // if heroes are still alive
                if (!heroParty.IsDefeated())
                {

                    // call hero Party move method
                    heroParty.Move(enemyParty);
                }
                // if enemies are still alive
                // this is to prevent the enemy party from moving on they turn the last one dies
                if (!enemyParty.IsDefeated())
                {
                    // call enemy Party move method
                    enemyParty.Move(heroParty);
                }
            }
            
            if (currentLevel % 10 == 0)
            {

                GameHandler.bossMusic.Stop();
            }

            //return true if heroes won, otherwise false
            if (!heroParty.IsDefeated())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void DungeonLoop()
        {
            currentLevel = highestLevelReached - ((highestLevelReached - 1) % 20);
            currentStatBoost = new float[] { 1, 1, 1, 1 };

            GameHandler.backgroundMusic.PlayLooping();

            while (!GameHandler.heroParty.IsDefeated())
            {
                // display the current level
                Console.WriteLine("Level: " + currentLevel);

                // every level ending in 5 is a random event
                if (currentLevel % 10 == 5)
                {
                    RandomEvent(GameHandler.heroParty);
                    currentLevel++;
                }
                else
                {
                    // minibosses and bosses appear at levels 5 and 10 respectively so these are set to false at the beginning of each level
                    bool miniBoss = false;
                    bool boss = false;

                    // initialize numEnemies. this is the amount of enemies that will be spawned on the current level
                    int numEnemies = 1;

                    // set numEnemies depending on the last digit of the level
                    switch (currentLevel % 10)
                    {
                        case 1: // levels 1, 11, 21, 31...
                            numEnemies = 1;
                            break;
                        case 2:
                            numEnemies = 2;
                            break;
                        case 3:
                            numEnemies = 3;
                            break;
                        case 4:
                            numEnemies = 4;
                            break;
                        case 6:
                            numEnemies = 5;
                            break;
                        case 7:
                            numEnemies = 6;
                            break;
                        case 8:
                            numEnemies = 7;
                            break;
                        case 9:
                            numEnemies = 8;
                            break;
                        case 0:
                            if (currentLevel % 20 == 10)
                            {
                                // a miniboss appears at levels 10, 30, 50, 70, 90, 110...
                                miniBoss = true;
                                numEnemies = 8;
                            }
                            else
                            {
                                // a boss appears at levels 20, 40, 60, 80, 120...
                                boss = true;
                                numEnemies = 8;
                            }
                            break;
                    }

                    // create a new Battler array
                    Battler[] enemyArray = new Battler[8];

                    // loop once for each enemy
                    for (int i = 0; i < numEnemies; i++)
                    {
                        Battler enemy;

                        // if a miniboss is being spawned this level
                        if (miniBoss)
                        {
                            // add a deep copy of a random miniboss to the enemy array
                            // a deep copy is needed because otherwise status effects and buffs/debuffs would carry over if the same enemy gets respawned
                            enemy = minibosses[rnd.Next(0, minibosses.Length)].DeepCopy();

                            enemy.statusResist = 75;
                            enemy.statusDamageMultiplier = 0.2f;
                            enemy.goldDrop = 250;

                            // set miniBoss to false so only one gets spawned
                            miniBoss = false;
                        }
                        // if a boss is being spawned this level
                        else if (boss)
                        {
                            // add a deep copy of a random boss to the enemy array
                            enemy = bosses[rnd.Next(0, bosses.Length)].DeepCopy();

                            enemy.statusResist = 90;
                            enemy.statusDamageMultiplier = 0.1f;
                            enemy.goldDrop = 1000;

                            // set boss to false so only one gets spawned
                            boss = false;
                        }
                        // regular enemies
                        else
                        {
                            // add a deep copy of a random enemy to enemy array
                            enemy = enemies[rnd.Next(0, enemies.Length)].DeepCopy();
                        }

                        do
                        {
                            enemy.partySpot = rnd.Next(0, 8);
                        } while (enemyArray[enemy.partySpot] != null);

                        enemyArray[enemy.partySpot] = enemy;
                        enemy.Spawn();
                    }

                    GameHandler.enemyParty.members = enemyArray;


                    // call Battle method. if heroes win, increment level
                    if (Battle(GameHandler.heroParty, GameHandler.enemyParty))
                    {
                        if (currentLevel % 10 == 0)
                        {
                            if (currentLevel % 20 == 0)
                            {
                                if (currentLevel < 100)
                                {
                                    for (int i = 0; i < 4; i++)
                                    {
                                        currentStatBoost[i] *= incrementalStatBoost20[i];
                                    }
                                }
                                else
                                {
                                    for (int i = 0; i < 4; i++)
                                    {
                                        currentStatBoost[i] *= incrementalStatBoost10[i];
                                    }
                                }
                                
                            }
                            else
                            {
                                for (int i = 0; i < 4; i++)
                                {
                                    currentStatBoost[i] *= incrementalStatBoost10[i];
                                }
                            }
                        }

                        currentLevel++;
                    }
                }
            }

            if (currentLevel > highestLevelReached)
            {
                highestLevelReached = currentLevel;
            }

            GameHandler.backgroundMusic.Stop();
            GameHandler.menuMusic.PlayLooping();
        }

        public Dungeon(string name, int eliteChance, float[] baseStatBoost, float[] incrementalStatBoost10, float[] incrementalStatBoost20)
        {
            this.name = name;
            this.eliteChance = eliteChance;
            this.baseStatBoost = baseStatBoost;
            this.incrementalStatBoost10 = incrementalStatBoost10;
            this.incrementalStatBoost20 = incrementalStatBoost20;
        }
    }

    public class Forest : Dungeon
    {
        public Forest() : base("Forest", 0, new float[] { 1, 1, 1, 1}, new float[] { 1.1f, 1.1f, 1.05f, 1.1f }, new float[] { 1.2f, 1.2f, 1.1f, 1.2f })
        {
            enemies = new Battler[] { new Goblin(), new Spider(), new MushroomMonster(), new Wolf(), new Spriggan(), new Bat(), new Gnome(), new VineMonster(), new Dragonling() };
            minibosses = new Battler[] { new Dragon() };
            bosses = new Battler[] { new SpiderQueen() };
        }
    }

    public class Volcano : Dungeon
    {
        public Volcano() : base("Volcano", 5, new float[] { 3.507f, 3.507f, 1.915f, 3.507f }, new float[] { 1.05f, 1.05f, 1.025f, 1.05f }, new float[] { 1.1f, 1.1f, 1.05f, 1.1f })
        {
            enemies = new Battler[] { new Goblin(), new Spider(), new MushroomMonster(), new Wolf(), new Spriggan(), new Bat(), new Gnome(), new VineMonster(), new Dragonling() };
            minibosses = new Battler[] { new Dragon() };
            bosses = new Battler[] { new SpiderQueen() };
        }
    }
}
