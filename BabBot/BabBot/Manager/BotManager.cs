using System;
using System.Threading;

namespace BabBot.Manager
{
    ///<summary>
    /// Bot management thread
    ///</summary>
    public class BotManager
    {
        private readonly StateManager StateManager;
        private Thread mainThread;

        /// <summary>
        /// Constructor
        /// </summary>
        public BotManager()
        {
            StateManager = new StateManager();
        }

        public void Start()
        {
            mainThread = new Thread(Update);
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

                    // Dagli un po di respiro, almeno 10 ms
                    Thread.Sleep(10);
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