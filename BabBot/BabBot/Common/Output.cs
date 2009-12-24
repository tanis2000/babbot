using System;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using BabBot.Manager;
using System.Drawing;
using System.Collections;

namespace BabBot.Common
{
    public sealed class Output
    {

        private object Obj = new Object();
        private static Hashtable FacilityList = new Hashtable();

        private static readonly Output instance = new Output();
         
        public static Output Instance
        {
            get { return instance; }
        }


        #region Delegates

        public delegate void OutputEventHandler(string facility, string time, 
                                                            string message, Color color);

        #endregion


        private bool LogDebug = true; // We force logging of debug messages for now (it should become an option)
        private bool LogScript = true; // We force logging of script messages for debugging purpouses

        private static string DateString
        {
            get { return DateTime.Today.ToShortDateString().Replace("/", ""); }
        }

        private static string TimeString
        {
            get { return DateTime.Now.ToLongTimeString(); }
        }

        private static string BTimeString
        {
            get { return TimeString; }
        }

        public static event OutputEventHandler OutputEvent;

        /// <summary>
        /// Convert system facility name into something human readable
        /// Replace undescore with space and capitailze each word
        /// </summary>
        /// <param name="lfs">System Logging Facility</param>
        /// <returns>String used to create logging file name</returns>
        public static string GetLogNameByLfs(string lfs)
        {
            return GetLogNameByLfs(lfs, " ");
        }

        public static string GetLogNameByLfs(string lfs, string separator)
        {
            string[] ss = lfs.Replace('_', ' ').Split(' ');
            for (int i = 0; i < ss.Length; i++)
                ss[i] = char.ToUpper(ss[i][0]) + ss[i].Substring(1);
            return string.Join(separator, ss);
        }

        /// <summary>
        /// A simple logging method. Will output to DebugLog.txt.
        /// </summary>
        /// <param name="message">The message to be logged.</param>
        /// <returns>Nothing.</returns>
        internal void Log(string message)
        {
            Log("misc", message);
        }

        internal void Log(string facility, string message)
        {
            Log(facility, message, Color.Black);
        }

        /// <summary>
        /// logging method for specific facility. Will output to Debug-<Facility>.txt.
        /// </summary>
        /// <param name="message">The message to be logged.</param>
        /// <returns>Nothing.</returns>
        internal void Log(string lfs, string message, Color color)
        {
            lock (Obj)
            {
                object cf = (string)FacilityList[lfs];
                string cur_facility = null;

                if (cf == null)
                {
                    cur_facility = GetLogNameByLfs(lfs);
                    FacilityList.Add(lfs, cur_facility);
                }
                else
                    cur_facility = cf.ToString();

                try
                {
                    using (StreamWriter w = new StreamWriter(Format("{1}\\{0}-Log-{2}.txt",
                        DateString, ProcessManager.Config.LogParams.Dir, cur_facility), true))
                    {
                        w.WriteLine(Format("{0},{1}", BTimeString, message));
                    }
                }
                catch (Exception)
                {
                    // Disk migh full
                    // Keep working
                }

                if (OutputEvent != null)
                {
                    OutputEvent(cur_facility, BTimeString, message, color);
                }
            }
        }

        // Added for compatibility
        internal void LogError(Exception exception)
        {
            LogError("misc", exception);
        }

        /// <summary>
        /// A simple logging method. Will output to ErrorLog.txt.
        /// </summary>
        /// <param name="exception">The exception to be logged</param>
        /// <returns>Nothing.</returns>
        internal void LogError(string facility, Exception exception)
        {
            Log(facility, Format("{0}{1}{2}{1}{3}", exception.InnerException, 
                            Environment.NewLine, exception.Message, exception.StackTrace), Color.Red);
        }

        /// <summary>
        /// A simple logging method. Will output to ErrorLog.txt.
        /// </summary>
        /// <param name="facility">Name of logging facility</param>
        /// <param name="err">Error string</param>
        /// <param name="exception">The exception to be logged</param>
        /// <returns>Nothing.</returns>
        internal void LogError(string facility, string err, Exception exception)
        {
            Log(facility, err, Color.Red);
            Log(facility, Format("{0}{1}{2}{1}{3}", exception.InnerException,
                            Environment.NewLine, exception.Message, exception.StackTrace), Color.Red);
        }

        /// <summary>
        /// A simple logging method. Will output to ErrorLog.txt.
        /// </summary>
        /// <param name="facility">Name of logging facility</param>
        /// <param name="err">Error string</param>
        internal void LogError(string facility, string err)
        {
            Log(facility, err, Color.Red);
        }

        /// <summary>
        /// A simple logging method. Will output to ChatLog.txt.
        /// </summary>
        /// <param name="message">The message to be logged.</param>
        /// <returns>Nothing.</returns>
        internal void ChatLog(string message, Color color)
        {
            Log("chat", message, color);
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

        // Added for compatibility
        internal void Debug(string message, object sender)
        {
            Debug("misc", message, sender);
        }

        internal void Debug(string facility, string message, object sender)
        {
            Debug(facility, Format("[{0}] {1}", sender.GetType().Name, message));
        }

        // Added for compatibility
        internal void Debug(string message)
        {
            Debug("misc", message, Color.Gray);
        }

        /// <summary>
        /// A simple debug method. Will log debug messages.
        /// </summary>
        /// <param name="sender">Should always be "this"</param>
        /// <param name="message">The message to be logged.</param>
        /// <returns>Nothing.</returns>
        internal void Debug(string facility, string message, object sender, Color color)
        {
            Debug(facility, Format("[{0}] {1}", sender.GetType().Name, message), color);
        }

        internal void Debug(string facility, string message)
        {
            Debug(facility, Format("{0}", message), Color.Gray);
        }

        /// <summary>
        /// Debugs the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        internal void Debug(string facility, string message, Color color)
        {
            if (LogDebug)
            {
                Log(facility, Format("{0}", message), color);
            }
        }

        // Added for compatibility
        public void Script(string message, object sender)
        {
            Script(Format("[{0}] {1}", sender.GetType().Name, message));
        }

        // Added for compatibility
        public void Script(string message)
        {
            Script("misc", message, Color.Blue);
        }

        /// <summary>
        /// Simple logging for scripts. Will output to ScriptLog.txt.
        /// </summary>
        /// <param name="message">The message to be logged.</param>
        /// <returns>Nothing.</returns>
        public void Script(string facility, string message, Color color)
        {
            if (LogScript)
            {
                Log(facility, message, color);
            }
        }

        /// <summary>
        /// Log a script message with reference to the script itself
        /// </summary>
        /// <param name="sender">Should always be "this"</param>
        /// <param name="message">The message to be logged.</param>
        /// <returns>Nothing.</returns>
        public void Script(string facility, string message, object sender, Color color)
        {
            Script(facility, Format("[{0}] {1}", sender.GetType().Name, message), color);
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
