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
using BabBot.Scripting;
using BabBot.Wow;
using BabBot.States;
using BabBot.Common;

namespace BabBot.Scripts.Common
{
    /// <summary>
    /// GlobalBaseBotState is an abstract (inherit only) class for building
    /// bots of all kind and should be used as a global state.
    /// </summary>
    public class GlobalBaseBotState : State<WowPlayer>
    {
        #region Configurable properties

        public static int HpPctEmergency = 25; // Minimum health percentage at which we call the emergency healing routine
        public static int HpPctPotion = 20; // Minimum health percentage at which we look for a health potion
        protected WowUnit LastTarget;
        public static float MinMeleeDistance = 1.0f;
        public static float MaxMeleeDistance = 5.0f;
        public static float MaxRangedDistance = 25.0f;

        public static float MinDistanceFromCorpse = 20.0f;
                        // Minimum distance from corpse after which we stop pathing looking for it

        public static int MinHPPct = 80; // Minimum health percentage to start eating
        public static int MinMPPct = 80; // Minimum mana percentage to start drinking
        public static float MinRangedDistance = 15.0f;
        public static int MpPctPotion = 15; // Minimum mana percentage at which we look for a mana potion

        public static float PullRange = 25.0f; // Distance at which we want to pull mobs

        #endregion

        #region Lists

        protected PlayerActionList Actions { get; set; }
        protected BindingList Bindings { get; set; }
        protected SpellList HealingSpells { get; set; }

        #endregion

        protected bool _IsDeadStateRunning;
        public static SConsumable Consumable = SConsumable.Instance;

        /// <summary>
        /// This is called by the StateManager to check if we need to rest. It should be
        /// implemented by each class
        /// </summary>
        /// <returns>true if we need to rest</returns>
        public virtual bool NeedRest(WowPlayer player)
        {
            return false;
        }

        #region Ranges

        protected float MinRanged(WowPlayer player)
        {
            return 11.0f + player.TargetBoundingRadius();
        }

        protected float MaxRanged(WowPlayer player)
        {
            if (player.TargetBoundingRadius() < 2.0f)
            {
                return 27.0f + player.TargetBoundingRadius();
            }
            return 29.0f;
        }

        protected float MinMelee(WowPlayer player)
        {
            return 1.0f + player.TargetBoundingRadius();
        }

        protected float MaxMelee(WowPlayer player)
        {
            if (player.TargetBoundingRadius() < 2.0f)
            {
                return 2.9f + player.TargetBoundingRadius();
            }
            return 4.5f;
        }


        #endregion

        #region Casting

        protected bool IsCasting(WowPlayer player)
        {
            // TODO: We should also check if we're skinning, herbing etc as it counts as casting as well
            return player.IsCasting();
        }

        #endregion

        #region Common routines

        protected void Flee()
        {
            Flee(true);
        }

        protected void Flee(bool iToggle)
        {
            // TODO: implement
        }

        /// <summary>
        /// This method ensures that you've got the best target
        /// </summary>
        protected void TergetBestTarget() // aka BestTarget in Toon.iss
        {
            // TODO: implement
        }

        /// <summary>
        /// Check if you've got the best target
        /// </summary>
        /// <returns>true if you've got the best target</returns>
        protected bool TargetIsBestTarget()
        {
            // TODO: implement
            return true;
        }

        /// <summary>
        /// Returns the best target from the target collection
        /// </summary>
        /// <returns>the best target</returns>
        protected WowUnit BestTarget()
        {
            // TODO: implement
            return null;
        }

        /// <summary>
        /// Returns the next best target in the collection
        /// </summary>
        /// <returns>next best target</returns>
        protected WowUnit NextBestTarget()
        {
            // TODO: implement
            return null;
        }

        protected bool TargetIsNew()
        {
            // TODO: implement
            return true;
        }

        public static bool CanUseScroll(WowPlayer player)
        {
            return CanUseScroll(player, "ANY");
        }

        public static bool CanUseScroll(WowPlayer player, string kind)
        {
            string scroll = CheckForScroll(player, kind);
            if (!string.IsNullOrEmpty(scroll))
            {
                return true;
            }
            return false;
        }

        public static string CheckForScroll(WowPlayer player)
        {
            return CheckForScroll(player, "ANY");
        }

