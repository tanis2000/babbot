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
        public static void AcceptQuest(Quest q, bool use_state, string log_facility)
        {
            // Set player current zone
            WowPlayer player = ProcessManager.Player;

            player.setCurrentMapInfo();

            // Get quest giver npc
            NPC npc = q.SrcNpc;
            if (npc == null)
                throw new QuestSkipException(
                    "Quest giver NPC not found for quest '" + q.Name);

            Output.Instance.Log(log_facility, "Located NPC '" + npc.Name +
                "' as quest giver for quest '" + q.Name + "'");

            try
            {
                Output.Instance.Log(log_facility, "Moving to quest giver ... ");
                NpcHelper.MoveToNPC(npc, use_state);
            }
            catch (CantReachNpcException e1)
            {
                throw new QuestSkipException(e1.Message);
            }
            catch (Exception e)
            {
                throw new QuestSkipException("Unable reach NPC. " + e.Message);
            }

            Output.Instance.Debug(log_facility, "Reached the quest giver");
            if (!LuaHelper.TargetUnitByName(npc.Name))
                    throw new QuestSkipException("Unable target NPC");

            try
            {
                NpcHelper.InteractNpc(npc.Name);
            }
            catch (NpcInteractException ne)
            {
                throw new QuestSkipException(ne.Message);
            }

            // Check if quest available
            if (!CheckQuestAvail(q))
                throw new QuestSkipException("NPC doesn't have a quest");

            // After this something must come out but might be slow
            string dinfo;
            do
            {
                Thread.Sleep(2000);
                string[] ret = NpcHelper.GetTargetNpcDialogInfo(q.SrcNpc.Name, 0);
                dinfo = ret[0];
            } while (string.IsNullOrEmpty(dinfo));

            if (!dinfo.Equals("quest_start"))
                throw new QuestSkipException(
                     "NPC doesn't show quest start dialog bug '" + dinfo + "'");


            // Accept quest
            Output.Instance.Debug(log_facility, "Accepting quest ...");
            ProcessManager.Injector.Lua_ExecByName("AcceptQuest");
            // Wait a bit to add it into the log
            Thread.Sleep(1000);

            // Check that quest is in toon log
            if (FindLogQuest(q.Title) < 1)
                 throw new QuestSkipException(
                     "Unable find quest in toon log after it been accepted '");
            
            // Check if quest has objectives
            if (q.Objectives == null)
            {
                // TODO Update quest with objectives
                
                // TODO Generate export file
            }
            Output.Instance.Log(log_facility, "Quest '" + q.Name + "' successfully accepted'");
        }

        /// <summary>
        /// Check if current quest avail
        /// If it on NPC gossip frame than select it
        /// If it already open than we fine
        /// </summary>
        /// <param name="q">Quest</param>
        /// <returns>true if NPC has quest</returns>
        private static bool CheckQuestAvail(Quest q)
        {
            string[] info = NpcHelper.GetTargetNpcDialogInfo(q.SrcNpc.Name);

            string cur_service = info[0];

            if (cur_service == null)
                return false;

            if (cur_service.Equals("gossip"))
            {
                Output.Instance.Debug("char", "GossipFrame opened.");

                Output.Instance.Debug("char", "Looking for quest ...");

                int idx = QuestHelper.FindGossipQuestIdByTitle(q.Title);
                if (idx < 0)
                    return false;

                // Selecting the quest
                Output.Instance.Debug("char", "Selecting quest by Id: " + idx);
                ProcessManager.Injector.Lua_ExecByName(
                    "SelectGossipAvailableQuest", new string[] { Convert.ToString(idx) });

                // TODO wait for QuestFrame open
                return true;
            }
            else if (cur_service.Equals("quest_start"))
            {
                // Check open quest title
                // TODO
                Output.Instance.Debug("Parsing quest info line '" + info[1] + "'");
                string[] headers = info[1].Split(new string[] { "::" }, StringSplitOptions.None);
                if (headers.Length < 2)
                    return false;
                string title = headers[1];
                return (!string.IsNullOrEmpty(title) && title.Equals(q.Title));
            }
            else
                // Quest not found nor on gossip frame nor on active frame
                return false;
        }

        /// <summary>
        /// Check if quest in toon log
        /// </summary>
        /// <param name="q">Quest Title</param>
        /// <returns>
        /// Quest index in toon log (starting from 1) 
        /// or -1 if quest not found
        /// </returns>
        public static int FindLogQuest(string title)
        {
            string[] ret = ProcessManager.Injector.Lua_ExecByName("FindLogQuest", 
                new string[] {title});
 
            // Trying convert result
            int idx = -1;
            try
            {
                idx = Convert.ToInt32(ret[0]);
            }
            catch {}

            return idx;
        }
        
        public static string[] GetQuestObjectives(Quest q)
        {
            string[] ret = ProcessManager.Injector.Lua_ExecByName("GetQuestObjectives", 
                new string[] {q.Title});

            return ret;
        }
    }
}
