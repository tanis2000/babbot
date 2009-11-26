namespace BabBot.Forms
{
    partial class TalentsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TalentsForm));
            this.cbTalentTemplates = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lbLevelList = new System.Windows.Forms.ListBox();
            this.labelAssignment = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.labelLevel = new System.Windows.Forms.Label();
            this.labelTab = new System.Windows.Forms.Label();
            this.labelTalent = new System.Windows.Forms.Label();
            this.numTalent = new System.Windows.Forms.NumericUpDown();
            this.numTab = new System.Windows.Forms.NumericUpDown();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnImport = new System.Windows.Forms.Button();
            this.labelURL = new System.Windows.Forms.Label();
            this.tbTalentURL = new System.Windows.Forms.TextBox();
            this.btnRemove = new System.Windows.Forms.Button();
            this.cbClass = new System.Windows.Forms.ComboBox();
            this.labelClass = new System.Windows.Forms.Label();
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.labelDescr = new System.Windows.Forms.Label();
            this.numRank = new System.Windows.Forms.NumericUpDown();
            this.labelRank = new System.Windows.Forms.Label();
            this.labelLevelNum = new System.Windows.Forms.Label();
            this.btnUp = new System.Windows.Forms.Button();
            this.btnDown = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numTalent)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTab)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRank)).BeginInit();
            this.SuspendLayout();
            // 
            // cbTalentTemplates
            // 
            this.cbTalentTemplates.FormattingEnabled = true;
            this.cbTalentTemplates.Location = new System.Drawing.Point(105, 6);
            this.cbTalentTemplates.Name = "cbTalentTemplates";
            this.cbTalentTemplates.Size = new System.Drawing.Size(164, 21);
            this.cbTalentTemplates.TabIndex = 0;
            this.cbTalentTemplates.SelectedIndexChanged += new System.EventHandler(this.cbTalentTemplates_SelectedIndexChanged);
            this.cbTalentTemplates.DropDown += new System.EventHandler(this.cbTalentTemplates_DropDown);
            this.cbTalentTemplates.TextChanged += new System.EventHandler(this.cbTalentTemplates_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Talent Template:";
            // 
            // lbLevelList
            // 
            this.lbLevelList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lbLevelList.FormattingEnabled = true;
            this.lbLevelList.Location = new System.Drawing.Point(12, 166);
            this.lbLevelList.Name = "lbLevelList";
            this.lbLevelList.ScrollAlwaysVisible = true;
            this.lbLevelList.Size = new System.Drawing.Size(153, 212);
            this.lbLevelList.TabIndex = 3;
            this.lbLevelList.SelectedIndexChanged += new System.EventHandler(this.lbLevelList_SelectedIndexChanged);
            this.lbLevelList.SelectedValueChanged += new System.EventHandler(this.lbLevelList_SelectedValueChanged);
            // 
            // labelAssignment
            // 
            this.labelAssignment.AutoSize = true;
            this.labelAssignment.Location = new System.Drawing.Point(9, 147);
            this.labelAssignment.Name = "labelAssignment";
            this.labelAssignment.Size = new System.Drawing.Size(146, 13);
            this.labelAssignment.TabIndex = 4;
            this.labelAssignment.Text = "Talents Assignment per Level";
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(204, 401);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Enabled = false;
            this.btnSave.Location = new System.Drawing.Point(116, 401);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 6;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // labelLevel
            // 
            this.labelLevel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelLevel.AutoSize = true;
            this.labelLevel.Location = new System.Drawing.Point(192, 168);
            this.labelLevel.Name = "labelLevel";
            this.labelLevel.Size = new System.Drawing.Size(36, 13);
            this.labelLevel.TabIndex = 7;
            this.labelLevel.Text = "Level:";
            // 
            // labelTab
            // 
            this.labelTab.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelTab.AutoSize = true;
            this.labelTab.Location = new System.Drawing.Point(192, 194);
            this.labelTab.Name = "labelTab";
            this.labelTab.Size = new System.Drawing.Size(29, 13);
            this.labelTab.TabIndex = 9;
            this.labelTab.Text = "Tab:";
            // 
            // labelTalent
            // 
            this.labelTalent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelTalent.AutoSize = true;
            this.labelTalent.Location = new System.Drawing.Point(192, 220);
            this.labelTalent.Name = "labelTalent";
            this.labelTalent.Size = new System.Drawing.Size(40, 13);
            this.labelTalent.TabIndex = 10;
            this.labelTalent.Text = "Talent:";
            // 
            // numTalent
            // 
            this.numTalent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numTalent.Location = new System.Drawing.Point(239, 218);
            this.numTalent.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.numTalent.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numTalent.Name = "numTalent";
            this.numTalent.Size = new System.Drawing.Size(42, 20);
            this.numTalent.TabIndex = 11;
            this.numTalent.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // numTab
            // 
            this.numTab.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numTab.Location = new System.Drawing.Point(239, 192);
            this.numTab.Maximum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.numTab.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numTab.Name = "numTab";
            this.numTab.Size = new System.Drawing.Size(42, 20);
            this.numTab.TabIndex = 12;
            this.numTab.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // btnUpdate
            // 
            this.btnUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUpdate.Enabled = false;
            this.btnUpdate.Location = new System.Drawing.Point(196, 273);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(85, 23);
            this.btnUpdate.TabIndex = 13;
            this.btnUpdate.Tag = "";
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // btnImport
            // 
            this.btnImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImport.Enabled = false;
            this.btnImport.Location = new System.Drawing.Point(218, 77);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(63, 23);
            this.btnImport.TabIndex = 14;
            this.btnImport.Text = "Import";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // labelURL
            // 
            this.labelURL.AutoSize = true;
            this.labelURL.Location = new System.Drawing.Point(9, 34);
            this.labelURL.Name = "labelURL";
            this.labelURL.Size = new System.Drawing.Size(98, 13);
            this.labelURL.TabIndex = 15;
            this.labelURL.Text = "WoW Armory URL:";
            // 
            // tbTalentURL
            // 
            this.tbTalentURL.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbTalentURL.Location = new System.Drawing.Point(12, 51);
            this.tbTalentURL.Name = "tbTalentURL";
            this.tbTalentURL.Size = new System.Drawing.Size(269, 20);
            this.tbTalentURL.TabIndex = 16;
            this.tbTalentURL.Text = "http://www.wowarmory.com/talent-calc.xml?cid=6&tal=200000000000000000000000000000" +
                "0000000000000000000000000000000000000000000000000000000000";
            this.tbTalentURL.TextChanged += new System.EventHandler(this.tbTalentURL_TextChanged);
            // 
            // btnRemove
            // 
            this.btnRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemove.Enabled = false;
            this.btnRemove.Location = new System.Drawing.Point(196, 360);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(83, 23);
            this.btnRemove.TabIndex = 17;
            this.btnRemove.Text = "Remove";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // cbClass
            // 
            this.cbClass.FormattingEnabled = true;
            this.cbClass.Location = new System.Drawing.Point(50, 91);
            this.cbClass.Name = "cbClass";
            this.cbClass.Size = new System.Drawing.Size(108, 21);
            this.cbClass.TabIndex = 18;
            this.cbClass.TextChanged += new System.EventHandler(this.cbClass_TextChanged);
            // 
            // labelClass
            // 
            this.labelClass.AutoSize = true;
            this.labelClass.Location = new System.Drawing.Point(9, 94);
            this.labelClass.Name = "labelClass";
            this.labelClass.Size = new System.Drawing.Size(35, 13);
            this.labelClass.TabIndex = 19;
            this.labelClass.Text = "Class:";
            // 
            // tbDescription
            // 
            this.tbDescription.Location = new System.Drawing.Point(50, 118);
            this.tbDescription.Name = "tbDescription";
            this.tbDescription.Size = new System.Drawing.Size(229, 20);
            this.tbDescription.TabIndex = 20;
            this.tbDescription.TextChanged += new System.EventHandler(this.tbDescription_TextChanged);
            // 
            // labelDescr
            // 
            this.labelDescr.AutoSize = true;
            this.labelDescr.Location = new System.Drawing.Point(9, 121);
            this.labelDescr.Name = "labelDescr";
            this.labelDescr.Size = new System.Drawing.Size(38, 13);
            this.labelDescr.TabIndex = 21;
            this.labelDescr.Text = "Descr:";
            // 
            // numRank
            // 
            this.numRank.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numRank.Location = new System.Drawing.Point(239, 244);
            this.numRank.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numRank.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numRank.Name = "numRank";
            this.numRank.Size = new System.Drawing.Size(42, 20);
            this.numRank.TabIndex = 23;
            this.numRank.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // labelRank
            // 
            this.labelRank.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelRank.AutoSize = true;
            this.labelRank.Location = new System.Drawing.Point(192, 246);
            this.labelRank.Name = "labelRank";
            this.labelRank.Size = new System.Drawing.Size(36, 13);
            this.labelRank.TabIndex = 22;
            this.labelRank.Text = "Rank:";
            // 
            // labelLevelNum
            // 
            this.labelLevelNum.AutoSize = true;
            this.labelLevelNum.Location = new System.Drawing.Point(236, 168);
            this.labelLevelNum.Name = "labelLevelNum";
            this.labelLevelNum.Size = new System.Drawing.Size(0, 13);
            this.labelLevelNum.TabIndex = 24;
            // 
            // btnUp
            // 
            this.btnUp.Enabled = false;
            this.btnUp.Location = new System.Drawing.Point(196, 302);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(30, 23);
            this.btnUp.TabIndex = 25;
            this.btnUp.Text = "Up";
            this.btnUp.UseVisualStyleBackColor = true;
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // btnDown
            // 
            this.btnDown.Enabled = false;
            this.btnDown.Location = new System.Drawing.Point(232, 302);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(47, 23);
            this.btnDown.TabIndex = 26;
            this.btnDown.Text = "Down";
            this.btnDown.UseVisualStyleBackColor = true;
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Enabled = false;
            this.btnAdd.Location = new System.Drawing.Point(195, 331);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(84, 23);
            this.btnAdd.TabIndex = 27;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // TalentsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(292, 436);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnDown);
            this.Controls.Add(this.btnUp);
            this.Controls.Add(this.labelLevelNum);
            this.Controls.Add(this.numRank);
            this.Controls.Add(this.labelRank);
            this.Controls.Add(this.labelDescr);
            this.Controls.Add(this.tbDescription);
            this.Controls.Add(this.labelClass);
            this.Controls.Add(this.cbClass);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.tbTalentURL);
            this.Controls.Add(this.labelURL);
            this.Controls.Add(this.btnImport);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.numTab);
            this.Controls.Add(this.numTalent);
            this.Controls.Add(this.labelTalent);
            this.Controls.Add(this.labelTab);
            this.Controls.Add(this.labelLevel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.labelAssignment);
            this.Controls.Add(this.lbLevelList);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbTalentTemplates);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(300, 470);
            this.Name = "TalentsForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Talents";
            this.Load += new System.EventHandler(this.TalentsForm_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TalentsForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.numTalent)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTab)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRank)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbTalentTemplates;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox lbLevelList;
        private System.Windows.Forms.Label labelAssignment;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label labelLevel;
        private System.Windows.Forms.Label labelTab;
        private System.Windows.Forms.Label labelTalent;
        private System.Windows.Forms.NumericUpDown numTalent;
        private System.Windows.Forms.NumericUpDown numTab;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.Label labelURL;
        private System.Windows.Forms.TextBox tbTalentURL;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.ComboBox cbClass;
        private System.Windows.Forms.Label labelClass;
        private System.Windows.Forms.TextBox tbDescription;
        private System.Windows.Forms.Label labelDescr;
        private System.Windows.Forms.NumericUpDown numRank;
        private System.Windows.Forms.Label labelRank;
        private System.Windows.Forms.Label labelLevelNum;
        private System.Windows.Forms.Button btnUp;
        private System.Windows.Forms.Button btnDown;
        private System.Windows.Forms.Button btnAdd;
    }
}