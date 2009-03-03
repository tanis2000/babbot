using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Security;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using BabBot.Manager;
using BabBot.Wow;
using Magic;

namespace BabBot.Forms
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            Timer inGameTimer = new Timer();
            inGameTimer.Interval = 1000;
            inGameTimer.Start();
            inGameTimer.Tick += new EventHandler(inGameTimer_Tick);

            Timer playerTimer = new Timer();
            playerTimer.Interval = 1000;
            playerTimer.Start();
            playerTimer.Tick += new EventHandler(playerTimer_Tick);

        }

        void inGameTimer_Tick(object sender, EventArgs e)
        {
            ProcessManager.CheckInGame();
        }

        void playerTimer_Tick(object sender, EventArgs e)
        {
            ProcessManager.UpdatePlayer();
        }

        // TODO: Create a timed event that checks if WoW is running and that updates the UI accordingly

        private void btnRun_Click(object sender, EventArgs e)
        {
            try
            {
                ProcessManager.StartWow();
                btnRun.Enabled = false;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error" , MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void btnUpdateLocation_Click(object sender, EventArgs e)
        {
            if (ProcessManager.ProcessRunning)
            {
                ProcessManager.UpdatePlayerLocation();

                tbLocation.Text = String.Format("Loc: {0}, {1}, {2} | {3}", ProcessManager.Player.Location.X, ProcessManager.Player.Location.Y, ProcessManager.Player.Location.Z, ProcessManager.Player.CurTargetGuid);
            }
        }

        private void btnFindTLS_Click(object sender, EventArgs e)
        {
            try
            {
                ProcessManager.FindTLS();
                tbTLS.Text = string.Format("{0:X}", ProcessManager.TLS);
                tbClientConnectionPointer.Text = string.Format("{0:X}", Globals.ClientConnectionPointer);
                tbClientConnectionOffset.Text = string.Format("{0:X}", Globals.ClientConnectionOffset);
                tbPlayerBaseOffset.Text = string.Format("{0:X}", Globals.PlayerBaseOffset);
                tbCurMgr.Text = string.Format("{0:X}", Globals.CurMgr);
                tbLocalGUID.Text = ProcessManager.ObjectManager.GetLocalGUID().ToString();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}
