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

namespace BabBot.Wow
{
    public class Unit
    {
        public ulong ObjectGUID;
        public uint ObjectPointer;
        public uint UnitDescriptor;
        public uint VMT;

        public Unit(uint objectPointer)
        {
            ObjectPointer = objectPointer;
            if (ObjectPointer != 0)
            {
                UnitDescriptor = ProcessManager.WowProcess.ReadUInt(ObjectPointer + Globals.DescriptorOffset);
                //Console.WriteLine(string.Format("OP+DescOff: {0:X}", (ObjectPointer + Globals.DescriptorOffset)));
                ObjectGUID = ProcessManager.WowProcess.ReadUInt64(ObjectPointer + Globals.GuidOffset);
                VMT = ProcessManager.WowProcess.ReadUInt(ObjectPointer);
            }
            else
            {
                throw new Exception("Illegal object pointer");
            }
        }

        public uint GetHp()
        {
            return
                (uint)
                ProcessManager.WowProcess.ReadInt(UnitDescriptor + (uint) Descriptor.eUnitFields.UNIT_FIELD_HEALTH*0x04);
        }

        public uint GetMaxHp()
        {
            return
                (uint)
                ProcessManager.WowProcess.ReadInt(UnitDescriptor +
                                                  (uint) Descriptor.eUnitFields.UNIT_FIELD_MAXHEALTH*0x04);
        }

        public uint GetMp()
        {
            return
                (uint)
                ProcessManager.WowProcess.ReadInt(UnitDescriptor + (uint) Descriptor.eUnitFields.UNIT_FIELD_POWER1*0x04);
        }

        public uint GetMaxMp()
        {
            return
                (uint)
                ProcessManager.WowProcess.ReadInt(UnitDescriptor +
                                                  (uint) Descriptor.eUnitFields.UNIT_FIELD_MAXPOWER1*0x04);
        }

        public uint GetXp()
        {
            return
                (uint)
                ProcessManager.WowProcess.ReadInt(UnitDescriptor + (uint) Descriptor.ePlayerFields.PLAYER_XP*0x04);
        }

        public uint GetLevel()
        {
            return
                (uint)
                ProcessManager.WowProcess.ReadInt(UnitDescriptor +
                                                  (uint)Descriptor.eUnitFields.UNIT_FIELD_LEVEL * 0x04);
        }
        public float GetFacing()
        {
            return ProcessManager.WowProcess.ReadFloat(ObjectPointer + Globals.PlayerRotationOffset);
        }

        public Vector3D GetPosition()
        {
            return new Vector3D(ProcessManager.WowProcess.ReadFloat(ObjectPointer + Globals.PlayerXOffset),
                                ProcessManager.WowProcess.ReadFloat(ObjectPointer + Globals.PlayerYOffset),
                                ProcessManager.WowProcess.ReadFloat(ObjectPointer + Globals.PlayerZOffset));
        }

        public ulong GetCurTargetGuid()
        {
            return
                ProcessManager.WowProcess.ReadUInt64(UnitDescriptor +
                                                     (uint) Descriptor.eUnitFields.UNIT_FIELD_TARGET*0x04);
        }

        public string GetCurTargetName()
        {
            return ProcessManager.ObjectManager.GetName(
                ProcessManager.ObjectManager.GetObjectByGUID(GetCurTargetGuid()), GetCurTargetGuid());
        }

        public WowUnit GetCurTarget()
        {
            var o = new WowObject();
            o.Guid = GetCurTargetGuid();
            o.ObjectPointer = ProcessManager.ObjectManager.GetObjectByGUID(o.Guid);
            o.Type = ProcessManager.ObjectManager.GetTypeByObject(o.ObjectPointer);
            var u = new WowUnit(o);
            return u;
        }

        public List<WowObject> GetNearObjects()
        {
            List<WowObject> l = ProcessManager.ObjectManager.GetAllObjectsAroundLocalPlayer();
            return l;
        }

        public List<WowUnit> GetNearMobs()
        {
            var Mobs = new List<WowUnit>();
            List<WowObject> l = ProcessManager.ObjectManager.GetAllObjectsAroundLocalPlayer();
            foreach (WowObject o in l)
            {
                Descriptor.eObjType type = ProcessManager.ObjectManager.GetTypeByObject(o.ObjectPointer);
                if (type == Descriptor.eObjType.OT_UNIT)
                {
                    var u = new WowUnit(o);
                    u.Name = ProcessManager.ObjectManager.GetName(u.ObjectPointer, u.Guid);
                    Mobs.Add(u);
                }
            }
            return Mobs;
        }

        public List<WowCorpse> GetNearCorpses()
        {
            var Corpses = new List<WowCorpse>();
            List<WowObject> l = ProcessManager.ObjectManager.GetAllObjectsAroundLocalPlayer();
            foreach (WowObject o in l)
            {
                Descriptor.eObjType type = ProcessManager.ObjectManager.GetTypeByObject(o.ObjectPointer);
                if (type == Descriptor.eObjType.OT_CORPSE)
                {
                    var c = new WowCorpse(o);
                    //c.Name = ProcessManager.ObjectManager.GetName(c.ObjectPointer, c.Guid);
                    Corpses.Add(c);
                }
            }
            return Corpses;
        }

