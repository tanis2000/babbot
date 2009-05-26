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
using System.Data;
using System.Linq;
using System.Text;
using Pather.Helpers;
using Glider.Common.Objects;
using System.Text.RegularExpressions;
using Pather.Helpers.UI;
using System.Threading;

/*
 * Contributor(s): toblakai, swolbyn
 */ 
namespace Pather.Helpers
{
    public static class ItemManager
    {
        private static Dictionary<string, Item> ItemCache = new Dictionary<string, Item>();
        private static Dictionary<string, int> Protected = new Dictionary<string, int>();
        private static Boolean connected = false;
        private static Database db;
        private static WowHeadFetcher wh;

        private static Object CacheLock = new Object();
        private static Object ProtectedLock = new Object();

        public static void StartUp()
        {
            PPather.WriteLine("Database.StartUp()");

            if (!connected)
            {
                wh = new WowHeadFetcher();
                db = new Database();
                db.Open();
                connected = true;
            }

            //Character.GetEquippedItems();


            PPather.WriteLine("Starting up Item Manager");
        }

        /// <summary>
        /// Shutdown and clean up.
        /// </summary>
        public static void ShutDown()
        {
            PPather.WriteLine("Database.ShutDown()");

            if (connected)
            {
                wh = null;
                db.Close();
                db = null;
                connected = false;
            }

            /// 
            /// Remove event reference.
            /// 

        }



        public static string Clean(string raw)
        {
            return raw.Replace("\"", "").Replace("'", "").Replace(" ", "");
        }

        public static Boolean IsCached(string item)
        {
            //if (!connected) init();

            // 1. check in memory cache
            if (ItemCache.ContainsKey(Clean(item))) return true;

            // 2. check in db
            DataRow data = db.GetItem(Clean(item));
            if (data != null) return true;

            return false;
        }

        public static Item get(string item)
        {
            Item i = null;

            // 1. check in memory cache
            if (ItemCache.ContainsKey(Clean(item)))
            {
                i = ItemCache[Clean(item)];
                PPather.Debug("ItemManager.get: {0} fetched from ItemCache",item);
                return i;
            }

            // 2. check in db
            DataRow data = db.GetItem(Clean(item));
            if (data != null)
            {
                i = DataRow2Item(item, data);
                PPather.Debug("ItemManager.get: {0} fetched from Database", item);
                put(i,item);
                return i;
            }
            
            // 3. fetch from WowHead
            i = GetItemFromWowHead(item);
            if (i != null)
            {
                if (i.Name == "0" || i.Name == null || i.Name == "") i.Name = item;
                PPather.Debug("ItemManager.get: {0} fetched from WoWHead", item);
                db.PutItem(i,item);
                put(i,item);
                return i;
            }
            return null;
        }

