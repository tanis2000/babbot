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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.msMain = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControlMain = new System.Windows.Forms.TabControl();
            this.tabPageMain = new System.Windows.Forms.TabPage();
            this.btnAttachToWow = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.tbProfileDescription = new System.Windows.Forms.TextBox();
            this.tbProfileName = new System.Windows.Forms.TextBox();
            this.btnSaveProfile = new System.Windows.Forms.Button();
            this.btnLoadProfile = new System.Windows.Forms.Button();
            this.tbProfile = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.btnUpdateLocation = new System.Windows.Forms.Button();
            this.lblOrientation = new System.Windows.Forms.Label();
            this.lblLocation = new System.Windows.Forms.Label();
            this.tbOrientation = new System.Windows.Forms.TextBox();
            this.tbLocation = new System.Windows.Forms.TextBox();
            this.btnRun = new System.Windows.Forms.Button();
            this.tabPageDebug = new System.Windows.Forms.TabPage();
            this.tbWndHandle = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
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
            this.tabPageWayPoints = new System.Windows.Forms.TabPage();
            this.cbAutoAddWaypoints = new System.Windows.Forms.CheckBox();
            this.btnAddWayPoint = new System.Windows.Forms.Button();
            this.comboWayPointTypes = new System.Windows.Forms.ComboBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.btnCopyFromVendor = new System.Windows.Forms.Button();
            this.tbCountRepair = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.tbCountVendor = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tbCountGhost = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.gbNormalWaypoints = new System.Windows.Forms.GroupBox();
            this.tbCountNormal = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.tabPagePlayer = new System.Windows.Forms.TabPage();
            this.tbPlayerMaxMp = new System.Windows.Forms.TextBox();
            this.tbPlayerMaxHp = new System.Windows.Forms.TextBox();
            this.tbPlayerXp = new System.Windows.Forms.TextBox();
            this.tbPlayerMp = new System.Windows.Forms.TextBox();
            this.tbPlayerHp = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.btnMovementTest = new System.Windows.Forms.Button();
            this.msMain.SuspendLayout();
            this.tabControlMain.SuspendLayout();
            this.tabPageMain.SuspendLayout();
            this.tabPageDebug.SuspendLayout();
            this.tabPageWayPoints.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.gbNormalWaypoints.SuspendLayout();
            this.tabPagePlayer.SuspendLayout();
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
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(92, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
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
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // tabControlMain
            // 
            this.tabControlMain.Controls.Add(this.tabPageMain);
            this.tabControlMain.Controls.Add(this.tabPageDebug);
            this.tabControlMain.Controls.Add(this.tabPageWayPoints);
            this.tabControlMain.Controls.Add(this.tabPagePlayer);
            this.tabControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlMain.Location = new System.Drawing.Point(0, 24);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.Size = new System.Drawing.Size(521, 291);
            this.tabControlMain.TabIndex = 1;
            // 
            // tabPageMain
            // 
            this.tabPageMain.Controls.Add(this.btnAttachToWow);
            this.tabPageMain.Controls.Add(this.label9);
            this.tabPageMain.Controls.Add(this.label8);
            this.tabPageMain.Controls.Add(this.tbProfileDescription);
            this.tabPageMain.Controls.Add(this.tbProfileName);
            this.tabPageMain.Controls.Add(this.btnSaveProfile);
            this.tabPageMain.Controls.Add(this.btnLoadProfile);
            this.tabPageMain.Controls.Add(this.tbProfile);
            this.tabPageMain.Controls.Add(this.label7);
            this.tabPageMain.Controls.Add(this.btnUpdateLocation);
            this.tabPageMain.Controls.Add(this.lblOrientation);
            this.tabPageMain.Controls.Add(this.lblLocation);
            this.tabPageMain.Controls.Add(this.tbOrientation);
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
            // btnAttachToWow
            // 
            this.btnAttachToWow.Location = new System.Drawing.Point(87, 6);
            this.btnAttachToWow.Name = "btnAttachToWow";
            this.btnAttachToWow.Size = new System.Drawing.Size(95, 23);
            this.btnAttachToWow.TabIndex = 15;
            this.btnAttachToWow.Text = "Attach to WoW";
            this.btnAttachToWow.UseVisualStyleBackColor = true;
            this.btnAttachToWow.Click += new System.EventHandler(this.btnAttachToWow_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(11, 96);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(35, 13);
            this.label9.TabIndex = 14;
            this.label9.Text = "Desc:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(8, 68);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(38, 13);
            this.label8.TabIndex = 13;
            this.label8.Text = "Name:";
            // 
            // tbProfileDescription
            // 
            this.tbProfileDescription.Location = new System.Drawing.Point(48, 93);
            this.tbProfileDescription.Multiline = true;
            this.tbProfileDescription.Name = "tbProfileDescription";
            this.tbProfileDescription.Size = new System.Drawing.Size(234, 83);
            this.tbProfileDescription.TabIndex = 12;
            // 
            // tbProfileName
            // 
            this.tbProfileName.Location = new System.Drawing.Point(48, 66);
            this.tbProfileName.Name = "tbProfileName";
            this.tbProfileName.Size = new System.Drawing.Size(234, 20);
            this.tbProfileName.TabIndex = 11;
            // 
            // btnSaveProfile
            // 
            this.btnSaveProfile.Location = new System.Drawing.Point(369, 34);
            this.btnSaveProfile.Name = "btnSaveProfile";
            this.btnSaveProfile.Size = new System.Drawing.Size(75, 23);
            this.btnSaveProfile.TabIndex = 10;
            this.btnSaveProfile.Text = "Save";
            this.btnSaveProfile.UseVisualStyleBackColor = true;
            // 
            // btnLoadProfile
            // 
            this.btnLoadProfile.Location = new System.Drawing.Point(288, 34);
            this.btnLoadProfile.Name = "btnLoadProfile";
            this.btnLoadProfile.Size = new System.Drawing.Size(75, 23);
            this.btnLoadProfile.TabIndex = 9;
            this.btnLoadProfile.Text = "Load";
            this.btnLoadProfile.UseVisualStyleBackColor = true;
            this.btnLoadProfile.Click += new System.EventHandler(this.btnLoadProfile_Click);
            // 
            // tbProfile
            // 
            this.tbProfile.Location = new System.Drawing.Point(48, 36);
            this.tbProfile.Name = "tbProfile";
            this.tbProfile.Size = new System.Drawing.Size(234, 20);
            this.tbProfile.TabIndex = 7;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(8, 39);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(39, 13);
            this.label7.TabIndex = 6;
            this.label7.Text = "Profile:";
            // 
            // btnUpdateLocation
            // 
            this.btnUpdateLocation.Location = new System.Drawing.Point(381, 197);
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
            this.lblOrientation.Location = new System.Drawing.Point(7, 234);
            this.lblOrientation.Name = "lblOrientation";
            this.lblOrientation.Size = new System.Drawing.Size(61, 13);
            this.lblOrientation.TabIndex = 4;
            this.lblOrientation.Text = "Orientation:";
            // 
            // lblLocation
            // 
            this.lblLocation.AutoSize = true;
            this.lblLocation.Location = new System.Drawing.Point(7, 202);
            this.lblLocation.Name = "lblLocation";
            this.lblLocation.Size = new System.Drawing.Size(51, 13);
            this.lblLocation.TabIndex = 3;
            this.lblLocation.Text = "Location:";
            // 
            // tbOrientation
            // 
            this.tbOrientation.Location = new System.Drawing.Point(74, 231);
            this.tbOrientation.Name = "tbOrientation";
            this.tbOrientation.Size = new System.Drawing.Size(301, 20);
            this.tbOrientation.TabIndex = 2;
            // 
            // tbLocation
            // 
            this.tbLocation.Location = new System.Drawing.Point(74, 199);
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
            this.tabPageDebug.Controls.Add(this.btnMovementTest);
            this.tabPageDebug.Controls.Add(this.tbWndHandle);
            this.tabPageDebug.Controls.Add(this.label19);
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
            // tbWndHandle
            // 
            this.tbWndHandle.Location = new System.Drawing.Point(401, 32);
            this.tbWndHandle.Name = "tbWndHandle";
            this.tbWndHandle.Size = new System.Drawing.Size(89, 20);
            this.tbWndHandle.TabIndex = 31;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(295, 35);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(87, 13);
            this.label19.TabIndex = 30;
            this.label19.Text = "HWND Window:";
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
            // tabPageWayPoints
            // 
            this.tabPageWayPoints.Controls.Add(this.cbAutoAddWaypoints);
            this.tabPageWayPoints.Controls.Add(this.btnAddWayPoint);
            this.tabPageWayPoints.Controls.Add(this.comboWayPointTypes);
            this.tabPageWayPoints.Controls.Add(this.groupBox4);
            this.tabPageWayPoints.Controls.Add(this.groupBox3);
            this.tabPageWayPoints.Controls.Add(this.groupBox2);
            this.tabPageWayPoints.Controls.Add(this.gbNormalWaypoints);
            this.tabPageWayPoints.Location = new System.Drawing.Point(4, 22);
            this.tabPageWayPoints.Name = "tabPageWayPoints";
            this.tabPageWayPoints.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageWayPoints.Size = new System.Drawing.Size(513, 265);
            this.tabPageWayPoints.TabIndex = 2;
            this.tabPageWayPoints.Text = "WayPoints";
            this.tabPageWayPoints.UseVisualStyleBackColor = true;
            // 
            // cbAutoAddWaypoints
            // 
            this.cbAutoAddWaypoints.AutoSize = true;
            this.cbAutoAddWaypoints.Location = new System.Drawing.Point(320, 220);
            this.cbAutoAddWaypoints.Name = "cbAutoAddWaypoints";
            this.cbAutoAddWaypoints.Size = new System.Drawing.Size(119, 17);
            this.cbAutoAddWaypoints.TabIndex = 6;
            this.cbAutoAddWaypoints.Text = "Auto add waypoints";
            this.cbAutoAddWaypoints.UseVisualStyleBackColor = true;
            // 
            // btnAddWayPoint
            // 
            this.btnAddWayPoint.Location = new System.Drawing.Point(209, 216);
            this.btnAddWayPoint.Name = "btnAddWayPoint";
            this.btnAddWayPoint.Size = new System.Drawing.Size(94, 23);
            this.btnAddWayPoint.TabIndex = 5;
            this.btnAddWayPoint.Text = "Add WayPoint";
            this.btnAddWayPoint.UseVisualStyleBackColor = true;
            // 
            // comboWayPointTypes
            // 
            this.comboWayPointTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboWayPointTypes.FormattingEnabled = true;
            this.comboWayPointTypes.Items.AddRange(new object[] {
            "Normal",
            "Ghost",
            "Vendor",
            "Repair"});
            this.comboWayPointTypes.Location = new System.Drawing.Point(8, 218);
            this.comboWayPointTypes.Name = "comboWayPointTypes";
            this.comboWayPointTypes.Size = new System.Drawing.Size(195, 21);
            this.comboWayPointTypes.TabIndex = 4;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.btnCopyFromVendor);
            this.groupBox4.Controls.Add(this.tbCountRepair);
            this.groupBox4.Controls.Add(this.label13);
            this.groupBox4.Location = new System.Drawing.Point(209, 112);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(200, 100);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Repair";
            // 
            // btnCopyFromVendor
            // 
            this.btnCopyFromVendor.Location = new System.Drawing.Point(9, 44);
            this.btnCopyFromVendor.Name = "btnCopyFromVendor";
            this.btnCopyFromVendor.Size = new System.Drawing.Size(110, 23);
            this.btnCopyFromVendor.TabIndex = 5;
            this.btnCopyFromVendor.Text = "Copy from Vendor";
            this.btnCopyFromVendor.UseVisualStyleBackColor = true;
            // 
            // tbCountRepair
            // 
            this.tbCountRepair.Enabled = false;
            this.tbCountRepair.Location = new System.Drawing.Point(50, 18);
            this.tbCountRepair.Name = "tbCountRepair";
            this.tbCountRepair.Size = new System.Drawing.Size(100, 20);
            this.tbCountRepair.TabIndex = 4;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(6, 21);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(38, 13);
            this.label13.TabIndex = 3;
            this.label13.Text = "Count:";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.tbCountVendor);
            this.groupBox3.Controls.Add(this.label12);
            this.groupBox3.Location = new System.Drawing.Point(3, 112);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(200, 100);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Vendor";
            // 
            // tbCountVendor
            // 
            this.tbCountVendor.Enabled = false;
            this.tbCountVendor.Location = new System.Drawing.Point(51, 18);
            this.tbCountVendor.Name = "tbCountVendor";
            this.tbCountVendor.Size = new System.Drawing.Size(100, 20);
            this.tbCountVendor.TabIndex = 3;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(7, 21);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(38, 13);
            this.label12.TabIndex = 2;
            this.label12.Text = "Count:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tbCountGhost);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Location = new System.Drawing.Point(209, 6);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(200, 100);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Ghost";
            // 
            // tbCountGhost
            // 
            this.tbCountGhost.Enabled = false;
            this.tbCountGhost.Location = new System.Drawing.Point(50, 17);
            this.tbCountGhost.Name = "tbCountGhost";
            this.tbCountGhost.Size = new System.Drawing.Size(100, 20);
            this.tbCountGhost.TabIndex = 2;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(6, 20);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(38, 13);
            this.label11.TabIndex = 1;
            this.label11.Text = "Count:";
            // 
            // gbNormalWaypoints
            // 
            this.gbNormalWaypoints.Controls.Add(this.tbCountNormal);
            this.gbNormalWaypoints.Controls.Add(this.label10);
            this.gbNormalWaypoints.Location = new System.Drawing.Point(3, 6);
            this.gbNormalWaypoints.Name = "gbNormalWaypoints";
            this.gbNormalWaypoints.Size = new System.Drawing.Size(200, 100);
            this.gbNormalWaypoints.TabIndex = 0;
            this.gbNormalWaypoints.TabStop = false;
            this.gbNormalWaypoints.Text = "Normal";
            // 
            // tbCountNormal
            // 
            this.tbCountNormal.Enabled = false;
            this.tbCountNormal.Location = new System.Drawing.Point(51, 17);
            this.tbCountNormal.Name = "tbCountNormal";
            this.tbCountNormal.Size = new System.Drawing.Size(100, 20);
            this.tbCountNormal.TabIndex = 1;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(7, 20);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(38, 13);
            this.label10.TabIndex = 0;
            this.label10.Text = "Count:";
            // 
            // tabPagePlayer
            // 
            this.tabPagePlayer.Controls.Add(this.tbPlayerMaxMp);
            this.tabPagePlayer.Controls.Add(this.tbPlayerMaxHp);
            this.tabPagePlayer.Controls.Add(this.tbPlayerXp);
            this.tabPagePlayer.Controls.Add(this.tbPlayerMp);
            this.tabPagePlayer.Controls.Add(this.tbPlayerHp);
            this.tabPagePlayer.Controls.Add(this.label18);
            this.tabPagePlayer.Controls.Add(this.label17);
            this.tabPagePlayer.Controls.Add(this.label16);
            this.tabPagePlayer.Controls.Add(this.label15);
            this.tabPagePlayer.Controls.Add(this.label14);
            this.tabPagePlayer.Location = new System.Drawing.Point(4, 22);
            this.tabPagePlayer.Name = "tabPagePlayer";
            this.tabPagePlayer.Padding = new System.Windows.Forms.Padding(3);
            this.tabPagePlayer.Size = new System.Drawing.Size(513, 265);
            this.tabPagePlayer.TabIndex = 3;
            this.tabPagePlayer.Text = "Player";
            this.tabPagePlayer.UseVisualStyleBackColor = true;
            // 
            // tbPlayerMaxMp
            // 
            this.tbPlayerMaxMp.Enabled = false;
            this.tbPlayerMaxMp.Location = new System.Drawing.Point(186, 32);
            this.tbPlayerMaxMp.Name = "tbPlayerMaxMp";
            this.tbPlayerMaxMp.Size = new System.Drawing.Size(88, 20);
            this.tbPlayerMaxMp.TabIndex = 9;
            // 
            // tbPlayerMaxHp
            // 
            this.tbPlayerMaxHp.Enabled = false;
            this.tbPlayerMaxHp.Location = new System.Drawing.Point(185, 3);
            this.tbPlayerMaxHp.Name = "tbPlayerMaxHp";
            this.tbPlayerMaxHp.Size = new System.Drawing.Size(88, 20);
            this.tbPlayerMaxHp.TabIndex = 8;
            // 
            // tbPlayerXp
            // 
            this.tbPlayerXp.Enabled = false;
            this.tbPlayerXp.Location = new System.Drawing.Point(37, 62);
            this.tbPlayerXp.Name = "tbPlayerXp";
            this.tbPlayerXp.Size = new System.Drawing.Size(88, 20);
            this.tbPlayerXp.TabIndex = 7;
            // 
            // tbPlayerMp
            // 
            this.tbPlayerMp.Enabled = false;
            this.tbPlayerMp.Location = new System.Drawing.Point(37, 35);
            this.tbPlayerMp.Name = "tbPlayerMp";
            this.tbPlayerMp.Size = new System.Drawing.Size(88, 20);
            this.tbPlayerMp.TabIndex = 6;
            // 
            // tbPlayerHp
            // 
            this.tbPlayerHp.Enabled = false;
            this.tbPlayerHp.Location = new System.Drawing.Point(37, 3);
            this.tbPlayerHp.Name = "tbPlayerHp";
            this.tbPlayerHp.Size = new System.Drawing.Size(88, 20);
            this.tbPlayerHp.TabIndex = 5;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(6, 65);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(24, 13);
            this.label18.TabIndex = 4;
            this.label18.Text = "XP:";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(131, 6);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(48, 13);
            this.label17.TabIndex = 3;
            this.label17.Text = "Max HP:";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(131, 35);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(49, 13);
            this.label16.TabIndex = 2;
            this.label16.Text = "Max MP:";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(6, 35);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(26, 13);
            this.label15.TabIndex = 1;
            this.label15.Text = "MP:";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(6, 6);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(25, 13);
            this.label14.TabIndex = 0;
            this.label14.Text = "HP:";
            // 
            // btnMovementTest
            // 
            this.btnMovementTest.Enabled = false;
            this.btnMovementTest.Location = new System.Drawing.Point(298, 61);
            this.btnMovementTest.Name = "btnMovementTest";
            this.btnMovementTest.Size = new System.Drawing.Size(105, 23);
            this.btnMovementTest.TabIndex = 32;
            this.btnMovementTest.Text = "Movement Test";
            this.btnMovementTest.UseVisualStyleBackColor = true;
            this.btnMovementTest.Click += new System.EventHandler(this.btnMovementTest_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(521, 315);
            this.Controls.Add(this.tabControlMain);
            this.Controls.Add(this.msMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.msMain;
            this.Name = "MainForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.msMain.ResumeLayout(false);
            this.msMain.PerformLayout();
            this.tabControlMain.ResumeLayout(false);
            this.tabPageMain.ResumeLayout(false);
            this.tabPageMain.PerformLayout();
            this.tabPageDebug.ResumeLayout(false);
            this.tabPageDebug.PerformLayout();
            this.tabPageWayPoints.ResumeLayout(false);
            this.tabPageWayPoints.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.gbNormalWaypoints.ResumeLayout(false);
            this.gbNormalWaypoints.PerformLayout();
            this.tabPagePlayer.ResumeLayout(false);
            this.tabPagePlayer.PerformLayout();
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
        private System.Windows.Forms.TextBox tbOrientation;
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
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.TabPage tabPageWayPoints;
        private System.Windows.Forms.Button btnSaveProfile;
        private System.Windows.Forms.Button btnLoadProfile;
        private System.Windows.Forms.TextBox tbProfile;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tbProfileDescription;
        private System.Windows.Forms.TextBox tbProfileName;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.GroupBox gbNormalWaypoints;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.CheckBox cbAutoAddWaypoints;
        private System.Windows.Forms.Button btnAddWayPoint;
        private System.Windows.Forms.ComboBox comboWayPointTypes;
        private System.Windows.Forms.Button btnCopyFromVendor;
        private System.Windows.Forms.TextBox tbCountRepair;
        private System.Windows.Forms.TextBox tbCountVendor;
        private System.Windows.Forms.TextBox tbCountGhost;
        private System.Windows.Forms.TextBox tbCountNormal;
        private System.Windows.Forms.TabPage tabPagePlayer;
        private System.Windows.Forms.TextBox tbPlayerMaxMp;
        private System.Windows.Forms.TextBox tbPlayerMaxHp;
        private System.Windows.Forms.TextBox tbPlayerXp;
        private System.Windows.Forms.TextBox tbPlayerMp;
        private System.Windows.Forms.TextBox tbPlayerHp;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Button btnAttachToWow;
        private System.Windows.Forms.TextBox tbWndHandle;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Button btnMovementTest;
    }
}

