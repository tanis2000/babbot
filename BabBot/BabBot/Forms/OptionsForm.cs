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
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            ProcessManager.Config.WowExePath = tbWowExePath.Text;
            ProcessManager.Config.GuestUsername = tbGuestUsername.Text;
            ProcessManager.Config.GuestPassword = tbGuestPassword.Text;
            ProcessManager.Config.DebugMode = cbDebugMode.Checked;
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
    }
}