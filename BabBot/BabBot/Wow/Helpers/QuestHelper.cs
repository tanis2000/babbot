using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BabBot.Manager;
using BabBot.Common;
using System.Threading;
using System.Text.RegularExpressions;

namespace BabBot.Wow.Helpers
{
    #region Quest Reguest

    public abstract class AbstractQuestReq
    {
        public int NpcId;
        public string ActionName = null;
        public string QuestStatus = null;
        public Quest Quest;
        public string LogFs = null;
        public string NpcDestText = null;

        internal abstract bool DoBeforeStart();

        public AbstractQuestReq(Quest q, string lfs)
        {
            Quest = q;
            LogFs = lfs;
        }

        internal abstract void DoAfterAction();
    }

    internal class AcceptQuestReq : AbstractQuestReq
    {
        public AcceptQuestReq(Quest q, string lfs)
            :base (q, lfs)
        {
            NpcId = 0;
            ActionName = "Accept";
            NpcDestText = "giver";
            QuestStatus = "quest_start";
        }

        internal override bool DoBeforeStart()
        {
            // Check if quest already in quest log
            if (QuestHelper.FindLogQuest(Quest.Title, LogFs) > 0)
            {
                Output.Instance.Log(LogFs, "Quest already accepted");
                return false;
            }

            return true;
        }

        internal override void DoAfterAction()
        {
            // Check that quest is in toon log
            if (QuestHelper.FindLogQuest(Quest.Title, LogFs) == 0)
                throw new QuestSkipException(
                    "Unable find quest in toon log after it been accepted '");
        }
    }

    internal class DeliverQuestReq : AbstractQuestReq
    {
        public DeliverQuestReq(Quest q, string lfs)
            :base (q, lfs)
        {
            NpcId = 1;
            ActionName = "Deliver";
            NpcDestText = "receiver";
            QuestStatus = "quest_end";
        }

        internal override bool DoBeforeStart()
        {
            // Check if quest already in quest log
            int idx = QuestHelper.FindLogQuest(Quest.Title, LogFs);
            if (idx == 0)
            {
                Output.Instance.Log(LogFs, "Quest not in a log list");
                return false;
            }

            // Check if quest completed
            if (!QuestHelper.IsQuestLogCompleted(idx))
            {
                Output.Instance.Log(LogFs, "Quest not is not completed");
                return false;
            }

            return true;
        }

        internal override void DoAfterAction()
        {
            // Check that quest is in toon log
            if (QuestHelper.FindLogQuest(Quest.Title, LogFs) > 0)
                throw new QuestSkipException(
                    "Quest still in toon's quest log after it been delivered");
        }
    }

    #endregion Quest Request

    #region Quest Processing Exceptions

    /// <summary>
    /// Define exception happened during quest processing
    /// </summary>
    public class QuestProcessingException : Exception
    {
        public QuestProcessingException(string msg) :
            base(msg) { }
    }

    /// <summary>
    /// Generate exception that causes bot abandom quest
    /// </summary>
    public class QuestSkipException : Exception
    {
        public QuestSkipException(string msg) :
            base(msg + ". Skipping the quest") { }
    }

    #endregion

    public static class QuestHelper
    {
        public static Regex ItemPattern = new Regex("^(.*): (/d+)/(d+)$");

        /// <summary>
        /// Get quest list from opened Gossip dialog
        /// </summary>
        /// <returns></returns>
        public static string[] GetAvailGossipQuests()
        {
            // Get list of quests
            string[] res = ProcessManager.
                Injector.Lua_ExecByName("GetAvailGossipQuests");
            return res;
        }

        /// <summary>
        /// Find quest ID on NPC opened Gossip dialog
        /// </summary>
        /// <param name="title">Quest title</param>
        /// <returns>Quest Id or -1 if quest not found</returns>
        public static int FindGossipQuestIdByTitle(string title)
        {
            string[] qinfo = GetAvailGossipQuests();

            int max_num = (int)(qinfo.Length / 3);
            for (int i = 0; i < max_num; i++)
            {
                string t = qinfo[i * 3];
                if ((t != null) && t.Equals(title))
                    return i + 1;
            }
            return -1;
        }

