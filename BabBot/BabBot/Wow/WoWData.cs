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
using System.Collections.Generic;

// TODO Add alliance and assign alliance to races
// Define player alliance based on race

namespace BabBot.Wow
{
    [XmlRoot("wow_data")]
    public class WoWData : CommonSortedTable<WoWVersion>
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

    public class WoWVersion : CommonMergeListItem
    {
        [XmlAttribute("max_lvl")]
        public int MaxLvl;

        [XmlElement("talent_config")]
        public TalentConfig TalentConfig;

        [XmlElement("quest_config")]
        public QuestConfig QuestConfig;

        // Mergeable elements start
        // Must be not-null and included into MergeList

        [XmlElement("classes")]
        public CharClasses Classes
        {
            get { return (CharClasses) MergeList[0]; }
            set { MergeList[0] = value; }
        }

        [XmlElement("races")]
        public Races Races
        {
            get { return (Races)MergeList[1]; }
            set { MergeList[1] = value; }
        }

        [XmlElement("continents")]
        public Continents Continents
        {
            get { return (Continents)MergeList[2]; }
            set { MergeList[2] = value; }
        }

        [XmlElement("lua")]
        public LuaProc LuaList
        {
            get { return (LuaProc)MergeList[3]; }
            set { MergeList[3] = value; }
        }

        internal string Build
        {
            get { return Name; }
        }
        // Mergeable elements end

        // Globals unique for each version and not include into merge
        [XmlElement("globals")]
        public GlobalOffsets Globals;

        [XmlIgnore]
        public GameDataVersion GameObjData;

        public WoWVersion() 
            : base()
        {

            MergeList = new IMergeable[4];
        }

        public LuaFunction FindLuaFunction(string name)
        {
            return LuaList.FindLuaFunction(name);
        }

        public override void MergeWith(object obj)
        {
            if (!MergeHelper.IsMergeable(this, obj))
                return;

            WoWVersion pver = (WoWVersion) obj;

            // Manually check confuration elements
            TalentConfig = TalentConfig ?? pver.TalentConfig;
            QuestConfig = QuestConfig ?? pver.QuestConfig;

            base.MergeWith(pver);
        }

        
        public override int CompareTo(object obj)
        {
            if (obj == null)
                return -1;

            string[] v1 = Build.Split('.');
            string[] v2 = ((WoWVersion)obj).Build.Split('.');

            int num = Math.Min(v1.Length, v2.Length);
            for (int i = 0; i < num; i++)
            {
                int i1 = Convert.ToInt32(v1);
                int i2 = Convert.ToInt32(v2);

                if (i1 < i2)
                    return -1;
                else if (i1 > i2)
                    return 1;
            }

            return 0;
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
        public LuaFunctionHelper FRet;

        [XmlElement("parameters")]
        public LuaFunctionHelper FParams;

        [XmlElement("description")]
        public string Description;

        [XmlIgnore]
        public int RetSize
        {
            get { return (FRet == null) ? 0 : FRet.Size; }
        }

        [XmlIgnore]
        public int ParamSize
        {
            get { return (FParams == null) ? 0 : FParams.Size; }
        }

        [XmlIgnore]
        public string Code
        {
            get { return TextData; }
        }

        // public LuaFunction() { }
    }

    public class LuaFunctionHelper
    {
        [XmlAttribute("size")]
        public int Size;

        public LuaFunctionHelper() { }
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
    
    /// <summary>
    /// Class with Class Description
    /// </summary>
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
        internal Regex HeaderRx;
        internal Regex InfoRx;
        internal Regex DetailRx;
        internal Regex ObjectiveRx;

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

        [XmlAttribute("objective_pattern")]
        public string ObjectivePattern
        {
            get { return ObjectiveRx.ToString(); }
            set { ObjectiveRx = new Regex(value); }
        }

        public QuestConfig() { }
    }

    /// <summary>
    /// Class for Global App Configuration that not included into Config
    /// </summary>
    public class AppConfig
    {
        [XmlAttribute("min_refresh_time")]
        public int MinBotRefreshTime;

        [XmlAttribute("max_get_target_retries")]
        public int MaxTargetGetRetries;

        [XmlAttribute("max_npc_interact_time")]
        public int MaxNpcInteractTime;

