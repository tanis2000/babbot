using System;
using System.Collections.Generic;
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
        /// Simple wrapper around Injector Lua_ExecByName
        /// </summary>
        /// <param name="lfname">Lua Function Name from WoWData.xml</param>
        /// <param name="param_list">List of parameters</param>
        /// <returns></returns>
        public static string[] Exec(string lfname, string[] param_list)
        {
            return ProcessManager.Injector.Lua_ExecByName(lfname, param_list);
        }

        /// <summary>
        /// Wrapper around Injector Lua_ExecByName with one input integer parameter
        /// </summary>
        /// <param name="lfname">Lua Function Name from WoWData.xml</param>
        /// <param name="i">Single integet parameter</param>
        /// <returns></returns>
        public static string[] Exec(string lfname, int i)
        {
            return ProcessManager.Injector.
                Lua_ExecByName(lfname, new string[] { Convert.ToString(i) });
        }

        /// <summary>
        /// Wrapper around Injector Lua_ExecByName with one boolean input parameter
        /// </summary>
        /// <param name="lfname"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        public static string[] Exec(string lfname, bool f)
        {
            return ProcessManager.Injector.Lua_ExecByName(lfname,
                    new string[] { Convert.ToString(f).ToLower() });
        }

        /// <summary>
        /// Wrapper around Injector Lua_ExecByName with one string input parameter
        /// </summary>
        /// <param name="lfname"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string[] Exec(string lfname, string s)
        {
            return ProcessManager.Injector.Lua_ExecByName(lfname, new string[] { s });
        }

        /// <summary>
        /// Wrapper around Injector Lua_ExecByName without parameters
        /// </summary>
        /// <param name="lfname"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string[] Exec(string lfname)
        {
            return ProcessManager.Injector.Lua_ExecByName(lfname);
        }

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
