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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgWaypoints = new System.Windows.Forms.DataGridView();
            this.X = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Y = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Z = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.popWaypoints = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.goToToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearEverythingAfterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gbRouteDetails = new System.Windows.Forms.GroupBox();
            this.tbZoneB = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.tbZoneA = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cbObjB = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cbReverseable = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.cbObjA = new System.Windows.Forms.ComboBox();
            this.cbTypeA = new System.Windows.Forms.ComboBox();
            this.cbTypeB = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnControl = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.lblRecDescr = new System.Windows.Forms.Label();
            this.numRecDistance = new System.Windows.Forms.NumericUpDown();
            this.lblRecDistance = new System.Windows.Forms.Label();
            this.gbRecOptions = new System.Windows.Forms.GroupBox();
            this.botDataSet = new BabBot.Data.BotDataSet();
            this.bsGameObjects1 = new System.Windows.Forms.BindingSource(this.components);
            this.bsGameObjects2 = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dgWaypoints)).BeginInit();
            this.popWaypoints.SuspendLayout();
            this.gbRouteDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numRecDistance)).BeginInit();
            this.gbRecOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.botDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsGameObjects1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsGameObjects2)).BeginInit();
            this.SuspendLayout();
            // 
            // btnHelp
            // 
            this.btnHelp.Location = new System.Drawing.Point(12, 478);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(310, 478);
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(229, 478);
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
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.DarkKhaki;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgWaypoints.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgWaypoints.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgWaypoints.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.X,
            this.Y,
            this.Z});
            this.dgWaypoints.ContextMenuStrip = this.popWaypoints;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.ScrollBar;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgWaypoints.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgWaypoints.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgWaypoints.EnableHeadersVisualStyles = false;
            this.dgWaypoints.Location = new System.Drawing.Point(12, 12);
            this.dgWaypoints.Name = "dgWaypoints";
            this.dgWaypoints.ReadOnly = true;
            this.dgWaypoints.RowHeadersVisible = false;
            this.dgWaypoints.RowTemplate.Height = 16;
            this.dgWaypoints.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgWaypoints.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgWaypoints.Size = new System.Drawing.Size(184, 451);
            this.dgWaypoints.TabIndex = 3;
            this.dgWaypoints.DoubleClick += new System.EventHandler(this.dgWaypoints_DoubleClick);
            this.dgWaypoints.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dgWaypoints_RowsAdded);
            this.dgWaypoints.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.dgWaypoints_RowsRemoved);
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
            // gbRouteDetails
            // 
            this.gbRouteDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.gbRouteDetails.Controls.Add(this.tbZoneB);
            this.gbRouteDetails.Controls.Add(this.label11);
            this.gbRouteDetails.Controls.Add(this.tbZoneA);
            this.gbRouteDetails.Controls.Add(this.label10);
            this.gbRouteDetails.Controls.Add(this.textBox1);
            this.gbRouteDetails.Controls.Add(this.label1);
            this.gbRouteDetails.Controls.Add(this.label7);
            this.gbRouteDetails.Controls.Add(this.label3);
            this.gbRouteDetails.Controls.Add(this.cbObjB);
            this.gbRouteDetails.Controls.Add(this.label5);
            this.gbRouteDetails.Controls.Add(this.cbReverseable);
            this.gbRouteDetails.Controls.Add(this.label4);
            this.gbRouteDetails.Controls.Add(this.label6);
            this.gbRouteDetails.Controls.Add(this.cbObjA);
            this.gbRouteDetails.Controls.Add(this.cbTypeA);
            this.gbRouteDetails.Controls.Add(this.cbTypeB);
            this.gbRouteDetails.Controls.Add(this.label2);
            this.gbRouteDetails.Location = new System.Drawing.Point(202, 12);
            this.gbRouteDetails.Name = "gbRouteDetails";
            this.gbRouteDetails.Size = new System.Drawing.Size(185, 356);
            this.gbRouteDetails.TabIndex = 4;
            this.gbRouteDetails.TabStop = false;
            this.gbRouteDetails.Text = "Route Details";
            // 
            // tbZoneB
            // 
            this.tbZoneB.Location = new System.Drawing.Point(58, 142);
            this.tbZoneB.Name = "tbZoneB";
            this.tbZoneB.ReadOnly = true;
            this.tbZoneB.Size = new System.Drawing.Size(121, 20);
            this.tbZoneB.TabIndex = 17;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(6, 145);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(35, 13);
            this.label11.TabIndex = 16;
            this.label11.Text = "Zone:";
            // 
            // tbZoneA
            // 
            this.tbZoneA.Location = new System.Drawing.Point(58, 35);
            this.tbZoneA.Name = "tbZoneA";
            this.tbZoneA.ReadOnly = true;
            this.tbZoneA.Size = new System.Drawing.Size(121, 20);
            this.tbZoneA.TabIndex = 15;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 38);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(35, 13);
            this.label10.TabIndex = 14;
            this.label10.Text = "Zone:";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(9, 247);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox1.Size = new System.Drawing.Size(170, 80);
            this.textBox1.TabIndex = 13;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(6, 231);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(113, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "Route Description:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(44, 121);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(73, 13);
            this.label7.TabIndex = 11;
            this.label7.Text = "Endpoint B:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 198);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Object:";
            // 
            // cbObjB
            // 
            this.cbObjB.DataSource = this.bsGameObjects2;
            this.cbObjB.DisplayMember = "NAME";
            this.cbObjB.FormattingEnabled = true;
            this.cbObjB.Location = new System.Drawing.Point(58, 195);
            this.cbObjB.Name = "cbObjB";
            this.cbObjB.Size = new System.Drawing.Size(121, 21);
            this.cbObjB.TabIndex = 8;
            this.cbObjB.ValueMember = "ID";
            this.cbObjB.Visible = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 91);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Object:";
            // 
            // cbReverseable
            // 
            this.cbReverseable.AutoSize = true;
            this.cbReverseable.Location = new System.Drawing.Point(9, 333);
            this.cbReverseable.Name = "cbReverseable";
            this.cbReverseable.Size = new System.Drawing.Size(135, 17);
            this.cbReverseable.TabIndex = 12;
            this.cbReverseable.Text = "Route can be reversed";
            this.cbReverseable.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 64);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(34, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Type:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 171);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(34, 13);
            this.label6.TabIndex = 9;
            this.label6.Text = "Type:";
            // 
            // cbObjA
            // 
            this.cbObjA.DataSource = this.bsGameObjects1;
            this.cbObjA.DisplayMember = "NAME";
            this.cbObjA.FormattingEnabled = true;
            this.cbObjA.Location = new System.Drawing.Point(58, 88);
            this.cbObjA.Name = "cbObjA";
            this.cbObjA.Size = new System.Drawing.Size(121, 21);
            this.cbObjA.TabIndex = 4;
            this.cbObjA.ValueMember = "ID";
            this.cbObjA.Visible = false;
            // 
            // cbTypeA
            // 
            this.cbTypeA.FormattingEnabled = true;
            this.cbTypeA.Location = new System.Drawing.Point(58, 61);
            this.cbTypeA.Name = "cbTypeA";
            this.cbTypeA.Size = new System.Drawing.Size(121, 21);
            this.cbTypeA.TabIndex = 1;
            this.cbTypeA.SelectedIndexChanged += new System.EventHandler(this.cbType_SelectedIndexChanged);
            // 
            // cbTypeB
            // 
            this.cbTypeB.FormattingEnabled = true;
            this.cbTypeB.Location = new System.Drawing.Point(58, 168);
            this.cbTypeB.Name = "cbTypeB";
            this.cbTypeB.Size = new System.Drawing.Size(121, 21);
            this.cbTypeB.TabIndex = 7;
            this.cbTypeB.SelectedIndexChanged += new System.EventHandler(this.cbType_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(44, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Endpoint A:";
            // 
            // btnControl
            // 
            this.btnControl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnControl.Location = new System.Drawing.Point(104, 55);
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
            this.btnReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReset.Location = new System.Drawing.Point(23, 55);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(75, 23);
            this.btnReset.TabIndex = 7;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // lblRecDescr
            // 
            this.lblRecDescr.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblRecDescr.AutoSize = true;
            this.lblRecDescr.Location = new System.Drawing.Point(6, 16);
            this.lblRecDescr.Name = "lblRecDescr";
            this.lblRecDescr.Size = new System.Drawing.Size(79, 26);
            this.lblRecDescr.TabIndex = 8;
            this.lblRecDescr.Text = "Record coord.\r\non turn or each";
            // 
            // numRecDistance
            // 
            this.numRecDistance.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numRecDistance.Location = new System.Drawing.Point(91, 16);
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
            this.lblRecDistance.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblRecDistance.AutoSize = true;
            this.lblRecDistance.Location = new System.Drawing.Point(136, 18);
            this.lblRecDistance.Name = "lblRecDistance";
            this.lblRecDistance.Size = new System.Drawing.Size(32, 13);
            this.lblRecDistance.TabIndex = 10;
            this.lblRecDistance.Text = "yards";
            // 
            // gbRecOptions
            // 
            this.gbRecOptions.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.gbRecOptions.Controls.Add(this.lblRecDescr);
            this.gbRecOptions.Controls.Add(this.numRecDistance);
            this.gbRecOptions.Controls.Add(this.lblRecDistance);
            this.gbRecOptions.Controls.Add(this.btnControl);
            this.gbRecOptions.Controls.Add(this.btnReset);
            this.gbRecOptions.Location = new System.Drawing.Point(202, 374);
            this.gbRecOptions.Name = "gbRecOptions";
            this.gbRecOptions.Size = new System.Drawing.Size(185, 89);
            this.gbRecOptions.TabIndex = 11;
            this.gbRecOptions.TabStop = false;
            this.gbRecOptions.Text = "Recording Options";
            // 
            // botDataSet
            // 
            this.botDataSet.DataSetName = "BotDataSet";
            this.botDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // bsGameObjects1
            // 
            this.bsGameObjects1.DataMember = "GameObjects";
            this.bsGameObjects1.DataSource = this.botDataSet;
            this.bsGameObjects1.CurrentChanged += new System.EventHandler(this.bsGameObjects1_CurrentChanged);
            // 
            // bsGameObjects2
            // 
            this.bsGameObjects2.DataMember = "GameObjects";
            this.bsGameObjects2.DataSource = this.botDataSet;
            // 
            // RouteRecorderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(397, 513);
            this.Controls.Add(this.dgWaypoints);
            this.Controls.Add(this.gbRouteDetails);
            this.Controls.Add(this.gbRecOptions);
            this.Name = "RouteRecorderForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Route Recorder";
            this.Controls.SetChildIndex(this.gbRecOptions, 0);
            this.Controls.SetChildIndex(this.gbRouteDetails, 0);
            this.Controls.SetChildIndex(this.dgWaypoints, 0);
            this.Controls.SetChildIndex(this.btnHelp, 0);
            this.Controls.SetChildIndex(this.btnClose, 0);
            this.Controls.SetChildIndex(this.btnSave, 0);
            ((System.ComponentModel.ISupportInitialize)(this.dgWaypoints)).EndInit();
            this.popWaypoints.ResumeLayout(false);
            this.gbRouteDetails.ResumeLayout(false);
            this.gbRouteDetails.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numRecDistance)).EndInit();
            this.gbRecOptions.ResumeLayout(false);
            this.gbRecOptions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.botDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsGameObjects1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsGameObjects2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbRouteDetails;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbObjB;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cbObjA;
        private System.Windows.Forms.ComboBox cbTypeA;
        private System.Windows.Forms.ComboBox cbTypeB;
        private System.Windows.Forms.Button btnControl;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblRecDescr;
        private System.Windows.Forms.NumericUpDown numRecDistance;
        private System.Windows.Forms.Label lblRecDistance;
        private System.Windows.Forms.ContextMenuStrip popWaypoints;
        private System.Windows.Forms.ToolStripMenuItem goToToolStripMenuItem;
        private System.Windows.Forms.CheckBox cbReverseable;
        private System.Windows.Forms.GroupBox gbRecOptions;
        private System.Windows.Forms.DataGridViewTextBoxColumn X;
        private System.Windows.Forms.DataGridViewTextBoxColumn Y;
        private System.Windows.Forms.DataGridViewTextBoxColumn Z;
        private System.Windows.Forms.TextBox tbZoneB;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox tbZoneA;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.DataGridView dgWaypoints;
        private System.Windows.Forms.ToolStripMenuItem clearEverythingAfterToolStripMenuItem;
        private System.Windows.Forms.BindingSource bsGameObjects1;
        private BabBot.Data.BotDataSet botDataSet;
        private System.Windows.Forms.BindingSource bsGameObjects2;
    }
}
