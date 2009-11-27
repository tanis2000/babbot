using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BabBot.Forms
{
    public partial class GenericDialog : Form
    {
        // Internal name
        private string _name;
        
        GenericDialog()
        {
            InitializeComponent();
        }

        public GenericDialog(string name) : this()
        {
            _name = name;
            
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            // TODO Open Default Browswer
            ShowInfoMessage("Test message. Help for '" + 
                                        _name + "' coming soon");
        }

        protected void ShowErrorMessage(string err) {
            MessageBox.Show(this, err, "ERROR",
                MessageBoxButtons.OK, MessageBoxIcon.Error);   
        }

        protected void ShowInfoMessage(string info)
        {
            MessageBox.Show(this, info, "INFO",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
