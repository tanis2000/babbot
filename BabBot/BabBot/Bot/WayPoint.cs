using System.Collections.Generic;
using BabBot.Wow;

namespace BabBot.Bot
{
    public enum WayPointType
    {
        Normal,
        Vendor,
        Repair,
        Ghost,
        Branch
    }

    public class WayPointCollection : List<WayPoint>
    {
    }

    public class WayPoint
    {
        public Vector3D Location;
        public WayPointType Type;

        public WayPoint(Vector3D location, WayPointType type)
        {
            Location = location;
            Type = type;
        }

        public WayPoint(Vector3D location)
        {
            Location = location;
            Type = WayPointType.Normal;
        }

        public WayPoint()
        {
            Location = new Vector3D(0, 0, 0);
            Type = WayPointType.Normal;
        }
    }
}