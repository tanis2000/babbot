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
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using System.Globalization;
using System.Drawing;
using BabBot.Bot;
using BabBot.Common;
using BabBot.Manager;
using BabBot.Wow;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BabBot.Forms.Radar;
using System.Runtime.InteropServices;

namespace BabBot.Forms
{
    public partial class MainForm : Form
    {
        private Hashtable LogFS = new Hashtable();
        private Radar.Radar Radar;
        private AppOptionsForm AppOptionsForm;
        private OptionsForm BotOptionsForm;
        private NPCListForm NPCListForm;
        // number of lines in logging window
        private int _log_len = 500;

        public MainForm()
        {
            InitializeComponent();

            // Log Output controlled by Config.LogOutput parameter
            Output.OutputEvent += LogOutput;
            Output.Instance.Log("char", "Initializing..");

            // Process manager first
            ProcessManager.Initialize(
                OnFirstTimeRun, OnConfigFileChanged, ShowErrorMessage);

            // Custom initialization of some components
            Initialize();
            Output.Instance.Log("char", "Initialization done.");

            // ProcessManager events binding
            ProcessManager.WoWProcessStarted += wow_ProcessStarted;
            ProcessManager.WoWProcessEnded += wow_ProcessEnded;
            ProcessManager.WoWProcessFailed += wow_ProcessFailed;
            ProcessManager.WoWProcessAccessFailed += wow_ProcessAccessFailed;
            ProcessManager.WoWInGame += wow_InGame;

            // Starts the bot thread
            ProcessManager.PlayerUpdate += PlayerUpdate;
            ProcessManager.PlayerWayPoint += PlayerWayPoint;
            ProcessManager.UpdateStatus += UpdateStatus;
        }

        #region Exception Handler

        public void UnhandledThreadExceptionHandler(object sender, ThreadExceptionEventArgs e)
        {
            HandleUnhandledException(e.Exception);
        }

        public void HandleUnhandledException(Exception e)
        {
            // do what you want here.
            if (MessageBox.Show("An unexpected error has occurred. Continue?",
                                "My application", MessageBoxButtons.YesNo, MessageBoxIcon.Stop,
                                MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                Application.Exit();
            }
        }

        #endregion

        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0xBEEF)
            {
                Output.Instance.Debug("0xBEEF message recieved, resuming main WoW thread!", this);

                ProcessManager.ResumeMainWowThread();
            }

            base.WndProc(ref m);
        }

        private void Initialize()
        {
            // Set a default value for waypoint type combo
            comboWayPointTypes.SelectedIndex = 0;

            AppHelper.BotHandle = this.Handle;
        }

        private void PlayerWayPoint(Vector3D waypoint)
        {
            if (InvokeRequired)
            {
                PlayerWayPointDelegate del = PlayerWayPoint;
                object[] parameters = {waypoint};
                Invoke(del, parameters);
            }
            else
            {
                /*
                if (cbWPRecord.Checked)
                {
                    lbWayPoints.Items.Insert(0, waypoint.ToString());
                }
                */

                if (cbAutoAddWaypoints.Checked)
                {
                    var wp = new WayPoint(waypoint);
                    switch (comboWayPointTypes.SelectedItem.ToString())
                    {
                        case "Vendor":
                            wp.WPType = WayPointType.Vendor;
                            break;
                        case "Repair":
                            wp.WPType = WayPointType.Repair;
                            break;
                        case "Ghost":
                            wp.WPType = WayPointType.Ghost;
                            break;
                        case "Normal":
                            wp.WPType = WayPointType.Normal;
                            break;
                    }
                    WayPointManager.Instance.AddWayPoint(wp);
                }
            }
        }

        //private WayPoint GetLatestPlayerWayPoint()
        //{
        //    switch (comboWayPointTypes.SelectedItem.ToString())
        //    {
        //        case "Vendor":
        //            return WayPointManager.Instance.VendorPath.Last();
        //        case "Repair":
        //            return WayPointManager.Instance.RepairPath.Last();
        //        case "Ghost":
        //            return WayPointManager.Instance.GhostPath.Last();
        //        case "Normal":
        //            return WayPointManager.Instance.NormalPath.Last();
        //    }

        //    throw new Exception("Unrecognized WayPoint type " + comboWayPointTypes.SelectedItem.ToString());
        //}

