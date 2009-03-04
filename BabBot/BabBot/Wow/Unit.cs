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
                /*
                UnitDescriptor = ProcessManager.WowProcess.ReadUInt(ObjectPointer + Globals.DescriptorOffset);
                //Console.WriteLine(string.Format("OP+DescOff: {0:X}", (ObjectPointer + Globals.DescriptorOffset)));
                UnitField = ProcessManager.WowProcess.ReadUInt(ObjectPointer + Globals.UnitFieldOffset);
                ObjectGUID = ProcessManager.WowProcess.ReadUInt64(ObjectPointer + Globals.GuidOffset);
                */

                UnitDescriptor = ProcessManager.WowProcess.ReadUInt(ObjectPointer + (uint)Descriptor.eObjectFields.OBJECT_FIELD_PADDING + 0x04);
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
            return (uint) ProcessManager.WowProcess.ReadInt(UnitDescriptor + (uint)Descriptor.eUnitFields.UNIT_FIELD_HEALTH);
        }

        public uint GetMaxHp()
        {
            return (uint)ProcessManager.WowProcess.ReadInt(UnitDescriptor + (uint)Descriptor.eUnitFields.UNIT_FIELD_MAXHEALTH * 4);
        }

        public uint GetMp()
        {
            return (uint)ProcessManager.WowProcess.ReadInt(UnitDescriptor + (uint)Descriptor.eUnitFields.UNIT_FIELD_POWER1 * 4);
        }

        public uint GetMaxMp()
        {
            return (uint)ProcessManager.WowProcess.ReadInt(UnitDescriptor + (uint)Descriptor.eUnitFields.UNIT_FIELD_MAXPOWER1 * 4);
        }

        public uint GetXp()
        {
            return (uint)ProcessManager.WowProcess.ReadInt(UnitDescriptor + (uint)Descriptor.ePlayerFields.PLAYER_XP * 4);
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
