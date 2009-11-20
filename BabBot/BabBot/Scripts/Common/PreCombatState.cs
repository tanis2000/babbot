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
        protected static WowUnit _MobToAttack;
        protected static DateTime _LastCTMCheck = DateTime.Now;

        public bool HasMobToAttack()
        {
            if (_MobToAttack == null) return false;

            return true;
        }

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

            DateTime start = DateTime.Now;
            
            if (Entity.IsBeingAttacked())
            {
                Output.Instance.Script("OnPreCombat() - We are being attacked", this);
                /// We are being attacked by a Mob. That means that we should fight back
                /// by finding the mob first of all
                if (Entity.SelectWhoIsAttackingUsWithCTM())
                {
                    /// We found who is attacking us and we fight back (no rebuffing now)
                    /// (If everything is correct at this point the StateManager will take care
                    /// of switching to the OnCombat state)
                    _MobToAttack = Entity.CurTarget;
                }
            }
            else
            {
                Output.Instance.Script("OnPreCombat() - We are going to attack someone", this);
                
                // Check if we already have a valid Unit to attack from a previous state
                if (_MobToAttack == null)
                {
                    Output.Instance.Script("Looking for a new enemy to attack", this);
                    // Find a new mob to attack
                    if (Entity.EnemyInSight())
                    {
                        Output.Instance.Script("We have something", this);
                        _MobToAttack = Entity.GetClosestEnemyInSight();
                        if (_MobToAttack != null)
                        {
                            Output.Instance.Script(
                                string.Format("The mob we're going to attack is a {0} with GUID {1:X}",
                                              _MobToAttack.Name, _MobToAttack.Guid), this);
                        } else
                        {
                            Output.Instance.Script("Couldn't find the closest enemy in sight", this);
                        }
                    }
                }

                // Check if this is good
                if (_MobToAttack != null)
                {
                    Output.Instance.Script("We have a mob, checking if it's dead", this);
                    if (!_MobToAttack.IsDead)
                    {
                        Output.Instance.Script("Attacking it with CTM", this);
                        TimeSpan timeDiff = start - _LastCTMCheck;
                        if (timeDiff.TotalMilliseconds > 2000) 
                        {
                            Entity.AttackMobWithCTM(_MobToAttack);
                        }
                        _LastCTMCheck = DateTime.Now;
                    } else
                    {
                        Output.Instance.Script("The mob we were looking for is dead :(", this);
                        _MobToAttack = null;
                    }
                }

                /*
                if (Entity.EnemyInSight())
                {
                    // Face the closest enemy
                    Output.Instance.Script("OnPreCombat() - Facing closest enemy (we should get a target this way)", this);
                    //Entity.FaceClosestEnemy();
                    //Entity.AttackClosestEnemyWithCTM();
                    // Get the enemy unit and save it
                    // Check that it's not dead or anything like that
                    // Move to it

                    Entity.MoveToClosestEnemy();

                    // Let's check if we actually got it as our target
                    if ((Entity.HasTarget) && (!Entity.IsTargetDead()) && (Entity.IsTargetInEnemyList()))
                    {
                        Output.Instance.Script("OnPreCombat() - Affirmative. We have a target", this);
                        /// Ok, we have the target, it's time to start attacking,
                        /// but first we rebuff and drink up just in case
                    }
                    else
                    {
                        // Let's try moving closer. We should already be facing our wanted target
                        // TODO: Change this so that we use clicktomove instead
                        Entity.MoveForward(1000);
                        Output.Instance.Script("OnPreCombat() - Can't target. This should not happen :-P", this);
                    }
                }
                 * */
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