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
using BabBot.Manager;
using System.Collections.Generic;

namespace BabBot.Wow
{
    public class WowUnit : WowObject
    {
        public WowUnit(uint ObjectPointer)
            : base(ObjectPointer)
        {

        }

        public ulong CurTargetGuid
        {
            get { return ReadDescriptor<ulong>(Descriptor.eUnitFields.UNIT_FIELD_TARGET); }
        }

        public float Orientation
        {
            get { return ReadOffset<float>(Globals.PlayerRotationOffset); }
        }
        
        public uint Hp
        {
            get { return ReadDescriptor<uint>(Descriptor.eUnitFields.UNIT_FIELD_HEALTH); }
        }

        public uint MaxHp
        {
            get { return ReadDescriptor<uint>(Descriptor.eUnitFields.UNIT_FIELD_MAXHEALTH); }
        }

        public uint MaxMp
        {
            get { return ReadDescriptor<uint>(Descriptor.eUnitFields.UNIT_FIELD_MAXPOWER1); }
        }

        public uint Mp
        {
            get { return ReadDescriptor<uint>(Descriptor.eUnitFields.UNIT_FIELD_POWER1); }
        }

        public int Level
        {
            get { return ReadDescriptor<int>(Descriptor.eUnitFields.UNIT_FIELD_LEVEL); }
        }

        public bool HasTarget
        {
            get { return (CurTargetGuid != 0); }
        }

        public bool IsAggro
        {
            get { return Convert.ToBoolean((ReadDescriptor<uint>(Descriptor.eUnitFields.UNIT_FIELD_FLAGS) >> 0x13) & 1); }
        }

        public bool IsDead
        {
            get
            {
                if (Hp <= 0 || IsGhost) return true;
                else return false;
            }
        }

        public bool IsLootable
        {
            get
            {
                return (ReadDescriptor<int>(Descriptor.eUnitFields.UNIT_DYNAMIC_FLAGS) & 0x0D) != 0;
            }
        }

        public bool IsNpc
        {
            get
            {
                return ReadDescriptor<bool>(Descriptor.eUnitFields.UNIT_NPC_FLAGS);
            }
        }

        public WowObject SummonedBy
        {
            get
            {
                uint summonGuid = ReadDescriptor<uint>(Descriptor.eUnitFields.UNIT_FIELD_SUMMONEDBY);

                if (summonGuid == 0) return null;

                uint sObjectPointer = ProcessManager.ObjectManager.GetObjectByGUID(summonGuid);

                return WowObject.GetCorrentWowObjectFromPointer(sObjectPointer);
            }
        }

        public float BoundingRadius
        {
            get { return ReadDescriptor<float>(Descriptor.eUnitFields.UNIT_FIELD_BOUNDINGRADIUS); }
        }

        public WowUnit CurTarget
        {
            get
            {
                if (CurTargetGuid == 0) return null;

                uint oPointer = ProcessManager.ObjectManager.GetObjectByGUID(CurTargetGuid);

                WowUnit o = new WowUnit(oPointer);

                return o;
            }
        }

        public bool IsSitting
        {
            get
            {
                return
                    Convert.ToBoolean(ReadDescriptor<uint>((uint)Descriptor.eUnitFields.UNIT_FIELD_BYTES_1) &
                        (int)Descriptor.eUnitFlags.UF_SITTING);
            }
        }

        public bool IsTapped
        {
            get
            {
                return (ReadDescriptor<int>((uint)Descriptor.eUnitFields.UNIT_DYNAMIC_FLAGS) & 4) != 0;
            }
        }

        public bool IsTappedByMe
        {
            get
            {
                return (ReadDescriptor<int>((uint)Descriptor.eUnitFields.UNIT_DYNAMIC_FLAGS) & 8) != 0;
            }
        }

        public bool IsSkinnable
        {
            get
            {
                return (ReadDescriptor<int>((uint)Descriptor.eUnitFields.UNIT_DYNAMIC_FLAGS) & (int)Descriptor.eUnitFlags.UF_SKINNABLE) != 0;
            }
        }

        public bool IsGhost
        {
            get
            {
                return (ReadDescriptor<int>((uint)Descriptor.ePlayerFields.PLAYER_FLAGS) & 0x10) != 0;
            }
        }



        /// <summary>Interacts with the object, loot, target etc.</summary>
        public void Interact()
        {
            if (ObjectPointer != 0)
            {
                ProcessManager.Injector.Interact(ObjectPointer);
            }
        }

        public bool Equals(WowUnit obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            return (obj.Guid == Guid);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj.GetType() != typeof (WowUnit))
            {
                return false;
            }
            return Equals((WowUnit) obj);
        }

        public override int GetHashCode()
        {
            return Guid.GetHashCode();
        }
    }
}