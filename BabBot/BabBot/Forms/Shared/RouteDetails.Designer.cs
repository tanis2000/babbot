namespace BabBot.Forms.Shared
{
    partial class RouteDetails
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.flpMain = new System.Windows.Forms.FlowLayoutPanel();
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
            this.lblWaypointFile = new System.Windows.Forms.Label();
            this.tbDescr = new System.Windows.Forms.TextBox();
            this.cbReversible = new System.Windows.Forms.CheckBox();
            this.lblRouteDescr = new System.Windows.Forms.Label();
            this.bsGameObjectsA = new System.Windows.Forms.BindingSource(this.components);
            this.botDataSet = new BabBot.Data.BotDataSet();
            this.bsGameObjectsB = new System.Windows.Forms.BindingSource(this.components);
            this.bsQuestListA = new System.Windows.Forms.BindingSource(this.components);
            this.bsQuestListB = new System.Windows.Forms.BindingSource(this.components);
            this.fkQuestItemsA = new System.Windows.Forms.BindingSource(this.components);
            this.fkQuestItemsB = new System.Windows.Forms.BindingSource(this.components);
            this.flpMain.SuspendLayout();
            this.gbRouteDetails.SuspendLayout();
            this.flpRouteDescr.SuspendLayout();
            this.pOptA.SuspendLayout();
            this.pObjA.SuspendLayout();
            this.pSubObjA.SuspendLayout();
            this.pOptB.SuspendLayout();
            this.pObjB.SuspendLayout();
            this.pSubObjB.SuspendLayout();
            this.pRouteDescr.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bsGameObjectsA)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.botDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsGameObjectsB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsQuestListA)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsQuestListB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fkQuestItemsA)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fkQuestItemsB)).BeginInit();
            this.SuspendLayout();
            // 
            // flpMain
            // 
            this.flpMain.AutoSize = true;
            this.flpMain.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpMain.Controls.Add(this.gbRouteDetails);
            this.flpMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpMain.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpMain.Location = new System.Drawing.Point(0, 0);
            this.flpMain.Name = "flpMain";
            this.flpMain.Size = new System.Drawing.Size(200, 392);
            this.flpMain.TabIndex = 13;
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
            this.gbRouteDetails.Size = new System.Drawing.Size(200, 392);
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
            this.flpRouteDescr.Size = new System.Drawing.Size(198, 377);
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
            this.cbObjA0.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbObjA0.FormattingEnabled = true;
            this.cbObjA0.Location = new System.Drawing.Point(50, 3);
            this.cbObjA0.Name = "cbObjA0";
            this.cbObjA0.Size = new System.Drawing.Size(142, 21);
            this.cbObjA0.TabIndex = 4;
            this.cbObjA0.DataSourceChanged += new System.EventHandler(this.cbObjA0_DataSourceChanged);
            this.cbObjA0.SelectedIndexChanged += new System.EventHandler(this.OnRegisterChanges);
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
            this.cbObjA1.DisplayMember = "IDX";
            this.cbObjA1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbObjA1.FormattingEnabled = true;
            this.cbObjA1.Location = new System.Drawing.Point(50, 2);
            this.cbObjA1.Name = "cbObjA1";
            this.cbObjA1.Size = new System.Drawing.Size(142, 21);
            this.cbObjA1.TabIndex = 20;
            this.cbObjA1.ValueMember = "IDX";
            this.cbObjA1.SelectedIndexChanged += new System.EventHandler(this.OnRegisterChanges);
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
            this.cbObjB0.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbObjB0.FormattingEnabled = true;
            this.cbObjB0.Location = new System.Drawing.Point(50, 3);
            this.cbObjB0.Name = "cbObjB0";
            this.cbObjB0.Size = new System.Drawing.Size(142, 21);
            this.cbObjB0.TabIndex = 4;
            this.cbObjB0.SelectedIndexChanged += new System.EventHandler(this.OnRegisterChanges);
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
            this.cbObjB1.DisplayMember = "IDX";
            this.cbObjB1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbObjB1.FormattingEnabled = true;
            this.cbObjB1.Location = new System.Drawing.Point(50, 2);
            this.cbObjB1.Name = "cbObjB1";
            this.cbObjB1.Size = new System.Drawing.Size(142, 21);
            this.cbObjB1.TabIndex = 20;
            this.cbObjB1.ValueMember = "IDX";
            this.cbObjB1.SelectedIndexChanged += new System.EventHandler(this.OnRegisterChanges);
            // 
            // pRouteDescr
            // 
            this.pRouteDescr.Controls.Add(this.lblWaypointFile);
            this.pRouteDescr.Controls.Add(this.tbDescr);
            this.pRouteDescr.Controls.Add(this.cbReversible);
            this.pRouteDescr.Controls.Add(this.lblRouteDescr);
            this.pRouteDescr.Location = new System.Drawing.Point(3, 239);
            this.pRouteDescr.Name = "pRouteDescr";
            this.pRouteDescr.Size = new System.Drawing.Size(192, 135);
            this.pRouteDescr.TabIndex = 18;
            // 
            // lblWaypointFile
            // 
            this.lblWaypointFile.AutoSize = true;
            this.lblWaypointFile.Location = new System.Drawing.Point(3, 110);
            this.lblWaypointFile.Name = "lblWaypointFile";
            this.lblWaypointFile.Size = new System.Drawing.Size(74, 13);
            this.lblWaypointFile.TabIndex = 14;
            this.lblWaypointFile.Text = "Waypoint File:";
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
            this.tbDescr.TextChanged += new System.EventHandler(this.OnRegisterChanges);
            // 
            // cbReversible
            // 
            this.cbReversible.AutoSize = true;
            this.cbReversible.Checked = true;
            this.cbReversible.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbReversible.Location = new System.Drawing.Point(4, 79);
            this.cbReversible.Name = "cbReversible";
            this.cbReversible.Size = new System.Drawing.Size(135, 17);
            this.cbReversible.TabIndex = 12;
            this.cbReversible.Text = "Route can be reversed";
            this.cbReversible.UseVisualStyleBackColor = true;
            this.cbReversible.CheckedChanged += new System.EventHandler(this.OnRegisterChanges);
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
            // bsGameObjectsA
            // 
            this.bsGameObjectsA.DataMember = "GameObjects";
            this.bsGameObjectsA.DataSource = this.botDataSet;
            this.bsGameObjectsA.Sort = "NAME";
            this.bsGameObjectsA.CurrentChanged += new System.EventHandler(this.bsGameObjectsA_CurrentChanged);
            // 
            // botDataSet
            // 
            this.botDataSet.DataSetName = "BotDataSet";
            this.botDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // bsGameObjectsB
            // 
            this.bsGameObjectsB.DataMember = "GameObjects";
            this.bsGameObjectsB.DataSource = this.botDataSet;
            this.bsGameObjectsB.Sort = "NAME";
            // 
            // bsQuestListA
            // 
            this.bsQuestListA.DataMember = "QuestList";
            this.bsQuestListA.DataSource = this.botDataSet;
            this.bsQuestListA.Filter = "OBJ_CNT > 0";
            this.bsQuestListA.Sort = "TITLE";
            // 
            // bsQuestListB
            // 
            this.bsQuestListB.DataMember = "QuestList";
            this.bsQuestListB.DataSource = this.botDataSet;
            this.bsQuestListB.Filter = "OBJ_CNT > 0";
            this.bsQuestListB.Sort = "TITLE";
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
            // RouteDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.flpMain);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "RouteDetails";
            this.Size = new System.Drawing.Size(200, 392);
            this.flpMain.ResumeLayout(false);
            this.flpMain.PerformLayout();
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
            ((System.ComponentModel.ISupportInitialize)(this.bsGameObjectsA)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.botDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsGameObjectsB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsQuestListA)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsQuestListB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fkQuestItemsA)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fkQuestItemsB)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flpMain;
        private System.Windows.Forms.FlowLayoutPanel flpRouteDescr;
        private System.Windows.Forms.Panel pOptA;
        private System.Windows.Forms.Label lblEndA;
        private System.Windows.Forms.Label lblZoneA;
        private System.Windows.Forms.Label lblTypeA;
        private System.Windows.Forms.Panel pObjA;
        private System.Windows.Forms.Label lblObjA0;
        private System.Windows.Forms.Panel pSubObjA;
        private System.Windows.Forms.Label lblObjA1;
        private System.Windows.Forms.Panel pOptB;
        private System.Windows.Forms.Label lblEndB;
        private System.Windows.Forms.Label lblZoneB;
        private System.Windows.Forms.Label lblTypeB;
        private System.Windows.Forms.Panel pObjB;
        private System.Windows.Forms.Label lblObjB0;
        private System.Windows.Forms.Panel pSubObjB;
        private System.Windows.Forms.Label lblObjB1;
        private System.Windows.Forms.Panel pRouteDescr;
        private System.Windows.Forms.Label lblRouteDescr;
        public System.Windows.Forms.BindingSource bsGameObjectsA;
        private BabBot.Data.BotDataSet botDataSet;
        public System.Windows.Forms.BindingSource bsGameObjectsB;
        public System.Windows.Forms.BindingSource bsQuestListA;
        public System.Windows.Forms.BindingSource bsQuestListB;
        public System.Windows.Forms.BindingSource fkQuestItemsA;
        public System.Windows.Forms.BindingSource fkQuestItemsB;
        public System.Windows.Forms.TextBox tbZoneA;
        public System.Windows.Forms.TextBox tbZoneB;
        public System.Windows.Forms.GroupBox gbRouteDetails;
        public System.Windows.Forms.ComboBox cbTypeA;
        public System.Windows.Forms.ComboBox cbObjA0;
        public System.Windows.Forms.ComboBox cbObjA1;
        public System.Windows.Forms.ComboBox cbTypeB;
        public System.Windows.Forms.ComboBox cbObjB0;
        public System.Windows.Forms.ComboBox cbObjB1;
        public System.Windows.Forms.TextBox tbDescr;
        public System.Windows.Forms.CheckBox cbReversible;
        public System.Windows.Forms.Label lblWaypointFile;
    }
}
