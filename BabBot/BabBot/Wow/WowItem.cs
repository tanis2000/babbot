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
using System.Linq;
using System.Text;
using BabBot.Manager;

namespace BabBot.Wow
{
    public class WowItem : WowObject
    {
        public WowItem(WowObject o)
        {
            Guid = o.Guid;
            ObjectPointer = o.ObjectPointer;
            Type = o.Type;
        }

        public uint GetDurability()
        {
            return (uint)ProcessManager.WowProcess.ReadInt(ObjectPointer + (uint)Descriptor.eItemFields.ITEM_FIELD_DURABILITY * 0x04);
        }

        public uint GetMaxDurability()
        {
            return (uint)ProcessManager.WowProcess.ReadInt(ObjectPointer + (uint)Descriptor.eItemFields.ITEM_FIELD_MAXDURABILITY * 0x04);
        }

        public uint GetStackCount()
        {
            return (uint)ProcessManager.WowProcess.ReadInt(ObjectPointer + (uint)Descriptor.eItemFields.ITEM_FIELD_STACK_COUNT * 0x04);
        }

        public UInt64 GetContained(UInt64 guid)
        {
            return ProcessManager.WowProcess.ReadUInt64(ObjectPointer + (uint)Descriptor.eItemFields.ITEM_FIELD_CONTAINED * 0x04);
        }

    }
}
