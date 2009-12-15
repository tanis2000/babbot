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
using BabBot.Common;
using BabBot.Wow.Helpers;

namespace BabBot.Forms
{
    public partial class NPCListForm : BabBot.Forms.GenericDialog
    {
        private string[] CurServiceList;
        private string[] CurZoneList;

        public NPCListForm() : base ("npc_list")
        {
            InitializeComponent();
        }

        public void Open()
        {
            labelWoWVersion.Text = ProcessManager.CurWoWVersion.Build;

            // Bind property
            lbNpcList.DataSource = ProcessManager.CurWoWVersion.NPCData.Items;

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

            MessageBox.Show("TEST: " + string.Join(",", dlg.FileNames));
            foreach (string fname in dlg.FileNames)
            {
                // Load file
                // NPC npc;
                // ProcessManager.CurWoWVersion.NPCData.Add(npc);
            }
        }

        private void NPCListForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true; // this cancels the close event.
        }

        private void lbNpcList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbNpcList.SelectedItem == null)
                return;

            NPC npc = (NPC)lbNpcList.SelectedItem;
            tbName.Text = npc.Name;

            // Pull list of services
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
                    lbNpcList.SelectedItem = npc;
                    // Highlight new npc on the listbox
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
            if (MessageBox.Show(this, "Are you sure ?", "Confirmation",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) ==
                                                        DialogResult.Yes)
            {
                // TODO
                MessageBox.Show("Not implmeted yet");
                // Remove NPC and save data
            }
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
    }
}
