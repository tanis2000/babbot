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

namespace BabBot.States.Common
{
    /// <summary>
    /// GlobalBaseBotState is an abstract (inherit only) class for building
    /// bots of all kind and should be used as a global state.
    /// </summary>
    public class GlobalBaseBotState : State<WowPlayer>
    {
        #region Configurable properties

        protected int HpPctEmergency = 25; // Minimum health percentage at which we call the emergency healing routine
        protected int HpPctPotion = 20; // Minimum health percentage at which we look for a health potion
        protected WowUnit LastTarget;
        protected float MaxMeleeDistance = 5.0f;
        protected float MaxRangedDistance = 25.0f;

        protected float MinDistanceFromCorpse = 20.0f;
                        // Minimum distance from corpse after which we stop pathing looking for it

        protected int MinHPPct = 80; // Minimum health percentage to start eating
        protected float MinMeleeDistance = 1.0f;
        protected int MinMPPct = 80; // Minimum mana percentage to start drinking
        protected float MinRangedDistance = 15.0f;
        protected int MpPctPotion = 15; // Minimum mana percentage at which we look for a mana potion

        protected float PullRange = 25.0f; // Distance at which we want to pull mobs

        #endregion

        #region Lists

        protected PlayerActionList Actions { get; set; }
        protected BindingList Bindings { get; set; }
        protected SpellList HealingSpells { get; set; }

        #endregion

        protected bool _IsDeadStateRunning;
        protected SConsumable Consumable = SConsumable.Instance;

        /// <summary>
        /// This is called by the StateManager to check if we need to rest. It should be
        /// implemented by each class
        /// </summary>
        /// <returns>true if we need to rest</returns>
        public virtual bool NeedRest()
        {
            return false;
        }

        protected override void DoEnter(WowPlayer Entity)
        {
            Console.WriteLine("DoEnter() -- Begin");

            // Initialize all the lists
            Bindings = new BindingList();
            Actions = new PlayerActionList();
            HealingSpells = new SpellList();

            //Init SConsumable
            SConsumable.Instance.Init(Entity);

            Console.WriteLine("DoEnter() -- End");
        }

        protected override void DoExecute(WowPlayer Entity)
        {
            //Main update loop.  Basically just do a bunch of checks and then 
            // let another state take over.

            //Check if dead, if so then switch over to the dead state
            if (Entity.IsDead)
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

            //auto attack a target
            if (!Entity.StateMachine.IsInState(typeof (AttackNearMobState)))
            {
                var anbs = new AttackNearMobState();
                CallChangeStateEvent(Entity, anbs, true, false);
            }
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