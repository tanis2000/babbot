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
    }
}