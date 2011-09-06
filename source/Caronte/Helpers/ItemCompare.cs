using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glider.Common.Objects;
using Pather.Helpers.UI;
using System.Threading;

namespace Pather.Helpers
{
    public static class ItemCompare
    {
        private static bool initialized = false;
        private static double eq_item_score = 0;
        private static double new_item_score = 0;

        /* standard options */
        private static bool EquipBOE = false;
        private static bool ArmorUpgrade = false;
        private static string MaxGearQuality = "Green";
        private static List<string> ProtectedSlots = null;
        private static List<string> WeaponSkills = null;
        private static bool EquipArmor = false;
        private static bool EquipWeapons = false;
        private static bool UseShield = false;
        private static bool PreferShield = false;

        /* overridable */
        private static double Agility = -1;
        private static double Strength = -1;
        private static double Intellect = -1;
        private static double Spirit = -1;
        private static double Stamina = -1;
        private static double Armor = -1;
        private static double Block = -1;
        private static double DPS = -1;
        private static double AttackPower = -1;
        private static double RangedAttackPower = -1;
        private static double Defense = -1;
        private static double Resilience = -1;
        private static double Dodge = -1;
        private static double Parry = -1;
        private static double Hit = -1;
        private static double CriticalStrike = -1;
        private static double SpellPower = -1;
        private static double SpellHit = -1;
        private static double SpellCrit = -1;
        private static double MP5 = -1;
        private static double SpellHaste = -1;

        private static double SpellPenetration = -1;
        private static double RangedAttackSpeed = -1;
        private static double RangedCrit = -1;
        private static double AttackPowerFeral = -1;
        private static double Expertice = -1;
        private static double ArmorPenetration = -1;
        private static double ShieldBlock = -1;
        private static double MinDamage = -1;
        private static double MaxDamage = -1;
        private static double Speed = -1;

        private static double HealthRegen = -1;

        private static double DamageArcane = -1;
        private static double DamageFire = -1;
        private static double DamageFrost = -1;
        private static double DamageHoly = -1;
        private static double DamageNature = -1;
        private static double DamageShadow = -1;

        private static double ResistArcane = -1;
        private static double ResistFire = -1;
        private static double ResistFrost = -1;
        private static double ResistHoly = -1;
        private static double ResistNature = -1;
        private static double ResistShadow = -1;

        public static void init(bool iEquipBOE, bool iArmorUpgrade, string iMaxGearQuality, List<string> iProtectedSlots,
            List<string> iWeaponSkills, double iAgility, double iStrength, double iIntellect, double iSpirit,
            double iStamina, double iArmor, double iBlock, double iDPS, double iAttackPower, double iRangedAttackPower,
            double iDefense, double iResilience, double iDodge, double iParry, double iHit, double iCriticalStrike,
            double iSpellPower, double iSpellHit, double iSpellCrit, double iMP5, double iSpellHaste, 
            bool iEquipArmor, bool iEquipWeapons, bool iUseShield, bool iPreferShield, double iSpellPenetration, 
            double iRangedAttackSpeed, double iRangedCrit, double iAttackPowerFeral, double iExpertice, double iArmorPenetration,
            double iShieldBlock, double iMinDamage, double iMaxDamage, double iSpeed, double iHealthRegen, double iDamageArcane,
            double iDamageFire, double iDamageFrost, double iDamageHoly, double iDamageNature, double iDamageShadow, double iResistArcane,
            double iResistFire, double iResistFrost, double iResistHoly, double iResistNature, double iResistShadow)
        {
            EquipBOE = iEquipBOE;
            EquipArmor = iEquipArmor;
            EquipWeapons = iEquipWeapons;
            ArmorUpgrade = iArmorUpgrade;
            MaxGearQuality = iMaxGearQuality;
            ProtectedSlots = iProtectedSlots;
            WeaponSkills = iWeaponSkills;
            Agility = iAgility;
            Strength = iStrength;
            Intellect = iIntellect;
            Spirit = iSpirit;
            Stamina = iStamina;
            Armor = iArmor;
            Block = iBlock;
            DPS = iDPS;
            AttackPower = iAttackPower;
            RangedAttackPower = iRangedAttackPower;
            Defense = iDefense;
            Resilience = iResilience;
            Dodge = iDodge;
            Parry = iParry;
            Hit = iHit;
            CriticalStrike = iCriticalStrike;
            SpellPower = iSpellPower;
            SpellHit = iSpellHit;
            SpellCrit = iSpellCrit;
            MP5 = iMP5;
            SpellHaste = iSpellHaste;
            UseShield = iUseShield;
            PreferShield = iPreferShield;
            SpellPenetration = iSpellPenetration;
            RangedAttackSpeed = iRangedAttackSpeed;
            RangedCrit = iRangedCrit;
            AttackPowerFeral = iAttackPowerFeral;
            Expertice = iExpertice;
            ArmorPenetration = iArmorPenetration;
            ShieldBlock = iShieldBlock;
            MinDamage = iMinDamage;
            MaxDamage = iMaxDamage;
            Speed = iSpeed;
            HealthRegen = iHealthRegen;
            DamageArcane = iDamageArcane;
            DamageFire = iDamageFire;
            DamageFrost = iDamageFrost;
            DamageHoly = iDamageHoly;
            DamageNature = iDamageNature;
            DamageShadow = iDamageShadow;
            ResistArcane = iResistArcane;
            ResistFire = iResistFire;
            ResistFrost = iResistFrost;
            ResistHoly = iResistHoly;
            ResistNature = iResistNature;
            ResistShadow = iResistShadow;

            initialized = true;
        }

