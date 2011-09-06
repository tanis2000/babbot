using System;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels.Ipc;
using System.Threading;
using EasyHook;
using SharedInterface;

namespace LUAHost
{
    public class Main : IEntryPoint
    {
        public static SystemLog Logger;

        public Main(RemoteHooking.IContext InContext, string LogChannelName)
        {
            Logger = RemoteHooking.IpcConnectClient<SystemLog>(LogChannelName);
            LogMessage("LUAHost Main");
        }

        public void Run(RemoteHooking.IContext InContext, string ChannelName)
        {
            LogMessage("LUAHost Run");
            try
            {
                string outChannelName = null;
                IpcServerChannel ipcLogChannel = RemoteHooking.IpcCreateServer<IPCInterface>(ref outChannelName,
                                                                                             WellKnownObjectMode.
                                                                                                 Singleton);

                //notify client of channel creation via logger
                Logger.InjectedDLLChannelName = outChannelName;

                LogMessage("LUAHost Run (NativeAPI.RhWakeUpProcess)");
                NativeAPI.RhWakeUpProcess();

                while (true)
                {
                    Thread.Sleep(100);
                }
            }
            catch (Exception e)
            {
                LogMessage(string.Format("LUAHost Run(Exception) {0}", e.Message));
            }
        }

        private static void LogMessage(string Message)
        {
            if (Logger != null)
            {
                Logger.Log(string.Format("[{0}]: {1}", DateTime.Now.ToLongTimeString(), Message));
            }
        }
    }
}