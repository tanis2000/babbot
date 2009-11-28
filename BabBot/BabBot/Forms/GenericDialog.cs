using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Diagnostics;

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

        private static string GetDefaultBrowserPath()
        {
            // Registry info for default browser
            string key = @"http\shell\open\command";
            RegistryKey registryKey = Registry.ClassesRoot.OpenSubKey(key, false);
            // get default browser path
            return ((string)registryKey.GetValue(null, null)).Split('"')[1];
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            // Open local URL in default browswer
            string url = "file:///" + Environment.CurrentDirectory.Replace("\\", "/") +
                "/Doc/index.html" + "#" + _name;

            Process.Start(GetDefaultBrowserPath(), url);
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
