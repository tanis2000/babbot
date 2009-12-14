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

namespace BabBot.Forms
{
    public partial class NPCListForm : BabBot.Forms.GenericDialog
    {
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
    }
}