        public static string CheckForScroll(WowPlayer player, string kind)
        {
            if (kind == "ANY")
            {
                if (Consumable.HasScroll("Strength") && !player.HasBuff("Strength"))
                {
                    return "Strength";
                }
                if (Consumable.HasScroll("Agility") && !player.HasBuff("Agility"))
                {
                    return "Agility";
                }
                if (Consumable.HasScroll("Stamina") && !player.HasBuff("Stamina"))
                {
                    return "Stamina";
                }
                if (Consumable.HasScroll("Protection") && !player.HasBuff("Protection"))
                {
                    return "Protection";
                }
                if (Consumable.HasScroll("Spirit") && !player.HasBuff("Spirit"))
                {
                    return "Spirit";
                }
                if (Consumable.HasScroll("Intellect") && !player.HasBuff("Intellect"))
                {
                    return "Intellect";
                }
            }
            else if (kind == "Protection" || kind == "Armor")
            {
                if (Consumable.HasScroll("Protection") && !player.HasBuff("Armor"))
                {
                    return "Protection";
                }
            }
            else if (Consumable.HasScroll(kind) && !player.HasBuff(kind))
            {
                return kind;
            }
            return "";
        }

        public static void UseScroll(WowPlayer player)
        {
            UseScroll(player, "ANY");
        }

        public static void UseScroll(WowPlayer player, string kind)
        {
            string scroll = CheckForScroll(player, kind);
            if (scroll != "")
            {
                //Output.Instance.Script(string.Format("Using scroll of {0}", kind), this);
                player.TargetMe();
                Consumable.UseScroll(kind);
                return;
            }
        }


        #endregion

        #region Sit & Stand

        protected static bool IsSitting(WowPlayer player)
        {
            // This should prevent drowning
            if (player.IsSitting)
            {
                player.DoString("DescendStop()");
                return true;
            }
            player.DoString("DescendStop()");
            return false;
        }

        public static void Sit(WowPlayer player)
        {
            if (!IsSitting(player))
            {
                player.DoString("DoEmote(\"SIT\")");
                player.Wait(300);
            }
        }

        public static void Stand(WowPlayer player)
        {
            if (IsSitting(player))
            {
                player.DoString("DoEmote(\"STAND\")");
                player.Wait(300);
            }
        }

        #endregion

        #region Rest stuff



        protected bool NeedMana(WowPlayer player)
        {
            if (player.MpPct < MinMPPct)
            {
                return true;
            }
            return false;
        }

