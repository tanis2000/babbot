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
    [XmlRoot("wow_data")]
    public class WoWData : CommonTable<WoWVersion>
    {
        [XmlElement("version")]
        public WoWVersion[] Versions
        {
            get { return Items; }
            set { Items = value; }
        }

        public WoWVersion FindVersion(string version)
        {
            return (WoWVersion) FindItemByName(version);
        }
    }
    
    [Serializable]
    public class WoWVersion : CommonItem
    {
        [XmlAttribute("max_lvl")]
        public int MaxLvl;

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

        public LuaFunction FindLuaFunction(string name)
        {
            return LuaList.FindLuaFunction(name);
        }
    }

    #region Lua Function

    public class LuaProc : CommonTable<LuaFunction>
    {
        [XmlElement("function")]
        public LuaFunction[] FList
        {
            get { return Items; }
            set { Items = value; }
        }

        public LuaFunction FindLuaFunction(string name)
        {
            return (LuaFunction)FindItemByName(name);
        }
    }

    public class LuaFunction : CommonItem
    {
        [XmlElement("text", typeof(XmlCDataSection))]
        public XmlCDataSection Text;

        [XmlElement("return")]
        public LuaResult FRet;

        public LuaFunction() { }

        [XmlIgnore]
        public int RetSize
        {
            get { return (FRet == null) ? 0 : FRet.Size; }
        }

        public LuaFunction(string name, string text) :
            base(name)
        {
            XmlDocument doc = new XmlDocument();
            Text = doc.CreateCDataSection(text);
        }

        [XmlIgnore]
        public string Code
        {
            get { return ((Text != null) ? Text.InnerText : null); }
        }
    }

    public class LuaResult
    {
        [XmlAttribute("size")]
        public int Size;

        public LuaResult() { }
    }

    #endregion

    #region Char Class

    public class CharClasses : CommonTable<CharClass>
    {
        [XmlElement("class")]
        public CharClass[] ClassList
        {
            get { return Items; }
            set { Items = value; }
        }

        public CharClass FindClassByName(string name)
        {
            return FindItemByName(name);
        }

        public CharClass FindClassByArmoryId(byte id)
        {
            CharClass res = null;
            foreach (CharClass c in Table.Values)
                if (c.ArmoryId == id)
                {
                    res = c;
                    break;
                }
            return res;
        }
    }
    
    public class CharClass : CommonItem
    {
        [XmlAttribute("armory_id")]
        public byte ArmoryId { get; set; }

        [XmlAttribute("long_name")]
        public string LongName { get; set; }

        [XmlAttribute("tab_1_max")]
        public byte TabMax1 { get; set; }

        [XmlAttribute("tab_2_max")]
        public byte TabMax2 { get; set; }

        [XmlAttribute("tab_3_max")]
        public byte TabMax3 { get; set; }

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
    }

    #endregion

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

    #region Continets

    public class ContinentList : CommonTable<Continent>
    {
        [XmlElement("continent")]
        public Continent[] Continents
        {
            get { return Items; }
            set { Items = value; }
        }

        public Continent FindContinentById(int id)
        {
            return FindItemByName(Convert.ToString(id));
        }

        public string FindContinentNameById(int id)
        {
            Continent c = FindContinentById(id);
            return (c != null) ? c.Name : null;
        }
    }

    public class Continent : CommonItemEx
    {
        public Continent() { }
    }

    #endregion
}
