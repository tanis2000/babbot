/*
    This file is part of BabBot.

    BabBot is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    BabBot is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with BabBot.  If not, see <http://www.gnu.org/licenses/>.
  
    Copyright 2009 BabBot Team
*/
using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using BabBot.Common;

namespace BabBot.Wow
{
    // TODO organize NPC continent ID into list 
    // with table of zones NPC was found. Keep waypoints assigned per zone
    #region NPC
    
    [XmlRoot("npc_data")]
    public class NPCData : CommonTable<NPCVersion>
    {
        [XmlAttribute("version")]
        public int Version;

        [XmlElement("wow_version")]
        public NPCVersion[] Versions
        {
            get { return (NPCVersion[])Items; }
            set { Items = value; }
        }

        public NPCVersion FindVersion(string version)
        {
            return FindItemByName(version);
        }
    }
    
    [Serializable]
    public class NPCVersion : CommonNameTable<NPC>
    {
        [XmlElement("npc")]
        public NPC[] NPCList
        {
            get { return Items; }
            set { Items = value; }
        }

        public NPC FindNpcByName(string name)
        {
            return FindItemByName(name);
        }
    }

    public class NPC : CommonItem
    {
        [XmlAttribute("type")] 
        public string Type;

        [XmlAttribute("race")]
        public string Race;

        [XmlAttribute("sex")]
        public string Sex;

        [XmlAttribute("continent_id")] 
        public int ContinentId;

        [XmlAttribute("zone_text")] 
        public string ZoneText;

        [XmlAttribute("class")] 
        public string Class;

        [XmlElement("services")]
        public NPCServices Services;

        [XmlElement("quests")]
        public Quests QuestList;

        [XmlElement("wp_list")]
        public Waypoints WPList;

        public NPC() {
            WPList = new Waypoints();
            Services = new NPCServices();
            QuestList = new Quests();
        }

        public NPC(WowPlayer player, string race, string sex) : this()
        {
            WowUnit w = player.CurTarget;

            Init(w.Name, race, sex, player.ContinentID, 
                player.ZoneText, (Vector3D) w.Location.Clone());
        }

        public void Init(string name, string race, string sex, 
                int continent_id, string zone_text, Vector3D waypoint)
        {
            Name = name;
            Race = race;
            Sex = sex;

            ContinentId = continent_id;
            ZoneText = zone_text;

            WPList.Add(waypoint);
        }

        public void AddService(NPCService service)
        {
            Services.Add(service);
        }

        public void AddQuest(Quest qh)
        {
            QuestList.Add(qh);
        }
        
        public override bool Equals(object obj)
        {
            NPC npc = (NPC)obj;

            return (
                // Check name
                Name.Equals(npc.Name) && 
                // Continent ID
                (ContinentId == npc.ContinentId) &&
                // Zone Text
                (ZoneText.Equals(npc.ZoneText)) &&
                // Service list
                Services.Equals(npc.Services) &&
                // Waypoints
                WPList.Equals(npc.WPList) &&
                // Quest List
                QuestList.Equals(npc.QuestList));
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public void Merge(NPC npc)
        {
            // Merge services
            Services.Merge(npc.Services);

            // Merge Waypoints
            WPList.Merge(npc.WPList);
            
            // Merge Quest List
            QuestList.Merge(npc.QuestList);
        }
    }

    #endregion
    
    #region Waypoints
    
    public class Waypoints : CommonList<Vector3D>
    {
        [XmlElement("waypoint")]
        public Vector3D[] VectorList
        {
            get { return Items; }
            set { Items = value; }
        }
    }

    #endregion
    
    #region Quests

    public class Quests : CommonTable<Quest>
    {
        [XmlElement("quest")]
        public Quest[] QuestList
        {
            get { return Items; }
            set { Items = value; }
        }
    }

    public class Quest : CommonText
    {
        [XmlAttribute("level")]
        public int Level;

        [XmlAttribute("bonus_spell")]
        public string BonusSpell;

        [XmlIgnore]
        internal QuestItem[] QuestItems = new QuestItem[3];

        [XmlElement("req_items")]
        public QuestItem ReqItems
        {
            get { return QuestItems[0]; }
            set { QuestItems[0] = value; }
        }


        [XmlElement("reward_items")]
        public QuestItem RewardItems
        {
            get { return QuestItems[1]; }
            set { QuestItems[1] = value; }
        }
        
