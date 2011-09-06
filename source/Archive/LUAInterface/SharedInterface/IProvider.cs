using System;

namespace SharedInterface
{
    [Serializable]
    public class ServerEventArgs : EventArgs
    {
        private readonly string m_message;

        public ServerEventArgs(string message)
        {
            m_message = message;
        }

        public string Message
        {
            get { return m_message; }
        }
    }

    public interface IProvider
    {
        void ClientEvent(string message);

        event EventHandler OnServerEvent;
    }

    public class EventShim : MarshalByRefObject
    {
        private EventShim(EventHandler callback)
        {
            m_target += callback;
        }

        private event EventHandler m_target;

        public void DoInvoke(object sender, EventArgs args)
        {
            m_target(sender, args);
        }

        public static EventHandler Create(EventHandler target)
        {
            var shim = new EventShim(target);
            return shim.DoInvoke;
        }
    }
}