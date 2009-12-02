using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace BabBot.Forms
{
    public partial class NPCListForm : BabBot.Forms.GenericDialog
    {
        public NPCListForm() : base ("npc_list")
        {
            InitializeComponent();
        }
    }
}
