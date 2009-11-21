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
            this.btnOk = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tbWinTitle = new System.Windows.Forms.TextBox();
            this.tbLuaCallback = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
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
            this.ChkBoxLogOutput = new System.Windows.Forms.CheckBox();
            this.cbDebugMode = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(342, 279);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 7;
            this.btnOk.Text = "Save";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1.Location = new System.Drawing.Point(423, 279);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 8;
            this.button1.Text = "Cancel";
            this.button1.UseVisualStyleBackColor = true;
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
            // 
            // tbLuaCallback
            // 
            this.tbLuaCallback.Location = new System.Drawing.Point(158, 27);
            this.tbLuaCallback.Name = "tbLuaCallback";
            this.tbLuaCallback.Size = new System.Drawing.Size(86, 20);
            this.tbLuaCallback.TabIndex = 10;
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
            this.groupBox2.Text = "WoW Start Options";
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
            this.label1.Location = new System.Drawing.Point(14, 74);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 13);
            this.label1.TabIndex = 263;
            this.label1.Text = "Guest password";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 48);
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
            this.tbGuestPassword.Location = new System.Drawing.Point(103, 71);
            this.tbGuestPassword.Name = "tbGuestPassword";
            this.tbGuestPassword.Size = new System.Drawing.Size(170, 20);
            this.tbGuestPassword.TabIndex = 260;
            // 
            // tbGuestUsername
            // 
            this.tbGuestUsername.Location = new System.Drawing.Point(103, 45);
            this.tbGuestUsername.Name = "tbGuestUsername";
            this.tbGuestUsername.Size = new System.Drawing.Size(170, 20);
            this.tbGuestUsername.TabIndex = 259;
            // 
            // tbWowExePath
            // 
            this.tbWowExePath.Location = new System.Drawing.Point(103, 19);
            this.tbWowExePath.Name = "tbWowExePath";
            this.tbWowExePath.Size = new System.Drawing.Size(241, 20);
            this.tbWowExePath.TabIndex = 258;
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
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.ChkBoxLogOutput);
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
            // ChkBoxLogOutput
            // 
            this.ChkBoxLogOutput.AutoSize = true;
            this.ChkBoxLogOutput.Location = new System.Drawing.Point(8, 76);
            this.ChkBoxLogOutput.Name = "ChkBoxLogOutput";
            this.ChkBoxLogOutput.Size = new System.Drawing.Size(79, 17);
            this.ChkBoxLogOutput.TabIndex = 269;
            this.ChkBoxLogOutput.Text = "Log Output";
            this.ChkBoxLogOutput.UseVisualStyleBackColor = true;
            // 
            // cbDebugMode
            // 
            this.cbDebugMode.AutoSize = true;
            this.cbDebugMode.Location = new System.Drawing.Point(12, 284);
            this.cbDebugMode.Name = "cbDebugMode";
            this.cbDebugMode.Size = new System.Drawing.Size(88, 17);
            this.cbDebugMode.TabIndex = 12;
            this.cbDebugMode.Text = "Debug Mode";
            this.cbDebugMode.UseVisualStyleBackColor = true;
            // 
            // AppOptionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(510, 314);
            this.Controls.Add(this.cbDebugMode);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnOk);
            this.MinimumSize = new System.Drawing.Size(253, 188);
            this.Name = "AppOptionsForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Application Options";
            this.Load += new System.EventHandler(this.AppOptionsForm_Load);
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

        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button button1;
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
        private System.Windows.Forms.CheckBox ChkBoxLogOutput;
        private System.Windows.Forms.CheckBox cbDebugMode;
    }
}