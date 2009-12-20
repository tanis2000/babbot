namespace BabBot.Forms
{
    partial class GameObjectsForm
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
            this.lbGameObjList = new System.Windows.Forms.ListBox();
            this.popGameObject = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveToObjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteGameObjectToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.bsGameObjects = new System.Windows.Forms.BindingSource(this.components);
            this.botDataSet = new BabBot.Data.BotDataSet();
            this.labelWoWVersion = new System.Windows.Forms.Label();
            this.btnImport = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.tbName = new System.Windows.Forms.TextBox();
            this.gbDescription = new System.Windows.Forms.GroupBox();
            this.btnAddService = new System.Windows.Forms.Button();
            this.lbActiveServices = new System.Windows.Forms.ListBox();
            this.popServiceActions = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteServiceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fkGameObjectsNpcServices = new System.Windows.Forms.BindingSource(this.components);
            this.cbAvailServices = new System.Windows.Forms.ComboBox();
            this.bsServiceTypesFiltered = new System.Windows.Forms.BindingSource(this.components);
            this.tbZ = new System.Windows.Forms.TextBox();
            this.tbY = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.tbX = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btnAddCoord = new System.Windows.Forms.Button();
            this.lbCoordinates = new System.Windows.Forms.ListBox();
            this.popCoordinates = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteCoordinatesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fKCoordinatesZoneCoordinates = new System.Windows.Forms.BindingSource(this.components);
            this.fKGameObjectsCoordinatesZone = new System.Windows.Forms.BindingSource(this.components);
            this.gbQuestList = new System.Windows.Forms.GroupBox();
            this.lbQuestList = new System.Windows.Forms.ListBox();
            this.popQuestActions = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.acceptQuestToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteQuestToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fkGameObjectsQuestList = new System.Windows.Forms.BindingSource(this.components);
            this.btnAddQuest = new System.Windows.Forms.Button();
            this.btnAddNPC = new System.Windows.Forms.Button();
            this.cbServiceList = new System.Windows.Forms.ComboBox();
            this.bsServiceTypesFull = new System.Windows.Forms.BindingSource(this.components);
            this.btnMoveToNearest = new System.Windows.Forms.Button();
            this.labelServices = new System.Windows.Forms.Label();
            this.gbDebug = new System.Windows.Forms.GroupBox();
            this.cbLearnSkills = new System.Windows.Forms.CheckBox();
            this.cbUseState = new System.Windows.Forms.CheckBox();
            this.gbCoordinates = new System.Windows.Forms.GroupBox();
            this.cbCoordZone = new System.Windows.Forms.ComboBox();
            this.btnAddPlayerTargetCoord = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.cbPlayerTarget = new System.Windows.Forms.ComboBox();
            this.btnAddNewObj = new System.Windows.Forms.Button();
            this.btnAddItem = new System.Windows.Forms.Button();
            this.cbItemList = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnEditObject = new System.Windows.Forms.Button();
            this.cbAllZones = new System.Windows.Forms.ComboBox();
            this.bsZoneList = new System.Windows.Forms.BindingSource(this.components);
            this.gbAddCoord = new System.Windows.Forms.GroupBox();
            this.gbAutoAdd = new System.Windows.Forms.GroupBox();
            this.popGameObject.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bsGameObjects)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.botDataSet)).BeginInit();
            this.gbDescription.SuspendLayout();
            this.popServiceActions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fkGameObjectsNpcServices)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsServiceTypesFiltered)).BeginInit();
            this.popCoordinates.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fKCoordinatesZoneCoordinates)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fKGameObjectsCoordinatesZone)).BeginInit();
            this.gbQuestList.SuspendLayout();
            this.popQuestActions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fkGameObjectsQuestList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsServiceTypesFull)).BeginInit();
            this.gbDebug.SuspendLayout();
            this.gbCoordinates.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bsZoneList)).BeginInit();
            this.gbAddCoord.SuspendLayout();
            this.gbAutoAdd.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnHelp
            // 
            this.btnHelp.Location = new System.Drawing.Point(12, 458);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(631, 458);
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(550, 458);
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
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
            this.label1.Size = new System.Drawing.Size(130, 18);
            this.label1.TabIndex = 33;
            this.label1.Text = "Game Object List";
            // 
            // lbGameObjList
            // 
            this.lbGameObjList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lbGameObjList.ContextMenuStrip = this.popGameObject;
            this.lbGameObjList.DataSource = this.bsGameObjects;
            this.lbGameObjList.DisplayMember = "NAME";
            this.lbGameObjList.FormattingEnabled = true;
            this.lbGameObjList.Location = new System.Drawing.Point(12, 37);
            this.lbGameObjList.MultiColumn = true;
            this.lbGameObjList.Name = "lbGameObjList";
            this.lbGameObjList.Size = new System.Drawing.Size(574, 186);
            this.lbGameObjList.TabIndex = 34;
            this.lbGameObjList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lbGameObjectList_KeyDown);
            // 
            // popGameObject
            // 
            this.popGameObject.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportToolStripMenuItem,
            this.moveToObjectToolStripMenuItem,
            this.deleteGameObjectToolStripMenuItem1});
            this.popGameObject.Name = "popNpc";
            this.popGameObject.Size = new System.Drawing.Size(182, 92);
            this.popGameObject.Opening += new System.ComponentModel.CancelEventHandler(this.popGameObject_Opening);
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.exportToolStripMenuItem.Text = "Export to XML";
            this.exportToolStripMenuItem.Click += new System.EventHandler(this.exportToolStripMenuItem_Click);
            // 
            // moveToObjectToolStripMenuItem
            // 
            this.moveToObjectToolStripMenuItem.Name = "moveToObjectToolStripMenuItem";
            this.moveToObjectToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.moveToObjectToolStripMenuItem.Text = "Move To ";
            this.moveToObjectToolStripMenuItem.Click += new System.EventHandler(this.moveToObjectToolStripMenuItem_Click);
            // 
            // deleteGameObjectToolStripMenuItem1
            // 
            this.deleteGameObjectToolStripMenuItem1.Name = "deleteGameObjectToolStripMenuItem1";
            this.deleteGameObjectToolStripMenuItem1.Size = new System.Drawing.Size(181, 22);
            this.deleteGameObjectToolStripMenuItem1.Text = "Delete Game Object";
            this.deleteGameObjectToolStripMenuItem1.Click += new System.EventHandler(this.deleteGameObjectToolStripMenuItem_Click);
            // 
            // bsGameObjects
            // 
            this.bsGameObjects.DataMember = "GameObjects";
            this.bsGameObjects.DataSource = this.botDataSet;
            this.bsGameObjects.Sort = "NAME";
            this.bsGameObjects.CurrentChanged += new System.EventHandler(this.gameObjectsBindingSource_CurrentChanged);
            // 
            // botDataSet
            // 
            this.botDataSet.DataSetName = "BotDataSet";
            this.botDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
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
            this.btnImport.Location = new System.Drawing.Point(481, 458);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(63, 23);
            this.btnImport.TabIndex = 36;
            this.btnImport.Text = "Import";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 13);
            this.label3.TabIndex = 37;
            this.label3.Text = "Name:";
            // 
            // tbName
            // 
            this.tbName.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bsGameObjects, "NAME", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.tbName.Location = new System.Drawing.Point(9, 30);
            this.tbName.Name = "tbName";
            this.tbName.ReadOnly = true;
            this.tbName.Size = new System.Drawing.Size(146, 20);
            this.tbName.TabIndex = 38;
            // 
            // gbDescription
            // 
            this.gbDescription.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.gbDescription.Controls.Add(this.btnAddService);
            this.gbDescription.Controls.Add(this.tbName);
            this.gbDescription.Controls.Add(this.lbActiveServices);
            this.gbDescription.Controls.Add(this.cbAvailServices);
            this.gbDescription.Controls.Add(this.label3);
            this.gbDescription.Location = new System.Drawing.Point(12, 260);
            this.gbDescription.Name = "gbDescription";
            this.gbDescription.Size = new System.Drawing.Size(161, 192);
            this.gbDescription.TabIndex = 39;
            this.gbDescription.TabStop = false;
            this.gbDescription.Text = "Object Description";
            // 
            // btnAddService
            // 
            this.btnAddService.Location = new System.Drawing.Point(120, 61);
            this.btnAddService.Name = "btnAddService";
            this.btnAddService.Size = new System.Drawing.Size(35, 23);
            this.btnAddService.TabIndex = 42;
            this.btnAddService.Text = "Add";
            this.btnAddService.UseVisualStyleBackColor = true;
            this.btnAddService.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // lbActiveServices
            // 
            this.lbActiveServices.ContextMenuStrip = this.popServiceActions;
            this.lbActiveServices.DataSource = this.fkGameObjectsNpcServices;
            this.lbActiveServices.DisplayMember = "SERVICE_NAME";
            this.lbActiveServices.FormattingEnabled = true;
            this.lbActiveServices.Location = new System.Drawing.Point(10, 90);
            this.lbActiveServices.Name = "lbActiveServices";
            this.lbActiveServices.ScrollAlwaysVisible = true;
            this.lbActiveServices.Size = new System.Drawing.Size(145, 95);
            this.lbActiveServices.TabIndex = 39;
            this.lbActiveServices.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lbActiveServices_KeyDown);
            // 
            // popServiceActions
            // 
            this.popServiceActions.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteServiceToolStripMenuItem});
            this.popServiceActions.Name = "popServiceActions";
            this.popServiceActions.Size = new System.Drawing.Size(155, 26);
            this.popServiceActions.Opening += new System.ComponentModel.CancelEventHandler(this.popServiceActions_Opening);
            // 
            // deleteServiceToolStripMenuItem
            // 
            this.deleteServiceToolStripMenuItem.Name = "deleteServiceToolStripMenuItem";
            this.deleteServiceToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.deleteServiceToolStripMenuItem.Text = "Delete Service";
            this.deleteServiceToolStripMenuItem.Click += new System.EventHandler(this.deleteServiceToolStripMenuItem_Click);
            // 
            // fkGameObjectsNpcServices
            // 
            this.fkGameObjectsNpcServices.DataMember = "FK_GameObjects_NpcServices";
            this.fkGameObjectsNpcServices.DataSource = this.bsGameObjects;
            this.fkGameObjectsNpcServices.ListChanged += new System.ComponentModel.ListChangedEventHandler(this.bsFKGameObjectsNpcServices_ListChanged);
            // 
            // cbAvailServices
            // 
            this.cbAvailServices.DataSource = this.bsServiceTypesFiltered;
            this.cbAvailServices.DisplayMember = "NAME";
            this.cbAvailServices.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbAvailServices.FormattingEnabled = true;
            this.cbAvailServices.Location = new System.Drawing.Point(9, 63);
            this.cbAvailServices.Name = "cbAvailServices";
            this.cbAvailServices.Size = new System.Drawing.Size(104, 21);
            this.cbAvailServices.TabIndex = 41;
            // 
            // bsServiceTypesFiltered
            // 
            this.bsServiceTypesFiltered.DataMember = "ServiceTypes";
            this.bsServiceTypesFiltered.DataSource = this.botDataSet;
            // 
            // tbZ
            // 
            this.tbZ.Location = new System.Drawing.Point(26, 95);
            this.tbZ.Name = "tbZ";
            this.tbZ.Size = new System.Drawing.Size(68, 20);
            this.tbZ.TabIndex = 78;
            // 
            // tbY
            // 
            this.tbY.Location = new System.Drawing.Point(26, 69);
            this.tbY.Name = "tbY";
            this.tbY.Size = new System.Drawing.Size(68, 20);
            this.tbY.TabIndex = 77;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(9, 98);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(14, 13);
            this.label8.TabIndex = 76;
            this.label8.Text = "Z";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(9, 72);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(14, 13);
            this.label7.TabIndex = 75;
            this.label7.Text = "Y";
            // 
            // tbX
            // 
            this.tbX.Location = new System.Drawing.Point(26, 43);
            this.tbX.Name = "tbX";
            this.tbX.Size = new System.Drawing.Size(68, 20);
            this.tbX.TabIndex = 74;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(9, 46);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(14, 13);
            this.label6.TabIndex = 74;
            this.label6.Text = "X";
            // 
            // btnAddCoord
            // 
            this.btnAddCoord.Location = new System.Drawing.Point(57, 121);
            this.btnAddCoord.Name = "btnAddCoord";
            this.btnAddCoord.Size = new System.Drawing.Size(37, 23);
            this.btnAddCoord.TabIndex = 70;
            this.btnAddCoord.Text = "Add";
            this.btnAddCoord.UseVisualStyleBackColor = true;
            this.btnAddCoord.Click += new System.EventHandler(this.btnAddCoord_Click);
            // 
            // lbCoordinates
            // 
            this.lbCoordinates.ContextMenuStrip = this.popCoordinates;
            this.lbCoordinates.DataSource = this.fKCoordinatesZoneCoordinates;
            this.lbCoordinates.DisplayMember = "COORD";
            this.lbCoordinates.FormattingEnabled = true;
            this.lbCoordinates.Location = new System.Drawing.Point(6, 49);
            this.lbCoordinates.Name = "lbCoordinates";
            this.lbCoordinates.ScrollAlwaysVisible = true;
            this.lbCoordinates.Size = new System.Drawing.Size(173, 160);
            this.lbCoordinates.TabIndex = 70;
            this.lbCoordinates.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lbCoordinates_KeyDown);
            // 
            // popCoordinates
            // 
            this.popCoordinates.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteCoordinatesToolStripMenuItem});
            this.popCoordinates.Name = "popCoordinates";
            this.popCoordinates.Size = new System.Drawing.Size(178, 26);
            this.popCoordinates.Opening += new System.ComponentModel.CancelEventHandler(this.popCoordinates_Opening);
            // 
            // deleteCoordinatesToolStripMenuItem
            // 
            this.deleteCoordinatesToolStripMenuItem.Name = "deleteCoordinatesToolStripMenuItem";
            this.deleteCoordinatesToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.deleteCoordinatesToolStripMenuItem.Text = "Delete Coordinates";
            this.deleteCoordinatesToolStripMenuItem.Click += new System.EventHandler(this.deleteCoordinatesToolStripMenuItem_Click);
            // 
            // fKCoordinatesZoneCoordinates
            // 
            this.fKCoordinatesZoneCoordinates.DataMember = "FK_CoordinatesZone_Coordinates";
            this.fKCoordinatesZoneCoordinates.DataSource = this.fKGameObjectsCoordinatesZone;
            // 
            // fKGameObjectsCoordinatesZone
            // 
            this.fKGameObjectsCoordinatesZone.DataMember = "FK_GameObjects_CoordinatesZone";
            this.fKGameObjectsCoordinatesZone.DataSource = this.bsGameObjects;
            this.fKGameObjectsCoordinatesZone.Sort = "ZONE_NAME";
            // 
            // gbQuestList
            // 
            this.gbQuestList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbQuestList.Controls.Add(this.lbQuestList);
            this.gbQuestList.Controls.Add(this.btnAddQuest);
            this.gbQuestList.Location = new System.Drawing.Point(480, 260);
            this.gbQuestList.Name = "gbQuestList";
            this.gbQuestList.Size = new System.Drawing.Size(226, 192);
            this.gbQuestList.TabIndex = 45;
            this.gbQuestList.TabStop = false;
            this.gbQuestList.Text = "Quest List";
            // 
            // lbQuestList
            // 
            this.lbQuestList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lbQuestList.ContextMenuStrip = this.popQuestActions;
            this.lbQuestList.DataSource = this.fkGameObjectsQuestList;
            this.lbQuestList.DisplayMember = "TITLE";
            this.lbQuestList.FormattingEnabled = true;
            this.lbQuestList.Location = new System.Drawing.Point(6, 19);
            this.lbQuestList.Name = "lbQuestList";
            this.lbQuestList.Size = new System.Drawing.Size(214, 134);
            this.lbQuestList.TabIndex = 45;
            this.lbQuestList.DoubleClick += new System.EventHandler(this.lbQuestList_DoubleClick);
            this.lbQuestList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lbQuestList_KeyDown);
            // 
            // popQuestActions
            // 
            this.popQuestActions.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.acceptQuestToolStripMenuItem,
            this.deleteQuestToolStripMenuItem});
            this.popQuestActions.Name = "popQuestActions";
            this.popQuestActions.Size = new System.Drawing.Size(151, 48);
            this.popQuestActions.Text = "1";
            this.popQuestActions.Opening += new System.ComponentModel.CancelEventHandler(this.popQuestActions_Opening);
            // 
            // acceptQuestToolStripMenuItem
            // 
            this.acceptQuestToolStripMenuItem.Name = "acceptQuestToolStripMenuItem";
            this.acceptQuestToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.acceptQuestToolStripMenuItem.Text = "Accept Quest";
            this.acceptQuestToolStripMenuItem.Click += new System.EventHandler(this.acceptQuestToolStripMenuItem_Click);
            // 
            // deleteQuestToolStripMenuItem
            // 
            this.deleteQuestToolStripMenuItem.Name = "deleteQuestToolStripMenuItem";
            this.deleteQuestToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.deleteQuestToolStripMenuItem.Text = "Delete Quest";
            this.deleteQuestToolStripMenuItem.Click += new System.EventHandler(this.deleteQuestToolStripMenuItem_Click);
            // 
            // fkGameObjectsQuestList
            // 
            this.fkGameObjectsQuestList.DataMember = "FK_GameObjects_QuestList";
            this.fkGameObjectsQuestList.DataSource = this.bsGameObjects;
            // 
            // btnAddQuest
            // 
            this.btnAddQuest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddQuest.Location = new System.Drawing.Point(145, 163);
            this.btnAddQuest.Name = "btnAddQuest";
            this.btnAddQuest.Size = new System.Drawing.Size(75, 23);
            this.btnAddQuest.TabIndex = 44;
            this.btnAddQuest.Text = "Add Quest";
            this.btnAddQuest.UseVisualStyleBackColor = true;
            this.btnAddQuest.Click += new System.EventHandler(this.btnAddQuest_Click);
            // 
            // btnAddNPC
            // 
            this.btnAddNPC.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAddNPC.Location = new System.Drawing.Point(12, 229);
            this.btnAddNPC.Name = "btnAddNPC";
            this.btnAddNPC.Size = new System.Drawing.Size(99, 23);
            this.btnAddNPC.TabIndex = 64;
            this.btnAddNPC.Text = "Add Target NPC";
            this.btnAddNPC.UseVisualStyleBackColor = true;
            this.btnAddNPC.Click += new System.EventHandler(this.btnAddNPC_Click);
            // 
            // cbServiceList
            // 
            this.cbServiceList.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.botDataSet, "ServiceTypes.NAME", true, System.Windows.Forms.DataSourceUpdateMode.Never));
            this.cbServiceList.DataSource = this.bsServiceTypesFull;
            this.cbServiceList.DisplayMember = "NAME";
            this.cbServiceList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbServiceList.FormattingEnabled = true;
            this.cbServiceList.Location = new System.Drawing.Point(6, 55);
            this.cbServiceList.Name = "cbServiceList";
            this.cbServiceList.Size = new System.Drawing.Size(102, 21);
            this.cbServiceList.TabIndex = 65;
            this.cbServiceList.SelectedIndexChanged += new System.EventHandler(this.cbServiceList_SelectedIndexChanged);
            // 
            // bsServiceTypesFull
            // 
            this.bsServiceTypesFull.DataMember = "ServiceTypes";
            this.bsServiceTypesFull.DataSource = this.botDataSet;
            // 
            // btnMoveToNearest
            // 
            this.btnMoveToNearest.Location = new System.Drawing.Point(6, 82);
            this.btnMoveToNearest.Name = "btnMoveToNearest";
            this.btnMoveToNearest.Size = new System.Drawing.Size(102, 23);
            this.btnMoveToNearest.TabIndex = 66;
            this.btnMoveToNearest.Text = "Move to Nearest NPC";
            this.btnMoveToNearest.UseVisualStyleBackColor = true;
            this.btnMoveToNearest.Click += new System.EventHandler(this.btnMoveToNearest_Click);
            // 
            // labelServices
            // 
            this.labelServices.AutoSize = true;
            this.labelServices.Location = new System.Drawing.Point(6, 39);
            this.labelServices.Name = "labelServices";
            this.labelServices.Size = new System.Drawing.Size(43, 13);
            this.labelServices.TabIndex = 67;
            this.labelServices.Text = "Service";
            // 
            // gbDebug
            // 
            this.gbDebug.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbDebug.Controls.Add(this.cbLearnSkills);
            this.gbDebug.Controls.Add(this.cbUseState);
            this.gbDebug.Controls.Add(this.labelServices);
            this.gbDebug.Controls.Add(this.cbServiceList);
            this.gbDebug.Controls.Add(this.btnMoveToNearest);
            this.gbDebug.Location = new System.Drawing.Point(592, 33);
            this.gbDebug.Name = "gbDebug";
            this.gbDebug.Size = new System.Drawing.Size(114, 190);
            this.gbDebug.TabIndex = 68;
            this.gbDebug.TabStop = false;
            this.gbDebug.Text = "Debug";
            // 
            // cbLearnSkills
            // 
            this.cbLearnSkills.AutoSize = true;
            this.cbLearnSkills.Location = new System.Drawing.Point(9, 111);
            this.cbLearnSkills.Name = "cbLearnSkills";
            this.cbLearnSkills.Size = new System.Drawing.Size(102, 17);
            this.cbLearnSkills.TabIndex = 72;
            this.cbLearnSkills.Text = "And Learn Skills";
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
            // gbCoordinates
            // 
            this.gbCoordinates.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.gbCoordinates.Controls.Add(this.cbCoordZone);
            this.gbCoordinates.Controls.Add(this.lbCoordinates);
            this.gbCoordinates.Location = new System.Drawing.Point(179, 260);
            this.gbCoordinates.Name = "gbCoordinates";
            this.gbCoordinates.Size = new System.Drawing.Size(189, 221);
            this.gbCoordinates.TabIndex = 69;
            this.gbCoordinates.TabStop = false;
            this.gbCoordinates.Text = "Coordinates";
            // 
            // cbCoordZone
            // 
            this.cbCoordZone.DataSource = this.fKGameObjectsCoordinatesZone;
            this.cbCoordZone.DisplayMember = "ZONE_NAME";
            this.cbCoordZone.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCoordZone.FormattingEnabled = true;
            this.cbCoordZone.Location = new System.Drawing.Point(6, 19);
            this.cbCoordZone.Name = "cbCoordZone";
            this.cbCoordZone.Size = new System.Drawing.Size(173, 21);
            this.cbCoordZone.TabIndex = 83;
            this.cbCoordZone.SelectedIndexChanged += new System.EventHandler(this.cbCoordZone_SelectedIndexChanged);
            // 
            // btnAddPlayerTargetCoord
            // 
            this.btnAddPlayerTargetCoord.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddPlayerTargetCoord.Location = new System.Drawing.Point(57, 42);
            this.btnAddPlayerTargetCoord.Name = "btnAddPlayerTargetCoord";
            this.btnAddPlayerTargetCoord.Size = new System.Drawing.Size(36, 23);
            this.btnAddPlayerTargetCoord.TabIndex = 82;
            this.btnAddPlayerTargetCoord.Text = "Add";
            this.btnAddPlayerTargetCoord.UseVisualStyleBackColor = true;
            this.btnAddPlayerTargetCoord.Click += new System.EventHandler(this.btnAddPlayerTargetCoord_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 50);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(34, 13);
            this.label9.TabIndex = 81;
            this.label9.Text = "coord";
            // 
            // cbPlayerTarget
            // 
            this.cbPlayerTarget.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPlayerTarget.FormattingEnabled = true;
            this.cbPlayerTarget.Items.AddRange(new object[] {
            "Player",
            "Target"});
            this.cbPlayerTarget.Location = new System.Drawing.Point(6, 18);
            this.cbPlayerTarget.Name = "cbPlayerTarget";
            this.cbPlayerTarget.Size = new System.Drawing.Size(88, 21);
            this.cbPlayerTarget.TabIndex = 80;
            // 
            // btnAddNewObj
            // 
            this.btnAddNewObj.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddNewObj.Location = new System.Drawing.Point(544, 229);
            this.btnAddNewObj.Name = "btnAddNewObj";
            this.btnAddNewObj.Size = new System.Drawing.Size(94, 23);
            this.btnAddNewObj.TabIndex = 70;
            this.btnAddNewObj.Text = "Add New Object";
            this.btnAddNewObj.UseVisualStyleBackColor = true;
            this.btnAddNewObj.Click += new System.EventHandler(this.btnAddNewObj_Click);
            // 
            // btnAddItem
            // 
            this.btnAddItem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAddItem.Location = new System.Drawing.Point(306, 231);
            this.btnAddItem.Name = "btnAddItem";
            this.btnAddItem.Size = new System.Drawing.Size(39, 23);
            this.btnAddItem.TabIndex = 71;
            this.btnAddItem.Text = "Add";
            this.btnAddItem.UseVisualStyleBackColor = true;
            this.btnAddItem.Click += new System.EventHandler(this.btnAddItem_Click);
            // 
            // cbItemList
            // 
            this.cbItemList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbItemList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbItemList.FormattingEnabled = true;
            this.cbItemList.Location = new System.Drawing.Point(179, 231);
            this.cbItemList.Name = "cbItemList";
            this.cbItemList.Size = new System.Drawing.Size(121, 21);
            this.cbItemList.TabIndex = 72;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(135, 234);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(38, 13);
            this.label5.TabIndex = 73;
            this.label5.Text = "Object";
            // 
            // btnEditObject
            // 
            this.btnEditObject.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEditObject.Location = new System.Drawing.Point(644, 229);
            this.btnEditObject.Name = "btnEditObject";
            this.btnEditObject.Size = new System.Drawing.Size(62, 23);
            this.btnEditObject.TabIndex = 74;
            this.btnEditObject.Text = "Edit";
            this.btnEditObject.UseVisualStyleBackColor = true;
            this.btnEditObject.Click += new System.EventHandler(this.btnEditObject_Click);
            // 
            // cbAllZones
            // 
            this.cbAllZones.DataSource = this.bsZoneList;
            this.cbAllZones.DisplayMember = "NAME";
            this.cbAllZones.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbAllZones.FormattingEnabled = true;
            this.cbAllZones.Location = new System.Drawing.Point(6, 16);
            this.cbAllZones.Name = "cbAllZones";
            this.cbAllZones.Size = new System.Drawing.Size(88, 21);
            this.cbAllZones.TabIndex = 80;
            // 
            // bsZoneList
            // 
            this.bsZoneList.DataMember = "ZoneList";
            this.bsZoneList.DataSource = this.botDataSet;
            this.bsZoneList.Sort = "NAME";
            // 
            // gbAddCoord
            // 
            this.gbAddCoord.Controls.Add(this.tbY);
            this.gbAddCoord.Controls.Add(this.cbAllZones);
            this.gbAddCoord.Controls.Add(this.label6);
            this.gbAddCoord.Controls.Add(this.btnAddCoord);
            this.gbAddCoord.Controls.Add(this.label8);
            this.gbAddCoord.Controls.Add(this.tbX);
            this.gbAddCoord.Controls.Add(this.tbZ);
            this.gbAddCoord.Controls.Add(this.label7);
            this.gbAddCoord.Location = new System.Drawing.Point(374, 260);
            this.gbAddCoord.Name = "gbAddCoord";
            this.gbAddCoord.Size = new System.Drawing.Size(100, 147);
            this.gbAddCoord.TabIndex = 75;
            this.gbAddCoord.TabStop = false;
            this.gbAddCoord.Text = "Add Coordinates";
            // 
            // gbAutoAdd
            // 
            this.gbAutoAdd.Controls.Add(this.cbPlayerTarget);
            this.gbAutoAdd.Controls.Add(this.btnAddPlayerTargetCoord);
            this.gbAutoAdd.Controls.Add(this.label9);
            this.gbAutoAdd.Location = new System.Drawing.Point(374, 413);
            this.gbAutoAdd.Name = "gbAutoAdd";
            this.gbAutoAdd.Size = new System.Drawing.Size(100, 68);
            this.gbAutoAdd.TabIndex = 76;
            this.gbAutoAdd.TabStop = false;
            this.gbAutoAdd.Text = "Auto Add";
            // 
            // GameObjectsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(718, 493);
            this.Controls.Add(this.btnImport);
            this.Controls.Add(this.labelWoWVersion);
            this.Controls.Add(this.gbDescription);
            this.Controls.Add(this.gbAutoAdd);
            this.Controls.Add(this.gbDebug);
            this.Controls.Add(this.gbCoordinates);
            this.Controls.Add(this.btnEditObject);
            this.Controls.Add(this.gbAddCoord);
            this.Controls.Add(this.btnAddNewObj);
            this.Controls.Add(this.lbGameObjList);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cbItemList);
            this.Controls.Add(this.gbQuestList);
            this.Controls.Add(this.btnAddNPC);
            this.Controls.Add(this.btnAddItem);
            this.MinimumSize = new System.Drawing.Size(625, 490);
            this.Name = "GameObjectsForm";
            this.Text = "NPC List";
            this.Activated += new System.EventHandler(this.GameObjectsForm_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
            this.Controls.SetChildIndex(this.btnAddItem, 0);
            this.Controls.SetChildIndex(this.btnAddNPC, 0);
            this.Controls.SetChildIndex(this.gbQuestList, 0);
            this.Controls.SetChildIndex(this.cbItemList, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.lbGameObjList, 0);
            this.Controls.SetChildIndex(this.btnAddNewObj, 0);
            this.Controls.SetChildIndex(this.gbAddCoord, 0);
            this.Controls.SetChildIndex(this.btnEditObject, 0);
            this.Controls.SetChildIndex(this.gbCoordinates, 0);
            this.Controls.SetChildIndex(this.gbDebug, 0);
            this.Controls.SetChildIndex(this.gbAutoAdd, 0);
            this.Controls.SetChildIndex(this.gbDescription, 0);
            this.Controls.SetChildIndex(this.labelWoWVersion, 0);
            this.Controls.SetChildIndex(this.btnImport, 0);
            this.Controls.SetChildIndex(this.btnHelp, 0);
            this.Controls.SetChildIndex(this.btnClose, 0);
            this.Controls.SetChildIndex(this.btnSave, 0);
            this.popGameObject.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bsGameObjects)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.botDataSet)).EndInit();
            this.gbDescription.ResumeLayout(false);
            this.gbDescription.PerformLayout();
            this.popServiceActions.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.fkGameObjectsNpcServices)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsServiceTypesFiltered)).EndInit();
            this.popCoordinates.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.fKCoordinatesZoneCoordinates)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fKGameObjectsCoordinatesZone)).EndInit();
            this.gbQuestList.ResumeLayout(false);
            this.popQuestActions.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.fkGameObjectsQuestList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsServiceTypesFull)).EndInit();
            this.gbDebug.ResumeLayout(false);
            this.gbDebug.PerformLayout();
            this.gbCoordinates.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bsZoneList)).EndInit();
            this.gbAddCoord.ResumeLayout(false);
            this.gbAddCoord.PerformLayout();
            this.gbAutoAdd.ResumeLayout(false);
            this.gbAutoAdd.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox lbGameObjList;
        private System.Windows.Forms.Label labelWoWVersion;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.GroupBox gbDescription;
        private System.Windows.Forms.ListBox lbActiveServices;
        private System.Windows.Forms.Button btnAddService;
        private System.Windows.Forms.ComboBox cbAvailServices;
        private System.Windows.Forms.Button btnAddNPC;
        private System.Windows.Forms.ComboBox cbServiceList;
        private System.Windows.Forms.Button btnMoveToNearest;
        private System.Windows.Forms.Label labelServices;
        private System.Windows.Forms.ContextMenuStrip popServiceActions;
        private System.Windows.Forms.ContextMenuStrip popQuestActions;
        private System.Windows.Forms.ToolStripMenuItem deleteQuestToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip popGameObject;
        private System.Windows.Forms.ToolStripMenuItem deleteGameObjectToolStripMenuItem1;
        private System.Windows.Forms.GroupBox gbDebug;
        private System.Windows.Forms.CheckBox cbUseState;
        private System.Windows.Forms.CheckBox cbLearnSkills;
        private System.Windows.Forms.ToolStripMenuItem deleteServiceToolStripMenuItem;
        private System.Windows.Forms.GroupBox gbQuestList;
        private System.Windows.Forms.Button btnAddQuest;
        private System.Windows.Forms.GroupBox gbCoordinates;
        private System.Windows.Forms.Button btnAddCoord;
        private System.Windows.Forms.ListBox lbCoordinates;
        private System.Windows.Forms.Button btnAddNewObj;
        private System.Windows.Forms.Button btnAddItem;
        private System.Windows.Forms.ComboBox cbItemList;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbZ;
        private System.Windows.Forms.TextBox tbY;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tbX;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.BindingSource bsGameObjects;
        private BabBot.Data.BotDataSet botDataSet;
        private System.Windows.Forms.BindingSource bsServiceTypesFiltered;
        private System.Windows.Forms.BindingSource fkGameObjectsNpcServices;
        private System.Windows.Forms.ContextMenuStrip popCoordinates;
        private System.Windows.Forms.ToolStripMenuItem deleteCoordinatesToolStripMenuItem;
        private System.Windows.Forms.BindingSource bsServiceTypesFull;
        private System.Windows.Forms.Button btnEditObject;
        private System.Windows.Forms.ListBox lbQuestList;
        private System.Windows.Forms.BindingSource fkGameObjectsQuestList;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem acceptQuestToolStripMenuItem;
        private System.Windows.Forms.ComboBox cbAllZones;
        private System.Windows.Forms.GroupBox gbAddCoord;
        private System.Windows.Forms.ComboBox cbPlayerTarget;
        private System.Windows.Forms.ComboBox cbCoordZone;
        private System.Windows.Forms.Button btnAddPlayerTargetCoord;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.BindingSource fKGameObjectsCoordinatesZone;
        private System.Windows.Forms.BindingSource bsZoneList;
        private System.Windows.Forms.BindingSource fKCoordinatesZoneCoordinates;
        private System.Windows.Forms.GroupBox gbAutoAdd;
        private System.Windows.Forms.ToolStripMenuItem moveToObjectToolStripMenuItem;
    }
}
