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

namespace BabBot.Scripts
{
    public class Paladin : Script, IScript
    {
         
        void IScript.Init()
        {
            Console.WriteLine("Paladin->Init()");
            // TODO: find a way to override this function so that we don't have to clone those lines
            Bindings = new BindingList();
            Actions = new PlayerActionList();
            
            // TODO: get that stuff out of the way and load bindings from the appropriate XML file
            Binding b = new Binding("melee", 1, "1");
            Bindings.Add(b.Name, b);
            b = new Binding("test", 1, "2");
            Bindings.Add(b.Name, b);

            PlayerAction a = new PlayerAction("attack", Bindings["melee"], 0.0f, 0.0f, false, true);
            Actions.Add(a.Name, a);
            a = new PlayerAction("fakeattack", Bindings["test"], 0.0f, 2.0f, true, true);
            Actions.Add(a.Name, a);

        }

        // TODO: add an interface for all common functions like this
        protected override void Fight()
        {
            // TODO: Implement actual fight logic (only by PlayAction)

            Console.WriteLine("attack");
            player.PlayAction(Actions["attack"], true);
            Console.WriteLine("fakeattack");
            player.PlayAction(Actions["fakeattack"]);
        }

        // TODO: with some refactoring we could use this as a generic routine
        protected override void OnInCombat()
        {
            Console.WriteLine("Paladin->OnInCombat()");
            if (player.IsBeingAttacked())
            {
                Console.WriteLine("Paladin->OnInCombat() - We are being attacked");
                // Someone initiated combat with us
                /// We are being attacked by a Mob. That means that we should fight back
                /// by finding the mob first of all
                if (player.SelectWhoIsAttackingUs())
                {
                    /// We found who is attacking us and we fight back
                    if (Math.Abs(player.FacingDegrees() - player.AngleToTargetDegrees()) > 20.0f)
                    {
                        player.FaceTarget();
                    }
                    if (player.DistanceFromTarget() > 3.0f)
                    {
                        /// we have to get closer (melee only though, we should also check if we're 
                        /// using a melee or spell ability)
                        /// Ok, we're a low level paladin.. and we do not have any ranged attacks yet..
                        /// So we get close first of all
                        /// 
                        Console.WriteLine("Paladin->OnInCombat() - Moving to target");
                        player.MoveToTarget(3.0f);
                    }
                    Fight();
                }
            } 
            else
            {
                Console.WriteLine("Paladin->OnInCombat() - We are initiating combat");
                // We initiate combat
                if (player.HasTarget())
                {
                    Console.WriteLine("Paladin->OnInCombat() - We've got a target");
                    /// We found who is attacking us and we fight back
                    if (Math.Abs(player.FacingDegrees() - player.AngleToTargetDegrees()) > 20.0f)
                    {
                        player.FaceTarget();
                    }
                    if (player.DistanceFromTarget() > 3.0f)
                    {
                        /// we have to get closer (melee only though, we should also check if we're 
                        /// using a melee or spell ability)
                        /// Ok, we're a low level paladin.. and we do not have any ranged attacks yet..
                        /// So we get close first of all
                        Console.WriteLine("Paladin->OnInCombat() - Moving to target");
                        player.MoveToTarget(3.0f);
                    }
                    Fight();
                }
            }
        }

    }
}
