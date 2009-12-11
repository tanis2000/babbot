using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BabBot.Manager;
using BabBot.Common;
using System.Threading;

namespace BabBot.Wow.Helpers
{
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

    public static class QuestHelper
    {
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

        /// <summary>
        /// Accept currently opened quest
        /// </summary>
        /// <param name="q">Quest</param>
        /// <param name="use_state">Test flag for movement</param>
        public static void AcceptQuest(Quest q, bool use_state, string lfs)
        {
            // Set player current zone
            WowPlayer player = ProcessManager.Player;

            player.setCurrentMapInfo();

            // Get quest giver npc
            NPC npc = q.SrcNpc;
            if (npc == null)
                throw new QuestSkipException(
                    "Quest giver NPC not found for quest '" + q.Name);

            Output.Instance.Log(lfs, "Located NPC '" + npc.Name +
                "' as quest giver for quest '" + q.Name + "'");

            // Check if quest already in quest log
            if (FindLogQuest(q.Name, lfs) > 0)
            {
                Output.Instance.Log(lfs, "Quest already accepted");
                return;
            }

            try
            {
                Output.Instance.Log(lfs, "Moving to quest giver ... ");
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

            Output.Instance.Debug(lfs, "Reached the quest giver");
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
            if (!dinfo[0].Equals("quest_start"))
            {
                // Check if quest available
                try
                {
                    if (!CheckQuestAvail(q, dinfo, lfs))
                        throw new QuestSkipException("NPC doesn't have a quest");
                }
                catch (Exception e)
                {
                    throw new QuestSkipException(e.Message);
                }
            }

            // Accept quest
            Output.Instance.Debug(lfs, "Accepting quest ...");
            ProcessManager.Injector.Lua_ExecByName("AcceptQuest");
            // Wait a bit to add it into the log
            Thread.Sleep(1000);

            // Check that quest is in toon log
            if (FindLogQuest(q.Title, lfs) == 0)
                 throw new QuestSkipException(
                     "Unable find quest in toon log after it been accepted '");
            
            // Check if quest has objectives
            if (q.Objectives == null)
            {
                // TODO Update quest with objectives
                
                // TODO Generate export file
            }
            Output.Instance.Log(lfs, "Quest '" + q.Name + "' successfully accepted'");
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
    }
}
