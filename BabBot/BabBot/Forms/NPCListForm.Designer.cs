namespace BabBot.Forms
{
    partial class NPCListForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NPCListForm));
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.imlRaces = new System.Windows.Forms.ImageList(this.components);
            this.lbNpcList = new System.Windows.Forms.ListBox();
            this.labelWoWVersion = new System.Windows.Forms.Label();
            this.btnImport = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.tbName = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnHelp
            // 
            this.btnHelp.Location = new System.Drawing.Point(12, 414);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(447, 414);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(366, 414);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 13);
            this.label2.TabIndex = 32;
            this.label2.Text = "WoW Version:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(257, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 18);
            this.label1.TabIndex = 33;
            this.label1.Text = "NPC List";
            // 
            // imlRaces
            // 
            this.imlRaces.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imlRaces.ImageStream")));
            this.imlRaces.TransparentColor = System.Drawing.Color.Transparent;
            this.imlRaces.Images.SetKeyName(0, "Ui-charactercreate-races_undead-male.png");
            this.imlRaces.Images.SetKeyName(1, "Ui-charactercreate-races_bloodelf-female.png");
            this.imlRaces.Images.SetKeyName(2, "Ui-charactercreate-races_bloodelf-male.png");
            this.imlRaces.Images.SetKeyName(3, "Ui-charactercreate-races_draenei-female.png");
            this.imlRaces.Images.SetKeyName(4, "Ui-charactercreate-races_draenei-male.png");
            this.imlRaces.Images.SetKeyName(5, "Ui-charactercreate-races_dwarf-female.png");
            this.imlRaces.Images.SetKeyName(6, "Ui-charactercreate-races_dwarf-male.png");
            this.imlRaces.Images.SetKeyName(7, "Ui-charactercreate-races_gnome-female.png");
            this.imlRaces.Images.SetKeyName(8, "Ui-charactercreate-races_gnome-male.png");
            this.imlRaces.Images.SetKeyName(9, "Ui-charactercreate-races_human-female.png");
            this.imlRaces.Images.SetKeyName(10, "Ui-charactercreate-races_human-male.png");
            this.imlRaces.Images.SetKeyName(11, "Ui-charactercreate-races_nightelf-female.png");
            this.imlRaces.Images.SetKeyName(12, "Ui-charactercreate-races_nightelf-male.png");
            this.imlRaces.Images.SetKeyName(13, "Ui-charactercreate-races_orc-female.png");
            this.imlRaces.Images.SetKeyName(14, "Ui-charactercreate-races_orc-male.png");
            this.imlRaces.Images.SetKeyName(15, "Ui-charactercreate-races_tauren-female.png");
            this.imlRaces.Images.SetKeyName(16, "Ui-charactercreate-races_tauren-male.png");
            this.imlRaces.Images.SetKeyName(17, "Ui-charactercreate-races_troll-female.png");
            this.imlRaces.Images.SetKeyName(18, "Ui-charactercreate-races_troll-male.png");
            this.imlRaces.Images.SetKeyName(19, "Ui-charactercreate-races_undead-female.png");
            // 
            // lbNpcList
            // 
            this.lbNpcList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lbNpcList.FormattingEnabled = true;
            this.lbNpcList.Location = new System.Drawing.Point(12, 33);
            this.lbNpcList.MultiColumn = true;
            this.lbNpcList.Name = "lbNpcList";
            this.lbNpcList.Size = new System.Drawing.Size(510, 199);
            this.lbNpcList.TabIndex = 34;
            // 
            // labelWoWVersion
            // 
            this.labelWoWVersion.AutoSize = true;
            this.labelWoWVersion.Location = new System.Drawing.Point(91, 9);
            this.labelWoWVersion.Name = "labelWoWVersion";
            this.labelWoWVersion.Size = new System.Drawing.Size(0, 13);
            this.labelWoWVersion.TabIndex = 35;
            // 
            // btnImport
            // 
            this.btnImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImport.Location = new System.Drawing.Point(285, 414);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(75, 23);
            this.btnImport.TabIndex = 36;
            this.btnImport.Text = "Import";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 13);
            this.label3.TabIndex = 37;
            this.label3.Text = "Name:";
            // 
            // tbName
            // 
            this.tbName.Location = new System.Drawing.Point(50, 19);
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(136, 20);
            this.tbName.TabIndex = 38;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.btnAdd);
            this.groupBox1.Controls.Add(this.comboBox1);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.listBox1);
            this.groupBox1.Controls.Add(this.tbName);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Enabled = false;
            this.groupBox1.Location = new System.Drawing.Point(12, 238);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(510, 170);
            this.groupBox1.TabIndex = 39;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "NPC Description";
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(105, 65);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 42;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(6, 67);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(93, 21);
            this.comboBox1.TabIndex = 41;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 51);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(51, 13);
            this.label4.TabIndex = 40;
            this.label4.Text = "Services:";
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(6, 94);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(174, 69);
            this.listBox1.TabIndex = 39;
            // 
            // NPCListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(534, 449);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnImport);
            this.Controls.Add(this.labelWoWVersion);
            this.Controls.Add(this.lbNpcList);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Name = "NPCListForm";
            this.Text = "NPC List";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.NPCListForm_FormClosing);
            this.Controls.SetChildIndex(this.btnHelp, 0);
            this.Controls.SetChildIndex(this.btnClose, 0);
            this.Controls.SetChildIndex(this.btnSave, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.lbNpcList, 0);
            this.Controls.SetChildIndex(this.labelWoWVersion, 0);
            this.Controls.SetChildIndex(this.btnImport, 0);
            this.Controls.SetChildIndex(this.groupBox1, 0);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ImageList imlRaces;
        private System.Windows.Forms.ListBox lbNpcList;
        private System.Windows.Forms.Label labelWoWVersion;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.ComboBox comboBox1;
    }
}
