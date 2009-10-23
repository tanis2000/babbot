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
using BabBot.Common;
using BabBot.Scripts.Common;
using BabBot.Wow;
using BabBot.States;

namespace BabBot.Scripts.Paladin
{
    public class RestState : Common.RestState
    {
        protected override void DoEnter(WowPlayer Entity)
        {
            
        }

        /// <summary>
        /// This is the routine called when we need resting
        /// This should be implemented in the spcific class script
        /// </summary>
        protected override void DoExecute(WowPlayer player)
        {
            Output.Instance.Script("OnRest()", this);

            if (player.IsCasting() && player.HpPct > 90)
            {
                // we don't need to finish casting a healing spell, let's stop it
                player.SpellStopCasting();
            }

            if (player.IsMoving())
            {
                // If we're still moving, let's stop
                player.Stop();
            }

            if (player.HasDebuff("Resurrection Sickness") && !player.IsSitting)
            {
                GlobalBaseBotState.Sit(player);
                Output.Instance.Script("OnRest() - We have resurrection sickness. We stay put.", this);
                return;
            }

            Core.DebuffAll();


            if (Core.scrollSpam && GlobalBaseBotState.CanUseScroll(player))
            {
                GlobalBaseBotState.UseScroll(player);
            }


            if (!Core.debuffStatus)
            {
                if (player.MpPct >= Core.RestMana && player.HpPct <= GlobalBaseBotState.MinHPPct && GlobalBaseBotState.Consumable.HasBandage() && Core.useBandage)
                {
                    GlobalBaseBotState.Consumable.UseBandage();
                }
            }

            if (player.HpPct >= GlobalBaseBotState.MinHPPct && player.MpPct >= GlobalBaseBotState.MinMPPct)
            {
                GlobalBaseBotState.Stand(player);
                return;
            }

            if (player.HpPct <= Core.RestHp && player.MpPct > 10 && !player.HasBuff("Drink") && !player.IsCasting())
            {
                if (player.IsSitting)
                {
                    GlobalBaseBotState.Stand(player);
                }

                if (!player.IsCasting())
                {
                    Core.HealSystem(player);
                    return;
                }
            }

            if (!player.HasBuff("Drink") && player.MpPct <= Core.RestMana && !player.IsCasting())
            {
                if (GlobalBaseBotState.Consumable.HasDrink())
                {
                    GlobalBaseBotState.Consumable.UseDrink();
                }
                else
                {
                    player.CastSpellByName("Blessing of Wisdom");
                }
            }
        }

        protected override void DoExit(WowPlayer Entity)
        {
            //on exit, if there is a previous state, go back to it
            if (PreviousState != null)
            {
                CallChangeStateEvent(Entity, PreviousState, false, false);
            }
        }

        protected override void DoFinish(WowPlayer Entity)
        {
        }
    }
}