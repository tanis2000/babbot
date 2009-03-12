using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BabBot.Wow
{
    public class WowUnit : WowObject
    {
        public string Name;
        private Unit unit;

        public WowUnit(WowObject o)
        {
            Guid = o.Guid;
            ObjectPointer = o.ObjectPointer;
            Type = o.Type;
            unit = new Unit(ObjectPointer);
        }

        public bool HasTarget()
        {
            if (CurTargetGuid != 0)
            {
                return true;
            }
            return false;
        }

        public UInt64 CurTargetGuid
        {
            get
            {
                return unit.GetCurTargetGuid();
            }
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

        public Vector3D Location
        {
            get
            {
                return unit.GetPosition();
            }
        }
    }
}
