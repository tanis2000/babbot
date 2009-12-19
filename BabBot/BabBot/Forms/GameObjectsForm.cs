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
                Filter = "NPC data files (*.npc)|*.npc" };
            dlg.InitialDirectory = "Data" + Path.DirectorySeparatorChar + "Import";

            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            bool f = false;
            foreach (string fname in dlg.FileNames)
            {
                try
                {
                    NPC npc = DataManager.LoadXml(fname);
                    npc.Changed = false;
                    DataManager.CurWoWVersion.GameObjData.Add(npc);

                    // Save & Index data
                    DataManager.SaveNpcData();
                }
                catch
                {
                    ShowErrorMessage("Failed import data from " + fname +
                                            ". Check format and try again");
                }
            }
        }

        private void NPCListForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true; // this cancels the close event.
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

            // Check if npc selected
            if (!ProcessManager.Player.HasTarget)
            {
                ShowErrorMessage("NPC is not selected");
                return;
            }

            try
            {
                NPC npc = NpcHelper.AddNpc("npc");
                if (npc != null)
                {
                    // Unselect all
                    foreach (int idx in lbGameObjList.SelectedIndices)
                        lbGameObjList.SetSelected(idx, false);
                    // Highlight new npc on the listbox
                    lbGameObjList.SelectedItem = npc;
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("Can't add current NPC. " + ex.Message);
            }
        }

        private void btnMoveToNearest_Click(object sender, EventArgs e)
        {
            if (!CheckBeforeNpcTest())
                return;

            // Find nearest class trainer
            try
            {
                btnMoveToNearest.Enabled = false;
                NPC npc = NpcHelper.MoveInteractService(cbServiceList.SelectedItem.ToString(), "npc");
                // TODO Flash found npc
                // labelNPC.Tag = npc;
                // labelNPC.Text = "NPC: " + npc.Name;
            }
            catch (Exception ex)
            {
                ShowErrorMessage("Service Move Error: " + ex.Message);
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

            ((DataRowView)bsFKGameObjectsNpcServices.Current).Delete();

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
                // TODO

                DataManager.SaveGameObjRow(row);
                DataManager.GameData.AcceptChanges();

                SetEditControls(false);

                IsChanged = false;
            }
            catch
            {
                // Keep edit state
            }
        }

        #region GameObjects

        private void gameObjectsBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            
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
            // Possible multi selection
            foreach (object obj in lbGameObjList.SelectedItems)
            {
                string npc_name = ((NPC)obj).Name;
                if (MessageBox.Show(this, "Are you sure to delete " + npc_name + "?",
                    "Confirmation", MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question) == DialogResult.Yes)
                    // Delete from all version or it will merge again
                    foreach (GameDataVersion v in DataManager.GameObjList.Table.Values)
                        v.STable.Remove(npc_name);
            }

            // Now we need save & reindex data
            DataManager.SaveNpcData();
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

        private void btnAddQuest_Click(object sender, EventArgs e)
        {
            // TODO
            ShowErrorMessage("Not implemented yet");
        }

        private void deleteQuestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteSelectedObject();
        }

        public void DeleteSelectedQuest()
        {
            if (lbQuestList.SelectedItem == null)
                return;

            ((DataRowView)bsFKGameObjectsQuestList.Current).Delete();

            IsChanged = true;
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
            else if (lbCoordinates.Items.Count == 1)
            {
                ShowErrorMessage("The base object coordinates cannot be deleted");
                return;
            }

            ((DataRowView)bsFKGameObjectsCoordinates.Current).Delete();

            IsChanged = true;
        }

        private void popCoordinates_Opening(object sender, CancelEventArgs e)
        {
            deleteCoordinatesToolStripMenuItem.Enabled =
                            (lbCoordinates.SelectedItem != null);
        }

        private void btnAddAsPlayerCoord_Click(object sender, EventArgs e)
        {
            if (!CheckInGame())
                return;

            AddGameObjCoord(ProcessManager.Player.Location);
        }

        private void btnAddAsTargetCoord_Click(object sender, EventArgs e)
        {
            if (!CheckTarget())
                return;

            AddGameObjCoord(ProcessManager.Player.CurTarget.Location);
        }

        private void btnAddCoord_Click(object sender, EventArgs e)
        {
            try
            {
                AddGameObjCoord(Convert.ToDecimal(tbX.Text),
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
                ShowErrorMessage(ex.Message);
            }
        }

        private void AddGameObjCoord(Vector3D v)
        {
            AddGameObjCoord((decimal)v.X, (decimal)v.Y, (decimal)v.Z);
        }

        private void AddGameObjCoord(decimal x, decimal y, decimal z)
        {
            DataManager.GameData.Coordinates.AddCoordinatesRow(
                GetCurrentRow(), x, y, z, null);

            // Select last row
            lbCoordinates.SelectedIndex = lbCoordinates.Items.Count - 1;

            IsChanged = true;
        }

        #endregion

        private void btnEditObject_Click(object sender, EventArgs e)
        {
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

            // Combo Boxes
            cbItemList.Enabled = !enabled;
            cbAvailServices.Enabled = !enabled;

            // Buttons
            btnAddNewObj.Enabled = !enabled;
            btnImport.Enabled = !enabled;
            btnAddNPC.Enabled = !enabled;
            btnAddItem.Enabled = !enabled;
            btnAddService.Enabled = !enabled;
            btnAddAsPlayerCoord.Enabled = !enabled;
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
            DataRowView srv = (DataRowView)bsFKGameObjectsNpcServices.Current;
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
    }
}
