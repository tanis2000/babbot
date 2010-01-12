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
using System;
using BabBot.Bot;
using BabBot.Common;
using BabBot.Scripting;
using BabBot.Scripts.Common;
using BabBot.Wow;

namespace BabBot.Scripts.Paladin
{
    public class Core : GlobalBaseBotState
    {
        #region Paladin specific settings

        public static string multiAura = "Retribution Aura";
        public static int HpHammer = 40;
        public static int exorcismHp = 100;
        public static bool useExorcism = true;
        public static bool combatDebuff = false;
        public static bool scrollSpam = true;
        public static bool debuffStatus = false;
        public static int RestMana = 50;
        public static int RestHp = 50;
        public static bool useBandage = false;
        public static bool divineShield = true;
        public static bool emergBless = true;
        public static string emBless = "Blessing of Light";


        #endregion

        protected override void DoEnter(WowPlayer Entity)
        {
            base.DoEnter(Entity);
            Output.Instance.Script("Core.DoEnter() -- Begin", this);

            // We override the default states with our own
            inCombatState = new Paladin.InCombatState();
            restState = new Paladin.RestState();

            Output.Instance.Script("Core.DoEnter() -- End", this);
        }

        protected override bool IsHealer()
        {
            return true;
        }

        protected void SelfHeal(WowPlayer player)
        {
            Output.Instance.Script("SelfHeal()", this);
            foreach (var spell in HealingSpells)
            {
                if (!spell.IsOnCooldown() && (spell.Cost < player.Mp))
                {
                    Output.Instance.Script("SelfHeal() - Casting " + spell.Name, this);
                    player.CastSpellByName(spell.Name, true);
                }
            }
        }




        public static void Emergency(WowPlayer player)
        {
            if (!player.IsCasting("Holy Light") && !player.IsCasting("Flash of Light"))
            {
                if (!player.HasBuff("Forbearance"))
                {
                    if (player.CanCast("Divine Protection"))
                    {
                        if (player.CanCast("Divine Shield") && divineShield)
                        {
                            player.SpellStopCasting();
                            player.CastSpellByName("Divine Shield");
                        }
                        else
                        {
                            player.SpellStopCasting();
                            player.CastSpellByName("Divine Protection");
                        }
                        HealSystem(player);
                    }
                    else if (player.CanCast("Blessing of Protection"))
                    {
                        player.SpellStopCasting();
                        player.CastSpellByName("Blessing of Protection");
                        HealSystem(player);
                    }
                }

                if (!player.HasBuff(emBless) && player.CanCast(emBless) && emergBless)
                {
                    player.CastSpellByName(emBless);
                }

                if (player.CanCast("Lay on Hands") && !player.HasBuff("Forbearance"))
                {
                    player.SpellStopCasting();
                    player.CastSpellByName("Lay on Hands");
                }

                if (player.HasBuff("Forbearance"))
                {
                    if (player.CanCast("Lay on Hands") && player.HpPct < 24) 
                    {
                        player.SpellStopCasting();
                        player.CastSpellByName("Lay on Hands");
                    }
                }

                if (player.HpPct < MinHPPct)
                {
                    HealSystem(player);
                }
            }
        }

        public static void HealSystem(WowPlayer player)
        {
            if (player.CanCast("Flash of Light"))
            {
                player.CastSpellByName("Flash of Light", true);
                return;
            }

            if (player.CanCast("Holy Light"))
            {
                player.CastSpellByName("Holy Light", true);
                return;
            }
        }

        protected void CastInterruption()
        { 
        }



        public static void DebuffAll()
        {
        }

        public override bool NeedRest(WowPlayer player)
        {
            if (player.IsDead)
            {
                return false;
            }

            if (player.IsInCombat())
            {
                return false;
            }

            if (player.HasBuff("Resurrection Sickness"))
            {
                return true;
            }

            if (player.MpPct < RestMana && !player.IsCasting() && !player.HasBuff("Drink"))
            {
                Output.Instance.Script("Resting for mana", this);
                return true;
            }

            if (player.MpPct < MinMPPct && player.HasBuff("Drink"))
            {
                Output.Instance.Script("Resting to continue drinking", this);
                return true;
            }

            if (player.HpPct < RestHp && !player.IsCasting() && !player.HasBuff("Drink"))
            {
                Output.Instance.Script("Resting for health", this);
                return true;
            }


            return false;
        }


    }
}