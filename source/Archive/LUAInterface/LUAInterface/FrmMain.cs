using System;
using System.Diagnostics;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels.Ipc;
using System.Windows.Forms;
using CommandPrompt;
using EasyHook;
using LUAHost;
using SharedInterface;

namespace LUAInterface
{
    public partial class FrmMain : Form
    {
        private static bool IsInjected;
        private IPCInterface IPCObject;
        private SystemLog remoteLog;

        public FrmMain()
        {
            InitializeComponent();
            cmd.PromptString = ">";
        }

        private string InjectLUAHost()
        {
            string logChannelName = null;
            string res = "";

            try
            {
                if (IsInjected)
                {
                    return "WoW is already injected.";
                }

                IpcServerChannel ipcLogChannel = RemoteHooking.IpcCreateServer<SystemLog>(ref logChannelName,
                                                                                          WellKnownObjectMode.Singleton);
                remoteLog = RemoteHooking.IpcConnectClient<SystemLog>(logChannelName);
                remoteLog.OnServerEvent += EventShim.Create(prov_OnServerEvent);

                int outprocessid;
                RemoteHooking.CreateAndInject(WoWHelper.GetWowInstallationPath(),
                                              "",
                                              (int) WoWHelper.CreationFlags.NewConsole |
                                              (int) WoWHelper.CreationFlags.Suspended,
                                              "LUAHost.dll",
                                              "LUAHost.dll",
                                              out outprocessid,
                                              logChannelName);

                IPCObject = RemoteHooking.IpcConnectClient<IPCInterface>(remoteLog.InjectedDLLChannelName);
                res = "Injection has been completed";

                IsInjected = true;
            }
            catch (Exception e)
            {
                res = e.Message;
                IsInjected = false;
            }

            return res;
        }

        private void OnServerEvent(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                OnServerEventDelegate ev = OnServerEvent;
                object[] parameters = {sender, e};
                Invoke(ev, parameters);
            }
            else
            {
                cmd.AddMessage(((ServerEventArgs) e).Message);
            }
        }

        private void prov_OnServerEvent(object sender, EventArgs e)
        {
            OnServerEventDelegate d = OnServerEvent;
            AsyncCallback cb = MyAsyncCallback;
            IAsyncResult ar = d.BeginInvoke(sender, e, cb, d);
        }

        public void MyAsyncCallback(IAsyncResult ar)
        {
            var dlgt = (OnServerEventDelegate) ar.AsyncState;
            dlgt.EndInvoke(ar);
        }

        private void commandPrompt1_Command(object sender, CommandEventArgs e)
        {
            string command = e.Command.ToLower();

            switch (command)
            {
                case "cls":
                    cmd.ClearMessages();
                    e.Cancel = true;
                    break;

                case "inject":
                    e.Message = InjectLUAHost();
                    break;

                case "exit":
                    Application.Exit();
                    break;

                case "kill":
                    Process[] processes = Process.GetProcessesByName("wow");
                    foreach (Process p in processes)
                    {
                        e.Message = string.Format("[{0}] WoW has been killed", p.Id);
                        p.Kill();
                    }
                    IsInjected = false;
                    break;

                default:
                    e.Message = string.Format(" invalid command {0}", e.Command);
                    break;
            }
        }

        #region Nested type: OnServerEventDelegate

        private delegate void OnServerEventDelegate(object sender, EventArgs e);

        #endregion
    }
}