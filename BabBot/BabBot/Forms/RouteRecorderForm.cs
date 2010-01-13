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
using System.Data;
using System.Text;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.Generic;
using BabBot.Wow;
using BabBot.Manager;
using BabBot.Common;
using BabBot.Wow.Helpers;
using BabBot.States.Common;
using BabBot.Data;
using BabBot.Forms.Shared;

namespace BabBot.Forms
{
    public enum RecStates
    {
        IDLE = 0,
        RECORDING = 1,
        SUSPENDED = 2
    }

    public partial class RouteRecorderForm : BabBot.Forms.GenericDialog
    {
        
        RecStates _rec_state = RecStates.IDLE;
        RouteRecordingState _route_rec_state;
        private Route _route;
        private string _lfs = "route_recorder";

        public RouteRecorderForm()
            : base ("route_record")
        {
            InitializeComponent();
            ctrlRouteDetails.RegisterChanges += new EventHandler(RegisterChanges);
            ctrlRouteDetails.SetDataSource(DataManager.GameData);

            dgWaypoints.Rows.Clear();
            
#if DEBUG
            if (ProcessManager.Config.Test == 3)
            {
                ctrlRouteDetails.tbZoneA.Text = "Teldrassil";
                ctrlRouteDetails.tbZoneB.Text = "Teldrassil";

                _rec_state = RecStates.RECORDING;
                for (int i = 1; i <= 55; i++)
                {
                    float v = (float)(i + 0.11);
                    RecordWp(new Vector3D(v, v, v));
                }
                _rec_state = RecStates.IDLE;
            }
#endif
        }

        private void btnControl_Click(object sender, EventArgs e)
        {
            if (_rec_state == RecStates.IDLE)
            {
                // Check that endpoints type selected
                if (!ctrlRouteDetails.CheckEndpoints())
                    return;

                // Ask for reset if table has data and last row not selected
                // If last row selected than keep appending data
                bool ask_reset = (dgWaypoints.RowCount > 1) &&
                            (dgWaypoints.Rows[dgWaypoints.RowCount - 1].Selected == false);

                if (!((AbstractListEndpoint)ctrlRouteDetails.cbTypeA.SelectedItem).Check ||
                    (!(CheckInGame() && (!ask_reset || ResetRoute()))))
                    return;
                
#if DEBUG
                if (ProcessManager.Player != null)
                {
#endif
                    if (ProcessManager.Player.StateMachine.IsRunning)
                    {
                        ShowErrorMessage("Bot is running. Stop it first before recording");
                        return;
                    }

                    // Set from zone
                    ProcessManager.Player.SetCurrentMapInfo();
                    ctrlRouteDetails.tbZoneA.Text = ProcessManager.Player.ZoneText;

                    // Load RouteRecordingState and start
                    _route_rec_state = new RouteRecordingState(RecordWp, numRecDistance.Value);
                    ProcessManager.Player.StateMachine.
                            InitState = new TestGlobalState(_route_rec_state);

#if DEBUG
                }
                else ctrlRouteDetails.tbZoneA.Text = "Teldrassil";
#endif
                btnControl.Text = "Stop";
                btnReset.Text = "Suspend";

                // Last call
                SetControls(RecStates.RECORDING);
            }
            else
            {
                // Stop recording
                // First call
                SetControls(RecStates.IDLE);

#if DEBUG
                ctrlRouteDetails.tbZoneB.Text = "Teldrassil";
                if (ProcessManager.Player != null)
                {
#endif
                // Put state machine back in idle status
                _route_rec_state.Exit(ProcessManager.Player);
                ProcessManager.Player.StateMachine.IsRunning = false;

                ProcessManager.Player.SetCurrentMapInfo();
                ctrlRouteDetails.tbZoneB.Text = ProcessManager.Player.ZoneText;

#if DEBUG
                }
#endif
                btnControl.Text = "Start";
                btnReset.Text = "Reset";
            }
        }

        private void SetControls(RecStates state)
        {
            _rec_state = state;
            bool rec_state = (_rec_state == RecStates.RECORDING);
            bool spd_state = (_rec_state == RecStates.SUSPENDED);
            bool start_state = rec_state || spd_state;

            ctrlRouteDetails.gbRouteDetails.Enabled = !start_state;
            lblRecDescr.Enabled = !start_state;
            lblRecDistance.Enabled = !start_state;
            numRecDistance.Enabled = !start_state;
            dgWaypoints.Enabled = !rec_state;

            btnSave.Enabled = !start_state && IsChanged;
        }

