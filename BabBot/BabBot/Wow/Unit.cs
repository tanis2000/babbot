using System;
using System.Collections.Generic;
using System.Text;
using BabBot.Manager;

namespace BabBot.Wow
{
    public class Unit
    {
        public uint ObjectPointer;
        public uint UnitDescriptor;
        public uint UnitField;
        public ulong ObjectGUID;

        public Unit(uint objectPointer)
        {
            ObjectPointer = objectPointer;
            if (ObjectPointer != 0)
            {
                UnitDescriptor = ProcessManager.WowProcess.ReadUInt(ObjectPointer + Globals.DescriptorOffset);
                //Console.WriteLine(string.Format("OP+DescOff: {0:X}", (ObjectPointer + Globals.DescriptorOffset)));
                UnitField = ProcessManager.WowProcess.ReadUInt(ObjectPointer + Globals.UnitFieldOffset);
                ObjectGUID = ProcessManager.WowProcess.ReadUInt64(ObjectPointer + Globals.GuidOffset);
            }
            else
            {
                throw new Exception("Illegal object pointer");
            }

        }

        public uint GetHp()
        {
            return (uint)ProcessManager.WowProcess.ReadInt(UnitDescriptor + (uint)Descriptor.eObjectFields.OBJECT_FIELD_PADDING + 0x4 + (uint)Descriptor.eUnitFields.UNIT_FIELD_HEALTH);
        }

        public uint GetMaxHp()
        {
            return (uint)ProcessManager.WowProcess.ReadInt(UnitDescriptor + (uint)Descriptor.eObjectFields.OBJECT_FIELD_PADDING + 0x4 + (uint)Descriptor.eUnitFields.UNIT_FIELD_MAXHEALTH);
        }

        public uint GetMp()
        {
            return (uint)ProcessManager.WowProcess.ReadInt(UnitDescriptor + (uint)Descriptor.eObjectFields.OBJECT_FIELD_PADDING + 0x4 + (uint)Descriptor.eUnitFields.UNIT_FIELD_POWER1);
        }

        public uint GetMaxMp()
        {
            return (uint)ProcessManager.WowProcess.ReadInt(UnitDescriptor + (uint)Descriptor.eObjectFields.OBJECT_FIELD_PADDING + 0x4 + (uint)Descriptor.eUnitFields.UNIT_FIELD_MAXPOWER1);
        }

        public uint GetXp()
        {
            return (uint)ProcessManager.WowProcess.ReadInt(UnitDescriptor + (uint)Descriptor.eObjectFields.OBJECT_FIELD_PADDING + 0x4 + (uint)Descriptor.eUnitFields.UNIT_FIELD_PADDING + 0x04 + (uint)Descriptor.ePlayerFields.PLAYER_XP);
        }

        public float GetFacing()
        {
            return ProcessManager.WowProcess.ReadFloat(UnitField + 0x1C);
        }

        public Vector3D GetPosition()
        {
            return new Vector3D(ProcessManager.WowProcess.ReadFloat(ObjectPointer + Globals.PlayerXOffset),
                ProcessManager.WowProcess.ReadFloat(ObjectPointer + Globals.PlayerYOffset),
                ProcessManager.WowProcess.ReadFloat(ObjectPointer + Globals.PlayerZOffset));
        }

        public ulong GetCurTargetGuid()
        {
            return ProcessManager.WowProcess.ReadUInt64(UnitDescriptor + (uint)Descriptor.eObjectFields.OBJECT_FIELD_PADDING + 0x4 + (uint)Descriptor.eUnitFields.UNIT_FIELD_TARGET);
        }

        public string GetCurTargetName()
        {
            //return ProcessManager.ObjectManager.GetName(ProcessManager.ObjectManager.GetObjectByGUID(GetCurTargetGuid()));
            return ProcessManager.ObjectManager.GetName(ProcessManager.ObjectManager.GetObjectByGUID(GetCurTargetGuid()), GetCurTargetGuid());
        }

        public WowUnit GetCurTarget()
        {
            WowObject o = new WowObject();
            o.Guid = GetCurTargetGuid();
            o.ObjectPointer = ProcessManager.ObjectManager.GetObjectByGUID(o.Guid);
            o.Type = ProcessManager.ObjectManager.GetTypeByObject(o.ObjectPointer);
            WowUnit u = new WowUnit(o);
            return u;
        }

        public List<WowObject> GetNearObjects()
        {
            List<WowObject> l = ProcessManager.ObjectManager.GetAllObjectsAroundLocalPlayer();
            return l;
        }

        public List<WowUnit> GetNearMobs()
        {
            List<WowUnit>Mobs = new List<WowUnit>();
            List<WowObject> l = ProcessManager.ObjectManager.GetAllObjectsAroundLocalPlayer();
            foreach (WowObject o in l)
            {
                ObjectManager.ObjectType type = ProcessManager.ObjectManager.GetTypeByObject(o.ObjectPointer);
                if (type == ObjectManager.ObjectType.Unit)
                {
                    WowUnit u = new WowUnit(o);
                    u.Name = ProcessManager.ObjectManager.GetName(u.ObjectPointer, u.Guid);
                    Mobs.Add(u);
                }
            }
            return Mobs;
        }

        public bool IsSitting()
        {
            return Convert.ToBoolean(ProcessManager.WowProcess.ReadInt(UnitDescriptor + (uint)Descriptor.eObjectFields.OBJECT_FIELD_PADDING + 0x4 + (uint)Descriptor.eUnitFields.UNIT_FIELD_BYTES_1) & (int)Descriptor.eUnitFlags.UF_SITTING);
        }

       	public bool IsTapped()
	    {
                return (ProcessManager.WowProcess.ReadInt(UnitDescriptor + (uint)Descriptor.eObjectFields.OBJECT_FIELD_PADDING + 0x4 + (uint)Descriptor.eUnitFields.UNIT_DYNAMIC_FLAGS) & 4) != 0;
	    }

        
        public bool IsTappedByMe()
	    {
            return (ProcessManager.WowProcess.ReadInt(UnitDescriptor + (uint)Descriptor.eObjectFields.OBJECT_FIELD_PADDING + 0x4 + (uint)Descriptor.eUnitFields.UNIT_DYNAMIC_FLAGS) & 8) != 0;
	    }

        public bool IsAggro()
	    {
            return Convert.ToBoolean((ProcessManager.WowProcess.ReadInt(UnitDescriptor + (uint)Descriptor.eObjectFields.OBJECT_FIELD_PADDING + 0x4 + (uint)Descriptor.eUnitFields.UNIT_FIELD_FLAGS) >> 0x13) &1);
	    }

    }
}
