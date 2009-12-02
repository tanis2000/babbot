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

        [XmlAttribute("zone_id")] 
        public int ZoneId;

        [XmlAttribute("zone_map")] 
        public string ZoneMap;

        [XmlAttribute("class")] 
        public string Class;

        [XmlElement("services")]
        public NPCServices Services;

        [XmlIgnore]
        public int ServiceCount
        {
            get { return Services._services.Count; }
        }

        public NPC() {}

        public NPC(string name)
        {
            Name = name;
            Services = new NPCServices();
        }

        public void AddService(NPCService service)
        {
            Services._services.Add(service.SType, service);
        }
    }

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
    }

    
    public class NPCService
    {
        [XmlAttribute("type")]
        public string SType;

        [XmlAttribute("class")]
        public string CharClass;

        NPCService() { }

        public NPCService(string stype)
        {
            SType = stype;
        }
    }

    #region NPC Services

    public class TrainingService : NPCService
    {
        private string _class;

        public TrainingService(string class_name) : base("trainer")
        {
            _class = class_name;
        }
    }

    #endregion
}
