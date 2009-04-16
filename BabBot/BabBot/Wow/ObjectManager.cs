using System;
using System.Collections.Generic;
using System.Text;
using BabBot.Manager;

namespace BabBot.Wow
{
    /// <summary>
    /// This class contains all of the properties and methods used to access WoW's internal
    /// linked lists of objects.
    /// </summary>
    public class ObjectManager
    {
        private uint CurMgr;
        private ulong LocalGUID;
        private uint holder;
        private uint tempHolder;
        private uint curObject;

        public enum ObjectType : uint
        {
            Item = 0x01,
            Container = 0x02,
            Unit = 0x03,
            Player = 0x04,
            GameObject = 0x05, // Nodes, etc..
            DynamicObject = 0x06, // Spells and stuff
            Corpse = 0x07
        }

        public ObjectManager()
        {
            CurMgr = Globals.CurMgr; // Warning! This must have been initialized (aka logged in and in game)
            LocalGUID = ProcessManager.WowProcess.ReadUInt64(CurMgr + Globals.LocalGuidOffset);
        }

        public uint GetFirstObject()
        {
            return ProcessManager.WowProcess.ReadUInt(CurMgr + Globals.FirstObject);
        }

        public uint GetNextObject(uint CurrentObject)
        {
            return ProcessManager.WowProcess.ReadUInt(CurrentObject + Globals.NextObject);
        }

        public uint GetObjectByGUID(ulong GUID)
        {
            holder = GetFirstObject();
            tempHolder = holder;
            while ((holder != 0) && ((holder & 1) == 0))
            {
                if (ProcessManager.WowProcess.ReadUInt64(holder + Globals.GuidOffset) == GUID)
                {
                    return holder;
                }
                tempHolder = this.GetNextObject(holder);
                if ((tempHolder == 0) || (tempHolder == holder))
                {
                    break;
                }
                holder = tempHolder;
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
            return ProcessManager.WowProcess.ReadUInt64(Object + Globals.GuidOffset);
        }

        public List<WowObject> GetAllObjectsAroundLocalPlayer()
        {
            List<WowObject> list = new List<WowObject>();
            WowObject wo = new WowObject();
            curObject = GetFirstObject();
            tempHolder = curObject;
            try
            {
                while ((curObject != 0) && ((curObject & 1) == 0))
                {
                    wo = new WowObject();
                    wo.ObjectPointer = curObject;
                    wo.Guid = GetGUIDByObject(curObject);
                    wo.Type = GetTypeByObject(curObject);
                    list.Add(wo);

                    tempHolder = GetNextObject(curObject);
                    if ((tempHolder == 0) || (tempHolder == curObject))
                    {
                        return list;
                    }
                    curObject = tempHolder;
                }
                return list;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public ObjectType GetTypeByObject(uint obj)
        {
            //get the object's type from obj+0x14 (like normal)
            ObjectType type = (ObjectType)ProcessManager.WowProcess.ReadByte(obj + 0x14);
            return type;
        }

        public string GetName(uint obj, ulong guid)
        {
            //obj == base address of object
            if (obj <= 0)
                return String.Empty;

            //get the object's type from obj+0x14 (like normal)
            ObjectType type = GetTypeByObject(obj);

            //determine what type of object it is and call associated routine
            switch (type)
            {
                case ObjectType.Player:
                    //return GetPlayerName(obj);
                    return GetNameFromGuid(guid);

                case ObjectType.Unit:
                    return GetUnitName(obj);

                default:
                    return "Unknown Object with type " + type;
            }
        }

        public string GetNameFromGuid(ulong guid)
        {
            const ulong nameStorePtr = 0x01137CE0 + 8; // 3.0.9 0x11AF470 + 0x8;  // Player name database
            const ulong nameMaskOffset = 0x024;  // Offset for the mask used with GUID to select a linked list
            const ulong nameBaseOffset = 0x01c;  // Offset for the start of the name linked list
            const ulong nameStringOffset = 0x020;  // Offset to the C string in a name structure

            ulong mask, base_, offset, current, shortGUID, testGUID;

            mask = ProcessManager.WowProcess.ReadUInt((uint)(nameStorePtr + nameMaskOffset));
            base_ = ProcessManager.WowProcess.ReadUInt((uint)(nameStorePtr + nameBaseOffset));

            shortGUID = guid & 0xffffffff;  // Only half the guid is used to check for a hit
            offset = 12 * (mask & shortGUID);  // select the appropriate linked list

            current = ProcessManager.WowProcess.ReadUInt((uint)(base_ + offset + 8));
            offset = ProcessManager.WowProcess.ReadUInt((uint)(base_ + offset));  // next-4 ?
            //current == 0 || (current & 0x1)
            if ((current & 0x1) == 0x1) { return ""; }

            testGUID = ProcessManager.WowProcess.ReadUInt((uint)(current));

            while (testGUID != shortGUID)
            {
                current = ProcessManager.WowProcess.ReadUInt((uint)(current + offset + 4));

                if ((current & 0x1) == 0x1) { return ""; }
                testGUID = ProcessManager.WowProcess.ReadUInt((uint)(current));
            }

            // Found the guid in the name list...
            //ReadBytesIntoBuffer(current + nameStringOffset, numBytes, name);
            return ProcessManager.WowProcess.ReadASCIIString((uint)(current + nameStringOffset), 40);
        }

        
        private string GetUnitName(uint unit)
        {
            //unit names are easy to get!
            uint aaa = ProcessManager.WowProcess.ReadUInt((unit + 0x970));
            return ProcessManager.WowProcess.ReadASCIIString(ProcessManager.WowProcess.ReadUInt((aaa + 0x3C)), 40);
        }

/*
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
