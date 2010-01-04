using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

// BabBot import
using BabBot.Manager;
using BabBot.Wow;
using BabBot.Forms.Shared;
using System.IO;

namespace BabBot.Forms
{
    public partial class RoutesForm : BabBot.Forms.GenericDialog
    {
        private string _lfs = "routes";

        protected override bool IsChanged
        {
            set
            {
                btnImportRoute.Enabled = !value;
                base.IsChanged = value;
            }
        }

        public RoutesForm()
            : base("routes", true)
        {
            InitializeComponent();

            cbSortBy.SelectedIndex = 0;
            tvRoutes.TreeViewNodeSorter = new NodeSorter(cbSortBy);
            ctrlRouteDetails.RegisterChanges += new EventHandler(RegisterChange);
            ctrlRouteDetails.SetDataSource(DataManager.GameData);
        }

        private void RoutesForm_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible)
                ShowRoutes();
        }

        private void SetTreeNode(Route r, TreeNode tn)
        {
            int idx = (r.MakeFileName() == null) ? 0 : 1;

            tn.Text = r.ScreenName;
            tn.ImageIndex = idx;
            tn.SelectedImageIndex = idx;
            tn.Tag = r;
        }

        private TreeNode GetTreeNode(Route r)
        {
            TreeNode tn = new TreeNode();
            SetTreeNode(r, tn);

            return tn;
        }

        private void ShowRoutes()
        {
            tvRoutes.Nodes.Clear();
            
            for (int i = 0; i < DataManager.CurWoWVersion.Routes.Count; i++)
            {
                Route r = DataManager.CurWoWVersion.Routes[i];
                tvRoutes.Nodes.Add(GetTreeNode(r));
            }

            tvRoutes.Sort();
        }

        private void tvRoutes_AfterSelect(object sender, TreeViewEventArgs e)
        {
            ctrlRouteDetails.Enabled = true;
            Route r = GetSelectedRoute();

            ctrlRouteDetails.PopulateControls(r);

            IsChanged = false;
        }

        private void tvRoutes_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            e.Cancel = (IsChanged && !GetConfirmation("Are you sure cancel changes"));

            if (!e.Cancel)
                IsChanged = false;
        }

        private void tvRoutes_DoubleClick(object sender, EventArgs e)
        {
            OpenRoute();
        }

        private void openRouteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenRoute();
        }

        private void OpenRoute()
        {
            Route r = GetSelectedRoute();
            Program.mainForm.OpenRouteRecording(r);
        }


        private void deleteRouteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteRoute();
        }


        private void tvRoutes_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                DeleteRoute();
                e.Handled = true;
            }
        }

        private void DeleteRoute()
        {
            // Confirm
            if (!GetConfirmation("Are you sure delete selected route ???"))
                return;

            // Delete from the list
            Route r = GetSelectedRoute();
            RouteListManager.DeleteRoute(r, _lfs);

            // Delete from the Tree View
            tvRoutes.Nodes.Remove(tvRoutes.SelectedNode);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Route route = GetRoute();
            // Save route
            if (RouteListManager.SaveRoute(route))
            {
                IsChanged = false;

                // Replace saved node
                SetTreeNode(route, tvRoutes.SelectedNode); 
                tvRoutes.Sort();

                ShowSuccessMsg(route, "saved", false);
            }
        }

        private void ShowSuccessMsg(Route route, string msg, bool export)
        {
            string s = "Route successfully " + msg;
            if (export)
            {
                if (route.FileName == null)
                    s += ". No export file generated.";
                else
                    s += ".\nExport data located in the file '" + route.FileName + "'";
            }

            ShowSuccessMessage(s);
        }

        private Route GetSelectedRoute()
        {
            return (Route)tvRoutes.SelectedNode.Tag;
        }

        private Route GetRoute()
        {
            Route route = GetSelectedRoute();

            if (IsChanged)
            {
                Route new_route = ctrlRouteDetails.GetRoute(
                        route.PointA.Waypoint, route.PointB.Waypoint);
                new_route.Name = route.WaypointFileName;
                route = new_route;
            }
            return route;
        }

        private void btnImportRoute_Click(object sender, EventArgs e)
        {
            var dlg = new OpenFileDialog
            {
                RestoreDirectory = true,
                Multiselect = true,
                Filter = "Routes (*.route)|*.route"
            };

            dlg.InitialDirectory = "Data" + Path.DirectorySeparatorChar + "Import";

            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            foreach (string fname in dlg.FileNames)
            {
                string err = "Failed import data from " + fname +
                                            ". Check format and try again";
                try
                {
                    Route r = RouteListManager.ImportRoute(fname);
                    if (r == null)
                    {
                        ShowErrorMessage(err);
                    }
                    else
                    {
                        // Add note
                        tvRoutes.Nodes.Add(GetTreeNode(r));
                        tvRoutes.Sort();
                    }
                }
                catch
                {
                    ShowErrorMessage(err);
                }
            }
        }

        private void exportRouteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Route r = GetSelectedRoute();

            // Load waypoints
            Waypoints wp = RouteListManager.LoadWaypoints(r.WaypointFileName);
            if (RouteListManager.ExportRoute(r, wp))
                ShowSuccessMsg(r, "exported", true);

        }

        private void cbSortBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            tvRoutes.Sort();
        }
    }

    // Create a node sorter that implements the IComparer interface.
    public class NodeSorter : IComparer
    {
        private ComboBox _cb;

        public NodeSorter(ComboBox cb)
        {
            _cb = cb;
        }

        /// <summary>
        /// Compare quest by Endpoint A types in the same order it's defined
        /// in EndpointTypes except EndpointTypes going to end
        /// </summary>
        /// <param name="n1">Node 1</param>
        /// <param name="n2">Node 2</param>
        /// <returns>
        /// -1 if Node 1 less than Node 2 (shown below)
        /// 0 if node equals
        /// 1 if Node 1 greater than Node 2 (shown above)
        /// </returns>
        public int Compare(object n1, object n2)
        {
            TreeNode tn1 = (TreeNode)n1;
            TreeNode tn2 = (TreeNode)n2;

            int idx = _cb.SelectedIndex;
            byte b1 = (byte)((Route)tn1.Tag)[idx].PType;
            byte b2 = (byte)((Route)tn2.Tag)[idx].PType;

            if (b1 == b2)
                return string.Compare(tn1.Text, tn2.Text);
            else
                return (b1 < b2) ? -1 : 1;
        }
    }
}