        private static void ProcessQuestRequest(AbstractQuestReq req, 
                                                        bool use_state)
        {
            if (!req.DoBeforeStart())
                return;

            // Set player current zone
            WowPlayer player = ProcessManager.Player;

            player.setCurrentMapInfo();

            string lfs = req.LogFs;
            string qt = req.Quest.Title;

            // Get quest npc
            NPC npc = req.Quest.NpcList[req.NpcId];
            if (npc == null)
                throw new QuestSkipException(
                    "Quest " + req.NpcDestText + " NPC not found for quest '" + qt);

            Output.Instance.Log(lfs, "Located NPC '" + npc.Name +
                "' as quest " + req.NpcDestText + " for quest '" + qt + "'");

            try
            {
                Output.Instance.Log(lfs, "Moving to quest " + 
                                    req.NpcDestText + " ... ");
                NpcHelper.MoveToNPC(npc, use_state, lfs);
            }
            catch (CantReachNpcException e1)
            {
                throw new QuestSkipException(e1.Message);
            }
            catch (Exception e)
            {
                throw new QuestSkipException("Unable reach NPC. " + e.Message);
            }

            Output.Instance.Debug(lfs, "Reached the quest " + 
                                            req.NpcDestText + ".");
            if (!LuaHelper.TargetUnitByName(npc.Name))
                    throw new QuestSkipException("Unable target NPC");

            string[] dinfo;
            try
            {
                dinfo = NpcHelper.InteractNpc(npc.Name, false, lfs);
            }
            catch (NpcInteractException ne)
            {
                throw new QuestProcessingException(ne.Message);
            } 
            catch (Exception e)
            {
                throw new Exception("Unable Interact with NPC." +
                        e.Message);
            }

            // If NPC has a single quest it can be opened already
            // null dinfo[0] cause NpcInteractException
            if (!dinfo[0].Equals(req.QuestStatus))
            {
                // Check if quest available
                try
                {
                    if (!CheckQuestAvail(req.Quest, dinfo, lfs))
                        throw new QuestSkipException("NPC doesn't have a quest");
                }
                catch (Exception e)
                {
                    throw new QuestSkipException(e.Message);
                }
            }

            // Do QuestAction quest
            Output.Instance.Debug(lfs, req.ActionName + "ing quest ...");
            ProcessManager.Injector.Lua_ExecByName(req.ActionName + "Quest");
            // Wait a bit to update log
            Thread.Sleep(2000);

            req.DoAfterAction();

            Output.Instance.Log(lfs, "Quest '" + qt + 
                "' successfully " + req.ActionName.ToLower() + "ed'");

        }

        /// <summary>
        /// Accept currently opened quest
        /// </summary>
        /// <param name="q">Quest</param>
        /// <param name="use_state">Test flag for movement</param>
        public static void AcceptQuest(Quest q, bool use_state, string lfs)
        {
            ProcessQuestRequest(new AcceptQuestReq(q, lfs), use_state);
        }

        /// <summary>
        /// Check if current quest avail
        /// If it on NPC gossip frame than select it
        /// If it already open than we fine
        /// </summary>
        /// <param name="q">Quest</param>
        /// <returns>true if NPC has quest</returns>
        private static bool CheckQuestAvail(Quest q, string[] dinfo, string lfs)
        {
            string cur_service = null;

            if (dinfo == null)
            {
                dinfo = NpcHelper.GetTargetNpcDialogInfo(q.SrcNpc.Name, false, lfs);
                cur_service = dinfo[0];
            } else
                cur_service = dinfo[0];

            if (cur_service.Equals("gossip"))
            {
                Output.Instance.Debug(lfs, "GossipFrame opened.");

                Output.Instance.Debug(lfs, "Looking for quest ...");

                int idx = QuestHelper.FindGossipQuestIdByTitle(q.Title);
                if (idx < 0)
                    return false;

                // Selecting the quest
                Output.Instance.Debug(lfs, "Selecting quest by Id: " + idx);
                ProcessManager.Injector.Lua_ExecByName(
                    "SelectGossipAvailableQuest", new string[] { Convert.ToString(idx) });

                // Wait for quest frame pop up
                try
                {
                    NpcHelper.WaitDialogOpen("Quest", lfs);
                }
                catch (NpcInteractException ne)
                {
                    throw new QuestProcessingException(ne.Message);
                }
                catch (Exception e)
                {
                    throw new QuestProcessingException(
                        "NPC doesn't show QuestFrame. " + e.Message);
                }

                // Call itself again to parse the quest
                return CheckQuestAvail(q, null, lfs);

            }
            else if (cur_service.Equals("quest_start"))
            {
                Output.Instance.Debug("Parsing quest info line '" + dinfo[1] + "'");
                string[] headers = dinfo[1].Split(new string[] { 
                                        "::" }, StringSplitOptions.None);

                string title = headers[1];
                return (!string.IsNullOrEmpty(title) && title.Equals(q.Title));
            }
            else
                // Quest not found nor on gossip frame nor on active frame
                throw new QuestProcessingException(
                    "Quest not found nor on GossipFrame nor on ActiveFrame");
        }

