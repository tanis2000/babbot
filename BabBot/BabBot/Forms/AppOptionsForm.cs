using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BabBot.Manager;
using BabBot.Common;

namespace BabBot.Forms
{
    public partial class AppOptionsForm : Form
    {
        public AppOptionsForm()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                ProcessManager.Config.WowExePath = tbWowExePath.Text;
                ProcessManager.Config.GuestUsername = tbGuestUsername.Text;
                ProcessManager.Config.GuestPassword = tbGuestPassword.Text;
                ProcessManager.Config.DebugMode = cbDebugMode.Checked;
                ProcessManager.Config.LogPath = tbLogsPath.Text;

                ProcessManager.Config.NoSound = cbNoSound.Checked;
                ProcessManager.Config.Windowed = cbWindowed.Checked;
                ProcessManager.Config.Resize = cbResize.Checked;

                ProcessManager.Config.LuaCallback = tbLuaCallback.Text;
                ProcessManager.Config.WinTitle = tbWinTitle.Text;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void AppOptionsForm_Load(object sender, EventArgs e)
        {
            tbWowExePath.Text = ProcessManager.Config.WowExePath;
            tbGuestUsername.Text = ProcessManager.Config.GuestUsername;
            tbGuestPassword.Text = ProcessManager.Config.GuestPassword;
            cbDebugMode.Checked = ProcessManager.Config.DebugMode;
            tbLogsPath.Text = ProcessManager.Config.LogPath;

            cbNoSound.Checked = ProcessManager.Config.NoSound;
            cbWindowed.Checked = ProcessManager.Config.Windowed;
            cbResize.Checked = ProcessManager.Config.Resize;

            tbLuaCallback.Text = ProcessManager.Config.LuaCallback;
            tbWinTitle.Text = ProcessManager.Config.WinTitle;
        }

        private void btnBrowseWowExec_Click(object sender, EventArgs e)
        {
            var dlg = new OpenFileDialog { Multiselect = false, Filter = "WoW Executable (*.exe)|*.exe" };
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                tbWowExePath.Text = dlg.FileName;
            }
        }

        private void btnFindWowExePath_Click(object sender, EventArgs e)
        {
            string wowPath = AppHelper.GetWowInstallationPath();
            if (!string.IsNullOrEmpty(wowPath))
            {
                tbWowExePath.Text = wowPath;
            }
            else
            {
                MessageBox.Show("Cannot find WoW's installation path.");
            }
        }

        private void btnBrowseLogPath_Click(object sender, EventArgs e)
        {
            var dlg = new FolderBrowserDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                tbLogsPath.Text = dlg.SelectedPath;
            }
        }
    }
}
