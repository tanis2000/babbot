using System;
using System.Collections.Generic;
using System.Text;
using BabBot.Common;
using BabBot.Manager;
using System.Threading;

namespace BabBot.Wow.Helpers
{
    /// <summary>
    /// Helper Class for generic NPC operations
    /// </summary>
    public static class NpcHelper
    {

        /// <summary>
        /// Execute lua call and return parameters
        /// Raise exception if lua call return null
        /// </summary>
        /// <returns></returns>
        private static string[] DoGetNpcDialogInfo()
        {
            string[] res = ProcessManager.Injector.Lua_ExecByName("GetNpcDialogInfo");
            if (res == null)
                throw new Exception("Failed execute 'GetNpcDialogInfo' lua function.");
            return res;
        }

        /// <summary>
        /// Interact with NPC if no Gossip frame open
        /// and return type of open frame.
        /// If NPC has 1 quest or 1 service (vendor for ex) it might go to quest directly
        /// so it not always returning available gossip options
        /// </summary>
        /// <param name="npc_name"></param>
        /// <returns>See description for "GetNpcFrameInfo" lua call</returns>
        public static string[] GetTargetNpcDialogInfo(string npc_name)
        {
            string[] fparams = DoGetNpcDialogInfo();
            string cur_service = fparams[0];

            if (cur_service == null)
            {
                fparams = InteractNpc(npc_name);
                cur_service = fparams[0];
            }

            return fparams;
        }

        /// <summary>
        /// Interact with NPC, wait until NPC frame opens and return type of open frame.
        /// See description for GetTargetNpcFrameInfo for detail
        /// Max Waiting delay configured by max_npc_interact_time parameter from app_config element
        /// </summary>
        /// <param name="npc_name"></param>
        /// <returns></returns>
        public static string[] InteractNpc(string npc_name)
        {
            Output.Instance.Log("npc", "Interacting with NPC '" + npc_name + "' ...");
            ProcessManager.Injector.Lua_ExecByName("InteractWithTarget");

            // IF NPC doesn't have any quests and just on gossip option the WoW
            // use it option by default

            DateTime dt = DateTime.Now;
            Thread.Sleep(100);
            TimeSpan ts = DateTime.Now.Subtract(dt);

            string[] fparams = null;
            string cur_service = null;
            while ((cur_service == null) &&
              (DateTime.Now.Subtract(dt).TotalMilliseconds <= 
                        ProcessManager.AppConfig.MaxNpcInteractTime))
            {
                Thread.Sleep(2000);

                fparams = DoGetNpcDialogInfo();
                cur_service = fparams[0];
            }

            if (cur_service == null)
                throw new NpcInteractException();

            return fparams;
        }

    }

    public class NpcInteractException : Exception
    {
        public NpcInteractException()
            : base(string.Format("Unable open NPC dialog after {0:0} sec " +
                " of waiting or it's service unknown", 
                    ProcessManager.AppConfig.MaxNpcInteractTime/1000)) {}
    }
}