        [XmlElement("choice_items")]
        public QuestItem ChoiceItems
        {
            get { return QuestItems[2]; }
            set { QuestItems[2] = value; }
        }

        [XmlElement("objectives", typeof(XmlCDataSection))]
        public XmlCDataSection Objectives { get; set; }

        [XmlElement("dest_npc")]
        public string DestNpc = null;

        [XmlIgnore]
        public string TextObjectives
        {
            get { return ((Objectives != null) ? Objectives.InnerText : null); }
        }

        public Quest() {}

        public Quest(string title, string text, string objectives, int level, 
                int[] iqty, string[] det_list, string bonus_spell) :
            base(title, text)
        {
            Level = level;

            XmlDocument doc = new XmlDocument();
            Objectives = doc.CreateCDataSection(objectives);

            for (int i = 0; i < iqty.Length; i++)
                if (iqty[i] > 0)
                {
                    QuestItem qi = new QuestItem();
                    QuestItems[i] = qi;
                    string[] det_item = det_list[i].Split(new string[] { "::" }, 
                                                    StringSplitOptions.None);
                    for (int j = 0; j < iqty[i]; j++)
                    {
                        string[] d = det_item[j].Split(',');
                        qi.Add(new CommonQty(d[1], Convert.ToInt32(d[0])));
                    }
                }
                    
            BonusSpell = bonus_spell;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            return Equals((Quest)obj);
        }

        public bool Equals(Quest q)
        {
            bool f = q.Name.Equals(Name) &&
                q.TextData.Equals(TextData) &&
                (q.Level == Level) &&
                q.TextObjectives.Equals(TextObjectives) &&
                q.BonusSpell.Equals(BonusSpell);

            if (!f)
                return false;

            // Check Req List
            QuestItem[] rl = QuestItems;

            if ((rl != null) && (q.QuestItems != null))
            {
                for (int i = 0; i < rl.Length; i++)
                {
                    QuestItem ra1 = rl[i];
                    QuestItem ra2 = q.QuestItems[i];

                    if (ra1 == null)
                    {
                        if (ra2 != null)
                            return false;
                    }
                    else
                    {
                        if (ra2 == null)
                            return false;
                        else
                            // Check item by item
                            if (! ra1.Equals(ra2))
                                return false;
                    }
                }
            }
            else
            {
                if (((rl == null) && (q.QuestItems != null)) ||
                    ((rl != null) && (q.QuestItems == null)))
                    return false;
            }


            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public class QuestItem : CommonTable<CommonQty>
    {
        [XmlElement("item")]
        public CommonQty[] ItemList
        {
            get { return Items; }
            set { Items = value; }
        }
    }

    #endregion

    #region NPC Services

    // Service container
    public class NPCServices : CommonTable<NPCService>
    {
        [XmlElement("service")]
        public NPCService[] ServiceList
        {
            get { return Items; }
            set { Items = value; }
        }
    }

    // Base class
    [XmlInclude(typeof(ClassTrainingService))]
    [XmlInclude(typeof(TradeSkillTrainingService))]
    [XmlInclude(typeof(TradeSkillTrainingService))]
    [XmlInclude(typeof(VendorService))]
    public class NPCService
    {
        [XmlAttribute("type")]
        public string SType;

        public NPCService() { }

        public NPCService(string stype)
        {
            SType = stype;
        }

        public override string ToString()
        {
            return SType;
        }
    }

    public class ClassTrainingService : NPCService
    {
        [XmlAttribute("class")]
        public string CharClass;

        ClassTrainingService() { }

        public ClassTrainingService(string class_name)
            : base("class_trainer")
        {
            CharClass = class_name;
        }
    }

    public class TradeSkillTrainingService : NPCService
    {
        [XmlAttribute("trade_skill")]
        public string TradeSkill;

        TradeSkillTrainingService() { }

        public TradeSkillTrainingService(string trade_skill)
            : base("trade_skill_trainer")
        {
            TradeSkill = trade_skill;
        }
    }

    public class VendorService : NPCService
    {
        [XmlAttribute("can_repair")]
        public bool CanRepair;

        [XmlAttribute("has_water")]
        public bool HasWater;

        [XmlAttribute("has_food")]
        public bool HasFood;

        VendorService() { }

        public VendorService(bool can_repair, bool has_water, bool has_food)
            : base("vendor")
        {
            CanRepair = can_repair;
            HasWater = has_water;
            HasFood = has_food;
        }
    }

    #endregion
}
