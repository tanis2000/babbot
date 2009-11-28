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
            
            cbAutoLogin.Checked = ProcessManager.Config.WoWInfo.AutoLogin;
			tbCharacter.Text = ProcessManager.Config.Character;
            tbRealmLocation.Text = ProcessManager.Config.Account.RealmLocation;
            cbxGameType.Text = ProcessManager.Config.Account.GameType;
            tbRealm.Text = ProcessManager.Config.Account.Realm;
            tbLoginUsername.Text = ProcessManager.Config.Account.LoginUsername;
            tbLoginPassword.Text = ProcessManager.Config.Account.getAutoLoginPassword();

            cbAutoLogin_CheckedChanged(sender, e);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            ProcessManager.Config.WoWInfo.AutoLogin = cbAutoLogin.Checked;
            ProcessManager.Config.Character = tbCharacter.Text;
            ProcessManager.Config.Account.RealmLocation = tbRealmLocation.Text;
            ProcessManager.Config.Account.GameType = cbxGameType.Text;
            ProcessManager.Config.Account.Realm = tbRealm.Text;
            ProcessManager.Config.Account.LoginUsername = tbLoginUsername.Text;
            ProcessManager.Config.Account.EncryptPassword(tbLoginPassword.Text);
        }

        private void lstAll_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cbAutoLogin_CheckedChanged(object sender, EventArgs e)
        {
            bool is_checked = cbAutoLogin.Checked;

            tbRealmLocation.Enabled = is_checked;
            cbxGameType.Enabled = is_checked;
            tbRealm.Enabled = is_checked;
            tbLoginUsername.Enabled = is_checked;
            tbLoginPassword.Enabled = is_checked;
			tbCharacter.Enabled = is_checked;

            labelRealmLocation.Enabled = is_checked;
            labelGameType.Enabled = is_checked;
            labelRealm.Enabled = is_checked;
            labelUser.Enabled = is_checked;
            labelPwd.Enabled = is_checked;
			labelCharacter.Enabled = is_checked;
			labelComment.Enabled = is_checked;

            if (is_checked)
                tbRealmLocation.Focus();
        }
    }
}