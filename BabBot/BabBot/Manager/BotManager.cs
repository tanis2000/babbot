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

        private void OnInitialize()
        {
            //Output.Instance.Debug("OnInitialize", this);
        }

        private void OnException(Exception e, GThread.ThreadPhase phase)
        {
            //Output.Instance.LogError(e);
            Console.WriteLine(e.ToString());
        }

        private void OnRun()
        {
            if (ProcessManager.ProcessRunning)
            {
                ProcessManager.CheckInGame();
                if (ProcessManager.InGame)
                {
                    ProcessManager.UpdatePlayer();
                    //State Machien now in player object
                    //stateManager.UpdateState();
                    //Update Player State Machine
                    ProcessManager.Player.StateMachine.Update();

                    //ProcessManager.ScriptHost.Update();
                }
            }
            Thread.Sleep(250);
        }

        #endregion
    }
}