        public List<WowContainer> GetBags()
        {
            var Bags = new List<WowContainer>();

            for (int i = 0; i < 5; i++)
            {
                uint BagsPointer =
                    ProcessManager.WowProcess.ReadUInt(ObjectPointer + (uint)
                                                                       ((uint)
                                                                        Descriptor.ePlayerFields.
                                                                            PLAYER_FIELD_PACK_SLOT_1 + i*2)*
                                                                       0x04);
                Descriptor.eObjType type = ProcessManager.ObjectManager.GetTypeByObject(BagsPointer);
                if (type == Descriptor.eObjType.OT_CONTAINER)
                {
                    var o = new WowObject();
                    o.ObjectPointer = BagsPointer;
                    o.Guid = ProcessManager.ObjectManager.GetGUIDByObject(o.ObjectPointer);
                    o.Type = type;
                    var c = new WowContainer(o);
                    Bags.Add(c);
                }
            }
            /*
            List<WowObject> l = ProcessManager.ObjectManager.GetAllObjectsAroundLocalPlayer();
            foreach (WowObject o in l)
            {
                Descriptor.eObjType type = ProcessManager.ObjectManager.GetTypeByObject(o.ObjectPointer);
                if (type == Descriptor.eObjType.OT_UNIT)
                {
                    WowUnit u = new WowUnit(o);
                    u.Name = ProcessManager.ObjectManager.GetName(u.ObjectPointer, u.Guid);
                    Mobs.Add(u);
                }
            }*/
            return Bags;
        }

        public bool IsSitting()
        {
            return
                Convert.ToBoolean(
                    ProcessManager.WowProcess.ReadInt(UnitDescriptor +
                                                      (uint) Descriptor.eUnitFields.UNIT_FIELD_BYTES_1*0x04) &
                    (int) Descriptor.eUnitFlags.UF_SITTING);
        }

        public bool IsTapped()
        {
            return
                (ProcessManager.WowProcess.ReadInt(UnitDescriptor +
                                                   (uint) Descriptor.eUnitFields.UNIT_DYNAMIC_FLAGS*0x04) & 4) != 0;
        }


        public bool IsTappedByMe()
        {
            return
                (ProcessManager.WowProcess.ReadInt(UnitDescriptor +
                                                   (uint) Descriptor.eUnitFields.UNIT_DYNAMIC_FLAGS*0x04) & 8) != 0;
        }

        public bool IsAggro()
        {
            return
                Convert.ToBoolean(
                    (ProcessManager.WowProcess.ReadInt(UnitDescriptor +
                                                       (uint) Descriptor.eUnitFields.UNIT_FIELD_FLAGS*0x04) >> 0x13) & 1);
        }

        public bool IsGhost()
        {
            return
                (ProcessManager.WowProcess.ReadInt(UnitDescriptor + (uint) Descriptor.ePlayerFields.PLAYER_FLAGS*0x04) &
                 0x10) != 0;
        }

        public bool IsLootable()
        {
            return
                (ProcessManager.WowProcess.ReadInt(UnitDescriptor +
                                                   (uint) Descriptor.eUnitFields.UNIT_DYNAMIC_FLAGS*0x04) & 0x0D) != 0;
        }

        #region Code Injection Stuff

        /// <summary>
        /// Interacts with the object, loot, target etc.
        /// </summary>
        public void Interact()
        {
            if (ObjectPointer != 0)
            {
                ProcessManager.Injector.Interact(ObjectPointer);

                /*
                try
                {
                    ProcessManager.SuspendMainWowThread();
                    uint codecave = ProcessManager.WowProcess.AllocateMemory();
                    ProcessManager.WowProcess.Asm.Clear();
                    ProcessManager.WowProcess.Asm.AddLine("fs mov eax, [0x2C]");
                    ProcessManager.WowProcess.Asm.AddLine("mov eax, [eax]");
                    ProcessManager.WowProcess.Asm.AddLine("add eax, 0x10");
                    ProcessManager.WowProcess.Asm.AddLine("mov dword [eax], {0}", Globals.CurMgr);
                    ProcessManager.WowProcess.Asm.AddLine("mov ecx, {0}", ObjectPointer);
                    ProcessManager.WowProcess.Asm.AddLine("call {0}", ProcessManager.WowProcess.ReadUInt(VMT + 38 * 4));
                    ProcessManager.WowProcess.Asm.AddLine("retn");
                    ProcessManager.WowProcess.Asm.InjectAndExecute(codecave);
                    Thread.Sleep(10);
                    ProcessManager.ResumeMainWowThread();
                    ProcessManager.WowProcess.FreeMemory(codecave);
                }
                catch (Exception ex)
                {
                    ProcessManager.ResumeMainWowThread();
                    throw new Exception("Interact() failed miserably!");
                }
                */
            }
        }

        #endregion
    }
}