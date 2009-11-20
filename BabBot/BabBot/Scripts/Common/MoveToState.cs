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
using System.Threading;
using System.Windows.Forms;
using BabBot.Common;
using BabBot.Manager;
using BabBot.Wow;
using Pather.Graph;
using BabBot.States;

namespace BabBot.Scripts.Common
{
    public class MoveToState : State<WowPlayer>
    {
        protected float _LastDistance;
        protected static Vector3D _LastDestination;

        public MoveToState(Vector3D Destination)
        {
            SetDefaults(Destination);
        }

        public MoveToState(Vector3D Destination, float iTolerance)
        {
            SetDefaults(Destination, iTolerance);
        }

        public MoveToState(Path iTravelPath)
        {
            TravelPath = iTravelPath;

            SetDefaults(new Vector3D());
        }

        public Vector3D Destination { get; protected set; }
        public static Path TravelPath { get; protected set; }
        public Location CurrentWaypoint { get; protected set; }
        public float Tolerance { get; protected set; }

        protected void SetDefaults(Vector3D iDestination, float iTolerance)
        {
            Destination = iDestination;
            Tolerance = iTolerance;
        }

        protected void SetDefaults(Vector3D iDestination)
        {
            SetDefaults(iDestination, 3.0f);
        }

        protected override void DoEnter(WowPlayer Entity)
        {
            //if travel path is not defined then generate from location points
            if ((TravelPath == null) || (TravelPath.locations.Count == 0))
            {
                //get current and destination as ppather locations
                var currentLocation = new Location(Entity.Location.X, Entity.Location.Y, Entity.Location.Z);
                var destinationLocation = new Location(Destination.X, Destination.Y, Destination.Z);
                //calculate and store travel path
                Output.Instance.Script("Calculating path started.", this);
                TravelPath = ProcessManager.Caronte.CalculatePath(currentLocation, destinationLocation);
                //TravelPath.locations = new List<Location>(TravelPath.locations.Distinct<Location>());
                Output.Instance.Script("Calculating path finished.", this);
            }

            if (_LastDestination != null)
            {
                _LastDistance = Entity.Location.GetDistanceTo(_LastDestination);
                if (_LastDistance > 3f)
                {
                    CurrentWaypoint = WaypointVector3DHelper.Vector3DToLocation(_LastDestination);
                }
            }
            else
            {

                //if there are locations then set first waypoint
                if (TravelPath.locations.Count > 0)
                {
                    CurrentWaypoint = TravelPath.RemoveFirst();

                    if (CurrentWaypoint == null) return;

                    //Entity.Face(new Vector3D(CurrentWaypoint.X, CurrentWaypoint.Y, CurrentWaypoint.Z));
                    _LastDistance =
                        WaypointVector3DHelper.Vector3DToLocation(Entity.Location).GetDistanceTo(CurrentWaypoint);

                    //if the distance to the next waypoint is less then 1f, use the get next waypoint method
                    if (_LastDistance < 3f)
                    {
                        CurrentWaypoint = GetNextWayPoint();
                        _LastDistance =
                            WaypointVector3DHelper.Vector3DToLocation(Entity.Location).GetDistanceTo(CurrentWaypoint);
                    }
                }
            }
        }

        protected override void DoExecute(WowPlayer Entity)
        {
            //on execute, first verify we have a waypoit to follow, else exit
            if (CurrentWaypoint == null)
            {
                Exit(Entity);
                return;
            }

            // Move on...
            float distance = MathFuncs.GetDistance(WaypointVector3DHelper.LocationToVector3D(CurrentWaypoint),
                                                   Entity.Location, false);
            if (Math.Abs(distance - _LastDistance) < 1.0)
            {
                TravelPath = null;
            }
            /// We face our destination waypoint while we are already moving, so that it looks 
            /// more human-like
            //float angle = MathFuncs.GetFaceRadian(WaypointVector3DHelper.LocationToVector3D(CurrentWaypoint),
            //                                      Entity.Location);

            Output.Instance.Script(string.Format("Entity Location: X:{0} Y:{1} Z:{2}", Entity.Location.X, Entity.Location.Y, Entity.Location.Z), this);
            Output.Instance.Script(string.Format("Path Waypoint: X:{0} Y:{1} Z:{2}", CurrentWaypoint.X, CurrentWaypoint.Y, CurrentWaypoint.Z), this);
            //Entity.FaceUsingMemoryWrite(angle, true);

            Output.Instance.Script(string.Format("First ClickToMove(X:{0} Y:{1} Z:{2})", CurrentWaypoint.X, CurrentWaypoint.Y, CurrentWaypoint.Z), this);
            Entity.ClickToMove(WaypointVector3DHelper.LocationToVector3D(CurrentWaypoint));
            Finish(Entity);
            Exit(Entity);
        }

        protected Location GetNextWayPoint()
        {
            Location Next = null;

            //get the next waypoint
            // The only criteria is that the next waypoint be at least 3 yards away from current
            // if all fail then skip to end
            while (TravelPath.GetFirst() != null)
            {
                //get next waypoint and remove it from the list at the same time
                Next = TravelPath.RemoveFirst();

                //check distance to the waypoint
                float distance = CurrentWaypoint.GetDistanceTo(Next);

                //if distance greater then 3f then return this waypoint
                if (distance > 10f)
                {
                    break;
                }
            }

            return Next;
        }

        protected override void DoExit(WowPlayer Entity)
        {
            _LastDestination = Destination;
            //on exit, if there is a previous state, go back to it
            if (PreviousState != null)
            {
                CallChangeStateEvent(Entity, PreviousState, false, false);
            }
        }

        protected override void DoFinish(WowPlayer Entity)
        {
            return;
        }
    }
}