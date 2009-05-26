/*
  This file is part of PPather.

    PPather is free software: you can redistribute it and/or modify
    it under the terms of the GNU Lesser General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    PPather is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public License
    along with PPather.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;
using System.Text;

using Glider.Common.Objects;
using System.Text.RegularExpressions;

/*
 * Contributor(s): toblakai
 */ 
namespace Pather.Helpers
{
    // this class must be instantiated in the event that 2 independent things
    // want to check out the buffs you wouldn't want to use the same
    // BuffSnap object

    static class ChannelLogs
    {
        private static int BUFSIZE = 40;
        private static String[] ChatBuffer = new String[40];
        private static String[] CombatBuffer = new String[40];

        private static String lastKillerPlayer = "";
        private static String lastKillerMob = "";

        private static List<String> LootBuffer = new List<String>();
        private static List<String> LastLoot = new List<String>();
        private static int _LootAmount = 0;
        private static bool NewLoot = false;
        private static long LootID = -1;

        private static int chatPos = 0;
        private static int combatPos = 0;

        static Regex receiveNewLootPattern = null;
        static Regex durabilityLossPattern = null;
        static Regex playerDiedPattern = null;
        static Regex detectFatalityPattern = null;
        static String playerName = null;

        public static void init()
        {
            PPather.WriteLine("ChannelLogs.init()");
            playerName = GPlayerSelf.Me.Name;
            
            //GContext.Main.ChatLog += new GContext.GChatLogHandler(ChannelLogs_ChatLog);
            
            if (ChatBuffer == null)
                ChatBuffer = new String[BUFSIZE];
            if (CombatBuffer == null)
                CombatBuffer = new String[BUFSIZE];
                
            // * pattern for loot detection
            receiveNewLootPattern = new Regex("You receive loot: \\[(?<item>.*)\\].");
            // * patterns for killer detection
            durabilityLossPattern = new Regex("^Your positive_inner_check items suffer a 10.*");
            playerDiedPattern = new Regex(playerName + " died.*");
            detectFatalityPattern = new Regex("(?<killer>[^']+).*(?<player>" + playerName + "+)");
        }

        ///// <summary>
        ///// Shutdown and clean up.
        ///// </summary>
        //public static void ShutDown()
        //{
        //    PPather.WriteLine("ChannelLogs.ShutDown()");

        //    /// 
        //    /// Remove event reference.
        //    /// 
        //    //GContext.Main.ChatLog -= new GContext.GChatLogHandler(ChannelLogs_ChatLog);
        //}


        //public static void ChannelLogs_ChatLog(String rawText, String ParsedText)
        //{
        //    // put log msg in buffer
        //    //ChannelLogs.putChat(ParsedText);
        //    if (ParsedText != null)
        //    {
        //        ChatBuffer[chatPos] = ParsedText;
        //        chatPos = ++chatPos % BUFSIZE;
        //    }
        //}

        //public static void reset()
        //{
        //    ChatBuffer = new String[BUFSIZE];
        //    CombatBuffer = new String[BUFSIZE];
        //    chatPos = 0;
        //    combatPos = 0;
        //    lastKillerMob = "";
        //    lastKillerPlayer = "";
        //}

        public static void putChat(String ParsedText)
        {
            if (ParsedText != null)
            {
                //PPather.WriteLine(String.Format("ChannelLogs: putChat({0})", ParsedText));
                ChatBuffer[chatPos] = ParsedText;
                chatPos = ++chatPos % BUFSIZE;
            }
        }

        public static void putCombat(String msg)
        {
            if (msg == null)
                return;

            //PPather.WriteLine("setting CombatLog[" + combatPos + "] to " + msg);
            CombatBuffer[combatPos] = msg;
            combatPos = ++combatPos % BUFSIZE;
          
            // check if we got killed
            if (regExp(msg, playerDiedPattern) != null)
                setLastKiller();
        }

        public static Boolean setLoots(int posBeforeLoot)
        {
            NewLoot = false;
            String[] tmp = null;
            if (posBeforeLoot > chatPos)
            {
                _LootAmount = BUFSIZE - (posBeforeLoot - chatPos);
                //PPather.WriteLine(String.Format("ChannelLogs: LootAmount = {0}", _LootAmount));
            }
            else
            {
                _LootAmount = chatPos - posBeforeLoot;
                //PPather.WriteLine(String.Format("ChannelLogs: LootAmount = {0}", _LootAmount));
            }
            if (_LootAmount != 0)
            {
                if (LootID < 0) LootID = 0;
                tmp = getNChatLines(_LootAmount);
            }
            else
                return false;

            //if(tmp.Length > 0)
            //    LastLoot.Clear();

            for (int i = 0; i < tmp.Length; i++)
            {
                String line = tmp[i];
                if ((line == "") || (line == null)) continue;
                Match m = receiveNewLootPattern.Match(line);
                if (m.Success)
                {
                    string item = m.Groups["item"].Value;
                    //PPather.WriteLine(String.Format("ChannelLogs: item = {0}", item));
                    NewLoot = true;
                    LootID++;
                    try
                    {
                        LastLoot.Add(item);
                        LootBuffer.Add(item);
                        PPather.WriteLine("ChannelLogs: Added " + item + " to Loot Buffers");
                        GContext.Main.Debug("ChannelLogs: Added " + item + " to Loot Buffers");
                    }
                    catch (ArgumentException)
                    {
                        //PPather.WriteLine("ChannelLogs: " + item + " already added to Loot Buffers (skipping)");
                    }
                }
            }
            return NewLoot;
        }

