using System;
using System.Windows.Forms;
using BabBot.Manager;
using BabBot.Wow;

namespace BabBot.Forms
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            var inGameTimer = new Timer();
            inGameTimer.Interval = 1000;
            inGameTimer.Start();
            inGameTimer.Tick += inGameTimer_Tick;

            var playerTimer = new Timer();
            playerTimer.Interval = 1000;
            playerTimer.Start();
            playerTimer.Tick += playerTimer_Tick;

            // ProcessManager events binding
            ProcessManager.WoWProcessStarted += wow_ProcessStarted;
            ProcessManager.WoWProcessEnded += wow_ProcessEnded;
            ProcessManager.WoWProcessFailed += wow_ProcessFailed;
            ProcessManager.WoWProcessAccessFailed += wow_ProcessAccessFailed;
        }

        #region Events

        private void wow_ProcessEnded(int process)
        {
            // Cross-Thread operation
            if (InvokeRequired)
            {
                //Setup the cross-thread call
                ProcessEndedDelegate del = wow_ProcessEnded;
                object[] parameters = {process};
                Invoke(del, parameters);
            }
            else
            {
                // Main Thread
                btnRun.Enabled = true;
            }
        }

        private static void wow_ProcessFailed(string error)
        {
            MessageBox.Show(error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void wow_ProcessStarted(int process)
        {
            btnRun.Enabled = false;
        }

        private static void wow_ProcessAccessFailed(string error)
        {
            MessageBox.Show(error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private static void inGameTimer_Tick(object sender, EventArgs e)
        {
            ProcessManager.CheckInGame();
        }

        private static void playerTimer_Tick(object sender, EventArgs e)
        {
            ProcessManager.UpdatePlayer();
        }

        private delegate void ProcessEndedDelegate(int process);

        #endregion

        // TODO: Create a timed event that checks if WoW is running and that updates the UI accordingly

        private void btnRun_Click(object sender, EventArgs e)
        {
            ProcessManager.StartWow();
        }

        private void btnUpdateLocation_Click(object sender, EventArgs e)
        {
            if (ProcessManager.ProcessRunning)
            {
                ProcessManager.Player.UpdateFromClient();

                tbLocation.Text = String.Format("Loc: {0}, {1}, {2} | {3}", ProcessManager.Player.Location.X,
                                                ProcessManager.Player.Location.Y, ProcessManager.Player.Location.Z,
                                                ProcessManager.Player.CurTargetGuid);
                tbOrientation.Text = String.Format("Or.: {0}", ProcessManager.Player.Orientation);
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnLoadProfile_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Multiselect = false;
            dlg.Filter = "BabBot Profile (*.xml)|*.xml";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                ProcessManager.Profile.FileName = dlg.FileName;
                ProcessManager.Profile.Load();
            }

        }

    }
}