        public static bool CompareGear(EasyItem E, string item)
        {
            eq_item_score = 0;
            new_item_score = 0;
            if (!initialized) return false;
            if (E == null && E.Item == null) return false;
            if (IsEquippable(E))
            {
                if(IsWanted(E))
                {
                    if(!IsProtected(E))
                    {
                        if(IsUsable(E))
                        {
                            PPather.Debug("CompareGear: checking {0} (Type={1}, SubType={2}, Slot={3})", E.RealName, E.Item.Type, E.Item.SubType, E.Item.Slot);
                            return Compare(E, item); 
                        }
                    }
                }
            }
            return false;
        }

        public static bool IsEquippable(EasyItem E)
        {
            if (E.Item.Type.Equals("Armor") || E.Item.Type.Equals("Weapons") && E.Item.Slot != null && !E.Item.Slot.Equals('0'))
                return true;
            else
            {
                PPather.WriteLine(String.Format("ItemCompare: Type [{0}] is not equippable, skipping {1}", E.Item.Type, E.RealName));
                GContext.Main.Debug(String.Format("ItemCompare: Type [{0}] is not equippable, skipping {1}", E.Item.Type, E.RealName));
                return false;
            }
        }

        public static bool IsWanted(EasyItem E)
        {
            if ((EquipArmor && E.Item.Type.Equals("Armor")) || (EquipWeapons && E.Item.Type.Equals("Weapons")))
                return true;
            else
            {
                PPather.WriteLine(String.Format("ItemCompare: Type [{0}] is not wanted, skipping {1}", E.Item.Type, E.RealName));
                GContext.Main.Debug(String.Format("ItemCompare: Type [{0}] is not wanted, skipping {1}", E.Item.Type, E.RealName));
                return false;
            }
        }

