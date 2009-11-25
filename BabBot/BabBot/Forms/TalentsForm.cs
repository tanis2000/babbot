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
        // Template on URL for wow armory
        Regex trex = new Regex("^http://www.wowarmory.com/talent-calc.xml\\?cid=(\\d)\\&tal=(\\d+)$");
        // Currently selected talents list
        Talents CurTalents = null;
        // Default directory with talents template
        private string DefaultDir = "Profiles\\Talents";
        private BindingManagerBase bc;

        public TalentsForm()
        {
            InitializeComponent();
            btnApply.Tag = 1;
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
                dir = Directory.GetFiles(DefaultDir, "*.xml");
            }
            catch (System.IO.DirectoryNotFoundException)
            {
                MessageBox.Show("Directory '" + DefaultDir + "' not found");
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
            lbLevelList.DataSource = CurTalents.LevelList;

            lbLevelList.SelectedIndex = 0;
            CheckSaveBtn();
        }

        private void CheckSaveBtn()
        {
            btnSave.Enabled = ((lbLevelList.Items.Count > 0) &&
                                        !tbDescription.Text.Equals("") &&
                                        !cbClass.Text.Equals("") &&
                                        // Talent template not empty and has .xml extension
                                        !cbTalentTemplates.Text.Equals("") &&
                                        (cbTalentTemplates.Text.IndexOf(".xml") == 
                                                    cbTalentTemplates.Text.Length - 4));
        }

        private void lbLevelList_SelectedIndexChanged(object sender, EventArgs e)
        {
            Level l = (Level) lbLevelList.SelectedItem;

            if (l == null)
                SetBtnType(1);
            else
            {
                try
                {
                    labelLevelNum.Text = Convert.ToString(l.Num);
                    numTab.Value = l.TabId;
                    numTalent.Value = l.TalentId;

                    SetBtnType(0);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);

                    // Unselect everything
                    lbLevelList.SelectedIndex = -1;
                    btnApply.Enabled = false;
                    btnRemove.Enabled = false;
                }
            }
        }

        private void SetBtnType(int type)
        {
            btnApply.Tag = type;
            switch (type)
            {
                case 0: // Update
                    btnApply.Text = "Update";
                    btnApply.Enabled = true;
                    btnRemove.Enabled = (lbLevelList.SelectedIndex == 
                                                            (lbLevelList.Items.Count - 1));

                    break;

                case 1:
                    btnApply.Text = "Add";
                    btnApply.Enabled = true;
                    btnRemove.Enabled = false;

                    break;
            }
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            Level l;

            switch ((int)btnApply.Tag)
            {
                case 0:
                    l = (Level) lbLevelList.SelectedItem;
                    l.Update((byte)numTab.Value, 
                                (int) numTalent.Value, (byte) numRank.Value);

                    CurrencyManager cm = (CurrencyManager)BindingContext[CurTalents.LevelList];
                    cm.Refresh();
                    break;
                case 1:
                    l = new Level(Convert.ToByte(labelLevelNum.Text), (byte)numTab.Value,
                                    (int)numTalent.Value, (byte)numRank.Value);
                    CurTalents.AddLevel(l);
                    lbLevelList.SelectedIndex = lbLevelList.Items.Count - 1;

                    CheckSaveBtn();

                    break;
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            // Only can remove last item
            int idx = lbLevelList.Items.Count - 1;
            lbLevelList.Items.RemoveAt(idx);
            CheckSaveBtn();
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

                MainForm.ShowTopMostMsg(this, "File " + CurTalents.FullPath +
                    " successfully saved", "SUCCESS");
            }
            catch (Exception ex)
            {
                MainForm.ShowTopMostMsg(this, ex.Message, "ERROR");

            }
        }
    }
}
