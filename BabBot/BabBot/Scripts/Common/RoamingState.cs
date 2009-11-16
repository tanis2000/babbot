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
using BabBot.Common;
using BabBot.Wow;
using BabBot.States;
using BabBot.Manager;

namespace BabBot.Scripts.Common
{
    public class RoamingState : State<WowPlayer>
    {
        protected static WayPoint _lastWayPoint = null;

        protected override void DoEnter(WowPlayer Entity)
        {
        }

        /// <summary>
        /// We are roaming through the waypoints with nothing else to do
        /// </summary>
        protected override void DoExecute(WowPlayer Entity)
        {
            /// This is where we should walk through the waypoints and 
            /// check what happens around us (like if there's anything to
            /// attack or anything attacking us, or if we run out of mana/health,
            /// or if we should rebuff something)
            /// 
            /// Right now we only walk through the waypoints as a proof of concept
            Output.Instance.Script("OnRoaming() -- Walking to the next waypoint");
            //Entity.WalkToNextWayPoint(WayPointType.Normal);
            WayPoint wp = null;
            if (_lastWayPoint != null)
            {
                Output.Instance.Script("OnRoaming() -- We have a last waypoint. Checking if we reached it");

                float distanceFromLast = MathFuncs.GetDistance(_lastWayPoint.Location, Entity.Location, false);
                if (distanceFromLast <= 3.0f)
                {
                    Output.Instance.Script("OnRoaming() -- We reached the last waypoint. Let's get a new one");
                    wp = WayPointManager.Instance.GetNextWayPoint(WayPointType.Normal);
                }
                else
                {
                    Output.Instance.Script("OnRoaming() -- We still need to reach the last waypoint. We reuse the last one.");
                    wp = _lastWayPoint;
                }
            }
            else
            {
                Output.Instance.Script("OnRoaming() -- This is the first waypoint. We try to get a new one.");
                wp = WayPointManager.Instance.GetNextWayPoint(WayPointType.Normal);
            }
            if (wp != null)
            {
                _lastWayPoint = wp;
                Output.Instance.Script(string.Format("Moving to waypoint. Index:{0}", WayPointManager.Instance.CurrentNormalWayPointIndex), this);
                Output.Instance.Script(string.Format("WayPoint: X:{0} Y:{1} Z:{2}", wp.Location.X, wp.Location.Y, wp.Location.Z), this);
                //MoveTo(wp.Location);
                float distance = MathFuncs.GetDistance(wp.Location, Entity.Location, false);
                if (distance > 3.0f)
                {
                    var mtsTarget = new MoveToState(wp.Location, 3.0f);

                    //request that we move to this location
                    CallChangeStateEvent(Entity, mtsTarget, true, false);

                    return;
                }

            }
            else
            {
                Output.Instance.Script("We are supposed to walk through waypoints but there's no waypoints defined", this);
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