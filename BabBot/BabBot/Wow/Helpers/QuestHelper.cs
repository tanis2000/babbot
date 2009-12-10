using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BabBot.Manager;
using BabBot.Common;

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
    public class QuestSkippingException : Exception
    {
        public QuestSkippingException(string msg) :
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
            if (qinfo == null || (qinfo.Length == 0))
                return -1;

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
        public static void AcceptQuest(Quest q, bool use_state)
        {
            // Set player current zone
            WowPlayer player = ProcessManager.Player;

            player.setCurrentMapInfo();

            // Get quest giver npc
            NPC npc = q.SrcNpc;
            if (npc == null)
                throw new QuestSkippingException("Quest giver NPC not found for quest '" +
                    q.Name + "'.");

            Output.Instance.Log("char", "Located NPC '" + npc.Name +
                "' as quest giver for quest '" + q.Name + "'");

            Output.Instance.Log("char", "Checking NPC coordinates ...");

            // Check if NPC has coordinates in the same continent
            ContinentId c = npc.Continents.FindContinentById(player.ContinentID);
            if (c == null)
                throw new QuestSkippingException("Quest giver NPC for quest '" +
                    q.Name + "' located on different continent.");

            // Check if NPC located in the same zone
            Zone z = c.FindZoneByName(player.ZoneText);

            if (z == null)
                throw new QuestSkippingException("Quest giver NPC for quest '" +
                    q.Name + "' located on different zone. " +
                    "Multi-zone traveling not implemented yet.");

            // Check if NPC has any waypoints assigned
            if (z.Items.Length == 0)
                throw new QuestSkippingException("Quest giver NPC for quest '" +
                    q.Name + "' doesn't have any waypoints assigned. " +
                    "Check NPCData.xml and try again.");

            // By default check first waypoint
            Vector3D npc_loc = z.Items[0];
            Output.Instance.Debug("char", "Usning NPC waypoint: " + npc_loc);

            // TODO add retries
            if (!NpcHelper.MoveToDest(npc_loc, use_state))
                throw new QuestProcessingException("Unable reach NPC");
            
            Output.Instance.Log("char", "Bot has reached the destination");
            if (!LuaHelper.TargetUnitByName(npc.Name))
                    throw new QuestProcessingException("Unable target NPC");

            NpcHelper.InteractNpc(npc.Name);

            // Check if quest available
            if (!CheckQuestAvail(q))
                throw new QuestSkippingException("NPC doesn't have quest.");

            // Accept quest
            ProcessManager.Injector.Lua_ExecByName("AcceptQuest");
            // Check quest in toon log
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


    }
}
