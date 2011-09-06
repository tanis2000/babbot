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
using BabBot.Manager;
using System;
using BabBot.Common;
namespace BabBot.Wow
{
    public class WowObject
    {
        public uint ObjectPointer { get; protected set; }

        public WowObject(uint ObjectPointer)
        {
            this.ObjectPointer = ObjectPointer;
        }

        public ulong Guid
        {
            get { return ReadOffset<UInt64>(DataManager.
                            CurWoWVersion.Globals.GuidOffset); }
        }

        public Descriptor.eObjType Type
        {
            get { return (Descriptor.eObjType)ReadOffset<int>(
                ProcessManager.GlobalOffsets.TypeOffset);
            }
        }

        public virtual string Name
        {
            // Each type of wow object has its own procedure for name
            get { return string.Empty; }
        }

        public virtual Vector3D Location
        {
            get
            {
                return new Vector3D(
                    ReadOffset<float>(ProcessManager.GlobalOffsets.PlayerXOffset),
                        ReadOffset<float>(ProcessManager.GlobalOffsets.PlayerYOffset),
                            ReadOffset<float>(ProcessManager.GlobalOffsets.PlayerZOffset));
            }
        }

        public float Rotation
        {
            get {return ReadOffset<float>(DataManager.
                CurWoWVersion.Globals.PlayerRotationOffset); }
        }

        public virtual float DistanceFromPlayer
        {
            get
            {
                Vector3D playerLocation = ProcessManager.Player.Location;

                return MathFuncs.GetDistance(playerLocation, Location, true);
            }
        }

        public virtual uint Descriptors
        {
            get { return ProcessManager.WowProcess.
                ReadUInt(ObjectPointer + DataManager.
                    CurWoWVersion.Globals.DescriptorOffset); }
        }

        protected T ReadOffset<T>(object offset) where T : struct
        {
            return (T)ProcessManager.WowProcess.
                ReadObject(ObjectPointer + Convert.ToUInt32(offset), typeof(T));
        }

        protected T ReadDescriptor<T>(object field) where T : struct
        {
            return (T)ProcessManager.WowProcess.ReadObject(Descriptors + 
                                (Convert.ToUInt32(field) * 4), typeof(T));
        }

        public static WowObject GetCorrentWowObjectFromPointer(uint ObjectPointer)
        {
            switch (ProcessManager.ObjectManager.GetTypeByObject(ObjectPointer))
            {
                case Descriptor.eObjType.OT_CONTAINER:
                    return new WowContainer(ObjectPointer);
                case Descriptor.eObjType.OT_CORPSE:
                    return new WowCorpse(ObjectPointer);
                case Descriptor.eObjType.OT_DYNOBJ:
                    return new WowDynamicObject(ObjectPointer);
                case Descriptor.eObjType.OT_GAMEOBJ:
                    return new WowGameObject(ObjectPointer);
                case Descriptor.eObjType.OT_ITEM:
                    return new WowItem(ObjectPointer);
                case Descriptor.eObjType.OT_PLAYER:
                    return new WowPlayer(ObjectPointer);
                case Descriptor.eObjType.OT_UNIT:
                    return new WowUnit(ObjectPointer);
                default:
                    return new WowPlayer(ObjectPointer);
            }
        }
    }
}