        public static bool IsProtected(EasyItem E)
        {

            string failsafe1 = E.Item.Slot;
            string failsafe2 = E.Item.Slot;
            if(failsafe1 == null)
                failsafe1 = "";
            if (failsafe2 == null)
                failsafe2 = "";
            try
            {
                failsafe1.Replace("slot", "");
            }
            catch { }

            try
            {
                failsafe2.Replace("Slot", "");
            }
            catch { }

            if (ProtectedSlots.Contains(E.Item.Slot) || ProtectedSlots.Contains(failsafe1) || ProtectedSlots.Contains(failsafe2))
            {
                /*
                 * protected by slot
                 */
                PPather.WriteLine(String.Format("ItemCompare: Slot [{0}] is protected, skipping {1}", E.Item.Slot, E.RealName));
                GContext.Main.Debug(String.Format("ItemCompare: Slot [{0}] is protected, skipping {1}", E.Item.Slot, E.RealName));
                return true;
            }
            else
            {
                /*
                 * protected by quality
                 */
                if (!initialized) return true;
                if (E == null) return false;
                if (E.GItem == null) return false; // wtf?
                int max = Quality(MaxGearQuality);
                int iq = 10;
                switch (E.GItem.Definition.Quality)
                {
                    case GItemQuality.Unknown: iq = 10; break;
                    case GItemQuality.Poor: iq = Quality("poor"); break;
                    case GItemQuality.Common: iq = Quality("common"); break;
                    case GItemQuality.Uncommon: iq = Quality("uncommon"); break;
                    case GItemQuality.Rare: iq = Quality("rare"); break;
                    case GItemQuality.Epic: iq = Quality("epic"); break;   
                    case GItemQuality.Legendary: iq = Quality("legendary"); break;
                    case GItemQuality.Artifact: iq = Quality("artifact"); break;
                    default: iq = 10; break;
                }


                if (iq > max)
                {
                    PPather.WriteLine(String.Format("ItemCompare: Quality [{0}] is protected, skipping {1}", E.GItem.Definition.Quality.ToString(), E.RealName));
                    GContext.Main.Debug(String.Format("ItemCompare: Quality [{0}] is protected, skipping {1}", E.GItem.Definition.Quality.ToString(), E.RealName));
                    return true;
                }
            }
            return false;
        }

        public static int Quality(string quality)
        {
            switch (quality.ToLower())
            {
                case "poor": return 0;
                case "grey": return 0;
                case "common": return 1;
                case "white": return 1;
                case "uncommon": return 2;
                case "green": return 2;
                case "rare": return 3;
                case "blue": return 3;
                case "epic": return 4;
                case "purple": return 4;
                case "legendary": return 5;
                case "orange": return 5;
                case "artifact": return 6;
                case "red": return 6;
                case "gold": return 6;
                case "beige": return 6;
                default: return -1;
            }
        }

        public static bool IsUsable(EasyItem E)
        {
            bool usable = false;
            int PlayerLevel = GPlayerSelf.Me.Level;
            if (E.Item.Type.Equals("Armor"))
            {
                if (E.Item.SubType.Equals("Shields") && UseShield) usable = true;
                else if (E.Item.SubType.Equals("Cloaks")) usable = true;
                else
                {
                    switch (GPlayerSelf.Me.PlayerClass.ToString())
                    {
                        case "Warrior":
                            if (ArmorUpgrade && PlayerLevel > 40 && E.Item.SubType.Equals("Plate Armor")) usable = true;
                            else if (E.Item.SubType.Equals("Plate Armor")) usable = false;
                            else usable = true;
                            break;
                        case "Paladin":
                            if (ArmorUpgrade && PlayerLevel > 40 && E.Item.SubType.Equals("Plate Armor")) usable = true;
                            else if (E.Item.SubType.Equals("Plate Armor")) usable = false;
                            else usable = true;
                            break;
                        case "Shaman":
                            if (ArmorUpgrade && PlayerLevel > 40 && E.Item.SubType.Equals("Mail Armor")) usable = true;
                            else if (E.Item.SubType.Equals("Plate Armor")) usable = false;
                            else if (E.Item.SubType.Equals("Mail Armor")) usable = false;
                            else usable = true;
                            break;
                        case "Hunter":
                            if (ArmorUpgrade && PlayerLevel > 40 && E.Item.SubType.Equals("Mail Armor")) usable = true;
                            else if (E.Item.SubType.Equals("Plate Armor")) usable = false;
                            else if (E.Item.SubType.Equals("Mail Armor")) usable = false;
                            else usable = true;
                            break;
                        case "Rogue":
                            if (E.Item.SubType.Equals("Plate Armor")) usable = false;
                            else if (E.Item.SubType.Equals("Mail Armor")) usable = false;
                            else usable = true;
                            break;
                        case "Druid":
                            if (E.Item.SubType.Equals("Plate Armor")) usable = false;
                            else if (E.Item.SubType.Equals("Mail Armor")) usable = false;
                            else usable = true;
                            break;
                        default: // Priest, Mage and Warlock
                            if (E.Item.SubType.Equals("Plate Armor")) usable = false;
                            else if (E.Item.SubType.Equals("Mail Armor")) usable = false;
                            else if (E.Item.SubType.Equals("Leather Armor")) usable = false;
                            else usable = true;
                            break;
                    }
                }
            }
            else if (E.Item.Type.Equals("Weapons"))
            {
                if (WeaponSkills.Contains(E.Item.SubType)) usable = true;
                else usable = false;
            }
            else
            {
                usable = false;
            }
            if (!usable)
            {
                PPather.WriteLine(String.Format("ItemCompare: SubType [{0}] is not usable, skipping {1}", E.Item.SubType, E.RealName));
                GContext.Main.Debug(String.Format("ItemCompare: SubType [{0}] is not usable, skipping {1}", E.Item.SubType, E.RealName));
            }
            return usable;
        
        }