        internal string MaxNpcInteractSec
        {
            get { return Convert.ToString(
                (int) (MaxNpcInteractTime/1000) + " sec."); }
        }

        AppConfig() { }
    }

    #endregion

    #region Continets

    /// <summary>
    /// Class with list of continents
    /// </summary>
    public class Continents : CommonTable<Continent>
    {
        [XmlElement("continent")]
        public Continent[] ContinentList
        {
            get { return Items; }
            set { Items = value; }
        }

        public Continent FindContinentById(int id)
        {
            return FindItemByName(id.ToString());
        }

        public string FindContinentNameById(int id)
        {
            Continent c = FindContinentById(id);
            return (c != null) ? c.Name : null;
        }
    }

    /// <summary>
    /// Class with Continent Description
    /// </summary>
    public class Continent : CommonList<Zone>
    {
        [XmlAttribute("id")]
        public int Id;

        [XmlAttribute("name")]
        public string Name;

        [XmlElement("zone")]
        public Zone[] ZoneList
        {
            get { return Items; }
            set { Items = value; }
        }

        public Continent() : base(true) { }

        public override string ToString()
        {
            return Id.ToString();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj) && 
                ((Continent) obj).Id.Equals(Id) &&
                    ((Continent)obj).Name.Equals(Name);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public class Zone : CommonItem {}

    #endregion

    #region Globals

    public class GlobalOffsets
    {
        [XmlAttribute("game_offset")]
        public string StrGameOffset
        {
            get { return GameOffset.ToString(); }
            set { GameOffset = Convert.ToUInt32(value, 16); }
        }
        internal uint GameOffset;

        [XmlAttribute("move_mouse_over_guid_offset")]
        public string StrMouseOverGuidOffset
        {
            get { return MouseOverGuidOffset.ToString(); }
            set { MouseOverGuidOffset = Convert.ToUInt32(value, 16); }
        }
        internal uint MouseOverGuidOffset;

        [XmlAttribute("name_store_pointer")]
        public string StrNameStorePointer
        {
            get { return NameStorePointer.ToString(); }
            set { NameStorePointer = Convert.ToUInt32(value, 16); }
        }
        internal uint NameStorePointer;

        [XmlAttribute("player_base_offset1")]
        public string StrPlayerBaseOffset1
        {
            get { return PlayerBaseOffset1.ToString(); }
            set { PlayerBaseOffset1 = Convert.ToUInt32(value, 16); }
        }
        internal uint PlayerBaseOffset1;

        [XmlAttribute("player_base_offset2")]
        public string StrPlayerBaseOffset2
        {
            get { return PlayerBaseOffset2.ToString(); }
            set { PlayerBaseOffset2 = Convert.ToUInt32(value, 16); }
        }
        internal uint PlayerBaseOffset2;

        [XmlAttribute("player_cur_target_guil_offset")]
        public string StrPlayerCurTargetGuidOffset
        {
            get { return PlayerCurTargetGuidOffset.ToString(); }
            set { PlayerCurTargetGuidOffset = Convert.ToUInt32(value, 16); }
        }
        internal uint PlayerCurTargetGuidOffset;

        [XmlAttribute("player_rotation_offset")]
        public string StrPlayerRotationOffset
        {
            get { return PlayerRotationOffset.ToString(); }
            set { PlayerRotationOffset = Convert.ToUInt32(value, 16); }
        }
        internal uint PlayerRotationOffset;

        [XmlAttribute("player_x_offset")]
        public string StrPlayerXOffset
        {
            get { return PlayerXOffset.ToString(); }
            set { PlayerXOffset = Convert.ToUInt32(value, 16); }
        }
        internal uint PlayerXOffset;

        [XmlAttribute("player_y_offset")]
        public string StrPlayerYOffset
        {
            get { return PlayerYOffset.ToString(); }
            set { PlayerYOffset = Convert.ToUInt32(value, 16); }
        }
        internal uint PlayerYOffset;

        [XmlAttribute("player_z_offset")]
        public string StrPlayerZOffset
        {
            get { return PlayerZOffset.ToString(); }
            set { PlayerZOffset = Convert.ToUInt32(value, 16); }
        }
        internal uint PlayerZOffset;

        [XmlAttribute("local_player_corpse_offset")]
        public string StrLocalPlayerCorpseOffset
        {
            get { return LocalPlayerCorpseOffset.ToString(); }
            set { LocalPlayerCorpseOffset = Convert.ToUInt32(value, 16); }
        }
        internal uint LocalPlayerCorpseOffset;

