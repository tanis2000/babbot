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
using BabBot.Scripts;

namespace BabBot.Scripts.Paladin
{
    public class InCombatState : Common.InCombatState
    {
        protected override void DoEnter(WowPlayer Entity)
        {
        }

        /// <summary>
        /// This routine gets called every time we end up fighting (because we pulled or 
        /// because a mob aggroed)
        /// This should be implemented in the spcific class script
        /// </summary>
        protected override void DoExecute(WowPlayer player)
        {
            Output.Instance.Script("OnInCombat()", this);

            if (!player.HasTarget || player.IsTargetDead())
            {
                Output.Instance.Script("We no longer have a target or our target is dead. We exit this state.", this);
                return;
            }

            // TODO: Before uncommenting this part we need to work out a FindBestTarget routine
            /*
            if (player.IsTargetTapped() && !player.IsTargetTappedByMe())
            {
                player.SpellStopCasting();
                player.ClearTarget();
                player.FindBestTarget();
                player.AttackTarget();
            }
            */

            // We turn to face the target if we're facing away for some reason
            float angleDifference = Math.Abs(player.FacingDegrees() - player.AngleToTargetDegrees());
            Output.Instance.Script(string.Format("Degrees difference between player and target: {0}", angleDifference));
            if (angleDifference > 20.0f)
            {
                Output.Instance.Script("Facing target", this);
                player.FaceTarget();
            }

            if (player.DistanceFromTarget() > Core.MaxMeleeDistance)
            {
                Output.Instance.Script("Moving towards target", this);
                player.FaceTarget();
                //player.MoveToTarget(Core.MinMeleeDistance);
                var mts = new MoveToState(player.CurTarget.Location, Core.MinMeleeDistance);
                CallChangeStateEvent(player, mts, true, false);
                return;
            }

            if (player.IsMoving() && player.DistanceFromTarget() < Core.MaxMeleeDistance && player.DistanceFromTarget() > Core.MinMeleeDistance)
            {
                Output.Instance.Script("We reached the correct distance to attack. Stopping all movement.", this);
                player.Stop();
            }

            if (player.DistanceFromTarget() < Core.MinMeleeDistance)
            {
                Output.Instance.Script("We are too close to the mob, let's take a step back", this);
                player.MoveBackward(300);
            }

            if (!player.IsAttacking())
            {
                Output.Instance.Script("We're not yet attacking but we should. Let's do this!", this);
                player.AttackTarget();
            }

            if (player.HpPct <= Core.HpPctEmergency)
            {
                Output.Instance.Script("We're about to die. Time to perform the Emergency routine", this);
                Core.Emergency(player);
            }

            if (player.HpPct <= Core.HpPctPotion && Core.HasHealthPotion())
            {
                Output.Instance.Script("We're still dying.. but we have a health potion to use! weee! :D", this);
                player.SpellStopCasting();
                // TODO: implement a function to select the best health potion and drink it
                //player.TakePotion("HP");
            }

            if (player.MpPct <= Core.MpPctPotion && Core.HasManaPotion())
            {
                Output.Instance.Script("We're low on mana but we do have a mana potion, let's use it!", this);
                player.SpellStopCasting();
                // TODO: implement a function to select the best mana potion and drink it
                //player.TakePotion("MP");
            }

            if (player.HpPct <= Core.HpHammer && player.MpPct > 5 && player.HpPct >= Core.HpPctEmergency && player.CanCast("Hammer of Justice"))
            {
                Output.Instance.Script("We can use Hammer Of Justice, let's do it.", this);
                player.CastSpellByName("Hammer of Justice", true);

                if (player.TargetHpPct > 10)
                {
                    Core.HealSystem(player);
                    Output.Instance.Script("Hammer of Justice healing", this);
                    return;
                }
                return;
            }

            // TODO: implement IsBeingAttackedByManyMobs by going through all the mobs who have aggro on us and are in range
            /*
            if (player.IsBeingAttackedByManyMobs())
            {
                if (player.CanCast(multiAura))
                {
                    player.CastSpellByName(multiAura);
                }

                if (player.CanCast("Consecration"))
                {
                    player.CastSpellByName("Consecration");
                }
            }
            */

            // TODO: implement TargetIsCasting()
            /*
            if (player.TargetIsCasting())
            {
                CastInterruption();
            }
            */

            if (player.TargetHpPct <= 20)
            {
                FinalFight();
            }
            else
            {
                NormalFight();
            }


            // TODO: implement TargetCreatureType()
            /*
            if (useExorcism)
            {
                if (player.MpPct > 30 && player.TargetHpPct > exorcismHp && player.CanCast("Exorcism") && player.TargetCreatureType() == "Undead" || player.TargetCreatureType() == "Demon")
                {
                    player.CastSpellByName("Exorcism");
                }
            }
            */

            if (Core.combatDebuff)
            {
                Output.Instance.Script("Debuffing.", this);
                Core.DebuffAll();
            }

            // TODO: implement reading of the player race
            /*
            if (player.Race() == "Blood Elf")
            {
                if (player.Buff("Mana Tap").Application > 0 && player.MpPct <= 20)
                {
                    player.SpellStopCasting();
                    player.CastSpellByName("Arcane Torrent");
                }
                if (player.TargetMpPct > 0 && player.IsActionUsable("Mana Tap") && player.Buff("Mana Tap").Application < 3) 
                {
                    player.CastSpellByName("Mana Tap");
                }
            }
             */
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

        protected void FinalFight()
        {
        }

        protected void NormalFight()
        {
        }
    }
}