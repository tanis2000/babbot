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
            this.cbWoWVersion = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tviewNpcList = new System.Windows.Forms.TreeView();
            this.imlRaces = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // btnHelp
            // 
            this.btnHelp.Location = new System.Drawing.Point(12, 334);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(267, 334);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(186, 334);
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
            // cbWoWVersion
            // 
            this.cbWoWVersion.FormattingEnabled = true;
            this.cbWoWVersion.Location = new System.Drawing.Point(91, 6);
            this.cbWoWVersion.Name = "cbWoWVersion";
            this.cbWoWVersion.Size = new System.Drawing.Size(86, 21);
            this.cbWoWVersion.TabIndex = 31;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 13);
            this.label1.TabIndex = 33;
            this.label1.Text = "NPC List";
            // 
            // tviewNpcList
            // 
            this.tviewNpcList.ImageIndex = 0;
            this.tviewNpcList.ImageList = this.imlRaces;
            this.tviewNpcList.Location = new System.Drawing.Point(12, 57);
            this.tviewNpcList.Name = "tviewNpcList";
            this.tviewNpcList.SelectedImageIndex = 0;
            this.tviewNpcList.Size = new System.Drawing.Size(165, 271);
            this.tviewNpcList.TabIndex = 34;
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
            // NPCListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(354, 369);
            this.Controls.Add(this.tviewNpcList);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cbWoWVersion);
            this.Name = "NPCListForm";
            this.Load += new System.EventHandler(this.NPCListForm_Load);
            this.Controls.SetChildIndex(this.btnHelp, 0);
            this.Controls.SetChildIndex(this.btnClose, 0);
            this.Controls.SetChildIndex(this.btnSave, 0);
            this.Controls.SetChildIndex(this.cbWoWVersion, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.tviewNpcList, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbWoWVersion;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TreeView tviewNpcList;
        private System.Windows.Forms.ImageList imlRaces;
    }
}
