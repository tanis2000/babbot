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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using BabBot.Manager;
using System.IO;
using BabBot.Wow;
using BabBot.Data;
using BabBot.Common;
using BabBot.Wow.Helpers;

namespace BabBot.Forms
{
    public partial class GameObjectsForm : BabBot.Forms.GenericDialog
    {
        private DataTable CurServiceList;

        protected override bool IsChanged
        {
            set
            {
                base.IsChanged = value;

                if (value)
                    btnClose.Text = "Cancel";
                else
                    btnClose.Text = "Close";
            }
        }

        public GameObjectsForm() : base ("npc_list")
        {
            InitializeComponent();

            bsGameObjects.DataSource = DataManager.GameData;
            bsServiceTypesFull.DataSource = DataManager.GameData;
            bsServiceTypesFiltered.DataSource = DataManager.GameData;
            bsZoneList.DataSource = DataManager.GameData;

#if DEBUG
            if (ProcessManager.Config.Test == 1)
            {
                tbX.Text = "1.1";
                tbY.Text = "2.2";
                tbZ.Text = "3.3";
            }
#endif
        }

        public void Open()
        {
            labelWoWVersion.Text = DataManager.CurWoWVersion.Build;

            IsChanged = false;
            Show();
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            // TODO load file from Data\Import directory
            var dlg = new OpenFileDialog { RestoreDirectory = true, Multiselect = true, 
                Filter = "Game Object data files (*.obj)|*.obj" };
            dlg.InitialDirectory = "Data" + Path.DirectorySeparatorChar + "Import";

            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            foreach (string fname in dlg.FileNames)
            {
                try
                {
                    GameObject g = DataManager.LoadXml(fname);
                    g.Changed = false;
                    DataManager.CurWoWVersion.GameObjData.Add(g);

                    // Save & Index data
                    DataManager.SaveGameObjData();

                    // Add npc to dataset
                    DataManager.AddGameObject(g);
                }
                catch
                {
                    ShowErrorMessage("Failed import data from " + fname +
                                            ". Check format and try again");
                }
            }
        }

        protected override void OnFormClosing(
                        object sender, FormClosingEventArgs e)
        {
            // For some reason called twice
            if (!e.Cancel)
                base.OnFormClosing(sender, e);

            if (!e.Cancel)
            {
                if (!btnClose.Text.Equals("Close"))
                {
                    DataManager.GameData.RejectChanges();
                    btnClose.Text = "Close";
                }

                e.Cancel = true; // this cancels the close event.
                Visible = false;
                // Hide();
            }
        }

        void SetFormControls(bool Enabled)
        {
            gbDescription.Enabled = Enabled;
        }

        private bool CheckBeforeNpcTest()
        {
            if (!CheckTarget())
                return false;

            // Start NPC channel and switch to main form
            Output.Instance.Log("npc", "Starting npc test ...");
            Program.mainForm.SelectLogTab("npc");

            return true;
        }

        internal void btnAddNPC_Click(object sender, EventArgs e)
        {
            if (!CheckBeforeNpcTest())
                return;

#if DEBUG
            //\\ TEST
            if (ProcessManager.Config.IsTest)
                if ((ProcessManager.Config.Test == 1) &&
                    !ProcessManager.Player.HasTarget)
                {
                    // Target NPC
                    // string name = "Melithar Staghelm";
                    // string name = "Conservator Ilthalaine";
                    // string name = "Gilshalan Windwalker";
                    string name = "Dellylah";
                    LuaHelper.TargetUnitByName(name);
                }
#endif

            try
            {
                NPC npc = NpcHelper.AddNpc("npc");
                if (npc != null)
                {
                    // Check for duplication
                    int idx = bsGameObjects.Find("NAME", npc.Name);
                    if (idx >= 0)
                        ((DataView)((DataRowView)bsGameObjects.Current).DataView).Delete(idx);
                    DataManager.AddGameObject(npc);
                    SelectGameObj(npc);
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("Can't add current NPC. " + ex.Message);
            }
        }

        private void SelectGameObj(GameObject obj)
        {
            lbGameObjList.SelectedIndex = bsGameObjects.Find("NAME", obj.Name);
        }

        private bool CheckGameObjSelected()
        {
            if (lbGameObjList.SelectedItem == null)
            {
                ShowErrorMessage("No NPC Selected");
                return false;
            }

            return true;
        }

        private void moveToObjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!CheckGameObjSelected())
                return;
            
