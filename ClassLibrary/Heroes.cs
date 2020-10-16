using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    // Player Hero class
    public class Player : Battler
    {
        public static Skill PowerAttack = new Skill("Power Attack", 15, true, "single", 200, false);

        // constructor with base stats
        public Player() : base("Player", 750, 75, 75, true, PowerAttack) { }
    }

    // Barbarian Hero class
    public class Barbarian : Battler
    {
        public Mod innerFury = new Mod("Inner Fury", 1);

        public static Skill ThrowWeapon = new Skill("Throw Weapon", 20, true, "single", 300, true, Effect("Disarm", 100));
        public static Skill ChainAttack = new Skill("Chain Attack", 25, true, "single", 200, false, Effect("HitAgain", 100));
        public static Skill Battlecry = new Skill("Battlecry", 25, false, "all", 0, true, Effect("+Attack 1", 100));
        public static Skill SavageStrike = new Skill("Savage Strike", 5, true, "all", 6000000, false, Effect("Bleed 2", 100));
        public override string GetInfo()
        {
            string returnString = "";
            returnString += "Focused Rage: Passive. ignore chance to hit self with rage (not implemented)\n";
            returnString += $"Inner Fury: Passive. deal 1% extra damage for each 1% of health you are missing. Current Boost: +{Math.Round((1 - (hp / (double)maxHp)) * 100, 2)}% \n";
            returnString += $"Throw Weapon: {ThrowWeapon.manaCost} mana, physical, ranged, {ThrowWeapon.baseDamage} damage to an enemy but lose your weapon for one turn\n";
            returnString += $"Chain Attack: {ChainAttack.manaCost} mana, physical, melee, {ChainAttack.baseDamage} damage to an enemy. if it dies, hit the next weakest enemy with 50% more damage. continue until it doesn't kill\n";
            returnString += $"Battlecry: {Battlecry.manaCost} mana, no contact, ranged, give all allies {Battlecry.effects[0].name}\n";
            returnString += $"Savage Strike: {SavageStrike.manaCost} mana, physical, melee, {SavageStrike.baseDamage} damage to an enemy with {SavageStrike.pierce}% pierce and {SavageStrike.effects[0].chance}% {SavageStrike.effects[0].name}\n";
            return returnString;
        }

        // constructor with base stats
        public Barbarian() : base("Barbarian", 1100, 125, 105, true, ThrowWeapon, ChainAttack, Battlecry, SavageStrike)
        {
            attackModifiers.Add(innerFury);
            ChainAttack.hitAgainStyle = new Skill.HitAgainStyle(1.5f, 8, "killed target");
            SavageStrike.pierce = 25;
        }
    }

    // Sentinel Hero class
    public class Sentinel : Battler
    {
        public int saviorCooldown = 0;
        public float defender = 0.25f;
        public static Skill ShieldBash = new Skill("Shield Bash", 15, true, "single", 300, false, Effect("Stun 1", 100));
        public static Skill WallOfShields = new Skill("Wall of Shields", 20, false, "all", 0, true, Effect("+Defense 2", 100));
        public static Skill HeavyCharge = new Skill("Heavy Charge", 30, true, "single", 400, false, Effect("Stun 2", 100), Effect("HitAgain", 100));

        public override string GetInfo()
        {
            string returnString = "";
            returnString += $"Defender: Passive. Whenever an ally is attacked, they take {defender * 100}% less damage and it is dealt to the Sentinel instead\n";
            returnString += $"Sturdy: Passive. +{damageReduction}% damage reduction\n";
            returnString += $"Savior: Passive. When an ally would take fatal damage from an attack, the Sentinel will take the full hit. can only be used every other turn.\n";
            returnString += $"Shield Bash: {ShieldBash.manaCost} mana, physical, melee, {ShieldBash.baseDamage} damage to an enemy and {ShieldBash.effects[0].name}\n";
            returnString += $"Wall of Shields: {WallOfShields.manaCost} mana, no contact, ranged, give all allies {WallOfShields.effects[0].name}\n";
            returnString += $"Heavy Charge: {HeavyCharge.manaCost} mana, physical, melee, {HeavyCharge.baseDamage} damage to an enemy and {HeavyCharge.effects[0].name}, {(int)(HeavyCharge.baseDamage * HeavyCharge.hitAgainStyle.damageMult)}x damage to the enemy behind\n";
            return returnString;
        }

        // constructor with base stats
        public Sentinel() : base("Sentinel", 1350, 70, 135, true, WallOfShields, ShieldBash, HeavyCharge)
        {
            damageReduction = 25;
            HeavyCharge.hitAgainStyle = new Skill.HitAgainStyle(0.5f, 2, "behind");
        }
    }

    // Sorcerer Hero class
    public class Sorcerer : Battler
    {
        public float[] manaFlow = new float[] { 2, 25 };
        public float magicAbsorb = 30;
        public static Skill MagicArmor = new Skill("Magic Armor", 10, false, "single", 0, true, Effect("+Defense 4", 100));
        public static Skill Frostbolt = new Skill("Frostbolt", 20, true, "single", 250, true, Effect("Freeze 1", 50));
        public static Skill Fireball = new Skill("Fireball", 30, true, "single", 400, true, Effect("Burn 1", 75));
        public static Skill Manastorm = new Skill("Manastorm", 40, true, "all", 200, true);

        public override string GetInfo()
        {
            return "Info\n";
        }

        // constructor with base stats
        public Sorcerer() : base("Sorcerer", 800, 180, 80, true, MagicArmor, Frostbolt, Fireball, Manastorm)
        {
            Attack.ranged = true;
            manaRegen += manaFlow[0];
            manaRetention = manaFlow[1];
            Fireball.isFire = true;
        }
    }

    // Paladin Hero class
    public class Paladin : Battler
    {
        public float divineBlessing = 50;
        public static Skill Smite = new Skill("Smite", 20, true, "single", 300, false, Effect("Heal", 100));
        public static Skill HolyLight = new Skill("Holy Light", 30, false, "all", 0, true, Effect("Heal", 100));
        public static Skill MajorHealing = new Skill("Major Healing", 20, false, "single", 0, true, Effect("Heal", 100));

        public override string GetInfo()
        {
            return "Info\n";
        }

        public Battler[] DivineBlessingTargets()
        {
            Battler[] targets = new Battler[9];
            GetAdjacent().CopyTo(targets, 1);
            targets[0] = this;
            return targets;
        }

        // constructor with base stats
        public Paladin() : base("Paladin", 1200, 100, 120, true, MajorHealing, Smite, HolyLight)
        {
            Smite.healStyle = new Skill.HealStyle(300, 0, "self");
            HolyLight.healStyle = new Skill.HealStyle(200, 0, "ally");
            MajorHealing.healStyle = new Skill.HealStyle(800, 0, "ally");
        }
    }

    // Witch Doctor Hero class
    public class WitchDoctor : Battler
    {
        public static Skill VoodooCurse = new Skill("Voodoo Curse", 20, true, "single", 250, true, Effect("Curse 1", 100));
        public static Skill PoisonMist = new Skill("Poison Mist", 30, true, "all", 50, true, Effect("Poison 1", 75));
        public static Skill VoodooHealing = new Skill("Voodoo Healing", 10, false, "single", 0, true, Effect("Heal", 100), Effect("Curse 1", 35));

        public WitchDoctor() : base("Witch Doctor", 950, 155, 90, true, VoodooHealing, VoodooCurse, PoisonMist)
        {
            immunities.Add("Poison");
            immunities.Add("Curse");
            Attack.ranged = true;
            VoodooHealing.healStyle = new Skill.HealStyle(750, 0, "ally");
        }
    }

    //Shadowalker Hero class
    public class Shadowalker : Battler
    {
        public float precision = 10;
        public static Skill ShadowShuriken = new Skill("Shadow Shuriken", 1, true, "single", 150, true, Effect("HitAgain", 100));
        public static Skill Backstab = new Skill("Backstab", 3, true, "single", 500, true);
        public Shadowalker() : base("Shadowalker", 800, 170, 90, true, ShadowShuriken, Backstab)
        {
            stealth = true;
            critChance += precision;
            ShadowShuriken.hitAgainStyle = new Skill.HitAgainStyle(1, 3, "random");
            Backstab.triggersBleed = true;
        }
    }

    // Ranger Hero class
    public class Ranger : Battler
    {
        public float precision = 10;
        public static Skill Mark = new Skill("Mark", 10, true, "single", 0, true, Effect("-Defense 3", 100));
        public static Skill FlamingArrow = new Skill("Flaming Arrow", 10, true, "single", 175, true, Effect("Burn 1", 75));
        public static Skill SplitShot = new Skill("Split Shot", 30, true, "single", 400, true, Effect("HitAgain", 100));
        public static Skill PowerShot = new Skill("Power Shot", 20, true, "single", 250, true, Effect("Bleed 1", 100), Effect("HitAgain", 100));

        public Ranger() : base("Ranger", 900, 150, 100, true, Mark, FlamingArrow, PowerShot, SplitShot)
        {
            critChance += precision;
            Attack.ranged = true;
            FlamingArrow.isFire = true;
            SplitShot.hitAgainStyle = new Skill.HitAgainStyle(1, 3, "next to");
            PowerShot.hitAgainStyle = new Skill.HitAgainStyle(2 / 3f, 2, "behind");
            PowerShot.pierce = 25;
        }
    }

    // Death Knight Hero class
    public class DeathKnight : Battler
    {
        public static Skill FrostBlade = new Skill("Frost Blade", 15, true, "single", 200, false, Effect("Freeze 1", 100));
        public static Skill Plague = new Skill("Plague", 30, true, "all", 0, true, Effect("Poison 1", 100), Effect("-Attack 2", 100));
        public static Skill Doomblast = new Skill("Doomblast", 25, true, "single", 400, true);
        public static Skill DrainLife = new Skill("Drain Life", 30, true, "single", 350, true);

        public DeathKnight() : base("Death Knight", 1250, 120, 120, true, FrostBlade, Plague, Doomblast, DrainLife)
        {
            damageReduction = 20;
            undead = true;
            immunities.AddRange(new string[] { "Poison", "Bleed", "Sleep", "Confuse" });
            DrainLife.lifesteal = 50;
        }
    }

}
