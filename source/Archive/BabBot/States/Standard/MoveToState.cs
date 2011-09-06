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
using BabBot.Wow;
using BabBot.Manager;
using Pather.Graph;
using BabBot.Common;

namespace BabBot.States.Standard
{
    public class MoveToState : State<WowPlayer>
    {
        public Vector3D Destination { get; protected set; }
        public Path TravelPath { get; protected set; }
        public Location CurrentWaypoint { get; protected set; }
        public float Tolerance { get; protected set; }

        protected float _LastDistance = 0f;

        public MoveToState(Vector3D Destination)
        {
            SetDefaults(Destination);
        }

        public MoveToState(Path TravelPath)
        {
            this.TravelPath = TravelPath;

            SetDefaults(new Vector3D());
        }

        protected void SetDefaults(Vector3D Destination)
        {
            this.Destination = Destination;
            Tolerance = 1.0f;
        }

        protected override void DoEnter(WowPlayer Entity)
        {
            //if travel path is not defined then generate from location points
            if (TravelPath == null)
            {
                //get current and destination as ppather locations
                Location currentLocation = new Location(Entity.Location.X, Entity.Location.Y, Entity.Location.Z);
                Location destinationLocation = new Location(Destination.X, Destination.Y, Destination.Z);
                //calculate and store travel path
                TravelPath = ProcessManager.Caronte.CalculatePath(currentLocation, destinationLocation);
            }
            
            //if there are locations then set first waypoint
            if (TravelPath.locations.Count > 0)
            {
                CurrentWaypoint = TravelPath.RemoveFirst();

                Entity.Face(new Vector3D(CurrentWaypoint.X, CurrentWaypoint.Y, CurrentWaypoint.Z));
                _LastDistance = WaypointVector3DHelper.Vector3DToLocation(Entity.Location).GetDistanceTo(CurrentWaypoint);
            }
        }

        protected override void DoExecute(WowPlayer Entity)
        {
            //on execute, first verify we have a waypoit to follow, else exit
            if (CurrentWaypoint == null)  { Exit(Entity); return; }

            //verify we are moving, if we aren't then start moving       
            if (!Entity.IsMoving())
            {
                Entity.MoveForward();
            }
                       
            //get distances to waypoint
            float fDistance = MathFuncs.GetDistance(
                                        WaypointVector3DHelper.LocationToVector3D(CurrentWaypoint),
                                        Entity.Location, false);

            //if distance is growing instead of shrinking them face again
            if (fDistance >_LastDistance)
            {
                Entity.Face(new Vector3D(CurrentWaypoint.X, CurrentWaypoint.Y, CurrentWaypoint.Z));
            }

            //if distance to current waypoint is less then / equal to our tolerance, then move to the next waypoint if it exists, else stop/finish.
            if (fDistance <= Tolerance)
            {
                //if another waypoint exists, then switch to it
                if (TravelPath.GetFirst() != null)
                {
                    CurrentWaypoint = TravelPath.RemoveFirst();
                    Entity.Face(new Vector3D(CurrentWaypoint.X, CurrentWaypoint.Y, CurrentWaypoint.Z));
                }
                else
                {
                    Exit(Entity);
                    Finish(Entity);
                }
            }



            _LastDistance = fDistance;
        }

        protected override void DoExit(WowPlayer Entity)
        {
            //on exit ensure that the player is stopped.
            Entity.StopMoveForward();
        }

        protected override void DoFinish(WowPlayer Entity)
        {
            return;
        }
    }
}
