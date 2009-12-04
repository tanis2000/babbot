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
    [XmlRoot("wow_data")]
    public class WoWData
    {
        private Hashtable _versions;

        public WoWData() 
        {
            Init();
        }

        public WoWData(WoWVersion[] versions)
        {
            Init();
            Versions = versions;
        }

        private void Init()
        {
            _versions = new Hashtable();
        }

        [XmlElement("version")]
        public WoWVersion[] Versions
        {
            get {
                WoWVersion[] res = new WoWVersion[_versions.Count];
                _versions.Values.CopyTo(res,0);
                return res;
            }
            set
            {
                if (value == null) return;
                WoWVersion[] items = (WoWVersion[])value;
                _versions.Clear();
                foreach (WoWVersion item in items)
                    _versions.Add(item.Number, item);
            }
        }

        public WoWVersion FindVersion(string version)
        {
            return (WoWVersion) _versions[version];
        }
    }
    
    [Serializable]
    public class WoWVersion
    {
        private ArrayList _tlist;

        [XmlAttribute("max_lvl")]
        public int MaxLvl;

        [XmlAttribute("num")]
        public string Number;

        [XmlElement("lua")]
        public LuaProc LuaList;

        [XmlElement("talents")]
        public TalentConfig TalentConfig;

        [XmlElement("classes")]
        public CharClasses Classes;

        [XmlElement("continents")]
        public ContinentList Continents;

        [XmlIgnore]
        public NPCVersion NPCData;

        public WoWVersion()
        {
            _tlist = new ArrayList();
        }

        public LuaFunction FindLuaFunction(string name)
        {
            return LuaList.FindLuaFunction(name);
        }

        public override string ToString()
        {
            return Number;
        }
    }

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

    public class LuaFunction
    {
        [XmlAttribute("name")] 
        public string Name;

        [XmlElement("text", typeof(XmlCDataSection))]
        public XmlCDataSection Text;

        [XmlElement("return")]
        public LuaResult FRet;

        [XmlIgnore]
        public int RetSize
        {
            get { return (FRet == null) ? 0 : FRet.Size; }
        }

        public LuaFunction() {}

        public LuaFunction(string name, string text)
        {
            Name = name;
            XmlDocument doc = new XmlDocument();
            Text = doc.CreateCDataSection(text);
        }

        [XmlIgnore]
        public string Code
        {
            get { return ((Text != null) ? Text.InnerText : null); }
        }

        public override string ToString()
        {
            return Code;
        }
    }

    public class LuaResult
    {
        [XmlAttribute("size")]
        public int Size;

        public LuaResult() { }
    }

    public class CharClasses
    {
        // Sorted by Armory ID
        private Hashtable _clist;
        // Sorted by Long Name
        private SortedList _clist1;
        // Sorted by Sys Name
        private SortedList _clist2;

        [XmlElement("class")]
        public CharClass[] ClassList
        {
            get
            {
                CharClass[] res = new CharClass[_clist.Count];
                _clist.Values.CopyTo(res, 0);
                return res;
            }

            set
            {
                if (value == null) return;
                CharClass[] items = (CharClass[])value;
                _clist.Clear();
                _clist1.Clear();
                _clist2.Clear();

                foreach (CharClass item in items)
                {
                    _clist.Add(item.ArmoryId, item);
                    _clist1.Add(item.LongName, item);
                    _clist2.Add(item.SysName, item);
                }
            }
        }

        [XmlIgnore]
        public CharClass[] ClassListByName
        {
            get
            {
                CharClass[] res = new CharClass[_clist1.Count];
                _clist1.Values.CopyTo(res, 0);
                return res;
            }
        }

        public CharClasses () 
        {
            _clist = new Hashtable();
            _clist1 = new SortedList();
            _clist2 = new SortedList();
        }

        public int FindClassBySysName(string name)
        {
            return _clist2.IndexOfKey(name);
        }

        public CharClass FindClassByArmoryId(byte id)
        {
            return (CharClass) _clist[id];
        }
    }
    
    public class CharClass
    {
        [XmlAttribute("armory_id")]
        public byte ArmoryId;

        [XmlAttribute("long_name")]
        public string LongName;

        [XmlAttribute("sys_name")]
        public string SysName;

        [XmlAttribute("tab_1_max")]
        public byte TabMax1;

        [XmlAttribute("tab_2_max")]
        public byte TabMax2;

        [XmlAttribute("tab_3_max")]
        public byte TabMax3;

        [XmlIgnore]
        public byte[] Tabs
        {
            get { return new byte[] {TabMax1, TabMax2, TabMax3}; }
        }

        [XmlIgnore]
        public int TotalTalentSum
        {
            get { return TabMax1 + TabMax2 + TabMax3; }
        }

        public CharClass () {}

        public override string ToString()
        {
            return LongName;
        }
    }
    
    public class TalentConfig
    {
        [XmlAttribute("lvl_start")]
        public byte StartLevel;

        [XmlAttribute("armory_pattern")]
        public string ArmoryPattern;

        [XmlAttribute("delay")]
        public int Delay;

        [XmlAttribute("retry")]
        public int Retry;

        public TalentConfig() { }
    }

    public class ContinentList
    {
        private Hashtable _list;

        [XmlElement("continent")]
        public Continent[] Continents
        {
            get
            {
                Continent[] res = new Continent[_list.Count];
                _list.Values.CopyTo(res, 0);
                return res;
            }

            set
            {
                if (value == null) return;
                Continent[] items = (Continent[])value;
                _list.Clear();

                foreach (Continent item in items)
                {
                    _list.Add(item.ID, item);
                }
            }
        }

        public ContinentList()
        {
            _list = new Hashtable();
        }

        public Continent FindContinentById(int id)
        {
            return (Continent)_list[id];
        }

        public string FindContinentNameById(int id)
        {
            Continent c = FindContinentById(id);
            return (c != null) ? c.Name : null;
        }
    }

    public class Continent
    {
        [XmlAttribute("id")]
        public int ID;

        [XmlAttribute("name")]
        public string Name;

        public Continent() { }
    }
}
