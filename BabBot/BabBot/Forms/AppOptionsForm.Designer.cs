namespace BabBot.Forms
{
    partial class AppOptionsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AppOptionsForm));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tbWinTitle = new System.Windows.Forms.TextBox();
            this.tbLuaCallback = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.numIdleSleep = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.numRefresh = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.cbWoWVersion = new System.Windows.Forms.ComboBox();
            this.cbResize = new System.Windows.Forms.CheckBox();
            this.cbNoSound = new System.Windows.Forms.CheckBox();
            this.cbWindowed = new System.Windows.Forms.CheckBox();
            this.btnFindWowExePath = new System.Windows.Forms.Button();
            this.btnBrowseWowExec = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.tbGuestPassword = new System.Windows.Forms.TextBox();
            this.tbGuestUsername = new System.Windows.Forms.TextBox();
            this.tbWowExePath = new System.Windows.Forms.TextBox();
            this.btnBrowseLogPath = new System.Windows.Forms.Button();
            this.label85 = new System.Windows.Forms.Label();
            this.tbLogsPath = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.chkLogOutput = new System.Windows.Forms.CheckBox();
            this.cbDebugMode = new System.Windows.Forms.CheckBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.tbProfilesPath = new System.Windows.Forms.TextBox();
            this.btnProfilesPath = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numIdleSleep)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRefresh)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnHelp
            // 
            this.btnHelp.Location = new System.Drawing.Point(12, 363);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(423, 363);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(335, 363);
            this.btnSave.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tbWinTitle);
            this.groupBox1.Controls.Add(this.tbLuaCallback);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(269, 100);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Bot Customization";
            // 
            // tbWinTitle
            // 
            this.tbWinTitle.Location = new System.Drawing.Point(108, 53);
            this.tbWinTitle.Name = "tbWinTitle";
            this.tbWinTitle.Size = new System.Drawing.Size(136, 20);
            this.tbWinTitle.TabIndex = 11;
            this.tbWinTitle.Click += new System.EventHandler(this.RegisterChange);
            // 
            // tbLuaCallback
            // 
            this.tbLuaCallback.Location = new System.Drawing.Point(158, 27);
            this.tbLuaCallback.Name = "tbLuaCallback";
            this.tbLuaCallback.Size = new System.Drawing.Size(86, 20);
            this.tbLuaCallback.TabIndex = 10;
            this.tbLuaCallback.Click += new System.EventHandler(this.RegisterChange);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(134, 30);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(18, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "0x";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(25, 56);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Windows Title:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(25, 30);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(103, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Lua Callback Offset:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.numIdleSleep);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.numRefresh);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.cbWoWVersion);
            this.groupBox2.Controls.Add(this.cbResize);
            this.groupBox2.Controls.Add(this.cbNoSound);
            this.groupBox2.Controls.Add(this.cbWindowed);
            this.groupBox2.Controls.Add(this.btnFindWowExePath);
            this.groupBox2.Controls.Add(this.btnBrowseWowExec);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.tbGuestPassword);
            this.groupBox2.Controls.Add(this.tbGuestUsername);
            this.groupBox2.Controls.Add(this.tbWowExePath);
            this.groupBox2.Location = new System.Drawing.Point(12, 118);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(486, 148);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "WoW Options";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(318, 75);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(80, 13);
            this.label12.TabIndex = 279;
            this.label12.Text = "Idle Sleep Time";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(451, 75);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(24, 13);
            this.label11.TabIndex = 278;
            this.label11.Text = "sec";
            // 
            // numIdleSleep
            // 
            this.numIdleSleep.Location = new System.Drawing.Point(402, 73);
            this.numIdleSleep.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numIdleSleep.Name = "numIdleSleep";
            this.numIdleSleep.Size = new System.Drawing.Size(41, 20);
            this.numIdleSleep.TabIndex = 277;
            this.numIdleSleep.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numIdleSleep.ValueChanged += new System.EventHandler(this.RegisterChange);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(447, 101);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(32, 13);
            this.label10.TabIndex = 276;
            this.label10.Text = "msec";
            // 
            // numRefresh
            // 
            this.numRefresh.Increment = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numRefresh.Location = new System.Drawing.Point(394, 99);
            this.numRefresh.Maximum = new decimal(new int[] {
            60000,
            0,
            0,
            0});
            this.numRefresh.Name = "numRefresh";
            this.numRefresh.Size = new System.Drawing.Size(49, 20);
            this.numRefresh.TabIndex = 275;
            this.numRefresh.Value = new decimal(new int[] {
            250,
            0,
            0,
            0});
            this.numRefresh.ValueChanged += new System.EventHandler(this.RegisterChange);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(318, 101);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(70, 13);
            this.label9.TabIndex = 274;
            this.label9.Text = "Refresh Time";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(24, 48);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(73, 13);
            this.label7.TabIndex = 273;
            this.label7.Text = "WoW Version";
            // 
            // cbWoWVersion
            // 
            this.cbWoWVersion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbWoWVersion.FormattingEnabled = true;
            this.cbWoWVersion.Location = new System.Drawing.Point(103, 45);
            this.cbWoWVersion.Name = "cbWoWVersion";
            this.cbWoWVersion.Size = new System.Drawing.Size(121, 21);
            this.cbWoWVersion.TabIndex = 272;
            this.cbWoWVersion.SelectedIndexChanged += new System.EventHandler(this.RegisterChange);
            // 
            // cbResize
            // 
            this.cbResize.AutoSize = true;
            this.cbResize.Location = new System.Drawing.Point(192, 123);
            this.cbResize.Name = "cbResize";
            this.cbResize.Size = new System.Drawing.Size(92, 17);
            this.cbResize.TabIndex = 271;
            this.cbResize.Text = "Resize to min.";
            this.cbResize.UseVisualStyleBackColor = true;
            this.cbResize.CheckedChanged += new System.EventHandler(this.RegisterChange);
            // 
            // cbNoSound
            // 
            this.cbNoSound.AutoSize = true;
            this.cbNoSound.Location = new System.Drawing.Point(114, 123);
            this.cbNoSound.Name = "cbNoSound";
            this.cbNoSound.Size = new System.Drawing.Size(72, 17);
            this.cbNoSound.TabIndex = 270;
            this.cbNoSound.Text = "No sound";
            this.cbNoSound.UseVisualStyleBackColor = true;
            this.cbNoSound.CheckedChanged += new System.EventHandler(this.RegisterChange);
            // 
            // cbWindowed
            // 
            this.cbWindowed.AutoSize = true;
            this.cbWindowed.Location = new System.Drawing.Point(9, 123);
            this.cbWindowed.Name = "cbWindowed";
            this.cbWindowed.Size = new System.Drawing.Size(99, 17);
            this.cbWindowed.TabIndex = 269;
            this.cbWindowed.Text = "Windows mode";
            this.cbWindowed.UseVisualStyleBackColor = true;
            this.cbWindowed.CheckedChanged += new System.EventHandler(this.RegisterChange);
            // 
            // btnFindWowExePath
            // 
            this.btnFindWowExePath.Location = new System.Drawing.Point(413, 17);
            this.btnFindWowExePath.Name = "btnFindWowExePath";
            this.btnFindWowExePath.Size = new System.Drawing.Size(64, 23);
            this.btnFindWowExePath.TabIndex = 265;
            this.btnFindWowExePath.Text = "Auto-Find";
            this.btnFindWowExePath.UseVisualStyleBackColor = true;
            this.btnFindWowExePath.Click += new System.EventHandler(this.btnFindWowExePath_Click);
            // 
            // btnBrowseWowExec
            // 
            this.btnBrowseWowExec.Location = new System.Drawing.Point(350, 17);
            this.btnBrowseWowExec.Name = "btnBrowseWowExec";
            this.btnBrowseWowExec.Size = new System.Drawing.Size(57, 23);
            this.btnBrowseWowExec.TabIndex = 264;
            this.btnBrowseWowExec.Text = "Browse..";
            this.btnBrowseWowExec.UseVisualStyleBackColor = true;
            this.btnBrowseWowExec.Click += new System.EventHandler(this.btnBrowseWowExec_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 101);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 13);
            this.label1.TabIndex = 263;
            this.label1.Text = "Guest password";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 75);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 13);
            this.label2.TabIndex = 262;
            this.label2.Text = "Guest username";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 22);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(91, 13);
            this.label6.TabIndex = 261;
            this.label6.Text = "WoW Executable";
            // 
            // tbGuestPassword
            // 
            this.tbGuestPassword.Location = new System.Drawing.Point(103, 98);
            this.tbGuestPassword.Name = "tbGuestPassword";
            this.tbGuestPassword.Size = new System.Drawing.Size(170, 20);
            this.tbGuestPassword.TabIndex = 260;
            this.tbGuestPassword.TextChanged += new System.EventHandler(this.RegisterChange);
            // 
            // tbGuestUsername
            // 
            this.tbGuestUsername.Location = new System.Drawing.Point(103, 72);
            this.tbGuestUsername.Name = "tbGuestUsername";
            this.tbGuestUsername.Size = new System.Drawing.Size(170, 20);
            this.tbGuestUsername.TabIndex = 259;
            this.tbGuestUsername.TextChanged += new System.EventHandler(this.RegisterChange);
            // 
            // tbWowExePath
            // 
            this.tbWowExePath.Location = new System.Drawing.Point(103, 19);
            this.tbWowExePath.Name = "tbWowExePath";
            this.tbWowExePath.Size = new System.Drawing.Size(241, 20);
            this.tbWowExePath.TabIndex = 258;
            this.tbWowExePath.TextChanged += new System.EventHandler(this.RegisterChange);
            this.tbWowExePath.Click += new System.EventHandler(this.RegisterChange);
            // 
            // btnBrowseLogPath
            // 
            this.btnBrowseLogPath.Location = new System.Drawing.Point(122, 11);
            this.btnBrowseLogPath.Name = "btnBrowseLogPath";
            this.btnBrowseLogPath.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseLogPath.TabIndex = 268;
            this.btnBrowseLogPath.Text = "Browse..";
            this.btnBrowseLogPath.UseVisualStyleBackColor = true;
            this.btnBrowseLogPath.Click += new System.EventHandler(this.btnBrowseLogPath_Click);
            // 
            // label85
            // 
            this.label85.AutoSize = true;
            this.label85.Location = new System.Drawing.Point(5, 21);
            this.label85.Name = "label85";
            this.label85.Size = new System.Drawing.Size(55, 13);
            this.label85.TabIndex = 267;
            this.label85.Text = "Logs Path";
            // 
            // tbLogsPath
            // 
            this.tbLogsPath.Location = new System.Drawing.Point(8, 40);
            this.tbLogsPath.Name = "tbLogsPath";
            this.tbLogsPath.Size = new System.Drawing.Size(191, 20);
            this.tbLogsPath.TabIndex = 266;
            this.tbLogsPath.Click += new System.EventHandler(this.RegisterChange);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.chkLogOutput);
            this.groupBox3.Controls.Add(this.label85);
            this.groupBox3.Controls.Add(this.tbLogsPath);
            this.groupBox3.Controls.Add(this.btnBrowseLogPath);
            this.groupBox3.Location = new System.Drawing.Point(292, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(205, 99);
            this.groupBox3.TabIndex = 11;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Log Options";
            // 
            // chkLogOutput
            // 
            this.chkLogOutput.AutoSize = true;
            this.chkLogOutput.Location = new System.Drawing.Point(8, 76);
            this.chkLogOutput.Name = "chkLogOutput";
            this.chkLogOutput.Size = new System.Drawing.Size(81, 17);
            this.chkLogOutput.TabIndex = 269;
            this.chkLogOutput.Text = "Display Log";
            this.chkLogOutput.UseVisualStyleBackColor = true;
            this.chkLogOutput.CheckedChanged += new System.EventHandler(this.RegisterChange);
            // 
            // cbDebugMode
            // 
            this.cbDebugMode.AutoSize = true;
            this.cbDebugMode.Location = new System.Drawing.Point(9, 50);
            this.cbDebugMode.Name = "cbDebugMode";
            this.cbDebugMode.Size = new System.Drawing.Size(88, 17);
            this.cbDebugMode.TabIndex = 12;
            this.cbDebugMode.Text = "Debug Mode";
            this.cbDebugMode.UseVisualStyleBackColor = true;
            this.cbDebugMode.CheckedChanged += new System.EventHandler(this.RegisterChange);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label8);
            this.groupBox4.Controls.Add(this.tbProfilesPath);
            this.groupBox4.Controls.Add(this.btnProfilesPath);
            this.groupBox4.Controls.Add(this.cbDebugMode);
            this.groupBox4.Location = new System.Drawing.Point(12, 272);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(485, 73);
            this.groupBox4.TabIndex = 13;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "General";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 22);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(66, 13);
            this.label8.TabIndex = 270;
            this.label8.Text = "Profiles Path";
            // 
            // tbProfilesPath
            // 
            this.tbProfilesPath.Location = new System.Drawing.Point(78, 19);
            this.tbProfilesPath.Name = "tbProfilesPath";
            this.tbProfilesPath.Size = new System.Drawing.Size(318, 20);
            this.tbProfilesPath.TabIndex = 269;
            this.tbProfilesPath.TextChanged += new System.EventHandler(this.RegisterChange);
            // 
            // btnProfilesPath
            // 
            this.btnProfilesPath.Location = new System.Drawing.Point(402, 17);
            this.btnProfilesPath.Name = "btnProfilesPath";
            this.btnProfilesPath.Size = new System.Drawing.Size(75, 23);
            this.btnProfilesPath.TabIndex = 271;
            this.btnProfilesPath.Text = "Browse..";
            this.btnProfilesPath.UseVisualStyleBackColor = true;
            this.btnProfilesPath.Click += new System.EventHandler(this.btnProfilesPath_Click);
            // 
            // AppOptionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(510, 398);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(253, 188);
            this.Name = "AppOptionsForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Application Options";
            this.Load += new System.EventHandler(this.AppOptionsForm_Load);
            this.Controls.SetChildIndex(this.btnClose, 0);
            this.Controls.SetChildIndex(this.groupBox1, 0);
            this.Controls.SetChildIndex(this.groupBox2, 0);
            this.Controls.SetChildIndex(this.btnHelp, 0);
            this.Controls.SetChildIndex(this.groupBox3, 0);
            this.Controls.SetChildIndex(this.btnSave, 0);
            this.Controls.SetChildIndex(this.groupBox4, 0);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numIdleSleep)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRefresh)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox tbWinTitle;
        private System.Windows.Forms.TextBox tbLuaCallback;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnBrowseLogPath;
        private System.Windows.Forms.Label label85;
        private System.Windows.Forms.TextBox tbLogsPath;
        private System.Windows.Forms.Button btnFindWowExePath;
        private System.Windows.Forms.Button btnBrowseWowExec;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tbGuestPassword;
        private System.Windows.Forms.TextBox tbGuestUsername;
        private System.Windows.Forms.TextBox tbWowExePath;
        private System.Windows.Forms.CheckBox cbResize;
        private System.Windows.Forms.CheckBox cbNoSound;
        private System.Windows.Forms.CheckBox cbWindowed;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox chkLogOutput;
        private System.Windows.Forms.CheckBox cbDebugMode;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cbWoWVersion;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tbProfilesPath;
        private System.Windows.Forms.Button btnProfilesPath;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown numRefresh;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.NumericUpDown numIdleSleep;
    }
}