        public static double ItemValue(Item i, double BaseScore, EquipModifier EQ, bool IncludeArmor)
        {
            
            if (!initialized) return 0;
            //PPather.Debug("ItemValue: \n{0}", i.ToString());
            double score = BaseScore;

            if (IncludeArmor) score += EQ.Armor * i.Armor;
            if (Strength != -1) score += Strength * i.Strength; else score += EQ.Strength * i.Strength;
            if (Intellect != -1) score += Intellect * i.Intellect; else score += EQ.Intellect * i.Intellect;
            if (Spirit != -1) score += Spirit * i.Spirit; else score += EQ.Spirit * i.Spirit;
            if (Stamina != -1) score += Stamina * i.Stamina; else score += EQ.Stamina * i.Stamina;
            if (AttackPower != -1) score += AttackPower * i.AttackPower; else score += EQ.AttackPower * i.AttackPower;
            if (RangedAttackPower != -1) score += RangedAttackPower * i.RangedAttackPower; else score += EQ.RangedAttackPower * i.RangedAttackPower;
            if (SpellPower != -1) score += SpellPower * i.SpellPower; else score += EQ.SpellPower * i.SpellPower;
            if (Defense != -1) score += Defense * i.Defense; else score += EQ.Defense * i.Defense;
            if (Resilience != -1) score += Resilience * i.Resilience; else score += EQ.Resilience * i.Resilience;
            if (Parry != -1) score += Parry * i.Parry; else score += EQ.Parry * i.Parry;
            if (Hit != -1) score += Hit * i.Hit; else score += EQ.Hit * i.Hit;
            if (SpellHaste != -1) score += SpellHaste * i.SpellHaste; else score += EQ.SpellHit * i.SpellHaste;
            if (CriticalStrike != -1) score += CriticalStrike * i.CriticalStrike; else score += EQ.Crit * i.CriticalStrike;
            if (SpellCrit != -1) score += SpellCrit * i.SpellCrit; else score += EQ.SpellCrit * i.SpellCrit;
            if (MP5 != -1) score += MP5 * i.ManaRegen; else score += EQ.MP5 * i.ManaRegen;
            if (Block != -1) score += Block * i.Block; else score += EQ.Block * i.Block;
            if (Dodge != -1) score += Dodge * i.Dodge; else score += EQ.Dodge * i.Dodge;
            if (DamageShadow != -1) score += DamageShadow * i.DamageShadow; else score += EQ.DamageShadow * i.DamageShadow;

            if (SpellPenetration != -1) score += SpellPenetration * i.SpellPenetration;
            if (RangedAttackSpeed != -1) score += RangedAttackSpeed * i.RangedAttackSpeed;
            if (RangedCrit != -1) score += RangedCrit * i.RangedCrit;
            if (AttackPowerFeral != -1) score += AttackPowerFeral * i.AttackPowerFeral;
            if (Expertice != -1) score += Expertice * i.Expertise;
            if (ArmorPenetration != -1) score += ArmorPenetration * i.ArmorPenetration;
            if (ShieldBlock != -1) score += ShieldBlock * i.ShieldBlock;
            if (MinDamage != -1) score += MinDamage * i.MinDamage;
            if (MaxDamage != -1) score += MaxDamage * i.MaxDamage;
            if (Speed != -1) score += Speed * i.Speed;
            if (HealthRegen != -1) score += HealthRegen * i.HealthRegen;
            if (DamageArcane != -1) score += DamageArcane * i.DamageArcane;
            if (DamageFire != -1) score += DamageFire * i.DamageFire;
            if (DamageFrost != -1) score += DamageFrost * i.DamageFrost;
            if (DamageHoly != -1) score += DamageHoly * i.DamageHoly;
            if (DamageNature != -1) score += DamageNature * i.DamageNature;
            if (DamageShadow != -1) score += DamageShadow * i.DamageShadow;
            if (ResistArcane != -1) score += ResistArcane * i.ResistArcane;
            if (ResistFire != -1) score += ResistFire * i.ResistFire;
            if (ResistFrost != -1) score += ResistFrost * i.ResistFrost;
            if (ResistHoly != -1) score += ResistHoly * i.ResistHoly;
            if (ResistNature != -1) score += ResistNature * i.ResistNature;
            if (ResistShadow != -1) score += ResistShadow * i.ResistShadow;
            return score;
        }

