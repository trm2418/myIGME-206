using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    // Dragon enemy class
    public class Dragon : Battler
    {
        public static Skill DragonRoar = new Skill("Dragon Roar", 20, true, "all", 0, true, Effect("-Attack 3", 100));
        public static Skill FieryClaws = new Skill("Fiery Claws", 15, true, "single", 200, false, Effect("Burn 1", 50), Effect("Bleed 1", 50));
        public static Skill TailSwipe = new Skill("Tail Swipe", 25, true, "single", 400, false, Effect("Stun 2", 80));
        public static Skill DragonFire = new Skill("Dragon Fire", 30, true, "all", 150, true, Effect("Burn 2", 25), Effect("Burn 1", 75));
        public static Skill InfernoBlitz = new Skill("Inferno Blitz", 35, true, "single", 700, false, Effect("Burn 3", 100), Effect("Stun 3", 100));

        // constructor with base stats
        public Dragon() : base("Dragon", 5000, 500, 150, false, DragonRoar, FieryClaws, TailSwipe, DragonFire, InfernoBlitz)
        {
            miniBoss = true;
            armorPierce = 15;
            FieryClaws.critChance = 15;
            FieryClaws.isFire = true;
            DragonFire.isFire = true;
            InfernoBlitz.isFire = true;
            immunities.Add("Burn");
        }
    }

    // Vampire enemy class
    public class Vampire : Battler
    {
        public static Skill VampireBite = new Skill("Vampire Bite", 25, true, "single", 250, false);
        public static Skill DarknessBlast = new Skill("Darkness Blast", 25, true, "single", 250, false);
        public static Skill BloodRitual = new Skill("Blood Ritual", 30, true, "all", 150, true, Effect("Heal", 100));

        // constructor with base stats
        public Vampire() : base("Vampire", 4000, 400, 200, false, VampireBite)
        {
            miniBoss = true;
            armorPierce = 15;
            lifesteal = 25;
            VampireBite.lifesteal = 75;
        }
    }

    // Golem enemy class
    public class Golem : Battler
    {
        // Boulder Crush costs 30 mana, does 5x damage and has a 
        public static Skill BoulderCrush = new Skill("Boulder Crush", 30, true, "single", 500, false, Effect("Stun 3", 100), Effect("Stun 2", 100));

        // constructor with base stats
        public Golem() : base("Golem", 3000, 150, 300, false, BoulderCrush)
        {
            miniBoss = true;
        }
    }
}
