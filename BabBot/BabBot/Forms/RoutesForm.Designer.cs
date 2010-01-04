namespace BabBot.Forms
{
    partial class RoutesForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RoutesForm));
            this.lblRouteList = new System.Windows.Forms.Label();
            this.tvRoutes = new System.Windows.Forms.TreeView();
            this.popRouteList = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openRouteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportRouteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.deleteRouteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imgRoutes = new System.Windows.Forms.ImageList(this.components);
            this.botDataSet = new BabBot.Data.BotDataSet();
            this.ctrlRouteDetails = new BabBot.Forms.Shared.RouteDetails();
            this.btnImportRoute = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cbSortBy = new System.Windows.Forms.ComboBox();
            this.popRouteList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.botDataSet)).BeginInit();
            this.SuspendLayout();
            // 
            // btnHelp
            // 
            this.btnHelp.Location = new System.Drawing.Point(12, 431);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(505, 431);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(424, 431);
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lblRouteList
            // 
            this.lblRouteList.AutoSize = true;
            this.lblRouteList.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.lblRouteList.Location = new System.Drawing.Point(9, 9);
            this.lblRouteList.Name = "lblRouteList";
            this.lblRouteList.Size = new System.Drawing.Size(55, 13);
            this.lblRouteList.TabIndex = 3;
            this.lblRouteList.Text = "Route List";
            // 
            // tvRoutes
            // 
            this.tvRoutes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tvRoutes.ContextMenuStrip = this.popRouteList;
            this.tvRoutes.HideSelection = false;
            this.tvRoutes.ImageIndex = 0;
            this.tvRoutes.ImageList = this.imgRoutes;
            this.tvRoutes.Location = new System.Drawing.Point(12, 25);
            this.tvRoutes.Name = "tvRoutes";
            this.tvRoutes.SelectedImageIndex = 0;
            this.tvRoutes.Size = new System.Drawing.Size(354, 389);
            this.tvRoutes.TabIndex = 4;
            this.tvRoutes.DoubleClick += new System.EventHandler(this.tvRoutes_DoubleClick);
            this.tvRoutes.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvRoutes_AfterSelect);
            this.tvRoutes.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.tvRoutes_BeforeSelect);
            this.tvRoutes.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tvRoutes_KeyDown);
            // 
            // popRouteList
            // 
            this.popRouteList.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openRouteToolStripMenuItem,
            this.exportRouteToolStripMenuItem,
            this.toolStripSeparator1,
            this.deleteRouteToolStripMenuItem});
            this.popRouteList.Name = "popRouteList";
            this.popRouteList.Size = new System.Drawing.Size(150, 76);
            // 
            // openRouteToolStripMenuItem
            // 
            this.openRouteToolStripMenuItem.Name = "openRouteToolStripMenuItem";
            this.openRouteToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.openRouteToolStripMenuItem.Text = "Open Route";
            this.openRouteToolStripMenuItem.Click += new System.EventHandler(this.openRouteToolStripMenuItem_Click);
            // 
            // exportRouteToolStripMenuItem
            // 
            this.exportRouteToolStripMenuItem.Name = "exportRouteToolStripMenuItem";
            this.exportRouteToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.exportRouteToolStripMenuItem.Text = "Export Route";
            this.exportRouteToolStripMenuItem.Click += new System.EventHandler(this.exportRouteToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(146, 6);
            // 
            // deleteRouteToolStripMenuItem
            // 
            this.deleteRouteToolStripMenuItem.Name = "deleteRouteToolStripMenuItem";
            this.deleteRouteToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.deleteRouteToolStripMenuItem.Text = "Delete Route";
            this.deleteRouteToolStripMenuItem.Click += new System.EventHandler(this.deleteRouteToolStripMenuItem_Click);
            // 
            // imgRoutes
            // 
            this.imgRoutes.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgRoutes.ImageStream")));
            this.imgRoutes.TransparentColor = System.Drawing.Color.Transparent;
            this.imgRoutes.Images.SetKeyName(0, "undef_route.gif");
            this.imgRoutes.Images.SetKeyName(1, "def_route.gif");
            // 
            // botDataSet
            // 
            this.botDataSet.DataSetName = "BotDataSet";
            this.botDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // ctrlRouteDetails
            // 
            this.ctrlRouteDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ctrlRouteDetails.AutoSize = true;
            this.ctrlRouteDetails.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ctrlRouteDetails.Enabled = false;
            this.ctrlRouteDetails.Location = new System.Drawing.Point(380, 25);
            this.ctrlRouteDetails.Margin = new System.Windows.Forms.Padding(0);
            this.ctrlRouteDetails.Name = "ctrlRouteDetails";
            this.ctrlRouteDetails.Size = new System.Drawing.Size(200, 288);
            this.ctrlRouteDetails.TabIndex = 5;
            // 
            // btnImportRoute
            // 
            this.btnImportRoute.Location = new System.Drawing.Point(343, 431);
            this.btnImportRoute.Name = "btnImportRoute";
            this.btnImportRoute.Size = new System.Drawing.Size(75, 23);
            this.btnImportRoute.TabIndex = 7;
            this.btnImportRoute.Text = "Import Route";
            this.btnImportRoute.UseVisualStyleBackColor = true;
            this.btnImportRoute.Click += new System.EventHandler(this.btnImportRoute_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(109, 436);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Sorted By";
            // 
            // cbSortBy
            // 
            this.cbSortBy.FormattingEnabled = true;
            this.cbSortBy.Items.AddRange(new object[] {
            "Point A",
            "Point B"});
            this.cbSortBy.Location = new System.Drawing.Point(168, 433);
            this.cbSortBy.Name = "cbSortBy";
            this.cbSortBy.Size = new System.Drawing.Size(105, 21);
            this.cbSortBy.TabIndex = 9;
            this.cbSortBy.SelectedIndexChanged += new System.EventHandler(this.cbSortBy_SelectedIndexChanged);
            // 
            // RoutesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(592, 466);
            this.Controls.Add(this.tvRoutes);
            this.Controls.Add(this.lblRouteList);
            this.Controls.Add(this.ctrlRouteDetails);
            this.Controls.Add(this.cbSortBy);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnImportRoute);
            this.MinimumSize = new System.Drawing.Size(580, 500);
            this.Name = "RoutesForm";
            this.Text = "Route Manager";
            this.VisibleChanged += new System.EventHandler(this.RoutesForm_VisibleChanged);
            this.Controls.SetChildIndex(this.btnImportRoute, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.cbSortBy, 0);
            this.Controls.SetChildIndex(this.ctrlRouteDetails, 0);
            this.Controls.SetChildIndex(this.lblRouteList, 0);
            this.Controls.SetChildIndex(this.tvRoutes, 0);
            this.Controls.SetChildIndex(this.btnHelp, 0);
            this.Controls.SetChildIndex(this.btnClose, 0);
            this.Controls.SetChildIndex(this.btnSave, 0);
            this.popRouteList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.botDataSet)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblRouteList;
        private System.Windows.Forms.TreeView tvRoutes;
        private BabBot.Data.BotDataSet botDataSet;
        private System.Windows.Forms.ImageList imgRoutes;
        private BabBot.Forms.Shared.RouteDetails ctrlRouteDetails;
        private System.Windows.Forms.ContextMenuStrip popRouteList;
        private System.Windows.Forms.ToolStripMenuItem openRouteToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem deleteRouteToolStripMenuItem;
        private System.Windows.Forms.Button btnImportRoute;
        private System.Windows.Forms.ToolStripMenuItem exportRouteToolStripMenuItem;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbSortBy;
    }
}