        public static double DPSValue(Item i, Item e)
        {
            if (!initialized) return 0;
            double DPSDifference = i.DPS - e.DPS;
            if (DPSDifference < -2) return 0;
            if (e.Speed < 1.8 && i.Speed > 2)
                if (DPSDifference < e.DPS * 0.12)
                    return 0;
            if (e.Speed >= 2.4)
                return (i.MinDamage + i.MaxDamage) / 2 / e.Speed;
            return e.DPS;
        }

        public static bool Compare(EasyItem E, string item)
        {
            if (!initialized) return false;
            double itemValue = 0;
            double equipValue = 0;
            bool IncludeArmor = false;
            EasyItem MainHand = null;
            EasyItem SecondHand = null;
            string nameMain = "none";
            string nameSecond = "none";

            EquipModifier EQ = new EquipModifier(GPlayerSelf.Me.PlayerClass.ToString(), ArmorUpgrade);

            // avoid some rare armor quest objects that sneaked through the tests earlier
            if (E.Item.Slot == null || E.Item.Slot.Equals("0"))
            {
                PPather.WriteLine(Pather.LOG_CATEGORY.INFORMATION,"Compare: Item slot is '0', {0} is most likely a quest armor", E.RealName);
                return false;
            }

            /* check slot for equipped item */
            if (!E.Item.SubType.Equals("Rings") && ((E.Item.Type.Equals("Armor") || E.Item.SubType.Equals("Ranged")) && Character.GetCurrent(E.Item.Slot) == null)
                || (E.Item.Type.Equals("Weapons") && 
                ((Character.GetCurrent(Character.Slots.MainHand) == null) && (Character.GetCurrent(Character.Slots.SecondaryHand) == null))
                || (E.Item.Slot.Equals(Character.Slots.Ranged) && Character.GetCurrent(Character.Slots.Ranged) == null)))
            {
                PPather.WriteLine(LOG_CATEGORY.INFORMATION,"Compare: No item found in {0}, we should equip this", E.Item.Slot);
                return Inventory.Equip(E, EquipBOE);
            }

            /* get equipped item */
            EasyItem equipped = null;
            Character.CurrentlyEquipped.TryGetValue(E.Item.Slot, out equipped);

            if (equipped == null || equipped.Item == null)
            {
                /* if we didn't hit on the tests for no item above
                 * and equipped is read as null
                 * retun false as a failsafe, something is wrong
                 */
                return false;
            }
            itemValue = ItemValue(E.Item, 0, EQ, false);

            /*
             * Compare against both slots if it's a Ring or Trinket
             */
            if (E.Item.Slot.Equals("Finger") || E.Item.Slot.Equals("Trinket"))
            {
                EasyItem Slot0 = null;
                if (Character.GetCurrent(Character.Slots.get(E.Item.Slot + "0")) != null)
                    Character.CurrentlyEquipped.TryGetValue(Character.Slots.get(E.Item.Slot + "0"), out Slot0);
                EasyItem Slot1 = null;
                if (Character.GetCurrent(Character.Slots.get(E.Item.Slot + "1")) != null)
                    Character.CurrentlyEquipped.TryGetValue(Character.Slots.get(E.Item.Slot + "1"), out Slot1);

                if (Slot0 == null) return Inventory.Equip(E, EquipBOE, Character.Slots.get(E.Item.Slot + "0"));
                if (Slot1 == null) return Inventory.Equip(E, EquipBOE, Character.Slots.get(E.Item.Slot + "1"));

                double slot0Value = 0;
                double slot1Value = 0;

                if (Slot0 != null)
                    slot0Value = ItemValue(Slot0.Item, 0, EQ, false);
                if (Slot1 != null)
                    slot1Value = ItemValue(Slot1.Item, 0, EQ, false);


                PPather.WriteLine(LOG_CATEGORY.DEBUG,"Compare: Ring or Trinket: [eq0] {0}({1}), [eq1] {2}({3}), [new] {4}({5})",Slot0.RealName, slot0Value, Slot1.RealName, slot1Value, E.RealName, itemValue);

                if (itemValue < slot0Value && itemValue < slot1Value)
                {
                    PPather.WriteLine(LOG_CATEGORY.INFORMATION,"Compare: Ring or Trinket: SCORE [new] {0}({1}) WORSE than both [equipped] {2}({3}) and [equippped] {4}({5}) => DISREGARD", E.RealName, itemValue, Slot0.RealName, slot0Value, Slot1.RealName, slot1Value);
                    return false;
                }
                else
                {
                    if (slot0Value < slot1Value)
                    {
                        PPather.WriteLine(LOG_CATEGORY.INFORMATION, "Compare: Ring or Trinket: SCORE [new] {0}({1}) BETTER than [equipped] {2}({3}) => EQUIP", E.RealName, itemValue, Slot0.RealName, slot0Value);
                        return Inventory.Equip(E, EquipBOE, Character.Slots.get(E.Item.Slot + "0"));
                    }
                    else
                    {
                        PPather.WriteLine(LOG_CATEGORY.INFORMATION, "Compare: Ring or Trinket: SCORE [new] {0}({1}) BETTER than [equipped] {2}({3}) => EQUIP", E.RealName, itemValue, Slot1.RealName, slot1Value);
                        return Inventory.Equip(E, EquipBOE, Character.Slots.get(E.Item.Slot + "1"));
                    }
                }
            }
            else if (E.Item.Type.Equals("Weapons") || E.Item.SubType.Equals("Shields"))
            {
                /* 
                 * Weapon comparsion logic
                 * If (Shield && MainHand != Two-Hand Weapon || PreferSheidl)
                 *   Compare against SecondaryHand
                 *   
                 * If (Two-Hand Weapon || MainHand==TwoHanded && (Item == OneHand || Item == SecondaryHand))
                 *   Compare against sum of MainHand and SecondaryHand
                 *
                 * If (Dual-Wield Skill)
                 *   If(OneHand Weapon)
                 *     Compare against both MainHand and SecondaryHand
                 *   If(OffHand Weapon)
                 *     Compare against SecondaryHand
                 * 
                 * If (OneHand Weapon)
                 *   Compare against MainHand slot (not OneHand slot)
                 *
                 * Else
                 *   Compare against MainHand
                 */


                /*
                 * Get Items currently equipped in MainHand and SecondaryHand slot
                 */
                if (Character.GetCurrent(Character.Slots.MainHand) != null)
                {
                    Character.CurrentlyEquipped.TryGetValue(Character.Slots.MainHand, out MainHand);
                    if (MainHand != null && MainHand.Item != null)
                        nameMain = MainHand.RealName;
                }
                if (Character.GetCurrent(Character.Slots.SecondaryHand) != null)
                {
                    Character.CurrentlyEquipped.TryGetValue(Character.Slots.SecondaryHand, out SecondHand);
                    if (SecondHand != null && SecondHand.Item != null)
                        nameSecond = SecondHand.RealName;
                }

                itemValue = ItemValue(E.Item, E.Item.DPS * EQ.DPS, EQ, false);
                equipValue = 0;


                /* If a shield and we don't have a two-handed weapon equipped,
                 * compare against SecondaryHandSlot */
                if (E.Item.SubType.Equals("Shields") && (!IsTwoHanded(MainHand.Item.SubType) || PreferShield))
                {
                    equipValue = ItemValue(SecondHand.Item, SecondHand.Item.DPS * EQ.DPS, EQ, IncludeArmor);
                    if (itemValue > equipValue)
                    {
                        PPather.WriteLine(LOG_CATEGORY.INFORMATION, "Compare: Shield: SCORE [new] {0}({1}) BETTER than [equipped] {2}({3}) AND MainHand is not Two-Handed Weapon OR PreferShield => EQUIP", E.RealName, itemValue, SecondHand.RealName, equipValue);
                        return Inventory.Equip(E, EquipBOE);
                    }
                    else
                    {
                        PPather.WriteLine(LOG_CATEGORY.INFORMATION, "Compare: Shield: SCORE [new] {0}({1}) WORSE than [equipped] {2}({3}) OR MainHand is Two-Handed Weapon => DISREGARD", E.Item.Slot, E.RealName, itemValue, equipped.RealName, equipValue);
                        return false;
                    }
                }
                /* If Two-Handed Weapon */
                else if (IsTwoHanded(E.Item.SubType) || (IsTwoHanded(MainHand.Item.SubType) && (E.Item.Slot.Equals(Character.Slots.OneHand) || E.Item.Slot.Equals(Character.Slots.SecondaryHand))))
                {
                    equipValue = 0;
                    if(MainHand != null)
                        equipValue += ItemValue(MainHand.Item, MainHand.Item.DPS * EQ.DPS, EQ, IncludeArmor);
                    if (SecondHand != null)
                        equipValue += ItemValue(SecondHand.Item, SecondHand.Item.DPS * EQ.DPS, EQ, IncludeArmor);
                    if (itemValue > equipValue)
                    {
                        PPather.WriteLine(LOG_CATEGORY.INFORMATION, "Compare: Two-Handed: SCORE [new] {0}({1}) BETTER than SUM OF [equipped] {2} and [equipped] {3} ({4}) => EQUIP", E.RealName, itemValue, nameMain, nameSecond, equipValue);
                        return Inventory.Equip(E, EquipBOE);
                    }
                    else
                    {
                        PPather.WriteLine(LOG_CATEGORY.INFORMATION, "Compare: Two-Handed: SCORE [new] {0}({1}) WORSE than SUM OF [equipped] {2} and [equipped] {3} (SUM: {4}) => DISREGARD", E.RealName, itemValue, nameMain, nameSecond, equipValue);
                        return false;
                    }
                }
                /* If Dual-Wield skill*/
                else if (WeaponSkills.Contains("Dual-Wield") && E.Item.Slot.Equals(Character.Slots.OneHand))
                {
                    double mainValue = 0;
                    double secondValue = 0;
                    if (MainHand != null)
                        mainValue = ItemValue(MainHand.Item, MainHand.Item.DPS * EQ.DPS, EQ, IncludeArmor);
                    if (SecondHand != null)
                        secondValue = ItemValue(SecondHand.Item, SecondHand.Item.DPS * EQ.DPS, EQ, IncludeArmor);

                    PPather.WriteLine(LOG_CATEGORY.DEBUG, "Compare: Dual-Wield: [eq0] {0}({1}), [eq1] {2}({3}), [new] {4}({5})", nameMain, mainValue, nameSecond, secondValue, E.RealName, itemValue);

                    if (itemValue < mainValue && itemValue < secondValue)
                    {
                        PPather.WriteLine(LOG_CATEGORY.INFORMATION, "Compare: Dual-Wield: SCORE [new] {0}({1}) WORSE than both [equipped] {2}({3}) and [equippped] {4}({5}) => DISREGARD", E.RealName, itemValue, nameMain, mainValue, nameSecond, secondValue);
                        return false;
                    }
                    else
                    {
                        if (mainValue < secondValue)
                        {
                            PPather.WriteLine(LOG_CATEGORY.INFORMATION, "Compare: Dual-Wield: SCORE [new] {0}({1}) BETTER than [equipped] {2}({3}) => EQUIP", E.RealName, itemValue, nameMain, mainValue);
                            return Inventory.Equip(E, EquipBOE, Character.Slots.MainHand);
                        }
                        else
                        {
                            PPather.WriteLine(LOG_CATEGORY.INFORMATION, "Compare: Dual-Wield: SCORE [new] {0}({1}) BETTER than [equipped] {2}({3}) => EQUIP", E.RealName, itemValue, nameSecond, secondValue);
                            return Inventory.Equip(E, EquipBOE, Character.Slots.SecondaryHand);
                        }
                    }
                }
                else if (E.Item.Slot.Equals(Character.Slots.OneHand))
                {
                    double mainValue = 0;
                    if (MainHand != null)
                        mainValue = ItemValue(MainHand.Item, MainHand.Item.DPS * EQ.DPS, EQ, IncludeArmor);

                    if (itemValue > mainValue)
                    {
                        PPather.WriteLine(LOG_CATEGORY.INFORMATION, "Compare: Weapons ({0}): SCORE [new] {0}({1}) BETTER than [equipped] {2}({3}) => EQUIP", E.RealName, itemValue, nameMain, mainValue);
                        return Inventory.Equip(E, EquipBOE, Character.Slots.MainHand);
                    }
                    else
                    {
                        PPather.WriteLine(LOG_CATEGORY.INFORMATION, "Compare: Weapons ({0}): SCORE [new] {0}({1}) WORSE than [equipped] {2}({3}) => EQUIP", E.RealName, itemValue, nameMain, mainValue);
                        return false;
                    }

                }
                else
                {
                    equipValue = ItemValue(equipped.Item, 0, EQ, false);
                    if (itemValue > equipValue)
                    {
                        PPather.WriteLine(LOG_CATEGORY.INFORMATION, "Compare: Weapons ({0}): SCORE [new] {1}({2}) BETTER than [equipped] {3}({4}) => EQUIP", E.Item.Slot, E.RealName, itemValue, equipped.RealName, equipValue);
                        return Inventory.Equip(E, EquipBOE);
                    }
                    else
                    {
                        PPather.WriteLine(LOG_CATEGORY.INFORMATION, "Compare: Weapons ({0}): SCORE [new] {1}({2}) WORSE than [equipped] {3}({4}) => DISREGARD", E.Item.Slot, E.RealName, itemValue, equipped.RealName, equipValue);
                        return false;
                    }
                }
            }
            else if (E.Item.Type.Equals("Armor"))
            {
                if (E.Item.Slot.Equals(Character.Slots.SecondaryHand) && IsTwoHanded(MainHand.Item.SubType))
                {
                    PPather.WriteLine(LOG_CATEGORY.INFORMATION, " Compare: Can't Equip {0} with Two-Handed Weapons ({1}) ==> DISREGARD", E.RealName, nameMain);
                    return false;
                }
                /* include armor value if any of the item are of preferred type */
                if (equipped.Item.SubType.Replace(" Armor", "").Equals(EQ.WantedArmor) ||
                    E.Item.SubType.Replace(" Armor", "").Equals(EQ.WantedArmor))
                    IncludeArmor = true;

                /* include armor if both of the items are of wrong type */
                else if (!equipped.Item.SubType.Replace(" Armor", "").Equals(EQ.WantedArmor) &&
                    !E.Item.SubType.Replace(" Armor", "").Equals(EQ.WantedArmor))
                    IncludeArmor = true;

                else if (E.Item.Slot.Equals(Character.Slots.Back))
                    IncludeArmor = true;

                else
                    IncludeArmor = false;

                itemValue = ItemValue(E.Item, 0, EQ, IncludeArmor);
                equipValue = ItemValue(equipped.Item, 0, EQ, IncludeArmor);


                eq_item_score = equipValue;
                new_item_score = itemValue;

                if (itemValue > equipValue)
                {
                    PPather.WriteLine(LOG_CATEGORY.INFORMATION, "Compare: Armor: SCORE [new] {0}({1}) BETTER than [equipped] {2}({3}) => EQUIP", E.RealName, itemValue, equipped.RealName, equipValue);
                    return Inventory.Equip(E, EquipBOE);
                }
                else
                {
                    PPather.WriteLine(LOG_CATEGORY.INFORMATION, "Compare: {0}: SCORE [new] {1}({2}) WORSE than [equipped] {3}({4}) => DISREGARD", E.Item.Slot, E.RealName, itemValue, equipped.RealName, equipValue);
                    return false;
                }
            }
            else
            {
                PPather.WriteLine(LOG_CATEGORY.INFORMATION, "WTF?");
                return false;
            }
        }

        public static bool IsTwoHanded(string WeaponType)
        {
            switch (WeaponType)
            {
                case "Polearms": return true;
                case "Staves": return true;
                case "Two-Handed Axe": return true;
                case "Two-Handed Maces": return true;
                case "Two-Handed Swords": return true;
                default: return false;
            }
        }
    }
}
