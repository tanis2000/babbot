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
                    streamWriter.WriteLine(string.Format("[{0:HH:mm:ss}] {1}", DateTime.Now, msg));
                }
            }
            catch
            {
                // someone could be writing to our file.. we just discard this log line for now
            }
        }
    }
}