        [XmlAttribute("camera_offset")]
        public string StrCameraOffset
        {
            get { return CameraOffset.ToString(); }
            set { CameraOffset = Convert.ToUInt32(value, 16); }
        }
        internal uint CameraOffset;

        [XmlAttribute("camera_pointer")]
        public string StrCameraPointer
        {
            get { return CameraPointer.ToString(); }
            set { CameraPointer = Convert.ToUInt32(value, 16); }
        }
        internal uint CameraPointer;

        [XmlAttribute("descriptor_offset")]
        public string StrDescriptorOffset
        {
            get { return DescriptorOffset.ToString(); }
            set { DescriptorOffset = Convert.ToUInt32(value, 16); }
        }
        internal uint DescriptorOffset;

        [XmlAttribute("first_object")]
        public string StrFirstObject
        {
            get { return FirstObject.ToString(); }
            set { FirstObject = Convert.ToUInt32(value, 16); }
        }
        internal uint FirstObject;

        [XmlAttribute("guid_offset")]
        public string StrGuidOffset
        {
            get { return GuidOffset.ToString(); }
            set { GuidOffset = Convert.ToUInt32(value, 16); }
        }
        internal uint GuidOffset;

        [XmlAttribute("local_guid_offset")]
        public string StrLocalGuidOffset
        {
            get { return LocalGuidOffset.ToString(); }
            set { LocalGuidOffset = Convert.ToUInt32(value, 16); }
        }
        internal uint LocalGuidOffset;

        [XmlAttribute("next_object")]
        public string StrNextObject
        {
            get { return NextObject.ToString(); }
            set { NextObject = Convert.ToUInt32(value, 16); }
        }
        internal uint NextObject;

        [XmlAttribute("type_offset")]
        public string StrTypeOffset
        {
            get { return TypeOffset.ToString(); }
            set { TypeOffset = Convert.ToUInt32(value, 16); }
        }
        internal uint TypeOffset;

        [XmlAttribute("first_buff")]
        public string StrFirstBuff
        {
            get { return FirstBuff.ToString(); }
            set { FirstBuff = Convert.ToUInt32(value, 16); }
        }
        internal uint FirstBuff;

        [XmlAttribute("next_buff")]
        public string StrNextBuff
        {
            get { return NextBuff.ToString(); }
            set { NextBuff = Convert.ToUInt32(value, 16); }
        }
        internal uint NextBuff;

        [XmlAttribute("unit_name_base_offset1")]
        public string StrUnitNameBaseOffset1
        {
            get { return UnitNameBaseOffset1.ToString(); }
            set { UnitNameBaseOffset1 = Convert.ToUInt32(value, 16); }
        }
        internal uint UnitNameBaseOffset1;

        [XmlAttribute("unit_name_base_offset2")]
        public string StrUnitNameBaseOffset2
        {
            get { return UnitNameBaseOffset2.ToString(); }
            set { UnitNameBaseOffset2 = Convert.ToUInt32(value, 16); }
        }
        internal uint UnitNameBaseOffset2;

        [XmlAttribute("unit_name_len")]
        public string StrUnitNameLen
        {
            get { return UnitNameLen.ToString(); }
            set { UnitNameLen = Convert.ToInt32(value, 16); }
        }
        internal int UnitNameLen;

        [XmlAttribute("click_to_move_base")]
        public string StrClickToMoveBase
        {
            get { return ClickToMoveBase.ToString(); }
            set { ClickToMoveBase = Convert.ToUInt32(value, 16); }
        }
        internal uint ClickToMoveBase;

        [XmlAttribute("click_to_move_unknown")]
        public string StrClickToMoveUnknown
        {
            get { return ClickToMoveUnknown.ToString(); }
            set { ClickToMoveUnknown = 
                        ClickToMoveBase + Convert.ToUInt32(value, 16); }
        }
        internal uint ClickToMoveUnknown;

        [XmlAttribute("click_to_move_turn_scale")]
        public string StrClickToMoveTurnScale
        {
            get { return ClickToMoveTurnScale.ToString(); }
            set { ClickToMoveTurnScale = 
                        ClickToMoveBase + Convert.ToUInt32(value, 16); }
        }
        internal uint ClickToMoveTurnScale;

