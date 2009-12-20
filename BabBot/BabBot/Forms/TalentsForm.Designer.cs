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
            this.labelLevel = new System.Windows.Forms.Label();
            this.labelTab = new System.Windows.Forms.Label();
            this.labelTalent = new System.Windows.Forms.Label();
            this.numTalent = new System.Windows.Forms.NumericUpDown();
            this.numTab = new System.Windows.Forms.NumericUpDown();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnImport = new System.Windows.Forms.Button();
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
            this.btnReset = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.cbWoWVersion = new System.Windows.Forms.ComboBox();
            this.tbLearningOrder = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnViewURL = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.numTalent)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTab)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRank)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnHelp
            // 
            this.btnHelp.Location = new System.Drawing.Point(15, 554);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(537, 554);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(456, 554);
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
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
            this.lbLevelList.Location = new System.Drawing.Point(9, 24);
            this.lbLevelList.MultiColumn = true;
            this.lbLevelList.Name = "lbLevelList";
            this.lbLevelList.ScrollAlwaysVisible = true;
            this.lbLevelList.Size = new System.Drawing.Size(481, 277);
            this.lbLevelList.TabIndex = 3;
            this.lbLevelList.SelectedIndexChanged += new System.EventHandler(this.lbLevelList_SelectedIndexChanged);
            // 
            // labelLevel
            // 
            this.labelLevel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelLevel.AutoSize = true;
            this.labelLevel.Location = new System.Drawing.Point(503, 25);
            this.labelLevel.Name = "labelLevel";
            this.labelLevel.Size = new System.Drawing.Size(36, 13);
            this.labelLevel.TabIndex = 7;
            this.labelLevel.Text = "Level:";
            // 
            // labelTab
            // 
            this.labelTab.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelTab.AutoSize = true;
            this.labelTab.Location = new System.Drawing.Point(503, 47);
            this.labelTab.Name = "labelTab";
            this.labelTab.Size = new System.Drawing.Size(29, 13);
            this.labelTab.TabIndex = 9;
            this.labelTab.Text = "Tab:";
            // 
            // labelTalent
            // 
            this.labelTalent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelTalent.AutoSize = true;
            this.labelTalent.Location = new System.Drawing.Point(503, 73);
            this.labelTalent.Name = "labelTalent";
            this.labelTalent.Size = new System.Drawing.Size(40, 13);
            this.labelTalent.TabIndex = 10;
            this.labelTalent.Text = "Talent:";
            // 
            // numTalent
            // 
            this.numTalent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numTalent.Location = new System.Drawing.Point(550, 71);
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
            this.numTalent.Visible = false;
            // 
            // numTab
            // 
            this.numTab.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numTab.Location = new System.Drawing.Point(550, 45);
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
            this.numTab.Visible = false;
            // 
            // btnUpdate
            // 
            this.btnUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUpdate.Enabled = false;
            this.btnUpdate.Location = new System.Drawing.Point(509, 132);
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
            this.btnImport.Location = new System.Drawing.Point(548, 45);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(46, 23);
            this.btnImport.TabIndex = 14;
            this.btnImport.Text = "Import";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // tbTalentURL
            // 
            this.tbTalentURL.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbTalentURL.Location = new System.Drawing.Point(6, 19);
            this.tbTalentURL.Name = "tbTalentURL";
            this.tbTalentURL.Size = new System.Drawing.Size(588, 20);
            this.tbTalentURL.TabIndex = 16;
            this.tbTalentURL.TextChanged += new System.EventHandler(this.tbTalentURL_TextChanged);
            // 
            // btnRemove
            // 
            this.btnRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemove.Enabled = false;
            this.btnRemove.Location = new System.Drawing.Point(512, 254);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(82, 23);
            this.btnRemove.TabIndex = 17;
            this.btnRemove.Text = "Remove";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // cbClass
            // 
            this.cbClass.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbClass.FormattingEnabled = true;
            this.cbClass.Location = new System.Drawing.Point(55, 47);
            this.cbClass.Name = "cbClass";
            this.cbClass.Size = new System.Drawing.Size(127, 21);
            this.cbClass.TabIndex = 18;
            this.cbClass.TextChanged += new System.EventHandler(this.cbClass_TextChanged);
            // 
            // labelClass
            // 
            this.labelClass.AutoSize = true;
            this.labelClass.Location = new System.Drawing.Point(14, 50);
            this.labelClass.Name = "labelClass";
            this.labelClass.Size = new System.Drawing.Size(35, 13);
            this.labelClass.TabIndex = 19;
            this.labelClass.Text = "Class:";
            // 
            // tbDescription
            // 
            this.tbDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbDescription.Location = new System.Drawing.Point(55, 74);
            this.tbDescription.Name = "tbDescription";
            this.tbDescription.Size = new System.Drawing.Size(540, 20);
            this.tbDescription.TabIndex = 20;
            this.tbDescription.TextChanged += new System.EventHandler(this.tbDescription_TextChanged);
            // 
            // labelDescr
            // 
            this.labelDescr.AutoSize = true;
            this.labelDescr.Location = new System.Drawing.Point(14, 77);
            this.labelDescr.Name = "labelDescr";
            this.labelDescr.Size = new System.Drawing.Size(38, 13);
            this.labelDescr.TabIndex = 21;
            this.labelDescr.Text = "Descr:";
            // 
            // numRank
            // 
            this.numRank.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numRank.Location = new System.Drawing.Point(550, 97);
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
            this.numRank.Visible = false;
            // 
            // labelRank
            // 
            this.labelRank.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelRank.AutoSize = true;
            this.labelRank.Location = new System.Drawing.Point(503, 99);
            this.labelRank.Name = "labelRank";
            this.labelRank.Size = new System.Drawing.Size(36, 13);
            this.labelRank.TabIndex = 22;
            this.labelRank.Text = "Rank:";
            // 
            // labelLevelNum
            // 
            this.labelLevelNum.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelLevelNum.AutoSize = true;
            this.labelLevelNum.Location = new System.Drawing.Point(547, 25);
            this.labelLevelNum.Name = "labelLevelNum";
            this.labelLevelNum.Size = new System.Drawing.Size(0, 13);
            this.labelLevelNum.TabIndex = 24;
            this.labelLevelNum.Visible = false;
            // 
            // btnUp
            // 
            this.btnUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUp.Enabled = false;
            this.btnUp.Location = new System.Drawing.Point(512, 196);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(30, 23);
            this.btnUp.TabIndex = 25;
            this.btnUp.Text = "Up";
            this.btnUp.UseVisualStyleBackColor = true;
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // btnDown
            // 
            this.btnDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDown.Enabled = false;
            this.btnDown.Location = new System.Drawing.Point(548, 196);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(47, 23);
            this.btnDown.TabIndex = 26;
            this.btnDown.Text = "Down";
            this.btnDown.UseVisualStyleBackColor = true;
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAdd.Enabled = false;
            this.btnAdd.Location = new System.Drawing.Point(511, 225);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(84, 23);
            this.btnAdd.TabIndex = 27;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnReset
            // 
            this.btnReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReset.Location = new System.Drawing.Point(512, 283);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(82, 23);
            this.btnReset.TabIndex = 28;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 13);
            this.label2.TabIndex = 30;
            this.label2.Text = "WoW Version:";
            // 
            // cbWoWVersion
            // 
            this.cbWoWVersion.FormattingEnabled = true;
            this.cbWoWVersion.Location = new System.Drawing.Point(96, 20);
            this.cbWoWVersion.Name = "cbWoWVersion";
            this.cbWoWVersion.Size = new System.Drawing.Size(86, 21);
            this.cbWoWVersion.TabIndex = 29;
            this.cbWoWVersion.SelectedIndexChanged += new System.EventHandler(this.cbWoWVersion_SelectedIndexChanged);
            // 
            // tbLearningOrder
            // 
            this.tbLearningOrder.Location = new System.Drawing.Point(92, 45);
            this.tbLearningOrder.Name = "tbLearningOrder";
            this.tbLearningOrder.Size = new System.Drawing.Size(70, 20);
            this.tbLearningOrder.TabIndex = 31;
            this.tbLearningOrder.TextChanged += new System.EventHandler(this.tbLearningOrder_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 48);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 13);
            this.label3.TabIndex = 32;
            this.label3.Text = "Learning Order:";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.btnViewURL);
            this.groupBox1.Controls.Add(this.tbTalentURL);
            this.groupBox1.Controls.Add(this.tbLearningOrder);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.btnImport);
            this.groupBox1.Location = new System.Drawing.Point(12, 33);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(600, 76);
            this.groupBox1.TabIndex = 33;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Import From WoW Armory URL";
            // 
            // btnViewURL
            // 
            this.btnViewURL.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnViewURL.Enabled = false;
            this.btnViewURL.Location = new System.Drawing.Point(498, 45);
            this.btnViewURL.Name = "btnViewURL";
            this.btnViewURL.Size = new System.Drawing.Size(44, 23);
            this.btnViewURL.TabIndex = 31;
            this.btnViewURL.Text = "View";
            this.btnViewURL.UseVisualStyleBackColor = true;
            this.btnViewURL.Click += new System.EventHandler(this.btnViewURL_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.tbDescription);
            this.groupBox2.Controls.Add(this.cbClass);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.labelClass);
            this.groupBox2.Controls.Add(this.cbWoWVersion);
            this.groupBox2.Controls.Add(this.labelDescr);
            this.groupBox2.Location = new System.Drawing.Point(12, 115);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(600, 100);
            this.groupBox2.TabIndex = 34;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Profile Header";
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.labelLevel);
            this.groupBox3.Controls.Add(this.labelTab);
            this.groupBox3.Controls.Add(this.labelTalent);
            this.groupBox3.Controls.Add(this.labelLevelNum);
            this.groupBox3.Controls.Add(this.numTalent);
            this.groupBox3.Controls.Add(this.btnReset);
            this.groupBox3.Controls.Add(this.btnAdd);
            this.groupBox3.Controls.Add(this.btnDown);
            this.groupBox3.Controls.Add(this.numTab);
            this.groupBox3.Controls.Add(this.btnUp);
            this.groupBox3.Controls.Add(this.labelRank);
            this.groupBox3.Controls.Add(this.numRank);
            this.groupBox3.Controls.Add(this.btnRemove);
            this.groupBox3.Controls.Add(this.lbLevelList);
            this.groupBox3.Controls.Add(this.btnUpdate);
            this.groupBox3.Location = new System.Drawing.Point(12, 226);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(600, 316);
            this.groupBox3.TabIndex = 35;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Talents Assignment per Level";
            // 
            // TalentsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(620, 589);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbTalentTemplates);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(300, 470);
            this.Name = "TalentsForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Talents";
            this.Load += new System.EventHandler(this.TalentsForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TalentsForm_KeyDown);
            this.Controls.SetChildIndex(this.btnClose, 0);
            this.Controls.SetChildIndex(this.btnSave, 0);
            this.Controls.SetChildIndex(this.btnHelp, 0);
            this.Controls.SetChildIndex(this.cbTalentTemplates, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.groupBox1, 0);
            this.Controls.SetChildIndex(this.groupBox2, 0);
            this.Controls.SetChildIndex(this.groupBox3, 0);
            ((System.ComponentModel.ISupportInitialize)(this.numTalent)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTab)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRank)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbTalentTemplates;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox lbLevelList;
        private System.Windows.Forms.Label labelLevel;
        private System.Windows.Forms.Label labelTab;
        private System.Windows.Forms.Label labelTalent;
        private System.Windows.Forms.NumericUpDown numTalent;
        private System.Windows.Forms.NumericUpDown numTab;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnImport;
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
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbWoWVersion;
        private System.Windows.Forms.TextBox tbLearningOrder;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnViewURL;
    }
}