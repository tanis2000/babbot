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