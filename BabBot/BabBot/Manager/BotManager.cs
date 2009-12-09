/*
    This file is part of BabBot.

    BabBot is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    BabBot is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with BabBot.  If not, see <http://www.gnu.org/licenses/>.
  
    Copyright 2009 BabBot Team
*/
using System;
using System.Threading;
using BabBot.Common;
using BabBot.Wow;

namespace BabBot.Manager
{
    ///<summary>
    /// Bot management thread
    ///</summary>
    public class BotManager
    {
        private int refresh_time;
        private int idle_sleep_time;

        //private readonly StateManager stateManager;
        private GThread workerThread;

        /// <summary>
        /// Constructor
        /// </summary>
        public BotManager()
        {
            //stateManager = StateManager.Instance;
            //stateManager.Init();
            workerThread = null;
        }

        //public PlayerState State
        //{
        //    get { return stateManager.State; }
        //}

        protected void InitThreadObj()
        {
            workerThread = new GThread {Name = "BotManagerThread"};
            workerThread.OnRun += OnRun;
            workerThread.OnException += OnException;
            workerThread.OnInitialize += OnInitialize;
            workerThread.OnBeforeStart += OnBeforeStart;
            workerThread.OnBeforeStop += OnBeforeStop;
            workerThread.OnFinalize += OnFinalize;
            workerThread.OnInit += InitConfigParams;
        }

        public void Start()
        {
            InitThreadObj();
            if (workerThread != null)
            {
                workerThread.Start();
            }
        }

        public void Stop()
        {
            Output.Instance.Log("char", "Stopping bot ...");
            if (workerThread != null)
            {
                workerThread.Stop();
                workerThread = null;
                Output.Instance.Log("char", "Bot stopped");
            } else
                Output.Instance.Log("char", "Bot not running.");
        }

        #region Thread Events

        private void OnFinalize()
        {
            //Output.Instance.Debug("OnFinalize", this);
        }

        private void OnBeforeStop()
        {
            Output.Instance.Debug("char", "OnBeforeStop", this);
        }

        private void OnBeforeStart()
        {
            //Output.Instance.Debug("OnBeforeStart", this);
        }

        public void ChangeConfig()
        {
            // Inter-thread call but we safe since changes are minor
            Output.Instance.Debug("char", "Re-Loading bot parameters ...", this);
            InitConfigParams();
        }

        private void InitConfigParams()
        {
            refresh_time = ProcessManager.Config.WoWInfo.RefreshTime;
            idle_sleep_time = ProcessManager.Config.WoWInfo.IdleSleepTime;
        }

        private void OnInitialize()
        {
            //Output.Instance.Debug("OnInitialize", this);
        }

        private void OnException(Exception e, GThread.ThreadPhase phase)
        {
            //Output.Instance.LogError(e);
            Console.WriteLine(e.ToString());
        }

        private void Debug(string msg)
        {
            Output.Instance.Debug("char", msg);
        }

        private void Log(string msg)
        {
            Output.Instance.Log("char", msg);
        }

        private void OnRun()
        {
            switch (ProcessManager.ProcessStatus)
            {
                case ProcessManager.ProcessStatuses.WOW_STARTING:
                    // Start Wow
                    if (!ProcessManager.StartWow())
                        // Something wrong with system. Doesn't make sense to continue
                        Stop();
                    break;

                case ProcessManager.ProcessStatuses.WOW_RUNNING:

                    // Do it on the beginning of each cycle
                    if (ProcessManager.ProcessRunning)
                        ProcessManager.CheckInGame();

                    // Analyze game status
                    switch (ProcessManager.GameStatus)
                    {
                        case ProcessManager.GameStatuses.INIT:
                            if (ProcessManager.Config.WoWInfo.AutoLogin)
                                AutoLogin();
                            else
                                // Go idle
                                ProcessManager.SetGameIdle(idle_sleep_time);
                            break;

                        case ProcessManager.GameStatuses.DISCONNECTED:
                            // Wait 5 sec it might be a crush
                            Debug("Entered 'DISCONNECTED' state. Waiting " +
                                (int)(idle_sleep_time / 1000) + 
                                " sec to make sure it's not the crash");
                            Thread.Sleep(idle_sleep_time);
                            if (!ProcessManager.ProcessRunning)
                            {
                                Debug("Wow.exe not running. It is the crush.");

                                return;
                            }
                            // or false alarem
                            else if (ProcessManager.CheckInGame())
                            {
                                Debug("Wow.exe still running. It is not the crush.");
                                if (ProcessManager.Config.WoWInfo.AutoLogin &&
                                    ProcessManager.Config.Account.ReConnect)
                                {
                                    Log("Disconnected. Reconnecting as configured ...");
                                    AutoLogin();
                                }
                                else
                                    // Just wait but don't reset status
                                    Thread.Sleep(idle_sleep_time);
                            }
                            else
                                // go idle
                                ProcessManager.SetGameIdle(idle_sleep_time);
                            break;

                        case ProcessManager.GameStatuses.INITIALIZED:
                            ProcessManager.UpdatePlayer();
                            ProcessManager.Player.StateMachine.Update();

                            break;

                        case ProcessManager.GameStatuses.ENTERING_WORLD:
                        case ProcessManager.GameStatuses.IDLE:
                            // Just wait
                            Thread.Sleep(idle_sleep_time);
                            break;

                        default:
                            Debug("Game State '"+ 
                                Enum.GetName(typeof(ProcessManager.GameStatuses), 
                                ProcessManager.GameStatus) + "' not implemented yet");
                            break;
                    }

                    break;

                case ProcessManager.ProcessStatuses.WOW_INJECTING:
                case ProcessManager.ProcessStatuses.WOW_INITIALIZING:
                case ProcessManager.ProcessStatuses.WOW_LOOK_FOR_TLS:
                    // Just wait
                    Thread.Sleep(idle_sleep_time);
                    break;

                case ProcessManager.ProcessStatuses.IDLE:
                case ProcessManager.ProcessStatuses.WOW_CRUSHED:
                case ProcessManager.ProcessStatuses.WOW_CLOSED:
                    // Stop on error or when it not suppose running
                    Stop();
                    break;

                default:
                    // Something we forgot ...
                    ProcessManager.ShowError("Internal bug - Unknown app status: " + 
                      Enum.GetName(typeof(ProcessManager.ProcessStatuses), 
                      ProcessManager.ProcessStatus) + ". Terminate execution");
                    Environment.Exit(2);

                    break;
            }

            Thread.Sleep(refresh_time);
        }

        /// <summary>
        /// Auto login in WoW with profile's parameters
        /// </summary>
        private void AutoLogin() {
            ProcessManager.GameStatus = ProcessManager.GameStatuses.LOGGING;

            ProcessManager.Injector.Lua_RegisterInputHandler();
            try
            {
                Config config = ProcessManager.Config;
                if (Login.AutoLogin(config.Account.RealmLocation, config.Account.GameType,
                        config.Account.Realm, config.Account.LoginUsername,
                            config.Account.getAutoLoginPassword(), config.Character, 5))
                    ProcessManager.GameStatus = ProcessManager.GameStatuses.ENTERING_WORLD;
                else
                    ProcessManager.SetGameIdle(idle_sleep_time);

            }
            catch (Exception e)
            {
                // Login exception means new patch
                // Exit bot
                ProcessManager.ShowError(e.Message);
                Environment.Exit(5);
            }
            ProcessManager.Injector.Lua_UnRegisterInputHandler();            
        }
        #endregion
    }
}