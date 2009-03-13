using System.Collections.Generic;
using BabBot.Bot;

namespace BabBot.Manager
{
    public class WayPointManager
    {
        public List<WayPointCollection> BranchesPath;
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
            BranchesPath = new List<WayPointCollection>();
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

        public int BranchesPathCount
        {
            get { return BranchesPath.Count; }
        }

        #endregion
    }
}