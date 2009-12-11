using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BabBot.Manager;
using System.Threading;

namespace BabBot.Wow.Helpers
{
    /// <summary>
    /// Class with common lua functions
    /// </summary>
    public static class LuaHelper
    {
        /// <summary>
        /// Target Unit by name and wait until it became current target
        /// </summary>
        /// <param name="name">Unit Name</param>
        /// <returns>True if unit became a new target and False if not</returns>
        public static bool TargetUnitByName(string name)
        {
            WowUnit player = ProcessManager.Player;
            if ((player.CurTarget == null) || !player.CurTarget.Name.Equals(name))
                ProcessManager.Injector.
                    Lua_ExecByName("TargetUnit",
                        new string[] { name });

            // Max wait 1 sec
            DateTime dt = DateTime.Now;
            WowUnit target = player.CurTarget;

            while (((target == null) || !target.Name.Equals(name)) 
                    && ((DateTime.Now - dt).TotalMilliseconds <= 5000))
            {
                Thread.Sleep(100);
                target = player.CurTarget;
            }

            return ((target != null) && target.Name.Equals(name));
        }
    }
}
