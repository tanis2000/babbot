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
using BabBot.States.Common;

namespace BabBot.Forms
{
    


    public partial class RouteRecorderForm : BabBot.Forms.GenericDialog
    {
        private string[] EndpointList;
        private Route _route;
        bool _rec_state = false;

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
            dgWaypoints.Rows.Clear();

#if DEBUG
            if (ProcessManager.Config.Test == 3)
            {
                for (int i = 1; i <= 55; i++)
                {
                    float v = (float)(i + 0.11);
                        RecordWp(new Vector3D(v, v, v));
                }
            }
#endif
        }

        private void btnControl_Click(object sender, EventArgs e)
        {
            if (btnControl.Text.Equals("Start"))
            {
                if (!(CheckInGame() && ResetRoute()))
                    return;

                // Start recording
                dgWaypoints.Rows.Clear();

                ProcessManager.Player.StateMachine.ChangeState(
                        new RouteRecordingState(RecordWp, numRecDistance.Value), false, true);
                ProcessManager.Player.StateMachine.IsRunning = true;

                SetRecordingState();
                btnControl.Text = "Stop";
                btnReset.Text = "Suspend";
            }
            else
            {
                // Stop recordint
                SetRecordingState();
                btnControl.Text = "Start";
                btnReset.Text = "Reset";
            }
        }

        private void SetRecordingState()
        {
            _rec_state = !_rec_state;

            gbRouteDetails.Enabled = !_rec_state;
            lblRecDescr.Enabled = !_rec_state;
            lblRecDistance.Enabled = !_rec_state;
            numRecDistance.Enabled = !_rec_state;
        }

        public void RecordWp(Vector3D v)
        {
            if (InvokeRequired)
            {
                RouteRecordingState.WaypointRecordingHandler del = RecordWp;
                object[] parameters = { v };
                Invoke(del, parameters);
            }
            else
            {
                // Add row
                dgWaypoints.Rows.Add(v.X, v.Y, v.Z);
                int idx = dgWaypoints.Rows.Count - 1;

                // Unselect previous row
                dgWaypoints.Rows[idx - 1].Selected = false;

                // Auto Select last waypoint
                dgWaypoints.Rows[idx].Selected = true;
                dgWaypoints.FirstDisplayedScrollingRowIndex = idx;
            }
        }

        public Route Record(EndpointTypes type_a, string name_a, 
                                    EndpointTypes type_b, string name_b)
        {
            return null;
        }

        private void goToToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Move To
            // TODO
        }

        private void popWaypoints_Opening(object sender, CancelEventArgs e)
        {
            // Disable when multiselected
            // TODO
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            if (_rec_state)
            {
                // TODO
                // Suspend/Resume recordint

                if (btnReset.Text.Equals("Suspend"))
                    btnReset.Text = "Resume";
                else
                    btnReset.Text = "Resume";
            }
            else
                ResetRoute();
        }

        private bool ResetRoute()
        {
            if ((dgWaypoints.Rows.Count > 1) &&
                (MessageBox.Show(this, "Are you sure clear all waypoints ?",
                    "Clear Confirmation", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Exclamation,
                    MessageBoxDefaultButton.Button2) != DialogResult.Yes))
                return false;

            dgWaypoints.Rows.Clear();
            return true;
        }

        private void RouteRecorderForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // TODO
            // Add confirmation and stop Recording state if running
        }
    }
}
