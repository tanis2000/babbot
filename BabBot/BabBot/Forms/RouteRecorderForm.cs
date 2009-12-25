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
        
        private Route _route;
        RecStates _rec_state = RecStates.IDLE;
        ComboBox[] type_cb;

        public RouteRecorderForm()
            : base ("route_mgr")
        {
            InitializeComponent();

            pOptA.Tag = pObjA;
            pObjA.Tag = pSubObjA;
            cbObjA0.Tag = lblObjA0;
            cbObjA1.Tag = lblObjA1;

            pOptB.Tag = pObjB;
            pObjB.Tag = pSubObjB;
            cbObjB0.Tag = lblObjB0;
            cbObjB1.Tag = lblObjB1;

            type_cb = new ComboBox[] { cbTypeA, cbTypeB };
            Panel[] obj_p = new Panel[] { pOptA, pOptB };

            List<AbstractEndpoint>[] obj_list = new List<AbstractEndpoint>[] {
                new List<AbstractEndpoint>(), new List<AbstractEndpoint>() };

            foreach (EndpointTypes ept in Enum.GetValues(typeof(EndpointTypes)))
            {
                string class_name = Output.GetLogNameByLfs(
                    Enum.GetName(typeof(EndpointTypes), ept).ToLower(), "") + "Endpoint";

                Type reflect_class = Type.GetType("BabBot.Forms." + class_name);

                ConstructorInfo[] ci = reflect_class.GetConstructors();

                for (int i = 0; i < 2; i++)
                    obj_list[i].Add(Activator.CreateInstance(
                        reflect_class, new object[] { this, 
                            obj_p[i], (char) (65 + i) }) as AbstractEndpoint);
            }

            for (int i = 0; i < 2; i++) {
                type_cb[i].Tag = i;
                type_cb[i].DataSource = obj_list[i];
                type_cb[i].DisplayMember = "Name";
                type_cb[i].SelectedIndex = 0;
            }

            dgWaypoints.Rows.Clear();

            bsGameObjectsA.DataSource = DataManager.GameData;
            bsGameObjectsB.DataSource = DataManager.GameData;

            bsQuestListA.DataSource = DataManager.GameData;
            bsQuestListB.DataSource = DataManager.GameData;
#if DEBUG
            if (ProcessManager.Config.Test == 3)
            {
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

        private bool CheckEndpoints()
        {
            for (int i = 0; i < 2; i++)
            {
                ComboBox cb = type_cb[i];
                if (cb.SelectedItem == null)
                {
                    ShowErrorMessage("Type for Endpoint " + (char)(65 + i) + " not selected");
                    return false;
                }
            }

            return true;
        }

        private void btnControl_Click(object sender, EventArgs e)
        {
            if (!CheckEndpoints())
                return;

            if (_rec_state == RecStates.IDLE)
            {
                if (!((AbstractEndpoint) cbTypeA.SelectedItem).Check ||
                    (!(CheckInGame() && ResetRoute())))
                    return;

                
                // Set from zone
#if DEBUG
                if (ProcessManager.Config.Test == 3)
                    tbZoneA.Text = "Teldrassil";
                else
#endif
                tbZoneA.Text = ProcessManager.Player.ZoneText;


#if DEBUG
                if (ProcessManager.Player != null)
                {
#endif
                    if (ProcessManager.Player.StateMachine.IsRunning)
                    {
                        ShowErrorMessage("Bot is running. Stop it first before recording");
                        return;
                    }

                    ProcessManager.Player.StateMachine.ChangeState(
                                new RouteRecordingState(RecordWp, numRecDistance.Value), false, true);
                    ProcessManager.Player.StateMachine.IsRunning = true;
#if DEBUG
                }
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
                if (ProcessManager.Player != null)
                {
#endif
                // Put state machine back in idle status
                ProcessManager.Player.StateMachine.IsRunning = false;
                ProcessManager.Player.StateMachine.ChangeState(null, false, true);
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

            gbRouteDetails.Enabled = !start_state;
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
            NpcHelper.MoveToDest(new Vector3D(Convert.ToDouble(row.Cells[0]),
                Convert.ToDouble(row.Cells[1]), Convert.ToDouble(row.Cells[2])), "record");
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Check if B is set
            if (!(CheckEndpoints() && ((AbstractEndpoint) cbTypeB.SelectedItem).Check))
                return;

            // Save route
            // TODO

            IsChanged = false;
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

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void cbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cbs = (ComboBox)sender;
            if (cbs.SelectedItem == null)
                return;

            ((AbstractEndpoint)cbs.SelectedItem).OnSelection();
        }

        private void bsGameObjects1_CurrentChanged(object sender, EventArgs e)
        {
            if (cbObjA0.SelectedItem == null)
                return;

            bsGameObjectsB.Filter = "ID <>" + ((DataRowView)cbObjA0.SelectedItem).Row["ID"].ToString();
        }
    }

    #region Classes for Reflection

    public abstract class AbstractEndpoint
    {
        public virtual bool Check
        {
            get { return true; }
        }
        public abstract string Name{ get; }

        public abstract void OnSelection();
    }

    public class UndefEndpoint : AbstractEndpoint
    {
        protected char C;
        protected Panel[] PTargets = new Panel[3];

        protected RouteRecorderForm Owner;
        protected ComboBox[] cb_list = new ComboBox[2];

        public override string Name
        {
            get { return "undef"; }
        }

        public UndefEndpoint() { }

        public UndefEndpoint(RouteRecorderForm owner, Panel target, char a_b)
        {
            C = a_b;
            Owner = owner;
            PTargets[0] = target;
            for (int i = 0; i < 2; i++)
            {
                PTargets[i + 1] = (Panel)PTargets[i].Tag;
                cb_list[i] = (ComboBox)PTargets[i + 1].Controls["cbObj" + C + i];
            }
        }

        public override bool Check
        {
            get
            {
                return GenericDialog.GetConfirmation(Owner,
                  "Continue with undefined Endpoint " + C + " ?");
            }
        }

        public override void OnSelection()
        {
            SetControls(false, 2);
            foreach (ComboBox cb in cb_list)
                cb.DataSource = null;
        }

        protected void SetControls(bool enabled, int cnt)
        {
            for (int i = 0; i < cnt; i++)
                PTargets[i + 1].Visible = enabled;
        }
    }

    public abstract class AbstractDefEndpoint : UndefEndpoint
    {
        protected readonly BindingSource[] bs;
        private string[] DisplayMembers;
        private int _bs_cnt;
        protected abstract string TypeStr { get; }

        public AbstractDefEndpoint(string bs_name) : base() {}

        public AbstractDefEndpoint(RouteRecorderForm owner,
                        Panel target, char a_b, string[] bs_list, string[] members)
            : base(owner, target, a_b) 
        {
            _bs_cnt = bs_list.Length;
            DisplayMembers = members;
            bs = new BindingSource[_bs_cnt];

            for (int i = 0; i < bs_list.Length; i++)
                bs[i] = FindBindingSource(bs_list[i]);    
        }

        private BindingSource FindBindingSource(string name)
        {
            // Check for binding source
            FieldInfo[] fi = Owner.GetType().GetFields();

            foreach (FieldInfo f in fi)
                if (f.Name.Equals(name + C))
                    return (BindingSource) f.GetValue(Owner);

            return null;
        }

        public override bool Check
        {
            get
            {
                if (cb_list[0].SelectedItem == null)
                {
                    GenericDialog.ShowErrorMessage(Owner, 
                        "Select " + TypeStr + " for Point " + C);
                    return false;
                }

                return true;
            }
        }

        public override void OnSelection()
        {
            base.OnSelection();
            SetControls(true, _bs_cnt);

            for (int i = 0; i < _bs_cnt; i++)
                SetBindingSource(cb_list[i], i, DisplayMembers[i]);
        }

        protected void SetBindingSource(ComboBox cb, int idx, string member)
        {
            cb.DataSource = bs[idx];
            cb.DisplayMember = member;
            ((Label)cb.Tag).Text = DisplayMembers[idx][0] + 
                    DisplayMembers[idx].Substring(1).ToLower();
        }
    }

    public class GameObjEndpoint : AbstractDefEndpoint
    {
        public GameObjEndpoint(RouteRecorderForm owner, Panel target, char a_b)
            : base(owner, target, a_b, new string[] { "bsGameObjects" }, 
                                                new string[] { "NAME" } ) {}

        public override string Name
        {
            get { return "game_object"; }
        }

        protected override string TypeStr
        {
            get { return "Game Object"; }
        }
    }

    public class QuestObjEndpoint : AbstractDefEndpoint
    {
        public QuestObjEndpoint(RouteRecorderForm owner, Panel target, char a_b)
            : base(owner, target, a_b,
                new string[] { "bsQuestList", "fkQuestItems" },
                            new string[] { "TITLE", "NAME" }) { }

        public override string Name
        {
            get { return "quest_objective"; }
        }

        public override bool Check
        {
            get
            {
                if (!base.Check || (cb_list[1].SelectedItem == null))
                {
                    GenericDialog.ShowErrorMessage(Owner,
                        "Select Quest Objective for Point " + C);
                    return false;
                }

                return true;
            }
        }

        protected override string TypeStr
        {
            get { return "Quest"; }
        }
    }

    public class HotSpotEndpoint : UndefEndpoint
    {
        public HotSpotEndpoint(RouteRecorderForm owner, Panel target, char a_b)
            : base(owner, target, a_b) { }

        public override string Name
        {
            get { return "hot_spot"; }
        }
    }

    public class GraveyardEndpoint : UndefEndpoint
    {
        public GraveyardEndpoint(RouteRecorderForm owner, Panel target, char a_b)
            : base(owner, target, a_b) { }

        public override string Name
        {
            get { return "graveyard"; }
        }
    }
    
    #endregion
}
