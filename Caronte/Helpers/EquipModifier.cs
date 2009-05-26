using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pather.Helpers
{
    public class EquipModifier
    {
        public double Agility { get; set; }
        public double Strength { get; set; }
        public double Intellect { get; set; }
        public double Spirit { get; set; }
        public double Stamina { get; set; }
        public double Armor { get; set; }
        public double Block { get; set; }
        public double DPS { get; set; }

        public double AttackPower { get; set; }
        public double RangedAttackPower { get; set; }

        public double Defense { get; set; }
        public double Resilience { get; set; }
        public double Dodge { get; set; }
        public double Parry { get; set; }
        public double Hit { get; set; }
        public double Crit { get; set; }

        public double SpellPower { get; set; }
        public double SpellHit { get; set; }
        public double SpellCrit { get; set; }
        public double MP5 { get; set; }

        public double DamageShadow { get; set; }

        public string WantedArmor { get; set; }

        public EquipModifier(string PlayerClass, bool ArmorUpgrade)
        {
            //PPather.WriteLine(String.Format("EquipModifier: Initialising for {0} class with WantedArmor = {1}", PlayerClass, WantedArmor));
            /* init values */
            Agility = 0;
            Strength = 0;
            AttackPower = 0;
            RangedAttackPower = 0;
            Defense = 0;
            Resilience = 0;
            Dodge = 0;
            Parry = 0;
            Hit = 0;
            Crit = 0;
            SpellPower = 0;
            SpellHit = 0;
            SpellCrit = 0;
            MP5 = 0;
            DamageShadow = 0;

            /* some defaults */
            Stamina = 0.1;
            Armor = 0.04;
            DPS = 1;

            switch (PlayerClass)
            {
                case "Warrior":
                    Agility = 1.25;
                    Strength = 2;
                    Spirit = 0.1;
                    Stamina = 1.25;
                    Armor = 0.025;
                    DPS = 10;
                    AttackPower = 1;
                    Dodge = 10;
                    Parry = 15.385;
                    Hit = 20;
                    Crit = 20;
                    this.WantedArmor = "Mail";
                    if (ArmorUpgrade)
                        this.WantedArmor = "Plate";
                    break;
                case "Rogue":
                    Agility = 2;
                    Strength = 1;
                    Spirit = 0.1;
                    Stamina = 1.6;
                    DPS = 10;
                    AttackPower = 1;
                    Defense = 0.4;
                    Dodge = 10;
                    Parry = 15.385;
                    Hit = 15.385;
                    Crit = 20;
                    this.WantedArmor = "Leather";
                    break;
                case "Warlock":
                    Intellect = 1.6;
                    Spirit = 1;
                    Stamina = 2;
                    Armor = 0.05;
                    DPS = 0.1;
                    SpellPower = 4;
                    SpellHit = 20;
                    SpellCrit = 20;
                    MP5 = 2;
                    DamageShadow = 6;
                    this.WantedArmor = "Cloth";
                    break;
                case "Shaman":
                    Agility = 1;
                    Strength = 2;
                    Intellect = 2;
                    Spirit = 0.2;
                    Stamina = 2;
                    DPS = 10;
                    AttackPower = 1;
                    Defense = 0.4;
                    Dodge = 20;
                    Hit = 15.385;
                    Crit = 20;
                    SpellPower = 0.2;
                    SpellHit = 10;
                    SpellCrit = 10;
                    MP5 = 0.4;
                    this.WantedArmor = "Leather";
                    if (ArmorUpgrade)
                        this.WantedArmor = "Mail";
                    break;
                case "Druid":
                    Agility = 2;
                    Strength = 2;
                    Intellect = 2;
                    Spirit = 0.667;
                    Stamina = 2;
                    Armor = 0.025;
                    DPS = 2.5;
                    AttackPower = 2;
                    Defense = 0.4;
                    Dodge = 20;
                    Hit = 10;
                    Crit = 10;
                    SpellPower = 4;
                    SpellHit = 10;
                    SpellCrit = 10;
                    MP5 = 0.4;
                    this.WantedArmor = "Leather";
                    break;
                case "Priest":
                    Intellect = 1;
                    Spirit = 2;
                    Stamina = 2;
                    Armor = 0.5;
                    DPS = 0.1;
                    SpellPower = 6;
                    SpellHit = 20;
                    SpellCrit = 20;
                    MP5 = 3.077;
                    this.WantedArmor = "Cloth";
                    break;
                case "Paladin":
                    Agility = 1;
                    Strength = 2;
                    Intellect = 2;
                    Spirit = 0.2;
                    Stamina = 2;
                    Armor = 0.025;
                    DPS = 10;
                    AttackPower = 1;
                    Defense = 0.4;
                    Dodge = 20;
                    Parry = 10;
                    Hit = 10;
                    Crit = 20;
                    SpellPower = 0.6;
                    MP5 = 0.4;
                    this.WantedArmor = "Mail";
                    if (ArmorUpgrade)
                        this.WantedArmor = "Plate";
                    break;
                case "Mage":
                    Intellect = 2;
                    Spirit = 1;
                    Stamina = 2;
                    Armor = 0.05;
                    DPS = 0.1;
                    SpellPower = 2.5;
                    SpellHit = 10;
                    SpellCrit = 20;
                    MP5 = 1.6;
                    this.WantedArmor = "Cloth";
                    break;
                case "Hunter":
                    Agility = 2;
                    Strength = 1.6;
                    Intellect = 1.6;
                    Spirit = 0.5;
                    Stamina = 1.6;
                    Armor = 0.031;
                    DPS = 10;
                    AttackPower = 2;
                    RangedAttackPower = 2;
                    Defense = 0.4;
                    Dodge = 10;
                    Parry = 10;
                    Hit = 15.385;
                    Crit = 20;
                    MP5 = 1;
                    this.WantedArmor = "Leather";
                    if (ArmorUpgrade)
                        this.WantedArmor = "Mail";
                    break;
                default:
                    break;
            }
		}
    }
}
