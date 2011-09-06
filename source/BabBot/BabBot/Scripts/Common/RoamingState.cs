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
        protected static WayPoint LastWayPoint = null;

        protected override void DoEnter(WowPlayer entity)
        {
        }

        /// <summary>
        /// We are roaming through the waypoints with nothing else to do
        /// </summary>
        protected override void DoExecute(WowPlayer entity)
        {
            WayPoint wp = null;

            Output.Instance.Script("Checking if we have a last waypoint defined", this);
            if (LastWayPoint != null)
            {
                Output.Instance.Script("We have a last waypoint. Checking if we reached it", this);

                float distanceFromLast = MathFuncs.GetDistance(LastWayPoint.Location, entity.Location, false);
                if (distanceFromLast <= 3.0f)
                {
                    Output.Instance.Script("We reached the last waypoint. Let's get a new one", this);
                    wp = WayPointManager.Instance.GetNextWayPoint(WayPointType.Normal);
                }
                else
                {
                    Output.Instance.Script("We still need to reach the last waypoint. We reuse the last one.", this);
                    wp = LastWayPoint;
                }
            }
            else
            {
                Output.Instance.Script("This is the first waypoint. We try to get a new one.", this);
                wp = WayPointManager.Instance.GetNextWayPoint(WayPointType.Normal);
            }

            // Id we do have a waypoint we actually move
            if (wp != null)
            {
                LastWayPoint = wp;
                Output.Instance.Script(string.Format("Moving to waypoint. Index:{0}", WayPointManager.Instance.CurrentNormalWayPointIndex), this);
                Output.Instance.Script(string.Format("WayPoint: X:{0} Y:{1} Z:{2}", wp.Location.X, wp.Location.Y, wp.Location.Z), this);
                //MoveTo(wp.Location);
                float distance = MathFuncs.GetDistance(wp.Location, entity.Location, false);
                if (distance > 3.0f)
                {
                    var mtsTarget = new MoveToState(wp.Location, 3.0f);

                    //request that we move to this location
                    CallChangeStateEvent(entity, mtsTarget, true, false);

                    return;
                }

            }
            else
            {
                Output.Instance.Script("We are supposed to walk through waypoints but there's no waypoints defined", this);
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