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
            tbRealmLocation.Enabled = cbAutoLogin.Checked;
            cbxGameType.Enabled = cbAutoLogin.Checked;
            tbRealm.Enabled = cbAutoLogin.Checked;
            tbLoginUsername.Enabled = cbAutoLogin.Checked;
            tbLoginPassword.Enabled = cbAutoLogin.Checked;
			tbCharacter.Enabled = cbAutoLogin.Checked;

            labelRealmLocation.Enabled = cbAutoLogin.Checked;
            labelGameType.Enabled = cbAutoLogin.Checked;
            labelRealm.Enabled = cbAutoLogin.Checked;
            labelUser.Enabled = cbAutoLogin.Checked;
            labelPwd.Enabled = cbAutoLogin.Checked;
			labelCharacter.Enabled = cbAutoLogin.Checked;
			labelComment.Enabled = cbAutoLogin.Checked;
        }
    }
}