        public static Item GetItemFromWowHead(string item)
        {
            Item return_item = new Item();
            bool ReturnItemSet = false;
            int variation = 0;
            Dictionary<string, string> rd = wh.GetWowHeadItem(item);
            PPather.Debug("GetItemFromWowHead: Got " + item + " from WowHead");
            if (rd == null) return null;
            string random_enchant = "";
            rd.TryGetValue("randomenchant", out random_enchant);
            //PPather.WriteLine("ItemManager: random_enchant = " + random_enchant);
            if (random_enchant.Equals("Random Enchant"))
            {
                PPather.WriteLine(LOG_CATEGORY.DEBUG,"Getting all random enchant variants...");
                /* get bonuses for all variants of the item */
                Dictionary<string, string> tmp = new Dictionary<string, string>();
                string item_id = null;
                foreach (KeyValuePair<string, string> e in rd)
                {
                    if (e.Key.Equals("wowheadid")) item_id = e.Value;
                }
                List<string> lines = wh.GetEnchantLines(item_id);
                Regex bonusMatchPattern = new Regex("(?<=^|>)[^><]+?(?=<|$)");
                foreach (string line in lines)
                {
                    tmp.Clear();
                    PPather.Debug("GetItemFromWowHead: line = {0}", line);
                    foreach (KeyValuePair<string, string> e in rd)
                    {
                        tmp[e.Key] = new string(e.Value.ToCharArray());
                    }
                    List<string> bonuses = new List<string>();
                    bonuses.Clear();
                    string[] bonus_parts = null;
                    MatchCollection mc = bonusMatchPattern.Matches(line);
                    string subname = mc[0].Value.Replace("...", "");
                    string space = mc[1].Value;
                    string percent = mc[2].Value;
                    string bonus_string = mc[3].Value;
                    PPather.Debug("GetItemFromWowHead: subname={0}, space={1}, percent={2}, bonus_string={3}", subname, space, percent, bonus_string);
                    bonus_parts = bonus_string.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string bp in bonus_parts)
                    {
                        string[] sub_parts = null;
                        if (bp.Contains("and"))
                        {
                            sub_parts = bp.Split(new string[] { " and " }, StringSplitOptions.RemoveEmptyEntries);
                            foreach (string sp in sub_parts)
                                bonuses.Add(sp.TrimStart(new char[] { ' ' }));
                        }
                        else
                        {
                            bonuses.Add(bp.TrimStart(new char[] { ' ' }));
                        }
                    }
                    foreach (string b in bonuses)
                    {
                       PPather.Debug("GetItemFromWowHead: bonus\t=>\t{0}", b);
                    }

                    string last_val = null;

                    foreach (string bonus in bonuses)
                    {
                        string attribute = null;
                        string value = null;
                        if (bonus.Contains(')'))
                        {
                            string[] c = bonus.Split(new char[] { ')' });

                            string attr = c[1].Remove(0, 1); //Attribute
                            value = c[0].Replace(" ", "").Split(new char[] { '-' })[1]; //bonus
                            attribute = ConformAttribute(attr);
                        }
                        else
                        {
                            if (bonus.Contains("+"))
                            {
                                value = bonus.Replace("+", "").Split(new char[] { ' ' })[0];
                                last_val = value;
                            }
                            else
                            {
                                value = last_val;
                                last_val = "0";
                            }
                            string attr = bonus.Substring(bonus.IndexOf(" ") + 1, bonus.Length - bonus.IndexOf(" ") - 1);
                            attribute = ConformAttribute(attr);
                        }
                        PPather.Debug("GetItemFromWowHead: {0}[{1}] = {2}", subname, attribute, value);
                        tmp[attribute] = value;
                    }

                    string full_name = wh.GetBaseName(item) + " " + subname;
                    PPather.Debug("GetItemFromWowHead: Full Name = " + full_name);
                    Item i = WowHead2Item(tmp, full_name);
                    //PPather.WriteLine(Pather.LOG_CATEGORY.DEBUG,i.ToString());
                    if (full_name.Equals(item))
                    {
                        return_item = WowHead2Item(tmp, full_name);
                        ReturnItemSet = true;
                    }
                    else
                    {
                        db.PutItem(i,full_name);
                        put(i, full_name);
                    }
                    if (!ReturnItemSet && variation >= lines.Count)
                    {
                        return_item = WowHead2Item(tmp, full_name);
                        ReturnItemSet = true;
                    }
                    variation++;
                }
            }
            else
            {
                PPather.Debug("ItemManager: Regular item, parsing it...");
                return_item = WowHead2Item(rd, item);
            }
            //PPather.WriteLine(return_item.ToString());
            return return_item;
        }

