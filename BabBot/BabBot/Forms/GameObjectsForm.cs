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

        public GameObjectsForm() : base ("npc_list")
        {
            InitializeComponent();
        }

        public void Open()
        {
            labelWoWVersion.Text = DataManager.CurWoWVersion.Build;

            // Bind NpcList
            BindNpcList();

            IsChanged = false;
            Show();
        }

        private void BindNpcList()
        {
            lbGameObjList.DataSource = null;
            lbGameObjList.DataSource = DataManager.CurWoWVersion.GameObjData.Items;
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

        private void lbNpcList_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool f = lbGameObjList.SelectedItem != null;
            SetFormControls(f);
            if (!f)
                return;

            NPC npc = (NPC)lbGameObjList.SelectedItem;
            tbName.Text = npc.Name;

            SetNpcAvailServiceList(npc);
        }

        void SetFormControls(bool Enabled)
        {
            gbNpcDescription.Enabled = Enabled;
        }

        private bool CheckBeforeNpcTest()
        {
            if (!CheckInGame())
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
                    string name = "Gilshalan Windwalker";
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
                    // Refresh NPC list
                    BindNpcList();

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

        private void deleteNPCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteSelectedNpc();
        }

        private void DeleteSelectedNpc()
        {
            // Possible multi selection
            foreach (object obj in lbGameObjList.SelectedItems)
            {
                string npc_name = ((NPC) obj).Name;
                if (MessageBox.Show(this, "Are you sure to delete " + npc_name + "?",
                    "Confirmation", MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question) == DialogResult.Yes)
                    // Delete from all version or it will merge again
                    foreach (GameDataVersion v in DataManager.GameObjList.Table.Values)
                        v.STable.Remove(npc_name);
            }

            // Now we need save & reindex data
            DataManager.SaveNpcData();

            BindNpcList();
        }

        private void SetNpcAvailServiceList(NPC npc)
        {
            lbActiveServices.SelectedItem = null;
            serviceTypesNpcAvail.DataSource = null;

            // Pull list of services
            // Filter available services and add custom list
            CurServiceList = DataManager.ServiceTypeTable.Clone();
            foreach (DataRow row in DataManager.ServiceTypeTable.Rows)
                if (!npc.Services.Table.ContainsKey(row["NAME"].ToString()))
                    CurServiceList.ImportRow(row);
            serviceTypesNpcAvail.DataSource = CurServiceList;

            lbActiveServices.DataSource = npc.Services.Items;
        }

        private void lbNpcList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                DeleteSelectedNpc();
                e.Handled = true;
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
            Hide();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            NPC npc = (NPC)lbGameObjList.SelectedItem;
            string srv = cbAvailServices.Text;
            if (string.IsNullOrEmpty(srv))
                return;

            npc.Services.Add(new NPCService(srv));
            SetNpcAvailServiceList(npc);

            IsChanged = true;
        }

        private void NPCListForm_Load(object sender, EventArgs e)
        {
            serviceTypesAllAvail.DataSource = DataManager.ServiceTypeTable;
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

            NPC npc = (NPC)lbGameObjList.SelectedItem;
            npc.Services.Remove(lbActiveServices.Text);

            SetNpcAvailServiceList(npc);

            IsChanged = true;
        }

        private void deleteServiceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteSelectedNpcService();
        }

        private void popServiceActions_VisibleChanged(object sender, EventArgs e)
        {
            deleteServiceToolStripMenuItem.Enabled = (lbActiveServices.SelectedItem != null);
        }

        private void popNpc_VisibleChanged(object sender, EventArgs e)
        {
            deleteNPCToolStripMenuItem.Enabled = (lbGameObjList.SelectedItem != null);
        }

        private void btnAddQuest_Click(object sender, EventArgs e)
        {

        }

        private void btnAddNewObj_Click(object sender, EventArgs e)
        {
            lbGameObjList.SelectedItem = null;
            lbGameObjList.Enabled = false;
            btnAddNewObj.Enabled = false;
            gameObjectsBindingSource.AddNew();

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // gameObjectsBindingSource.Add((;
            btnAddNewObj.Enabled = true;
        }
    }
}
