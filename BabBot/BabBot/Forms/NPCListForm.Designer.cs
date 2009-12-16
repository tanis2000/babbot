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
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lbNpcList = new System.Windows.Forms.ListBox();
            this.popNpc = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteNPCToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.labelWoWVersion = new System.Windows.Forms.Label();
            this.btnImport = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.tbName = new System.Windows.Forms.TextBox();
            this.gbNpcDescription = new System.Windows.Forms.GroupBox();
            this.lvNpcList = new System.Windows.Forms.ListView();
            this.btnAdd = new System.Windows.Forms.Button();
            this.cbAvailServices = new System.Windows.Forms.ComboBox();
            this.serviceTypesNpcAvail = new System.Windows.Forms.BindingSource(this.components);
            this.botDataSet = new BabBot.Data.BotDataSet();
            this.label4 = new System.Windows.Forms.Label();
            this.lbActiveServices = new System.Windows.Forms.ListBox();
            this.popServiceActions = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteServiceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.serviceTypesAllAvail = new System.Windows.Forms.BindingSource(this.components);
            this.btnAddNPC = new System.Windows.Forms.Button();
            this.cbServiceList = new System.Windows.Forms.ComboBox();
            this.btnMoveToNearest = new System.Windows.Forms.Button();
            this.labelServices = new System.Windows.Forms.Label();
            this.popQuestActions = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteNPCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gbDebug = new System.Windows.Forms.GroupBox();
            this.cbLearnSkills = new System.Windows.Forms.CheckBox();
            this.cbUseState = new System.Windows.Forms.CheckBox();
            this.gbNpcQuests = new System.Windows.Forms.GroupBox();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.labelQuestObj = new System.Windows.Forms.Label();
            this.popNpc.SuspendLayout();
            this.gbNpcDescription.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.serviceTypesNpcAvail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.botDataSet)).BeginInit();
            this.popServiceActions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.serviceTypesAllAvail)).BeginInit();
            this.popQuestActions.SuspendLayout();
            this.gbDebug.SuspendLayout();
            this.gbNpcQuests.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnHelp
            // 
            this.btnHelp.Location = new System.Drawing.Point(12, 421);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(530, 421);
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(449, 421);
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
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(203, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 18);
            this.label1.TabIndex = 33;
            this.label1.Text = "NPC List";
            // 
            // lbNpcList
            // 
            this.lbNpcList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lbNpcList.ContextMenuStrip = this.popNpc;
            this.lbNpcList.FormattingEnabled = true;
            this.lbNpcList.Location = new System.Drawing.Point(12, 33);
            this.lbNpcList.MultiColumn = true;
            this.lbNpcList.Name = "lbNpcList";
            this.lbNpcList.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbNpcList.Size = new System.Drawing.Size(487, 147);
            this.lbNpcList.TabIndex = 34;
            this.lbNpcList.SelectedIndexChanged += new System.EventHandler(this.lbNpcList_SelectedIndexChanged);
            this.lbNpcList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lbNpcList_KeyDown);
            // 
            // popNpc
            // 
            this.popNpc.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteNPCToolStripMenuItem1});
            this.popNpc.Name = "popNpc";
            this.popNpc.Size = new System.Drawing.Size(140, 26);
            this.popNpc.VisibleChanged += new System.EventHandler(this.popNpc_VisibleChanged);
            // 
            // deleteNPCToolStripMenuItem1
            // 
            this.deleteNPCToolStripMenuItem1.Name = "deleteNPCToolStripMenuItem1";
            this.deleteNPCToolStripMenuItem1.Size = new System.Drawing.Size(139, 22);
            this.deleteNPCToolStripMenuItem1.Text = "Delete NPC";
            this.deleteNPCToolStripMenuItem1.Click += new System.EventHandler(this.deleteNPCToolStripMenuItem_Click);
            // 
            // labelWoWVersion
            // 
            this.labelWoWVersion.AutoSize = true;
            this.labelWoWVersion.Location = new System.Drawing.Point(85, 9);
            this.labelWoWVersion.Name = "labelWoWVersion";
            this.labelWoWVersion.Size = new System.Drawing.Size(0, 13);
            this.labelWoWVersion.TabIndex = 35;
            // 
            // btnImport
            // 
            this.btnImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImport.Location = new System.Drawing.Point(368, 421);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(75, 23);
            this.btnImport.TabIndex = 36;
            this.btnImport.Text = "Import";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 13);
            this.label3.TabIndex = 37;
            this.label3.Text = "Name:";
            // 
            // tbName
            // 
            this.tbName.Enabled = false;
            this.tbName.Location = new System.Drawing.Point(50, 19);
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(136, 20);
            this.tbName.TabIndex = 38;
            // 
            // gbNpcDescription
            // 
            this.gbNpcDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbNpcDescription.Controls.Add(this.gbNpcQuests);
            this.gbNpcDescription.Controls.Add(this.btnAdd);
            this.gbNpcDescription.Controls.Add(this.cbAvailServices);
            this.gbNpcDescription.Controls.Add(this.label4);
            this.gbNpcDescription.Controls.Add(this.lbActiveServices);
            this.gbNpcDescription.Controls.Add(this.tbName);
            this.gbNpcDescription.Controls.Add(this.label3);
            this.gbNpcDescription.Location = new System.Drawing.Point(12, 219);
            this.gbNpcDescription.Name = "gbNpcDescription";
            this.gbNpcDescription.Size = new System.Drawing.Size(593, 196);
            this.gbNpcDescription.TabIndex = 39;
            this.gbNpcDescription.TabStop = false;
            this.gbNpcDescription.Text = "NPC Description";
            // 
            // lvNpcList
            // 
            this.lvNpcList.Location = new System.Drawing.Point(10, 19);
            this.lvNpcList.Name = "lvNpcList";
            this.lvNpcList.Size = new System.Drawing.Size(121, 145);
            this.lvNpcList.TabIndex = 43;
            this.lvNpcList.UseCompatibleStateImageBehavior = false;
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(145, 65);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(35, 23);
            this.btnAdd.TabIndex = 42;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // cbAvailServices
            // 
            this.cbAvailServices.DataSource = this.serviceTypesNpcAvail;
            this.cbAvailServices.DisplayMember = "NAME";
            this.cbAvailServices.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbAvailServices.FormattingEnabled = true;
            this.cbAvailServices.Location = new System.Drawing.Point(6, 67);
            this.cbAvailServices.Name = "cbAvailServices";
            this.cbAvailServices.Size = new System.Drawing.Size(133, 21);
            this.cbAvailServices.TabIndex = 41;
            // 
            // serviceTypesNpcAvail
            // 
            this.serviceTypesNpcAvail.DataMember = "ServiceTypes";
            this.serviceTypesNpcAvail.DataSource = this.botDataSet;
            // 
            // botDataSet
            // 
            this.botDataSet.DataSetName = "BotDataSet";
            this.botDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 51);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(51, 13);
            this.label4.TabIndex = 40;
            this.label4.Text = "Services:";
            // 
            // lbActiveServices
            // 
            this.lbActiveServices.ContextMenuStrip = this.popServiceActions;
            this.lbActiveServices.FormattingEnabled = true;
            this.lbActiveServices.Location = new System.Drawing.Point(6, 94);
            this.lbActiveServices.Name = "lbActiveServices";
            this.lbActiveServices.Size = new System.Drawing.Size(174, 95);
            this.lbActiveServices.TabIndex = 39;
            this.lbActiveServices.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lbActiveServices_KeyDown);
            // 
            // popServiceActions
            // 
            this.popServiceActions.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteServiceToolStripMenuItem});
            this.popServiceActions.Name = "popServiceActions";
            this.popServiceActions.Size = new System.Drawing.Size(155, 26);
            this.popServiceActions.VisibleChanged += new System.EventHandler(this.popServiceActions_VisibleChanged);
            // 
            // deleteServiceToolStripMenuItem
            // 
            this.deleteServiceToolStripMenuItem.Name = "deleteServiceToolStripMenuItem";
            this.deleteServiceToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.deleteServiceToolStripMenuItem.Text = "Delete Service";
            this.deleteServiceToolStripMenuItem.Click += new System.EventHandler(this.deleteServiceToolStripMenuItem_Click);
            // 
            // serviceTypesAllAvail
            // 
            this.serviceTypesAllAvail.DataMember = "ServiceTypes";
            this.serviceTypesAllAvail.DataSource = this.botDataSet;
            // 
            // btnAddNPC
            // 
            this.btnAddNPC.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAddNPC.Location = new System.Drawing.Point(12, 190);
            this.btnAddNPC.Name = "btnAddNPC";
            this.btnAddNPC.Size = new System.Drawing.Size(99, 23);
            this.btnAddNPC.TabIndex = 64;
            this.btnAddNPC.Text = "Add Target NPC";
            this.btnAddNPC.UseVisualStyleBackColor = true;
            this.btnAddNPC.Click += new System.EventHandler(this.btnAddNPC_Click);
            // 
            // cbServiceList
            // 
            this.cbServiceList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbServiceList.DataSource = this.serviceTypesAllAvail;
            this.cbServiceList.DisplayMember = "NAME";
            this.cbServiceList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbServiceList.FormattingEnabled = true;
            this.cbServiceList.Location = new System.Drawing.Point(184, 192);
            this.cbServiceList.Name = "cbServiceList";
            this.cbServiceList.Size = new System.Drawing.Size(94, 21);
            this.cbServiceList.TabIndex = 65;
            this.cbServiceList.SelectedIndexChanged += new System.EventHandler(this.cbServiceList_SelectedIndexChanged);
            // 
            // btnMoveToNearest
            // 
            this.btnMoveToNearest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnMoveToNearest.Location = new System.Drawing.Point(284, 190);
            this.btnMoveToNearest.Name = "btnMoveToNearest";
            this.btnMoveToNearest.Size = new System.Drawing.Size(97, 23);
            this.btnMoveToNearest.TabIndex = 66;
            this.btnMoveToNearest.Text = "Move to Nearest NPC";
            this.btnMoveToNearest.UseVisualStyleBackColor = true;
            this.btnMoveToNearest.Click += new System.EventHandler(this.btnMoveToNearest_Click);
            // 
            // labelServices
            // 
            this.labelServices.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelServices.AutoSize = true;
            this.labelServices.Location = new System.Drawing.Point(135, 195);
            this.labelServices.Name = "labelServices";
            this.labelServices.Size = new System.Drawing.Size(43, 13);
            this.labelServices.TabIndex = 67;
            this.labelServices.Text = "Service";
            // 
            // popQuestActions
            // 
            this.popQuestActions.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteNPCToolStripMenuItem});
            this.popQuestActions.Name = "popQuestActions";
            this.popQuestActions.Size = new System.Drawing.Size(140, 26);
            this.popQuestActions.Text = "1";
            // 
            // deleteNPCToolStripMenuItem
            // 
            this.deleteNPCToolStripMenuItem.Name = "deleteNPCToolStripMenuItem";
            this.deleteNPCToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.deleteNPCToolStripMenuItem.Text = "Delete NPC";
            this.deleteNPCToolStripMenuItem.Click += new System.EventHandler(this.deleteNPCToolStripMenuItem_Click);
            // 
            // gbDebug
            // 
            this.gbDebug.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbDebug.Controls.Add(this.cbLearnSkills);
            this.gbDebug.Controls.Add(this.cbUseState);
            this.gbDebug.Location = new System.Drawing.Point(505, 33);
            this.gbDebug.Name = "gbDebug";
            this.gbDebug.Size = new System.Drawing.Size(100, 147);
            this.gbDebug.TabIndex = 68;
            this.gbDebug.TabStop = false;
            this.gbDebug.Text = "Debug";
            // 
            // cbLearnSkills
            // 
            this.cbLearnSkills.AutoSize = true;
            this.cbLearnSkills.Location = new System.Drawing.Point(6, 42);
            this.cbLearnSkills.Name = "cbLearnSkills";
            this.cbLearnSkills.Size = new System.Drawing.Size(77, 17);
            this.cbLearnSkills.TabIndex = 72;
            this.cbLearnSkills.Text = "LearnSkills";
            this.cbLearnSkills.UseVisualStyleBackColor = true;
            // 
            // cbUseState
            // 
            this.cbUseState.AutoSize = true;
            this.cbUseState.Location = new System.Drawing.Point(6, 19);
            this.cbUseState.Name = "cbUseState";
            this.cbUseState.Size = new System.Drawing.Size(96, 17);
            this.cbUseState.TabIndex = 71;
            this.cbUseState.Text = "Use Nav State";
            this.cbUseState.UseVisualStyleBackColor = true;
            this.cbUseState.CheckedChanged += new System.EventHandler(this.cbUseState_CheckedChanged);
            // 
            // gbNpcQuests
            // 
            this.gbNpcQuests.Controls.Add(this.labelQuestObj);
            this.gbNpcQuests.Controls.Add(this.listBox1);
            this.gbNpcQuests.Controls.Add(this.lvNpcList);
            this.gbNpcQuests.Location = new System.Drawing.Point(194, 19);
            this.gbNpcQuests.Name = "gbNpcQuests";
            this.gbNpcQuests.Size = new System.Drawing.Size(382, 170);
            this.gbNpcQuests.TabIndex = 45;
            this.gbNpcQuests.TabStop = false;
            this.gbNpcQuests.Text = "Quest List";
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(137, 32);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(120, 43);
            this.listBox1.TabIndex = 44;
            // 
            // labelQuestObj
            // 
            this.labelQuestObj.AutoSize = true;
            this.labelQuestObj.Location = new System.Drawing.Point(134, 16);
            this.labelQuestObj.Name = "labelQuestObj";
            this.labelQuestObj.Size = new System.Drawing.Size(57, 13);
            this.labelQuestObj.TabIndex = 45;
            this.labelQuestObj.Text = "Objectives";
            // 
            // NPCListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(617, 456);
            this.Controls.Add(this.gbNpcDescription);
            this.Controls.Add(this.btnImport);
            this.Controls.Add(this.labelWoWVersion);
            this.Controls.Add(this.lbNpcList);
            this.Controls.Add(this.gbDebug);
            this.Controls.Add(this.btnAddNPC);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnMoveToNearest);
            this.Controls.Add(this.cbServiceList);
            this.Controls.Add(this.labelServices);
            this.MinimumSize = new System.Drawing.Size(625, 490);
            this.Name = "NPCListForm";
            this.Text = "NPC List";
            this.Load += new System.EventHandler(this.NPCListForm_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.NPCListForm_FormClosing);
            this.Controls.SetChildIndex(this.labelServices, 0);
            this.Controls.SetChildIndex(this.cbServiceList, 0);
            this.Controls.SetChildIndex(this.btnMoveToNearest, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.btnAddNPC, 0);
            this.Controls.SetChildIndex(this.gbDebug, 0);
            this.Controls.SetChildIndex(this.lbNpcList, 0);
            this.Controls.SetChildIndex(this.labelWoWVersion, 0);
            this.Controls.SetChildIndex(this.btnImport, 0);
            this.Controls.SetChildIndex(this.gbNpcDescription, 0);
            this.Controls.SetChildIndex(this.btnHelp, 0);
            this.Controls.SetChildIndex(this.btnClose, 0);
            this.Controls.SetChildIndex(this.btnSave, 0);
            this.popNpc.ResumeLayout(false);
            this.gbNpcDescription.ResumeLayout(false);
            this.gbNpcDescription.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.serviceTypesNpcAvail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.botDataSet)).EndInit();
            this.popServiceActions.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.serviceTypesAllAvail)).EndInit();
            this.popQuestActions.ResumeLayout(false);
            this.gbDebug.ResumeLayout(false);
            this.gbDebug.PerformLayout();
            this.gbNpcQuests.ResumeLayout(false);
            this.gbNpcQuests.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox lbNpcList;
        private System.Windows.Forms.Label labelWoWVersion;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.GroupBox gbNpcDescription;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ListBox lbActiveServices;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.ComboBox cbAvailServices;
        private System.Windows.Forms.Button btnAddNPC;
        private System.Windows.Forms.ComboBox cbServiceList;
        private System.Windows.Forms.Button btnMoveToNearest;
        private System.Windows.Forms.Label labelServices;
        private System.Windows.Forms.ContextMenuStrip popServiceActions;
        private System.Windows.Forms.ContextMenuStrip popQuestActions;
        private System.Windows.Forms.ToolStripMenuItem deleteNPCToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip popNpc;
        private System.Windows.Forms.ToolStripMenuItem deleteNPCToolStripMenuItem1;
        private System.Windows.Forms.GroupBox gbDebug;
        private System.Windows.Forms.CheckBox cbUseState;
        private System.Windows.Forms.CheckBox cbLearnSkills;
        private System.Windows.Forms.BindingSource serviceTypesAllAvail;
        private BabBot.Data.BotDataSet botDataSet;
        private System.Windows.Forms.BindingSource serviceTypesNpcAvail;
        private System.Windows.Forms.ToolStripMenuItem deleteServiceToolStripMenuItem;
        private System.Windows.Forms.ListView lvNpcList;
        private System.Windows.Forms.GroupBox gbNpcQuests;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Label labelQuestObj;
    }
}
