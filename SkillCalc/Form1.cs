using SkillCalc.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar;

namespace SkillCalc
{

    public partial class Form1 : Form
    {
        Character character = new Character();
        Skill skill = new Skill();
        Monsters monsters = new Monsters();

        public Form1()
        {
            InitializeComponent();

            MainStatType.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            MainStatType.AutoCompleteSource = AutoCompleteSource.ListItems;

            PassivType.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            PassivType.AutoCompleteSource = AutoCompleteSource.ListItems;       

            MinBaseDamage.Text = Settings1.Default.MinBaseDamage;
            MaxBaseDamage.Text = Settings1.Default.MaxBaseDamage;
            MainStatbx.Text = Settings1.Default.MainStat;
            MainStatType.SelectedItem = Settings1.Default.MainStatType;
            PassivType.SelectedItem = Settings1.Default.PassiveType;
            PA_bx.Text = Settings1.Default.PA;
            PD_enemy.Text = Settings1.Default.PDEnemy;
            BD_bx.Text = Settings1.Default.BD;
            MD_bx.Text = Settings1.Default.MD;
            DifLvL.Text = Settings1.Default.DiffLvl;
            monstArmor.Text = Settings1.Default.Defense;
            ResDebuff.Text = Settings1.Default.DebuffDef;
            Pen.Text = Settings1.Default.Pen;
            BonusLevel.Text = Settings1.Default.BLvl;
            DamageAmplifier.Text = Settings1.Default.AmplDmg;
            CrystallAmpl.Text = Settings1.Default.CrystallAmp;
            IncBuffs.Text = Settings1.Default.Buffs;
            PercentFuse.Text = Settings1.Default.Fuse;
            PercentWeakness.Text = Settings1.Default.Weakness;
            PercentPoison.Text = Settings1.Default.Poison;
            base_damage.Text = Settings1.Default.BaseDamage;
            d_weapon.Text = Settings1.Default.AmpDWeapon;
            Add_damage.Text = Settings1.Default.AddDamage;
            Seal.Checked = Settings1.Default.SealCheck;
            Fuse.Checked = Settings1.Default.FuseCheck;
            Dragons.Checked = Settings1.Default.DragonsCheck;
            Weakness.Checked = Settings1.Default.WeaknessCheck;
            Poison.Checked = Settings1.Default.PoisonCheck;
        }

