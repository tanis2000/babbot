/*
  This file is part of PPather.

    PPather is free software: you can redistribute it and/or modify
    it under the terms of the GNU Lesser General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    PPather is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public License
    along with PPather.  If not, see <http://www.gnu.org/licenses/>.

*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/*
 * Contributor(s): toblakai, swolbyn
 */ 
namespace Pather.Helpers
{
    public class Item
    {
        public string Name { get; set; }
        public int WowHeadId { get; set; }
        public string Type { get; set; }
        public string SubType { get; set; }
        public string Classes { get; set; }
        public int Required { get; set; }
        public string Icon { get; set; }

        /* weapon and armor */
        public string Slot { get; set; }
        public int Armor { get; set; }
        public int Block { get; set; }
        public int Agility { get; set; }
        public int Intellect { get; set; }
        public int Stamina { get; set; }
        public int Spirit { get; set; }
        public int Strength { get; set; }
        public int Health { get; set; }
        public int Mana { get; set; }
        public int HealthRegen { get; set; }
        public int ManaRegen { get; set; }
        public int ResistArcane { get; set; }
        public int ResistFire { get; set; }
        public int ResistFrost { get; set; }
        public int ResistHoly { get; set; }
        public int ResistNature { get; set; }
        public int ResistShadow { get; set; }
        public int DamageArcane { get; set; }
        public int DamageFire { get; set; }
        public int DamageFrost { get; set; }
        public int DamageHoly { get; set; }
        public int DamageNature { get; set; }
        public int DamageShadow { get; set; }
        public int AttackPower { get; set; }
        public int RangedAttackPower { get; set; }
        public int RangedCrit { get; set; }
        // *note this is %percentage on wowhead site
        public int RangedAttackSpeed { get; set; }
        public int AttackPowerFeral { get; set; }
        public int Expertise { get; set; }
        public int Defense { get; set; }
        public int Resilience { get; set; }
        public int ArmorPenetration { get; set; }
        public int ShieldBlock { get; set; }
        public int CriticalStrike { get; set; }
        public int Hit { get; set; }
        public int Dodge { get; set; }
        public int Parry { get; set; }
        public int SpellPower { get; set; }
        public int SpellHit { get; set; }
        public int SpellCrit { get; set; }
        public int SpellHaste { get; set; }
        public int SpellPenetration { get; set; }

        /* weapon only */
        public int MinDamage { get; set; }
        public int MaxDamage { get; set; }
        public double Speed { get; set; }
        public double DPS { get; set; }

        public Item()
        {
            Name = "0";
            Icon = "0";
            Slot = "0";
            Type = "0";
            SubType = "0";
            WowHeadId = 0;
            Classes = "0";
            Required = 0;
            Armor = 0;
            Block = 0;
            Agility = 0;
            Intellect = 0;
            Stamina = 0;
            Spirit = 0;
            Strength = 0;
            Health = 0;
            Mana = 0;
            HealthRegen = 0;
            ManaRegen = 0;
            ResistArcane = 0;
            ResistFire = 0;
            ResistFrost = 0;
            ResistHoly = 0;
            ResistNature = 0;
            ResistShadow = 0;
            DamageArcane = 0;
            DamageFire = 0;
            DamageFrost = 0;
            DamageHoly = 0;
            DamageNature = 0;
            DamageShadow = 0;
            AttackPower = 0;
            RangedAttackPower = 0;
            RangedCrit = 0;
            RangedAttackSpeed = 0;
            AttackPowerFeral = 0;
            Expertise = 0;
            Defense = 0;
            Resilience = 0;
            ArmorPenetration = 0;
            ShieldBlock = 0;
            CriticalStrike = 0;
            Hit = 0;
            Dodge = 0;
            Parry = 0;
            SpellPower = 0;
            SpellHit = 0;
            SpellCrit = 0;
            SpellHaste = 0;
            SpellPenetration = 0;
            MinDamage = 0;
            MaxDamage = 0;
            Speed = 0.0;
            DPS = 0.0;
        }

        public override string ToString()
        {
            string output = "";
            output += "=== ITEM ===\n";
            output += "Name\t"+Name + "\n";
            output += "WowHeadId\t" + WowHeadId + "\n";
            output += "Type\t" +  Type+ "\n";
            output += "SubType\t" + SubType + "\n";
            output += "Classes\t" + Classes + "\n";
            output += "Required\t" + Required + "\n";
            output += "Icon\t" + Icon + "\n";
            output += "Slot\t" + Slot + "\n";
            output += "Armor\t" + Armor + "\n";
            output += "Block\t" + Block + "\n";
            output += "Agility\t" + Agility + "\n";
            output += "Intellect\t" + Intellect + "\n";
            output += "Stamina\t" + Stamina + "\n";
            output += "Spirit\t" + Spirit + "\n";
            output += "Strength\t" + Strength + "\n";
            output += "Health\t" + Health + "\n";
            output += "Mana\t" + Mana + "\n";
            output += "HealthRegen\t" + HealthRegen + "\n";
            output += "ManaRegen\t" + ManaRegen + "\n";
            output += "ResistArcane\t" + ResistArcane + "\n";
            output += "ResistFire\t" + ResistFire + "\n";
            output += "ResistFrost\t" + ResistFrost + "\n";
            output += "ResistHoly\t" + ResistHoly + "\n";
            output += "ResistNature\t" + ResistNature + "\n";
            output += "ResistShadow\t" + ResistShadow + "\n";
            output += "DamageArcane\t" + DamageArcane + "\n";
            output += "DamageFire\t" + DamageFire + "\n";
            output += "DamageFrost\t" + DamageFrost + "\n";
            output += "DamageHoly\t" + DamageHoly + "\n";
            output += "DamageNature\t" + DamageNature + "\n";
            output += "DamageShadow\t" + DamageShadow + "\n";
            output += "AttackPower\t" + AttackPower + "\n";
            output += "RangedAttackPower\t" + RangedAttackPower + "\n";
            output += "RangedCrit\t" + RangedCrit + "\n";
            output += "RangedAttackSpeed\t" + RangedAttackSpeed + "\n";
            output += "AttackPowerFeral\t" + AttackPowerFeral + "\n";
            output += "Expertise\t" + Expertise + "\n";
            output += "Defense\t" + Defense + "\n";
            output += "Resilience\t" + Resilience + "\n";
            output += "ArmorPenetration\t" + ArmorPenetration + "\n";
            output += "ShieldBlock\t" + ShieldBlock + "\n";
            output += "CriticalStrike\t" + CriticalStrike + "\n";
            output += "Hit\t" + Hit + "\n";
            output += "Dodge\t" + Dodge + "\n";
            output += "Parry\t" + Parry + "\n";
            output += "SpellPower\t" + SpellPower + "\n";
            output += "SpellHit\t" + SpellHit + "\n";
            output += "SpellCrit\t" + SpellCrit + "\n";
            output += "SpellHaste\t" + SpellHaste + "\n";
            output += "SpellPenetration\t" + SpellPenetration + "\n";
            output += "MinDamage\t" + MinDamage + "\n";
            output += "MaxDamage\t" + MaxDamage + "\n";
            output += "Speed\t" + Speed + "\n";
            output += "DPS\t" + DPS + "\n";
            return output;
        }
    }
}
