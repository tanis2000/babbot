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
using BabBot.Wow;
using BabBot.Scripting;

namespace BabBot.Manager
{
    public sealed class StateManager
    {
        private static readonly StateManager instance = new StateManager();

        private PlayerState CurrentState;
        private PlayerState LastState;
        private IScript script;
        public static StateManager Instance
        {
            get { return instance; }
        }

        public PlayerState State
        {
            get { return CurrentState; }
        }

        public IScript Script
        {
            get { return script; }
            set { script = value; }
        }

        public void Init()
        {
            CurrentState = LastState = PlayerState.Start;
            Stop();
        }


        public void UpdateState()
        {
            LastState = CurrentState;

            if (CurrentState == PlayerState.Start)
            {
                CurrentState = PlayerState.Roaming;
                return;
            }

            if (CurrentState == PlayerState.Roaming)
            {
                if (ProcessManager.Player.IsBeingAttacked())
                {
                    /// We should target the mob that is attacking us
                    /// and I have no clue how to do it at the moment.
                    /// That way we can also know the location of the mob
                    /// in case we want to move closer in order to be able to fight
                    /// if it's a caster
                    /// 
                    /// Idea #1:
                    /// get the mob GUID (we have it) 
                    /// get the location of that mob
                    /// turn in order to face it
                    /// send TAB and check the current target GUID and keep
                    /// TABbing until the GUID matches
                    /// 
                    /// Should we implement this in the cscript? (I think so)
                    CurrentState = PlayerState.PreCombat;
                    return;
                }

                if (ProcessManager.Player.EnemyInSight())
                {
                    /// We have an enemy somewhere around us, we'd better get ready for the fight
                    CurrentState = PlayerState.PreCombat;
                    return;
                }
            }

            if (CurrentState == PlayerState.PreCombat)
            {
                if ((ProcessManager.Player.IsBeingAttacked()) || (ProcessManager.Player.HasTarget()))
                {
                    CurrentState = PlayerState.InCombat;
                    return;
                }

                if (!ProcessManager.Player.HasTarget())
                {
                    CurrentState = PlayerState.Roaming;
                    return;
                }
            }

            if (CurrentState == PlayerState.InCombat)
            {
                /// We should check if our target died and
                /// in that case go to PostCombat, but we lose target once the
                /// mob dies, so we cannot use that. We've got to come up with
                /// a better idea. 
                if (!ProcessManager.Player.HasTarget())
                {
                    CurrentState = PlayerState.PostCombat;
                    return;
                }
            }

            if (CurrentState == PlayerState.PostCombat)
            {
                /// We should check if we need to rest
                CurrentState = PlayerState.PreRest;
                return;
            }

            if (CurrentState == PlayerState.PreRest)
            {
                /// We should check if we finished resting
                CurrentState = PlayerState.Rest;
                return;
            }

            if (CurrentState == PlayerState.Rest)
            {
                /// We ask the script if we should keep resting
                if (!script.NeedRest())
                {
                    CurrentState = PlayerState.PostRest;
                }
                return;
            }

            if (CurrentState == PlayerState.PostRest)
            {
                /// We finished resting, go back to roaming
                CurrentState = PlayerState.Roaming;
                return;
            }


            if (CurrentState == PlayerState.Dead)
            {
                /// Let's see if we are still dead or what
                if ((!ProcessManager.Player.IsDead()) && (!ProcessManager.Player.IsGhost()))
                {
                    CurrentState = PlayerState.Roaming;
                }
                return;
            }
            
            if (ProcessManager.Player.IsDead())
            {
                CurrentState = PlayerState.Dead;
                return;
            }

            if (ProcessManager.Player.IsGhost())
            {
                CurrentState = PlayerState.Dead;
                return;
            }

            /// We ask the script if we should keep resting
            if (script.NeedRest())
            {
                CurrentState = PlayerState.Rest;
                return;
            }

            /*
            if (ProcessManager.Player.IsAtGraveyard())
            {
                CurrentState = PlayerState.Graveyard;
                return;
            }
             */

        }

        public void Start()
        {
            // Start botting
            LastState = CurrentState;
            CurrentState = PlayerState.Start;
            Common.Output.Instance.Echo("Starting.....");
        }

        public void Stop()
        {
            // Stop botting
            Common.Output.Instance.Echo("Stoping.....");
            LastState = CurrentState;
            CurrentState = PlayerState.Stop;
        }
    }
}