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
using BabBot.Wow;
using BabBot.States;

namespace BabBot.Scripts.Common
{
    public class PostCombatState : State<WowPlayer>
    {
        protected override void DoEnter(WowPlayer Entity)
        {
        }

        /// <summary>
        /// This happens when a combat has just ended.
        /// </summary>
        protected override void DoExecute(WowPlayer Entity)
        {
            Output.Instance.Script("OnPostCombat()", this);

            // If we're being attacked by some hotile mob we switch back to the combat state
            if (Entity.IsBeingAttacked()) return;

            // We should be free to do what we want, so we check for lootable mobs nearby
            Output.Instance.Script("OnPostCombat() - Adding last target to loot list", this);
            Entity.AddLastTargetToLootList();
            Output.Instance.Script("OnPostCombat() Looting closest lootable mob", this);
            Entity.LootClosestLootableMob();
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