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

        public List<string> GetAllObjectsAroundLocalPlayer()
        {
            List<string> list = new List<string>();
            curObject = GetFirstObject();
            tempHolder = curObject;
            try
            {
                while ((curObject != 0) && ((curObject & 1) == 0))
                {
                    list.Add(curObject.ToString());
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
    }
}