        public void textChanged ()
        {
            character.seal = Seal.Checked ? 1.5 : 1.0;
            character.fuse = Fuse.Checked && double.TryParse(PercentFuse.Text, out double fuse) ? fuse : 0;
            character.dragons = Dragons.Checked ? 2.0 : 1.0;
            character.weakness = Weakness.Checked && double.TryParse(PercentWeakness.Text, out double weakness) ? weakness : 0;
            character.poison = Poison.Checked && double.TryParse(PercentPoison.Text, out double poison) ? poison : 0;
            character.fuse = character.fuse * 0.6;
            FuseDebuff.Text = "Снижение пз с запала " + Convert.ToString(character.fuse);
            character.weakness = 1 + (character.weakness / 100);
            character.poison = 1 + (character.poison / 100);
            character.ResultAmplification = character.seal * character.dragons * character.weakness * character.poison;

            int.TryParse(MinBaseDamage.Text, out int MinDamage);
            character.MinDamage = MinDamage;

            int.TryParse(MaxBaseDamage.Text, out int MaxDamage);
            character.MaxDamage = MaxDamage;

            int.TryParse(MainStatbx.Text, out int MainStat);
            character.MainStat = MainStat;

            double.TryParse(IncBuffs.Text, out double Buffs);
            character.Buffs = Math.Round(Buffs / 100 , 2);

            int.TryParse(PA_bx.Text, out int PA);
            character.PA = PA;

            int.TryParse(PD_enemy.Text, out int PD);
            monsters.PD = PD;

            double PA_Ampl = character.PA_Amplification(PA, monsters.PD, character.fuse);
            PA_amp.Text = "Множитель урона от ПА " + Convert.ToString(PA_Ampl);

            int.TryParse(BD_bx.Text, out int BD);
            character.BD = BD;
            double BD_Ampl = character.BD_Amplification(BD);
            BD_amp.Text = "Множитель урона от БД " + Convert.ToString(BD_Ampl);

            int.TryParse(MD_bx.Text, out int MD);
            character.MonsterDamage = MD;
            double MD_Ampl = character.MD_Amplification(MD);
            MD_amp.Text = "Множитель урона от МД " + Convert.ToString(MD_Ampl);

            int.TryParse(DifLvL.Text, out int DiffLevel);
            monsters.DecreasedWithLvL = DiffLevel;
            double DiffLvL = monsters.DecreasedLvL(DiffLevel);
            DifferentLvL.Text = "Множитель урона за разницу уровней " + Convert.ToString(DiffLvL);

            int.TryParse(monstArmor.Text, out int MonstArmor);
            monsters.Armor = MonstArmor;

            int.TryParse(ResDebuff.Text, out int ResDeb);
            character.ResistDebuff = ResDeb;
            monsters.Armor = Convert.ToInt32(character.CalculatePercentageDecrease(monsters.Armor, character.ResistDebuff));

            int.TryParse(BonusLevel.Text, out int bonusLevel);
            character.BonusLevel = bonusLevel;

            double.TryParse(Pen.Text, out double pen);
            double M_ArmorWithPen = monsters.DefenceWithPen(monsters.Armor, pen);
            ArmorWithPen.Text = "Защиты с учетом пробивания " + Convert.ToString(M_ArmorWithPen);
            monsters.ArmDecreasedDmg = monsters.DecreasedDmg(M_ArmorWithPen, character.BonusLevel);
            double M_Armor = Math.Round(1 - (monsters.DecreasedDmg(M_ArmorWithPen, character.BonusLevel) / 100),4);
            MArmor.Text = "Снижение урона за счет брони " + Convert.ToString(monsters.ArmDecreasedDmg) + "%";

            double DWeaponMin = character.CalculateDWeapon(character.MinDamage, character.MainStat, MainStatType.SelectedIndex, PassivType.SelectedIndex);
            MinDWeapon.Text = Convert.ToString(DWeaponMin) + " Мин УО";

            double DWeaponMax = character.CalculateDWeapon(character.MaxDamage, character.MainStat, MainStatType.SelectedIndex, PassivType.SelectedIndex);
            MaxDWeapon.Text = Convert.ToString(DWeaponMax) + " Макс УО";

            double damagewithbuffsmin = character.CalculateBaseDamageWithBuffs(character.MainStat, MainStatType.SelectedIndex, PassivType.SelectedIndex, character.Buffs, DWeaponMin);
            DamageWithBuffsMin.Text = Convert.ToString(damagewithbuffsmin) + " Мин урон с бафом";

            double damagewithbuffsmax = character.CalculateBaseDamageWithBuffs(character.MainStat, MainStatType.SelectedIndex, PassivType.SelectedIndex, character.Buffs, DWeaponMax);
            DamageWithBuffsMax.Text = Convert.ToString(damagewithbuffsmax) + " Макс урон с бафом";

            double.TryParse(d_weapon.Text, out double dweapon);
            skill.MultipleDWeapon = dweapon / 100;
            double.TryParse(base_damage.Text, out double basedamage);
            skill.MultipleBaseDamage = basedamage / 100;
            int.TryParse(Add_damage.Text, out int AddDamage);
            skill.AdditionalDamage = AddDamage;

            double.TryParse(DamageAmplifier.Text, out double DamageAmplif);
            character.DamageAmplifier = 1 + ( DamageAmplif / 100);

            double.TryParse(CrystallAmpl.Text, out double DamageCrystall);
            character.CrystallAmplifier = 1 + (DamageCrystall / 100);

            if (MainStatType.SelectedIndex == 2)
            {
                character.AmplificationPassiveDamage = PassivType.SelectedIndex == 0 ? 1.2 : 1.25;
            }
            else
            {
                character.AmplificationPassiveDamage = 1;
            }

            double SKillDamageMin = skill.SkillDamage(damagewithbuffsmin, skill.AdditionalDamage, DWeaponMin, skill.MultipleBaseDamage, skill.MultipleDWeapon);
            SkillDamageMin.Text = "Мин урон скила " + Convert.ToString(SKillDamageMin);

            double SKillDamageMax = skill.SkillDamage(damagewithbuffsmax, skill.AdditionalDamage, DWeaponMax, skill.MultipleBaseDamage, skill.MultipleDWeapon);
            SkillDamageMax.Text = "Макс урон скила " + Convert.ToString(SKillDamageMax);

            double DamageWithSkillMin = SKillDamageMin * PA_Ampl * BD_Ampl * MD_Ampl * M_Armor * DiffLvL * character.AmplificationPassiveDamage * character.DamageAmplifier * character.CrystallAmplifier * character.ResultAmplification;
            DmgWithSkillMin.Text = "Мин. урон от скила с усилениями " + Convert.ToString(Math.Round(DamageWithSkillMin, 0));

            double DamageWithSkillMax = SKillDamageMax * PA_Ampl * BD_Ampl * MD_Ampl * M_Armor * DiffLvL * character.AmplificationPassiveDamage * character.DamageAmplifier * character.CrystallAmplifier * character.ResultAmplification;
            DmgWithSkillMax.Text = "Макс. урон от скила с усилениями " + Convert.ToString(Math.Round(DamageWithSkillMax, 0));


        }

