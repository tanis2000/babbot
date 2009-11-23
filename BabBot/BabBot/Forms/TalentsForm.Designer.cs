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
            this.cbTalentTemplates = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.labelAssignment = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.labelLevel = new System.Windows.Forms.Label();
            this.nudLevel = new System.Windows.Forms.NumericUpDown();
            this.labelTab = new System.Windows.Forms.Label();
            this.labelTalent = new System.Windows.Forms.Label();
            this.numTalent = new System.Windows.Forms.NumericUpDown();
            this.numTab = new System.Windows.Forms.NumericUpDown();
            this.btnApply = new System.Windows.Forms.Button();
            this.btnImport = new System.Windows.Forms.Button();
            this.labelURL = new System.Windows.Forms.Label();
            this.tbTalentURL = new System.Windows.Forms.TextBox();
            this.btnRemove = new System.Windows.Forms.Button();
            this.cbClass = new System.Windows.Forms.ComboBox();
            this.labelClass = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nudLevel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTalent)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTab)).BeginInit();
            this.SuspendLayout();
            // 
            // cbTalentTemplates
            // 
            this.cbTalentTemplates.FormattingEnabled = true;
            this.cbTalentTemplates.Location = new System.Drawing.Point(105, 6);
            this.cbTalentTemplates.Name = "cbTalentTemplates";
            this.cbTalentTemplates.Size = new System.Drawing.Size(164, 21);
            this.cbTalentTemplates.TabIndex = 0;
            this.cbTalentTemplates.DropDown += new System.EventHandler(this.cbTalentTemplates_DropDown);
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
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(12, 140);
            this.listBox1.Name = "listBox1";
            this.listBox1.ScrollAlwaysVisible = true;
            this.listBox1.Size = new System.Drawing.Size(146, 238);
            this.listBox1.TabIndex = 3;
            // 
            // labelAssignment
            // 
            this.labelAssignment.AutoSize = true;
            this.labelAssignment.Location = new System.Drawing.Point(9, 124);
            this.labelAssignment.Name = "labelAssignment";
            this.labelAssignment.Size = new System.Drawing.Size(146, 13);
            this.labelAssignment.TabIndex = 4;
            this.labelAssignment.Text = "Talents Assignment per Level";
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(193, 397);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.Enabled = false;
            this.btnSave.Location = new System.Drawing.Point(104, 397);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 6;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // labelLevel
            // 
            this.labelLevel.AutoSize = true;
            this.labelLevel.Location = new System.Drawing.Point(185, 142);
            this.labelLevel.Name = "labelLevel";
            this.labelLevel.Size = new System.Drawing.Size(36, 13);
            this.labelLevel.TabIndex = 7;
            this.labelLevel.Text = "Level:";
            // 
            // nudLevel
            // 
            this.nudLevel.Location = new System.Drawing.Point(227, 140);
            this.nudLevel.Maximum = new decimal(new int[] {
            80,
            0,
            0,
            0});
            this.nudLevel.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudLevel.Name = "nudLevel";
            this.nudLevel.Size = new System.Drawing.Size(42, 20);
            this.nudLevel.TabIndex = 8;
            this.nudLevel.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // labelTab
            // 
            this.labelTab.AutoSize = true;
            this.labelTab.Location = new System.Drawing.Point(185, 168);
            this.labelTab.Name = "labelTab";
            this.labelTab.Size = new System.Drawing.Size(29, 13);
            this.labelTab.TabIndex = 9;
            this.labelTab.Text = "Tab:";
            // 
            // labelTalent
            // 
            this.labelTalent.AutoSize = true;
            this.labelTalent.Location = new System.Drawing.Point(185, 194);
            this.labelTalent.Name = "labelTalent";
            this.labelTalent.Size = new System.Drawing.Size(40, 13);
            this.labelTalent.TabIndex = 10;
            this.labelTalent.Text = "Talent:";
            // 
            // numTalent
            // 
            this.numTalent.Location = new System.Drawing.Point(227, 192);
            this.numTalent.Name = "numTalent";
            this.numTalent.Size = new System.Drawing.Size(42, 20);
            this.numTalent.TabIndex = 11;
            // 
            // numTab
            // 
            this.numTab.Location = new System.Drawing.Point(227, 166);
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
            // btnApply
            // 
            this.btnApply.Location = new System.Drawing.Point(193, 218);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(75, 23);
            this.btnApply.TabIndex = 13;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            // 
            // btnImport
            // 
            this.btnImport.Enabled = false;
            this.btnImport.Location = new System.Drawing.Point(206, 77);
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
            this.tbTalentURL.Location = new System.Drawing.Point(12, 51);
            this.tbTalentURL.Name = "tbTalentURL";
            this.tbTalentURL.Size = new System.Drawing.Size(257, 20);
            this.tbTalentURL.TabIndex = 16;
            this.tbTalentURL.Text = "http://www.wowarmory.com/talent-calc.xml?cid=6&tal=200000000000000000000000000000" +
                "0000000000000000000000000000000000000000000000000000000000";
            this.tbTalentURL.TextChanged += new System.EventHandler(this.tbTalentURL_TextChanged);
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(192, 276);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(75, 23);
            this.btnRemove.TabIndex = 17;
            this.btnRemove.Text = "Remove";
            this.btnRemove.UseVisualStyleBackColor = true;
            // 
            // cbClass
            // 
            this.cbClass.FormattingEnabled = true;
            this.cbClass.Location = new System.Drawing.Point(50, 91);
            this.cbClass.Name = "cbClass";
            this.cbClass.Size = new System.Drawing.Size(108, 21);
            this.cbClass.TabIndex = 18;
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
            // TalentsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(280, 432);
            this.Controls.Add(this.labelClass);
            this.Controls.Add(this.cbClass);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.tbTalentURL);
            this.Controls.Add(this.labelURL);
            this.Controls.Add(this.btnImport);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.numTab);
            this.Controls.Add(this.numTalent);
            this.Controls.Add(this.labelTalent);
            this.Controls.Add(this.labelTab);
            this.Controls.Add(this.nudLevel);
            this.Controls.Add(this.labelLevel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.labelAssignment);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbTalentTemplates);
            this.Name = "TalentsForm";
            this.Text = "Talents";
            this.Load += new System.EventHandler(this.TalentsForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nudLevel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTalent)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTab)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbTalentTemplates;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Label labelAssignment;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label labelLevel;
        private System.Windows.Forms.NumericUpDown nudLevel;
        private System.Windows.Forms.Label labelTab;
        private System.Windows.Forms.Label labelTalent;
        private System.Windows.Forms.NumericUpDown numTalent;
        private System.Windows.Forms.NumericUpDown numTab;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.Label labelURL;
        private System.Windows.Forms.TextBox tbTalentURL;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.ComboBox cbClass;
        private System.Windows.Forms.Label labelClass;
    }
}