using System;
using System.Collections.Generic;
using System.Text;
using BabBot.Common;
using BabBot.Manager;
using System.Threading;
using BabBot.Scripts.Common;
using Pather.Graph;
using System.Text.RegularExpressions;

namespace BabBot.Wow.Helpers
{
    public class NpcInteractException : Exception
    {
        public NpcInteractException()
            : base(string.Format("Unable open NPC dialog after {0:0} sec " +
                " of waiting or it's service(s) unknown",
                    ProcessManager.AppConfig.MaxNpcInteractTime / 1000)) { }
    }

    public class NpcProcessingException : Exception
    {
        public NpcProcessingException(string msg)
            : base(msg) { }
    }
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
        /// Interact with NPC if no Gossip frame open.
        /// Return type of open frame.
        /// If NPC has 1 quest or 1 service (vendor for ex) it might go to that frame directly
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

        /// <summary>
        /// Move character to destination
        /// </summary>
        /// <param name="dest"></param>
        /// <param name="use_state">Test flag</param>
        public static bool MoveToDest(Vector3D dest, bool use_state)
        {
            WowPlayer player = ProcessManager.Player;

            float distance = dest.GetDistanceTo(player.Location);

            // We have a 3 tries by default to reach target NPC
            int retry = 0;
            int max_retry = ProcessManager.AppConfig.MaxTargetGetRetries;

            // We might be already at the target
            while ((distance > 5F) && (retry <= max_retry))
            {
                if (retry > 0)
                    Output.Instance.Log("Retrying " + retry + " of " + 
                                max_retry + " to reach the destination");
                if (use_state)
            {
                // Use MoveToState
                MoveToState mt = new MoveToState(dest, 5F);
                ProcessManager.Player.StateMachine.SetGlobalState(mt);
                Output.Instance.Debug("char", "State: " +
                    ProcessManager.Player.StateMachine.CurrentState);

                Output.Instance.Log("char", "NPC coordinates located. Moving to NPC ...");
                ProcessManager.Player.StateMachine.IsRunning = true;

                Output.Instance.Debug("char", "State: " +
                    ProcessManager.Player.StateMachine.CurrentState);
                while (ProcessManager.Player.StateMachine.IsInState(typeof(MoveToState)))
                {
                    Thread.Sleep(1000);
                    Output.Instance.Debug("char", "State: " +
                        ProcessManager.Player.StateMachine.CurrentState);
                }
            }
            else
            {
                // Click to move on each waypoint
                // dynamically calculate time between each click 
                // based on distance to the next waypoint

                // Lets calculate path from character to NPC and move
                Path p = ProcessManager.Caronte.CalculatePath(
                    WaypointVector3DHelper.Vector3DToLocation(ProcessManager.Player.Location),
                    WaypointVector3DHelper.Vector3DToLocation(dest));

                // Travel path
                int max = p.Count();
                Vector3D vprev = dest;
                Vector3D vnext;
                for (int i = 0; i < max; i++)
                {
                    vnext = WaypointVector3DHelper.LocationToVector3D(p.Get(i));

                    // Calculate travel time
                    distance = vprev.GetDistanceTo(vnext);
                    int t = (int)((distance / 7F) * 1000);

                    ProcessManager.Player.ClickToMove(vnext);
                    Thread.Sleep(t);
                    vprev = vnext;
                    }
                }

                distance = dest.GetDistanceTo(player.Location);

                if (distance < 5F)
                    retry++;
            }

            return (dest.GetDistanceTo(player.Location) < 5F);
        }

        /// <summary>
        /// Check list of available NPC gossip options and click on each
        /// </summary>
        /// <param name="npc">NPC</param>
        /// <returns>true if NPC has any service</returns>
        private static bool FindAvailGossips(NPC npc)
        {
            // Get list of options
            string[] opts = ProcessManager.
                Injector.Lua_ExecByName("GetGossipOptions");
            if (opts == null || (opts.Length == 0))
                Output.Instance.Debug("No service detected.");
            else
            {
                Output.Instance.Debug((int)(opts.Length / 2) + " service(s) detected.");

                // Parse list of services
                int max_opts = (int)(opts.Length / 2);
                for (int i = 0; i < max_opts; i++)
                {
                    string gossip = opts[i * 2];
                    string service = opts[i * 2 + 1];

                    SelectNpcOption(npc, "SelectGossipOption", i + 1, max_opts);
                }
            }

            return true;
        }

