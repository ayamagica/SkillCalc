using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;

namespace SkillCalc
{
    internal class Monsters
    {
        public int Armor {get; set;}
        public double DecreasedWithLvL { get; set;}
        public double ArmDecreasedDmg { get;set;}
        public double PD { get; set;}

        public double DecreasedDmg(double armor, int BU)
        {
            double DecreasedDamage = 100 * (armor / ((40 * (105 + BU)) + armor - 25));
            return Math.Round(DecreasedDamage, 2);
        }

        public double DefenceWithPen(double armor, double pen)
        {
            double IgnDefence = Math.Round(pen / (10000 + pen), 4);
            double DefenceWithPen = (1 - IgnDefence) * armor;
            return Math.Round(DefenceWithPen, 0);
        }


        public double DecreasedLvL(int DifferentLevel)
        {

            if (DifferentLevel < 3)
            {
                DecreasedWithLvL = 1;
            }
            else if (DifferentLevel >= 3 & DifferentLevel <= 5)
            {
                DecreasedWithLvL = 0.9;
            }
            else if (DifferentLevel >= 6 & DifferentLevel <= 8)
            {
                DecreasedWithLvL = 0.8;
            }
            else if (DifferentLevel >= 9 & DifferentLevel <= 11)
            {
                DecreasedWithLvL = 0.7;
            }
            else if (DifferentLevel >= 12 & DifferentLevel <= 15)
            {
                DecreasedWithLvL = 0.6;
            }
            else if (DifferentLevel >= 16 & DifferentLevel <= 25)
            {
                DecreasedWithLvL = 0.5;
            }
            else if (DifferentLevel >= 26)
            {
                DecreasedWithLvL = 0.25;
            }
            return DecreasedWithLvL;
        }

    }
}