        private void MinBaseDamage_TextChanged(object sender, EventArgs e)
        {
            textChanged();
        }

        private void MaxBaseDamage_TextChanged(object sender, EventArgs e)
        {
            textChanged();
        }

        private void MainStatbx_TextChanged(object sender, EventArgs e)
        {
            textChanged();
        }

        private void MainStatType_SelectedIndexChanged(object sender, EventArgs e)
        {
            textChanged();
        }

        private void PassivType_SelectedIndexChanged(object sender, EventArgs e)
        {
            textChanged();
        }

        private void IncBuffs_TextChanged(object sender, EventArgs e)
        {
            textChanged();
        }

        private void PA_bx_TextChanged(object sender, EventArgs e)
        {
            textChanged();
        }

        private void BD_bx_TextChanged(object sender, EventArgs e)
        {
            textChanged();
        }

        private void MD_bx_TextChanged(object sender, EventArgs e)
        {
            textChanged();
        }

        private void DifLvL_TextChanged(object sender, EventArgs e)
        {
            textChanged();
        }

        private void monstArmor_TextChanged(object sender, EventArgs e)
        {
            textChanged();
        }

        private void Pen_TextChanged(object sender, EventArgs e)
        {
            textChanged();
        }

        private void d_weapon_TextChanged(object sender, EventArgs e)
        {
            textChanged();
        }

        private void base_damage_TextChanged(object sender, EventArgs e)
        {
            textChanged();
        }

        private void Add_damage_TextChanged(object sender, EventArgs e)
        {
            textChanged();
        }

        private void DamageAmplifier_TextChanged(object sender, EventArgs e)
        {
            textChanged();
        }

        private void CrystallAmpl_TextChanged(object sender, EventArgs e)
        {
            textChanged();
        }

        private void BonusLevel_TextChanged(object sender, EventArgs e)
        {
            textChanged();
        }

        private void PD_enemy_TextChanged(object sender, EventArgs e)
        {
            textChanged();
        }

        private void PercentFuse_TextChanged(object sender, EventArgs e)
        {
            textChanged();
        }