        private void PlayerUpdate()
        {
            if (InvokeRequired)
            {
                PlayerUpdateDelegate del = PlayerUpdate;
                BeginInvoke(del);
            }
            else
            {
                if (ProcessManager.ProcessRunning)
                {
                    //Misc info updates for the "Player" tab

                    WowPlayer Player = ProcessManager.Player;
                    try
                    {
#if DEBUG
                        tbLocation.Text = String.Format("Loc: {0}, {1}, {2} | {3}", Player.Location.X,
                                                Player.Location.Y, Player.Location.Z, Player.CurTargetGuid);


                        tbOrientation.Text = String.Format("Or.: {0}", ProcessManager.Player.Orientation);
                        tbPlayerHp.Text = Player.Hp.ToString();
                        tbPlayerMaxHp.Text = ProcessManager.Player.MaxHp.ToString();
                        tbPlayerMp.Text = ProcessManager.Player.Mp.ToString();
                        tbPlayerMaxMp.Text = ProcessManager.Player.MaxMp.ToString();
                        tbPlayerXp.Text = ProcessManager.Player.Xp.ToString();
                        tbPlayerTarget.Text = string.Format("{0:X}", ProcessManager.Player.CurTargetGuid);
                        tbPlayerTargetName.Text = ProcessManager.Player.CurTargetName;
                        tbPlayerNearObjects.Text = "Objects" + Environment.NewLine + "===========" + Environment.NewLine +
                                                   ProcessManager.Player.NearObjectsAsTextList + Environment.NewLine +
                                                   "Mobs" +
                                                   Environment.NewLine +
                                                   "===========" + Environment.NewLine +
                                                   ProcessManager.Player.NearMobsAsTextList;
#endif
                        // Update radar
                        Radar.AddCenter(Player.Guid, Player.Location, Player.Orientation);

                        List<WowObject> AllObj = Player.GetNearObjects();

                        // Add Mobs
                        foreach (WowObject wobj in AllObj)
                        {
                            switch (wobj.Type)
                            {
                                case Descriptor.eObjType.OT_UNIT:
                                    // Add mob
                                    WowUnit unit = (WowUnit)wobj;
                                    if (unit.IsDead)
                                        // Draw as a circle
                                        Radar.AddItem(unit.Guid, unit.Location,
                                        ((unit.IsLootable) ? Color.Gray : ((unit.IsSkinnable) ?
                                        Color.LightSteelBlue : Color.Silver)));
                                    else
                                        // Draw as triangle with orientation
                                        Radar.AddItem(unit.Guid, unit.Location, unit.Orientation,
                                            ((unit.IsAggro) ? Color.Red : ((unit.IsNpc) ?
                                            Color.Yellow : Color.Blue)));
                                    break;

                                case Descriptor.eObjType.OT_PLAYER:
                                    // Add Player
                                    unit = (WowUnit)wobj;
                                    if (unit.Guid != Player.Guid)
                                        Radar.AddItem(unit.Guid, unit.Location, unit.Orientation,
                                            ((unit.IsAggro) ? Color.Red :
                                            ((unit.IsDead) ? Color.Silver :
                                              ((unit.IsGhost ? Color.Gray : Color.Green)))));

                                    break;

                                // Resources - fish, herb, vein
                                /* case Descriptor.eObjType.
                                 */
                            }
                        }
                        Radar.Update();

                        tbCorpseX.Text = ProcessManager.Player.CorpseLocation.X.ToString();
                        tbCorpseY.Text = ProcessManager.Player.CorpseLocation.Y.ToString();
                        tbCorpseZ.Text = ProcessManager.Player.CorpseLocation.Z.ToString();

                        txtCurrentX.Text = ProcessManager.Player.Location.X.ToString();
                        txtCurrentY.Text = ProcessManager.Player.Location.Y.ToString();
                        txtCurrentZ.Text = ProcessManager.Player.Location.Z.ToString();
                        txtLastDistance.Text = ProcessManager.Player.LastDistance.ToString();
                        txtFaceRadian.Text = ProcessManager.Player.LastFaceRadian.ToString();

                        var orientation = (float)((ProcessManager.Player.Orientation * 180) / Math.PI);
                        txtCurrentFace.Text = string.Format("{0}°", orientation);

                        var facing = (float)((ProcessManager.Player.LastFaceRadian * 180) / Math.PI);
                        txtComputedFacing.Text = string.Format("{0}°", facing);

                        tbPlayerIsSitting.Text = ProcessManager.Player.IsSitting.ToString();
                        txtTravelTime.Text = string.Format("{0} ms", ProcessManager.Player.TravelTime);

                        tbCountNormal.Text = WayPointManager.Instance.NormalNodeCount.ToString();
                        tbCountVendor.Text = WayPointManager.Instance.VendorNodeCount.ToString();
                        tbCountGhost.Text = WayPointManager.Instance.GhostNodeCount.ToString();
                        tbCountRepair.Text = WayPointManager.Instance.RepairNodeCount.ToString();
                    }
                    catch (Exception e)
                    {
                        // Skip and continue
                        Output.Instance.Debug("char", e.Message);
                    }
                }
            }
        }

        private void wow_ProcessEnded(int process)
        {
            // Cross-Thread operation
            if (InvokeRequired)
            {
                // Setup the cross-thread call
                ProcessEndedDelegate del = wow_ProcessEnded;
                object[] parameters = {process};
                Invoke(del, parameters);
            }
            else
            {
                // Main Thread
               SetCtrlBtns(true);
            }
        }

