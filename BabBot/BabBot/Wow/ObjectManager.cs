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
using System.Collections.Generic;
using BabBot.Manager;
using BabBot.Common;

namespace BabBot.Wow
{
    /// <summary>
    /// This class contains all of the properties and methods used to access WoW's internal
    /// linked lists of objects.
    /// </summary>
    public class ObjectManager
    {
        private readonly uint CurMgr;
        private readonly ulong LocalGUID;
        

        public ObjectManager()
        {
            CurMgr = Globals.CurMgr; // Warning! This must have been initialized (aka logged in and in game)
            LocalGUID = ProcessManager.WowProcess.ReadUInt64(CurMgr + 
                                    ProcessManager.GlobalOffsets.LocalGuidOffset);
        }

        public uint GetFirstObject()
        {
            return ProcessManager.WowProcess.ReadUInt(CurMgr + 
                                    ProcessManager.GlobalOffsets.FirstObject);
        }

        public uint GetNextObject(uint CurrentObject)
        {
            return ProcessManager.WowProcess.ReadUInt(CurrentObject + 
                                    ProcessManager.GlobalOffsets.NextObject);
        }

        public uint GetObjectByGUID(ulong GUID)
        {
            uint holder = GetFirstObject();

            while ((holder != 0) && ((holder & 1) == 0))
            {
                if (ProcessManager.WowProcess.ReadUInt64(holder + 
                    ProcessManager.GlobalOffsets.GuidOffset) == GUID)
                        return holder;

                uint temp = GetNextObject(holder);
                if ((temp == 0) || (temp == holder))
                    break;

                holder = temp;
            }
            return 0;
        }

        public ulong GetLocalGUID()
        {
            return LocalGUID;
        }

        public uint GetLocalPlayerObject()
        {
            return GetObjectByGUID(LocalGUID);
        }

        public ulong GetGUIDByObject(uint Object)
        {
            return ProcessManager.WowProcess.ReadUInt64(Object + 
                                ProcessManager.GlobalOffsets.GuidOffset);
        }

