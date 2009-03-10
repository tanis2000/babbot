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

            /*
            var inGameTimer = new Timer();
            inGameTimer.Interval = 1000;
            inGameTimer.Start();
            inGameTimer.Tick += inGameTimer_Tick;

            var playerTimer = new Timer();
            playerTimer.Interval = 1000;
            playerTimer.Start();
            playerTimer.Tick += playerTimer_Tick;
            */

            // ProcessManager events binding
            ProcessManager.WoWProcessStarted += wow_ProcessStarted;
            ProcessManager.WoWProcessEnded += wow_ProcessEnded;
            ProcessManager.WoWProcessFailed += wow_ProcessFailed;
            ProcessManager.WoWProcessAccessFailed += wow_ProcessAccessFailed;

            // Starts the bot thread
            ProcessManager.PlayerUpdate += PlayerUpdate;
            StartBotThread();
        }

        #region Events

        private void PlayerUpdate()
        {
            if (InvokeRequired)
            {
                PlayerUpdateDelegate del = PlayerUpdate;
                Invoke(del);
            }
            else
            {
                txtCurrentX.Text = ProcessManager.Player.Location.X.ToString();
                txtCurrentY.Text = ProcessManager.Player.Location.Y.ToString();
                txtCurrentZ.Text = ProcessManager.Player.Location.Z.ToString();
                txtLastDistance.Text = ProcessManager.Player.LastDistance.ToString();
                txtFaceRadian.Text = ProcessManager.Player.LastFaceRadian.ToString();

                float orientation = (float)((ProcessManager.Player.Orientation*180)/Math.PI);
                txtCurrentFace.Text = string.Format("{0}°", orientation);

                float facing = (float)((ProcessManager.Player.LastFaceRadian * 180) / Math.PI);
                txtComputedFacing.Text = string.Format("{0}°", facing);
            }
        }

        private void wow_ProcessEnded(int process)
        {
            // Cross-Thread operation
            if (InvokeRequired)
            {
                // Setup the cross-thread call
                ProcessEndedDelegate del = wow_ProcessEnded;
                object[] parameters = {process};
                Invoke(del, parameters);
            }
            else
            {
                // Main Thread
                btnRun.Enabled = true;
                btnAttachToWow.Enabled = true;
            }
        }

        private static void wow_ProcessFailed(string error)
        {
            MessageBox.Show(error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void wow_ProcessStarted(int process)
        {
            btnRun.Enabled = false;
            btnAttachToWow.Enabled = false;
        }

        private static void wow_ProcessAccessFailed(string error)
        {
            MessageBox.Show(error, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        #region Nested type: PlayerUpdateDelegate

        private delegate void PlayerUpdateDelegate();

        #endregion

        /*
        private static void inGameTimer_Tick(object sender, EventArgs e)
        {
            ProcessManager.CheckInGame();
        }

        private static void playerTimer_Tick(object sender, EventArgs e)
        {
            ProcessManager.UpdatePlayer();
        }
        */

        #region Nested type: ProcessEndedDelegate

        private delegate void ProcessEndedDelegate(int process);

        #endregion

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
                tbPlayerHp.Text = ProcessManager.Player.Hp.ToString();
                tbPlayerMaxHp.Text = ProcessManager.Player.MaxHp.ToString();
                tbPlayerMp.Text = ProcessManager.Player.Mp.ToString();
                tbPlayerMaxMp.Text = ProcessManager.Player.MaxMp.ToString();
                tbPlayerXp.Text = ProcessManager.Player.Xp.ToString();
                tbPlayerTarget.Text = string.Format("{0:X}", ProcessManager.Player.CurTargetGuid);
                tbPlayerTargetName.Text = ProcessManager.Player.CurTargetName;
                tbPlayerNearObjects.Text = "Objects" + Environment.NewLine + "===========" + Environment.NewLine +
                                           ProcessManager.Player.NearObjectsAsTextList + Environment.NewLine + "Mobs" +
                                           Environment.NewLine +
                                           "===========" + Environment.NewLine +
                                           ProcessManager.Player.NearMobsAsTextList;
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
                tbWndHandle.Text = ProcessManager.WowHWND.ToString();
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
            var dlg = new OpenFileDialog();
            dlg.Multiselect = false;
            dlg.Filter = "BabBot Profile (*.xml)|*.xml";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                ProcessManager.Profile.FileName = dlg.FileName;
                ProcessManager.Profile.Load();
            }
        }

        private void btnAttachToWow_Click(object sender, EventArgs e)
        {
            ProcessManager.AttachToWow();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var f = new AboutForm();
            f.ShowDialog();
        }

        private void StartBotThread()
        {
            ProcessManager.BotManager.Start();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            ProcessManager.BotManager.Stop();
        }

        private void btnMovementTest_Click(object sender, EventArgs e)
        {
            var destPos = new Vector3D();
            destPos.X = float.Parse(txtX.Text);
            destPos.Y = float.Parse(txtY.Text);
            destPos.Z = float.Parse(txtZ.Text);
            ProcessManager.Player.MoveTo(destPos);
        }

        private void btnStopMovement_Click(object sender, EventArgs e)
        {
            ProcessManager.Player.Stop();
        }
    }
}