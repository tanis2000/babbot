/*
    This file is part of BabBot.

    BabBot is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    BabBot is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with BabBot.  If not, see <http://www.gnu.org/licenses/>.

    Copyright 2009 BabBot Team
*/
using BabBot.Bot;
using BabBot.Scripting;
using System.IO;
using System.Windows.Forms;
using System;
using System.Collections.Generic;

namespace BabBot.Scripts
{
    /*
     * this class is for loading zf-style bindings files.
     */
    public class ZFClassScript : Paladin, IScript
    {
        //delimiters for defact/precombat/combatseq/lootseq
        private static char[] dels = { ' ', '\t' };
        private static char[] dels2 = { ':','=' };

        private string scriptName;

        private List<string> preCombatList;
        private List<string> combatList;
        private List<string> lootSeqList;
        //private List<string> globalList;

        public ZFClassScript(string script_name)
        {
            scriptName = script_name;
            xxx();
        }

        void IScript.Init()
        {
            xxx();
        }

        void xxx()
        {
            Bindings = new BindingList();
            Actions = new PlayerActionList();

            preCombatList = new List<string>();
            combatList = new List<string>();
            lootSeqList = new List<string>();
            //globalList    = new List<string>();

            FileInfo exe_finfo = new FileInfo(Application.ExecutablePath);
            string full_script_file = Path.Combine(Path.Combine(exe_finfo.DirectoryName, "Scripts"), scriptName);

            Console.WriteLine("ZF: loading " + scriptName + "...");
            StreamReader re = File.OpenText(full_script_file);
            string input = null;
            while ((input = re.ReadLine()) != null)
            {
                //defact= act:sealOfComm slot:6 key:1 cooldown:1.4 prevacttime:30 js:hasNoBuff(20375)&&checkMana(80)
                string trimmed = input.Trim();
                if (trimmed.StartsWith("defact"))
                {
                    parseDefActLine(trimmed);
                }
                else if (trimmed.StartsWith("precombat"))
                {
                    string action_name = parseActionLine(trimmed);
                    if (Actions.ContainsKey(action_name))
                    {
                        preCombatList.Add(action_name);
                    }
                    else
                    {
                        Console.WriteLine("ZF: precombat: unknown action : " + action_name + ", ignored.");
                    }
                }
                else if (trimmed.StartsWith("combatseq"))
                {
                    string action_name = parseActionLine(trimmed);
                    if (Actions.ContainsKey(action_name))
                    {
                        combatList.Add(action_name);
                    }
                    else
                    {
                        Console.WriteLine("ZF: combatseq: unknown action : " + action_name + ", ignored.");
                    }
                }
                else if (trimmed.StartsWith("lootseq"))
                {
                    string action_name = parseActionLine(trimmed);
                    if (Actions.ContainsKey(action_name))
                    {
                        lootSeqList.Add(action_name);
                    }
                    else
                    {
                        Console.WriteLine("ZF: lootseq: unknown action : " + action_name + ", ignored.");
                    }
                }
                //else if (trimmed.StartsWith("globalact"))
                //{
                //    string action_name = parseActionLine(trimmed);
                //    if (Actions.ContainsKey(action_name))
                //    {
                //        globalList.Add(action_name);
                //    }
                //    else
                //    {
                //        Console.WriteLine("ZF: globalact: unknown action : " + action_name + ", ignored.");
                //    }
                //}
            }
            re.Close();
            Console.WriteLine("ZF: loading " + scriptName + " - done.");
        }

        private string parseActionLine(string trimmed)
        {
            //precombat= act:sdw
            //combatseq= act:hdz
            //lootseq= act:sdw

            string action_name = "<invalid>";
            int idx = trimmed.IndexOf("=");
            if (idx > 0)
            {
                trimmed = trimmed.Substring(idx + 1).Trim();
                //trimmed is now just act:sdw
                string[] ttokens = trimmed.Trim().Split(dels2);
                if (ttokens.Length == 2)
                {
                    if ("act".Equals(ttokens[0]))
                    {
                        action_name = ttokens[1].Trim();
                    }
                    else
                    {
                        Console.WriteLine("ZF: invalid line: '" + trimmed + "' - ignored.");
                    }
                }
                else
                {
                    Console.WriteLine("ZF: invalid line: '" + trimmed + "' - ignored.");
                }
            }
            return action_name;
        }

