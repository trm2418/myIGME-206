using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public class ForestGuardian : Battler
    {
        public static Skill ForestsWrath = new Skill("Forest's Wrath", 20, true, "all", 250, true);
        public ForestGuardian() : base("Forest Guardian", 100000, 5000, 250, false, ForestsWrath)
        {
            
        }
    }
}
