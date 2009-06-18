using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Dante
{
    public static class Log
    {

        public static void Debug(string msg)
        {
            using (TextWriter streamWriter = new StreamWriter(@"c:\tmp\DanteLog.txt", true))
            {
                streamWriter.WriteLine(string.Format("[{0}] {1}", DateTime.Now.TimeOfDay, msg));
            }
        }
    }
}
