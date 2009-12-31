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
   

    #region Game Objects

    [XmlRoot("game_object_data")]
    public class GameObjectData : CommonVersionTable<GameDataVersion> {}

    [Serializable]
    public class GameDataVersion : CommonNameTable<GameObject>
    {
        [XmlElement("game_object")]
        public GameObject[] GameObjList
        {
            get { return Items; }
            set { Items = value; }
        }

        public GameObject FindGameObjByName(string name)
        {
            return FindItemByName(name);
        }

        public Quest FindMaxQuestByTitle(string title)
        {
            int max = -1;
            Quest res = null;

            foreach (GameObject obj in STable.Values)
            {
                if (obj.FindQuestQtyByTitle(title) > 0)
                    foreach (Quest q in obj.QuestList.Table.Values)
                        if ((q.Title.Equals(title)) && (q.QNum > max))
                            res = q;

            }

            return res;
        }
    }

    /// <summary>
    /// Base class for all In-Game clickable objects
    /// It can own quests but doesn't allow interact with (target)
    /// GameObject can't move, have services or belong to faction
    /// </summary>
    [XmlRoot("game_object")]
    [XmlInclude(typeof(NPC))]
    public class GameObject : CommonMergeListItem
    {
        /// <summary>
        /// Base object coordinates
        /// </summary>
        [XmlElement("base_position")]
        public Vector3D BasePosition;

        /// <summary>
        /// Zone name where game object located
        /// for ex. "Teldrassil
        /// </summary>
        [XmlAttribute("zone")]
        public string ZoneText;

        /// <summary>
        /// List of quests related to this game object
        /// Includes only quests where it's act as quest giver
        /// </summary>
        [XmlElement("quests")]
        public Quests QuestList
        {
            get { return (Quests)MergeList[0]; }
            set { MergeList[0] = value; }
        }

        internal double X
        {
            get { return BasePosition.X; }
        }

        internal double Y
        {
            get { return BasePosition.Y; }
        }

        internal double Z
        {
            get { return BasePosition.Z; }
        }

        /// <summary>
        /// Full GameObject name include title
        /// i.e Item/Npc
        /// </summary>
        internal virtual string FullName
        {
            get { return "Item: " + Name; }
        }

        /// <summary>
        /// Object type according enum GameObjectTypes
        /// </summary>
        public virtual DataManager.GameObjectTypes ObjType
        {
            get { return DataManager.GameObjectTypes.ITEM; }
        }

        /// <summary>
        /// Base class constructor
        /// </summary>
        public GameObject()
            : base()
        {
            BasePosition = new Vector3D();

            Init();
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="name">GameObject name</param>
        /// <param name="zone">Zone name where object located</param>
        /// <param name="wp">Base coordinate</param>
        public GameObject(string name, string zone, Vector3D wp)
            : base(name)
        {
            ZoneText = zone;
            BasePosition = (Vector3D)wp.Clone();

            Init();
        }

        protected virtual void Init()
        {
            MergeList = new IMergeable[1];
            QuestList = new Quests();
        }

        /// <summary>
        /// Add quest related to object
        /// </summary>
        /// <param name="qh">Quest object</param>
        public void AddQuest(Quest qh)
        {
            QuestList.Add(qh);
        }

        /// <summary>
        /// Find Number of quest with same title 
        /// related to this GameObject
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public int FindQuestQtyByTitle(string title)
        {
            return QuestList.FindQuestQtyByTitle(title);
        }

        public override bool Equals(object arg)
        {
            if (!base.Equals(arg))
                return false;

            GameObject obj = (GameObject) arg;

            return ZoneText.Equals(obj.ZoneText) &&
                (ObjType == obj.ObjType) &&
                (BasePosition.GetDistanceTo(obj.BasePosition) < 5F) && 
                QuestList.Equals(obj.QuestList);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    /// <summary>
    /// NPC class. Inherited from GameObject
    /// In addition: 
    ///   - it can be interacted with,
    ///   - belong to faction (Alliance/Horde or be neuteral)
    ///   - have multiple services (for ex. vendor, inn),
    ///   - can be mobile (have multiple coordinates)
    /// </summary>
    public class NPC : GameObject
    {
        /// <summary>
        /// NPC faction (Alliance/Horde)
        /// If null/empty might be Neuteral. Not tested
        /// </summary>
        [XmlAttribute("faction")]
        public string Faction;

        /// <summary>
        /// List of NPC coordinates other than base coordinate
        /// </summary>
        [XmlElement("coordinates")]
        public WpZones Coordinates
        {
            get { return (WpZones)MergeList[1]; }
            set { MergeList[1] = value; }
        }

        /// <summary>
        /// List of NPC services
        /// </summary>
        [XmlElement("services")]
        public NPCServices Services
        {
            get { return (NPCServices)MergeList[2]; }
            set { MergeList[2] = value; }
        }

        /// <summary>
        /// Is NPC moving i.e has additional coordinates other than base coordinate
        /// </summary>
        internal bool Mobile
        {
            get { return Coordinates.Table.Count > 0; }
        }

        internal override string FullName
        {
            get { return "Npc: " + Name; }
        }

        internal bool HasTaxi
        {
            get { return Services.Table.ContainsKey("taxi"); }
        }

        internal bool HasInn
        {
            get { return Services.Table.ContainsKey("inn"); }
        }

        internal bool IsVendor
        {
            get { return Services.Table.ContainsKey("vendor"); }
        }

        public override DataManager.GameObjectTypes ObjType
        {
            get { return DataManager.GameObjectTypes.NPC; }
        }
        
        public NPC() : base() { }

        public NPC(string name, string zone, Vector3D wp, string faction)
            : base(name, zone, wp)
        {
            Faction = faction;
        }

        protected override void Init()
        {
            base.Init();

            Array.Resize<IMergeable>(ref MergeList, 3);

            Coordinates = new WpZones();
            Services = new NPCServices();
        }

        public NPC(WowPlayer player, string faction)
            : this(player.CurTarget.Name, player.ZoneText, player.CurTarget.Location, faction) { }

        public void AddService(NPCService service)
        {
            // if (string.IsNullOrEmpty(Service)) Service = service.Name;
            Services.Add(service);
        }

        public bool IsFriendly(string faction)
        {
            return faction.Equals(Faction);
        }
        
        public override bool Equals(object obj)
        {
            if (!base.Equals(obj))
                return false;


            NPC npc = (NPC)obj;

            return
                // Faction can be null
                MergeHelper.Compare(Faction, npc.Faction) &&

                // Service list
                Services.Equals(npc.Services) &&
                // Waypoints
                Coordinates.Equals(npc.Coordinates);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    #endregion
    
    #region Quests

    /// <summary>
    /// Quest states during execution
    /// </summary>
    public enum QuestStates : sbyte
    {
        SKIPPED,
        UNKNOWN,
        OBJ_FOUND,
        MOVING_TO_OBJ,
        OBJ_REACHED,
        OBJ_TARGETED,
        SELECTED, 
        ACCEPTED,
        PROGRESS,
        COMPLETED,
        DELIVERED
    }

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

    /// <summary>
    /// Quest class
    /// </summary>
    public class Quest : CommonText, IMergeable
    {
        /// <summary>
        /// WoW Quest Id
        /// </summary>
        [XmlAttribute("id")]
        public int Id;

        /// <summary>
        /// Quest link as it retrieved from last wow version
        /// </summary>
        [XmlAttribute("link")]
        public string Link = "";

        /// <summary>
        /// Recommended toon level to accomplish quest
        /// </summary>
        [XmlAttribute("level")]
        public int Level;

        /// <summary>
        /// Number of quest with same title found in local list
        /// </summary>
        internal int QNum = 0;

        /// <summary>
        /// Number of quest with same title found in local list
        /// </summary>
        [XmlAttribute("num")]
        public string Num
        {
            get { return (QNum > 0) ? null : QNum.ToString(); }
            set { QNum = (value == null) ? 0 : Convert.ToInt32(value); }
        }

        /// <summary>
        /// Bonus Spell (if any) rewarded by quest
        /// </summary>
        [XmlAttribute("bonus_spell")]
        public string BonusSpell = "";

        /// <summary>
        /// Name of destination game object
        /// </summary>
        private string _dest_name = "";

        /// <summary>
        /// Name of destination game object
        /// </summary>
        [XmlAttribute("dest_name")]
        public string DestName
        {
            get { return _dest_name; }
            set
            {
                _changed = true;
                _dest_name = value;
            }
        }

        /// <summary>
        /// List of comma delimited quest id this quest related to
        /// </summary>
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
        /// Required items (not objectives)
        /// </summary>
        [XmlElement("req_items")]
        public QuestItem ReqItems
        {
            get 
            { 
                return QuestItems[Array.IndexOf(DataManager.QuestItemSeq, 
                            DataManager.QuestItemTypes.REQUIRED)];
            }
            set
            {
                QuestItems[Array.IndexOf(DataManager.QuestItemSeq,
                          DataManager.QuestItemTypes.REQUIRED)] = value;
            }
        }

        /// <summary>
        /// Reward items provided by default
        /// </summary>
        [XmlElement("reward_items")]
        public QuestItem RewardItems
        {
            get
            {
                return QuestItems[Array.IndexOf(DataManager.QuestItemSeq,
                          DataManager.QuestItemTypes.REWARD)];
            }
            set
            {
                QuestItems[Array.IndexOf(DataManager.QuestItemSeq,
                        DataManager.QuestItemTypes.REWARD)] = value;
            }
        }
        
        /// <summary>
        /// List of Reward items that required selection
        /// </summary>
        [XmlElement("choice_items")]
        public QuestItem ChoiceItems
        {
            get
            {
                return QuestItems[Array.IndexOf(DataManager.QuestItemSeq,
                        DataManager.QuestItemTypes.CHOICE)];
            }
            set
            {
                QuestItems[Array.IndexOf(DataManager.QuestItemSeq,
                      DataManager.QuestItemTypes.CHOICE)] = value;
            }
        }

        /// <summary>
        /// Register changes during merge or update
        /// </summary>
        private bool _changed = false;

        /// <summary>
        /// Register changes during merge or update
        /// </summary>
        [XmlIgnore]
        public bool Changed
        {
            get { return _changed || Relations.Changed; }
            set { _changed = value; }
        }

        /// <summary>
        /// Quest title
        /// </summary>
        internal string Title
        {
            get { return Name; }
        }

        /// <summary>
        /// Actual array with dependency links of other quests
        /// </summary>
        internal QuestRelations Relations = new QuestRelations();

        /// <summary>
        /// Total array with all quest items except objectives
        /// </summary>
        internal QuestItem[] QuestItems = new QuestItem[3];

        /// <summary>
        /// Game Object that act as a Quest giver
        /// </summary>
        internal GameObject Src
        {
            get { return GameObjList[0]; }
            set { GameObjList[0] = value; }
        }

        /// <summary>
        /// Game Object that act as a Quest destination
        /// </summary>
        internal GameObject Dest
        {
            get { return GameObjList[1]; }
            set { GameObjList[1] = value; }
        }

        /// <summary>
        /// Array with Quest start/end Game Objects
        /// </summary>
        internal GameObject[] GameObjList = new GameObject[2];

        /// <summary>
        /// Greeting text
        /// </summary>
        internal string GreetingText
        {
            get { return TextData;  }
        }

        /// <summary>
        /// Greeting text
        /// </summary>
        [XmlElement("objectives_text", typeof(XmlCDataSection))]
        public XmlCDataSection TextObjectives { get; set; }

        /// <summary>
        /// Objectives text
        /// </summary>
        internal string ObjectivesText
        {
            get { return ((TextObjectives != null) ? TextObjectives.InnerText : null); }
        }

        /// <summary>
        /// Reward text
        /// </summary>
        [XmlElement("reward_text", typeof(XmlCDataSection))]
        public XmlCDataSection TextRewards { get; set; }

        internal string RewardsText
        {
            get { return ((TextRewards != null) ? TextRewards.InnerText : null); }
        }

        /// <summary>
        /// List of objectives
        /// </summary>
        [XmlElement("objectives")]
        public QuestObjectives ObjList;

        /// <summary>
        /// Index in toon quest log if quest already accepted
        /// </summary>
        internal int Idx = 0;

        /// <summary>
        /// Quest State
        /// </summary>
        internal QuestStates State = QuestStates.UNKNOWN;

        internal bool Completed
        {
            get { return State == QuestStates.COMPLETED; }
            set { if (value) State = QuestStates.COMPLETED; }
        }

        /// <summary>
        /// Class parameterless constructor
        /// </summary>
        public Quest() :base() {}

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="id">Quest ID</param>
        /// <param name="title">Quest Title</param>
        /// <param name="text">Greeting text</param>
        /// <param name="objectives">Objectives text</param>
        /// <param name="level">Recommended level to accept</param>
        /// <param name="bonus_spell">Bonus spell (if any)</param>
        /// <param name="link">Quest link</param>
        public Quest(int id, string title, string text, string objectives, 
                                int level, string bonus_spell, string link) :
            base(title, text)
        {
            Id = id;
            Link = link;
            Level = level;
            BonusSpell = bonus_spell;

            XmlDocument doc = new XmlDocument();
            TextObjectives = doc.CreateCDataSection(objectives);
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="id">Quest ID</param>
        /// <param name="title">Quest Title</param>
        /// <param name="text">Greeting text</param>
        /// <param name="objectives">Objectives text</param>
        /// <param name="level">Recommended level to accept</param>
        /// <param name="det_qty">Array with qty for quest items</param>
        /// <param name="det_list">Array with quest items</param>
        /// <param name="objs">Quest objectives</param>
        /// <param name="bonus_spell">Bonus spell (if any)</param>
        /// <param name="link">Quest link</param>
        public Quest(int id, string title, string text, string objectives, int level, 
                        int[] det_qty, string[] det_list, string objs, 
                                string bonus_spell, string link) :
            this(id, title, text, objectives, level, bonus_spell, link)
        {
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

            
        }

        public override bool Equals(object obj)
        {
            if ((obj == null) || (obj.GetType() != typeof(Quest)))
                return false;

            return Equals((Quest)obj);
        }

        public bool Equals(Quest q)
        {
            bool f = Title.Equals(q.Title) &&
                GreetingText.Equals(q.GreetingText) &&
                (Level == q.Level) &&
                ObjectivesText.Equals(q.ObjectivesText) &&
                BonusSpell.Equals(q.BonusSpell) &&
                (Id == q.Id) &&
                DestName.Equals(q.DestName) &&
                Link.Equals(q.Link);

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
            if (string.IsNullOrEmpty(DestName))
                DestName = q2.DestName;
                    
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

    /// <summary>
    /// Base class for any NPC service
    /// </summary>
    [XmlInclude(typeof(ClassTrainingService))]
    [XmlInclude(typeof(TradeSkillTrainingService))]
    [XmlInclude(typeof(TradeSkillTrainingService))]
    [XmlInclude(typeof(VendorService))]
    [XmlInclude(typeof(ZoneNpcService))]
    public class NPCService : CommonItem
    {
        /// <summary>
        /// NPC type as stored in XML
        /// </summary>
        internal string SType
        {
            get { return Name; }
        }

        /// <summary>
        /// NPC type as stored in Bot Data Set
        /// </summary>
        internal virtual DataManager.ServiceTypes SrvType
        {
            get {
                switch (SType)
                {
                    case "banker": return DataManager.ServiceTypes.BANKER;
                    case "battlemaster": return DataManager.ServiceTypes.BATTLEMASTER;
                    default:
                        throw new ServiceNotFound(SType);
                }
            }
        }

        /// <summary>
        /// Retrieve service specific description
        /// i.e class name for class trainers and so on
        /// </summary>
        internal virtual string Descr
        {
            get { return ""; }
        }

        public NPCService() : base() { }

        public NPCService(string stype) : base(stype) {}
    }

    /// <summary>
    /// NPC service related to local zone as inn or taxi. 
    /// It requires know the NPC subzone to local final destination point
    /// </summary>
    public class ZoneNpcService : NPCService
    {
        [XmlAttribute("sub_zone")]
        public string SubZone;

        internal override string Descr
        {
            get { return SubZone; }
        }

        internal override DataManager.ServiceTypes SrvType
        {
            get
            {
                switch (SType)
                {
                    case "taxi": return DataManager.ServiceTypes.TAXI;
                    case "inn": return DataManager.ServiceTypes.INN;
                    default: return base.SrvType;
                }
            }
        }

        public ZoneNpcService() { }

        public ZoneNpcService(string stype, string subzone) : base(stype) 
        {
            SubZone = subzone;
        }
    }

    public class ClassTrainingService : NPCService
    {
        [XmlAttribute("class")]
        public string ClassName;

        internal override DataManager.ServiceTypes SrvType
        {
            get { return DataManager.ServiceTypes.CLASS_TRAINER; }
        }

        internal override string Descr
        {
            get { return ClassName.ToLower(); }
        }

        public ClassTrainingService() : base() { }

        public ClassTrainingService(string class_name)
            : base("class_trainer")
        {
            ClassName = class_name;
        }
    }

    public class WepSkillService : NPCService
    {
        [XmlAttribute("wep_skills")]
        public string WepSkills;

        internal override DataManager.ServiceTypes SrvType
        {
            get { return DataManager.ServiceTypes.WEP_SKILL_TRAINER; }
        }

        internal override string Descr
        {
            get { return WepSkills; }
        }

        public WepSkillService() : base() { }

        public WepSkillService(string wep_skills)
            : base("wep_skill_trainer")
        {
            WepSkills = wep_skills;
        }
    }

    public class TradeSkillTrainingService : NPCService
    {
        [XmlAttribute("trade_skill")]
        public string TradeSkill;

        internal override DataManager.ServiceTypes SrvType
        {
            get { return DataManager.ServiceTypes.TRADE_SKILL_TRAINER; }
        }

        internal override string Descr
        {
            get { return TradeSkill; }
        }

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

        [XmlAttribute("has_drink")]
        public bool HasDrink;

        [XmlAttribute("has_food")]
        public bool HasFood;

        internal override DataManager.ServiceTypes SrvType
        {
            get 
            {
                if (CanRepair)
                    return DataManager.ServiceTypes.VENDOR_REPAIR;
                else if (HasFood || HasDrink)
                    return DataManager.ServiceTypes.VENDOR_GROSSERY;
                else
                    return DataManager.ServiceTypes.VENDOR_REGULAR;
            }
        }

        internal bool HasGrossery
        {
            get { return HasFood && HasDrink; }
        }

        public VendorService() : base() { }

        public VendorService(bool can_repair, bool has_drink, bool has_food)
            : base("vendor")
        {
            CanRepair = can_repair;
            HasDrink = has_drink;
            HasFood = has_food;
        }
    }

    #endregion

    #region Waypoints


    public class WpZones : CommonTable<ZoneWp>
    {
        [XmlElement("zone")]
        public ZoneWp[] ZoneList
        {
            get { return Items; }
            set { Items = value; }
        }

        public WpZones() : base() { }

        public ZoneWp FindZoneWpByName(string name)
        {
            return FindItemByName(name);
        }
    }

    public class ZoneWp : CommonNameList<Vector3D>
    {
        [XmlElement("waypoint")]
        public Vector3D[] VectorList
        {
            get { return Items; }
            set { Items = value; }
        }

        public ZoneWp() { }
        public ZoneWp(string name) : base(name) { }
        public ZoneWp(string name, Vector3D v)
            : this(name)
        {
            List.Add(v);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            ZoneWp z = (ZoneWp)obj;
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

        public override int GetHashCode()
        {
            return base.GetHashCode();
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

        [XmlElement("wp_list")]
        public WpZones Waypoints;

        internal virtual string FullName
        {
            get { return Name; }
        }

        internal bool Finished = false;

        internal virtual bool HasQty
        {
            get { return false; }
        }

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
    public abstract class AbstractQtyQuestObjective : AbstractQuestObjective
    {
        [XmlAttribute("qty")]
        public int ReqQty;

        internal readonly int BagQty = 0;

        internal override bool HasQty
        {
            get { return true; }
        }

        internal string ItemName
        {
            get { return Name; }
            set { Name = value; }
        }

        internal virtual string FullName
        {
            get { return ItemName + ": 0/" + ReqQty; }
        }

        public AbstractQtyQuestObjective(string stype)
            : base(stype) { }

        public AbstractQtyQuestObjective(string type, string item_str, bool is_finished)
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
    public class ItemQuestObjective : AbstractQtyQuestObjective
    {
        public ItemQuestObjective()
            : base("item") {}

        public ItemQuestObjective(string text, bool is_finished)
            : base("item", text, is_finished) { }
    }
    
    /// <summary>
    /// Class for quest objectives that requires slaying a number of NPCs
    /// </summary>
    public class MonsterQuestObjective : AbstractQtyQuestObjective
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

    #region Route

    [XmlRoot("route_list")]
    public class RouteList : CommonVersionTable<RouteListVersion>  {}

    public class RouteListVersion : CommonNameTable<Route>
    {
        [XmlElement("route")]
        public Route[] Routes
        {
            get { return Items; }
            set { Items = value; }
        }
    }

    /// <summary>
    /// Type of route Endpoints
    /// </summary>
    public enum EndpointTypes : byte
    {
        UNDEF = 0,
        GAME_OBJ = 1,
        QUEST_OBJ = 2,
        HOT_SPOT = 3,
        GRAVEYARD = 4
    }

    /// <summary>
    /// Route Endpoint class
    /// </summary>
    [XmlInclude(typeof(GameObjEndpoint))]
    [XmlInclude(typeof(QuestItemEndpoint))]
    public class Endpoint
    {
        [XmlAttribute("type")]
        public string TypeStr
        {
            get { return Enum.GetName(typeof(EndpointTypes), PType).ToLower(); }
            set { PType = DataManager.EndpointsSet[value]; }
        }

        [XmlAttribute("zone")]
        public string ZoneText;

        [XmlElement("waypoint")]
        public Vector3D Waypoint;

        internal EndpointTypes PType;

        internal virtual string Descr
        {
            get { return Enum.GetName(typeof(EndpointTypes), PType); }
        }

        public Endpoint() { }

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="type">Endpoint type. See EndpointTypes</param>
        /// <param name="zone_text">Endpoint zone name</param>
        public Endpoint(EndpointTypes type, string zone_text, Vector3D waypoint)
        {
            PType = type;
            ZoneText = zone_text;
            Waypoint = waypoint;
        }

        public override bool  Equals(object obj)
        {
            if (obj == null)
                return false;

            Endpoint e = (Endpoint) obj;

            return (PType == e.PType) && 
                   (ZoneText == e.ZoneText);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return Descr;
        }

    }

    public class GameObjEndpoint : Endpoint
    {
        [XmlAttribute("game_obj_name")]
        public string GameObjName;

        public GameObjEndpoint() : base() { }

        public GameObjEndpoint(EndpointTypes type, string name, string zone_text, Vector3D waypoint)
            : base(EndpointTypes.GAME_OBJ, zone_text, waypoint)
        {
            GameObjName = name;
        }

        internal override string Descr
        {
            get { return GameObjName.ToString().ToLower().Replace(' ', '_'); }
        }

        public override bool Equals(object obj)
        {
            if (!base.Equals(obj))
                return false;

            return GameObjName.Equals(((GameObjEndpoint)obj).GameObjName);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public class QuestItemEndpoint : Endpoint
    {
        [XmlAttribute("quest_id")]
        public int QuestId;

        [XmlAttribute("item_name")]
        public string ItemName;

        internal override string Descr
        {
            get
            {
                return QuestId + "#" + ItemName.ToString().ToLower().Replace(' ', '_'); ;
            }
        }

        public QuestItemEndpoint() : base() { }

        public QuestItemEndpoint(EndpointTypes type,
                        int quest_id, string item_name, string zone_text, Vector3D waypoint)
            : base(EndpointTypes.QUEST_OBJ, zone_text, waypoint)
        {
            QuestId = quest_id;
            ItemName = item_name;
        }

        public override bool Equals(object obj)
        {
            if (!base.Equals(obj))
                return false;

            QuestItemEndpoint qie = (QuestItemEndpoint)obj;
            return (QuestId == qie.QuestId) &&
                ItemName.Equals(qie.ItemName);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    /// <summary>
    /// Route Class.
    /// Set of pre-recordered waypoints for route from A to B
    /// </summary>
    [XmlRoot("route")]
    public class Route : CommonItem
    {
        /// <summary>
        /// Starting point
        /// </summary>
        [XmlElement("point_a")]
        public Endpoint PointA;

        /// <summary>
        /// Ending point
        /// </summary>
        [XmlElement("point_b")]
        public Endpoint PointB;

        /// <summary>
        /// Route Description
        /// </summary>
        [XmlElement("descr")]
        public string Description;

        /// <summary>
        /// Name of external file used for saving/loading waypoints
        /// </summary>
        public string WaypointFileName
        {
            get { return Name; }
        }

        /// <summary>
        /// Is route can be reversed (i.e go from B to A)
        /// In most cases yes unless toon jumps down
        /// </summary>
        [XmlAttribute("reversible")]
        public bool Reversible;

        /// <summary>
        /// Route index in case route already exists between same endpoints
        /// </summary>
        [XmlAttribute("idx")]
        public int idx = 0;

        /// <summary>
        /// List of waypoints. Used for export.
        /// </summary>
        [XmlElement("waypoints")]
        public Waypoints WpList = null;

        internal string FileName;
        
        internal Endpoint this[char idx]
        {
            get 
            { 
                if (idx.Equals('a') || idx.Equals("A"))
                    return PointA;
                else if (idx.Equals('b') || idx.Equals("B"))
                    return PointB;
                else
                    throw new Exception("Unknown Endpoint " + idx);
            }
        }

        /// <summary>
        /// Class contstructor
        /// </summary>
        public Route() : base() { }

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="point_a">Starting point A</param>
        /// <param name="point_b">Ending point B</param>
        /// <param name="descr">Route description</param>
        /// <param name="reversible">Reversible flag</param>
        public Route(Endpoint point_a, Endpoint point_b, 
                                string descr, bool reversible)
        {
            PointA = point_a;
            PointB = point_b;
            Description = descr;
            Reversible = reversible;
        }

        /// <summary>
        /// Make waypoint file name
        /// </summary>
        /// <returns>Waypoint file name based on System.Guid function</returns>
        public string MakeWaypointFileName()
        {
            byte[] guid = Guid.NewGuid().ToByteArray();
            Name = Convert.ToString(guid[0], 16).ToUpper();
            for (int i = 1; i < 8; i++)
                Name += Convert.ToString(guid[i], 16).ToUpper();
            return Name;
        }

        /// <summary>
        /// Make File Name based on PointA and PointB types
        /// </summary>
        /// <returns>Route file name based on PointA and PointB types.
        /// Null if PointA or PointB are undef</returns>
        public string MakeFileName()
        {
            if ((PointA.PType == EndpointTypes.UNDEF) ||
                    (PointB.PType == EndpointTypes.UNDEF))
                return null;

            return PointA.Descr + "-" + PointB.Descr;
        }

        public void DoBeforeExport(string version, Waypoints wp)
        {
            base.DoBeforeExport(version);
            WpList = wp;
        }

        public override void DoAfterExport()
        {
            base.DoAfterExport();
            WpList = null;
        }
    }

    /// <summary>
    /// List of waypoints
    /// </summary>
    [XmlRoot("waypoints")]
    public class Waypoints : CommonNameList<Vector3D>
    {
        [XmlElement("waypoint")]
        public Vector3D[] WpList
        {
            get { return Items; }
            set { Items = value; }
        }

        public Waypoints() : base() { }

        public Waypoints(string name) : base(name) { }
    }

    #endregion
}
