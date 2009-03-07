using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace BabBot.Manager
{
    ///<summary>
    /// Bot management thread
    ///</summary>
    public class BotManager
    {
        private Thread mainThread;
        private StateManager StateManager;

        /// <summary>
        /// Constructor
        /// </summary>
        public BotManager()
        {
            StateManager = new StateManager();
        }

        public void Start()
        {
            mainThread = new Thread(new ThreadStart(Update));
            mainThread.Start();
            Thread.Sleep(0);
        }

        private void Update()
        {
            try
            {
                while (true)
                {
                    // TODO: This is where we should periodically update the player data and
                    // status
                    Console.WriteLine("Ciao sono un thread");
                    StateManager.UpdateState();
                    Thread.Sleep(0);
                }
            }
            catch (ThreadAbortException ex)
            {
                // TODO: put cleanup code here
            }
            finally
            {
                // By placing a finally statement w avoid the thread to throw back the exception as
                // per default behavior
            }
        }

        public void Stop()
        {
            mainThread.Abort();
        }
    }
}
