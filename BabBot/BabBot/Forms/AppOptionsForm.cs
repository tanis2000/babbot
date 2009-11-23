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
            if (tbLuaCallback.Text.Equals(""))
            {
                MessageBox.Show(this, "Lua Callback Address is empty");
            } else {
                try
                {
                    ProcessManager.Config.WowExePath = tbWowExePath.Text;
                    ProcessManager.Config.GuestUsername = tbGuestUsername.Text;
                    ProcessManager.Config.GuestPassword = tbGuestPassword.Text;
                    ProcessManager.Config.DebugMode = cbDebugMode.Checked;
                    ProcessManager.Config.LogPath = tbLogsPath.Text;
                    ProcessManager.Config.LogOutput = chkLogOutput.Checked;
                    ProcessManager.Config.NoSound = cbNoSound.Checked;
                    ProcessManager.Config.Windowed = cbWindowed.Checked;
                    ProcessManager.Config.Resize = cbResize.Checked;

                    ProcessManager.Config.LuaCallback = tbLuaCallback.Text;
                    ProcessManager.Config.WinTitle = tbWinTitle.Text;

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void AppOptionsForm_Load(object sender, EventArgs e)
        {
            Config config = ProcessManager.Config;

            tbWowExePath.Text = config.WowExePath;
            tbGuestUsername.Text = config.GuestUsername;
            tbGuestPassword.Text = config.GuestPassword;
            cbDebugMode.Checked = config.DebugMode;
            tbLogsPath.Text = config.LogPath;
            chkLogOutput.Checked = config.LogOutput;

            cbNoSound.Checked = config.NoSound;
            cbWindowed.Checked = config.Windowed;
            cbResize.Checked = config.Resize;

            tbLuaCallback.Text = config.LuaCallback;
            tbWinTitle.Text = config.WinTitle;
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
