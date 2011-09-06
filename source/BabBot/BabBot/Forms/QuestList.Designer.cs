using BabBot.Manager;

namespace BabBot.Forms
{
    partial class QuestList
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
            this.botDataSet = new BabBot.Data.BotDataSet();
            this.questInfo1 = new BabBot.Forms.Shared.QuestInfo();
            ((System.ComponentModel.ISupportInitialize)(this.botDataSet)).BeginInit();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(529, 241);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(448, 241);
            // 
            // botDataSet
            // 
            this.botDataSet.DataSetName = "BotDataSet";
            this.botDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // questInfo1
            // 
            this.questInfo1.Location = new System.Drawing.Point(3, 2);
            this.questInfo1.Name = "questInfo1";
            this.questInfo1.Size = new System.Drawing.Size(610, 233);
            this.questInfo1.TabIndex = 3;
            // 
            // QuestList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(616, 276);
            this.Controls.Add(this.questInfo1);
            this.Name = "QuestList";
            this.Text = "QuestList";
            this.Load += new System.EventHandler(this.QuestList_Load);
            this.Controls.SetChildIndex(this.questInfo1, 0);
            this.Controls.SetChildIndex(this.btnHelp, 0);
            this.Controls.SetChildIndex(this.btnClose, 0);
            this.Controls.SetChildIndex(this.btnSave, 0);
            ((System.ComponentModel.ISupportInitialize)(this.botDataSet)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private BabBot.Data.BotDataSet botDataSet;
        private BabBot.Forms.Shared.QuestInfo questInfo1;
    }
}