        public List<WowObject> GetAllObjectsAroundLocalPlayer()
        {
            List<WowObject> list = new List<WowObject>();
            WowObject wo = null;
            uint curObject = GetFirstObject();
            try
            {
                while ((curObject != 0) && ((curObject & 1) == 0))
                {
                    wo = WowObject.GetCorrentWowObjectFromPointer(curObject);

                    // don't add itseld
                    if (wo.Guid != LocalGUID)
                        list.Add(wo);

                    uint temp = GetNextObject(curObject);
                    if ((temp == 0) || (temp == curObject))
                        return list;

                    curObject = temp;
                }
                return list;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public WowObject LookForGameObj(string name)
        {
            WowObject res = null;

            uint holder = GetFirstObject();
            try
            {
                while ((holder != 0) && ((holder & 1) == 0))
                {
                    WowObject wo = WowObject.GetCorrentWowObjectFromPointer(holder);

                    // don't add itseld
                    if (wo.Name.Equals(name))
                    {
                        res = wo;
                        break;
                    }

                    uint temp = GetNextObject(holder);
                    if ((temp == 0) || (temp == holder))
                        break;

                    holder = temp;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            return res;
        }

        public Descriptor.eObjType GetTypeByObject(uint obj)
        {
            //get the object's type from obj+0x14 (like normal)
            var type = (Descriptor.eObjType) ProcessManager.
                WowProcess.ReadByte(obj + DataManager.CurWoWVersion.Globals.TypeOffset);
            return type;
        }

        /// <summary>
        /// Search thru linked list for given player GUID
        /// Works for local user as well
        /// </summary>
        /// <param name="guid">Player GUID</param>
        /// <returns></returns>
        public string GetPlayerNameFromGuid(ulong guid)
        {
            uint base_addr = ProcessManager.GlobalOffsets.NameStorePointer + 0x11C;

            // Offset to the C string in a name structure
            const uint name_offset = 0x20;

            // Only half the guid is used to check for a hit
            uint short_guid = (uint)guid & 0xffffffff;

            // First element
            uint current = ProcessManager.
                        WowProcess.ReadUInt(base_addr + 8);
            // Offset between elements in linked list
            uint offset = ProcessManager.
                        WowProcess.ReadUInt(base_addr);

            if ((current == 0) || (current & 1) == 1)
                return "";

            uint test_guid = ProcessManager.WowProcess.ReadUInt(current);

            while (test_guid != short_guid)
            {
                current = ProcessManager.WowProcess.ReadUInt(current + offset + 4);

                if ((current == 0) || (current & 1) == 1)
                    return "";

                test_guid = ProcessManager.WowProcess.ReadUInt(current);
            }


            return ProcessManager.WowProcess.
                ReadASCIIString(current + name_offset, 40);
        }

        /*
        public string GetName(uint obj, ulong guid)
        {
            //obj == base address of object
            if (obj <= 0)
                return String.Empty;

            //get the object's type from obj+0x14 (like normal)
            Descriptor.eObjType type = GetTypeByObject(obj);

            try
            {
                //determine what type of object it is and call associated routine
                switch (type)
                {
                    case Descriptor.eObjType.OT_PLAYER:
                        //return GetPlayerName(obj);
                        return GetNameFromGuid(guid);

                    case Descriptor.eObjType.OT_UNIT:
                        return GetUnitName(obj);

                    default:
                        return ProcessManager.WowProcess.ReadASCIIString(
                            ProcessManager.WowProcess.ReadUInt(
                            ProcessManager.WowProcess.ReadUInt(obj + 0x968) + 0x54), 0x40);
                }
            }
            catch (Exception e)
            {
                Output.Instance.LogError("char", "Unable read object name", e);
                return String.Empty;
            }
        }

        public string GetNameFromGuid(ulong guid)
        {
            ulong nameStorePtr = ProcessManager.GlobalOffsets.NameStorePointer; // Player name database
            const ulong nameMaskOffset = 0x024; // Offset for the mask used with GUID to select a linked list
            const ulong nameBaseOffset = 0x01c; // Offset for the start of the name linked list
            const ulong nameStringOffset = 0x020; // Offset to the C string in a name structure

            ulong mask, base_, offset, current, shortGUID, testGUID;

            mask = ProcessManager.WowProcess.ReadUInt((uint) (nameStorePtr + nameMaskOffset));
            base_ = ProcessManager.WowProcess.ReadUInt((uint)(nameStorePtr + nameBaseOffset));

            shortGUID = guid & 0xffffffff; // Only half the guid is used to check for a hit
            offset = 12*(mask & shortGUID); // select the appropriate linked list

            current = ProcessManager.WowProcess.ReadUInt((uint) (base_ + offset + 8));
            offset = ProcessManager.WowProcess.ReadUInt((uint) (base_ + offset)); // next-4 ?
            //current == 0 || (current & 0x1)
            if ((current & 0x1) == 0x1)
                return "";

            testGUID = ProcessManager.WowProcess.ReadUInt((uint) (current));

            while (testGUID != shortGUID)
            {
                current = ProcessManager.WowProcess.ReadUInt((uint) (current + offset + 4));

                if ((current & 0x1) == 0x1)
                    return "";

                testGUID = ProcessManager.WowProcess.ReadUInt((uint) (current));
            }

            // Found the guid in the name list...
            //ReadBytesIntoBuffer(current + nameStringOffset, numBytes, name);
            return ProcessManager.WowProcess.ReadASCIIString((uint) (current + nameStringOffset), 40);
        }

        private string GetPlayerName(uint player)
        {
            //read the object's GUID from obj+0x30 (again, pretty basic)
            UInt64 GUID = ProcessManager.WowProcess.ReadUInt64((player + 0x30));

            //some sort of list index
            int var1 = ProcessManager.WowProcess.ReadInt((0x00D4C4F8 + 0x24));
            if (var1 == -1)
                return "Unknown Player";

            //here we're getting the pointer to the start of the linked list
            int var2 = ProcessManager.WowProcess.ReadInt((0x00D4C4F8 + 0x1C));
            var1 &= (int)GUID;
            var1 += var1 * 2;
            var1 = (var2 + (var1 * 4) + 4);
            var1 = ProcessManager.WowProcess.ReadInt((uint)(var1 + 4));

            //iterate through the linked list until the current entry has
            //the same GUID as the object whose name we want
            while (ProcessManager.WowProcess.ReadInt((uint)var1) != (int)GUID)
            {
                int var3 = ProcessManager.WowProcess.ReadInt((0x00D4C4F8 + 0x1C));
                var2 = (int)GUID;
                var2 &= ProcessManager.WowProcess.ReadInt((0x00D4C4F8 + 0x24));
                var2 += var2 * 2;
                var2 = ProcessManager.WowProcess.ReadInt((uint)(var3 + (var2 * 4)));
                var2 += var1;
                var1 = ProcessManager.WowProcess.ReadInt((uint)(var2 + 4));
            }

            //now that we have the correct entry in the linked list,
            //read its name from entry+0x20
            return ProcessManager.WowProcess.ReadASCIIString((uint)(var1 + 0x20), 40);
        }
        */
    }
}