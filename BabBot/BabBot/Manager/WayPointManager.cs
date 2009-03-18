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