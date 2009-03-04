using System;
using System.Collections.Generic;
using System.Text;
using BabBot.Wow;

namespace BabBot.Navigation
{
    public enum WayPointType : int
    {
        Normal,
        Vendor,
        Repair,
        Ghost
    }

    public class WayPointCollection : List<WayPoint>
    {
        
    }

    public class WayPoint
    {
        public Vector3D Location;
        public WayPointType Type;

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
