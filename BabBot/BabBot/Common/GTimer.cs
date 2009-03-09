using System;
using System.Runtime.InteropServices;

namespace BabBot.Common
{
    public class GTimer
    {
        #region -- Private Variables --

        private readonly double countDowntime;

        #endregion

        #region Dll import

        /// <summary>
        /// The QueryPerformanceCounter function retrieves the current 
        /// value of the high-resolution performance counter.
        /// </summary>
        /// <param name="x">
        /// Pointer to a variable that receives the 
        /// current performance-counter value, in counts.
        /// </param>
        /// <returns>
        /// If the function succeeds, the return value is 
        /// nonzero.
        /// </returns>
        [DllImport("kernel32.dll")]
        private static extern int QueryPerformanceCounter(ref long x);

        /// <summary>
        /// The QueryPerformanceFrequency function retrieves the 
        /// frequency of the high-resolution performance counter, 
        /// if one exists. The frequency cannot change while the 
        /// system is running.
        /// </summary>
        /// <param name="x">
        /// Pointer to a variable that receives 
        /// the current performance-counter frequency, in counts 
        /// per second. If the installed hardware does not support 
        /// a high-resolution performance counter, this parameter 
        /// can be zero.
        /// </param>
        /// <returns>
        /// If the installed hardware supports a 
        /// high-resolution performance counter, the return value 
        /// is nonzero.
        /// </returns>
        [DllImport("kernel32.dll")]
        private static extern int QueryPerformanceFrequency(ref long x);

        #endregion

        public GTimer()
        {
            countDowntime = 0;
            Frequency = GetFrequency();
            Reset();
        }

        public GTimer(double countDowntime)
        {
            this.countDowntime = countDowntime;
            Frequency = GetFrequency();
            Reset();
        }

        private long StartTime { get; set; }

        private long Frequency { get; set; }

        public bool isReady()
        {
            try
            {
                if (Peek() > countDowntime)
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return false;
        }

        public void Reset()
        {
            StartTime = GetValue();
        }

        public long Peek()
        {
            return (long) (((GetValue() - StartTime)/(double) Frequency)*10000);
        }

        private static long GetValue()
        {
            long ret = 0;
            if (QueryPerformanceCounter(ref ret) == 0)
            {
                throw new NotSupportedException("Error while querying " + "the high-resolution performance counter.");
            }
            return ret;
        }

        private static long GetFrequency()
        {
            long ret = 0;
            if (QueryPerformanceFrequency(ref ret) == 0)
            {
                throw new NotSupportedException(
                    "Error while querying "
                    + "the performance counter frequency.");
            }
            return ret;
        }
    }
}