        [XmlAttribute("click_to_move_unknown_2")]
        public string StrClickToMoveUnknown2
        {
            get { return ClickToMoveUnknown2.ToString(); }
            set { ClickToMoveUnknown2 = 
                        ClickToMoveBase + Convert.ToUInt32(value, 16); }
        }
        internal uint ClickToMoveUnknown2;

        [XmlAttribute("click_to_move_interact_distance")]
        public string StrClickToMoveInteractDistance
        {
            get { return ClickToMoveInteractDistance.ToString(); }
            set { ClickToMoveInteractDistance = 
                        ClickToMoveBase + Convert.ToUInt32(value, 16); }
        }
        internal uint ClickToMoveInteractDistance;

        [XmlAttribute("click_to_move_action_type")]
        public string StrClickToMoveActionType
        {
            get { return ClickToMoveActionType.ToString(); }
            set { ClickToMoveActionType = 
                        ClickToMoveBase + Convert.ToUInt32(value, 16); }
        }
        internal uint ClickToMoveActionType;

        [XmlAttribute("click_to_move_target")]
        public string StrClickToMoveTarget
        {
            get { return ClickToMoveTarget.ToString(); }
            set { ClickToMoveTarget =
                        ClickToMoveBase + Convert.ToUInt32(value, 16);
            }
        }
        internal uint ClickToMoveTarget;

        [XmlAttribute("click_to_move_dest_x")]
        public string StrClickToMoveDestX
        {
            get { return ClickToMoveDestX.ToString(); }
            set { ClickToMoveDestX =
                        ClickToMoveBase + Convert.ToUInt32(value, 16);
            }
        }
        internal uint ClickToMoveDestX;

        [XmlAttribute("click_to_move_dest_y")]
        public string StrClickToMoveDestY
        {
            get { return ClickToMoveDestY.ToString(); }
            set { ClickToMoveDestY =
                        ClickToMoveBase + Convert.ToUInt32(value, 16);
            }
        }
        internal uint ClickToMoveDestY;

        [XmlAttribute("click_to_move_dest_z")]
        public string StrClickToMoveDestZ
        {
            get { return ClickToMoveDestZ.ToString(); }
            set { ClickToMoveDestZ =
                        ClickToMoveBase + Convert.ToUInt32(value, 16);
            }
        }
        internal uint ClickToMoveDestZ;

        [XmlAttribute("lua_dostring")]
        public string StrLuaDoString
        {
            get { return LuaDelegates["lua_dostring"].ToString(); }
            set
            {
                LuaDelegates.Add("lua_dostring", 
                            Convert.ToUInt32(value, 16));
            }
        }

        [XmlAttribute("lua_getstate")]
        public string StrLuaGetState
        {
            get { return LuaDelegates["lua_getstate"].ToString(); }
            set
            {
                LuaDelegates.Add("lua_getstate", 
                            Convert.ToUInt32(value, 16));
            }
        }

        [XmlAttribute("lua_gettop")]
        public string StrLuaGetTop
        {
            get { return LuaDelegates["lua_gettop"].ToString(); }
            set
            {
                LuaDelegates.Add("lua_gettop", 
                            Convert.ToUInt32(value, 16));
            }
        }

        [XmlAttribute("lua_register")]
        public string StrLuaRegister
        {
            get { return LuaDelegates["lua_register"].ToString(); }
            set
            {
                LuaDelegates.Add("lua_register", 
                            Convert.ToUInt32(value, 16));
            }
        }

        [XmlAttribute("lua_tostring")]
        public string StrLuaToString
        {
            get { return LuaDelegates["lua_tostring"].ToString(); }
            set
            {
                LuaDelegates.Add("lua_tostring", 
                            Convert.ToUInt32(value, 16));
            }
        }

        [XmlAttribute("lua_getcurrentmapzone")]
        public string StrLuaGetCurrentMapZone
        {
            get { return LuaDelegates["lua_getcurrentmapzone"].ToString(); }
            set
            {
                LuaDelegates.Add("lua_getcurrentmapzone", 
                            Convert.ToUInt32(value, 16));
            }
        }

        internal Dictionary<string, uint> LuaDelegates =
                                new Dictionary<string, uint>();

    }

    #endregion
}
