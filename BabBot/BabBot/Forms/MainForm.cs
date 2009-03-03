using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Security;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using Magic;

namespace BabBot.Forms
{
    public partial class MainForm : Form
    {
        private BlackMagic bm;

        public MainForm()
        {
            InitializeComponent();
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            Process p;

            try
            {
                p = Process.Start(@"c:\games\world of warcraft\wow.exe", "", "guest", new SecureString(), "");
            } 
            catch(Exception)
            {
                MessageBox.Show("Cannot run an instance of WoW", "Error" , MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (p == null)
            {
                MessageBox.Show("Cannot obtain process information", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                bm = new BlackMagic();
                bm.OpenProcessAndThread(p.Id);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void btnUpdateLocation_Click(object sender, EventArgs e)
        {
            if (bm != null)
            {
                bm.SuspendThread(bm.ThreadHandle);

                uint playerBase = bm.ReadUInt(bm.ReadUInt(bm.ReadUInt(0x0127F13C) + 0x30) + 0x28);

                float x = bm.ReadFloat(playerBase + 0x7D0);
                float y = bm.ReadFloat(playerBase + 0x7D4);
                float z = bm.ReadFloat(playerBase + 0x7D8);


                bm.ResumeThread();

                UInt64 CurTargetGuid = bm.ReadUInt64(0x10A68E0);

                tbLocation.Text = String.Format("Loc: {0}, {1}, {2} | {3}", x, y, z, CurTargetGuid);
            }
        }
    }
}
