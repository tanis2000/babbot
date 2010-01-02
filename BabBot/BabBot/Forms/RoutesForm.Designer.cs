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
            this.imgRoutes = new System.Windows.Forms.ImageList(this.components);
            this.botDataSet = new BabBot.Data.BotDataSet();
            this.ctrlRouteDetails = new BabBot.Forms.Shared.RouteDetails();
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
            this.tvRoutes.ImageIndex = 0;
            this.tvRoutes.ImageList = this.imgRoutes;
            this.tvRoutes.Location = new System.Drawing.Point(12, 25);
            this.tvRoutes.Name = "tvRoutes";
            this.tvRoutes.SelectedImageIndex = 0;
            this.tvRoutes.Size = new System.Drawing.Size(354, 389);
            this.tvRoutes.TabIndex = 4;
            // 
            // imgRoutes
            // 
            this.imgRoutes.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgRoutes.ImageStream")));
            this.imgRoutes.TransparentColor = System.Drawing.Color.Transparent;
            this.imgRoutes.Images.SetKeyName(0, "def_route.gif");
            this.imgRoutes.Images.SetKeyName(1, "undef_route.gif");
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
            this.ctrlRouteDetails.Location = new System.Drawing.Point(380, 25);
            this.ctrlRouteDetails.Margin = new System.Windows.Forms.Padding(0);
            this.ctrlRouteDetails.Name = "ctrlRouteDetails";
            this.ctrlRouteDetails.Size = new System.Drawing.Size(200, 253);
            this.ctrlRouteDetails.TabIndex = 5;
            // 
            // RoutesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(592, 466);
            this.Controls.Add(this.tvRoutes);
            this.Controls.Add(this.lblRouteList);
            this.Controls.Add(this.ctrlRouteDetails);
            this.MinimumSize = new System.Drawing.Size(580, 500);
            this.Name = "RoutesForm";
            this.Text = "Route Manager";
            this.VisibleChanged += new System.EventHandler(this.RoutesForm_VisibleChanged);
            this.Controls.SetChildIndex(this.ctrlRouteDetails, 0);
            this.Controls.SetChildIndex(this.lblRouteList, 0);
            this.Controls.SetChildIndex(this.tvRoutes, 0);
            this.Controls.SetChildIndex(this.btnHelp, 0);
            this.Controls.SetChildIndex(this.btnClose, 0);
            this.Controls.SetChildIndex(this.btnSave, 0);
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
    }
}
