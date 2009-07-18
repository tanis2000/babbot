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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BabBot.States;
using BabBot.Wow;
using BabBot.Bot;
using BabBot.Scripting;

namespace BabBot.States.Common
{
    /// <summary>
    /// GlobalBaseBotState is an abstract (inherit only) class for building
    /// bots of all kind and should be used as a global state.
    /// </summary>
    public class GlobalBaseBotState : State<Wow.WowPlayer>
    {
        #region Configurable properties

        protected int MinMPPct = 80; // Minimum mana percentage to start drinking
        protected int MinHPPct = 80; // Minimum health percentage to start eating
        protected float MinMeleeDistance = 1.0f;
        protected float MaxMeleeDistance = 5.0f;
        protected float MinRangedDistance = 15.0f;
        protected float MaxRangedDistance = 25.0f;
        protected int HpPctEmergency = 25; // Minimum health percentage at which we call the emergency healing routine
        protected int HpPctPotion = 20; // Minimum health percentage at which we look for a health potion
        protected int MpPctPotion = 15; // Minimum mana percentage at which we look for a mana potion
        protected float MinDistanceFromCorpse = 20.0f; // Minimum distance from corpse after which we stop pathing looking for it
        protected float PullRange = 25.0f; // Distance at which we want to pull mobs
        protected WowUnit LastTarget = null;

        #endregion

        #region Lists

        protected PlayerActionList Actions { get; set; }
        protected BindingList Bindings { get; set; }
        protected SpellList HealingSpells { get; set; }

        #endregion

        /// <summary>
        /// This is called by the StateManager to check if we need to rest. It should be
        /// implemented by each class
        /// </summary>
        /// <returns>true if we need to rest</returns>
        public virtual bool NeedRest()
        {
            return false;
        }
          
        protected SConsumable Consumable = SConsumable.Instance;

        protected bool _IsDeadStateRunning = false;

        protected override void DoEnter(BabBot.Wow.WowPlayer Entity)
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

        protected override void DoExecute(BabBot.Wow.WowPlayer Entity)
        {
            //Main update loop.  Basically just do a bunch of checks and then 
            // let another state take over.

            //Check if dead, if so then switch over to the dead state
            if (Entity.IsDead)
            {
                //if the current state is not already dead then don't worry about it
                if (!_IsDeadStateRunning)
                {
                    DeadState ds = new DeadState();

                    ds.Exited += new EventHandler<StateEventArgs<WowPlayer>>(deadState_Exited);

                    CallChangeStateEvent(Entity, ds, true, false);

                    _IsDeadStateRunning = true;
                }

                //now we return, as there isn't anything else to do while we are dead
                return;
            }

            //auto attack a target
            if(!Entity.StateMachine.IsInState(typeof(AttackNearMobState)))
            {
                AttackNearMobState anbs = new AttackNearMobState();
                CallChangeStateEvent(Entity, anbs, true, false);
            }

        }

        void deadState_Exited(object sender, StateEventArgs<WowPlayer> e)
        {
            _IsDeadStateRunning = false;
        }

        protected override void DoExit(BabBot.Wow.WowPlayer Entity)
        {
            Entity.Stop();
        }

        protected override void DoFinish(BabBot.Wow.WowPlayer Entity)
        {
            throw new NotImplementedException();
        }
    
        
    }
}
