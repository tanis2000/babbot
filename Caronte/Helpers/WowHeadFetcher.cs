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
using System.Text.RegularExpressions;
using System.Xml;
using System.Net;
using System.IO;
using System.Web;

/*
 * Contributor(s): swolbyn
 */ 
namespace Pather.Helpers
{
    class WowHeadFetcher
    {
        Dictionary<string, string> types = new Dictionary<string, string>();

        public WowHeadFetcher()
        {
            types.Add("items", "?item=");
            types.Add("itemsets", "?itemsets=");
            types.Add("npcs", "?npcs=");
            types.Add("objects", "?objects=");
            types.Add("quests", "?quests=");
            types.Add("spells", "?spells=");
            types.Add("zones", "?zones=");
            types.Add("pets", "?pets=");
        }

        public string GetBaseName(string name)
        {
            if (name.Contains(" of"))
                return name = name.Substring(0, name.IndexOf(" of")).TrimEnd();
            else
                return name;
        }

        /// <summary>
        /// Sends a request to WowHead for the selected data type. Returns parsed
        /// data in a string delimited by '|' for each property.
        /// </summary>
        /// <param name="name">The name of the object to be queried</param>
        /// <param name="type">The type of the object (item, npc etc.)</param>
        /// <returns>Returns the parsed data in a string delimited by '|'</returns>
        public Dictionary<string,string> GetWowHeadItem(string name)
        {
            string output = GetWebResponse(name);
            if (output == null) return null;

            if(output.Equals("<?xml version=\"1.0\" encoding=\"UTF-8\"?><wowhead><error>Item not found!</error></wowhead>"))
            {
                string base_name = GetBaseName(name);
                output = GetWebResponse(base_name);
            }
            if (output != null)
            {
                PPather.Debug("WowHeadFetcher: response = {0}",output);
                return ProcessWHData(name, output);
            }
            return null;
        }

        public string GetWebResponse(string name)
        {
            string item = HttpUtility.UrlEncode(name);
            string parameters = "?item=" + item + "&xml";
            //PPather.WriteLine("parameters = " + parameters);

            Uri uri = new Uri("http://www.wowhead.com/" + parameters);
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);
            request.Method = WebRequestMethods.Http.Get;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string output = reader.ReadToEnd();
            response.Close();
            return output;
        }

        public List<string> GetEnchantLines(string item_id)
        {
            WebRequest req = HttpWebRequest.Create("http://www.wowhead.com/?item=" + item_id.ToString());
            WebResponse res = req.GetResponse();
            System.IO.StreamReader reader = new StreamReader(res.GetResponseStream());

            List<string> lines = new List<string>();
            string line = null;
            while ((line = reader.ReadLine()) != null)
            {
                if (line.StartsWith("<li><div><span class=\"q2"))
                {
                    lines.Add(line);
                    PPather.Debug("EnchantLine: {0}",line);
                }
            }

            return lines;
        }