        public static bool SelectNpcOption(NPC npc, string lua_fname, int idx, int max)
        {
            // Choose gossip option and check npc again
            ProcessManager.Injector.Lua_ExecByName(
                lua_fname, new string[] { Convert.ToString(idx) });
            if (!AddTargetNpcInfo(npc))
                return false;

            // After gossip option processed interact with npc again
            // if (idx < max)
            // NpcHelper.InteractNpc(npc.Name);

            return true;
        }

        #region Add NPC

        private class QuestHeader
        {
            internal string Name;
            internal int Level;

            public QuestHeader(string name, int level)
            {
                Name = name;
                Level = level;
            }
        }

        private static QuestHeader qh;

        public static bool AddNpc()
        {
            // Initialize parameters
            qh = null;

            string npc_name = ProcessManager.Player.CurTarget.Name;
            ProcessManager.Player.setCurrentMapInfo();

            string[] npc_info = ProcessManager.Injector.
                Lua_ExecByName("GetUnitInfo", new object[] { "target" });

            Output.Instance.Log("Checking NPC Info ...");
            NPC npc = new NPC(ProcessManager.Player, npc_info[3], npc_info[4]);

            if (!AddTargetNpcInfo(npc))
                return false;

            // Check if NPC already exists
            NPC check = ProcessManager.
                CurWoWVersion.NPCData.FindNpcByName(npc.Name);
            if ((check != null))
            {
                if (npc.Equals(check))
                {
                    throw new NpcProcessingException("NPC '" + npc.Name +
                        "' already added with identicall parameters");
                    return false;
                }
                else
                {
                    // NPC in database but with different parameters
                    check.MergeWith(npc);
                }
            }
            else
                ProcessManager.CurWoWVersion.NPCData.Add(npc);

            if (ProcessManager.SaveNpcData())
                Output.Instance.Log("npc", "NPC '" + npc_name +
                        "' successfully added to NPCData.xml");
            return true;
        }

        /// <summary>
        /// Check list of quest available on NPC gossip window and click on each
        /// to assign with NPC
        /// </summary>
        /// <param name="npc"></param>
        /// <returns></returns>
        private static bool AddAvailQuests(NPC npc)
        {
            Output.Instance.Debug("npc", "Checking available quests ...");

            // Get list of quests
            string[] quests = QuestHelper.GetAvailGossipQuests();

            if (quests == null || (quests.Length == 0))
                Output.Instance.Debug("npc", "No quests detected.");
            else
            {
                Output.Instance.Debug("npc", (int)(quests.Length / 3) + " quests(s) detected.");

                // Parse list of quests
                int max_num = (int)(quests.Length / 3);
                for (int i = 0; i < max_num; i++)
                {
                    string sqlevel = quests[i * 3 + 1];

                    try
                    {
                        qh = new QuestHeader(quests[i * 3], Convert.ToInt32(sqlevel));

                        // Last parameter we not interested in
                        Output.Instance.Debug("npc", "Adding quest '" +
                                                    qh.Name + "'; Level: " + qh.Level);

                        SelectNpcOption(npc, "SelectAvailableQuest", i + 1, max_num);
                    }
                    catch (Exception e)
                    {
                        Output.Instance.LogError("npc", "Failed convert quests level '" +
                        sqlevel + "' to integer. " + e.Message);
                        return false;
                    }

                }
            }

            return true;
        }

        // This method is recursive
        private static bool AddTargetNpcInfo(NPC npc)
        {
            string[] fparams = NpcHelper.GetTargetNpcDialogInfo(npc.Name);
            if (fparams == null)
            {
                Output.Instance.Log("Unable retrieve NPC gossip information. " +
                        "Does it communicate at all ?");
                return false;
            }

            string cur_service = fparams[0];

            if (cur_service == null)
            {
                Output.Instance.Log("NPC is useless. No services or quests are detected");
                return true;
            }

            if (cur_service.Equals("gossip"))
            {
                Output.Instance.Debug("npc", "GossipFrame opened.");

                Output.Instance.Debug("npc", "Checking available options ...");
                // Get number of gossips and quests
                string[] opts = ProcessManager.
                    Injector.Lua_ExecByName("GetNpcGossipInfo");

                // Convert result to int format
                int[] opti = new int[opts.Length];
                for (int i = 0; i < opti.Length; i++)
                {
                    try
                    {
                        opti[i] = Convert.ToInt32(opts[i]);
                    }
                    catch (Exception e)
                    {
                        throw new NpcProcessingException("Unable convert " + i + " parameter '" +
                            opts[i] + "' from  'GetNpcGossipInfo' result to integer. " + e);
                        return false;
                    }
                }

                if (opti[0] > 0)
                    if (!AddAvailQuests(npc))
                        return false;

                // TODO CheckActiveQuests
                // if (opti[1] > 0)
                //      if (!FindActiveQuests(npc))
                //            return false;

                if (opti[2] > 0)
                    if (!FindAvailGossips(npc))
                        return false;

            }
            else if (cur_service.Equals("quest_start"))
                AddNpcQuestStart(npc, fparams);
            else if (cur_service.Equals("quest_progress"))
                Output.Instance.Log("Ignoring quest progress. If you need add initial quest " +
                    "than drop it and talk to NPC again");
            else if (cur_service.Equals("quest_end"))
                AddNpcQuestEnd(npc, fparams);
            else
                AddNpcService(npc, cur_service, fparams);

            return true;
        }