        public static string ConformAttribute(string a)
        {
            a = a.ToLower();
            a = a.Replace(".", "");
            switch (a)
            {
                case "agility": return "agility";
                case "intellect": return "intellect";
                case "stamina": return "stamina";
                case "spirit": return "spirit";
                case "strength": return "strength";
                case "healing spells": return "healingbonus";
                case "healing": return "healingbonus";
                case "damage spells": return "spelldamage";
                case "mana every 5 sec": return "manaregen";
                case "spell critical strike rating": return "spellcrit";
                case "arcane spell damage": return "damagearcane";
                case "arcane damage": return "damagearcane";
                case "arcane resistance": return "resistarcane";
                case "fire spell damage": return "damagefire";
                case "fire damage": return "damagefire";
                case "fire resistance": return "resistfire";
                case "frost spell damage": return "damagefrost";
                case "frost damage": return "damagefrost";
                case "frost resistance": return "resistfrost";
                case "holy spell damage": return "damageholy";
                case "holy damage": return "damageholy";
                case "holy resistance": return "resistholy";
                case "nature spell damage": return "damagenature";
                case "nature damage": return "damagenature";
                case "nature resistance": return "resistnature";
                case "shadow spell damage": return "damageshadow";
                case "shadow damage": return "damageshadow";
                case "shadow resistance": return "resistshadow";
                case "defense rating": return "defense";
                case "ranged attack power": return "rangedattackpower";
                default: return a.Replace(" ","");
            }
        }

        public static void put(Item i, string item)
        {
            //if (!connected) init();
            lock (CacheLock)
            {
                // add without check. overrides any old objects
                try
                {
                    ItemCache.Add(Clean(i.Name), i);
                    PPather.Debug("ItemManager: Added " + item + " to ItemCache");
                }
                catch (ArgumentException)
                {
                    PPather.Debug("ItemManager: " + item + " already added to ItemCache (skipping)");
                }
            }
        }