        public static bool HasLoot()
        {
            return NewLoot;
        }

        public static long getLootID()
        {
            return LootID;
        }

        public static List<String> getLastLoot()
        {
            return LastLoot;
        }

        public static void clearLastLoot()
        {
            LastLoot.Clear();
        }

        public static List<String> getLoot()
        {
            return LootBuffer;
        }

        public static void clearLoot()
        {
            LootBuffer.Clear();
        }


        public static int getBufSize() { return BUFSIZE; }
        public static int getChatPosition() { return chatPos; }
        public static int getCombatPosition() { return combatPos; }

        public static String getLastKillerPlayer() { return lastKillerPlayer; }
        public static String getLastKillerMob() { return lastKillerMob; }

        public static String getLastChatLine() { return ChatBuffer[(chatPos-1)%BUFSIZE]; }
        public static String getLastCombatLine() { return CombatBuffer[(combatPos-1)%BUFSIZE]; }

        public static String getChatLine(int pos)
        {
            int offset = (chatPos - 1 - pos) % BUFSIZE;
            if (offset < 0) offset += BUFSIZE;
            //PPather.WriteLine("ChannelLogs -> getChatLine: (pos,offset,chatPos) = (" + pos+","+offset+","+chatPos+ ")");
            return ChatBuffer[offset];
        }

        public static String getCombatLine(int pos)
        {
            int offset = (combatPos - 1 - pos) % BUFSIZE;
            if (offset < 0) offset += BUFSIZE;
            //PPather.WriteLine("ChannelLogs -> getCombatLine: (pos,offset,combatPos) = (" + pos + "," + offset + "," + combatPos + ")");
            return CombatBuffer[offset];
        }

        public static String[] getNChatLines(int amount)
        {
            //PPather.WriteLine("ChannelLogs -> getNChatLines: (amount,chatPos) = (" + amount+ "," +chatPos+")");
            if (ChatBuffer.Length < 1) return null;
            if (amount > BUFSIZE) amount = BUFSIZE;

            String[] msgs = new String[amount];
            for (int i = 0; i < amount; i++)
                msgs[i] = getChatLine(i);

            return msgs;
        }

        public static String[] getNCombatLines(int amount)
        {
            //PPather.WriteLine("ChannelLogs -> getNCombatLines; (amount,combatPos) = (" + amount + ","+combatPos+")");
            if (CombatBuffer.Length < 1) return null;
            if (amount > BUFSIZE) amount = BUFSIZE;

            String[] msgs = new String[amount];
            for (int i = 0; i < amount; i++)
                msgs[i] = getCombatLine(i);
            return msgs;
        }

        public static String[] getAllChatLines()
        {
            return getNChatLines(BUFSIZE);
        }

        public static String[] getAllCombatLines()
        {
            return getNCombatLines(BUFSIZE);
        }

        private static void setLastKiller()
        {
            int offset = 0;
            int startOffset = 1;

            //// check for 10% durability loss to see if it was PVP combat or not
            //Match durability = regExp(getCombatLine(startOffset), durabilityLossPattern);
            //for (offset = 1; offset < BUFSIZE - startOffset; offset++)
            //{
            //    if (durability != null)
            //        if (durability.Success)
            //        {
            //            startOffset = 2;
            //            break;
            //        }
            //    durability = regExp(getCombatLine(startOffset + offset), durabilityLossPattern);
            //}

            //// reset offset
            //offset = 0;

            //// find killer
            //Match fatality = regExp(getCombatLine(startOffset + offset), detectFatalityPattern);
            //for (offset = 1; offset < BUFSIZE - startOffset; offset++)
            //{
            //    // find the last combat line that
            //    // 1. contains someone doing something to our toon
            //    // 2. is not our toon doing something to self
            //    if (fatality != null)
            //        if (fatality.Success)
            //            if (fatality.Groups["killer"].Value.CompareTo(playerName) != 0)
            //                break;
            //    fatality = regExp(getCombatLine(startOffset + offset), detectFatalityPattern);
            //}

            //if (fatality == null || (offset >= BUFSIZE - startOffset && !fatality.Success))
            //    // didn't find any good combat line after all. Just skip it this time.
            //    return;

            //// we most likely found our bane
            //String killer = fatality.Groups["killer"].Value;
            //if (startOffset == 2)
            //{
            //    // killed by mob
            //    lastKillerMob = killer;
            //    PPather.WriteLine("I was killed by " + killer + " (a mob) :(");
            //}
            //else
            //{    // killed by player
            //    lastKillerPlayer = killer;
            //    PPather.WriteLine("I was killed by " + killer + " (in PVP) :(");
            //}

            // print out combat history for debugging
            //for (int i = 7; i >= 0; i--)
            //    PPather.WriteLine("CombatHistory(-" + i + "): " + getCombatLine(i));
        }


        public static Match regExp(String s, Regex p)
        {
            Match m = null;
            if (s == null) return m;
            if (p == null) return m;
            m = p.Match(s);
            if (m.Success)
                return m;
            return null;
        }

    }
}
