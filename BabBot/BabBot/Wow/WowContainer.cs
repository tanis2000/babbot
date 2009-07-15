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

namespace BabBot.Wow
{
    public class WowContainer : WowItem
    {
        public WowContainer(uint ObjectPointer)
            : base(ObjectPointer)
        {

        }

        public uint GetSlots()
        {
            return
                (uint)
                ProcessManager.WowProcess.ReadInt(ObjectPointer +
                                                  (uint) Descriptor.eContainerFields.CONTAINER_FIELD_NUM_SLOTS*0x04);
        }

        public WowItem GetContainedItem(uint n)
        {
            if (n >= GetSlots())
            {
                return null;
            }

            UInt64 guid =
                ProcessManager.WowProcess.ReadUInt64(ObjectPointer +
                                                     (uint) Descriptor.eContainerFields.CONTAINER_FIELD_SLOT_1 +
                                                     (2*n)*0x04);

            uint iObjectPointer= ProcessManager.ObjectManager.GetObjectByGUID(guid);

            var item = new WowItem(iObjectPointer);
            return item;
        }

        public uint GetFilledSlots()
        {
            uint nSlots = GetSlots();
            uint nFilled = 0;
            for (uint i = 0; i < nSlots; i++)
            {
                UInt64 guid =
                    ProcessManager.WowProcess.ReadUInt64(ObjectPointer +
                                                         (uint) Descriptor.eContainerFields.CONTAINER_FIELD_SLOT_1 +
                                                         (2*i)*0x04);
                if (guid != 0)
                {
                    nFilled++;
                }
            }

            return nFilled;
        }

        public uint GetEmptySlots()
        {
            uint nSlots = GetSlots();
            uint nEmpty = 0;
            for (uint i = 0; i < nSlots; i++)
            {
                UInt64 guid =
                    ProcessManager.WowProcess.ReadUInt64(ObjectPointer +
                                                         (uint) Descriptor.eContainerFields.CONTAINER_FIELD_SLOT_1 +
                                                         (2*i)*0x04);
                if (guid == 0)
                {
                    nEmpty++;
                }
            }

            return nEmpty;
        }
    }
}