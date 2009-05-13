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
using BabBot.Bot;

namespace BabBot.Manager
{
    public class WayPointManager
    {
        public WayPointCollection BranchPath;
        public WayPointCollection GhostPath;
        public WayPointCollection NormalPath;
        public WayPointCollection RepairPath;
        public WayPointCollection VendorPath;

        /// <summary>
        /// Base constructor
        /// </summary>
        public WayPointManager()
        {
            NormalPath = new WayPointCollection();
            GhostPath = new WayPointCollection();
            VendorPath = new WayPointCollection();
            RepairPath = new WayPointCollection();
            BranchPath = new WayPointCollection();
        }

        public void Reverse(WayPointType wpt)
        {
            switch (wpt)
            {
                case WayPointType.Vendor:
                    VendorPath.Reverse();
                    break;
                case WayPointType.Repair:
                    RepairPath.Reverse();
                    break;
                case WayPointType.Normal:
                    NormalPath.Reverse();
                    break;
                case WayPointType.Ghost:
                    GhostPath.Reverse();
                    break;
                case WayPointType.Branch:
                    BranchPath.Reverse();
                    break;
            }
        }


        #region Properties

        public int NormalNodeCount
        {
            get { return NormalPath.Count; }
        }

        public int GhostNodeCount
        {
            get { return GhostPath.Count; }
        }

        public int VendorNoteCount
        {
            get { return VendorPath.Count; }
        }

        public int RepairNodeCount
        {
            get { return RepairPath.Count; }
        }

        public int BranchNodeCount
        {
            get { return BranchPath.Count; }
        }

        #endregion
    }
}