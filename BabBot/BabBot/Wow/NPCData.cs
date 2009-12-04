using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace BabBot.Wow
{
    [XmlRoot("npc_data")]
    public class NPCData
    {
        private Hashtable _versions;

        public NPCData() 
        {
            _versions = new Hashtable();
        }

        [XmlElement("version")]
        public NPCVersion[] Versions
        {
            get
            {
                NPCVersion[] res = new NPCVersion[_versions.Count];
                _versions.Values.CopyTo(res, 0);
                return res;
            }
            set
            {
                if (value == null) return;
                NPCVersion[] items = (NPCVersion[])value;
                _versions.Clear();
                foreach (NPCVersion item in items)
                    _versions.Add(item.Number, item);
            }
        }

        public NPCVersion FindVersion(string version)
        {
            return (NPCVersion)_versions[version];
        }
    }
    
    [Serializable]
    public class NPCVersion
    {
        private Hashtable _npc_list;

        [XmlAttribute("num")]
        public string Number;

        [XmlElement("npc")]
        public NPC[] NPCList
        {
            get
            {
                NPC[] res = new NPC[_npc_list.Count];
                _npc_list.Values.CopyTo(res, 0);
                return res;
            }

            set
            {
                if (value == null) return;
                NPC[] items = (NPC[])value;
                _npc_list.Clear();
                foreach (NPC item in items)
                    _npc_list.Add(item.Name, item);
            }
        }

        public NPCVersion()
        {
            _npc_list = new Hashtable();
        }

        public NPC FindNPCByName(string name)
        {
            // TODO
            return null; // NPCList.FindLuaFunction(name);
        }

        public override string ToString()
        {
            return Number;
        }

        public void AddNPC(NPC npc)
        {
            _npc_list.Add(npc.Name, npc);
        }
    }

/*
    public class LuaProc
    {
        private Hashtable _flist;

        public LuaProc() { 
            _flist = new Hashtable(); 
        }

        public LuaProc(LuaFunction[] flist)
        {
            FList = flist;
        }

        [XmlElement("function")]
        public LuaFunction[] FList
        {
            get
            {
                LuaFunction[] res = new LuaFunction[_flist.Count];
                _flist.Values.CopyTo(res, 0);
                return res;
            }

            set
            {
                if (value == null) return;
                LuaFunction[] items = (LuaFunction[])value;
                _flist.Clear();
                foreach (LuaFunction item in items)
                    _flist.Add(item.Name, item);
            }
        }

        public LuaFunction FindLuaFunction(string name)
        {
            LuaFunction res = (LuaFunction)_flist[name];
            return res;
        }
    }
*/
    public class NPC
    {
        [XmlAttribute("name")] 
        public string Name;

        [XmlAttribute("type")] 
        public string Type;

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

        [XmlIgnore]
        public int ServiceCount
        {
            get { return Services._services.Count; }
        }

        public NPC() {}

        public NPC(WowPlayer player)
        {
            WowUnit w = player.CurTarget;

            Init(w.Name, player.ContinentID, player.ZoneText, w.Location.Clone());
        }

        public void Init(string name, int continent_id, 
                                    string zone_text, object waypoint)
        {
            Name = name;
            ContinentId = continent_id;
            ZoneText = zone_text;

            WPList = new Waypoints();
            Services = new NPCServices();
            QuestList = new Quests();

            WPList.Add(waypoint);
        }

        public void AddService(NPCService service)
        {
            Services.Add(service);
        }

        public void AddQuest(QuestHeader qh)
        {
            QuestList.Add(qh);
        }
    }

    public class Waypoints
    {
        internal ArrayList _wplist;

        [XmlElement("waypoint")]
        public Vector3D[] VectorList
        {
            get
            {
                Vector3D[] res = new Vector3D[_wplist.Count];
                _wplist.CopyTo(res, 0);
                return res;
            }

            set
            {
                if (value == null) return;
                Vector3D[] items = (Vector3D[])value;
                _wplist.Clear();
                foreach (Vector3D item in items)
                    _wplist.Add(item);
            }
        }

        public Waypoints() {
            _wplist = new ArrayList();
        }

        public void Add(object wp)
        {
            _wplist.Add(wp);
        }
    }


    #region Quests

    public class Quests
    {
        internal Hashtable _quests;

        [XmlElement("quest")]
        public QuestHeader[] QuestList
        {
            get
            {
                QuestHeader[] res = new QuestHeader[_quests.Count];
                _quests.Values.CopyTo(res, 0);
                return res;
            }

            set
            {
                if (value == null) return;
                QuestHeader[] items = (QuestHeader[])value;
                _quests.Clear();
                foreach (QuestHeader item in items)
                    Add(item);
            }
        }

        public Quests() {
            _quests = new Hashtable();
        }

        internal void Add(QuestHeader qh)
        {
            _quests.Add(qh.Name, qh);
        }
    }

    public class QuestHeader
    {
        [XmlAttribute("name")]
        public string Name;

        [XmlAttribute("level")]
        public int Level;

        public QuestHeader() {}

        public QuestHeader(string name, int level) {
            Name = name;
            Level = level;
        }
    }

    #endregion

    #region NPC Services

    // Service container
    public class NPCServices
    {
        internal Hashtable _services;

        [XmlElement("service")]
        public NPCService[] ServiceList
        {
            get
            {
                NPCService[] res = new NPCService[_services.Count];
                _services.Values.CopyTo(res, 0);
                return res;
            }

            set
            {
                if (value == null) return;
                NPCService[] items = (NPCService[])value;
                _services.Clear();
                foreach (NPCService item in items)
                    _services.Add(item.SType, item);
            }
        }

        public NPCServices()
        {
            _services = new Hashtable();
        }

        internal void Add(NPCService service)
        {
            _services.Add(service.SType, service);
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