        private static void AddNpcQuestStart(NPC npc, string[] opts)
        {
            Quest q = null;

            // Checking parameters first
            if (opts.Length < 4)
            {
                Output.Instance.LogError("npc", "Not enough " +
                        opts.Length + " parameters to add quest start");
                return;
            }

            // Check parsing result
            for (int i = 1; i < 4; i++)
            {
                string s = opts[i];

                if (string.IsNullOrEmpty(opts[i]))
                {
                    Output.Instance.LogError("npc", i +
                        " parameter from quest info result is empty");
                    return;
                }

                // Check each value against pattern
                Regex rx = ProcessManager.CurWoWVersion.QuestConfig.Patterns[i - 1];
                if (!rx.IsMatch(s))
                {
                    Output.Instance.LogError("npc", i +
                        " parameter '" + s + "' from quest scan" +
                        " doesn't match template '" + rx.ToString() + "'");
                    return;
                }
            }

            // Read header
            string[] headers = opts[1].Split(new string[] { "::" }, StringSplitOptions.None);
            string[] info = opts[2].Split(',');
            string[] details = opts[3].Split(new string[] { "||" }, StringSplitOptions.None);

            try
            {
                int qlevel = Convert.ToInt32(headers[0]);
                if (qh != null)
                    qlevel = qh.Level;

                Output.Instance.Log("Assign current NPC as start for quest '" +
                                                                        headers[1] + "'");

                q = new Quest(headers[1], headers[2], headers[3], qlevel,
                    new int[] { Convert.ToInt32(info[3]), Convert.ToInt32(info[4]), 
                        Convert.ToInt32(info[5]) }, details, info[0]);
            }
            catch
            {
                Output.Instance.LogError("npc", "Error creating quest with parameters " +
                    "Header: " + headers[1] + "; Text: " + headers[2] + "; Level: " + headers[3]);
                return;
            }

            if (q != null)
                npc.AddQuest(q);
        }

        private static void AddNpcService(NPC npc, string cur_service, string[] opts)
        {
            NPCService npc_service = null;

            switch (cur_service)
            {
                case "class_trainer":
                    npc_service = new ClassTrainingService(ProcessManager.Player.CharClass);
                    break;

                case "trade_skill_trainer":
                    npc_service = new TradeSkillTrainingService("");
                    break;

                case "vendor":
                    npc_service = new VendorService(
                            ((opts[1] != null) && opts[1].Equals("1")),
                            ((opts[2] != null) && opts[2].Equals("1")),
                            ((opts[3] != null) && opts[3].Equals("1")));
                    break;
                case "wep_skill_trainer":
                case "taxi":
                case "banker":
                case "battlemaster":
                    npc_service = new NPCService(cur_service);
                    break;

                default:
                    Output.Instance.Log("npc", "Unknown npc service type '" +
                                cur_service + "'");
                    break;
            }

            if (npc_service != null)
                npc.AddService(npc_service);
        }

        private static void AddNpcQuestEnd(NPC npc, string[] opts)
        {
            if ((opts.Length < 2) || (opts[1] == null))
            {
                Output.Instance.LogError("npc", "Not enough " +
                        opts.Length + " parameters to add quest end");
                return;
            }

            string qtitle = opts[1];

            // Now find NPC who has an original quest

            Quest q = ProcessManager.CurWoWVersion.NPCData.FindQuestByTitle(qtitle);

            if (q != null)
            {
                Output.Instance.Log("Assign current NPC as end for quest '" +
                                                                qtitle + "'");

                q.DestNpc = npc.Name;
            }
        }

        #endregion Add NPC end 
    }
}
