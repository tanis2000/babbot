using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using BabBot.Common;
using BabBot.Wow;
using BabBot.Manager;

namespace BabBot.Forms
{
    


    public partial class RouteRecorderForm : BabBot.Forms.GenericDialog
    {
        private string[] EndpointList;
        private Route route = new Route();

        public RouteRecorderForm()
            : base ("route_mgr")
        {
            InitializeComponent();

            EndpointList = new string[DataManager.EndpointsSet.Count];
            DataManager.EndpointsSet.Keys.CopyTo(EndpointList, 0);

            int idx = Array.IndexOf(EndpointList, "undef");
            cbTypeA.DataSource = EndpointList;
            cbTypeA.SelectedIndex = idx;
            cbTypeB.DataSource = EndpointList;
            cbTypeB.SelectedIndex = idx;

            dataGridView1.DataSource = route.List;
        }

        private void btnControl_Click(object sender, EventArgs e)
        {
            if (btnControl.Text.Equals("Start"))
            {
                // Start recordint
                // TODO
                btnControl.Text.Equals("Stop");
            }
            else
            {
                // Start recordint
                // TODO
                btnControl.Text.Equals("Start");
            }
        }

        public Route Record(EndpointTypes type_a, string name_a, 
                                    EndpointTypes type_b, string name_b)
        {
            return null;
        }
    }
}