        private void Seal_CheckedChanged(object sender, EventArgs e)
        {
            textChanged();
        }

        private void Fuse_CheckedChanged(object sender, EventArgs e)
        {
            textChanged();
        }

        private void Dragons_CheckedChanged(object sender, EventArgs e)
        {
            textChanged();
        }

        private void Weakness_CheckedChanged(object sender, EventArgs e)
        {
            textChanged();
        }

        private void Poison_CheckedChanged(object sender, EventArgs e)
        {
            textChanged();
        }

        private void PercentWeakness_TextChanged(object sender, EventArgs e)
        {
            textChanged();
        }

        private void PercentPoison_TextChanged(object sender, EventArgs e)
        {
            textChanged();
        }

        private void ResDebuff_TextChanged(object sender, EventArgs e)
        {
            textChanged();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Settings1.Default.MinBaseDamage = MinBaseDamage.Text;
            Settings1.Default.MaxBaseDamage = MaxBaseDamage.Text;
            Settings1.Default.MainStat = MainStatbx.Text;
            Settings1.Default.MainStatType = MainStatType.SelectedItem.ToString();
            Settings1.Default.PassiveType = PassivType.SelectedItem.ToString();
            Settings1.Default.PA = PA_bx.Text;
            Settings1.Default.PDEnemy = PD_enemy.Text;
            Settings1.Default.BD = BD_bx.Text;
            Settings1.Default.MD = MD_bx.Text;
            Settings1.Default.DiffLvl = DifLvL.Text;
            Settings1.Default.Defense = monstArmor.Text;
            Settings1.Default.DebuffDef = ResDebuff.Text;
            Settings1.Default.Pen = Pen.Text;
            Settings1.Default.BLvl = BonusLevel.Text;
            Settings1.Default.AmplDmg = DamageAmplifier.Text;
            Settings1.Default.CrystallAmp = CrystallAmpl.Text;
            Settings1.Default.Buffs = IncBuffs.Text;
            Settings1.Default.Fuse = PercentFuse.Text;
            Settings1.Default.Weakness = PercentWeakness.Text;
            Settings1.Default.Poison = PercentPoison.Text;
            Settings1.Default.BaseDamage = base_damage.Text;
            Settings1.Default.AmpDWeapon = d_weapon.Text;
            Settings1.Default.AddDamage = Add_damage.Text;
            Settings1.Default.SealCheck = Seal.Checked;
            Settings1.Default.FuseCheck = Fuse.Checked;
            Settings1.Default.DragonsCheck = Dragons.Checked;
            Settings1.Default.WeaknessCheck = Weakness.Checked;
            Settings1.Default.PoisonCheck = Poison.Checked;
            Settings1.Default.Save();

            System.Windows.Forms.Label SettingSave = new System.Windows.Forms.Label();
            SettingSave.Text = "Поля успешно сохранены!";
            SettingSave.Font = new Font("Montserrat", 14);
            SettingSave.AutoSize = true;
            SettingSave.ForeColor = Color.FromArgb(204, 204, 204);
            SettingSave.BackColor = Color.FromArgb(51, 51, 51);
            panel1.Controls.Add(SettingSave);
            SettingSave.Location = new Point(13, 615);
            SettingSave.Show();

            Timer timer = new Timer();
            timer.Interval = 2000; // 2 секунды
            timer.Tick += (t, args) => { SettingSave.Hide(); };
            timer.Start();
        }

        private class MyKeyHandler : IMessageFilter
        {
            private Form1 _form;

            public MyKeyHandler(Form1 form)
            {
                _form = form;
            }

            public bool PreFilterMessage(ref Message m)
            {
                if (m.Msg == 0x100 && (Keys)m.WParam == Keys.S && ModifierKeys == Keys.Control)
                {
                    _form.button1.PerformClick();
                    return true;
                }
                return false;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Application.AddMessageFilter(new MyKeyHandler(this));

        }
    }

}