        private void parseDefActLine(string trimmed)
        {
            string actname = null;
            int slot = -1;
            string key = null;
            float reach = 0.0f;
            float cooldown = 0.0f;
            bool self_cast = false;
            bool toggle = false;
            int lifele = -1;
            int lifege = -1;
            int distle = -1;
            int distge = -1;
            float gcd = 0.0f;

            //power: energy,mana,rage...
            //int powle = -1;
            //int powge = -1;
            int idx = trimmed.IndexOf("=");
            if (idx > 0)
            {
                trimmed = trimmed.Substring(idx + 1).Trim();
                //act:sealOfComm slot:6 key:1 cooldown:1.4 prevacttime:30 js:hasNoBuff(20375)&&checkMana(80)
                //now split at spaces
                string[] tokens = trimmed.Split(dels);
                //tokens contains now "act:sealOfComm", "slot:6", "key:1", ...

                foreach (string s in tokens)
                {
                    string[] ttokens = s.Trim().Split(dels2);
                    if (ttokens.Length == 2)
                    {
                        string first = ttokens[0].Trim(); //act
                        string second = ttokens[1].Trim(); //sealOfComm

                        switch (first)
                        {
                            case "act":
                                actname = second;
                                break;
                            case "slot":
                                if ("key".Equals(second))
                                {
                                    //no slot, just key. (f.e. t for attack)
                                    slot = -1;
                                }
                                else
                                {
                                    slot = System.Convert.ToInt32(second);
                                }
                                break;
                            case "key":
                                key = second;
                                break;
                            case "prevacttime":
                                //TODO: check units (seconds, ms?)
                                cooldown = System.Convert.ToSingle(second);
                                break;
                            case "cooldown":
                                //TODO: check units (seconds, ms?)
                                //FIXME: 1.4 is converted to 14.0 !!!
                                gcd = System.Convert.ToSingle(second);
                                break;
                            case "lifele":
                                lifele = System.Convert.ToInt32(second);
                                break;
                            case "lifege":
                                lifege = System.Convert.ToInt32(second);
                                break;
                            case "distle":
                                distle = System.Convert.ToInt32(second);
                                reach = distle;
                                break;
                            case "distge":
                                distge = System.Convert.ToInt32(second);
                                break;

                            default:
                                Console.WriteLine("ZF: token '" + first + "' ignored.");
                                break;
                        }
                    }
                }

                if (actname != null)
                {
                    // 1. slot MUST not be set - actions can work without actionbar ("t" => attack )                    
                    BabBot.Bot.Binding b = new BabBot.Bot.Binding(actname, slot, key);
                    // 2. who needs the Bindings list??? each actiojn has its bbinding...
                    Bindings.Add(b.Name, b);

                    ZFPlayerAction a = new ZFPlayerAction(actname, b, reach, cooldown, self_cast, toggle);
                    a.lifele = lifele;
                    a.lifege = lifege;
                    a.distle = distle;
                    a.distge = distge;
                    a.gcd    = gcd;
                    Actions.Add(a.Name, a);
                }
                else
                {
                    Console.WriteLine("ZF: ignoring '" + trimmed + "' - no act name found.");
                    //no action name => input line input ignored.
                }
            }
            else
            {
                Console.WriteLine("ZF: ignoring '" + trimmed + "' - no = found.");
                // error parsing script_name: bad defact line: input
            }
        }


        //TODO:
        //1. implement global actions (buffs) - check Paladin.cs when it ready, may be this can be done only once for all (sub)classes
        //2. implement precombat/combat/loot sequences (the same as #1 - check Paladin.cs when it is ready)
        //3. ??? support for customer scripting? or just provide some more checkMana/checkBuff/checkTargetisCasting keywords?   

        private void condPlayAction(string action)
        {
            ZFPlayerAction p = (ZFPlayerAction)Actions[action];
            if (p.shouldBeExecuted(player) /*&& p.isReady()*/)
            {
                Console.WriteLine("ZF: invoking " + action);
                player.PlayAction(p);
                if (p.gcd > 0)
                {
                    //sleep zfCooldown seconds 
                }
            }
            else
            {
                Console.WriteLine("ZF: skipping " + action);
            }
        }

        //protected override void OnPostCombat()
        //{
        //    //check all buffs - the same onStart/afterRes/precombat
        //    foreach (string action in globalList)
        //    {
        //        condPlayAction(action);
        //    }
        //}

        protected override void Fight()
        {
            //play once precombat actions
            //for action in precombatactions
            // if action.shouldBeExecuted(player) && action.isReady()
            //   playaction
            foreach (string action in preCombatList)
            {
                condPlayAction(action);
            }


            //while target is not dead and bot is not stopped and player is not dead
            //play combat sequence over and over again
            //
            //while(bot_is_running && have_target && target_is_not_dead)
            //  for action in combatactions
            //    if action.shouldBeExecuted(player) && action.isReady()
            //      playaction
            while (player.HasTarget() /*&& !target.isDead() && bot.isRunning()*/)
            {
                foreach (string action in combatList)
                {
                    condPlayAction(action);
                }
            }
        }
    }

    public class ZFPlayerAction : PlayerAction
    {
        //true if life equals or less then percent 
        public int lifele;
        //true if life equals or greater then percent
        public int lifege;
        //true if distance to target is equal or less
        public int distle;
        //true if distance to target is greater or less
        public int distge;
        //global cooldown
        public float gcd;


        public ZFPlayerAction(string iName, BabBot.Bot.Binding iBinding, float iRange, float iCoolDown, bool iSelfCast, bool iToggle)
            : base(iName, iBinding, iRange, iCoolDown, iSelfCast, iToggle)
        {
        }


        //checks if the action should executed according to distle, distge, lifele, lifege, checkBuff ...
        public bool shouldBeExecuted(IPlayerWrapper me)
        {
            bool ret = true;
            //currently we have no access to player life - this must be changed.

            //check life
            //int life_prc = me.life*100/me.maxLife;
            //if(lifele!=-1)
            //{
            //    ret = lifele<=life_prc;
            //}
            //if(lifege!=-1)
            //{
            //    ret = ret && lifege>=life_prc;
            //}

            //check distance
            //int dist = ...;
            //if(distle!=-1)
            //{
            //    ret = ret && distle <= dist;
            //}
            //if(distge!=-1)
            //{
            //    ret = ret && distge >= dist;
            //}

            //check power (energy,mana,rage)
            
            //check buffs/debuffs

            //TODO: rest of the keywords
            return ret;
        }
    }
}