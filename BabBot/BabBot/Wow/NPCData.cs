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

namespace BabBot.Wow
{
    #region Common

    /// <summary>
    /// Common class for collection item that has a unique name 
    /// </summary>
    public abstract class CommonItem : IComparable
    {
        [XmlAttribute("name")]
        public string Name;

        public CommonItem() { }

        public int CompareTo(object obj)
        {
            return ToString().CompareTo(((CommonItem)obj).ToString());
        }

        public override string ToString()
        {
            return Name;
        }
    }

    /// <summary>
    /// Class with internal hashtable that needs to be serialized
    /// </summary>
    /// <typeparam name="T">Type of elements in the table</typeparam>
    public class CommonTable<T>
    {
        internal readonly Hashtable _htable;
        
        [XmlIgnore]
        internal T[] Items
        {
            get
            {
                T[] res = new T[_htable.Count];
                _htable.Values.CopyTo(res, 0);
                return res;
            }
            set
            {
                if (value == null) return;
                T[] items = (T[])value;
                _htable.Clear();
                foreach (T item in items)
                    _htable.Add(item.ToString(), item);
            }
        }

        public CommonTable()
        {
            _htable = new Hashtable();
        }
        
        [XmlIgnore]
        public Hashtable Table
        {
            get { return _htable; }
        }
        
        public override bool Equals(object obj)
        {
            CommonTable<T> t = (CommonTable<T>)obj;
            
            // Check size first
            if (_htable.Count != t.Table.Count)
                return false;
                
            // Check values
            foreach (DictionaryEntry item1 in _htable)
            {
                T item2 = (T) t.Table[item1.Key];
                if ((item2 == null) || !item1.Value.Equals(item2))
                    return false;
            }
            
            // No differences found
            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public T FindItemByName(string name)
        {
            return (T) _htable[name];
        }

        public void Add(T item)
        {
            _htable.Add(item.ToString(), item);
        }

        public int ItemCount()
        {
            return _htable.Count;
        }
    }

    /// <summary>
    /// Class with internal hashtable that needs to be serialized and have a unique name
    /// </summary>
    /// <typeparam name="T">Type of elements in the table</typeparam>
    public class CommonNameTable<T> : CommonTable<T>
    {
        [XmlAttribute("name")]
        public string Name;

    }

    /// <summary>
    /// Class with internal list that needs to be serialized
    /// </summary>
    /// <typeparam name="T">Type of elements in the list</typeparam>
    public abstract class CommonList<T>
    {
        internal readonly ArrayList _list;

        public CommonList()
        {
            _list = new ArrayList();
        }

        [XmlIgnore]
        internal T[] Items
        {
            get
            {
                T[] res = new T[_list.Count];
                _list.CopyTo(res, 0);
                return res;
            }
            set
            {
                if (value == null) return;
                T[] items = (T[])value;
                _list.Clear();
                foreach (T item in items)
                    _list.Add(item);
            }
        }

        [XmlIgnore]
        public ArrayList List
        {
            get { return _list; }
        }
        
        public override bool Equals(object obj)
        {
            CommonList<T> l = (CommonList<T>)obj;
            
            // Check size first
            if (_list.Count != l.List.Count)
                return false;
                
            // Check values
            for (int i = 0; i < _list.Count; i++)
            {
                T item1 = (T) _list[i];
                T item2 = (T) l.List[i];

                if ((item2 == null) || !item1.Equals(item2))
                    return false;
            }
            
            // No differences found
            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public void Add(T item)
        {
            _list.Add(item);
        }

    }

    /// <summary>
    /// Class with internal list that needs to be serialized and have a unique name
    /// </summary>
    /// <typeparam name="T">Type of elements in the list</typeparam>
    public abstract class CommonNameList<T> : CommonList<T>
    {
        [XmlAttribute("name")]
        public string Name;
    }

    #endregion

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
            return (NPCVersion)_htable[version];
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
    }

    public class NPC : CommonItem
    {
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

        public NPC() {
            WPList = new Waypoints();
            Services = new NPCServices();
            QuestList = new Quests();
        }

        public NPC(WowPlayer player)
        {
            WowUnit w = player.CurTarget;

            Init(w.Name, player.ContinentID, 
                player.ZoneText, (Vector3D) w.Location.Clone());
        }

        public void Init(string name, int continent_id, 
                                    string zone_text, Vector3D waypoint)
        {
            Name = name;
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

            return (!(
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
                QuestList.Equals(npc.QuestList)));
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

    public class Quest : CommonItem
    {
        [XmlAttribute("level")]
        public int Level;

        public Quest() {}

        public Quest(string name, int level)
        {
            Name = name;
            Level = level;
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
