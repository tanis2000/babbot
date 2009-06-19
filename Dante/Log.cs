using System;
using System.IO;

namespace Dante
{
    public static class Log
    {
        public static void Debug(string msg)
        {
            try
            {
                using (TextWriter streamWriter = new StreamWriter(@"c:\tmp\DanteLog.txt", true))
                {
                    streamWriter.WriteLine(string.Format("[{0}] {1}", DateTime.Now.TimeOfDay, msg));
                }
            }
            catch
            {
                // someone could be writing to our file.. we just discard this log line for now
            }
        }
    }
}