        private static Item DataRow2Item(string name, DataRow data)
        {
            Item i = new Item();
            string type = db.QueryFetchCell(String.Format("SELECT type_name FROM item_types WHERE type_id = {0}", Convert.ToInt32(data["_type_id"].ToString())));
            string subtype = db.QueryFetchCell(String.Format("SELECT subtype_name FROM item_subtypes WHERE subtype_id = {0}", Convert.ToInt32(data["_subtype_id"].ToString())));
            string slot = db.QueryFetchCell(String.Format("SELECT slot_name FROM item_slots WHERE slot_id = {0}", Convert.ToInt32(data["_slot_id"].ToString())));

            i.Name = name;
            i.WowHeadId = Convert.ToInt32(data["wowhead_id"].ToString());
            i.Type = type;
            i.SubType = subtype;
            i.Slot = slot;
            i.Icon = data["icon"].ToString();
            try { i.Armor = Convert.ToInt32(data["armor"].ToString()); } catch { }
            try { i.Required = Convert.ToInt32(data["required"].ToString()); } catch { }
            i.Classes = data["classes"].ToString();
            try { i.Block = Convert.ToInt32(data["block"].ToString()); } catch { }
            try { i.MinDamage = Convert.ToInt32(data["min_damage"].ToString()); } catch { }
            try { i.MaxDamage = Convert.ToInt32(data["max_damage"].ToString()); } catch { }
            try { i.Speed = Convert.ToDouble(data["speed"].ToString()); } catch { }
            try { i.DPS = Convert.ToDouble(data["dps"].ToString()); } catch { }
            try { i.Agility = Convert.ToInt32(data["agility"].ToString()); } catch { }
            try { i.Intellect = Convert.ToInt32(data["intellect"].ToString()); } catch { }
            try { i.Stamina = Convert.ToInt32(data["stamina"].ToString()); } catch { }
            try { i.Spirit = Convert.ToInt32(data["spirit"].ToString()); } catch { }
            try { i.Strength = Convert.ToInt32(data["strength"].ToString()); } catch { }
            try { i.Health = Convert.ToInt32(data["health"].ToString()); } catch { }
            try { i.Mana = Convert.ToInt32(data["mana"].ToString()); } catch { }
            try { i.HealthRegen = Convert.ToInt32(data["hp_regen"].ToString()); } catch { }
            try { i.ManaRegen = Convert.ToInt32(data["mana_regen"].ToString()); } catch { }
            try { i.ResistArcane = Convert.ToInt32(data["resist_arcane"].ToString()); } catch { }
            try { i.ResistFire = Convert.ToInt32(data["resist_fire"].ToString()); } catch { }
            try { i.ResistFrost = Convert.ToInt32(data["resist_frost"].ToString()); } catch { }
            try { i.ResistHoly = Convert.ToInt32(data["resist_holy"].ToString()); } catch { }
            try { i.ResistNature = Convert.ToInt32(data["resist_nature"].ToString()); } catch { }
            try { i.ResistShadow = Convert.ToInt32(data["resist_shadow"].ToString()); } catch { }
            try { i.AttackPower = Convert.ToInt32(data["attack_power"].ToString()); } catch { }
            try { i.RangedAttackPower = Convert.ToInt32(data["ranged_attack_power"].ToString()); } catch { }
            try { i.RangedCrit = Convert.ToInt32(data["ranged_crit"].ToString()); } catch { }
            try { i.RangedAttackSpeed = Convert.ToInt32(data["ranged_attack_speed"].ToString()); } catch { }
            try { i.AttackPowerFeral = Convert.ToInt32(data["attack_power_feral"].ToString()); } catch { }
            try { i.Expertise = Convert.ToInt32(data["expertise"].ToString()); } catch { }
            try { i.Defense = Convert.ToInt32(data["defense"].ToString()); } catch { }
            try { i.Resilience = Convert.ToInt32(data["resilience"].ToString()); } catch { }
            try { i.ArmorPenetration = Convert.ToInt32(data["armor_penetration"].ToString()); } catch { }
            try { i.ShieldBlock = Convert.ToInt32(data["shield_block"].ToString()); } catch { }
            try { i.CriticalStrike = Convert.ToInt32(data["critical_strike"].ToString()); } catch { }
            try { i.Hit = Convert.ToInt32(data["hit"].ToString()); } catch { }
            try { i.Dodge = Convert.ToInt32(data["dodge"].ToString()); } catch { }
            try { i.Parry = Convert.ToInt32(data["parry"].ToString()); } catch { }
            try { i.SpellPower = Convert.ToInt32(data["spell_power"].ToString()); } catch { }
            try { i.SpellHit = Convert.ToInt32(data["spell_hit"].ToString()); } catch { }
            try { i.SpellCrit = Convert.ToInt32(data["spell_crit"].ToString()); } catch { }
            try { i.SpellHaste = Convert.ToInt32(data["spell_haste"].ToString()); } catch { }
            try { i.SpellPenetration = Convert.ToInt32(data["spell_penetration"].ToString()); } catch { }
            try { i.DamageArcane = Convert.ToInt32(data["damage_arcane"].ToString()); } catch { }
            try { i.DamageFire = Convert.ToInt32(data["damage_fire"].ToString()); } catch { }
            try { i.DamageFrost = Convert.ToInt32(data["damage_frost"].ToString()); } catch { }
            try { i.DamageHoly = Convert.ToInt32(data["damage_holy"].ToString()); } catch { }
            try { i.DamageNature = Convert.ToInt32(data["damage_nature"].ToString()); } catch { }
            try { i.DamageShadow = Convert.ToInt32(data["damage_shadow"].ToString()); } catch { }
            return i;
        }

