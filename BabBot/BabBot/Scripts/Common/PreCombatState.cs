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
        protected static WowUnit MobToAttack;
        protected static DateTime LastCtmCheck = DateTime.Now;
        /// <summary> Time elapsed trying to attack the same mob (used to blacklist a mob that is evading/inside solids) </summary>
        protected static DateTime AttackTimeStart = DateTime.Now;

        public bool HasMobToAttack()
        {
            if (MobToAttack == null) return false;

            return true;
        }

        protected override void DoEnter(WowPlayer entity)
        {
        }

        /// <summary>
        /// This happens when we are being attacked by some mobs or when we
        /// have found something to kill 
        /// </summary>
        protected override void DoExecute(WowPlayer entity)
        {
            Output.Instance.Script("OnPreCombat() Begin", this);

            DateTime start = DateTime.Now;
            
            if (entity.IsBeingAttacked())
            {
                Output.Instance.Script("OnPreCombat() - We are being attacked", this);
                /// We are being attacked by a Mob. That means that we should fight back
                /// by finding the mob first of all
                if (entity.SelectWhoIsAttackingUsWithCTM())
                {
                    /// We found who is attacking us and we fight back (no rebuffing now)
                    /// (If everything is correct at this point the StateManager will take care
                    /// of switching to the OnCombat state)
                    MobToAttack = entity.CurTarget;
                    AttackTimeStart = DateTime.Now; // Reset the time check for blacklisting mobs
                }
            }
            else
            {
                Output.Instance.Script("OnPreCombat() - We are going to attack someone", this);
                
                // Check if we already have a valid Unit to attack from a previous state
                if (MobToAttack == null)
                {
                    Output.Instance.Script("Looking for a new enemy to attack", this);
                    // Find a new mob to attack
                    if (entity.EnemyInSight())
                    {
                        Output.Instance.Script("We have something", this);
                        MobToAttack = entity.GetClosestEnemyInSight();
                        if (MobToAttack != null)
                        {
                            Output.Instance.Script(
                                string.Format("The mob we're going to attack is a {0} with GUID {1:X}",
                                              MobToAttack.Name, MobToAttack.Guid), this);
                            AttackTimeStart = DateTime.Now; // Reset the time check for blacklisting mobs
                        }
                        else
                        {
                            Output.Instance.Script("Couldn't find the closest enemy in sight", this);
                        }
                    }
                }

                // Check if this is good
                if (MobToAttack != null)
                {
                    Output.Instance.Script("We have a mob, checking if it's dead", this);
                    if (!MobToAttack.IsDead)
                    {
                        TimeSpan attackTimeDiff = start - AttackTimeStart;
                        if (attackTimeDiff.TotalMilliseconds > 10000)
                        {
                            Output.Instance.Script("We spent more than 10 seconds trying to attack the same mob without reaching it. Moving on and blacklisting it.", this);
                            entity.MobBlackList.Add(MobToAttack);
                            MobToAttack = null;
                            return;
                        }
                            
                        Output.Instance.Script("Checking distance", this);
                        float distance = MathFuncs.GetDistance(MobToAttack.Location, entity.Location, false);
                        if (distance > 10.0f)
                        {
                            Output.Instance.Script("We're too far to CTM it, moving closer first", this);
                            var mtsTarget = new MoveToState(MobToAttack.Location, 3.0f);

                            //request that we move to this location
                            CallChangeStateEvent(entity, mtsTarget, true, false);

                            return;
                        }

                        Output.Instance.Script("Attacking it with CTM", this);
                        TimeSpan timeDiff = start - LastCtmCheck;
                        if (timeDiff.TotalMilliseconds > 2000) 
                        {
                            entity.AttackMobWithCTM(MobToAttack);
                            LastCtmCheck = DateTime.Now;
                        }
                    } else
                    {
                        Output.Instance.Script("The mob we were looking for is dead :(", this);
                        MobToAttack = null;
                    }
                }

                /*
                if (entity.EnemyInSight())
                {
                    // Face the closest enemy
                    Output.Instance.Script("OnPreCombat() - Facing closest enemy (we should get a target this way)", this);
                    //entity.FaceClosestEnemy();
                    //entity.AttackClosestEnemyWithCTM();
                    // Get the enemy unit and save it
                    // Check that it's not dead or anything like that
                    // Move to it

                    entity.MoveToClosestEnemy();

                    // Let's check if we actually got it as our target
                    if ((entity.HasTarget) && (!entity.IsTargetDead()) && (entity.IsTargetInEnemyList()))
                    {
                        Output.Instance.Script("OnPreCombat() - Affirmative. We have a target", this);
                        /// Ok, we have the target, it's time to start attacking,
                        /// but first we rebuff and drink up just in case
                    }
                    else
                    {
                        // Let's try moving closer. We should already be facing our wanted target
                        // TODO: Change this so that we use clicktomove instead
                        entity.MoveForward(1000);
                        Output.Instance.Script("OnPreCombat() - Can't target. This should not happen :-P", this);
                    }
                }
                 * */
            }
        }

        protected override void DoExit(WowPlayer entity)
        {
            //on exit, if there is a previous state, go back to it
            if (PreviousState != null)
            {
                CallChangeStateEvent(entity, PreviousState, false, false);
            }
        }

        protected override void DoFinish(WowPlayer entity)
        {
        }
    }
}