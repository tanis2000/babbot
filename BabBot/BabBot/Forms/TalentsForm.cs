using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Net;
using System.IO;
using BabBot.Wow;
using BabBot.Manager;

namespace BabBot.Forms
{
    public partial class TalentsForm : Form
    {
        Regex trex = new Regex("^http://www.wowarmory.com/talent-calc.xml\\?cid=(\\d)\\&tal=(\\d+)$");

        public TalentsForm()
        {
            InitializeComponent();
        }

        private string ReadURL(string url)
        {
            // Create a request for the URL.         
            WebRequest request = WebRequest.Create(url);
            // If required by the server, set the credentials.
            request.Credentials = CredentialCache.DefaultCredentials;
            // Get the response.
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            // Display the status.
            // response.StatusDescription;
            // Get the stream containing content returned by the server.
            Stream stream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(stream);
            // Read the content.
            string res = reader.ReadToEnd();
            // Cleanup the streams and the response.
            reader.Close();
            stream.Close();
            response.Close();

            return res;
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            try
            {
                string url = tbTalentURL.Text;
                // Check for class id
                Match m = trex.Match(url);

                if (m.Success && (m.Groups.Count == 3))
                {
                    string s = m.Groups[1].ToString();
                    int cid = Convert.ToInt32(s);
                    string response = ReadURL(url);
                } else {
                    MessageBox.Show(string.Format(
                        "Invalid URL. '{0}' excpected", trex.ToString()));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable read URL: " + ex.Message);
            }
        }

        private void tbTalentURL_TextChanged(object sender, EventArgs e)
        {
            btnImport.Enabled = !tbTalentURL.Text.Equals("");
        }

        private void TalentsForm_Load(object sender, EventArgs e)
        {
            tbTalentURL_TextChanged(sender, e);
        }

        private void cbTalentTemplates_DropDown(object sender, EventArgs e)
        {
            cbTalentTemplates.Items.Clear();
            string[] dir;

            // Scan Profiles/Talents for list
            try
            {
                 dir = Directory.GetFiles("Profiles\\Talents", "*.xml");
            }
            catch (System.IO.DirectoryNotFoundException ex)
            {
                MessageBox.Show("Directory Profiles\\Talents not found");
                return;
            }
            
            // Check each file
            foreach (string fname in dir)
            {
                try
                {
                    Talents tlist = ProcessManager.ReadTalents(fname);
                    if ((tlist != null) && (tlist.Description != null))
                        cbTalentTemplates.Items.Add(tlist);
                } catch { 
                    // Continue 
                }
            }
        }
    }
}
