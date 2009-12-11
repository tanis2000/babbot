using System;

namespace KnowObjects
{
    /// <summary>
    /// Is called by the server when a message is sent.
    /// </summary>
    public delegate void MessageDeliveredEventHandler(string message);

    /// <summary>
    /// Dispatcher provides common methods for logging.
    /// </summary>
    public interface IDispatcher
    {
        /// <summary>
        /// Sends the message to all clients.
        /// </summary>
        /// <param name="message">Message to send.</param>
        void SendMessage(string message);

        /// <summary>
        /// Message delivered event.
        /// </summary>
        event MessageDeliveredEventHandler MessageDelivered;
    }

    /// <summary>
    /// Small wrapper because event require well-known class.
    /// </summary>
    public class MessageReceiver : MarshalByRefObject
    {
        /// <summary>
        /// Late-bound method as a workaround.
        /// </summary>
        public MessageDeliveredEventHandler MessageDeliveredHandler;

        /// <summary>
        /// It's not a client or a server. So I call late-bound method here.
        /// </summary>
        /// <param name="message"></param>
        public void MessageDelivered(string message)
        {
            if (MessageDeliveredHandler != null)
            {
                MessageDeliveredHandler(message);
            }
        }
    }
}