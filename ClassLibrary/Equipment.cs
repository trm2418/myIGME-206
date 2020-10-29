using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public class Equipment
    {
        public string name;
        public string type;
        public int hp;
        public int atk;
        public int def;
        public Battler hero = null;

        public void Equip(Battler hero)
        {
            if (hero.loadout[type] != null)
            {
                hero.loadout[type].Unequip(hero);
            }
            hero.loadout[type] = this;
            hero.maxHp += hp;
            hero.atk += atk;
            hero.def += def;
            GameHandler.inventory[this]--;
            Console.WriteLine("Equipped " + name);
        }

        public void Unequip(Battler hero)
        {
            Console.WriteLine("Unequipped " + name);
            hero.loadout[type] = null;
            hero.maxHp -= hp;
            hero.atk -= atk;
            hero.def -= def;
            GameHandler.inventory[this]++;
        }

        public Equipment(string name, string type, int hp, int atk, int def)
        {
            this.name = name;
            this.type = type;
            this.hp = hp;
            this.atk = atk;
            this.def = def;
        }
    }
}
