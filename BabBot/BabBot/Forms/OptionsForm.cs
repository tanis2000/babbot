using System;
using System.Windows.Forms;
using BabBot.Common;
using BabBot.Manager;

namespace BabBot.Forms
{
    public partial class OptionsForm : Form
    {
        public OptionsForm()
        {
            InitializeComponent();
        }

        private void OptionsForm_Load(object sender, EventArgs e)
        {
            tbWowExePath.Text = ProcessManager.Config.WowExePath;
            tbGuestUsername.Text = ProcessManager.Config.GuestUsername;
            tbGuestPassword.Text = ProcessManager.Config.GuestPassword;
            cbDebugMode.Checked = ProcessManager.Config.DebugMode;
            tbLogsPath.Text = ProcessManager.Config.LogPath;
            cbAutoLogin.Checked = ProcessManager.Config.AutoLogin;
            tbLoginUsername.Text = ProcessManager.Config.LoginUsername;
            tbLoginPassword.Text = ProcessManager.Config.getAutoLoginPassword();
            cbNoSound.Checked = ProcessManager.Config.NoSound;
            cbWindowed.Checked = ProcessManager.Config.Windowed;
            cbResize.Checked = ProcessManager.Config.Resize;

            cbAutoLogin_CheckedChanged(sender, e);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            ProcessManager.Config.WowExePath = tbWowExePath.Text;
            ProcessManager.Config.GuestUsername = tbGuestUsername.Text;
            ProcessManager.Config.GuestPassword = tbGuestPassword.Text;
            ProcessManager.Config.DebugMode = cbDebugMode.Checked;
            ProcessManager.Config.LogPath = tbLogsPath.Text;
            ProcessManager.Config.AutoLogin = cbAutoLogin.Checked;
            ProcessManager.Config.LoginUsername = tbLoginUsername.Text;
            ProcessManager.Config.EncryptPassword(tbLoginPassword.Text);
            ProcessManager.Config.NoSound = cbNoSound.Checked;
            ProcessManager.Config.Windowed = cbWindowed.Checked;
            ProcessManager.Config.Resize = cbResize.Checked;
        }

        private void btnBrowseWowExec_Click(object sender, EventArgs e)
        {
            var dlg = new OpenFileDialog {Multiselect = false, Filter = "WoW Executable (*.exe)|*.exe"};
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

        private void lstAll_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnBrowseLogPath_Click(object sender, EventArgs e)
        {
            var dlg = new FolderBrowserDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                tbLogsPath.Text = dlg.SelectedPath;
            }
        }

        private void cbAutoLogin_CheckedChanged(object sender, EventArgs e)
        {
            tbLoginUsername.Enabled = cbAutoLogin.Checked;
            tbLoginPassword.Enabled = cbAutoLogin.Checked;
            labelUser.Enabled = cbAutoLogin.Checked;
            labelPwd.Enabled = cbAutoLogin.Checked;
        }
    }
}