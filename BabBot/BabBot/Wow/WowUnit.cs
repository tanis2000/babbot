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

namespace BabBot.Wow
{
    public class WowUnit : WowObject
    {
        private readonly Unit unit;
        public string Name;

        public WowUnit(WowObject o)
        {
            Guid = o.Guid;
            ObjectPointer = o.ObjectPointer;
            Type = o.Type;
            unit = new Unit(ObjectPointer);
        }

        public UInt64 CurTargetGuid
        {
            get { return unit.GetCurTargetGuid(); }
        }

        public Vector3D Location
        {
            get { return unit.GetPosition(); }
        }

        public float Orientation
        {
            get { return unit.GetFacing(); }
        }

        public bool HasTarget()
        {
            if (CurTargetGuid != 0)
            {
                return true;
            }
            return false;
        }

        public bool IsAggro()
        {
            return unit.IsAggro();
        }

        public bool IsDead()
        {
            if (unit.GetHp() <= 0)
            {
                return true;
            }
            return false;
        }

        public bool IsLootable()
        {
            return unit.IsLootable();
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