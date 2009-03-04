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
            objectPointer = objectPointer;
            if (ObjectPointer != 0)
            {
                UnitDescriptor = ProcessManager.WowProcess.ReadUInt(ObjectPointer + Globals.DescriptorOffset);
                UnitField = ProcessManager.WowProcess.ReadUInt(ObjectPointer + Globals.UnitFieldOffset);
                ObjectGUID = ProcessManager.WowProcess.ReadUInt64(ObjectPointer + Globals.GuidOffset);
            }
            else
            {
                throw new Exception("Illegal object pointer");
            }

        }

        public int GetHp()
        {
            return ProcessManager.WowProcess.ReadInt(UnitDescriptor + (uint)Descriptor.eUnitFields.UNIT_FIELD_HEALTH * 4);
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

    }
}
