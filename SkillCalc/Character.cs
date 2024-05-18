using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillCalc
{
    internal class Character
    {
        public int MinDamage { get; set; }
        public int MaxDamage { get; set; }
        public int MainStat { get; set; }
        public int MainStatType { get; set; }
        public double Buffs { get; set; }
        public int PA { get; set; }
        public int BD { get; set; }
        public int MonsterDamage { get; set; }
        public int IncreasedSkillDamage { get; set; }
        public double AmplificationPassiveDamage { get; set; }
        public double DamageAmplifier { get; set; }
        public double CrystallAmplifier { get; set; }
        public int BonusLevel { get; set; }
        public double seal { get; set; }
        public double dragons { get; set; }
        public double weakness { get; set; }
        public double poison { get; set; }
        public double fuse { get; set; }
        public double ResultAmplification { get; set; }
        public int ResistDebuff { get; set; }


        public double CalculatePercentageDecrease(double originalValue, double percentageDecrease)
        {
            double decreaseAmount = (originalValue / 100) * percentageDecrease;
            double newValue = originalValue - decreaseAmount;
            return Math.Max(newValue, 0); // Обеспечивает, что результат не будет меньше 0
        }

        public double CalculateDWeapon(int BaseDamage, int MainStat, int MainStatType, int PassivesType)
        {
            if (MainStatType == 0 || MainStatType == 1) // 0 - Strength, 1 - Dexterity
            {
                double DWeapon = BaseDamage / Math.Round((1 + (MainStat / 150.0) + (PassivesType == 0 ? 0.75 : 0.9)), 2);
                return Math.Round(DWeapon, 0);
            }
            else if (MainStatType == 2) // 2 - Intelligence
            {
                double DWeapon = BaseDamage / Math.Round((1 + (MainStat / 100.0)), 2);
                return Math.Round(DWeapon, 0);
            }
            return 0;
        }

        public double CalculateBaseDamageWithBuffs(double MainStat, int MainStatType, int PassivesType, double Buffs, double DWeapon)
        {
            if (MainStatType == 0 || MainStatType == 1) // 0 - Strength, 1 - Dexterity
            {
                double BaseDamageWithFullBuffs = DWeapon * Math.Round((1 + (MainStat / 150) + (PassivesType == 0 ? 0.75 : 0.9) + Buffs ), 2);
                return Math.Round(BaseDamageWithFullBuffs, 0);

            }
            else if (MainStatType == 2) // 2 - Intelligence
            {
                double BaseDamageWithFullBuffs = DWeapon * Math.Round((1 + (MainStat / 100) + Buffs), 2);
                return Math.Round(BaseDamageWithFullBuffs, 0);
            }
            return 0;
        }

        public double PA_Amplification (double PA, double PD, double fuse)
        {
            if (PA > PD - fuse)
            {

                double PA_Amplification = 1 + (PA - PD + fuse) / 100;

                return Math.Round(PA_Amplification, 2);
            }
            else
            {
                double PA_Amplification = 1 / (1 + (1.2 * (PD - PA - fuse) / 100));

                return Math.Round(PA_Amplification, 2);
            }
        }

        public double MD_Amplification (double MonsterDamage)
        {
            double MD_Amplif = 1 + (3 * MonsterDamage / (300 + MonsterDamage));
            return Math.Round(MD_Amplif, 2);
        }

        public double BD_Amplification (double BD)
        {
            double BD_Amplif = (4000 + BD) / 4000;
            return Math.Round(BD_Amplif, 2);
        }

    }
}
