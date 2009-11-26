using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Net;
using System.IO;
using BabBot.Wow;
using BabBot.Manager;
using System.Xml.Serialization;

namespace BabBot.Forms
{
    public partial class TalentsForm : Form
    {
        // Template on WoW Armory URL 
        Regex trex;
        // Currently selected talents list
        Talents CurTalents = null;
        // Change tracking
        private bool _changed = false;

        public TalentsForm()
        {
            InitializeComponent();

            trex = new Regex(ProcessManager.CurVersion.TalentConfig.ArmoryPattern);
        }

        private int LevelLabel
        {
            get
            {
                return (labelLevelNum.Text.Equals("")) ? 0 : 
                            Convert.ToInt32(labelLevelNum.Text);
            }
        }

        private string ReadURL(string url)
        {
            // Create a request for the URL.         
            WebRequest request = WebRequest.Create(url);
            // If required by the server, set the credentials.
            request.Credentials = CredentialCache.DefaultCredentials;
            // Get the response.
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            // Display the status.
            // response.StatusDescription;
            // Get the stream containing content returned by the server.
            Stream stream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(stream);
            // Read the content.
            string res = reader.ReadToEnd();
            // Cleanup the streams and the response.
            reader.Close();
            stream.Close();
            response.Close();

            return res;
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            try
            {
                string url = tbTalentURL.Text;
                // Check for class id
                Match m = trex.Match(url);

                if (m.Success && (m.Groups.Count == 3))
                {
                    string s = m.Groups[1].ToString();
                    int cid = Convert.ToInt32(s);
                    string response = ReadURL(url);
                } else {
                    MessageBox.Show(string.Format(
                        "Invalid URL. '{0}' excpected", trex.ToString()));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable read URL: " + ex.Message);
            }
        }

        private void tbTalentURL_TextChanged(object sender, EventArgs e)
        {
            btnImport.Enabled = !tbTalentURL.Text.Equals("");
        }

        private void TalentsForm_Load(object sender, EventArgs e)
        {
            tbTalentURL_TextChanged(sender, e);
        }

        private void cbTalentTemplates_DropDown(object sender, EventArgs e)
        {
            cbTalentTemplates.Items.Clear();
            string[] dir;

            // Scan Profiles/Talents for list
            try
            {
                dir = Directory.GetFiles(ProcessManager.Config.TalentTemplateDir, "*.xml");
            }
            catch (System.IO.DirectoryNotFoundException)
            {
                MessageBox.Show("Directory '" + 
                    ProcessManager.Config.TalentTemplateDir + "' not found");
                return;
            }
            
            // Check each file
            foreach (string fname in dir)
            {
                try
                {
                    Talents tlist = ProcessManager.ReadTalents(fname);
                    if ((tlist != null) && (tlist.Description != null))
                        cbTalentTemplates.Items.Add(tlist);
                } catch { 
                    // Continue 
                }
            }
        }

        private void cbTalentTemplates_SelectedIndexChanged(object sender, EventArgs e)
        {
            CurTalents = (Talents)cbTalentTemplates.SelectedItem;

            cbClass.Text = CurTalents.Class;
            cbClass.Enabled = false;

            tbDescription.Text = CurTalents.Description;

            lbLevelList.Items.Clear();

            // Clear binding
            if (lbLevelList.DataSource != null)
                lbLevelList.DataSource = null;

            BindLevels();

            lbLevelList.SelectedIndex = 0;
            CheckSaveBtn();
        }

        private void CheckSaveBtn()
        {
            bool is_header_set = (!tbDescription.Text.Equals("") &&
                                        !cbClass.Text.Equals("") &&
                                        // Talent template not empty and has .xml extension
                                        !cbTalentTemplates.Text.Equals("") &&
                                        (cbTalentTemplates.Text.IndexOf(".xml") == 
                                                    cbTalentTemplates.Text.Length - 4));

            btnSave.Enabled = ((lbLevelList.Items.Count > 0) && is_header_set && _changed);

            btnAdd.Enabled = ((LevelLabel < ProcessManager.CurVersion.MaxLvl) && is_header_set);

            btnRemove.Enabled = is_header_set && (lbLevelList.SelectedIndex ==
                                        (lbLevelList.Items.Count - 1));

            btnUp.Enabled = lbLevelList.Items.Count > 1;
            btnDown.Enabled = btnUp.Enabled;
        }

        private void lbLevelList_SelectedIndexChanged(object sender, EventArgs e)
        {
            Level l = (Level) lbLevelList.SelectedItem;

            if (l == null)
                btnUpdate.Enabled = false;
            else
            {
                try
                {
                    labelLevelNum.Text = Convert.ToString(l.Num);
                    numTab.Value = l.TabId;
                    numTalent.Value = l.TalentId;

                    btnUpdate.Enabled = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);

                    // Unselect everything
                    lbLevelList.SelectedIndex = -1;
                    btnUpdate.Enabled = false;
                    btnRemove.Enabled = false;
                }
            }
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            if (lbLevelList.SelectedValue == null) return;

            ((Level)lbLevelList.SelectedValue).Update((byte)numTab.Value, 
                (int) numTalent.Value, (byte) numRank.Value);

            RefreshLevelList();
            RegisterChange();
        }

        private void RegisterChange()
        {
            _changed = true;
            CheckSaveBtn();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            // Only can remove last item
            int idx = lbLevelList.Items.Count - 1;
            lbLevelList.Items.RemoveAt(idx);

            RegisterChange();
        }

        private void tbDescription_TextChanged(object sender, EventArgs e)
        {
            CheckSaveBtn();
        }

        private void cbClass_TextChanged(object sender, EventArgs e)
        {
            CheckSaveBtn();
        }

        private void cbTalentTemplates_TextChanged(object sender, EventArgs e)
        {
            CheckSaveBtn();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                XmlSerializer s = new XmlSerializer(typeof(Talents));
                TextWriter w = new StreamWriter(CurTalents.FullPath);
                s.Serialize(w, CurTalents);
                w.Close();

                _changed = false;
                MessageBox.Show(this, "File " + CurTalents.FullPath +
                    " successfully saved", "SUCCESS", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "ERROR", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Level l = new Level((byte) (ProcessManager.CurVersion.TalentConfig.StartLevel + 
                         lbLevelList.Items.Count),(byte) numTab.Value, 
                        (int)numTalent.Value, (byte)numRank.Value);

            bool is_new = (CurTalents == null);
            if (is_new)
            {
                CurTalents = new Talents(cbTalentTemplates.Text,
                                        tbTalentURL.Text, tbDescription.Text);
                BindLevels();
            }

            CurTalents.AddLevel(l);

            RefreshLevelList();
            lbLevelList.SelectedIndex = lbLevelList.Items.Count - 1;

            RegisterChange();
        }

        private void RefreshLevelList()
        {
            CurrencyManager cm = (CurrencyManager)BindingContext[CurTalents.Levels];
            cm.Refresh();
        }

        private void BindLevels()
        {
            lbLevelList.DataSource = CurTalents.Levels;
        }

        private void lbLevelList_SelectedValueChanged(object sender, EventArgs e)
        {
            if (lbLevelList.SelectedIndex != -1)
                lbLevelList.Text = lbLevelList.SelectedValue.ToString();
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            RegisterChange();
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            RegisterChange();
        }

        private void TalentsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = (_changed &&
                (MessageBox.Show(this, "Are you sure you want close and cancel changes ?",
                    "Confirmation", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2) != DialogResult.Yes));
        }
    }
}