        public void RecordWp(Vector3D v)
        {
            if (_rec_state == RecStates.IDLE)
                return;

            if (InvokeRequired)
            {
                RouteRecordingState.WaypointRecordingHandler del = RecordWp;
                object[] parameters = { v };
                Invoke(del, parameters);
            }
            else
            {
                // Unselect previous row
                int idx = dgWaypoints.Rows.Count;
                dgWaypoints.Rows[idx - 1].Selected = false;

                // Add row
                dgWaypoints.Rows.Add(v.X, v.Y, v.Z);

                // For some reason the first row keep selected
                if (idx == 2)
                    dgWaypoints.Rows[0].Selected = false;

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
            // Get selection
            DataGridViewSelectedRowCollection rows = dgWaypoints.SelectedRows;

            string err = null;
            if (rows.Count != 1)
            {
                err = "Impossible move to dest - ";
                if (rows.Count == 0)
                    err += "Nothing selected";
                else
                    err += "Single selection required";

            }

            DataGridViewRow row = rows[0];

            // Last record always empty
            if (row.IsNewRow)
                err = "Destination is empty";

            if (err != null)
            {
                ShowErrorMessage(err + " !!!");
                return;
            }


#if DEBUG
            if (ProcessManager.Player != null)
#endif
            // Move To
            NpcHelper.MoveToDest(MakeVector(row), "record");
        }

        private Vector3D MakeVector(DataGridViewRow row)
        {
            return new Vector3D(Convert.ToDouble(row.Cells[0].Value),
                                    Convert.ToDouble(row.Cells[1].Value),
                                        Convert.ToDouble(row.Cells[2].Value));
        }

        private void popWaypoints_Opening(object sender, CancelEventArgs e)
        {
            // Disable when multiselected
            goToToolStripMenuItem.Enabled = (dgWaypoints.SelectedRows.Count == 1);
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            if (_rec_state != RecStates.IDLE)
            {
                if (_rec_state == RecStates.RECORDING)
                {
                    SetControls(RecStates.SUSPENDED);
                    btnReset.Text = "Resume";
                }
                else
                {
                    SetControls(RecStates.RECORDING);
                    btnReset.Text = "Suspend";
                }
            }
            else
                ResetRoute();
        }

        private bool ResetRoute()
        {
            if ((dgWaypoints.Rows.Count > 1) &&
                (!GetConfirmation("Are you sure clear all waypoints ?")))
                return false;

            dgWaypoints.Rows.Clear();
            return true;
        }

        protected override void OnFormClosing(
                            object sender, FormClosingEventArgs e)
        {
            base.OnFormClosing(sender, e);

            // Stop state machine if recording
            if (!e.Cancel && _rec_state != RecStates.IDLE)
                btnControl_Click(sender, e);
        }

        private void dgWaypoints_DoubleClick(object sender, EventArgs e)
        {
            goToToolStripMenuItem_Click(sender, e);
        }

        private Waypoints GetWaypointsList(string fname)
        {
            return GetWaypointsList(fname, false);
        }

        private Waypoints GetReverseWaypointsList(string fname)
        {
            return GetWaypointsList(fname, true);
        }

        private Waypoints GetWaypointsList(string fname, bool reverse)
        {
            float len = 0;
            Waypoints waypoints = new Waypoints(fname);
            Vector3D v_prev = MakeVector(dgWaypoints.Rows[0]);
            waypoints.Add(v_prev);

            for (int i = 1; i < dgWaypoints.Rows.Count - 1; i++)
            {
                Vector3D v_cur = MakeVector(dgWaypoints.Rows[i]);
                len += v_prev.GetDistanceTo(v_cur);
                waypoints.Add(v_cur);
                v_prev = v_cur;
            }

            if (reverse)
                waypoints.List.Reverse();

            return waypoints;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Make route
            AbstractListEndpoint point_a = ctrlRouteDetails.
                        cbTypeA.SelectedItem as AbstractListEndpoint;
            AbstractListEndpoint point_b = ctrlRouteDetails.
                        cbTypeB.SelectedItem as AbstractListEndpoint;

            if (_route == null)
            {
                // Make new route
                _route = ctrlRouteDetails.GetRoute(MakeVector(dgWaypoints.Rows[0]),
                            MakeVector(dgWaypoints.Rows[dgWaypoints.Rows.Count - 2]));
                if (_route == null)
                    return;
            }
            else
            {
                // Update existing
                _route.PointA = point_a.GetEndpoint(ctrlRouteDetails.tbZoneA.Text,
                        MakeVector(dgWaypoints.Rows[0]));
                _route.PointB = point_b.GetEndpoint(ctrlRouteDetails.tbZoneB.Text,
                        MakeVector(dgWaypoints.Rows[dgWaypoints.Rows.Count - 2]));
                _route.Description = ctrlRouteDetails.tbDescr.Text;
                _route.Reversible = ctrlRouteDetails.cbReversible.Checked;
            }

            // Add waypoints except  last empty line
            Waypoints waypoints = GetWaypointsList(_route.FileName);

            // Save route
            if (RouteListManager.SaveRoute(_route, waypoints, _lfs))
            {
                string s = "Route successfully saved";

                if (_route.FileName == null)
                    s += ". No export file generated.";
                else
                    s += ".\nExport data located in the file '" + 
                                                _route.FileName + "'";

                ctrlRouteDetails.lblWaypointFile.Text = 
                                "Waypoint : " + _route.WaypointFileName;

                ShowSuccessMessage(s);
                IsChanged = false;
            }
        }

        private void clearEverythingAfterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgWaypoints.SelectedRows.Count != 1)
            {
                ShowErrorMessage("Select the single waypoint first");
                return;
            }

