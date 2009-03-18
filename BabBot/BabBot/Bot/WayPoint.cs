using System;
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
        Branch,
        None
    }

    public class WayPointCollection : List<WayPoint>
    {
    }

    public class WayPoint : IComparable<WayPoint>
    {
        public WayPoint ConnectedTO;
        public Vector3D Location;
        public WayPointType WPType;

        public WayPoint(Vector3D location, WayPointType type)
        {
            Location = location;
            WPType = type;
        }

        public WayPoint(Vector3D location)
        {
            Location = location;
            WPType = WayPointType.Normal;
        }

        public WayPoint()
        {
            Location = new Vector3D(0, 0, 0);
            WPType = WayPointType.Normal;
        }

        public bool IsConnected
        {
            get { return ConnectedTO != null; }
        }

        public WayPointType ConnectedType
        {
            get
            {
                if (IsConnected)
                {
                    return ConnectedTO.WPType;
                }
                return WayPointType.None;
            }
        }

        #region IComparable<WayPoint> Members

        public int CompareTo(WayPoint wp)
        {
            return 0; //Index.CompareTo(wp.Index);
        }

        #endregion

    }
}