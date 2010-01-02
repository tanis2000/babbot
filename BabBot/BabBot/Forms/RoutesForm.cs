using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using BabBot.Manager;
using BabBot.Wow;

namespace BabBot.Forms
{
    public partial class RoutesForm : BabBot.Forms.GenericDialog
    {
        public RoutesForm()
            : base("routes", true)
        {
            InitializeComponent();
            ctrlRouteDetails.RegisterChanges += new EventHandler(RegisterChange);
            ctrlRouteDetails.SetDataSource(DataManager.GameData);
        }

        private void RoutesForm_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible)
                ShowRoutes();
        }

        private void ShowRoutes()
        {
            tvRoutes.Nodes.Clear();

            for (int i = 0; i < DataManager.CurWoWVersion.Routes.Count; i++)
            {
                Route r = DataManager.CurWoWVersion.Routes[i];
                string rname = r.MakeFileName();
                int idx = (rname == null) ? 0 : 1;
                string name = (rname == null) ? r.Name : rname;

                TreeNode tn = new TreeNode(rname, idx, idx);
                tvRoutes.Nodes.Add(tn);
            }
        }
    }
}
