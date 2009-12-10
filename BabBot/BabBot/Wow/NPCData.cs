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
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using BabBot.Common;
using BabBot.Manager;

namespace BabBot.Wow
{
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

        [XmlIgnore]
        public Quest[] QuestList;

        public NPC FindNpcByName(string name)
        {
            return FindItemByName(name);
        }

        public Quest FindQuestByTitle(string title)
        {
            Quest q = null;

            foreach (NPC npc in Items)
            {
                q = npc.FindQuestByTitle(title);
                if (q != null)
                    break;
            }

            return q;
        }

        public void IndexData()
        {
            IndexQuestList();
        }

        /// <summary>
        /// Extract quests from NPC and index into sorted list
        /// </summary>
        private void IndexQuestList()
        {
            SortedList l = new SortedList();

            foreach (NPC npc in Table.Values)
            {
                Quests ql = npc.QuestList;
                if (ql != null)
                    foreach (Quest q in ql.Table.Values)
                    {
                        q.SrcNpc = npc;
                        l.Add(q.Name, q);
                    }
            }

            QuestList = new Quest[l.Count];
            l.Values.CopyTo(QuestList, 0);
        }
    }

    public class NPC : CommonMergeListItem
    {
        [XmlAttribute("type")] 
        public string Type;

        [XmlAttribute("race")]
        public string Race;

        [XmlAttribute("sex")]
        public string Sex;

        [XmlAttribute("class")]
        public string Class;

        [XmlElement("wp_list")]
        public ContinentListId Continents
        {
            get { return (ContinentListId)MergeList[0]; }
            set { MergeList[0] = value; }
        }

        [XmlElement("services")]
        public NPCServices Services
        {
            get { return (NPCServices)MergeList[1]; }
            set { MergeList[1] = value; }
        }

        [XmlElement("quests")]
        public Quests QuestList
        {
            get { return (Quests)MergeList[2]; }
            set { MergeList[2] = value; }
        }

        public NPC() {
            MergeList = new IMergeable[3];
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

            Continents.Add(new ContinentId(continent_id, new Zone(zone_text, waypoint)));
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
                
                // Service list
                Services.Equals(npc.Services) &&
                // Waypoints
                Continents.Equals(npc.Continents) &&
                // Quest List
                QuestList.Equals(npc.QuestList));
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public Quest FindQuestByTitle(string title)
        {
            return QuestList.FindQuestByTitle(title);
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

        public Quest FindQuestByTitle(string title)
        {
            return FindItemByName(title);
        }
    }

    public class Quest : CommonText
    {
        [XmlAttribute("level")]
        public int Level;

        [XmlAttribute("bonus_spell")]
        public string BonusSpell;

        [XmlAttribute("dest_npc")]
        public string DestNpc = null;

        [XmlAttribute("depends_of")]
        public string DependsOf = null;

        [XmlIgnore]
        internal QuestItem[] QuestItems = new QuestItem[3];

        [XmlIgnore]
        public NPC SrcNpc = null;
        
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
            if ((obj == null) || (typeof(object) != typeof(Quest)))
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

    public class QuestItem : CommonList<CommonQty>
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
    public class NPCServices : CommonTable<NPCService>, IMergeable
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
    public class NPCService : CommonItem
    {
        [XmlIgnore]
        public string SType
        {
            get { return Name; }
        }

        public NPCService() : base() { }

        public NPCService(string stype) : base(stype) {}
    }

    public class ClassTrainingService : NPCService
    {
        [XmlAttribute("class")]
        public string CharClass;

        public ClassTrainingService() : base() { }

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

        public TradeSkillTrainingService() : base() { }

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

        public VendorService() : base() { }

        public VendorService(bool can_repair, bool has_water, bool has_food)
            : base("vendor")
        {
            CanRepair = can_repair;
            HasWater = has_water;
            HasFood = has_food;
        }
    }

    #endregion

    #region Waypoints

    public class ContinentListId : CommonTable<ContinentId>
    {
        [XmlElement("continent")]
        public ContinentId[] Continents
        {
            get { return Items; }
            set { Items = value; }
        }

        public ContinentId FindContinentById(int id)
        {
            return FindItemByName(Convert.ToString(id));
        }
    }

    public class ContinentId : CommonIdTable<Zone>
    {
        [XmlElement("zone")]
        public Zone[] ZList
        {
            get { return Items; }
            set { Items = value; }
        }

        public ContinentId() { }
        public ContinentId(int id) : base(id) { }
        public ContinentId(int id, Zone z) 
            : this(id) 
        {
            Table.Add(z.Name, z);
        }

        public Zone FindZoneByName(string name)
        {
            return FindItemByName(name);
        }
    }

    

    public class Zone : CommonNameList<Vector3D>
    {
        [XmlElement("waypoint")]
        public Vector3D[] VectorList
        {
            get { return Items; }
            set { Items = value; }
        }

        public Zone() { }
        public Zone(string name) : base(name) { }
        public Zone(string name, Vector3D v)
            : this(name)
        {
            List.Add(v);
        }
    }

    #endregion
}
