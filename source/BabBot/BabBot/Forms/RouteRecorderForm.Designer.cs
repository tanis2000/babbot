namespace BabBot.Forms
{
    partial class RouteRecorderForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgWaypoints = new System.Windows.Forms.DataGridView();
            this.X = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Y = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Z = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.popWaypoints = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.goToToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearEverythingAfterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnControl = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.lblRecDescr = new System.Windows.Forms.Label();
            this.numRecDistance = new System.Windows.Forms.NumericUpDown();
            this.lblRecDistance = new System.Windows.Forms.Label();
            this.gbRecOptions = new System.Windows.Forms.GroupBox();
            this.btnTest = new System.Windows.Forms.Button();
            this.flpMain = new System.Windows.Forms.FlowLayoutPanel();
            this.ctrlRouteDetails = new BabBot.Forms.Shared.RouteDetails();
            this.bwRouteNavigation = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)(this.dgWaypoints)).BeginInit();
            this.popWaypoints.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numRecDistance)).BeginInit();
            this.gbRecOptions.SuspendLayout();
            this.flpMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnHelp
            // 
            this.btnHelp.Location = new System.Drawing.Point(12, 523);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(357, 523);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(273, 523);
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // dgWaypoints
            // 
            this.dgWaypoints.AllowUserToResizeColumns = false;
            this.dgWaypoints.AllowUserToResizeRows = false;
            this.dgWaypoints.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgWaypoints.BackgroundColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.dgWaypoints.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.DarkKhaki;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgWaypoints.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgWaypoints.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgWaypoints.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.X,
            this.Y,
            this.Z});
            this.dgWaypoints.ContextMenuStrip = this.popWaypoints;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.ScrollBar;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgWaypoints.DefaultCellStyle = dataGridViewCellStyle4;
            this.dgWaypoints.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgWaypoints.EnableHeadersVisualStyles = false;
            this.dgWaypoints.Location = new System.Drawing.Point(12, 12);
            this.dgWaypoints.Name = "dgWaypoints";
            this.dgWaypoints.ReadOnly = true;
            this.dgWaypoints.RowHeadersVisible = false;
            this.dgWaypoints.RowTemplate.Height = 16;
            this.dgWaypoints.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgWaypoints.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgWaypoints.Size = new System.Drawing.Size(190, 497);
            this.dgWaypoints.TabIndex = 3;
            this.dgWaypoints.DoubleClick += new System.EventHandler(this.dgWaypoints_DoubleClick);
            this.dgWaypoints.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dgWaypoints_RowsAdded);
            this.dgWaypoints.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.dgWaypoints_RowsRemoved);
            this.dgWaypoints.SelectionChanged += new System.EventHandler(this.dgWaypoints_SelectionChanged);
            // 
            // X
            // 
            this.X.HeaderText = "X";
            this.X.Name = "X";
            this.X.ReadOnly = true;
            this.X.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.X.Width = 55;
            // 
            // Y
            // 
            this.Y.HeaderText = "Y";
            this.Y.Name = "Y";
            this.Y.ReadOnly = true;
            this.Y.Width = 55;
            // 
            // Z
            // 
            this.Z.HeaderText = "Z";
            this.Z.Name = "Z";
            this.Z.ReadOnly = true;
            this.Z.Width = 55;
            // 
            // popWaypoints
            // 
            this.popWaypoints.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.goToToolStripMenuItem,
            this.clearEverythingAfterToolStripMenuItem});
            this.popWaypoints.Name = "popWaypoints";
            this.popWaypoints.Size = new System.Drawing.Size(193, 48);
            this.popWaypoints.Opening += new System.ComponentModel.CancelEventHandler(this.popWaypoints_Opening);
            // 
            // goToToolStripMenuItem
            // 
            this.goToToolStripMenuItem.Name = "goToToolStripMenuItem";
            this.goToToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.goToToolStripMenuItem.Text = "Go To this waypoint";
            this.goToToolStripMenuItem.Click += new System.EventHandler(this.goToToolStripMenuItem_Click);
            // 
            // clearEverythingAfterToolStripMenuItem
            // 
            this.clearEverythingAfterToolStripMenuItem.Name = "clearEverythingAfterToolStripMenuItem";
            this.clearEverythingAfterToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.clearEverythingAfterToolStripMenuItem.Text = "Clear everything after";
            this.clearEverythingAfterToolStripMenuItem.Click += new System.EventHandler(this.clearEverythingAfterToolStripMenuItem_Click);
            // 
            // btnControl
            // 
            this.btnControl.Location = new System.Drawing.Point(126, 59);
            this.btnControl.Name = "btnControl";
            this.btnControl.Size = new System.Drawing.Size(75, 23);
            this.btnControl.TabIndex = 6;
            this.btnControl.Tag = "false";
            this.btnControl.Text = "Start";
            this.btnControl.UseVisualStyleBackColor = true;
            this.btnControl.Click += new System.EventHandler(this.btnControl_Click);
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(37, 59);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(75, 23);
            this.btnReset.TabIndex = 7;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // lblRecDescr
            // 
            this.lblRecDescr.AutoSize = true;
            this.lblRecDescr.Location = new System.Drawing.Point(23, 21);
            this.lblRecDescr.Name = "lblRecDescr";
            this.lblRecDescr.Size = new System.Drawing.Size(79, 26);
            this.lblRecDescr.TabIndex = 8;
            this.lblRecDescr.Text = "Record coord.\r\non turn or each";
            // 
            // numRecDistance
            // 
            this.numRecDistance.Location = new System.Drawing.Point(116, 24);
            this.numRecDistance.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.numRecDistance.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numRecDistance.Name = "numRecDistance";
            this.numRecDistance.Size = new System.Drawing.Size(39, 20);
            this.numRecDistance.TabIndex = 9;
            this.numRecDistance.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // lblRecDistance
            // 
            this.lblRecDistance.AutoSize = true;
            this.lblRecDistance.Location = new System.Drawing.Point(161, 26);
            this.lblRecDistance.Name = "lblRecDistance";
            this.lblRecDistance.Size = new System.Drawing.Size(32, 13);
            this.lblRecDistance.TabIndex = 10;
            this.lblRecDistance.Text = "yards";
            // 
            // gbRecOptions
            // 
            this.gbRecOptions.Controls.Add(this.btnTest);
            this.gbRecOptions.Controls.Add(this.lblRecDescr);
            this.gbRecOptions.Controls.Add(this.numRecDistance);
            this.gbRecOptions.Controls.Add(this.lblRecDistance);
            this.gbRecOptions.Controls.Add(this.btnControl);
            this.gbRecOptions.Controls.Add(this.btnReset);
            this.gbRecOptions.Location = new System.Drawing.Point(0, 253);
            this.gbRecOptions.Margin = new System.Windows.Forms.Padding(0);
            this.gbRecOptions.Name = "gbRecOptions";
            this.gbRecOptions.Size = new System.Drawing.Size(207, 124);
            this.gbRecOptions.TabIndex = 11;
            this.gbRecOptions.TabStop = false;
            this.gbRecOptions.Text = "Recording Options";
            // 
            // btnTest
            // 
            this.btnTest.Enabled = false;
            this.btnTest.Location = new System.Drawing.Point(37, 88);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(164, 23);
            this.btnTest.TabIndex = 11;
            this.btnTest.Text = "Test";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // flpMain
            // 
            this.flpMain.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.flpMain.AutoSize = true;
            this.flpMain.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpMain.Controls.Add(this.ctrlRouteDetails);
            this.flpMain.Controls.Add(this.gbRecOptions);
            this.flpMain.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpMain.Location = new System.Drawing.Point(222, 12);
            this.flpMain.Name = "flpMain";
            this.flpMain.Size = new System.Drawing.Size(207, 377);
            this.flpMain.TabIndex = 12;
            // 
            // ctrlRouteDetails
            // 
            this.ctrlRouteDetails.AutoSize = true;
            this.ctrlRouteDetails.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ctrlRouteDetails.Location = new System.Drawing.Point(0, 0);
            this.ctrlRouteDetails.Margin = new System.Windows.Forms.Padding(0);
            this.ctrlRouteDetails.Name = "ctrlRouteDetails";
            this.ctrlRouteDetails.Size = new System.Drawing.Size(200, 253);
            this.ctrlRouteDetails.TabIndex = 13;
            // 
            // bwRouteNavigation
            // 
            this.bwRouteNavigation.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bwRouteNavigation_DoWork);
            // 
            // RouteRecorderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(441, 558);
            this.Controls.Add(this.dgWaypoints);
            this.Controls.Add(this.flpMain);
            this.Name = "RouteRecorderForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Route Recorder";
            this.Controls.SetChildIndex(this.flpMain, 0);
            this.Controls.SetChildIndex(this.dgWaypoints, 0);
            this.Controls.SetChildIndex(this.btnClose, 0);
            this.Controls.SetChildIndex(this.btnHelp, 0);
            this.Controls.SetChildIndex(this.btnSave, 0);
            ((System.ComponentModel.ISupportInitialize)(this.dgWaypoints)).EndInit();
            this.popWaypoints.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numRecDistance)).EndInit();
            this.gbRecOptions.ResumeLayout(false);
            this.gbRecOptions.PerformLayout();
            this.flpMain.ResumeLayout(false);
            this.flpMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnControl;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Label lblRecDescr;
        private System.Windows.Forms.NumericUpDown numRecDistance;
        private System.Windows.Forms.Label lblRecDistance;
        private System.Windows.Forms.ContextMenuStrip popWaypoints;
        private System.Windows.Forms.ToolStripMenuItem goToToolStripMenuItem;
        private System.Windows.Forms.GroupBox gbRecOptions;
        private System.Windows.Forms.DataGridViewTextBoxColumn X;
        private System.Windows.Forms.DataGridViewTextBoxColumn Y;
        private System.Windows.Forms.DataGridViewTextBoxColumn Z;
        private System.Windows.Forms.DataGridView dgWaypoints;
        private System.Windows.Forms.ToolStripMenuItem clearEverythingAfterToolStripMenuItem;
        private System.Windows.Forms.FlowLayoutPanel flpMain;
        private System.Windows.Forms.Button btnTest;
        private System.ComponentModel.BackgroundWorker bwRouteNavigation;
        private BabBot.Forms.Shared.RouteDetails ctrlRouteDetails;
    }
}