            if (!CheckInGame())
                return;

            try
            {
                NpcHelper.MoveToGameObjByName(GetCurrentRow().NAME, "npc");
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }

        private void btnMoveToNearest_Click(object sender, EventArgs e)
        {
            if (!CheckInGame())
                return;

            if (cbServiceList.SelectedItem == null)
            {
                ShowErrorMessage("No services selected");
                return;
            }

            // Find nearest class trainer
            try
            {
                BotDataSet.ServiceTypesRow srv_row = (BotDataSet.ServiceTypesRow)
                                            ((DataRowView)cbServiceList.SelectedItem).Row;
                btnMoveToNearest.Enabled = false;
                NPC npc = NpcHelper.MoveInteractService(srv_row.NAME, "npc");

                // Select found npc
                if (npc != null)
                    SelectGameObj(npc);
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
            finally
            {
                btnMoveToNearest.Enabled = true;
            }
        }

        #region DEBUG
        
        private void LearnNpcSkills(NPC npc)
        {
            // Learn all skills
            string skill = cbServiceList.SelectedItem.ToString();
            NpcHelper.LearnSkills(npc, skill, "npc");
        }

        #endregion

        private void cbServiceList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbServiceList.SelectedItem == null)
                return;

            string skill = cbServiceList.SelectedItem.ToString();
            // btnLearnSkill.Enabled = (skill.Equals("class_trainer") || skill.Equals("wep_skill_trainer"));
            btnMoveToNearest.Enabled = cbServiceList.SelectedItem != null;
        }