        private void SetCtrlBtns(bool Enabled)
        {
            btnRun.Enabled = Enabled;
            btnLogin.Enabled = !Enabled;
        }

        private void wow_InGame()
        {
            // Cross-Thread operation
            if (InvokeRequired)
            {
                // Setup the cross-thread call
                InGameDelegate del = wow_InGame;
                Invoke(del);
            }
            else
            {
                btnLogin.Enabled = false;
            }
        }

        private static void wow_ProcessFailed(string error)
        {
            ShowErrorMessage(error);
        }

        private static void ShowErrorMessage(string error)
        {
            MessageBox.Show(error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void wow_ProcessStarted(int process)
        {
            if (InvokeRequired)
            {
                ProcessManager.WoWProcessStartedEventHandler del = wow_ProcessStarted;
                object[] parameters = {process};
                BeginInvoke(del, parameters);
            }
            else
            {
                SetCtrlBtns(false);
            }
        }

        private void OnFirstTimeRun()
        {
            showAppOptionsForm("First time run detected.\nConfigure application settings and save configuration");
        }

        private void OnConfigFileChanged()
        {
            showAppOptionsForm("Configuration file changed.\nCheck and save new configuration");
        }
        
        private void UpdateStatus(string iStatus)
        {
            statusLabel.Text = iStatus;
        }

        private void ActivateDebugMode()
        {
            tabControlMain.TabPages["tabPageDebug"].Enabled = true;
            tabControlMain.TabPages["tabPageDebug2"].Enabled = true;
        }


        private void DeactivateDebugMode()
        {
            tabControlMain.TabPages["tabPageDebug"].Enabled = false;
            tabControlMain.TabPages["tabPageDebug2"].Enabled = false;
        }

        #region UI Event Handlers

        private static void wow_ProcessAccessFailed(string error)
        {
            MessageBox.Show(error, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            // Run wow.exe via bot thread
            ProcessManager.StartBot();
        }

        private void btnFindTLS_Click(object sender, EventArgs e)
        {
            try
            {
                ProcessManager.FindTLS();
                tbTLS.Text = string.Format("{0:X}", ProcessManager.TLS);
                tbClientConnectionPointer.Text = string.Format("{0:X}", Globals.ClientConnectionPointer);
                tbClientConnectionOffset.Text = string.Format("{0:X}", Globals.ClientConnectionOffset);
                tbPlayerBaseOffset.Text = string.Format("{0:X}", Globals.PlayerBaseOffset);
                tbCurMgr.Text = string.Format("{0:X}", Globals.CurMgr);
                tbLocalGUID.Text = ProcessManager.ObjectManager.GetLocalGUID().ToString();
                tbWndHandle.Text = ProcessManager.WowHWND.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnLoadProfile_Click(object sender, EventArgs e)
        {
            var dlg = new OpenFileDialog {Multiselect = false, RestoreDirectory=true, Filter = "BabBot Profile (*.xml)|*.xml"};
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                var serializer = new Serializer<Profile>();
                ProcessManager.Profile = serializer.Load(dlg.FileName);
                ProcessManager.Profile.FileName = dlg.FileName;
                tbProfileName.Text = ProcessManager.Profile.Name;
                tbProfileDescription.Text = ProcessManager.Profile.Description;
                WayPointManager.Instance.NormalPath = ProcessManager.Profile.NormalWayPoints;
                WayPointManager.Instance.VendorPath = ProcessManager.Profile.VendorWayPoints;
                WayPointManager.Instance.RepairPath = ProcessManager.Profile.RepairWayPoints;
                WayPointManager.Instance.GhostPath = ProcessManager.Profile.GhostWayPoints;

                // UI Stuff
                tbProfile.Text = dlg.FileName;
                RefreshEnemiesList();
            }
        }

        private void btnSaveProfile_Click(object sender, EventArgs e)
        {
            var dlg = new SaveFileDialog { RestoreDirectory = true, Filter = "BabBot Profile (*.xml)|*.xml" };
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                ProcessManager.Profile.FileName = dlg.FileName;
                var serializer = new Serializer<Profile>();
                ProcessManager.Profile.NormalWayPoints = WayPointManager.Instance.NormalPath;
                ProcessManager.Profile.VendorWayPoints = WayPointManager.Instance.VendorPath;
                ProcessManager.Profile.RepairWayPoints = WayPointManager.Instance.RepairPath;
                ProcessManager.Profile.GhostWayPoints = WayPointManager.Instance.GhostPath;
                serializer.Save(dlg.FileName, ProcessManager.Profile);

                // UI Stuff
                tbProfile.Text = dlg.FileName;
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var f = new AboutForm();
            f.ShowDialog();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            ProcessManager.BotManager.Stop();
            Process.LeaveDebugMode();

            // Save Bot & Wow positions
            ProcessManager.Config.BotPos = new WinPos(Location, Size, TopMost, Opacity);
            if (ProcessManager.WowHWND != 0)
                ProcessManager.Config.WowPos = new WinPos(BabBot.Common.WindowSize.
                                    GetPositionSize((IntPtr)ProcessManager.WowHWND));
            
            ProcessManager.SaveConfig();
        }

        private void btnMovementTest_Click(object sender, EventArgs e)
        {
            var destPos = new Vector3D();
            try
            {
                destPos.X = float.Parse(txtX.Text);
                destPos.Y = float.Parse(txtY.Text);
                destPos.Z = float.Parse(txtZ.Text);
                ProcessManager.Player.ClickToMove(destPos);
            }
            catch
            {
            }
        }

        private void btnStopMovement_Click(object sender, EventArgs e)
        {
            ProcessManager.Player.Stop();
        }

        private void btnWPTest_Click(object sender, EventArgs e)
        {
            cbWPRecord.Checked = false;
            if (lbWayPoints.Items.Count > 0)
            {
                foreach (string wp in lbWayPoints.Items)
                {
                    string[] arrWp = wp.Split('|');
                    var dest = new Vector3D(float.Parse(arrWp[0]), float.Parse(arrWp[1]), float.Parse(arrWp[2]));
                    if (ProcessManager.Player.MoveTo(dest) == NavigationState.WayPointTimeout)
                    {
                        MessageBox.Show("We are stuck somewhere!");
                        break;
                    }
                }
            }
        }

        private void btnClearWP_Click(object sender, EventArgs e)
        {
            cbWPRecord.Checked = false;
            lbWayPoints.Items.Clear();
        }

        private void cbWPRecord_CheckedChanged(object sender, EventArgs e)
        {
            if (!cbWPRecord.Checked)
            {
                ProcessManager.ResetWayPoint();
            }
        }

        private void tbProfileName_TextChanged(object sender, EventArgs e)
        {
            ProcessManager.Profile.Name = tbProfileName.Text;
        }

        private void tbProfileDescription_TextChanged(object sender, EventArgs e)
        {
            ProcessManager.Profile.Description = tbProfileDescription.Text;
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showAppOptionsForm(null);
        }

        private void showAppOptionsForm(string msg)
        {
            if (AppOptionsForm == null)
                AppOptionsForm = new AppOptionsForm();

            AppOptionsForm.Msg = msg;
            AppOptionsForm.TopMost = this.TopMost;
            DialogResult res = AppOptionsForm.ShowDialog();

            if (res == DialogResult.OK)
            {
                ProcessManager.SaveConfig();
                Text = ProcessManager.Config.CustomParams.WinTitle;
                ProcessManager.Injector.SetPatchOffset(
                    Convert.ToUInt32(ProcessManager.Config.CustomParams.LuaCallback, 16));

                if (ProcessManager.Config.DebugMode)
                    ActivateDebugMode();
                else
                    DeactivateDebugMode();
            }
            else
            {
                if (msg != null)
                    // Configuration wasn't saved
                    Environment.Exit(3);
            }
        }

        private void botOptionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (BotOptionsForm == null)
                BotOptionsForm = new OptionsForm();

            BotOptionsForm.TopMost = this.TopMost;
            DialogResult res = BotOptionsForm.ShowDialog();

            if (res == DialogResult.OK)
                ProcessManager.SaveConfig();
        }

        private void btnStartBot_Click(object sender, EventArgs e)
        {
            Output.Instance.Log("Starting StateMachine");
            ////get location of nearest mobs and then create a circle path between some MPC's
            //List<WowObject> mobs = Player.GetNearObjects();

            //Path p = new Path();

            ////first waypoint
            //var d1 = from c in mobs where c.Type == Descriptor.eObjType.OT_UNIT && c.Name == "Balir Frosthammer" select c;
            //p.AddLast(WaypointVector3DHelper.Vector3DToLocation(d1.First<WowObject>().Location));

            ////second waypoint
            //var d2 = from c in mobs where c.Type == Descriptor.eObjType.OT_UNIT && c.Name == "Sten Stoutarm" select c;
            //p.AddLast(WaypointVector3DHelper.Vector3DToLocation(d2.First<WowObject>().Location));

            ////third waypoint
            //var d3 = from c in mobs where c.Type == Descriptor.eObjType.OT_UNIT && c.Name == "Adlin Pridedrift" select c;
            //p.AddLast(WaypointVector3DHelper.Vector3DToLocation(d3.First<WowObject>().Location));

            ////back to first
            //p.AddLast(WaypointVector3DHelper.Vector3DToLocation(d1.First<WowObject>().Location));


            //lets try and calculate a path from the dwarf starting area to IronForge and then walk it
            //Path p =
            //    Caronte.CalculatePath(
            //        WaypointVector3DHelper.Vector3DToLocation(Player.Location),
            //        new Location(-6003.86f, -232.1742f, 410.5543f));


            if (ProcessManager.Player == null)
            {
                Output.Instance.Log("Cannot start StateMachine, we are not in the game yet");
                return;
            }

            /*
            //if a normal path exists, use it, else exit and don't start
            // TODO: this is wrong, we should start the state machine even if there's no waypoints (I guess we're doing this because the test script is just a pathing one for now)
            if (WayPointManager.Instance.NormalNodeCount <= 0)
                return;

            Pather.Graph.Path p = new Pather.Graph.Path();

            //add each waypoint in the waypoint list
            foreach (WayPoint wp in WayPointManager.Instance.NormalPath)
            {
                p.AddLast(WaypointVector3DHelper.Vector3DToLocation(wp.Location));
            }

            ProcessManager.Player.StateMachine.SetGlobalState(new BabBot.States.Common.MoveToState(p));
            */

            ProcessManager.Player.StateMachine.IsRunning = true;
            //StateManager.Instance.Start();
            Output.Instance.Log("StateMachine started");
        }

        private void btnStopBot_Click(object sender, EventArgs e)
        {
            Output.Instance.Log("Stopping StateMachine");

            if (ProcessManager.Player == null)
            {
                Output.Instance.Log("Cannot stop StateMachine, we are not in the game yet");
                return;
            }
            
            ProcessManager.Player.StateMachine.IsRunning = false;
            //StateManager.Instance.Stop();
            Output.Instance.Log("StateMachine stopped");
        }

        private void btnAddEnemyToList_Click(object sender, EventArgs e)
        {
            if (ProcessManager.Player.CurTargetName != "")
            {
                var enemy = new Enemy(ProcessManager.Player.CurTargetName);
                ProcessManager.Profile.Enemies.Add(enemy);
                RefreshEnemiesList();
            }
        }

        private void btnRemoveEnemyFromList_Click(object sender, EventArgs e)
        {
            if (lbEnemies.Items.Count == 0)
            {
                return;
            }

            if (lbEnemies.SelectedItem != null)
            {
                var enemy = new Enemy(lbEnemies.SelectedItem.ToString());
                ProcessManager.Profile.Enemies.Remove(enemy);
                RefreshEnemiesList();
            }
        }

        private void RefreshEnemiesList()
        {
            lbEnemies.Items.Clear();
            foreach (Enemy en in ProcessManager.Profile.Enemies)
            {
                lbEnemies.Items.Add(en.Name);
            }
        }

        private void btnDumpBagsToConsole_Click(object sender, EventArgs e)
        {
            Output.Instance.Debug(ProcessManager.Player.BagsAsTextList, this);
        }

        private void btnInteract_Click(object sender, EventArgs e)
        {
            if (ProcessManager.Player.CurTargetGuid != 0)
            {
                WowUnit u = ProcessManager.Player.GetCurTarget();
                if (u != null)
                {
                    u.Interact();
                }
            }
        }

        private void btnDoString_Click(object sender, EventArgs e)
        {
            ProcessManager.Injector.Lua_DoString(tbLuaScript.Text);
        }

        private void btnInputHandler_Click(object sender, EventArgs e)
        {
            ProcessManager.Injector.Lua_DoStringEx(tbLuaScript.Text);
            btnGetLuaText_Click(sender, e);
        }

        private void btnGetLuaText_Click(object sender, EventArgs e)
        {
            List<string> s = ProcessManager.Injector.Lua_GetValues();
            tbLuaResult.Clear();

            if (s.Count > 0)
                foreach (string tmp in s)
                {
                    tbLuaResult.Text += tmp + Environment.NewLine;
                }
            else
                tbLuaResult.Text = "null";
        }

        #endregion

        #region Nested type: PlayerUpdateDelegate

        private delegate void PlayerUpdateDelegate();

        #endregion

        #region Nested type: PlayerWayPointDelegate

        private delegate void PlayerWayPointDelegate(Vector3D waypoint);

        #endregion

        #region Nested type: ProcessEndedDelegate

        private delegate void ProcessEndedDelegate(int process);
        private delegate void InGameDelegate();

        #endregion

        #region Logging

        internal void LogOutput(string facility, string time, string message, Color color)
        {
            if (!ProcessManager.Config.LogParams.DisplayLogs) return;

            if (txtConsole.InvokeRequired)
            {
                Output.OutputEventHandler handler = LogOutput;
                BeginInvoke(handler, new object[] { facility, time, message, color });
            }
            else
            {
                try
                {
                    // Check if facility exists
                    // TODO: if we log something and we're terminating the process, rt will throw an exception!

                    RichTextBox rt = (RichTextBox) LogFS[facility];
                    if (rt == null)
                    {
                        TabPage tab = new TabPage(facility);
                        rt = new RichTextBox();
                        rt.Size = txtConsole.Size;
                        rt.BackColor = txtConsole.BackColor;

                        tab.Controls.Add(rt);

                        LogFS.Add(facility, rt);
                        // Insert "char" tab first
                        // some bug in implementation
                        if (tabLogs.TabPages.Count == 1)
                        {
                            TabPage cur = tabAll;
                            tabLogs.TabPages.Remove(tabAll);
                            tabLogs.TabPages.Add(tab);
                            tabLogs.TabPages.Add(tabAll);
                        }
                        else
                            tabLogs.TabPages.Insert(1, tab);
                    }

                    string text = String.Format(CultureInfo.CurrentCulture, "[{0}] {1}{2}",
                                                time, message, Environment.NewLine);
                    
                    // Append to facility log
                    AppendText(rt, text, color);

                    // Append to All logs as well
                    AppendText(txtConsole, text, color);
                } 
                catch(Exception)
                {
                    
                }
            }
        }

        private void AppendText(RichTextBox rtb, string text, Color color)
        {
            rtb.SelectionColor = color;
            rtb.AppendText(text);
            rtb.ScrollToCaret();

            // Check if limit exceed
            if (rtb.Lines.Length > _log_len)
            {

                int count = rtb.Lines.Length - _log_len;
                for (int i = 0; i < count; i++)
                {
                    rtb.SelectionStart = 0;
                    rtb.SelectionLength = rtb.Lines[0].Length + 1;
                    rtb.ReadOnly = false;
                    rtb.SelectedText = "";
                    rtb.ReadOnly = true;
                    rtb.SelectionStart = rtb.Text.Length;
                }
            }
        }

        #endregion

        private void btnLoadScript_Click(object sender, EventArgs e)
        {
            if (ProcessManager.Player == null)
            {
                Output.Instance.Log("Cannot load a script as the bot has not yet been initialized. Make sure that WoW is running and that a character has been logged in.");
                return;
            }

            Output.Instance.Log("Path before load: " + Environment.CurrentDirectory);
            var dlg = new OpenFileDialog { RestoreDirectory = true, Multiselect = false, Filter = "BabBot Script (*.cs)|*.cs" };
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                // Make sure the State Machine is stopped
                ProcessManager.Player.StateMachine.IsRunning = false;

                Output.Instance.Log("Path before host start: " + Environment.CurrentDirectory);
                ProcessManager.ScriptHost.Start(dlg.FileName);
                Output.Instance.Log("Path after host start: " + Environment.CurrentDirectory);

                // UI Stuff
                tbScript.Text = dlg.FileName;
            }
            Output.Instance.Log("Path after load: " + Environment.CurrentDirectory);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Text = ProcessManager.Config.CustomParams.WinTitle;

            if (ProcessManager.Config.BotPos != null)
            {
                Location = ProcessManager.Config.BotPos.Pos.Location;
                Size = ProcessManager.Config.BotPos.Pos.Size;
				hsOpacity.Value = Convert.ToInt32(ProcessManager.Config.BotPos.Opacity * 100);
                hsOpacity_Scroll(sender, null);
				cbStayOnTop.Checked = ProcessManager.Config.BotPos.TopMost;
                cbStayOnTop_CheckedChanged(sender, e);
            }

            Radar = new Radar.Radar(imgRadar);
            SetDebugBtns();

            if (ProcessManager.Config.DebugMode)
                ActivateDebugMode();
            else
                DeactivateDebugMode();

            // Always last
            if (ProcessManager.AutoRun)
            {
                // Start wow for now
                btnRun_Click(sender, e);
            }

            /// Test
            if (ProcessManager.Config.Test == 1)
                talentTemplatesToolStripMenuItem_Click(sender, e);
        }

        private void SetDebugBtns()
        {
            btnRegisterInputHandler.Enabled = !ProcessManager.Injector.IsLuaRegistered;
            btnUnregisterInputHandler.Enabled = !btnRegisterInputHandler.Enabled;
        }

        private void btnResetBot_Click(object sender, EventArgs e)
        {
            Output.Instance.Log("Resetting StateMachine");

            if (ProcessManager.Player == null)
            {
                Output.Instance.Log("Cannot reset the StateMachine, we are not in the game yet");
                return;
            }

            
            ProcessManager.Player.StateMachine.SetGlobalState(ProcessManager.Player.StateMachine.GlobalState);

            Output.Instance.Log("StateMachine resetted");
        }

        private void btnAddWayPoint_Click(object sender, EventArgs e)
        {

        }

        private void btnClearNormalWP_Click(object sender, EventArgs e)
        {
            WayPointManager.Instance.ClearWaypoints(WayPointType.Normal);
        }

        private void btnClearGhostWP_Click(object sender, EventArgs e)
        {
            WayPointManager.Instance.ClearWaypoints(WayPointType.Ghost);
        }

        private void btnClearVendorWP_Click(object sender, EventArgs e)
        {
            WayPointManager.Instance.ClearWaypoints(WayPointType.Vendor);
        }

        private void btnClearRepairWP_Click(object sender, EventArgs e)
        {
            WayPointManager.Instance.ClearWaypoints(WayPointType.Repair);
        }

        private void cbStayOnTop_CheckedChanged(object sender, EventArgs e)
        {
            TopMost = cbStayOnTop.Checked;
        }

        private void hsOpacity_Scroll(object sender, ScrollEventArgs e)
        {
            double x = Convert.ToDouble(hsOpacity.Value)/100;
            Opacity = x;
        }

        private void btnRegisterInputHandler_Click(object sender, EventArgs e)
        {
            ProcessManager.Injector.Lua_RegisterInputHandler();
            SetDebugBtns();
        }

        private void btnUnregisterInputHandler_Click(object sender, EventArgs e)
        {
            ProcessManager.Injector.Lua_UnRegisterInputHandler();
            SetDebugBtns();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            LoginInfo Account = ProcessManager.Config.Account;
            ProcessManager.Injector.Lua_RegisterInputHandler();
            Login.AutoLogin(Account.RealmLocation, Account.GameType,
                Account.Realm, Account.LoginUsername, Account.getAutoLoginPassword(), 
                ProcessManager.Config.Character, 5);
        }

        private void btnUp_KeyDown(object sender, KeyEventArgs e)
        {
            ProcessManager.CommandManager.SendArrowKey(CommandManager.ArrowKey.Up);
        }

        private void btnLeft_KeyDown(object sender, KeyEventArgs e)
        {
            ProcessManager.CommandManager.SendArrowKey(CommandManager.ArrowKey.Left);
        }

        private void btnDown_KeyDown(object sender, KeyEventArgs e)
        {
            ProcessManager.CommandManager.SendArrowKey(CommandManager.ArrowKey.Down);
        }

        private void btnRight_KeyDown(object sender, KeyEventArgs e)
        {
            ProcessManager.CommandManager.SendArrowKey(CommandManager.ArrowKey.Right);
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            ProcessManager.CommandManager.SendArrowKey(CommandManager.ArrowKey.Up);
        }

        private void tbZoom_Scroll(object sender, EventArgs e)
        {
            Radar.Zoom = tbZoom.Value;
        }

        private void talentTemplatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Create every time new dialog. Don't save old editor
            TalentsForm form = new TalentsForm();
            form.TopMost = this.TopMost;
            form.ShowDialog();
            form = null;
        }

        private void cbTalentTemplates_DropDown(object sender, EventArgs e)
        {
            // Remember last selection
            string saved = cbTalentTemplates.Text;

            cbTalentTemplates.DataSource = null;

            cbTalentTemplates.DataSource = ProcessManager.TalentTemplateList;
            cbTalentTemplates.SelectedIndex = -1;


            // Restore back edited template
            cbTalentTemplates.Text = saved;
        }

        private void btnLearnTalents_Click(object sender, EventArgs e)
        {
            if (cbTalentTemplates.SelectedItem == null)
            {
                MessageBox.Show("Select talent tempalte from the list");
                return;
            }

            if (!CheckInGame())
                return;

            Talents t = (Talents)cbTalentTemplates.SelectedItem;

            if (t == null)
            {
                MessageBox.Show("Talents list is empty");
                return;
            }

            // Switch to main tabl
            tabControlMain.SelectedIndex = 0;

            // Start learning
            LearnAllTalents(t);
        }

        #region Talents

        internal void LearnAllTalents(Talents t)
        {
            bool all_learned = false;
            while (!all_learned)
                try
                {
                    all_learned = (LearnTalents(t) == 0);
                }
                catch (TalentLearnException te)
                {
                    // Register error but still can continue
                    Output.Instance.LogError("char", te.Message);
                    break;
                }
                catch (Exception ex)
                {
                    // Fatal error
                    Output.Instance.LogError("char", ex);
                    break;
                }
        }

        internal int LearnTalents(Talents t)
        {
            // Check how many talent point available
            Output.Instance.Log("char", "Checking for the number of talent points available ...");
            string[] lret = ProcessManager.Injector.Lua_ExecByName("GetAvailTalentPoints");
            
            int points = 0;
            int delay = ProcessManager.CurWoWVersion.TalentConfig.Delay;

            try
            {
                points = Convert.ToInt32(lret[0]);
            }
            catch (Exception ex)
            {
                throw new Exception("Unable retrieve number of talent points.", ex);
            }

            Output.Instance.Log("char", points + " talent points currently available");
            if (points == 0)
                return 0;

            int lvl = ProcessManager.Player.Level;

            int cur_lvl = lvl - points + 1;
            Output.Instance.Log("char", "Looking up talent " + " for lvl " + cur_lvl);

            Level l = t.GetLevel(cur_lvl - ProcessManager.CurWoWVersion.TalentConfig.StartLevel);
            Output.Instance.Debug("char", "Checking talent " + l.TalentToString());

            TalentInfo t1 = GetTalentInfo(l.TabId, l.TalentId);

            Output.Instance.Debug(string.Format("Located talent: '{0}' rank: {1}; meets: {2}",
                    t1.Name, t1.Rank, t1.Meets));

            // Check if any talents already learned in a past
            if (t1.Meets && (t1.Rank == (l.Rank - 1)))
                    LearnTalent(l, t1, delay);
            else {
                // Go backwards
                bool f = false;
                for (int i = cur_lvl - ProcessManager.CurWoWVersion.TalentConfig.StartLevel - 1;
                    i >= 0 ; i--)
                {
                    Level l1 = t.GetLevel(i);
                    TalentInfo t2 = GetTalentInfo(l1.TabId, l1.TalentId);
                    if (t2.Meets && (t2.Rank == l1.Rank - 1))
                    {
                        Output.Instance.Log("Found talent '" + t2.Name + "' Rank: " + t2.Rank +
                            " that wasn't learned for level " + l1.Num);
                        LearnTalent(l1, t2, delay);
                        f = true;
                    }
                }

                //TODO Check previous talent
                // for now stop
                if (!f)
                    // Profile un-synchronized with talents
                    throw new TalentLearnException("Some talents already learned " + 
                        "that not configured in template. Talent template might be mis-configured " + 
                        "or outdated", t1.Name,l.TalentId, l.Rank);
            }

            return (points - 1);
        }

        internal void LearnTalent(Level l, TalentInfo t, int delay)
        {
            int rank1 = t.Rank + 1;

            int retry = 0;
            int max_retry = ProcessManager.CurWoWVersion.TalentConfig.Retry;

            bool learned = false;

            do
            {
                TalentInfo t2;

                if (retry > 0)
                {
                    Output.Instance.Log("char",
                        "Retrying " + retry + " of " + max_retry);

                    Thread.Sleep(delay);

                    // Check if talent already learned
                    t2 = GetTalentInfo(l.TabId, l.TalentId);

                    Output.Instance.Debug("Learning result for talent: '" +
                                t.Name + "' is rank: " + t2.Rank);

                    if (t2.Rank == rank1)
                    {
                        learned = true;
                        Output.Instance.Log("char",
                            "Successfully learned '" + t.Name + "' rank " + rank1);
                        break;
                    }
                }

                Output.Instance.Log("char",
                    "Learning '" + t.Name + "' rank " + rank1 + " ...");
                ProcessManager.Injector.Lua_ExecByName("LearnTalent",
                                new object[] { l.TabId, l.TalentId });

                // WoW prevent learning talents fast
                Thread.Sleep(delay);

                t2 = GetTalentInfo(l.TabId, l.TalentId);

                Output.Instance.Debug("Learning result for talent: '" +
                                t.Name + "' is rank: " + t2.Rank);

                // Converting result
                if (t2.Rank == rank1)
                    {
                        learned = true;
                        Output.Instance.Log("char",
                            "Successfully learned '" + t.Name + "' rank " + rank1);
                    }
                    else
                        retry++;

            } while (!learned && (retry <= max_retry));

            if (!learned)
                // Something wrong with template
                throw new TalentLearnException ("Failed learned after " + 
                    retry + " tries with " + delay +
                    " msec delay between each try", t.Name, l.TalentId, l.Rank);
        }

        internal TalentInfo GetTalentInfo(int tab, int id)
        {
            // Checking rank increased
            string[] lret = ProcessManager.Injector.Lua_ExecByName("GetTalentInfo",
                        new object[] { tab, id });

            // Converting result
            int rank;

            try
            {
                rank = Convert.ToInt32(lret[4]);
            }
            catch (Exception ex)
            {
                throw new Exception("Unable retrieve result of 'GetTalentInfo' function.", ex);
            }

            return new TalentInfo(lret[0], rank, lret[7] != null && lret[7].Equals("1"));
        }

        #endregion

        private void contentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(this, "doc/index.html");
        }

        private void btnAddNPC_Click(object sender, EventArgs e)
        {
            if (!CheckInGame())
                return;

            // TODO Add npc

            // Open NPC List for configuration
            npcListToolStripMenuItem_Click(sender, e);
        }

        private bool CheckInGame()
        {
            // Check if InGame
            if (!ProcessManager.InGame)
            {
                MessageBox.Show("Not in game");
                return false;
            }

            return true;
        }

        private void npcListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (NPCListForm == null)
                NPCListForm = new NPCListForm();
            NPCListForm.TopMost = this.TopMost;
            NPCListForm.ShowDialog();
        }
    }
}