            DataGridViewRow row = dgWaypoints.SelectedRows[0];
            int idx = dgWaypoints.Rows.IndexOf(row) + 1;
            int cnt = dgWaypoints.Rows.Count;
            for (int i = idx; i < (cnt - 1); i++)
                dgWaypoints.Rows.RemoveAt(idx);
        }

        private void dgWaypoints_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            IsChanged = true;
        }

        private void dgWaypoints_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            IsChanged = true;
        }

        private void RegisterChanges(object sender, EventArgs e)
        {
            IsChanged = true;
        }

        private void dgWaypoints_SelectionChanged(object sender, EventArgs e)
        {
            btnTest.Enabled = (dgWaypoints.Rows.Count > 1) && 
                (dgWaypoints.SelectedRows.Count == 1) &&
                (dgWaypoints.Rows[0].Selected || 
                    dgWaypoints.Rows[dgWaypoints.Rows.Count - 1].Selected);
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            if (!CheckInGame())
                return;

            Vector3D cur_pos = ProcessManager.Player.Location;

            // Check if bot at first point
            Vector3D vfirst = MakeVector(dgWaypoints.Rows[0]);
            if (cur_pos.IsClose(vfirst))
            {
                // Make vector array

                // Start navigation thread with vector array
                bwRouteNavigation.RunWorkerAsync(GetWaypointsList("Test Direct Run"));
            } 
            else if (cur_pos.IsClose(MakeVector(dgWaypoints.
                                Rows[dgWaypoints.Rows.Count - 2])))
            {
                bwRouteNavigation.RunWorkerAsync(GetReverseWaypointsList("Test Direct Run"));
            }
        }

        private void bwRouteNavigation_DoWork(object sender, DoWorkEventArgs e)
        {
            Waypoints wp = (Waypoints)e.Argument;
            NpcHelper.StartNavState(new NavigationState(wp, "test_run", wp.Name), 
                                                ProcessManager.Player, "test_run");
        }

        public void StartRecording(string obj_name, string q_name, string qi_name)
        {
            // From Game Object
            ctrlRouteDetails.cbTypeA.Text = "game_object";
            ctrlRouteDetails.cbObjA0.Text = obj_name;

            // To QuestItem
            ctrlRouteDetails.cbTypeB.Text = "quest_objective";
            ctrlRouteDetails.cbObjB0.Text = q_name;
            ctrlRouteDetails.cbObjB1.Text = qi_name;

            ShowDialog();
            btnControl_Click(this, null);
        }

        public void Open(Route route)
        {
            _route = route;
            ctrlRouteDetails.PopulateControls(route);

            Waypoints wp = null;
            try
            {
                // Fill waypoints
                wp = RouteListManager.LoadWaypoints(route.WaypointFileName);
            }
            catch (Exception e)
            {
                ShowErrorMessage(e);
                Dispose();
                return;
            }

            // Fill data table
            foreach (Vector3D v in wp.List)
                dgWaypoints.Rows.Add(v.X, v.Y, v.Z);

            // Select last
            int last = dgWaypoints.Rows.Count - 1;
            dgWaypoints.FirstDisplayedScrollingRowIndex = last;
            
            // Assign event on shown
            Shown += SelectLastRecord;
            
            ShowDialog();
        }

        private void SelectLastRecord(object sender, EventArgs e)
        {
            int last = dgWaypoints.Rows.Count - 1;
            dgWaypoints.Rows[0].Selected = false;
            dgWaypoints.Rows[last].Selected = true;

            // Remove event handler
            Shown -= SelectLastRecord;
        }
    }
}
