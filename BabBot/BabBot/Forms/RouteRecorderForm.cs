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
        ComboBox[] type_cb;
        RecStates _rec_state = RecStates.IDLE;
        RouteRecordingState _route_rec_state;

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

            List<AbstractListEndpoint>[] obj_list = new List<AbstractListEndpoint>[] {
                new List<AbstractListEndpoint>(), new List<AbstractListEndpoint>() };

            foreach (EndpointTypes ept in Enum.GetValues(typeof(EndpointTypes)))
            {
                string class_name = Output.GetLogNameByLfs(
                    Enum.GetName(typeof(EndpointTypes), ept).ToLower(), "") + "ListEndpoint";

                Type reflect_class = Type.GetType("BabBot.Forms." + class_name);

                for (int i = 0; i < 2; i++)
                    obj_list[i].Add(Activator.CreateInstance(
                        reflect_class, new object[] { this, 
                            obj_p[i], (char) (65 + i) }) as AbstractListEndpoint);
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
                tbZoneA.Text = "Teldrassil";
                tbZoneB.Text = "Teldrassil";

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
                if (!((AbstractListEndpoint) cbTypeA.SelectedItem).Check ||
                    (!(CheckInGame() && ResetRoute())))
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
                    ProcessManager.Player.setCurrentMapInfo();
                    tbZoneA.Text = ProcessManager.Player.ZoneText;

                    // Load RouteRecordingState and start
                    _route_rec_state = new RouteRecordingState(RecordWp, numRecDistance.Value);
                    ProcessManager.Player.StateMachine.ChangeState(_route_rec_state, false, true);
                    ProcessManager.Player.StateMachine.IsRunning = true;
#if DEBUG
                } else tbZoneA.Text = "Teldrassil";
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
                tbZoneB.Text = "Teldrassil";
                if (ProcessManager.Player != null)
                {
#endif
                // Put state machine back in idle status
                _route_rec_state.Exit(ProcessManager.Player);
                ProcessManager.Player.StateMachine.IsRunning = false;

                ProcessManager.Player.setCurrentMapInfo();
                tbZoneB.Text = ProcessManager.Player.ZoneText;
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Check if B is set
            if (!(CheckEndpoints() && ((AbstractListEndpoint) cbTypeB.SelectedItem).Check))
                return;

            // Make route
            AbstractListEndpoint point_a = cbTypeA.SelectedItem as AbstractListEndpoint;
            AbstractListEndpoint point_b = cbTypeB.SelectedItem as AbstractListEndpoint;
            Route route = new Route(
                point_a.GetEndpoint(tbZoneA.Text,MakeVector(dgWaypoints.Rows[0])),
                point_b.GetEndpoint(tbZoneB.Text, 
                        MakeVector(dgWaypoints.Rows[dgWaypoints.Rows.Count - 2])), 
                tbDescr.Text, cbReversible.Checked);

            // Add waypoints except  last empty line
            Waypoints waypoints = new Waypoints(route.WaypointFileName);
            for (int i = 0; i < dgWaypoints.Rows.Count - 1; i++)
                waypoints.List.Add(MakeVector(dgWaypoints.Rows[i]));

            // Save route
            if (RouteListManager.SaveRoute(route, waypoints))
            {
                string s = "Route successfully saved";

                if (route.FileName == null)
                    s += ". No export file generated.";
                else
                    s += ".\nExport data located in the file '" + route.FileName + "'";
                
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

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void cbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cbs = (ComboBox)sender;
            if (cbs.SelectedItem == null)
                return;

            ((AbstractListEndpoint)cbs.SelectedItem).OnSelection();

            IsChanged = true;
        }

        private void bsGameObjectsA_CurrentChanged(object sender, EventArgs e)
        {
            if (bsGameObjectsA.Current == null)
                return;

            bsGameObjectsB.Filter = "ID <> " + ((DataRowView)bsGameObjectsA.Current).Row["ID"].ToString();
        }

        private void RegisterChanges(object sender, EventArgs e)
        {
            IsChanged = true;
        }

        private void cbObjA0_DataSourceChanged(object sender, EventArgs e)
        {
            if (cbObjA0.DataSource != bsGameObjectsA)
                bsGameObjectsB.Filter = null;
            else
                bsGameObjectsA_CurrentChanged(sender, e);
        }
    }

    #region Classes for Reflection

    public abstract class AbstractListEndpoint
    {
        public virtual bool Check
        {
            get { return true; }
        }

        public abstract EndpointTypes Type { get; }

        public abstract string Name{ get; }

        public abstract void OnSelection();

        public abstract Endpoint GetEndpoint(string ZoneText, Vector3D waypoint);
    }

    public class UndefListEndpoint : AbstractListEndpoint
    {
        protected char C;
        protected Panel[] PTargets = new Panel[3];

        protected RouteRecorderForm Owner;
        protected ComboBox[] cb_list = new ComboBox[2];

        public override string Name
        {
            get { return "undef"; }
        }

        public override EndpointTypes Type
        {
            get { return EndpointTypes.UNDEF; }
        }

        public UndefListEndpoint() { }

        public UndefListEndpoint(RouteRecorderForm owner, Panel target, char a_b)
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

        public override Endpoint GetEndpoint(string ZoneText, Vector3D waypoint)
        {
            return new Endpoint(Type, ZoneText, waypoint);
        }
    }

    public abstract class AbstractDefListEndpoint : UndefListEndpoint
    {
        protected readonly BindingSource[] bs;
        private string[] DisplayMembers;
        private int _bs_cnt;
        protected abstract string TypeStr { get; }

        public AbstractDefListEndpoint(string bs_name) : base() {}

        public AbstractDefListEndpoint(RouteRecorderForm owner,
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

    public class GameObjListEndpoint : AbstractDefListEndpoint
    {
        public GameObjListEndpoint(RouteRecorderForm owner, Panel target, char a_b)
            : base(owner, target, a_b, new string[] { "bsGameObjects" }, 
                                                new string[] { "NAME" } ) {}

        public override string Name
        {
            get { return "game_object"; }
        }

        public override EndpointTypes Type
        {
            get { return EndpointTypes.GAME_OBJ; }
        }

        public override Endpoint GetEndpoint(string ZoneText, Vector3D waypoint)
        {
            return new GameObjEndpoint(Type, cb_list[0].Text, ZoneText, waypoint);
        }

        protected override string TypeStr
        {
            get { return "Game Object"; }
        }
    }

    public class QuestObjListEndpoint : AbstractDefListEndpoint
    {
        public QuestObjListEndpoint(RouteRecorderForm owner, Panel target, char a_b)
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

        public override Endpoint GetEndpoint(string ZoneText, Vector3D waypoint)
        {
            BotDataSet.QuestListRow row = (BotDataSet.QuestListRow) ((DataRowView) 
                ((BindingSource)cb_list[0].DataSource).Current).Row;
            return new QuestItemEndpoint(Type, row.ID, cb_list[1].Text, ZoneText, waypoint);
        }


    }

    public class HotSpotListEndpoint : UndefListEndpoint
    {
        public HotSpotListEndpoint(RouteRecorderForm owner, Panel target, char a_b)
            : base(owner, target, a_b) { }

        public override string Name
        {
            get { return "hot_spot"; }
        }

        public override EndpointTypes Type
        {
            get { return EndpointTypes.HOT_SPOT; }
        }
    }

    public class GraveyardListEndpoint : UndefListEndpoint
    {
        public GraveyardListEndpoint(RouteRecorderForm owner, Panel target, char a_b)
            : base(owner, target, a_b) { }

        public override string Name
        {
            get { return "graveyard"; }
        }

        public override EndpointTypes Type
        {
            get { return EndpointTypes.GRAVEYARD; }
        }
    }
    
    #endregion
}
