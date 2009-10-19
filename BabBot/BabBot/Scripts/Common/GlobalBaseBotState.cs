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
        protected UnitWrapper BestTarget()
        {
            // TODO: implement
            return null;
        }

        /// <summary>
        /// Returns the next best target in the collection
        /// </summary>
        /// <returns>next best target</returns>
        protected UnitWrapper NextBestTarget()
        {
            // TODO: implement
            return null;
        }

        protected bool TargetIsNew()
        {
            // TODO: implement
            return true;
        }

        protected bool CanUseScroll(WowPlayer player)
        {
            return CanUseScroll(player, "ANY");
        }

        protected bool CanUseScroll(WowPlayer player, string kind)
        {
            string scroll = CheckForScroll(player, kind);
            if (!string.IsNullOrEmpty(scroll))
            {
                return true;
            }
            return false;
        }

        protected string CheckForScroll(WowPlayer player)
        {
            return CheckForScroll(player, "ANY");
        }

        protected string CheckForScroll(WowPlayer player, string kind)
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

        protected void UseScroll(WowPlayer player)
        {
            UseScroll(player, "ANY");
        }

        protected void UseScroll(WowPlayer player, string kind)
        {
            string scroll = CheckForScroll(player, kind);
            if (scroll != "")
            {
                Console.WriteLine(string.Format("Using scroll of {0}", kind));
                player.TargetMe();
                Consumable.UseScroll(kind);
                return;
            }
        }


        #endregion

        #region Sit & Stand

        protected bool IsSitting(WowPlayer player)
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

        protected void Sit(WowPlayer player)
        {
            if (!IsSitting(player))
            {
                player.DoString("DoEmote(\"SIT\")");
                player.Wait(300);
            }
        }

        protected void Stand(WowPlayer player)
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

        protected bool HasHealthPotion()
        {
            return false;
        }

        protected bool HasManaPotion()
        {
            return false;
        }


        #endregion

        protected override void DoEnter(WowPlayer Entity)
        {
            Output.Instance.Script("DoEnter() -- Begin", this);

            // Initialize all the lists
            Bindings = new BindingList();
            Actions = new PlayerActionList();
            HealingSpells = new SpellList();

            //Init SConsumable
            SConsumable.Instance.Init(Entity);

            Output.Instance.Script("DoEnter() -- End", this);
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