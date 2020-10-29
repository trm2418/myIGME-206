using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public class SpiderQueen : Battler
    {
        public static Skill PoisonBite = new Skill("Poison Bite", 15, true, "single", 200, false, Effect("Poison 2", 100));
        public static Skill ToxicWeb = new Skill("Toxic Web", 25, true, "all", 100, true, Effect("Poison 1", 100));
        public static Skill WebShot = new Skill("Web Shot", 20, true, "single", 300, true, Effect("Stun 3", 100));
        public static Skill Devour = new Skill("Devour", 15, true, "single", 350, false, Effect("Heal", 100));
        public SpiderQueen() : base("Spider Queen", 15000, 1500, 200, false, PoisonBite, ToxicWeb, WebShot, Devour)
        {
            boss = true;
            Devour.healStyle = new Skill.HealStyle(0, 0.2f, "self");
            Devour.healStyle.condition = "kill";
        }
    }
}
