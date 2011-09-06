namespace LUAInterface
{
    partial class FrmMain
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
            this.cmd = new CommandPrompt.CommandPrompt();
            this.SuspendLayout();
            // 
            // cmd
            // 
            this.cmd.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cmd.BackColor = System.Drawing.Color.Black;
            this.cmd.Delimiters = new char[] {
        ' '};
            this.cmd.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmd.ForeColor = System.Drawing.Color.White;
            this.cmd.Location = new System.Drawing.Point(2, 1);
            this.cmd.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cmd.MessageColor = System.Drawing.Color.White;
            this.cmd.MinimumSize = new System.Drawing.Size(0, 17);
            this.cmd.Name = "cmd";
            this.cmd.PromptColor = System.Drawing.Color.White;
            this.cmd.Size = new System.Drawing.Size(522, 255);
            this.cmd.TabIndex = 1;
            this.cmd.Command += new CommandPrompt.CommandPrompt.CommandEventHandler(this.commandPrompt1_Command);
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(525, 253);
            this.Controls.Add(this.cmd);
            this.Name = "FrmMain";
            this.Text = "LUAInterface console";
            this.ResumeLayout(false);

        }

        #endregion

        private CommandPrompt.CommandPrompt cmd;
    }
}

