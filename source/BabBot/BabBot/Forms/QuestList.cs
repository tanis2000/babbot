using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BabBot.Manager;
using BabBot.Data;

namespace BabBot.Forms
{
    public partial class QuestList : GenericDialog
    {
        public QuestList() : base("quest_list")
        {
            InitializeComponent();
            /// botDataSet = (BotDataSet) DataManager.GameData.Copy();
            //continentListBindingSource.DataSource = DataManager.GameData;
            //zoneListBindingSource.DataSource = DataManager.GameData;

        }

        private void QuestList_Load(object sender, EventArgs e)
        {
            //listBox1.DataSource = DataManager.ContinentListTable;
            //listBox1.DisplayMember = "NAME";
            // 
            
            // 
            // continentListBindingSource.DataMember = "ContinentList";
        }

        private void continentListBindingSource_PositionChanged(object sender, EventArgs e)
        {
            // DataRowView row = (DataRowView)this.BindingContext
               //     [continentListBindingSource.DataSource].Current;
            //DataRowView row = (DataRowView) continentListBindingSource.Current;
            //object id = row["ID"];

            //zoneListBindingSource.Filter = "CID=" + row["ID"];
        }
    }
}
