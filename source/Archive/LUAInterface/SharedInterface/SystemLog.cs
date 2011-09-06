using System;

namespace SharedInterface
{
    public class SystemLog : MarshalByRefObject, IProvider
    {
        public string InjectedDLLChannelName { get; set; }

        #region IProvider Members

        public void ClientEvent(string message)
        {
            throw new NotImplementedException();
        }

        public event EventHandler OnServerEvent;

        #endregion

        public void Log(string Message)
        {
            //Console.WriteLine(Message);
            SendServerEvent(Message);
        }

        public override object InitializeLifetimeService()
        {
            return null;
        }

        public void SendServerEvent(string message)
        {
            if (null != OnServerEvent)
            {
                try
                {
                    OnServerEvent(this, new ServerEventArgs(message));
                }
                catch (Exception ex)
                {
                    Console.WriteLine("{0}", ex);
                }
            }
        }
    }
}