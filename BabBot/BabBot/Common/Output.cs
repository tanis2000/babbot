using System;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using BabBot.Manager;

namespace BabBot.Common
{
    public sealed class Output
    {

        private static readonly Output instance = new Output();

        public static Output Instance
        {
            get { return instance; }
        }


        #region Delegates

        public delegate void OutputEventHandler(string message);

        #endregion


        public bool LogDebug;
        public bool LogScript;

        private static string DateString
        {
            get { return DateTime.Today.ToShortDateString().Replace('/', '-'); }
        }

        private static string TimeString
        {
            get { return DateTime.Now.ToLongTimeString(); }
        }

        private static string BTimeString
        {
            get { return string.Format("[{0}]", TimeString); }
        }

        public static event OutputEventHandler OutputEvent;
        public static event OutputEventHandler DebugEvent;

        /// <summary>
        /// A simple logging method. Will output to DebugLog.txt.
        /// </summary>
        /// <param name="message">The message to be logged.</param>
        /// <returns>Nothing.</returns>
        internal void Log(string message)
        {
            using (StreamWriter w = new StreamWriter(Format("{1}\\{0}-DebugLog.txt", DateString, ProcessManager.Config.LogPath), true))
            {
                w.WriteLine(Format("{0} {1}", BTimeString, message));
            }
            if (OutputEvent != null)
            {
                OutputEvent(message);
            }
        }


        /// <summary>
        /// A simple logging method. Will output to ChatLog.txt.
        /// </summary>
        /// <param name="message">The message to be logged.</param>
        /// <returns>Nothing.</returns>
        internal void ChatLog(string message)
        {
            using (StreamWriter w = new StreamWriter(Format("{1}\\{0}-ChatLog.txt", DateString, ProcessManager.Config.LogPath), true))
            {
                w.WriteLine(Format("{0} {1}", BTimeString, message));
            }
        }

        /// <summary>
        /// A simple logging method. Will output to ErrorLog.txt.
        /// </summary>
        /// <param name="exception">The exception to be logged</param>
        /// <returns>Nothing.</returns>
        internal void LogError(Exception exception)
        {
            using (StreamWriter w = new StreamWriter(Format("{1}\\{0}-ErrorLog.txt", DateString, ProcessManager.Config.LogPath), true))
            {
                w.WriteLine(Format("{0} {1}{2}{3}{2}{4}", BTimeString, exception.InnerException, Environment.NewLine,
                                   exception.Message, exception.StackTrace));
            }
        }

        /// <summary>
        /// Logs a string value to a specified text file. You can use this as if it was a string.Format() statement for the message.
        /// </summary>
        /// <param name="textFile">The text file. Using a full path will output to that path, otherwise it will output to the current directory.</param>
        /// <param name="message">The message.</param>
        /// <param name="args">The args.</param>
        internal void LogText(string textFile, string message, params object[] args)
        {
            using (TextWriter tw = new StreamWriter(DateString + "-" + textFile, true))
            {
                tw.WriteLine(Format(message, args));
            }
        }

        /// <summary>
        /// A simple debug method. Will log debug messages.
        /// </summary>
        /// <param name="sender">Should always be "this"</param>
        /// <param name="message">The message to be logged.</param>
        /// <returns>Nothing.</returns>
        internal void Debug(string message, object sender)
        {
            if (LogDebug)
            {
                Log(Format("[{0}] {1}", sender.GetType().Name, message));
            }
            if (DebugEvent != null)
            {
                DebugEvent(Format("[{0}] {1}", sender.GetType().Name, message));
            }
        }

        /// <summary>
        /// Debugs the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        internal void Debug(string message)
        {
            if (LogDebug)
            {
                Log(Format("{0}", message));
            }
            if (DebugEvent != null)
            {
                DebugEvent(Format("{0}", message));
            }
        }

        /// <summary>
        /// Simple logging for scripts. Will output to ScriptLog.txt.
        /// </summary>
        /// <param name="message">The message to be logged.</param>
        /// <returns>Nothing.</returns>
        public void Script(string message)
        {
            using (StreamWriter w = new StreamWriter(Format("{1}\\{0}-ScriptLog.txt", DateString, ProcessManager.Config.LogPath), true))
            {
                w.WriteLine(Format("{0} {1}", BTimeString, message));
            }
        }

        /// <summary>
        /// Log a script message with reference to the script itself
        /// </summary>
        /// <param name="sender">Should always be "this"</param>
        /// <param name="message">The message to be logged.</param>
        /// <returns>Nothing.</returns>
        public void Script(string message, object sender)
        {
            if (LogScript)
            {
                Script(Format("[{0}] {1}", sender.GetType().Name, message));
            }
        }

        /*
        /// <summary>
        /// Normal output to the console window.
        /// </summary>
        /// <param name="message">The message to be logged.</param>
        /// <returns>Nothing.</returns>
        internal void Echo(string message)
        {
            if (OutputEvent != null)
            {
                OutputEvent(message);
            }
        }
        */

        /// <summary>
        /// Formats the specified message. This is the equivalent to String.Format, except that it already includes the culture
        /// information. This is mainly used to help keep code a bit cleaner.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="args">The args.</param>
        /// <returns></returns>
        internal static string Format(string message, params object[] args)
        {
            return string.Format(CultureInfo.CurrentCulture, message, args);
        }

        /// <summary>
        /// Creates a directory if it doesn't already exist. (This path may include subfolders leading up to the directory you want,
        /// all subfolders will be created as well.)
        /// </summary>
        /// <param name="path">The path.</param>
        internal static void CreateDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        /// <summary>
        /// Displays a message box with the specified message, and message type.
        /// This is basically MessageBox.Show with a default OK button, and Icon/Sound.
        /// This method is more-or-less to keep the CLR happy.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="messageType">Type of the message.</param>
        internal DialogResult MsgBox(string message, OBMessageBoxType messageType)
        {
            if (String.IsNullOrEmpty(message))
            {
                throw new ArgumentException("Invalid message passed. Cannot display message box!", "message");
            }
            switch (messageType)
            {
                case OBMessageBoxType.Error:
                    return MessageBox.Show(message, "Babbot - Error", MessageBoxButtons.OK, MessageBoxIcon.Error,
                                           MessageBoxDefaultButton.Button1);
                case OBMessageBoxType.Info:
                    return MessageBox.Show(message, "Babbot - Info", MessageBoxButtons.OK, MessageBoxIcon.Information,
                                           MessageBoxDefaultButton.Button1);
                case OBMessageBoxType.CriticalError:
                    return MessageBox.Show(message, "Babbot - Critical Error", MessageBoxButtons.OK, MessageBoxIcon.Stop,
                                           MessageBoxDefaultButton.Button1);
                case OBMessageBoxType.Warning:
                    return MessageBox.Show(message, "Babbot - Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning,
                                           MessageBoxDefaultButton.Button1);
                default:
                    throw new ArgumentException("Invalid message type provided. Cannot show message box!", "messageType");
            }
        }
    }

    internal enum OBMessageBoxType
    {
        Error,
        Info,
        CriticalError,
        Warning
    }

}
