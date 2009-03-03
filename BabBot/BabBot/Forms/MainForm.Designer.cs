namespace BabBot.Forms
{
    partial class MainForm
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
            this.msMain = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControlMain = new System.Windows.Forms.TabControl();
            this.tabPageMain = new System.Windows.Forms.TabPage();
            this.btnUpdateLocation = new System.Windows.Forms.Button();
            this.lblOrientation = new System.Windows.Forms.Label();
            this.lblLocation = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.tbLocation = new System.Windows.Forms.TextBox();
            this.btnRun = new System.Windows.Forms.Button();
            this.tabPageDebug = new System.Windows.Forms.TabPage();
            this.tbLocalGUID = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tbCurMgr = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tbPlayerBaseOffset = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbClientConnectionOffset = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbClientConnectionPointer = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnFindTLS = new System.Windows.Forms.Button();
            this.tbTLS = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.msMain.SuspendLayout();
            this.tabControlMain.SuspendLayout();
            this.tabPageMain.SuspendLayout();
            this.tabPageDebug.SuspendLayout();
            this.SuspendLayout();
            // 
            // msMain
            // 
            this.msMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.msMain.Location = new System.Drawing.Point(0, 0);
            this.msMain.Name = "msMain";
            this.msMain.Size = new System.Drawing.Size(521, 24);
            this.msMain.TabIndex = 0;
            this.msMain.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(40, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(111, 22);
            this.aboutToolStripMenuItem.Text = "&About..";
            // 
            // tabControlMain
            // 
            this.tabControlMain.Controls.Add(this.tabPageMain);
            this.tabControlMain.Controls.Add(this.tabPageDebug);
            this.tabControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlMain.Location = new System.Drawing.Point(0, 24);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.Size = new System.Drawing.Size(521, 291);
            this.tabControlMain.TabIndex = 1;
            // 
            // tabPageMain
            // 
            this.tabPageMain.Controls.Add(this.btnUpdateLocation);
            this.tabPageMain.Controls.Add(this.lblOrientation);
            this.tabPageMain.Controls.Add(this.lblLocation);
            this.tabPageMain.Controls.Add(this.textBox2);
            this.tabPageMain.Controls.Add(this.tbLocation);
            this.tabPageMain.Controls.Add(this.btnRun);
            this.tabPageMain.Location = new System.Drawing.Point(4, 22);
            this.tabPageMain.Name = "tabPageMain";
            this.tabPageMain.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageMain.Size = new System.Drawing.Size(513, 265);
            this.tabPageMain.TabIndex = 0;
            this.tabPageMain.Text = "Main";
            this.tabPageMain.UseVisualStyleBackColor = true;
            // 
            // btnUpdateLocation
            // 
            this.btnUpdateLocation.Location = new System.Drawing.Point(382, 34);
            this.btnUpdateLocation.Name = "btnUpdateLocation";
            this.btnUpdateLocation.Size = new System.Drawing.Size(112, 23);
            this.btnUpdateLocation.TabIndex = 5;
            this.btnUpdateLocation.Text = "Update Location";
            this.btnUpdateLocation.UseVisualStyleBackColor = true;
            this.btnUpdateLocation.Click += new System.EventHandler(this.btnUpdateLocation_Click);
            // 
            // lblOrientation
            // 
            this.lblOrientation.AutoSize = true;
            this.lblOrientation.Location = new System.Drawing.Point(8, 71);
            this.lblOrientation.Name = "lblOrientation";
            this.lblOrientation.Size = new System.Drawing.Size(61, 13);
            this.lblOrientation.TabIndex = 4;
            this.lblOrientation.Text = "Orientation:";
            // 
            // lblLocation
            // 
            this.lblLocation.AutoSize = true;
            this.lblLocation.Location = new System.Drawing.Point(8, 39);
            this.lblLocation.Name = "lblLocation";
            this.lblLocation.Size = new System.Drawing.Size(51, 13);
            this.lblLocation.TabIndex = 3;
            this.lblLocation.Text = "Location:";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(75, 68);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(301, 20);
            this.textBox2.TabIndex = 2;
            // 
            // tbLocation
            // 
            this.tbLocation.Location = new System.Drawing.Point(75, 36);
            this.tbLocation.Name = "tbLocation";
            this.tbLocation.Size = new System.Drawing.Size(301, 20);
            this.tbLocation.TabIndex = 1;
            // 
            // btnRun
            // 
            this.btnRun.Location = new System.Drawing.Point(6, 6);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(75, 23);
            this.btnRun.TabIndex = 0;
            this.btnRun.Text = "Run WoW";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // tabPageDebug
            // 
            this.tabPageDebug.Controls.Add(this.tbLocalGUID);
            this.tabPageDebug.Controls.Add(this.label6);
            this.tabPageDebug.Controls.Add(this.tbCurMgr);
            this.tabPageDebug.Controls.Add(this.label5);
            this.tabPageDebug.Controls.Add(this.tbPlayerBaseOffset);
            this.tabPageDebug.Controls.Add(this.label4);
            this.tabPageDebug.Controls.Add(this.tbClientConnectionOffset);
            this.tabPageDebug.Controls.Add(this.label3);
            this.tabPageDebug.Controls.Add(this.tbClientConnectionPointer);
            this.tabPageDebug.Controls.Add(this.label2);
            this.tabPageDebug.Controls.Add(this.btnFindTLS);
            this.tabPageDebug.Controls.Add(this.tbTLS);
            this.tabPageDebug.Controls.Add(this.label1);
            this.tabPageDebug.Location = new System.Drawing.Point(4, 22);
            this.tabPageDebug.Name = "tabPageDebug";
            this.tabPageDebug.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageDebug.Size = new System.Drawing.Size(513, 265);
            this.tabPageDebug.TabIndex = 1;
            this.tabPageDebug.Text = "Debug";
            this.tabPageDebug.UseVisualStyleBackColor = true;
            // 
            // tbLocalGUID
            // 
            this.tbLocalGUID.Location = new System.Drawing.Point(139, 156);
            this.tbLocalGUID.Name = "tbLocalGUID";
            this.tbLocalGUID.Size = new System.Drawing.Size(150, 20);
            this.tbLocalGUID.TabIndex = 29;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 159);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(63, 13);
            this.label6.TabIndex = 28;
            this.label6.Text = "LocalGUID:";
            // 
            // tbCurMgr
            // 
            this.tbCurMgr.Location = new System.Drawing.Point(139, 110);
            this.tbCurMgr.Name = "tbCurMgr";
            this.tbCurMgr.Size = new System.Drawing.Size(150, 20);
            this.tbCurMgr.TabIndex = 27;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 113);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(44, 13);
            this.label5.TabIndex = 26;
            this.label5.Text = "CurMgr:";
            // 
            // tbPlayerBaseOffset
            // 
            this.tbPlayerBaseOffset.Location = new System.Drawing.Point(139, 84);
            this.tbPlayerBaseOffset.Name = "tbPlayerBaseOffset";
            this.tbPlayerBaseOffset.Size = new System.Drawing.Size(150, 20);
            this.tbPlayerBaseOffset.TabIndex = 25;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 87);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(88, 13);
            this.label4.TabIndex = 24;
            this.label4.Text = "PlayerBaseOffset";
            // 
            // tbClientConnectionOffset
            // 
            this.tbClientConnectionOffset.Location = new System.Drawing.Point(139, 58);
            this.tbClientConnectionOffset.Name = "tbClientConnectionOffset";
            this.tbClientConnectionOffset.Size = new System.Drawing.Size(150, 20);
            this.tbClientConnectionOffset.TabIndex = 23;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 61);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(118, 13);
            this.label3.TabIndex = 22;
            this.label3.Text = "ClientConnectionOffset:";
            // 
            // tbClientConnectionPointer
            // 
            this.tbClientConnectionPointer.Location = new System.Drawing.Point(139, 32);
            this.tbClientConnectionPointer.Name = "tbClientConnectionPointer";
            this.tbClientConnectionPointer.Size = new System.Drawing.Size(150, 20);
            this.tbClientConnectionPointer.TabIndex = 21;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(126, 13);
            this.label2.TabIndex = 20;
            this.label2.Text = "ClientConnectionPointer::";
            // 
            // btnFindTLS
            // 
            this.btnFindTLS.Location = new System.Drawing.Point(199, 4);
            this.btnFindTLS.Name = "btnFindTLS";
            this.btnFindTLS.Size = new System.Drawing.Size(75, 23);
            this.btnFindTLS.TabIndex = 19;
            this.btnFindTLS.Text = "Find TLS";
            this.btnFindTLS.UseVisualStyleBackColor = true;
            this.btnFindTLS.Click += new System.EventHandler(this.btnFindTLS_Click);
            // 
            // tbTLS
            // 
            this.tbTLS.Location = new System.Drawing.Point(43, 6);
            this.tbTLS.Name = "tbTLS";
            this.tbTLS.Size = new System.Drawing.Size(150, 20);
            this.tbTLS.TabIndex = 18;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 13);
            this.label1.TabIndex = 17;
            this.label1.Text = "TLS:";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(521, 315);
            this.Controls.Add(this.tabControlMain);
            this.Controls.Add(this.msMain);
            this.MainMenuStrip = this.msMain;
            this.Name = "MainForm";
            this.msMain.ResumeLayout(false);
            this.msMain.PerformLayout();
            this.tabControlMain.ResumeLayout(false);
            this.tabPageMain.ResumeLayout(false);
            this.tabPageMain.PerformLayout();
            this.tabPageDebug.ResumeLayout(false);
            this.tabPageDebug.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip msMain;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.TabControl tabControlMain;
        private System.Windows.Forms.TabPage tabPageMain;
        private System.Windows.Forms.TabPage tabPageDebug;
        private System.Windows.Forms.Label lblOrientation;
        private System.Windows.Forms.Label lblLocation;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox tbLocation;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.Button btnUpdateLocation;
        private System.Windows.Forms.TextBox tbCurMgr;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbPlayerBaseOffset;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbClientConnectionOffset;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbClientConnectionPointer;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnFindTLS;
        private System.Windows.Forms.TextBox tbTLS;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbLocalGUID;
        private System.Windows.Forms.Label label6;
    }
}