        private void cbUseState_CheckedChanged(object sender, EventArgs e)
        {
            NpcHelper.UseState = cbUseState.Checked;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (IsChanged && (MessageBox.Show("Are you sure cancel changes ?",
                        "Confirmation", MessageBoxButtons.YesNo,
                        MessageBoxIcon.Exclamation) == DialogResult.No))
                return;
            else
            {
                BotDataSet.GameObjectsRow row = GetCurrentRow();
                // row.CancelEdit();
                DataManager.GameData.RejectChanges();
                tbName.Text = row.NAME;
            }

            if (btnClose.Text.Equals("Close"))
                Hide();
            else
            {
                tbName.TextChanged -= tbName_TextChanged;

                SetEditControls(false);
            }

            IsChanged = false;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {

            DataRowView srv_row = (DataRowView)bsServiceTypesFiltered.Current;

            if (srv_row == null)
                return;

            BotDataSet.ServiceTypesRow srow = (BotDataSet.ServiceTypesRow) srv_row.Row; 
            BotDataSet.GameObjectsRow mrow = GetCurrentRow();

            DataManager.GameData.NpcServices.AddNpcServicesRow(mrow, srow, srow.NAME);

            IsChanged = true;
        }

        private void lbActiveServices_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                DeleteSelectedNpcService();
                e.Handled = true;
            }
        }

        private void DeleteSelectedNpcService()
        {
            if (lbActiveServices.SelectedItem == null)
                return;

            ((DataRowView)fkGameObjectsNpcServices.Current).Delete();

            IsChanged = true;
        }

        private void deleteServiceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteSelectedNpcService();
        }

        private void btnAddNewObj_Click(object sender, EventArgs e)
        {
            // TODO
            ShowErrorMessage("Not implemented yet");
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            BotDataSet.GameObjectsRow row = GetCurrentRow();
            row.EndEdit();

            tbName.TextChanged -= tbName_TextChanged;

            try
            {
                // Save data on the disk
                DataManager.SaveGameObjRow(row);
                DataManager.GameData.AcceptChanges();

                SetEditControls(false);

                IsChanged = false;
            }
            catch (Exception ex)
            {
                // Keep edit state
                ShowErrorMessage(ex);
            }
        }

        #region GameObjects

        private void gameObjectsBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            BotDataSet.GameObjectsRow row = GetCurrentRow();
            if (row == null)
                return;

            if (btnSave.Enabled)
            {
                // we are in edit mode
                if (MessageBox.Show("Are you sure cancel changes ?",
                    "Confirmation", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Exclamation) == DialogResult.No) { }
                    // Select previous record
            }

            cbPlayerTarget.SelectedIndex = row.TYPE_ID;
        }

        private void lbGameObjectList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                DeleteSelectedObject();
                e.Handled = true;
            }
        }

        private void deleteGameObjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteSelectedObject();
        }

        private void DeleteSelectedObject()
        {
            if (!CheckGameObjSelected())
                return;

            // Possible multi selection
            DataRowView obj = (DataRowView)lbGameObjList.SelectedItem;
            string obj_name = obj.Row["NAME"].ToString();
            if (MessageBox.Show(this, "Are you sure to delete " + obj_name + "?",
                    "Confirmation", MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question) == DialogResult.Yes)
            {
                // Delete from all version or it will merge again
                obj.Delete();
                IsChanged = true;
            }
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (btnSave.Enabled)
            {
                ShowErrorMessage("Game Object in Edit Mode. Save data and try again");
                return;
            }

            BotDataSet.GameObjectsRow row = GetCurrentRow();
            GameObject obj = DataManager.CurWoWVersion.GameObjData.STable[row.NAME];
            if (obj == null)
            {
                ShowErrorMessage("Game Object not found. Restart Bot and try again");
                return;
            }

            DataManager.ExportGameObj(obj);
        }

        #endregion

        #region Services

        private void popServiceActions_Opening(object sender, CancelEventArgs e)
        {
            deleteServiceToolStripMenuItem.Enabled =
                    (lbActiveServices.SelectedItem != null);
        }

        #endregion

        #region Quests

        private void popQuestActions_Opening(object sender, CancelEventArgs e)
        {
            deleteQuestToolStripMenuItem.Enabled = 
                    (lbQuestList.SelectedItems.Count > 0);
        }

        private void deleteQuestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteSelectedQuest();
        }

        private void lbQuestList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                DeleteSelectedQuest();
                e.Handled = true;
            }
        }

        public void DeleteSelectedQuest()
        {
            if (lbQuestList.SelectedItem == null)
                return;

            ((DataRowView)fkGameObjectsQuestList.Current).Delete();

            IsChanged = true;
        }

        private void lbQuestList_DoubleClick(object sender, EventArgs e)
        {
            // TODO
            ShowErrorMessage("Not implemented yet");
        }

        private void btnAddQuest_Click(object sender, EventArgs e)
        {
            // TODO
            ShowErrorMessage("Not implemented yet");
        }

        private Quest CheckBeforeQuestTest()
        {
            if (!CheckInGame())
                return null;

            if (lbQuestList.SelectedItem == null)
            {
                ShowErrorMessage("No quest selected");
                return null;
            }

            BotDataSet.QuestListRow qrow = (BotDataSet.QuestListRow)
                                ((DataRowView)lbQuestList.SelectedItem).Row;
            int qid = qrow.ID;

            Quest q = DataManager.QuestList[qid];

            if (q == null)
            {
                ShowErrorMessage("Quest '" + qrow.TITLE + "' not found");
                return null;
            }

            // Start NPC channel and switch to main form
            Output.Instance.Log("quest_test", "Starting quest test ...");
            Program.mainForm.SelectLogTab("quest_test");

            return q;
        }

        private void acceptQuestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Quest q = CheckBeforeQuestTest();
            if (q == null)
                return;

            try
            {
                QuestHelper.AcceptQuest(q, "quest_test");
            }
            catch (QuestProcessingException qe)
            {
                ShowErrorMessage("Quest processing error: " + qe.Message);
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }

        private void deliverQuestToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        #endregion

        #region Coordinates

        private void deleteCoordinatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteSelectedCoordinates();
        }

        private void lbCoordinates_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                DeleteSelectedCoordinates();
                e.Handled = true;
            }
        }

        private void DeleteSelectedCoordinates()
        {
            if (lbCoordinates.SelectedItem == null)
                return;
            else if ((cbCoordZone.Items.Count == 1) && (lbCoordinates.Items.Count == 1))
            {
                ShowErrorMessage("The base object coordinates cannot be deleted");
                return;
            }

            ((DataRowView)fKCoordinatesZoneCoordinates.Current).Delete();
            
            // If no other coordinates delete zone too
            BotDataSet.CoordinatesDataTable coord_table = 
                (BotDataSet.CoordinatesDataTable) fKCoordinatesZoneCoordinates.DataSource;

            // TODO

            IsChanged = true;
        }

        private void popCoordinates_Opening(object sender, CancelEventArgs e)
        {
            deleteCoordinatesToolStripMenuItem.Enabled =
                            (lbCoordinates.SelectedItem != null);
        }

        private void btnAddPlayerTargetCoord_Click(object sender, EventArgs e)
        {
            if (cbPlayerTarget.SelectedIndex == 0)
                // Add player coord
                AddAsPlayerCoord();
            else if (cbPlayerTarget.SelectedIndex == 1)
                AddAsTargetCoord();

        }

        private void AddAsPlayerCoord()
        {
            if (!CheckInGame())
                return;

            AddGameObjCoord(ProcessManager.Player.ZoneText, 
                                            ProcessManager.Player.Location);
        }

        private void AddAsTargetCoord()
        {
            if (!CheckTarget())
                return;

            AddGameObjCoord(ProcessManager.Player.ZoneText, 
                                    ProcessManager.Player.CurTarget.Location);
        }

        private void btnAddCoord_Click(object sender, EventArgs e)
        {
            if (cbAllZones.SelectedItem == null)
            {
                ShowErrorMessage("Zone not selected");
                return;
            }

            try
            {
                AddGameObjCoord(cbAllZones.Text, Convert.ToDecimal(tbX.Text),
                    Convert.ToDecimal(tbY.Text), Convert.ToDecimal(tbZ.Text));

                tbX.Text = String.Empty;
                tbY.Text = String.Empty;
                tbZ.Text = String.Empty;
#if DEBUG
                if (ProcessManager.Config.Test == 1)
                {
                    tbX.Text = "1.1";
                    tbY.Text = "2.2";
                    tbZ.Text = "3.3";
                }
#endif
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }

        private void AddGameObjCoord(string zone, Vector3D v)
        {
            AddGameObjCoord(zone, (decimal)v.X, (decimal)v.Y, (decimal)v.Z);
        }

        private void AddGameObjCoord(string zone, decimal x, decimal y, decimal z)
        {
            // Check if new coord not too close
            foreach (DataRowView c in lbCoordinates.Items)
            {
                BotDataSet.CoordinatesRow row = (BotDataSet.CoordinatesRow)c.Row;
                if (Math.Sqrt(Math.Pow((double)(row.X - x), 2) + 
                        Math.Pow((double)(row.Y - y), 2) +
                        Math.Pow((double)(row.Z - z), 2)) < 5)
                {
                    ShowErrorMessage("New coordinates located in less than 5 yards with [" +
                        row.COORD + "]");
                    return;
                }
            }

            // Check if zone exists
            object zx = cbCoordZone.Items;
            DataView view = ((DataRowView)fKGameObjectsCoordinatesZone.Current).DataView;
            
            DataRowView[] rows = view.FindRows(zone);

            BotDataSet.CoordinatesZoneRow zone_row = null;
            if (rows.Length == 0)
                zone_row = DataManager.GameData.CoordinatesZone.
                    AddCoordinatesZoneRow(GetCurrentRow(), zone);
            else
                zone_row = (BotDataSet.CoordinatesZoneRow) rows[0].Row;

            DataManager.GameData.Coordinates.AddCoordinatesRow(zone_row, x, y, z);


            // Select last row
            lbCoordinates.SelectedIndex = lbCoordinates.Items.Count - 1;

            IsChanged = true;
        }

        #endregion

        private void btnEditObject_Click(object sender, EventArgs e)
        {
            if (!CheckGameObjSelected())
                return;

            SetEditControls(true);
            btnClose.Text = "Cancel";
            tbName.TextChanged += tbName_TextChanged;

            ((DataRowView)bsGameObjects.Current).Row.BeginEdit();
        }

        private void SetEditControls(bool enabled)
        {
#if DEBUG
            gbDebug.Enabled = !enabled;
#endif
            // Group Boxes
            gbQuestList.Enabled = !enabled;
            gbCoordinates.Enabled = !enabled;
            gbAddCoord.Enabled = !enabled;
            gbAutoAdd.Enabled = !enabled;

            // Combo Boxes
            cbItemList.Enabled = !enabled;
            cbAvailServices.Enabled = !enabled;

            // Buttons
            btnAddNewObj.Enabled = !enabled;
            btnImport.Enabled = !enabled;
            btnAddNPC.Enabled = !enabled;
            btnAddItem.Enabled = !enabled;
            btnAddService.Enabled = !enabled;
            btnEditObject.Enabled = !enabled;

            // ListBoxes
            lbGameObjList.Enabled = !enabled;
            lbActiveServices.Enabled = !enabled;

            // Enable name editing
            tbName.ReadOnly = !enabled;
        }

        private void tbName_TextChanged(object sender, EventArgs e)
        {
            RegisterChange(sender, e);
        }

        private BotDataSet.GameObjectsRow GetCurrentRow()
        {
            DataRowView rview = (DataRowView)bsGameObjects.Current;
            if (rview == null)
                return null;

            return  (BotDataSet.GameObjectsRow) rview.Row;

        }

        private void bsFKGameObjectsNpcServices_ListChanged(object sender, ListChangedEventArgs e)
        {
            // Set filter on available services
            BotDataSet.GameObjectsRow current = GetCurrentRow();
            DataRowView srv = (DataRowView)fkGameObjectsNpcServices.Current;
            bsServiceTypesFiltered.Filter = "";

            if (srv == null)
                bsServiceTypesFiltered.RemoveFilter();
            else
            {
                DataRow[] cur_srv = DataManager.
                    GameData.NpcServices.Select("GID=" + current.ID);

                if (cur_srv.Length > 0)
                {
                    string filter = "ID NOT IN (" + cur_srv[0]["SERVICE_ID"];
                    for (int i = 1; i < cur_srv.Length; i++)
                        filter += "," + cur_srv[i]["SERVICE_ID"];
                    bsServiceTypesFiltered.Filter = filter + ")";
                }
            }
        }

        private void btnAddItem_Click(object sender, EventArgs e)
        {
            ShowErrorMessage("Not implemented yet");
        }

        private void GameObjectsForm_Activated(object sender, EventArgs e)
        {
            SetControls(lbGameObjList.Items.Count > 0);
        }

        private void SetControls(bool enabled)
        {
            gbAddCoord.Enabled = enabled;
            gbCoordinates.Enabled = enabled;
            gbDescription.Enabled = enabled;
            gbQuestList.Enabled = enabled;
            gbAutoAdd.Enabled = enabled;
        }

        private void cbCoordZone_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbAllZones.Text = cbCoordZone.Text;
        }

        private void popGameObject_Opening(object sender, CancelEventArgs e)
        {
            moveToObjectToolStripMenuItem.Text = "Move To"; 

            if (!CheckGameObjSelected())
                return;

            moveToObjectToolStripMenuItem.Text += " '" + GetCurrentRow().NAME + "'"; 

        }
    }
}