        private static Item WowHead2Item(Dictionary<string, string> data, string name)
        {
            //PPather.WriteLine("WowHead2Item: Creating item of " + name);
            Item i = new Item();
            string type = null;
            string subtype = null;
            string slot = null;
            data.TryGetValue("type", out type);
            data.TryGetValue("subtype", out subtype);
            data.TryGetValue("slot", out slot);
            slot = Character.Slots.get(slot);

            PPather.Debug("WowHead2Item: TYPE='{0}' SUBTYPE='{1}' SLOT='{2}'", type, subtype, slot);

            //string type = db.QueryFetchCell(String.Format("SELECT type_name FROM item_types WHERE type_id = {0}", Convert.ToInt32(ty)));
            //string subtype = db.QueryFetchCell(String.Format("SELECT subtype_name FROM item_subtypes WHERE subtype_id = {0}", Convert.ToInt32(st)));
            //string slot = db.QueryFetchCell(String.Format("SELECT slot_name FROM item_slots WHERE slot_id = {0}", Convert.ToInt32(sl)));

            //PPather.WriteLine("WowHead2Item: DB replied with type={0}, subtype={1} and slot={2}", type, subtype, slot);

            string wowhead_id = "0";
            string icon = "0"; // http link to item icon on wowhead

            string required = "0"; string classes = "0";

            string armor = "0"; string block = "0";
            string min_damage = "0"; string max_damage = "0";
            string speed = "0"; string dps = "0";

            string agility = "0"; string intellect = "0";
            string stamina = "0"; string spirit = "0";
            string strength = "0"; string health = "0";
            string mana = "0"; string mana_regen = "0";
            string hp_regen = "0";

            // complex, they are nested within each other
            string attack_power = "0"; string ranged_attack_power = "0";
            string ranged_crit = "0"; string ranged_attack_speed = "0";
            string attack_power_feral = "0";
            string expertise = "0"; string defense = "0"; string resilience = "0";
            string armor_penetration = "0"; string shield_block = "0";
            string crit = "0"; string hit = "0";
            string dodge = "0"; string parry = "0";

            // complex, they are nested within each other but in 2 different ways
            string spell_power = "0";
            string spell_hit = "0"; string spell_crit = "0";
            string spell_haste = "0"; string spell_penetration = "0";

            // Resists omg so much
            string resist_arcane = "0"; string resist_fire = "0";
            string resist_frost = "0"; string resist_holy = "0";
            string resist_nature = "0"; string resist_shadow = "0";

            // + Damage
            string damage_arcane = "0"; string damage_fire = "0";
            string damage_frost = "0"; string damage_holy = "0";
            string damage_nature = "0"; string damage_shadow = "0";

            foreach (KeyValuePair<string, string> e in data)
            {
                //PPather.WriteLine("WowHead2Item: {0}\t=>\t{1}", e.Key, e.Value);
                switch (e.Key)
                {
                    case "wowheadid": wowhead_id = e.Value; break;
                    case "icon": icon = e.Value; break;
                    case "classes": classes = e.Value; break;
                    case "required": required = e.Value; break;
                    case "armor": armor = e.Value; break;
                    case "block": block = e.Value; break;
                    case "mindamage": min_damage = e.Value; break;
                    case "maxdamage": max_damage = e.Value; break;
                    case "speed": speed = e.Value; break;
                    case "dps": dps = e.Value; break;
                    case "agility": agility = e.Value; break;
                    case "intellect": intellect = e.Value; break;
                    case "stamina": stamina = e.Value; break;
                    case "spirit": spirit = e.Value; break;
                    case "strength": strength = e.Value; break;
                    case "health": health = e.Value; break;
                    case "mana": mana = e.Value; break;
                    case "manaregen": mana_regen = e.Value; break;
                    case "hpregen": hp_regen = e.Value; break;
                    case "attackpower": attack_power = e.Value; break;
                    case "rangedattackpower": ranged_attack_power = e.Value; break;
                    case "rangedcrit": ranged_crit = e.Value; break;
                    case "rangedattackspeed": ranged_attack_speed = e.Value; break;
                    case "attackpowerferal": attack_power_feral = e.Value; break;
                    case "expertise": expertise = e.Value; break;
                    case "defense": defense = e.Value; break;
                    case "resilience": resilience = e.Value; break;
                    case "armorpenetration": armor_penetration = e.Value; break;
                    case "shieldblock": shield_block = e.Value; break;
                    case "crit": crit = e.Value; break;
                    case "hit": hit = e.Value; break;
                    case "dodge": dodge = e.Value; break;
                    case "parry": parry = e.Value; break;
                    case "spellpower": spell_power = e.Value; break;
                    case "spellhit": spell_hit = e.Value; break;
                    case "spellcrit": spell_crit = e.Value; break;
                    case "spellhaste": spell_haste = e.Value; break;
                    case "spellpenetration": spell_penetration = e.Value; break;
                    case "resistarcane": resist_arcane = e.Value; break;
                    case "resistfire": resist_fire = e.Value; break;
                    case "resistfrost": resist_frost = e.Value; break;
                    case "resistholy": resist_holy = e.Value; break;
                    case "resistnature": resist_nature = e.Value; break;
                    case "resistshadow": resist_shadow = e.Value; break;
                    case "damagearcane": damage_arcane = e.Value; break;
                    case "damagefire": damage_fire = e.Value; break;
                    case "damagefrost": damage_frost = e.Value; break;
                    case "damageholy": damage_holy = e.Value; break;
                    case "damagenature": damage_nature = e.Value; break;
                    case "damageshadow": damage_shadow = e.Value; break;
                }
            }

            //PPather.WriteLine("WowHead2Item: About to get wowhead_id from RD");

            // data.TryGetValue("wowheadid", out wowhead_id);
            // data.TryGetValue("icon", out icon); // http link to item icon on wowhead

            // PPather.WriteLine("WowHead2Item: Set icon={0]",icon);




            // PPather.WriteLine("WowHead2Item: Set dps={0]",dps);



            // PPather.WriteLine("WowHead2Item: Set hp_regen={0]", hp_regen);

            // // complex, out  they are nested within each other

            //    // complex, out  they are nested within each other but in 2 different ways


            // PPather.WriteLine("WowHead2Item: Set spell_penetration={0]", spell_penetration);

            // // Resists omg so much


            // PPather.WriteLine("WowHead2Item: Set resist_shadow={0]", resist_shadow);

            // // + Damage


            // PPather.WriteLine("WowHead2Item: Set damage_shadwo={0]", damage_shadow);

            i.Name = name;
            i.Type = type;
            i.SubType = subtype;
            i.Slot = slot;
            i.Icon = icon;
            i.Classes = classes;

            try { i.WowHeadId = Convert.ToInt32(wowhead_id); }
            catch { }
            try { i.Required = Convert.ToInt32(required); } catch { }
            try { i.Armor = Convert.ToInt32(armor); } catch { }
            try { i.Block = Convert.ToInt32(block); } catch { }
            try { i.MinDamage = Convert.ToInt32(min_damage); }
            catch { }
            try { i.MaxDamage = Convert.ToInt32(max_damage); }
            catch { }
            try { i.Speed = Convert.ToDouble(speed); }
            catch { }
            try { i.DPS = Convert.ToDouble(dps); }
            catch { }
            try { i.Agility = Convert.ToInt32(agility); }
            catch { }
            try { i.Intellect = Convert.ToInt32(intellect); }
            catch { }
            try { i.Stamina = Convert.ToInt32(stamina); }
            catch { }
            try { i.Spirit = Convert.ToInt32(spirit); }
            catch { }
            try { i.Strength = Convert.ToInt32(strength); }
            catch { }
            try { i.Health = Convert.ToInt32(health); }
            catch { }
            try { i.Mana = Convert.ToInt32(mana); }
            catch { }
            try { i.HealthRegen = Convert.ToInt32(hp_regen); }
            catch { }
            try { i.ManaRegen = Convert.ToInt32(mana_regen); }
            catch { }
            try { i.ResistArcane = Convert.ToInt32(resist_arcane); }
            catch { }
            try { i.ResistFire = Convert.ToInt32(resist_fire); }
            catch { }
            try { i.ResistFrost = Convert.ToInt32(resist_frost); }
            catch { }
            try { i.ResistHoly = Convert.ToInt32(resist_holy); }
            catch { }
            try { i.ResistNature = Convert.ToInt32(resist_nature); }
            catch { }
            try { i.ResistShadow = Convert.ToInt32(resist_shadow); }
            catch { }
            try { i.AttackPower = Convert.ToInt32(attack_power); }
            catch { }
            try { i.RangedAttackPower = Convert.ToInt32(ranged_attack_power); }
            catch { }
            try { i.RangedCrit = Convert.ToInt32(ranged_crit); }
            catch { }
            try { i.RangedAttackSpeed = Convert.ToInt32(ranged_attack_speed); }
            catch { }
            try { i.AttackPowerFeral = Convert.ToInt32(attack_power_feral); }
            catch { }
            try { i.Expertise = Convert.ToInt32(expertise); }
            catch { }
            try { i.Defense = Convert.ToInt32(defense); }
            catch { }
            try { i.Resilience = Convert.ToInt32(resilience); }
            catch { }
            try { i.ArmorPenetration = Convert.ToInt32(armor_penetration); }
            catch { }
            try { i.ShieldBlock = Convert.ToInt32(shield_block); }
            catch { }
            try { i.CriticalStrike = Convert.ToInt32(crit); }
            catch { }
            try { i.Hit = Convert.ToInt32(hit); }
            catch { }
            try { i.Dodge = Convert.ToInt32(dodge); }
            catch { }
            try { i.Parry = Convert.ToInt32(parry); }
            catch { }
            try { i.SpellPower = Convert.ToInt32(spell_power); }
            catch { }
            try { i.SpellHit = Convert.ToInt32(spell_hit); }
            catch { }
            try { i.SpellCrit = Convert.ToInt32(spell_crit); }
            catch { }
            try { i.SpellHaste = Convert.ToInt32(spell_haste); }
            catch { }
            try { i.SpellPenetration = Convert.ToInt32(spell_penetration); }
            catch { }
            try { i.DamageArcane = Convert.ToInt32(damage_arcane); }
            catch { }
            try { i.DamageFire = Convert.ToInt32(damage_fire); }
            catch { }
            try { i.DamageFrost = Convert.ToInt32(damage_frost); }
            catch { }
            try { i.DamageHoly = Convert.ToInt32(damage_holy); }
            catch { }
            try { i.DamageNature = Convert.ToInt32(damage_nature); }
            catch { }
            try { i.DamageShadow = Convert.ToInt32(damage_shadow); }
            catch { }
            return i;
        }

