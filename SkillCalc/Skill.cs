using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillCalc
{
    internal class Skill
    {
        public double MultipleBaseDamage { get; set; }
        public int AdditionalDamage { get; set; }
        public double MultipleDWeapon { get; set; }

        public double SkillDamage (double BaseDamage, int AdditionalDamage, double DWeapon, double MultipleBaseDamage, double MultipleDWeapon)
        {
            double Damage = (MultipleBaseDamage * BaseDamage) + (MultipleDWeapon * DWeapon) + AdditionalDamage;

            return Math.Round(Damage, 2);
        }

    }
}