        /// <summary>
        /// Check if quest in toon log
        /// </summary>
        /// <param name="q">Quest Title</param>
        /// <returns>
        /// Quest index in toon log (starting from 1) 
        /// or -1 if quest not found
        /// </returns>
        public static int FindLogQuest(string title, string lfs)
        {
            Output.Instance.Debug(lfs, "Looking for quest index in toon quest log ...");
            string[] ret = ProcessManager.Injector.Lua_ExecByName("FindLogQuest", 
                new string[] {title});
 
            // Trying convert result
            int idx = 0;
            try
            {
                idx = Convert.ToInt32(ret[0]);
            }
            catch {}

            if (idx > 0)
                Output.Instance.Debug(lfs, "Quest located with index: " + idx);
            else
                Output.Instance.Debug(lfs, "Quest not found.");
            return idx;
        }
        
        public static string[] GetQuestObjectives(Quest q)
        {
            string[] ret = ProcessManager.Injector.Lua_ExecByName("GetQuestObjectives", 
                new string[] {q.Title});

            return ret;
        }

        public static void AbandonQuest(Quest q, string lfs)
        {
            // Find quest id in toon log
            Output.Instance.Log(lfs, "Abandoning Quest '" + q.Title + "' ... ");
            int idx = FindLogQuest(q.Title, lfs);
            if (idx == 0)
            {
                Output.Instance.Log(lfs, "Not found in toon's quest log");
                return;
            }

            // Open QuestLogFrame
            ProcessManager.Injector.Lua_ExecByName(
                        "ToggleFrame", new string[] { "QuestLog" });
            NpcHelper.WaitDialogOpen("QuestLog", lfs);

            string[] ret = ProcessManager.
                    Injector.Lua_ExecByName("SelectAbandonQuest", 
                        new string[] { Convert.ToString(idx) });
            string aq_name = ret[0];

            if (string.IsNullOrEmpty(aq_name))
                throw new QuestProcessingException(
                    "Abandoning preparation procedure didn't return quest name");
            else if (!aq_name.Equals(q.Title))
                throw new QuestProcessingException("Abandoned quest '" + 
                    q.Title + "' is different from expected '" + q.Title + "'.");
            else
            {
                // Wait to get selection activated
                Thread.Sleep(2000);
                ProcessManager.Injector.Lua_ExecByName("AbandonQuest");
                // Wait to get abandon action activated
                Thread.Sleep(2000);

                if (FindLogQuest(q.Title, lfs) == 0)
                    Output.Instance.Log(lfs, "Quest '" + 
                    q.Title + "' removed from toon logs");
                else
                    throw new QuestProcessingException("Abandoned quest '" + 
                        q.Title + "' is still in toon quest log after abandon action.");
            }
        }

        public static void DeliverQuest(Quest q, string lfs, bool use_state)
        {
            ProcessQuestRequest(new DeliverQuestReq(q, lfs), use_state);
        }

        private static string[] GetQuestObjectives(int quest_idx)
        {
            Output.Instance.Debug("xxx", "Checking Quest Objectives ... ");

            string[] ret = ProcessManager.Injector.Lua_ExecByName(
                "GetQuestObjectives", 
                new string[] { Convert.ToString(quest_idx) });

            string[] obj = new string[0];

            if (!string.IsNullOrEmpty(ret[0]))
                obj = ret[0].Split(new string[] { "::" },
                                        StringSplitOptions.None);

            if (obj.Length > 0) 
                Output.Instance.Debug("xxx", "Quest has " + obj.Length + " objectives");
            else
                Output.Instance.Debug("xxx", "Quest has no objectives");

            return obj;
        }

        public static bool IsQuestLogCompleted(int idx)
        {
            
            // Check if quest completed
            string[] obj = QuestHelper.GetQuestObjectives(idx);

            bool f = true;
            for (int i = 0; i < obj.Length; i++)
            {
                string[] items = obj[i].Split(',');
                if (string.IsNullOrEmpty(items[2]) || !items[2].Equals("1"))
                    return false;
            }

            return true;
        }

    }
}
