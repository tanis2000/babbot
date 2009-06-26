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
        private readonly StateManager stateManager;
        private GThread workerThread;

        /// <summary>
        /// Constructor
        /// </summary>
        public BotManager()
        {
            stateManager = StateManager.Instance;
            stateManager.Init();
            workerThread = null;
        }

        public PlayerState State
        {
            get { return stateManager.State; }
        }

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
            if (workerThread != null)
            {
                workerThread.Stop();
                workerThread = null;
            }
        }

        #region Thread Events

        private void OnFinalize()
        {
            Console.WriteLine("OnFinalize");
        }

        private void OnBeforeStop()
        {
            Console.WriteLine("OnBeforeStop");
        }

        private void OnBeforeStart()
        {
            Console.WriteLine("OnBeforeStart");
        }

        private void OnInitialize()
        {
            Console.WriteLine("OnInitialize");
        }

        private void OnException(Exception e, GThread.ThreadPhase phase)
        {
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
                    stateManager.UpdateState();
                    ProcessManager.ScriptHost.Update();
                }
            }
            Thread.Sleep(250);
        }

        #endregion
    }
}