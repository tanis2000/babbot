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
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Diagnostics;
using BabBot.Manager;

namespace BabBot.Forms
{
    public partial class GenericDialog : Form
    {
        // Internal name
        private string _name;
        // Change tracking
        private bool _changed = false;

        protected virtual bool IsChanged
        {
            get { return _changed; }
            set { 
                _changed = value;
                btnSave.Enabled = _changed;
            }
        }

        GenericDialog()
        {
            InitializeComponent();
        }

        #region Public Methods

        public GenericDialog(string name)
            : this()
        {
            _name = name;

        }

        #endregion

        #region Protected Methods

        protected bool CheckInGame()
        {
#if DEBUG
            if (ProcessManager.Config.Test == 3)
                return true;
#endif
            // Check if InGame
            if (!ProcessManager.InGame)
            {
                MessageBox.Show("Not in game");
                return false;
            }

            return true;
        }

        protected bool CheckTarget()
        {
            if (!CheckInGame())
                return false;

            if (ProcessManager.Player.CurTarget == null)
            {
                MessageBox.Show("Toon has no target");
                return false;
            }

            return true;
        }

        protected void ShowLfsTab(string lfs)
        {
            Program.mainForm.SelectLogTab(lfs);
        }

        protected void ShowErrorMessage(string err)
        {
            MessageBox.Show(this, err, "ERROR",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        protected void ShowErrorMessage(Exception e)
        {
            MessageBox.Show(this, "Caught exception " + e.GetType() + ": " + e.Message, 
                "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        protected void ShowInfoMessage(string info)
        {
            MessageBox.Show(this, info, "INFO",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        protected virtual void GenericDialog_Load(object sender, EventArgs e)
        {
            _changed = false;
        }

        protected void OpenURL(string url)
        {
            Process.Start(GetDefaultBrowserPath(), url);
        }

        protected virtual void RegisterChange(object sender, EventArgs e)
        {
            IsChanged = true;
        }

        #endregion

        #region Private Methods

        private static string GetDefaultBrowserPath()
        {
            // Registry info for default browser
            string key = @"http\shell\open\command";
            RegistryKey registryKey = Registry.ClassesRoot.OpenSubKey(key, false);
            // get default browser path
            return ((string)registryKey.GetValue(null, null)).Split('"')[1];
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            // Open local URL in default browswer
            string url = "file:///" + Environment.CurrentDirectory.Replace("\\", "/") +
                "/Doc/index.html" + "#" + _name;

            OpenURL(url);
        }

        protected virtual void OnFormClosing(
                            object sender, FormClosingEventArgs e)
        {
            e.Cancel = (IsChanged &&
                (MessageBox.Show(this, "Are you sure you want close and cancel changes ?",
                    "Confirmation", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2) != DialogResult.Yes));
        }

        #endregion
    }
}
