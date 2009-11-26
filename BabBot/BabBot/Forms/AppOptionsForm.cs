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
        public string Msg;
        
        public AppOptionsForm()
        {
            InitializeComponent();
            cbWoWVersion.DataSource = ProcessManager.WoWVersions;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (tbLuaCallback.Text.Equals(""))
            {
                MessageBox.Show(this, "Lua Callback Address is empty");
            } else if (cbWoWVersion.SelectedIndex < 0) {
                MessageBox.Show(this, "WoW Version field is empty");
            } else {
                try
                {
                    ProcessManager.Config.WoWInfo.ExePath = tbWowExePath.Text;
                    ProcessManager.Config.WoWInfo.GuestUsername = tbGuestUsername.Text;
                    ProcessManager.Config.WoWInfo.GuestPassword = tbGuestPassword.Text;
                    ProcessManager.Config.WoWInfo.NoSound = cbNoSound.Checked;
                    ProcessManager.Config.WoWInfo.Windowed = cbWindowed.Checked;
                    ProcessManager.Config.WoWInfo.Resize = cbResize.Checked;
                    ProcessManager.Config.WoWInfo.Version = cbWoWVersion.Text;

                    ProcessManager.Config.LogParams.Dir = tbLogsPath.Text;
                    ProcessManager.Config.LogParams.DisplayLogs = chkLogOutput.Checked;
                    
                    ProcessManager.Config.DebugMode = cbDebugMode.Checked;
                    ProcessManager.Config.ProfilesDir = tbProfilesPath.Text;

                    ProcessManager.Config.CustomParams.LuaCallback = tbLuaCallback.Text;
                    ProcessManager.Config.CustomParams.WinTitle = tbWinTitle.Text;

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

            tbWowExePath.Text = config.WoWInfo.ExePath;
            tbGuestUsername.Text = config.WoWInfo.GuestUsername;
            tbGuestPassword.Text = config.WoWInfo.GuestPassword;
            cbNoSound.Checked = config.WoWInfo.NoSound;
            cbWindowed.Checked = config.WoWInfo.Windowed;
            cbResize.Checked = config.WoWInfo.Resize;
            cbWoWVersion.Text = config.WoWInfo.Version;

            if (cbWoWVersion.Text.Equals("") && (cbWoWVersion.Items.Count == 1))
                // Auto Select first
                cbWoWVersion.SelectedIndex = 0;

            cbDebugMode.Checked = config.DebugMode;
            tbProfilesPath.Text = config.ProfilesDir;

            tbLogsPath.Text = config.LogParams.Dir;
            chkLogOutput.Checked = config.LogParams.DisplayLogs;


            tbLuaCallback.Text = config.CustomParams.LuaCallback;
            tbWinTitle.Text = config.CustomParams.WinTitle;
            
            if (Msg != null)
                MessageBox.Show(this,Msg);
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
                tbLogsPath.Text = dlg.SelectedPath;
        }

        private void btnProfilesPath_Click(object sender, EventArgs e)
        {
            var dlg = new FolderBrowserDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
                tbProfilesPath.Text = dlg.SelectedPath;
        }
    }
}
