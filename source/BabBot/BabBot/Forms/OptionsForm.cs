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
            Config config = ProcessManager.Config;
            cbAutoLogin.Checked = config.WoWInfo.AutoLogin;
            tbCharacter.Text = config.Character;
            tbRealmLocation.Text = config.Account.RealmLocation;
            cbxGameType.Text = config.Account.GameType;
            tbRealm.Text = config.Account.Realm;
            tbLoginUsername.Text = config.Account.LoginUsername;
            tbLoginPassword.Text = config.Account.getAutoLoginPassword();
            cbRestart.Checked = config.Account.ReStart;
            cbRelogin.Checked = config.Account.ReConnect;

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
            ProcessManager.Config.Account.ReConnect = cbRelogin.Checked;
            ProcessManager.Config.Account.ReStart = cbRestart.Checked;
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

            cbRelogin.Enabled = is_checked;
            cbRelogin.Enabled = is_checked;

            if (is_checked)
                tbRealmLocation.Focus();
        }
    }
}