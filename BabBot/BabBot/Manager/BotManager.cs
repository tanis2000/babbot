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

        /// <summary>
        /// Constructor
        /// </summary>
        public BotManager()
        {
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
                    Console.WriteLine("Ciao sono un thread");
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
