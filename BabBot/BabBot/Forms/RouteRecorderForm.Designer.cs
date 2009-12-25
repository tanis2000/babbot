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
            this.flpRouteDescr = new System.Windows.Forms.FlowLayoutPanel();
            this.pOptA = new System.Windows.Forms.Panel();
            this.lblEndA = new System.Windows.Forms.Label();
            this.tbZoneA = new System.Windows.Forms.TextBox();
            this.cbTypeA = new System.Windows.Forms.ComboBox();
            this.lblZoneA = new System.Windows.Forms.Label();
            this.lblTypeA = new System.Windows.Forms.Label();
            this.pObjA = new System.Windows.Forms.Panel();
            this.cbObjA0 = new System.Windows.Forms.ComboBox();
            this.lblObjA0 = new System.Windows.Forms.Label();
            this.pSubObjA = new System.Windows.Forms.Panel();
            this.lblObjA1 = new System.Windows.Forms.Label();
            this.cbObjA1 = new System.Windows.Forms.ComboBox();
            this.pOptB = new System.Windows.Forms.Panel();
            this.lblEndB = new System.Windows.Forms.Label();
            this.tbZoneB = new System.Windows.Forms.TextBox();
            this.cbTypeB = new System.Windows.Forms.ComboBox();
            this.lblZoneB = new System.Windows.Forms.Label();
            this.lblTypeB = new System.Windows.Forms.Label();
            this.pObjB = new System.Windows.Forms.Panel();
            this.cbObjB0 = new System.Windows.Forms.ComboBox();
            this.lblObjB0 = new System.Windows.Forms.Label();
            this.pSubObjB = new System.Windows.Forms.Panel();
            this.lblObjB1 = new System.Windows.Forms.Label();
            this.cbObjB1 = new System.Windows.Forms.ComboBox();
            this.pRouteDescr = new System.Windows.Forms.Panel();
            this.tbDescr = new System.Windows.Forms.TextBox();
            this.cbReverseable = new System.Windows.Forms.CheckBox();
            this.lblRouteDescr = new System.Windows.Forms.Label();
            this.bsGameObjectsB = new System.Windows.Forms.BindingSource(this.components);
            this.botDataSet = new BabBot.Data.BotDataSet();
            this.bsGameObjectsA = new System.Windows.Forms.BindingSource(this.components);
            this.btnControl = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.lblRecDescr = new System.Windows.Forms.Label();
            this.numRecDistance = new System.Windows.Forms.NumericUpDown();
            this.lblRecDistance = new System.Windows.Forms.Label();
            this.gbRecOptions = new System.Windows.Forms.GroupBox();
            this.bsQuestListA = new System.Windows.Forms.BindingSource(this.components);
            this.bsQuestListB = new System.Windows.Forms.BindingSource(this.components);
            this.flpMain = new System.Windows.Forms.FlowLayoutPanel();
            this.fkQuestItemsA = new System.Windows.Forms.BindingSource(this.components);
            this.fkQuestItemsB = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dgWaypoints)).BeginInit();
            this.popWaypoints.SuspendLayout();
            this.gbRouteDetails.SuspendLayout();
            this.flpRouteDescr.SuspendLayout();
            this.pOptA.SuspendLayout();
            this.pObjA.SuspendLayout();
            this.pSubObjA.SuspendLayout();
            this.pOptB.SuspendLayout();
            this.pObjB.SuspendLayout();
            this.pSubObjB.SuspendLayout();
            this.pRouteDescr.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bsGameObjectsB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.botDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsGameObjectsA)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRecDistance)).BeginInit();
            this.gbRecOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bsQuestListA)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsQuestListB)).BeginInit();
            this.flpMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fkQuestItemsA)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fkQuestItemsB)).BeginInit();
            this.SuspendLayout();
            // 
            // btnHelp
            // 
            this.btnHelp.Location = new System.Drawing.Point(12, 473);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(331, 473);
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(247, 473);
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
            this.dgWaypoints.Size = new System.Drawing.Size(185, 447);
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
            this.gbRouteDetails.AutoSize = true;
            this.gbRouteDetails.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.gbRouteDetails.Controls.Add(this.flpRouteDescr);
            this.gbRouteDetails.Location = new System.Drawing.Point(0, 0);
            this.gbRouteDetails.Margin = new System.Windows.Forms.Padding(0);
            this.gbRouteDetails.Name = "gbRouteDetails";
            this.gbRouteDetails.Padding = new System.Windows.Forms.Padding(1);
            this.gbRouteDetails.Size = new System.Drawing.Size(200, 357);
            this.gbRouteDetails.TabIndex = 4;
            this.gbRouteDetails.TabStop = false;
            this.gbRouteDetails.Text = "Route Details";
            // 
            // flpRouteDescr
            // 
            this.flpRouteDescr.AutoSize = true;
            this.flpRouteDescr.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpRouteDescr.Controls.Add(this.pOptA);
            this.flpRouteDescr.Controls.Add(this.pObjA);
            this.flpRouteDescr.Controls.Add(this.pSubObjA);
            this.flpRouteDescr.Controls.Add(this.pOptB);
            this.flpRouteDescr.Controls.Add(this.pObjB);
            this.flpRouteDescr.Controls.Add(this.pSubObjB);
            this.flpRouteDescr.Controls.Add(this.pRouteDescr);
            this.flpRouteDescr.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpRouteDescr.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpRouteDescr.Location = new System.Drawing.Point(1, 14);
            this.flpRouteDescr.Margin = new System.Windows.Forms.Padding(0);
            this.flpRouteDescr.Name = "flpRouteDescr";
            this.flpRouteDescr.Size = new System.Drawing.Size(198, 342);
            this.flpRouteDescr.TabIndex = 18;
            // 
            // pOptA
            // 
            this.pOptA.AutoSize = true;
            this.pOptA.Controls.Add(this.lblEndA);
            this.pOptA.Controls.Add(this.tbZoneA);
            this.pOptA.Controls.Add(this.cbTypeA);
            this.pOptA.Controls.Add(this.lblZoneA);
            this.pOptA.Controls.Add(this.lblTypeA);
            this.pOptA.Location = new System.Drawing.Point(0, 0);
            this.pOptA.Margin = new System.Windows.Forms.Padding(0);
            this.pOptA.Name = "pOptA";
            this.pOptA.Size = new System.Drawing.Size(195, 66);
            this.pOptA.TabIndex = 0;
            // 
            // lblEndA
            // 
            this.lblEndA.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblEndA.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEndA.Location = new System.Drawing.Point(0, 0);
            this.lblEndA.Name = "lblEndA";
            this.lblEndA.Size = new System.Drawing.Size(195, 13);
            this.lblEndA.TabIndex = 0;
            this.lblEndA.Text = "Endpoint A:";
            this.lblEndA.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // tbZoneA
            // 
            this.tbZoneA.Location = new System.Drawing.Point(50, 16);
            this.tbZoneA.Name = "tbZoneA";
            this.tbZoneA.ReadOnly = true;
            this.tbZoneA.Size = new System.Drawing.Size(142, 20);
            this.tbZoneA.TabIndex = 15;
            // 
            // cbTypeA
            // 
            this.cbTypeA.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTypeA.FormattingEnabled = true;
            this.cbTypeA.Location = new System.Drawing.Point(50, 42);
            this.cbTypeA.Name = "cbTypeA";
            this.cbTypeA.Size = new System.Drawing.Size(142, 21);
            this.cbTypeA.TabIndex = 1;
            this.cbTypeA.SelectedIndexChanged += new System.EventHandler(this.cbType_SelectedIndexChanged);
            // 
            // lblZoneA
            // 
            this.lblZoneA.AutoSize = true;
            this.lblZoneA.Location = new System.Drawing.Point(3, 19);
            this.lblZoneA.Name = "lblZoneA";
            this.lblZoneA.Size = new System.Drawing.Size(35, 13);
            this.lblZoneA.TabIndex = 14;
            this.lblZoneA.Text = "Zone:";
            // 
            // lblTypeA
            // 
            this.lblTypeA.AutoSize = true;
            this.lblTypeA.Location = new System.Drawing.Point(3, 45);
            this.lblTypeA.Name = "lblTypeA";
            this.lblTypeA.Size = new System.Drawing.Size(34, 13);
            this.lblTypeA.TabIndex = 5;
            this.lblTypeA.Text = "Type:";
            // 
            // pObjA
            // 
            this.pObjA.Controls.Add(this.cbObjA0);
            this.pObjA.Controls.Add(this.lblObjA0);
            this.pObjA.Location = new System.Drawing.Point(0, 66);
            this.pObjA.Margin = new System.Windows.Forms.Padding(0);
            this.pObjA.Name = "pObjA";
            this.pObjA.Size = new System.Drawing.Size(195, 26);
            this.pObjA.TabIndex = 1;
            this.pObjA.Visible = false;
            // 
            // cbObjA0
            // 
            this.cbObjA0.DisplayMember = "ID";
            this.cbObjA0.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbObjA0.FormattingEnabled = true;
            this.cbObjA0.Location = new System.Drawing.Point(50, 3);
            this.cbObjA0.Name = "cbObjA0";
            this.cbObjA0.Size = new System.Drawing.Size(142, 21);
            this.cbObjA0.TabIndex = 4;
            this.cbObjA0.ValueMember = "ID";
            // 
            // lblObjA0
            // 
            this.lblObjA0.AutoSize = true;
            this.lblObjA0.Location = new System.Drawing.Point(3, 6);
            this.lblObjA0.Name = "lblObjA0";
            this.lblObjA0.Size = new System.Drawing.Size(37, 13);
            this.lblObjA0.TabIndex = 6;
            this.lblObjA0.Text = "Text1:";
            // 
            // pSubObjA
            // 
            this.pSubObjA.Controls.Add(this.lblObjA1);
            this.pSubObjA.Controls.Add(this.cbObjA1);
            this.pSubObjA.Location = new System.Drawing.Point(0, 92);
            this.pSubObjA.Margin = new System.Windows.Forms.Padding(0);
            this.pSubObjA.Name = "pSubObjA";
            this.pSubObjA.Size = new System.Drawing.Size(195, 26);
            this.pSubObjA.TabIndex = 2;
            this.pSubObjA.Visible = false;
            // 
            // lblObjA1
            // 
            this.lblObjA1.AutoSize = true;
            this.lblObjA1.Location = new System.Drawing.Point(3, 5);
            this.lblObjA1.Name = "lblObjA1";
            this.lblObjA1.Size = new System.Drawing.Size(37, 13);
            this.lblObjA1.TabIndex = 21;
            this.lblObjA1.Text = "Text2:";
            // 
            // cbObjA1
            // 
            this.cbObjA1.DisplayMember = "ID";
            this.cbObjA1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbObjA1.FormattingEnabled = true;
            this.cbObjA1.Location = new System.Drawing.Point(50, 2);
            this.cbObjA1.Name = "cbObjA1";
            this.cbObjA1.Size = new System.Drawing.Size(142, 21);
            this.cbObjA1.TabIndex = 20;
            this.cbObjA1.ValueMember = "ID";
            // 
            // pOptB
            // 
            this.pOptB.AutoSize = true;
            this.pOptB.Controls.Add(this.lblEndB);
            this.pOptB.Controls.Add(this.tbZoneB);
            this.pOptB.Controls.Add(this.cbTypeB);
            this.pOptB.Controls.Add(this.lblZoneB);
            this.pOptB.Controls.Add(this.lblTypeB);
            this.pOptB.Location = new System.Drawing.Point(0, 118);
            this.pOptB.Margin = new System.Windows.Forms.Padding(0);
            this.pOptB.Name = "pOptB";
            this.pOptB.Size = new System.Drawing.Size(195, 66);
            this.pOptB.TabIndex = 16;
            // 
            // lblEndB
            // 
            this.lblEndB.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblEndB.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEndB.Location = new System.Drawing.Point(0, 0);
            this.lblEndB.Name = "lblEndB";
            this.lblEndB.Size = new System.Drawing.Size(195, 13);
            this.lblEndB.TabIndex = 0;
            this.lblEndB.Text = "Endpoint B:";
            this.lblEndB.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // tbZoneB
            // 
            this.tbZoneB.Location = new System.Drawing.Point(50, 16);
            this.tbZoneB.Name = "tbZoneB";
            this.tbZoneB.ReadOnly = true;
            this.tbZoneB.Size = new System.Drawing.Size(142, 20);
            this.tbZoneB.TabIndex = 15;
            // 
            // cbTypeB
            // 
            this.cbTypeB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTypeB.FormattingEnabled = true;
            this.cbTypeB.Location = new System.Drawing.Point(50, 42);
            this.cbTypeB.Name = "cbTypeB";
            this.cbTypeB.Size = new System.Drawing.Size(142, 21);
            this.cbTypeB.TabIndex = 1;
            this.cbTypeB.SelectedIndexChanged += new System.EventHandler(this.cbType_SelectedIndexChanged);
            // 
            // lblZoneB
            // 
            this.lblZoneB.AutoSize = true;
            this.lblZoneB.Location = new System.Drawing.Point(3, 19);
            this.lblZoneB.Name = "lblZoneB";
            this.lblZoneB.Size = new System.Drawing.Size(35, 13);
            this.lblZoneB.TabIndex = 14;
            this.lblZoneB.Text = "Zone:";
            // 
            // lblTypeB
            // 
            this.lblTypeB.AutoSize = true;
            this.lblTypeB.Location = new System.Drawing.Point(3, 45);
            this.lblTypeB.Name = "lblTypeB";
            this.lblTypeB.Size = new System.Drawing.Size(34, 13);
            this.lblTypeB.TabIndex = 5;
            this.lblTypeB.Text = "Type:";
            // 
            // pObjB
            // 
            this.pObjB.Controls.Add(this.cbObjB0);
            this.pObjB.Controls.Add(this.lblObjB0);
            this.pObjB.Location = new System.Drawing.Point(0, 184);
            this.pObjB.Margin = new System.Windows.Forms.Padding(0);
            this.pObjB.Name = "pObjB";
            this.pObjB.Size = new System.Drawing.Size(195, 26);
            this.pObjB.TabIndex = 7;
            this.pObjB.Visible = false;
            // 
            // cbObjB0
            // 
            this.cbObjB0.DisplayMember = "ID";
            this.cbObjB0.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbObjB0.FormattingEnabled = true;
            this.cbObjB0.Location = new System.Drawing.Point(50, 3);
            this.cbObjB0.Name = "cbObjB0";
            this.cbObjB0.Size = new System.Drawing.Size(142, 21);
            this.cbObjB0.TabIndex = 4;
            this.cbObjB0.ValueMember = "ID";
            // 
            // lblObjB0
            // 
            this.lblObjB0.AutoSize = true;
            this.lblObjB0.Location = new System.Drawing.Point(3, 6);
            this.lblObjB0.Name = "lblObjB0";
            this.lblObjB0.Size = new System.Drawing.Size(37, 13);
            this.lblObjB0.TabIndex = 6;
            this.lblObjB0.Text = "Text1:";
            // 
            // pSubObjB
            // 
            this.pSubObjB.Controls.Add(this.lblObjB1);
            this.pSubObjB.Controls.Add(this.cbObjB1);
            this.pSubObjB.Location = new System.Drawing.Point(0, 210);
            this.pSubObjB.Margin = new System.Windows.Forms.Padding(0);
            this.pSubObjB.Name = "pSubObjB";
            this.pSubObjB.Size = new System.Drawing.Size(195, 26);
            this.pSubObjB.TabIndex = 22;
            this.pSubObjB.Visible = false;
            // 
            // lblObjB1
            // 
            this.lblObjB1.AutoSize = true;
            this.lblObjB1.Location = new System.Drawing.Point(3, 5);
            this.lblObjB1.Name = "lblObjB1";
            this.lblObjB1.Size = new System.Drawing.Size(37, 13);
            this.lblObjB1.TabIndex = 21;
            this.lblObjB1.Text = "Text2:";
            // 
            // cbObjB1
            // 
            this.cbObjB1.DisplayMember = "ID";
            this.cbObjB1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbObjB1.FormattingEnabled = true;
            this.cbObjB1.Location = new System.Drawing.Point(50, 2);
            this.cbObjB1.Name = "cbObjB1";
            this.cbObjB1.Size = new System.Drawing.Size(142, 21);
            this.cbObjB1.TabIndex = 20;
            this.cbObjB1.ValueMember = "ID";
            // 
            // pRouteDescr
            // 
            this.pRouteDescr.Controls.Add(this.tbDescr);
            this.pRouteDescr.Controls.Add(this.cbReverseable);
            this.pRouteDescr.Controls.Add(this.lblRouteDescr);
            this.pRouteDescr.Location = new System.Drawing.Point(3, 239);
            this.pRouteDescr.Name = "pRouteDescr";
            this.pRouteDescr.Size = new System.Drawing.Size(192, 100);
            this.pRouteDescr.TabIndex = 18;
            // 
            // tbDescr
            // 
            this.tbDescr.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbDescr.Location = new System.Drawing.Point(4, 19);
            this.tbDescr.Multiline = true;
            this.tbDescr.Name = "tbDescr";
            this.tbDescr.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbDescr.Size = new System.Drawing.Size(185, 54);
            this.tbDescr.TabIndex = 13;
            // 
            // cbReverseable
            // 
            this.cbReverseable.AutoSize = true;
            this.cbReverseable.Location = new System.Drawing.Point(4, 79);
            this.cbReverseable.Name = "cbReverseable";
            this.cbReverseable.Size = new System.Drawing.Size(135, 17);
            this.cbReverseable.TabIndex = 12;
            this.cbReverseable.Text = "Route can be reversed";
            this.cbReverseable.UseVisualStyleBackColor = true;
            // 
            // lblRouteDescr
            // 
            this.lblRouteDescr.AutoSize = true;
            this.lblRouteDescr.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRouteDescr.Location = new System.Drawing.Point(1, 3);
            this.lblRouteDescr.Name = "lblRouteDescr";
            this.lblRouteDescr.Size = new System.Drawing.Size(113, 13);
            this.lblRouteDescr.TabIndex = 12;
            this.lblRouteDescr.Text = "Route Description:";
            // 
            // bsGameObjectsB
            // 
            this.bsGameObjectsB.DataMember = "GameObjects";
            this.bsGameObjectsB.DataSource = this.botDataSet;
            this.bsGameObjectsB.Sort = "NAME";
            // 
            // botDataSet
            // 
            this.botDataSet.DataSetName = "BotDataSet";
            this.botDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // bsGameObjectsA
            // 
            this.bsGameObjectsA.DataMember = "GameObjects";
            this.bsGameObjectsA.DataSource = this.botDataSet;
            this.bsGameObjectsA.Sort = "NAME";
            this.bsGameObjectsA.CurrentChanged += new System.EventHandler(this.bsGameObjects1_CurrentChanged);
            // 
            // btnControl
            // 
            this.btnControl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnControl.Location = new System.Drawing.Point(118, 59);
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
            this.gbRecOptions.Controls.Add(this.lblRecDescr);
            this.gbRecOptions.Controls.Add(this.numRecDistance);
            this.gbRecOptions.Controls.Add(this.lblRecDistance);
            this.gbRecOptions.Controls.Add(this.btnControl);
            this.gbRecOptions.Controls.Add(this.btnReset);
            this.gbRecOptions.Location = new System.Drawing.Point(0, 357);
            this.gbRecOptions.Margin = new System.Windows.Forms.Padding(0);
            this.gbRecOptions.Name = "gbRecOptions";
            this.gbRecOptions.Size = new System.Drawing.Size(199, 90);
            this.gbRecOptions.TabIndex = 11;
            this.gbRecOptions.TabStop = false;
            this.gbRecOptions.Text = "Recording Options";
            // 
            // bsQuestListA
            // 
            this.bsQuestListA.DataMember = "QuestList";
            this.bsQuestListA.DataSource = this.botDataSet;
            this.bsQuestListA.Filter = "ITEMS_CNT > 0";
            this.bsQuestListA.Sort = "TITLE";
            // 
            // bsQuestListB
            // 
            this.bsQuestListB.DataMember = "QuestList";
            this.bsQuestListB.DataSource = this.botDataSet;
            this.bsQuestListB.Filter = "ITEMS_CNT > 0";
            this.bsQuestListB.Sort = "TITLE";
            // 
            // flpMain
            // 
            this.flpMain.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.flpMain.AutoSize = true;
            this.flpMain.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpMain.Controls.Add(this.gbRouteDetails);
            this.flpMain.Controls.Add(this.gbRecOptions);
            this.flpMain.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpMain.Location = new System.Drawing.Point(203, 12);
            this.flpMain.Name = "flpMain";
            this.flpMain.Size = new System.Drawing.Size(200, 447);
            this.flpMain.TabIndex = 12;
            // 
            // fkQuestItemsA
            // 
            this.fkQuestItemsA.DataMember = "FK_QuestList_QuestItems";
            this.fkQuestItemsA.DataSource = this.bsQuestListA;
            this.fkQuestItemsA.Filter = "ITEM_TYPE_ID = 3";
            // 
            // fkQuestItemsB
            // 
            this.fkQuestItemsB.DataMember = "FK_QuestList_QuestItems";
            this.fkQuestItemsB.DataSource = this.bsQuestListB;
            this.fkQuestItemsB.Filter = "ITEM_TYPE_ID = 3";
            // 
            // RouteRecorderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(415, 508);
            this.Controls.Add(this.flpMain);
            this.Controls.Add(this.dgWaypoints);
            this.Name = "RouteRecorderForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Route Recorder";
            this.Controls.SetChildIndex(this.dgWaypoints, 0);
            this.Controls.SetChildIndex(this.flpMain, 0);
            this.Controls.SetChildIndex(this.btnClose, 0);
            this.Controls.SetChildIndex(this.btnHelp, 0);
            this.Controls.SetChildIndex(this.btnSave, 0);
            ((System.ComponentModel.ISupportInitialize)(this.dgWaypoints)).EndInit();
            this.popWaypoints.ResumeLayout(false);
            this.gbRouteDetails.ResumeLayout(false);
            this.gbRouteDetails.PerformLayout();
            this.flpRouteDescr.ResumeLayout(false);
            this.flpRouteDescr.PerformLayout();
            this.pOptA.ResumeLayout(false);
            this.pOptA.PerformLayout();
            this.pObjA.ResumeLayout(false);
            this.pObjA.PerformLayout();
            this.pSubObjA.ResumeLayout(false);
            this.pSubObjA.PerformLayout();
            this.pOptB.ResumeLayout(false);
            this.pOptB.PerformLayout();
            this.pObjB.ResumeLayout(false);
            this.pObjB.PerformLayout();
            this.pSubObjB.ResumeLayout(false);
            this.pSubObjB.PerformLayout();
            this.pRouteDescr.ResumeLayout(false);
            this.pRouteDescr.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bsGameObjectsB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.botDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsGameObjectsA)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRecDistance)).EndInit();
            this.gbRecOptions.ResumeLayout(false);
            this.gbRecOptions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bsQuestListA)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsQuestListB)).EndInit();
            this.flpMain.ResumeLayout(false);
            this.flpMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fkQuestItemsA)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fkQuestItemsB)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox gbRouteDetails;
        private System.Windows.Forms.Label lblEndA;
        private System.Windows.Forms.Label lblObjA0;
        private System.Windows.Forms.Label lblTypeA;
        private System.Windows.Forms.ComboBox cbObjA0;
        private System.Windows.Forms.ComboBox cbTypeA;
        private System.Windows.Forms.Button btnControl;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.TextBox tbDescr;
        private System.Windows.Forms.Label lblRouteDescr;
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
        private System.Windows.Forms.TextBox tbZoneA;
        private System.Windows.Forms.Label lblZoneA;
        private System.Windows.Forms.DataGridView dgWaypoints;
        private System.Windows.Forms.ToolStripMenuItem clearEverythingAfterToolStripMenuItem;
        private BabBot.Data.BotDataSet botDataSet;
        public System.Windows.Forms.BindingSource bsGameObjectsA;
        public System.Windows.Forms.BindingSource bsGameObjectsB;
        public System.Windows.Forms.BindingSource bsQuestListA;
        public System.Windows.Forms.BindingSource bsQuestListB;
        private System.Windows.Forms.FlowLayoutPanel flpRouteDescr;
        private System.Windows.Forms.Panel pOptA;
        private System.Windows.Forms.Panel pObjA;
        private System.Windows.Forms.Panel pSubObjA;
        private System.Windows.Forms.Panel pRouteDescr;
        private System.Windows.Forms.ComboBox cbObjA1;
        private System.Windows.Forms.Label lblObjA1;
        private System.Windows.Forms.Panel pOptB;
        private System.Windows.Forms.Label lblEndB;
        private System.Windows.Forms.TextBox tbZoneB;
        private System.Windows.Forms.ComboBox cbTypeB;
        private System.Windows.Forms.Label lblZoneB;
        private System.Windows.Forms.Label lblTypeB;
        private System.Windows.Forms.Panel pObjB;
        private System.Windows.Forms.ComboBox cbObjB0;
        private System.Windows.Forms.Label lblObjB0;
        private System.Windows.Forms.Panel pSubObjB;
        private System.Windows.Forms.Label lblObjB1;
        private System.Windows.Forms.ComboBox cbObjB1;
        private System.Windows.Forms.FlowLayoutPanel flpMain;
        public System.Windows.Forms.BindingSource fkQuestItemsA;
        public System.Windows.Forms.BindingSource fkQuestItemsB;
    }
}
