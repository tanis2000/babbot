using System;
using System.Threading;
using BabBot.Common;

namespace BabBot.Manager
{
    ///<summary>
    /// Bot management thread
    ///</summary>
    public class BotManager
    {
        private readonly StateManager StateManager;
        // private Thread mainThread;
        private GThread workerThread;

        /// <summary>
        /// Constructor
        /// </summary>
        public BotManager()
        {
            StateManager = new StateManager();
            workerThread = null;
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
            Console.WriteLine(e.Message);
        }

        private void OnRun()
        {
            StateManager.UpdateState();
            Thread.Sleep(50);
        }

        #endregion
    }
}