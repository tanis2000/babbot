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
using System.Text.RegularExpressions;

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

        [XmlElement("app_config")]
        public AppConfig AppConfig;

        public WoWVersion FindVersion(string version)
        {
            return (WoWVersion) FindItemByName(version);
        }
    }
    
    public class WoWVersion : CommonItem
    {
        [XmlAttribute("max_lvl")]
        public int MaxLvl;

        [XmlElement("lua")]
        public LuaProc LuaList;

        [XmlElement("talents")]
        public TalentConfig TalentConfig;

        [XmlElement("quests")]
        public QuestConfig QuestConfig;

        [XmlElement("classes")]
        public CharClasses Classes;

        [XmlElement("races")]
        public Races Races;

        [XmlElement("continents")]
        public ContinentList Continents;

        [XmlElement("globals")]
        public GlobalOffsets Globals;

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
        [XmlAttribute("fnew_pattern")]
        public string FNewPattern;

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

    public class LuaFunction : CommonText
    {
        [XmlElement("return")]
        public LuaResult FRet;

        [XmlIgnore]
        public int RetSize
        {
            get { return (FRet == null) ? 0 : FRet.Size; }
        }

        [XmlIgnore]
        public string Code
        {
            get { return TextData; }
        }

        // public LuaFunction() { }
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

    #region Races

    public class Races : CommonTable<Race> {
        [XmlElement("race")]
        public Race[] RaceList
        {
            get { return Items; }
            set { Items = value; }
        }

        public Race FindRaceByName(string name)
        {
            return FindItemByName(name);
        } 
    }

    public class Race : CommonItem
    {
        [XmlAttribute("long_name")]
        public string LongName;

        [XmlAttribute("id")]
        public int Id;
    }

    #endregion

    #region Configurations

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

    public class QuestConfig
    {
        [XmlIgnore]
        public Regex HeaderRx;

        [XmlIgnore]
        public Regex InfoRx;

        [XmlIgnore]
        public Regex DetailRx;

        [XmlIgnore]
        public Regex[] Patterns {
            get { return new Regex[3] { HeaderRx, InfoRx, DetailRx }; }
        }

        [XmlAttribute("header_pattern")]
        public string HeaderPattern {
            get { return HeaderRx.ToString(); }
            set { HeaderRx = new Regex(value, RegexOptions.Singleline); }
        }

        [XmlAttribute("info_pattern")]
        public string InfoPattern
        {
            get { return InfoRx.ToString(); }
            set { InfoRx = new Regex(value); }
        }

        [XmlAttribute("detail_pattern")]
        public string DetailPattern
        {
            get { return DetailRx.ToString(); }
            set { DetailRx = new Regex(value); }
        }

        public QuestConfig() { }
    }

    public class AppConfig
    {
        [XmlAttribute("min_refresh_time")]
        public int MinBotRefreshTime;

        AppConfig() { }
    }

    #endregion

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

    #region Globals

    public class GlobalOffsets
    {
        [XmlAttribute("game_offset")]
        public uint GameOffset;
        [XmlAttribute("move_mouse_over_guid_offset")]
        public uint MouseOverGuidOffset;
        [XmlAttribute("name_store_pointer")]
        public uint NameStorePointer;
        [XmlAttribute("player_base_offset1")]
        public uint PlayerBaseOffset1;
        [XmlAttribute("player_base_offset2")]
        public uint PlayerBaseOffset2;
        [XmlAttribute("player_cur_target_guil_offset")]
        public uint PlayerCurTargetGuidOffset;
        [XmlAttribute("player_rotation_offset")]
        public uint PlayerRotationOffset;
        [XmlAttribute("player_x_offset")]
        public uint PlayerXOffset;
        [XmlAttribute("player_y_offset")]
        public uint PlayerYOffset;
        [XmlAttribute("player_z_offset")]
        public uint PlayerZOffset;
        [XmlAttribute("local_player_corpse_offset")]
        public uint LocalPlayerCorpseOffset;
        [XmlAttribute("camera_offset")]
        public uint cameraOffset;
        [XmlAttribute("camera_pointer")]
        public uint cameraPointer;
        [XmlAttribute("descriptor_offset")]
        public uint DescriptorOffset;
        [XmlAttribute("first_object")]
        public uint FirstObject;
        [XmlAttribute("guid_offset")]
        public uint GuidOffset;
        [XmlAttribute("local_guid_offset")]
        public uint LocalGuidOffset;
        [XmlAttribute("next_object")]
        public uint NextObject;
        [XmlAttribute("type_offset")]
        public uint TypeOffset;
        [XmlAttribute("first_buff")]
        public uint FirstBuff;
        [XmlAttribute("next_buff")]
        public uint NextBuff;
        [XmlAttribute("unit_name_base_offset1")]
        public uint UnitNameBaseOffset1;
        [XmlAttribute("unit_name_base_offset2")]
        public uint UnitNameBaseOffset2;
        [XmlAttribute("unit_name_len")]
        public int UnitNameLen;
        [XmlAttribute("click_to_move_base")]
        public uint ClickToMoveBase;
        [XmlAttribute("click_to_move_unknown")]
        public uint ClickToMoveUnknown;
        [XmlAttribute("click_to_move_turn_scale")]
        public uint ClickToMoveTurnScale;
        [XmlAttribute("click_to_move_unknown_2")]
        public uint ClickToMoveUnknown2;
        [XmlAttribute("click_to_move_interact_distance")]
        public uint ClickToMoveInteractDistance;
        [XmlAttribute("click_to_move_action_type")]
        public uint ClickToMoveActionType;
        [XmlAttribute("click_to_move_target")]
        public uint ClickToMoveTarget;
        [XmlAttribute("click_to_move_dest_x")]
        public uint ClickToMoveDestX;
        [XmlAttribute("click_to_move_dest_y")]
        public uint ClickToMoveDestY;
        [XmlAttribute("click_to_move_dest_z")]
        public uint ClickToMoveDestZ;
    }

    #endregion
}
