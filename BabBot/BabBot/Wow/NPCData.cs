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
using BabBot.Wow.Helpers;
using System.Text.RegularExpressions;
using System.Collections.Generic;

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
    
    public class ZoneServices
    {
        List<SimpleNpc> TaxiServices;
        List<SimpleNpc> InnServices;

        public ZoneServices()
        {
            TaxiServices = new List<SimpleNpc>();
            InnServices = new List<SimpleNpc>();
        }

        public void AddTaxiService(SimpleNpc npc)
        {
            TaxiServices.Add(npc);
        }

        public void AddInnService(SimpleNpc npc)
        {
            InnServices.Add(npc);
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

        

        public Quest FindMaxQuestByTitle(string title)
        {
            int max = -1;
            Quest res = null;

            foreach (NPC npc in Items)
            {
                if (npc.FindQuestQtyByTitle(title) > 0)
                    foreach (Quest q in npc.QuestList.Table.Values)
                        if ((q.Title.Equals(title)) && (q.QIdx > max))
                            res = q;

            }

            return res;
        }
    }

    /// <summary>
    /// Abstract NPC base class
    /// </summary>
    public abstract class AbstractNpc : CommonMergeListItem
    {
        public AbstractNpc() : base() { }

        public AbstractNpc(string name) : base(name) { }
    }

    /// <summary>
    /// Class to keep NPC that has single service and never move (taxi, bankers, ah)
    /// in simple format in toon's profile or in indexed list
    /// </summary>
    public class SimpleNpc : AbstractNpc
    {
        [XmlAttribute("service")]
        public string Service;

        [XmlAttribute("continent_id")]
        public int CID;

        [XmlAttribute("zone")]
        public string ZoneText;

        internal Vector3D BaseWaypoint;

        [XmlAttribute("x")]
        public float X {
            get { return BaseWaypoint.X; }
            set { BaseWaypoint.X = value; }
        }

        [XmlAttribute("y")]
        public float Y {
            get { return BaseWaypoint.Y; }
            set { BaseWaypoint.Y = value; }
        }
        
        [XmlAttribute("z")]
        public float Z {
            get { return BaseWaypoint.Z; }
            set { BaseWaypoint.Z = value; }
        }

        public SimpleNpc()
        {
            BaseWaypoint = new Vector3D();
        }

        public SimpleNpc(string name, string service, int cid, string zone, Vector3D wp)
            : base(name)
        {
            Service = service;
            CID = cid;
            ZoneText = zone;
            BaseWaypoint = (Vector3D) wp.Clone();
        }
    }

    public class NPC :AbstractNpc
    {
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

        internal bool HasTaxi
        {
            get { return Services.Table.ContainsKey("taxi"); }
        }

        internal bool HasInn
        {
            get { return Services.Table.ContainsKey("inn"); }
        }

        public NPC()
            : base()
        {
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

            Continents = new ContinentListId();
            Services = new NPCServices();
            QuestList = new Quests();

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
            if ((obj == null) || (obj.GetType() != typeof(NPC)))
                return false;

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

        public int FindQuestQtyByTitle(string title)
        {
            return QuestList.FindQuestQtyByTitle(title);
        }

        public SimpleNpc GetSimpleFormat()
        {
            ContinentId c = Continents.ContinentList[0];
            Zone z = c.ZList[0];
            return new SimpleNpc(Name, Services.ServiceList[0].SType, c.Id, z.Name, z.List[0]);
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

        public int FindQuestQtyByTitle(string title)
        {
            int qty = 0;
            foreach (KeyValuePair<string, Quest> item in Table)
                if (item.Value.Title.Equals(title))
                    qty++;
            return qty;
        }
    }

    public class Quest : CommonText, IMergeable
    {
        private bool _changed = false;

        [XmlIgnore]
        public bool Changed
        {
            get { return _changed || Relations.Changed; }
            set { _changed = value; }
        }

        internal string Title
        {
            get { return Name; }
        }

        [XmlAttribute("id")]
        public int Id;

        [XmlAttribute("link")]
        public string Link = "";

        [XmlAttribute("level")]
        public int Level;

        internal int QIdx = 0;

        [XmlAttribute("idx")]
        public string Idx
        {
            get { return (QIdx > 0) ? null : QIdx.ToString(); }
            set {QIdx = (value == null) ? 0 : Convert.ToInt32(value); }
        }

        [XmlAttribute("bonus_spell")]
        public string BonusSpell = "";

        private string _dest_npc_name = "";

        [XmlAttribute("dest_npc")]
        public string DestNpcName
        {
            get { return _dest_npc_name; }
            set
            {
                _changed = true;
                _dest_npc_name = value;
            }
        }

        [XmlAttribute("related_to")]
        public string RelatedTo
        {
            get
            {
                if ((Relations == null) || (Relations.List.Count == 0))
                    return null;

                string[] res = new string[Relations.List.Count];
                for (int i = 0; i < Relations.List.Count; i++)
                    res[i] = Relations.List[i];

                return string.Join(",", res);
            }

            set
            {
                if (value == null)
                    return;

                _changed = true;
                string[] s = value.Split(',');
                Relations = new QuestRelations(s);
            }
        }

        /// <summary>
        /// Actual array with dependency links of other quests
        /// </summary>
        internal QuestRelations Relations = new QuestRelations();

        internal QuestItem[] QuestItems = new QuestItem[3];

        internal NPC SrcNpc
        {
            get { return NpcList[0]; }
            set { NpcList[0] = value; }
        }

        internal NPC DestNpc
        {
            get { return NpcList[1]; }
            set { NpcList[1] = value; }
        }

        internal NPC[] NpcList = new NPC[2];
        
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

        internal string GreetingText
        {
            get { return TextData;  }
        }

        [XmlElement("objectives_text", typeof(XmlCDataSection))]
        public XmlCDataSection TextObjectives { get; set; }

        internal string ObjectivesText
        {
            get { return ((TextObjectives != null) ? TextObjectives.InnerText : null); }
        }

        [XmlElement("reward_text", typeof(XmlCDataSection))]
        public XmlCDataSection TextRewards { get; set; }

        internal string RewardsText
        {
            get { return ((TextRewards != null) ? TextRewards.InnerText : null); }
        }

        [XmlElement("objectives")]
        public QuestObjectives ObjList;

        public Quest() :base() {}

        public Quest(int id, string title, string text, string objectives, int level, 
                        int[] det_qty, string[] det_list, string objs, 
                                string bonus_spell, string link) :
            base(title, text)
        {
            Id = id;
            Link = link;
            Level = level;

            XmlDocument doc = new XmlDocument();
            TextObjectives = doc.CreateCDataSection(objectives);

            for (int i = 0; i < det_qty.Length; i++)
            {
                if (det_qty[i] > 0)
                {
                    QuestItem qi = new QuestItem();
                    QuestItems[i] = qi;
                    string[] det_item = det_list[i].Split(new string[] { "::" },
                                                    StringSplitOptions.None);
                    for (int j = 0; j < det_qty[i]; j++)
                    {
                        string[] d = det_item[j].Split(',');
                        qi.Add(new CommonQty(d[1], Convert.ToInt32(d[0])));
                    }
                }
            }

            if (!string.IsNullOrEmpty(objs))
                ObjList = new QuestObjectives(objs);

            BonusSpell = bonus_spell;
        }

        public override bool Equals(object obj)
        {
            if ((obj == null) || (obj.GetType() != typeof(Quest)))
                return false;

            return Equals((Quest)obj);
        }

        public bool Equals(Quest q)
        {
            bool f = q.Title.Equals(Title) &&
                q.GreetingText.Equals(GreetingText) &&
                (q.Level == Level) &&
                q.ObjectivesText.Equals(ObjectivesText) &&
                q.BonusSpell.Equals(BonusSpell) &&
                q.Id == Id &&
                q.DestNpcName.Equals(DestNpcName) &&
                q.Link.Equals(Link);
                

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

        public override string ToString()
        {
            return Id.ToString();
        }

        public void MergeWith(object obj)
        {
            if (!MergeHelper.IsMergeable(this, obj))
                return;

            Quest q2 = (Quest) obj;

            // Update dest npc name
            if (string.IsNullOrEmpty(DestNpcName)) 
                DestNpcName = q2.DestNpcName;
                    
            // and merge dependencies
            Relations.MergeWith(q2.Relations);
        }
    }

    public class QuestRelations : CommonList<string>
    {

        public QuestRelations() : base(true) {}
        public QuestRelations(string[] list) : base(list, true) {}

        public void Add(int quest_id)
        {
            base.Add(quest_id.ToString());
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
        public ContinentId[] ContinentList
        {
            get { return Items; }
            set { Items = value; }
        }

        internal string[] ZoneList
        {
            get
            {
                List<string> list = new List<string>();
                foreach (ContinentId c in Table.Values)
                    foreach (string z in c.Table.Keys)
                        list.Add(z);

                string[] res = new string[list.Count];
                list.CopyTo(res);
                return res;
            }
        }

        public ContinentId FindContinentById(int id)
        {
            return FindItemByName(id.ToString());
        }
    }

    public class ContinentId : CommonIdMergeTable<Zone>
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

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            Zone z = (Zone) obj;
            if (!z.Name.Equals(Name))
                return false;

            // Compare vector list. If they are the same of stays in 5 yard distance 
            //    from zone waypoints than we ok

            Vector3D first = List[0];

            for (int i = 0; i < z.List.Count; i++)
            {
                Vector3D cur_wp = z.List[i];

                // First check if vectors identicall
                if (cur_wp.Equals(first))
                    return true;

                // Now check that distance no more than 5F. NPC can rotate
                if (cur_wp.GetDistanceTo(first) > 5F)
                {
                    // Compare this item with others
                    for (int j = 1; j < List.Count; j++)
                    {
                        if (cur_wp.Equals(first))
                            break;

                        bool found = false;
                        if (cur_wp.GetDistanceTo(List[j]) < 5F)
                        {
                            found = true;
                            break;
                        }

                        if (!found)
                            return false;
                    }
                }
            }

            return true;
        }
    }

    #endregion

    #region Quest Objectives

    public class QuestObjectives : CommonList<AbstractQuestObjective>
    {

        [XmlElement("objective")]
        public AbstractQuestObjective[] ObjList
        {
            get { return Items; }
            set { Items = value; }
        }

        /// <summary>
        /// Paremetless class constructor
        /// </summary>
        public QuestObjectives()
            : base() { }

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="objs">List of objectives in format obj::obj 
        /// where each obj is comma delited list of item, qty, is_finished</param>
        public QuestObjectives(string objs)
        {
            string[] obj = objs.Split(new string[] { "::" }, StringSplitOptions.None);
            foreach (string s in obj)
            {
                string[] items = s.Split(',');
                string text = items[0];
                string stype = items[1];
                bool is_finished = (!string.IsNullOrEmpty(items[2]) &&
                                                        items[2].Equals("1"));

                AbstractQuestObjective qobj = null;

                // TODO Add reflection here
                switch (stype)
                {
                    case "event":
                        qobj = new EventQuestObjective(text, is_finished);
                        break;

                    case "item":
                        qobj = new ItemQuestObjective(text, is_finished);
                        break;

                    case "object":
                        qobj = new ObjectQuestObjective(text, is_finished);
                        break;

                    case "monster":
                        qobj = new MonsterQuestObjective(text, is_finished);
                        break;

                    case "reputation":
                        qobj = new ReputationQuestObjective(text, is_finished);
                        break;

                    default:
                        throw new QuestSkipException(
                            "Unknown type of quest objectives '" + stype + "'");
                }

                List.Add(qobj);
            }
        }
    }
        
    /// <summary>
    /// Abstract class for all quest objective typed
    /// </summary>
    [XmlInclude(typeof(EventQuestObjective))]
    [XmlInclude(typeof(ItemQuestObjective))]
    [XmlInclude(typeof(MonsterQuestObjective))]
    [XmlInclude(typeof(ObjectQuestObjective))]
    [XmlInclude(typeof(ReputationQuestObjective))]
    public abstract class AbstractQuestObjective
    {
        [XmlAttribute("type")]
        public string SType;

        [XmlAttribute("name")]
        public string Name;

        [XmlElement("waypoints")]
        public ContinentListId Waypoints;

        internal bool Finished = false;

        public AbstractQuestObjective(string type)
        {
            SType = type;
        }

        public AbstractQuestObjective(string type, string name, bool is_finished)
            : this(type)
        {
            Name = name;
            Finished = is_finished;
        }
    }

    /// <summary>
    /// Abstract class for quest objective that have item -> qty assignment
    /// </summary>
    public abstract class AbstractQuestObjectiveWithQty : AbstractQuestObjective
    {
        internal readonly int BagQty = 0;

        internal string ItemName
        {
            get { return Name; }
            set { Name = value; }
        }

        [XmlAttribute("qty")]
        public int ReqQty;

        public AbstractQuestObjectiveWithQty(string stype)
            : base(stype) { }

        public AbstractQuestObjectiveWithQty(string type, string item_str, bool is_finished)
            : base(type)
        {
            Regex r = DataManager.CurWoWVersion.QuestConfig.ObjectiveRx;
            Match m = r.Match(item_str);

            if ((!m.Success) || (m.Groups.Count != 4))
                throw new QuestProcessingException(
                    "Unable parse quest item string '" + item_str +
                    "' according pattern " + DataManager.CurWoWVersion.
                                        QuestConfig.ObjectiveRx.ToString());

            ItemName = m.Groups[1].ToString();

            try
            {
                BagQty = Convert.ToInt32(m.Groups[2].ToString());
                ReqQty = Convert.ToInt32(m.Groups[3].ToString());
            }
            catch (Exception e)
            {
                throw new QuestSkipException("Invalid objective in objective string '" + 
                    item_str + "'. " + e.Message);
            }
        }
    }

    /// <summary>
    /// Class for quest objectives that requires completion of a scripted event
    /// </summary>
    public class EventQuestObjective : AbstractQuestObjective
    {   
        public EventQuestObjective()
            : base("event") {}

        public EventQuestObjective(string text, bool is_finished)
            : base("event", text, is_finished) { }
    }

    /// <summary>
    /// Class for quest objectives that requires collecting a number of items
    /// </summary>
    public class ItemQuestObjective : AbstractQuestObjectiveWithQty
    {
        public ItemQuestObjective()
            : base("item") {}

        public ItemQuestObjective(string text, bool is_finished)
            : base("item", text, is_finished) { }
    }
    
    /// <summary>
    /// Class for quest objectives that requires slaying a number of NPCs
    /// </summary>
    public class MonsterQuestObjective : AbstractQuestObjectiveWithQty
    {
        public MonsterQuestObjective()
            : base("monster") {}

        public MonsterQuestObjective(string text, bool is_finished)
            : base("monster", text, is_finished) { }
    }

    /// <summary>
    /// Class for quest objectives that requires interacting with a world object
    /// </summary>
    public class ObjectQuestObjective : AbstractQuestObjective
    {
        public ObjectQuestObjective()
            : base("object") {}

        public ObjectQuestObjective(string text, bool is_finished)
            : base("object", text, is_finished) { }
    }

    /// <summary>
    /// Class for quest objectives that requires attaining a 
    /// certain level of reputation with a faction
    /// </summary>
    public class ReputationQuestObjective : AbstractQuestObjective
    {
        public ReputationQuestObjective()
            : base("reputation") {}

        public ReputationQuestObjective(string text, bool is_finished)
            : base("reputation", text, is_finished) { }
    }

    #endregion
}