        protected bool NeedHealth(WowPlayer player)
        {
            if (player.HpPct < MinHPPct)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// This should be implemented by the script of the corresponing player class and return whether
        /// the toon can cast healing spells or not
        /// </summary>
        /// <returns>true if the toon can cast healing spells</returns>
        protected virtual bool IsHealer()
        {
            return false;
        }

        protected bool CanSelfHeal()
        {
            /// We should also have a list of self healing spells with a priority on them
            /// and go through that list and see if we have the mana and if the spell is
            /// not on cooldown
            if (IsHealer())
            {
                return true;
            }
            return false;
        }

        protected virtual void SelfHeal()
        {

        }




        protected void Eat()
        {
            /// we should go through our list of food and use one of them
        }

        #endregion

        #region Consumables

        public static bool HasHealthPotion()
        {
            return false;
        }

        public static bool HasManaPotion()
        {
            return false;
        }


        #endregion

        #region PreConfigured States

        /*
         * This is the list of states that we're using. We define them here so that
         * we can override them in specific scripts (see the example in the Paladin folder)
         */
        protected State<WowPlayer> inCombatState;
        protected State<WowPlayer> roamingState;
        protected State<WowPlayer> preCombatState;
        protected State<WowPlayer> postCombatState;
        protected State<WowPlayer> restState;

        #endregion

        protected override void DoEnter(WowPlayer Entity)
        {
            Output.Instance.Script("GlobalBaseBotState.DoEnter() -- Begin", this);

            // Initialize all the lists
            Bindings = new BindingList();
            Actions = new PlayerActionList();
            HealingSpells = new SpellList();

            //Init SConsumable
            SConsumable.Instance.Init(Entity);


            inCombatState = new InCombatState();
            roamingState = new RoamingState();
            preCombatState = new PreCombatState();
            postCombatState = new PostCombatState();
            restState = new RestState();

            Output.Instance.Script("GlobalBaseBotState.DoEnter() -- End", this);
        }

        protected override void DoExecute(WowPlayer Entity)
        {
            //Main update loop.  Basically just do a bunch of checks and then 
            // let another state take over.

            // Output the current state we are in for debugging purpouses
            if (Entity.StateMachine.CurrentState == null)
            {
                Output.Instance.Script(string.Format("Current State: null"),
                                       this);
            }
            else
            {
                Output.Instance.Script(string.Format("Current State: {0}", Entity.StateMachine.CurrentState.GetType()),
                                       this);
            }

            // This should never happen. But if we end up there, we simply stop all movements
            if (!Entity.StateMachine.IsRunning)
            {
                /// Right now we only make sure that we're no longer moving around.
                /// At some point we might want to switch to a stop state to also stop fighting,
                /// ressing or whatever else can happen
                Output.Instance.Script("The StateMachine is currently stopped. We stop movement as well", this);
                Entity.Stop();
                return;
            }

            // If it's the first time we enter here, we will have no CurrentState assigned
            // and we start by jumping to the RoamingState
            if (Entity.StateMachine.CurrentState == null)
            {
                // We just started, let's go roaming
                CallChangeStateEvent(Entity, roamingState, true, false);
                return;
            }

            // Are we roaming? Let's check if there's anything else to do
            if (Entity.StateMachine.IsInState(roamingState.GetType()))
            {
                if (Entity.IsBeingAttacked())
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
                    CallChangeStateEvent(Entity, preCombatState, true, false);
                    return;
                }

                Output.Instance.Script("Checking for enemies while roaming", this);
                if (Entity.EnemyInSight())
                {
                    Output.Instance.Script("We have an enemy in sight, switching to PreCombat", this);
                    /// We have an enemy somewhere around us, we'd better get ready for the fight
                    CallChangeStateEvent(Entity, preCombatState, true, false);
                    return;
                }

            }


            if (Entity.StateMachine.IsInState(preCombatState.GetType()))
            {
                if ((Entity.IsBeingAttacked()) || (Entity.HasTarget))
                {
                    CallChangeStateEvent(Entity, inCombatState, true, false);
                    return;
                }

                if (!Entity.HasTarget)
                {
                    CallChangeStateEvent(Entity, roamingState, true, false);
                    return;

                }
            }


            if (Entity.StateMachine.IsInState(inCombatState.GetType()))
            {
                /// We should check if our target died and
                /// in that case go to PostCombat, but we lose target once the
                /// mob dies, so we cannot use that. We've got to come up with
                /// a better idea. 
                Output.Instance.Script("Checking if we no longer have a target after a fight", this);
                if (!Entity.HasTarget || Entity.IsTargetDead())
                {
                    Output.Instance.Script("We no longer have a target, thus we enter PostCombatState", this);
                    CallChangeStateEvent(Entity, postCombatState, true, false);
                    return;
                }
            }

            if (Entity.StateMachine.IsInState(postCombatState.GetType()))
            {
                if (NeedRest(Entity))
                {
                    CallChangeStateEvent(Entity, restState, true, false);
                    return;
                }
                CallChangeStateEvent(Entity, roamingState, true, false);
                return;
            }

            /*
            if (Entity.StateMachine.IsInState(typeof(PreRestState)))
            {
                /// We should check if we finished resting
                CurrentState = PlayerState.Rest;
                return;
            }
            */

            if (Entity.StateMachine.IsInState(restState.GetType()))
            {
                /// We ask the script if we should keep resting
                if (!NeedRest(Entity))
                {
                    CallChangeStateEvent(Entity, roamingState, true, false);
                    return;
                }
                return;
            }

            /*
            if (Entity.StateMachine.IsInState(typeof(PostRestState)))
            {
                /// We finished resting, go back to roaming
                CurrentState = PlayerState.Roaming;
                return;
            }
            */

            if (Entity.StateMachine.IsInState(typeof(DeadState)))
            {
                /// Let's see if we are still dead or what
                if ((!Entity.IsDead) && (!Entity.IsGhost))
                {
                    CallChangeStateEvent(Entity, roamingState, true, false);
                }
                return;
            }

            //Check if dead, if so then switch over to the dead state
            if (Entity.IsDead || Entity.IsGhost)
            {
                //if the current state is not already dead then don't worry about it
                if (!_IsDeadStateRunning)
                {
                    var ds = new DeadState();

                    ds.Exited += deadState_Exited;

                    CallChangeStateEvent(Entity, ds, true, false);

                    _IsDeadStateRunning = true;
                }

                //now we return, as there isn't anything else to do while we are dead
                return;
            }

            /// We ask the script if we should keep resting
            if (NeedRest(Entity))
            {
                CallChangeStateEvent(Entity, restState, true, false);
            }

/*
            //auto attack a target
            if (!Entity.StateMachine.IsInState(typeof (AttackNearMobState)))
            {
                var anbs = new AttackNearMobState();
                CallChangeStateEvent(Entity, anbs, true, false);
            }
 */
        }

        private void deadState_Exited(object sender, StateEventArgs<WowPlayer> e)
        {
            _IsDeadStateRunning = false;
        }

        protected override void DoExit(WowPlayer Entity)
        {
            Entity.Stop();
        }

        protected override void DoFinish(WowPlayer Entity)
        {
            throw new NotImplementedException();
        }
    }
}