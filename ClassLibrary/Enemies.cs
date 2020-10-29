using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    // Wolf enemy class
    public class Wolf : Battler
    {
        public static Skill Bite = new Skill("Bite", 15, true, "single", 300, false, Effect("Bleed 1", 75));

        // constructor with base stats
        public Wolf() : base("Wolf", 500, 140, 100, false, Bite) { }
    }

    // Spriggan enemy class
    public class Spriggan : Battler
    {
        public static Skill PowerAttack = new Skill("Power Attack", 15, true, "single", 350, false);
        public static Skill ForestCurse = new Skill("Forest Curse", 25, true, "all", 0, true, Effect("Curse 1", 60));

        // constructor with base stats
        public Spriggan() : base("Spriggan", 750, 110, 110, false, PowerAttack, ForestCurse) { }
    }

    // Goblin enemy class
    public class Goblin : Battler
    {
        public static Skill Stab = new Skill("Stab", 20, true, "single", 350, false, Effect("Bleed 1", 100));

        // constructor with base stats
        public Goblin() : base("Goblin", 650, 130, 90, false, Stab) { }
    }

    // Spider enemy class
    public class Spider : Battler
    {
        public static Skill WebShot = new Skill("Web Shot", 20, true, "single", 325, true, Effect("Stun 2", 100));
        public static Skill PoisonBite = new Skill("Poison Bite", 15, true, "single", 250, false, Effect("Poison 1", 75));

        // constructor with base stats
        public Spider() : base("Spider", 600, 120, 100, false, PoisonBite, WebShot) { }
    }

    // Mushroom Monster enemy class
    public class MushroomMonster : Battler
    {
        public static Skill ToxicSpores = new Skill("Toxic Spores", 20, true, "all", 0, true, Effect("Poison 1", 40));
        public static Skill SleepSpores = new Skill("Sleep Spores", 20, true, "all", 0, true, Effect("Sleep 1", 40));
        public static Skill ShockSpores = new Skill("Shock Spores", 20, true, "all", 0, true, Effect("Shock 1", 40));

        public MushroomMonster() : base("Mushroom Monster", 550, 100, 120, false, ToxicSpores, SleepSpores, ShockSpores) { }
    }

    // Bat enemy class
    public class Bat : Battler
    {
        public static Skill Screech = new Skill("Screech", 20, true, "all", 75, true, Effect("Stun 1", 50));

        public Bat() : base("Bat", 450, 180, 80, false, Screech) { }
    }

    // Gnome enemy class
    public class Gnome : Battler
    {
        public static Skill DrainLife = new Skill("Drain Life", 20, true, "single", 350, true);
        public static Skill FreezeSpell = new Skill("Freeze Spell", 15, true, "single", 0, true, Effect("Freeze 3", 100));

        public Gnome() : base("Gnome", 550, 115, 140, false, FreezeSpell, DrainLife)
        {
            DrainLife.lifesteal = 100;
        }
    }

    // Vine Monster enemy class
    public class VineMonster : Battler
    {
        public static Skill VineSmash = new Skill("Vine Smash", 20, true, "single", 500, false, Effect("Stun 2", 50));

        public VineMonster() : base("Vine Monster", 800, 130, 120, false, VineSmash) { }
    }

    // Dragonling enemy class
    public class Dragonling : Battler
    {
        public static Skill FireSpit = new Skill("Fire Spit", 15, true, "single", 250, true, Effect("Burn 1", 100));
        public static Skill BlazingHeat = new Skill("Blazing Heat", 20, true, "all", 50, true, Effect("Burn 2", 50));

        public Dragonling() : base("Dragonling", 700, 150, 110, false, FireSpit, BlazingHeat)
        {
            FireSpit.isFire = true;
            BlazingHeat.isFire = true;
        }
    }
}
