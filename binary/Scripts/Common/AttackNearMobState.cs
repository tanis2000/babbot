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
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using BabBot.Manager;
using BabBot.Wow;
using BabBot.States;

namespace BabBot.Scripts.Common
{
    public class AttackNearMobState : State<WowPlayer>
    {
        protected override void DoEnter(WowPlayer Entity)
        {
            return;
        }

        protected override void DoExecute(WowPlayer Entity)
        {
            //if we don't have a target then get one
            if (!Entity.HasTarget)
            {
                List<WowObject> d = ProcessManager.ObjectManager.GetAllObjectsAroundLocalPlayer();

                IEnumerable<WowObject> m = from c in d
                                           where c.Type == Descriptor.eObjType.OT_UNIT && ((WowUnit) c).IsLootable
                                           select c;

                //get first unit and select it
                if (m.Count() > 0)
                {
                    Entity.SelectMob((WowUnit) m.First());
                }
            }

            //if distance to target is to far, then use a move to first
            if (Entity.DistanceFromTarget() > 0.5f)
            {
                var mtsTarget = new MoveToState(Entity.CurTarget.Location);

                //request that we move to this location
                CallChangeStateEvent(Entity, mtsTarget, true, false);

                return;
            }

            //interact with it
            Entity.CurTarget.Interact();

            while (Entity.CurTarget.Hp > 0)
            {
                Thread.Sleep(100);
            }
        }

        protected override void DoExit(WowPlayer Entity)
        {
            return;
        }

        protected override void DoFinish(WowPlayer Entity)
        {
            return;
        }
    }
}