        //private static Item Data2Item(string name, string data)
        //{
        //    Item i = new Item();
        //    string[] d = data.Split(new Char[] { '|' });
        //    i.Name = name;
        //    try { i.WowHeadId = Convert.ToInt32(d[0]); } catch { }
        //    i.Type = d[1];
        //    i.SubType = d[2];
        //    i.Slot = d[3];
        //    i.Icon = d[4];
        //    try { i.Required = Convert.ToInt32(d[5]); } catch { }
        //    i.Classes = d[6];
        //    try { i.Armor = Convert.ToInt32(d[7]); } catch { }
        //    try { i.Block = Convert.ToInt32(d[8]); } catch { }
        //    try { i.MinDamage = Convert.ToInt32(d[9]); } catch { }
        //    try { i.MaxDamage = Convert.ToInt32(d[10]); } catch { }
        //    try { i.Speed = Convert.ToDouble(d[11]); } catch { }
        //    try { i.DPS = Convert.ToDouble(d[12]); } catch { }
        //    try { i.Agility = Convert.ToInt32(d[13]); } catch { }
        //    try { i.Intellect = Convert.ToInt32(d[14]); } catch { }
        //    try { i.Stamina = Convert.ToInt32(d[15]); } catch { }
        //    try { i.Spirit = Convert.ToInt32(d[16]); } catch { }
        //    try { i.Strength = Convert.ToInt32(d[17]); } catch { }
        //    try { i.Health = Convert.ToInt32(d[18]); } catch { }
        //    try { i.Mana = Convert.ToInt32(d[19]); } catch { }
        //    try { i.HealthRegen = Convert.ToInt32(d[20]); } catch { }
        //    try { i.ManaRegen = Convert.ToInt32(d[21]); } catch { }
        //    try { i.ResistArcane = Convert.ToInt32(d[42]); } catch { }
        //    try { i.ResistFire = Convert.ToInt32(d[43]); } catch { }
        //    try { i.ResistFrost = Convert.ToInt32(d[44]); } catch { }
        //    try { i.ResistHoly = Convert.ToInt32(d[45]); } catch { }
        //    try { i.ResistNature = Convert.ToInt32(d[46]); } catch { }
        //    try { i.ResistShadow = Convert.ToInt32(d[47]); } catch { }
        //    try { i.AttackPower = Convert.ToInt32(d[22]); } catch { }
        //    try { i.RangedAttackPower = Convert.ToInt32(d[23]); } catch { }
        //    try { i.RangedCrit = Convert.ToInt32(d[24]); } catch { }
        //    try { i.RangedAttackSpeed = Convert.ToInt32(d[25]); } catch { }
        //    try { i.AttackPowerFeral = Convert.ToInt32(d[26]); } catch { }
        //    try { i.Expertise = Convert.ToInt32(d[27]); } catch { }
        //    try { i.Defense = Convert.ToInt32(d[28]); } catch { }
        //    try { i.Resilience = Convert.ToInt32(d[29]); } catch { }
        //    try { i.ArmorPenetration = Convert.ToInt32(d[30]); } catch { }
        //    try { i.ShieldBlock = Convert.ToInt32(d[31]); } catch { }
        //    try { i.CriticalStrike = Convert.ToInt32(d[32]); } catch { }
        //    try { i.Hit = Convert.ToInt32(d[33]); } catch { }
        //    try { i.Dodge = Convert.ToInt32(d[34]); } catch { }
        //    try { i.Parry = Convert.ToInt32(d[35]); } catch { }
        //    try { i.SpellDamage = Convert.ToInt32(d[36]); } catch { }
        //    try { i.HealingBonus = Convert.ToInt32(d[37]); } catch { }
        //    try { i.SpellHit = Convert.ToInt32(d[38]); } catch { }
        //    try { i.SpellCrit = Convert.ToInt32(d[39]); } catch { }
        //    try { i.SpellHaste = Convert.ToInt32(d[40]); } catch { }
        //    try { i.SpellPenetration = Convert.ToInt32(d[41]); } catch { }
        //    try { i.DamageArcane = Convert.ToInt32(d[48]); } catch { }
        //    try { i.DamageFire = Convert.ToInt32(d[49]); } catch { }
        //    try { i.DamageFrost = Convert.ToInt32(d[50]); } catch { }
        //    try { i.DamageHoly = Convert.ToInt32(d[51]); } catch { }
        //    try { i.DamageNature = Convert.ToInt32(d[52]); } catch { }
        //    try { i.DamageShadow = Convert.ToInt32(d[53]); } catch { }
        //    return i;
        //}

        /// <summary>
        /// Put a protection on this item, by increasing the protection by 1. As long
        /// as the protection on an item is higher then 0, the item is protected, and
        /// will stay protected until release() has been called for the item enough 
        /// times that protection has decreased to 0 again.
        /// </summary>
        /// <param name="item"></param>
        static public void protect(string item)
        {
            lock (ProtectedLock)
            {
                if (Protected.ContainsKey(item))
                    Protected[item]++;
                else
                    Protected[item] = 1;
            }
        }

        /// <summary>
        /// Decrease protection for this item by 1, until minium protection of 0 is reached.
        /// </summary>
        /// <param name="item"></param>
        static public void release(string item)
        {
            lock (ProtectedLock)
            {
                if (Protected.ContainsKey(item))
                    if (Protected[item] > 0)
                        Protected[item]--;
                    else
                        Protected[item] = 0;
            }
        }

        /// <summary>
        /// Returns true if one or more entities have called protect() on this item
        /// (such that it's protection is higher than 0). Return false only when
        /// protection is 0 on this item.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        static public Boolean IsProtected(string item)
        {
            if (Protected.ContainsKey(item))
                if (Protected[item] > 0)
                    return true;
            return false;
        }
    }
}