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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.IO;
using BabBot.Wow;
using BabBot.Manager;
using System.Xml.Serialization;

// TODO 
// 1 - Add Ctrl-S to save talent
// 2 - Extract Talent Name from armory URL and use it instead of Talent ID
// 3 - Add Ctrl-Up/Ctrl-Down to move talents

namespace BabBot.Forms
{
    public partial class TalentsForm : GenericDialog
    {
        // Template on WoW Armory URL 
        Regex trex;
        // Currently selected talents list
        Talents CurTalents = null;
        // Talents profile dir
        private string wdir;
        // Hunter BM
        // http://www.wowarmory.com/talent-calc.xml?cid=3&tal=05203001525210250130501341 205035200000000000000000000 0300000000000000000000000000
        // Pally Ret
        // http://www.wowarmory.com/talent-calc.xml?cid=2&tal=05000000000000000000000000 05000000000000000000000000 50232251223331322133231331

        public TalentsForm() : base ("talents")
        {
            InitializeComponent();

            trex = new Regex(ProcessManager.CurWoWVersion.TalentConfig.ArmoryPattern);
        }

        private int LevelLabel
        {
            get
            {
                return (labelLevelNum.Text.Equals("")) ? 0 : 
                            Convert.ToInt32(labelLevelNum.Text);
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            if (cbWoWVersion.SelectedItem == null)
            {
                ShowErrorMessage("WoW Version not selected");
                return;
            }
            
            // Check learning order
            int[] ls = null;
            if (tbLearningOrder.Text.Equals(""))
                ls = new int[] {1, 2, 3};
            else
            {
                string[] lorder = tbLearningOrder.Text.Split(',');
                int ll = lorder.Length;
                if (ll > 3)
                {
                    ShowErrorMessage("Invalid learning sequence '" + tbLearningOrder.Text + 
                    "'. Number of tabs exceed 3");
                    return;
                }
                
                // convert each tab to int
                ls = new int[ll];

                try
                {
                    for (int i = 0; i < ll; i++)
                        ls[i] = Convert.ToInt32(lorder[i]);
                } catch {
                    ShowErrorMessage("Invalid Tab Id parameter in learning sequence '" + 
                        tbLearningOrder.Text + "'");
                    return;
                }
            }
            
            // Finally process URL
            try
            {
                string url = tbTalentURL.Text;
                // Check for class id
                Match m = trex.Match(url);

                if (m.Success && (m.Groups.Count == 3))
                {
                    string s = m.Groups[1].ToString();
                    byte cid = Convert.ToByte(s);
                    string ts = m.Groups[2].ToString();
                    
                    // string response = ReadURL(url);
                    
                    // Find class
                    WoWVersion cur_version = (WoWVersion) cbWoWVersion.SelectedItem;
                    CharClass cc = cur_version.Classes.FindClassByArmoryId(cid);
                    
                    // Check if Class defined
                    if (cc == null)
                    {
                        ShowErrorMessage("Unable find class for Armory ID " + cid);
                        return;
                    }

                    // Check size of talents
                    int cc_total = cc.TotalTalentSum;
                    if (cc_total != ts.Length)
                    {
                        ShowErrorMessage("Length of talent's URL parameter " + ts.Length +
                            " different from class parameter length " + cc_total + 
                            " configured in WoWData.xml");
                        return;
                    }
                    
                    
                    byte[] tabs_len = cc.Tabs;

                    // Convert talent list to byte array
                    byte[] tlist = new byte[cc_total];
                    for (int i = 0; i < cc_total; i++)
                        tlist[i] = Convert.ToByte(ts.Substring(i, 1));

                    byte num = cur_version.TalentConfig.StartLevel;
                    
                    // Select class
                    cbClass.SelectedItem = cc;
                    cbClass.Enabled = false;

                    // Clear CurTalents
                    if (CurTalents == null)
                        CurTalents = new Talents();
                    else
                        btnReset_Click(sender, e);

                    BindLevels();

                    for (int i = 0; i < ls.Length; i++)
                    {
                        int cur_tab_id = ls[i];
                        int offset = 0;
                        for (int j = 0; j < cur_tab_id - 1; j++)
                            offset += tabs_len[j];

                        int cur_tab_len = tabs_len[cur_tab_id - 1];
                        for (int j = 0; j < cur_tab_len; j++)
                        {
                            byte ct = tlist[offset + j];
                            // Added current talent with all ranks
                            for (int k = 1; k <= ct; k++)
                            {
                                CurTalents.AddLevel(new Level(num, cur_tab_id, j + 1, k));
                                num++;
                                RefreshLevelList();
                            }
                        }
                    }
                } else {
                    ShowErrorMessage(string.Format(
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
            bool empty = tbTalentURL.Text.Equals("");
            btnImport.Enabled = !empty;
            btnViewURL.Enabled = !empty;

            RegisterChange();
        }

        private void BindClasses()
        {
            if (cbWoWVersion.SelectedItem != null)
            {
                cbClass.DataSource = ((WoWVersion)cbWoWVersion.
                            SelectedItem).Classes.Items;

                cbClass.DisplayMember = "LongName";
                cbClass.ValueMember = "Name";
                // cbClass.ValueMember = "LongName";
            }
            cbClass.SelectedItem = null;
        }

        private void TalentsForm_Load(object sender, EventArgs e)
        {
            wdir = ProcessManager.Config.ProfilesDir +
                        Path.DirectorySeparatorChar + "Talents";

            cbWoWVersion.DataSource = ProcessManager.WoWVersions;
            cbWoWVersion.SelectedItem = ProcessManager.CurWoWVersion;

            BindClasses();

            IsChanged = false;
        }

        private void cbTalentTemplates_DropDown(object sender, EventArgs e)
        {
            // Remember last selection
            string saved = cbTalentTemplates.Text;

            cbTalentTemplates.DataSource = null;

            // Disable event handler b4 assign datasource
            // EventHandler h = cbTalentTemplates.DropDown;
            cbTalentTemplates.SelectedIndexChanged -= 
                    cbTalentTemplates_SelectedIndexChanged;

            cbTalentTemplates.DataSource = ProcessManager.TalentTemplateList;
            cbTalentTemplates.SelectedIndex = -1;

            // Restore event handler back
            cbTalentTemplates.SelectedIndexChanged += 
                    cbTalentTemplates_SelectedIndexChanged;

            // Restore back edited template
            cbTalentTemplates.Text = saved;
        }

        private void SelectClass()
        {
            if ((cbWoWVersion.SelectedItem != null) && 
                (CurTalents != null) &&
                (CurTalents.Class != null))
            {
                cbClass.SelectedItem = ((WoWVersion)cbWoWVersion.SelectedItem).
                    Classes.FindClassByName(CurTalents.Class);
                cbClass.Enabled = false;

            }
            else
                cbClass.SelectedItem = null;
        }

        private void cbTalentTemplates_SelectedIndexChanged(object sender, EventArgs e)
        {
            CurTalents = (Talents)cbTalentTemplates.SelectedItem;
            // Doesn't make sence process if tallent not selected
            if (CurTalents == null)
                return;

            if (CurTalents.WoWVersion != null)
                cbWoWVersion.SelectedItem =
                    ProcessManager.FindWoWVersionByName(CurTalents.WoWVersion);
            else
                cbWoWVersion.SelectedItem = null;

            SelectClass();

            tbDescription.Text = CurTalents.Description;
            tbTalentURL.Text = CurTalents.URL;
            tbLearningOrder.Text = CurTalents.LearningOrder;

            // Clear binding
            if (lbLevelList.DataSource != null)
                lbLevelList.DataSource = null;

            lbLevelList.Items.Clear();

            BindLevels();

            lbLevelList.SelectedIndex = 0;

            IsChanged = false;
            CheckSaveBtn();
        }

        private void CheckSaveBtn()
        {
            bool is_header_set = (!tbDescription.Text.Equals("") &&
                                        !cbClass.Text.Equals("") &&
                                        // Talent template not empty 
                                        !cbTalentTemplates.Text.Equals("") );
            bool not_empty = (lbLevelList.Items.Count > 0);
            bool is_selected = (lbLevelList.SelectedIndex >= 0);

            btnUpdate.Enabled = is_selected;

            labelLevelNum.Visible = btnUpdate.Enabled;
            numTab.Visible = btnUpdate.Enabled;
            numTalent.Visible = btnUpdate.Enabled;
            numRank.Visible = btnUpdate.Enabled;

            btnSave.Enabled = (not_empty && is_header_set && IsChanged);

            btnAdd.Enabled = ((LevelLabel < ProcessManager.
                        CurWoWVersion.MaxLvl) && is_header_set);

            btnUp.Enabled = (lbLevelList.SelectedIndex > 0);
            btnDown.Enabled = (lbLevelList.SelectedIndex < (lbLevelList.Items.Count - 1));
            btnRemove.Enabled = (not_empty && 
                        (lbLevelList.SelectedIndex ==
                                        (lbLevelList.Items.Count - 1)));
        }

        private void lbLevelList_SelectedIndexChanged(object sender, EventArgs e)
        {
            Level l = (Level) lbLevelList.SelectedItem;

            if (l != null)
            {
                try
                {
                    labelLevelNum.Text = Convert.ToString(l.Num);
                    numTab.Value = l.TabId;
                    numTalent.Value = l.TalentId;
                    numRank.Value = l.Rank;

                    CheckSaveBtn();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);

                    // Unselect everything
                    lbLevelList.SelectedIndex = -1;
                }
            }

            CheckSaveBtn();
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            if (lbLevelList.SelectedValue == null) return;

            ((Level)lbLevelList.SelectedValue).Update((int) numTab.Value, 
                (int) numTalent.Value, (int) numRank.Value);

            RefreshLevelList();
            RegisterChange();
        }

        protected void RegisterChange()
        {
            IsChanged = true;
            CheckSaveBtn();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            // Select previous item
            lbLevelList.SelectedIndex = lbLevelList.Items.Count - 2;

            // Only can remove last item
            int idx = lbLevelList.Items.Count - 1;
            CurTalents.Levels.RemoveAt(idx);

            RefreshLevelList();
            RegisterChange();
        }

        private void tbDescription_TextChanged(object sender, EventArgs e)
        {
            RegisterChange();
        }

        private void cbClass_TextChanged(object sender, EventArgs e)
        {
            RegisterChange();
        }

        private void cbTalentTemplates_TextChanged(object sender, EventArgs e)
        {
            if (CurTalents != null)
                CurTalents.FullPath = wdir + Path.DirectorySeparatorChar + 
                                            cbTalentTemplates.Text + ".xml";

            RegisterChange();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("", ""); // Remove  xmlns: parameters

                XmlSerializer s = new XmlSerializer(typeof(Talents));
                TextWriter w = new StreamWriter(CurTalents.FullPath);

                // Save parameters as well
                CurTalents.URL = tbTalentURL.Text;
                CurTalents.LearningOrder = tbLearningOrder.Text;
                CurTalents.Description = tbDescription.Text;
                CurTalents.Class = ((CharClass) cbClass.SelectedItem).Name;
                CurTalents.WoWVersion = cbWoWVersion.Text;

                s.Serialize(w, CurTalents, ns);
                w.Close();

                IsChanged = false;
                btnSave.Enabled = false;

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
            int num = 10;
            int tab = 1;
            int talent = 1;
            int rank = 1;

            // Find last item
            if ((CurTalents != null) && (CurTalents.Levels.Count > 0))
            {
                Level last = (Level)CurTalents.Levels[CurTalents.Levels.Count - 1];

                num = last.Num + 1;
                tab = last.TabId;
                talent = last.TalentId;

                if (last.Rank < numRank.Maximum)
                    rank = last.Rank + 1;
                else
                    talent++;
            }

            Level l  = new Level(num , tab, talent, rank);

            bool is_new = (CurTalents == null);
            if (is_new)
            {
                // Don't forget add .xml extension for new file
                CurTalents = new Talents(cbTalentTemplates.Text + ".xml",
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
            ((CurrencyManager)BindingContext[CurTalents.Levels]).Refresh();
        }

        private void BindLevels()
        {
            lbLevelList.DataSource = CurTalents.Levels;
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            if (lbLevelList.SelectedIndex <= 0) return;

            int idx = lbLevelList.SelectedIndex;
            SwitchLevels((Level)CurTalents.Levels[idx],
                        (Level)CurTalents.Levels[idx - 1]);
            
            lbLevelList.SelectedIndex = (idx - 1);

            RegisterChange();
        }

        private void SwitchLevels(Level cur, Level prev)
        {
            Level saved = (Level)cur.Clone();

            cur.Update(prev.TabId, prev.TalentId, prev.Rank);
            RefreshLevelList();
            prev.Update(saved.TabId, saved.TalentId, saved.Rank);
            RefreshLevelList();
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            if ((lbLevelList.SelectedIndex < 0) || 
                    (lbLevelList.SelectedIndex == lbLevelList.Items.Count - 1)) return;

            int idx = lbLevelList.SelectedIndex;
            SwitchLevels((Level)CurTalents.Levels[idx],
                        (Level)CurTalents.Levels[idx + 1]);

            lbLevelList.SelectedIndex = (idx + 1);

            RegisterChange();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            if (CurTalents != null)
            {
                CurTalents.Levels.Clear();
                RefreshLevelList();

                RegisterChange();
            }
        }

        private void cbWoWVersion_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Change binding for Class
            BindClasses();
            SelectClass();
        }

        private void btnViewURL_Click(object sender, EventArgs e)
        {
            OpenURL(tbTalentURL.Text);
        }

        private void tbLearningOrder_TextChanged(object sender, EventArgs e)
        {
            RegisterChange();
        }
    }
}
