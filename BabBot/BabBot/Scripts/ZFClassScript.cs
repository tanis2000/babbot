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

namespace BabBot.Scripts
{
    /*
     * this class is for loading zf-style bindings files.
     * currently it parses only lines like
     * defact= key:1 slot:1 prevacttime:20 distle:30 cooldown:1.4 js:hasNoBuff(20375)&&checkMana(80)
     */
    public class ZFClassScript : Script, IScript
    {
        void IScript.Init()
        {        
            string script_name = "fz_test.txt";
            FileInfo exe_finfo = new FileInfo(Application.ExecutablePath);
            string full_script_file = Path.Combine(Path.Combine(exe_finfo.DirectoryName, "Scripts"), script_name);

            char[] dels = { ' ', '\t' };
            char[] dels2 = { ':' };

            Console.WriteLine("Loading "+script_name+"...");
            StreamReader re = File.OpenText(full_script_file);
            string input = null;
            while ((input = re.ReadLine()) != null)
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
                //power: energy,mana,rage...
                //int powle = -1;
                //int powge = -1;
                

                //defact= act:sealOfComm slot:6 key:1 cooldown:1.4 prevacttime:30 js:hasNoBuff(20375)&&checkMana(80)
                string trimmed = input.Trim();
                if (trimmed.StartsWith("defact"))
                {
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

                                if ("act".Equals(first))
                                {
                                    actname = second;
                                }
                                else if ("slot".Equals(first))
                                {
                                    slot = System.Convert.ToInt32(second);
                                }
                                else if ("key".Equals(first))
                                {
                                    key = second;
                                }
                                else if ("prevacttime".Equals(first))
                                {
                                    //TODO: check units (seconds, ms?)
                                    cooldown = System.Convert.ToSingle(second);
                                }
                                else if ("distle".Equals(first))
                                {
                                    //range?
                                    reach = System.Convert.ToSingle(second);
                                }
                                else if ("js".Equals(first))
                                {
                                    //???
                                    //any way to support customer code?
                                }
                                else if ("cooldown".Equals(first))
                                {
                                    Console.WriteLine("cooldown token ignored.");
                                    //???
                                    //is this(global cooldown) of any use in babbot?
                                    //TODO: check units (seconds, ms?)
                                }
                                else if ("lifele".Equals(first))
                                {
                                    lifele = System.Convert.ToInt32(second);
                                }
                                else if ("lifege".Equals(first))
                                {
                                    lifege = System.Convert.ToInt32(second);
                                }
                            }
                            else
                            {
                                Console.WriteLine("Ignoring bad token '"+s+"'.");
                                //ignoring bad token: s
                            }
                        }

                        if (actname != null)
                        {
                            // 1. slot MUST not be set - actions can work without actionbar ("t" => attack )
                            BabBot.Bot.Binding b = new BabBot.Bot.Binding(actname, slot, key);
                            Bindings.Add(b.Name, b);

                            //PlayerAction a = new ZFPlayerAction(actname, b, reach, cooldown, self_cast, toggle, lifele, lifege);
                            //Actions.Add(a.Name, a);
                        }
                        else
                        {
                            Console.WriteLine("Ignoring '"+trimmed+"' - no act name found.");
                            //no action name => input line input ignored.
                        }
                    }
                    else
                    {
                         Console.WriteLine("Ignoring '"+trimmed+"' - no = found.");
                        // error parsing script_name: bad defact line: input
                    }
                }
            }
            re.Close();
            Console.WriteLine("Loading "+script_name+" - done.");
        }

    //TODO:
    //1. implement global actions (buffs) - check Paladin.cs when it ready, may be this can be done only once for all (sub)classes
    //2. implement precombat/combat/loot sequences (the same as #1 - check Paladin.cs when it is ready)
    //3. ??? support for customer scripting? or just provide some more checkMana/checkBuff/checkTargetisCasting keywords?   


        protected override void Fight()
        {
            //play once precombat actions
            //for action in precombatactions
            // if action.shouldBeExecuted(player) && action.isReady()
            //   playaction

            //while target is not dead and bot is not stopped and player is not dead
            //play combat sequence over and over again
            //
            //while(bot_is_running && have_target && target_is_not_dead)
            //  for action in combatactions
            //    if action.shouldBeExecuted(player) && action.isReady()
            //      playaction
        }
    }


    //public class ZFPlayerAction : PlayerAction
    //{      
    //    //true if life equals or less then percent 
    //    public int lifele;
    //    //true if life equals or greater then percent
    //    public int lifege;        

    //    public ZFPlayerAction (string iName, Binding iBinding, float iRange, float iCoolDown, bool iSelfCast, bool iToggle, int iLifele, int iLifege) : base (iName, iBinding, iRange, iCoolDown, iSelfCast, iToggle)
    //    {
    //        lifele = iLifele;
    //        lifege = iLifege;
    //    }


    //    //checks if the action should executed according to distle, distge, lifele, lifege, checkBuff ...
    //    public bool shouldBeExecuted(IPlayerWrapper me)
    //    {
    //        bool ret = true;
    //        //currently we have no access to player life - this must be changed.
    //        //int life_prc = me.life*100/me.maxLife;
    //        //if(lifele!=-1)
    //        //{
    //        //    ret = lifele<=life_prc;
    //        //}
    //        //if(lifege!=-1)
    //        //{
    //        //    ret = ret && lifege>=life_prc;
    //        //}
    //        //TODO: rest of the keywords
    //        return ret;
    //    }        
        
    //}
}