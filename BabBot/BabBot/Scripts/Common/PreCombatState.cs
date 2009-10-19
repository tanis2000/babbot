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
    public class PreCombatState : State<WowPlayer>
    {
        protected override void DoEnter(WowPlayer Entity)
        {
        }

        /// <summary>
        /// This happens when we are being attacked by some mobs or when we
        /// have found something to kill 
        /// </summary>
        protected override void DoExecute(WowPlayer Entity)
        {
            Output.Instance.Script("OnPreCombat() Begin", this);
            if (Entity.IsBeingAttacked())
            {
                Output.Instance.Script("OnPreCombat() - We are being attacked", this);
                /// We are being attacked by a Mob. That means that we should fight back
                /// by finding the mob first of all
                if (Entity.SelectWhoIsAttackingUs())
                {
                    /// We found who is attacking us and we fight back (no rebuffing now)
                    /// (If everything is correct at this point the StateManager will take care
                    /// of switching to the OnCombat state)
                }
            }
            else
            {
                Output.Instance.Script("OnPreCombat() - We are going to attack someone", this);
                if (Entity.EnemyInSight())
                {
                    // Face the closest enemy
                    Output.Instance.Script("OnPreCombat() - Facing closest enemy (we should have a target now)", this);
                    Entity.FaceClosestEnemy();

                    // Let's check if we actually got it as our target
                    if (Entity.HasTarget)
                    {
                        Output.Instance.Script("OnPreCombat() - Affirmative. We have a target", this);
                        /// Ok, we have the target, it's time to start attacking,
                        /// but first we rebuff and drink up just in case
                    }
                    else
                    {
                        // Let's try moving closer
                        Output.Instance.Script("OnPreCombat() - Can't target. This should not happen :-P", this);
                    }
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