        // Process the wowhead xml into a string to pass to storeitem
        private Dictionary<string,string> ProcessWHData(string itemName, string data)
        {
            string _icon_path = "http://static.wowhead.com/images/icons/large/";
            string tooltip = "";

            Dictionary<string, string> rd = new Dictionary<string, string>(); // return_data          

            string wowhead_id = "0";
            string type = "0"; string subtype = "0"; string slot = "0";
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

            string random_enchant = "0";

            System.Xml.XmlDocument WHData;
            System.Xml.XmlNode WHRoot;

            data = data.Replace("<?xml version=\"1.0\" encoding=\"UTF-8\"?>", "");

            WHData = new System.Xml.XmlDocument();
            WHData.LoadXml(@data);
            WHRoot = WHData.SelectSingleNode("wowhead");

            foreach (XmlNode Node in WHRoot.ChildNodes)
            {
                if (Node.Name == "item")
                {
                    foreach (XmlNode Attrib in Node.Attributes)
                    {
                        if (Attrib.Name == "id") { wowhead_id = Attrib.Value; }
                    }
                    foreach (XmlNode Node2 in Node.ChildNodes)
                    {
                        if (Node2.Name == "class") { type = Node2.InnerText; }
                        if (Node2.Name == "subclass" && Node2.InnerText != "") { subtype = Node2.InnerText; }
                        if (Node2.Name == "inventorySlot" && Node2.InnerText != "") { slot = Node2.InnerText; }
                        if (Node2.Name == "icon" && Node2.InnerText != "") { icon = _icon_path + Node2.InnerText.ToLowerInvariant(); }
                        if (Node2.Name == "htmlTooltip") { tooltip = Node2.InnerText; }
                    }
                }
            }

            MatchCollection myMatchCollection = Regex.Matches(tooltip, @"Requires Level ([0-9]{1,2})");
            foreach (Match myMatch in myMatchCollection) { required = myMatch.ToString().Replace("Requires Level ", ""); }
            myMatchCollection = Regex.Matches(tooltip, @"Classes: ([^<]*)");
            foreach (Match myMatch in myMatchCollection) { classes = myMatch.ToString().Replace("Classes: ", ""); }
            myMatchCollection = Regex.Matches(tooltip, @"([0-9]*)\sArmor");
            foreach (Match myMatch in myMatchCollection) { armor = myMatch.ToString().Replace(" Armor", ""); }
            myMatchCollection = Regex.Matches(tooltip, @"([0-9]*)\sBlock");
            foreach (Match myMatch in myMatchCollection) { block = myMatch.ToString().Replace(" Block", ""); }
            myMatchCollection = Regex.Matches(tooltip, @"([0-9]*)\s-\s([0-9]*)");
            foreach (Match myMatch in myMatchCollection) { min_damage = myMatch.ToString().Replace(" ", ""); string[] result = min_damage.Split(new Char[] { '-' }); min_damage = result[0]; max_damage = result[1]; }
            myMatchCollection = Regex.Matches(tooltip, @"Speed\s([0-9\.]*)");
            foreach (Match myMatch in myMatchCollection) { speed = myMatch.ToString().Replace("Speed ", ""); }
            myMatchCollection = Regex.Matches(tooltip, @"([0-9\.]*)\sdamage\sper\ssecond");
            foreach (Match myMatch in myMatchCollection) { dps = myMatch.ToString().Replace(" damage per second", ""); }
            myMatchCollection = Regex.Matches(tooltip, @".*Random\senchantment.*");
            foreach (Match myMatch in myMatchCollection) { random_enchant = "Random Enchant"; }
            myMatchCollection = Regex.Matches(tooltip, @"\+([0-9]*)\sAgility");
            foreach (Match myMatch in myMatchCollection) { agility = myMatch.ToString().Replace("+", "").Replace(" Agility", ""); }
            myMatchCollection = Regex.Matches(tooltip, @"\+([0-9]*)\sIntellect");
            foreach (Match myMatch in myMatchCollection) { intellect = myMatch.ToString().Replace("+", "").Replace(" Intellect", ""); }
            myMatchCollection = Regex.Matches(tooltip, @"\+([0-9]*)\sStamina");
            foreach (Match myMatch in myMatchCollection) { stamina = myMatch.ToString().Replace("+", "").Replace(" Stamina", ""); }
            myMatchCollection = Regex.Matches(tooltip, @"\+([0-9]*)\sSpirit");
            foreach (Match myMatch in myMatchCollection) { spirit = myMatch.ToString().Replace("+", "").Replace(" Spirit", ""); }
            myMatchCollection = Regex.Matches(tooltip, @"\+([0-9]*)\sStrength");
            foreach (Match myMatch in myMatchCollection) { strength = myMatch.ToString().Replace("+", "").Replace(" Strength", ""); }
            myMatchCollection = Regex.Matches(tooltip, @"\+([0-9]*)\sHealth");
            foreach (Match myMatch in myMatchCollection) { health = myMatch.ToString().Replace("+", "").Replace(" Health", ""); }
            myMatchCollection = Regex.Matches(tooltip, @"\+([0-9]*)\sMana");
            foreach (Match myMatch in myMatchCollection) { mana = myMatch.ToString().Replace("+", "").Replace(" Mana", ""); }
            myMatchCollection = Regex.Matches(tooltip, @"Equip:\sRestores\s([0-9\.]*)\shealth\sper\s5\ssec");
            foreach (Match myMatch in myMatchCollection) { hp_regen = myMatch.ToString().Replace("Equip: Restores ", "").Replace(" health per 5 sec", ""); }
            myMatchCollection = Regex.Matches(tooltip, @"Equip:\sRestores\s([0-9\.]*)\smana\sper\s5\ssec");
            foreach (Match myMatch in myMatchCollection) { mana_regen = myMatch.ToString().Replace("Equip: Restores ", "").Replace(" mana per 5 sec", ""); }
            // Catch all for the hybrid items with both melee and ranged merged together
            myMatchCollection = Regex.Matches(tooltip, @"Equip:\sIncreases\smelee\sand\sranged\sattack\spower\sby\s([0-9]*)");
            foreach (Match myMatch in myMatchCollection) { attack_power = myMatch.ToString().Replace("Equip: Increases melee and ranged attack power by ", ""); ranged_attack_power = attack_power; }
            myMatchCollection = Regex.Matches(tooltip, @"Equip:\sIncreases\sattack\spower\sby\s([0-9]*)");
            foreach (Match myMatch in myMatchCollection) { attack_power = myMatch.ToString().Replace("Equip: Increases attack power by ", ""); }
            myMatchCollection = Regex.Matches(tooltip, @"Equip:\sIncreases\sranged\sattack\spower\sby\s([0-9]*)");
            foreach (Match myMatch in myMatchCollection) { ranged_attack_power = myMatch.ToString().Replace("Equip: Increases ranged attack power by ", ""); }
            myMatchCollection = Regex.Matches(tooltip, @"Equip:\sIncreases\syour\sranged\sweapon\scritical\sstrike\sdamage\sbonus\sby\s([0-9]*)");
            foreach (Match myMatch in myMatchCollection) { ranged_crit = myMatch.ToString().Replace("Equip: Increases your ranged weapon critical strike damage bonus by ", ""); }
            myMatchCollection = Regex.Matches(tooltip, @"Equip:\sIncreases\sranged\sattack\sspeed\sby\s([0-9]*)%");
            foreach (Match myMatch in myMatchCollection) { ranged_attack_speed = myMatch.ToString().Replace("Equip: Increases ranged attack speed by ", "").Replace("%", ""); }
            myMatchCollection = Regex.Matches(tooltip, @"Equip:\sIncreases\sattack\spower\sby\s([0-9]*)\sin\sCat,\sBear,\sDire\sBear,\sand\sMoonkin\sforms\sonly");
            foreach (Match myMatch in myMatchCollection) { attack_power_feral = myMatch.ToString().Replace("Equip: Increases attack power by ", "").Replace(" in Cat, Bear, Dire Bear, and Moonkin forms only", ""); }
            myMatchCollection = Regex.Matches(tooltip, @"Equip:\sIncreases\syour\sexpertise\srating\sby\s([0-9]*)");
            foreach (Match myMatch in myMatchCollection) { expertise = myMatch.ToString().Replace("Equip: Increases your expertise rating by ", ""); }
            myMatchCollection = Regex.Matches(tooltip, @"Equip:\sIncreases\sdefense\srating\sby\s([0-9]*)");
            foreach (Match myMatch in myMatchCollection) { defense = myMatch.ToString().Replace("Equip: Increases defense rating by ", ""); }
            myMatchCollection = Regex.Matches(tooltip, @"Equip:\sImproves\sresilience\srating\sby\s([0-9]*)");
            foreach (Match myMatch in myMatchCollection) { resilience = myMatch.ToString().Replace("Equip: Increases resilience rating by ", ""); }
            myMatchCollection = Regex.Matches(tooltip, @"Equip:\sYou\sattacks\signore\s([0-9]*)\sof\syour\sopponent");
            foreach (Match myMatch in myMatchCollection) { armor_penetration = myMatch.ToString().Replace("Equip: Your attacks ignore ", "").Replace(" of your oppenent", ""); }
            myMatchCollection = Regex.Matches(tooltip, @"Equip:\sIncreases\syour\sshield\sblock\srating\sby\s([0-9]*)");
            foreach (Match myMatch in myMatchCollection) { shield_block = myMatch.ToString().Replace("Equip: Increases your shield block rating by ", ""); }
            myMatchCollection = Regex.Matches(tooltip, @"Equip:\sImproves critical strike rating by\s([0-9]*)");
            foreach (Match myMatch in myMatchCollection) { crit = myMatch.ToString().Replace("Equip: Improves critical strike rating by ", ""); }
            myMatchCollection = Regex.Matches(tooltip, @"Equip:\sImproves hit rating by\s([0-9]*)");
            foreach (Match myMatch in myMatchCollection) { hit = myMatch.ToString().Replace("Equip: Improves hit rating by ", ""); }
            myMatchCollection = Regex.Matches(tooltip, @"Equip:\sIncreases your dodge rating by\s([0-9]*)");
            foreach (Match myMatch in myMatchCollection) { dodge = myMatch.ToString().Replace("Equip: Increases your dodge rating by ", ""); }
            myMatchCollection = Regex.Matches(tooltip, @"Equip:\sIncreases\syour\sparry\srating\sby\s([0-9]*)");
            foreach (Match myMatch in myMatchCollection) { parry = myMatch.ToString().Replace("Equip: Increases your parry rating by ", ""); }
            myMatchCollection = Regex.Matches(tooltip, @"Equip:\sIncreases\sspell\spower\sby\s([0-9]*)");
            foreach (Match myMatch in myMatchCollection) { spell_power = myMatch.ToString().Replace("Equip: Increases spell power by ", ""); }
            
            myMatchCollection = Regex.Matches(tooltip, @"Equip:\sImproves\sspell\shit\srating\sby\s([0-9]*)");
            foreach (Match myMatch in myMatchCollection) { spell_hit = myMatch.ToString().Replace("Equip: Improves spell hit rating by ", ""); }
            myMatchCollection = Regex.Matches(tooltip, @"Equip:\sImproves\sspell\scritical\sstrike\srating\sby\s([0-9]*)");
            foreach (Match myMatch in myMatchCollection) { spell_crit = myMatch.ToString().Replace("Equip: Improves spell critical strike rating by ", ""); }
            myMatchCollection = Regex.Matches(tooltip, @"Equip:\sImproves\sspell\shaste\srating\sby\s([0-9]*)");
            foreach (Match myMatch in myMatchCollection) { spell_haste = myMatch.ToString().Replace("Equip: Improves spell haste rating by ", ""); }
            myMatchCollection = Regex.Matches(tooltip, @"Equip:\sIncreases\syour\sspell\spenetration\sby\s([0-9]*)");
            foreach (Match myMatch in myMatchCollection) { spell_penetration = myMatch.ToString().Replace("Equip: Increases your spell penetration by ", ""); }
            myMatchCollection = Regex.Matches(tooltip, @"\+([0-9\.]*)\sArcane\sResistance");
            foreach (Match myMatch in myMatchCollection) { resist_arcane = myMatch.ToString().Replace("+", "").Replace(" Arcane Resistance", ""); }
            myMatchCollection = Regex.Matches(tooltip, @"\+([0-9\.]*)\sFire\sResistance");
            foreach (Match myMatch in myMatchCollection) { resist_fire = myMatch.ToString().Replace("+", "").Replace(" Fire Resistance", ""); }
            myMatchCollection = Regex.Matches(tooltip, @"\+([0-9\.]*)\sFrost\sResistance");
            foreach (Match myMatch in myMatchCollection) { resist_frost = myMatch.ToString().Replace("+", "").Replace(" Frost Resistance", ""); }
            myMatchCollection = Regex.Matches(tooltip, @"\+([0-9\.]*)\sHoly\sResistance");
            foreach (Match myMatch in myMatchCollection) { resist_holy = myMatch.ToString().Replace("+", "").Replace(" Holy Resistance", ""); }
            myMatchCollection = Regex.Matches(tooltip, @"\+([0-9\.]*)\sNature\sResistance");
            foreach (Match myMatch in myMatchCollection) { resist_nature = myMatch.ToString().Replace("+", "").Replace(" Nature Resistance", ""); }
            myMatchCollection = Regex.Matches(tooltip, @"\+([0-9\.]*)\sShadow\sResistance");
            foreach (Match myMatch in myMatchCollection) { resist_shadow = myMatch.ToString().Replace("+", "").Replace(" Shadow Resistance", ""); }
            myMatchCollection = Regex.Matches(tooltip, @"Equip:\sIncreases\sdamage\sdone\sby\sArcane\sspells\sand\seffects\sby\sup\sto\s([0-9]*)");
            foreach (Match myMatch in myMatchCollection) { damage_arcane = myMatch.ToString().Replace("Equip: Increases damage done by Arcane spells and effects by up to ", ""); }
            myMatchCollection = Regex.Matches(tooltip, @"Equip:\sIncreases\sdamage\sdone\sby\sFire\sspells\sand\seffects\sby\sup\sto\s([0-9]*)");
            foreach (Match myMatch in myMatchCollection) { damage_fire = myMatch.ToString().Replace("Equip: Increases damage done by Fire spells and effects by up to ", ""); }
            myMatchCollection = Regex.Matches(tooltip, @"Equip:\sIncreases\sdamage\sdone\sby\sFrost\sspells\sand\seffects\sby\sup\sto\s([0-9]*)");
            foreach (Match myMatch in myMatchCollection) { damage_frost = myMatch.ToString().Replace("Equip: Increases damage done by Frost spells and effects by up to ", ""); }
            myMatchCollection = Regex.Matches(tooltip, @"Equip:\sIncreases\sdamage\sdone\sby\sHoly\sspells\sand\seffects\sby\sup\sto\s([0-9]*)");
            foreach (Match myMatch in myMatchCollection) { damage_holy = myMatch.ToString().Replace("Equip: Increases damage done by Holy spells and effects by up to ", ""); }
            myMatchCollection = Regex.Matches(tooltip, @"Equip:\sIncreases\sdamage\sdone\sby\sNature\sspells\sand\seffects\sby\sup\sto\s([0-9]*)");
            foreach (Match myMatch in myMatchCollection) { damage_nature = myMatch.ToString().Replace("Equip: Increases damage done by Nature spells and effects by up to ", ""); }
            myMatchCollection = Regex.Matches(tooltip, @"Equip:\sIncreases\sdamage\sdone\sby\sShadow\sspells\sand\seffects\sby\sup\sto\s([0-9]*)");
            foreach (Match myMatch in myMatchCollection) { damage_shadow = myMatch.ToString().Replace("Equip: Increases damage done by Shadow spells and effects by up to ", ""); }

            //Context.Log(" Requires Level: " + required + " Classes: " + classes + " Armor: " + armor + " DPS: " + dps + " Agility: " + agility + " Intellect: " + intellect + " Stamina: " + stamina + " Spirit: " + spirit + " Strength: " + strength + " Health: " + health + " Mana: " + mana);
            string return_value = wowhead_id + "|" + type + "|" + subtype + "|" + slot + "|" + icon + "|" + required + "|" + classes + "|" + armor + "|" + block +
                "|" + min_damage + "|" + max_damage + "|" + speed + "|" + dps + "|" + agility + "|" + intellect +
                "|" + stamina + "|" + spirit + "|" + strength + "|" + health + "|" + mana + "|" + hp_regen +
                "|" + mana_regen + "|" + attack_power + "|" + ranged_attack_power + "|" + ranged_crit +
                "|" + ranged_attack_speed + "|" + attack_power_feral + "|" + expertise + "|" + defense +
                "|" + resilience + "|" + armor_penetration + "|" + shield_block + "|" + crit + "|" + hit +
                "|" + dodge + "|" + parry + "|" + spell_power + "|" + spell_hit +
                "|" + spell_crit + "|" + spell_haste + "|" + spell_penetration + "|" + resist_arcane +
                "|" + resist_fire + "|" + resist_frost + "|" + resist_holy + "|" + resist_nature +
                "|" + resist_shadow + "|" + damage_arcane + "|" + damage_fire + "|" + damage_frost +
                "|" + damage_holy + "|" + damage_nature + "|" + damage_shadow;
            PPather.Debug("WoWHead Return: {0}",return_value);
            rd.Add("wowheadid", wowhead_id);
            rd.Add("type", type); rd.Add("subtype", subtype); rd.Add("slot", slot);
            rd.Add("icon", icon); // http link to item icon on wowhead

            rd.Add("required", required); rd.Add("classes", classes);

            rd.Add("armor", armor); 
            rd.Add("block", block);
            rd.Add("mindamage", min_damage); 
            rd.Add("maxdamage", max_damage);
            rd.Add("speed", speed); 
            rd.Add("dps", dps);

            rd.Add("agility", agility); 
            rd.Add("intellect", intellect);
            rd.Add("stamina", stamina); 
            rd.Add("spirit", spirit);
            rd.Add("strength", strength);
            rd.Add("health", health);
            rd.Add("mana", mana); 
            rd.Add("manaregen", mana_regen);
            rd.Add("hpregen", hp_regen);

            // complex, they are nested within each other
            rd.Add("attackpower", attack_power);
            rd.Add("rangedattackpower", ranged_attack_power);
            rd.Add("rangedcrit", ranged_crit);
            rd.Add("rangedattackspeed", ranged_attack_speed);
            rd.Add("attackpowerferal", attack_power_feral);
            rd.Add("expertise", expertise); 
            rd.Add("defense", "0"); 
            rd.Add("resilience", resilience);
            rd.Add("armorpenetration", armor_penetration);
            rd.Add("shieldblock", shield_block);
            rd.Add("crit", crit); 
            rd.Add("hit", hit);
            rd.Add("dodge", dodge); 
            rd.Add("parry", parry);

            // complex, they are nested within each other but in 2 different ways
            rd.Add("spellpower", spell_power); 
            rd.Add("spellhit", spell_hit); 
            rd.Add("spellcrit", spell_crit);
            rd.Add("spellhaste", spell_haste); 
            rd.Add("spellpenetration", spell_penetration);

            // Resists omg so much
            rd.Add("resistarcane", resist_arcane); 
            rd.Add("resistfire", resist_fire);
            rd.Add("resistfrost", resist_frost); 
            rd.Add("resistholy", resist_holy);
            rd.Add("resistnature", resist_nature); 
            rd.Add("resistshadow", resist_shadow);

            // + Damage
            rd.Add("damagearcane", damage_arcane);
            rd.Add("damagefire", damage_fire);
            rd.Add("damagefrost", damage_frost); 
            rd.Add("damageholy", damage_holy);
            rd.Add("damagenature", damage_nature); 
            rd.Add("damageshadow", damage_shadow);

            // Random enchant
            rd.Add("randomenchant", random_enchant);

            return rd;
        }
    }
}
