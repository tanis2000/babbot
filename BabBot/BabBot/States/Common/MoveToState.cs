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
using System.Threading;
using System.Windows.Forms;

namespace BabBot.States.Common
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
                //TravelPath.locations = new List<Location>(TravelPath.locations.Distinct<Location>());
            }
            
            //if there are locations then set first waypoint
            if (TravelPath.locations.Count > 0)
            {
                CurrentWaypoint = TravelPath.RemoveFirst();

                //Entity.Face(new Vector3D(CurrentWaypoint.X, CurrentWaypoint.Y, CurrentWaypoint.Z));
                _LastDistance = WaypointVector3DHelper.Vector3DToLocation(Entity.Location).GetDistanceTo(CurrentWaypoint);

                //if the distance to the next waypoint is less then 1f, use the get next waypoint method
                if (_LastDistance < 3f)
                {
                    CurrentWaypoint = GetNextWayPoint();
                    _LastDistance = WaypointVector3DHelper.Vector3DToLocation(Entity.Location).GetDistanceTo(CurrentWaypoint);

                }
            }
        }

        protected override void DoExecute(WowPlayer Entity)
        {
            //on execute, first verify we have a waypoit to follow, else exit
            if (CurrentWaypoint == null)  { Exit(Entity); return; }

            const CommandManager.ArrowKey key = CommandManager.ArrowKey.Up;


            // Move on...
            float distance = MathFuncs.GetDistance(WaypointVector3DHelper.LocationToVector3D(CurrentWaypoint), Entity.Location, false);
            Entity.PlayerCM.ArrowKeyDown(key);

            /// We face our destination waypoint while we are already moving, so that it looks 
            /// more human-like
            float angle = MathFuncs.GetFaceRadian(WaypointVector3DHelper.LocationToVector3D(CurrentWaypoint), Entity.Location);

            Entity.FaceUsingMemoryWrite(angle, false);

            // Start profiler for WayPointTimeOut
            DateTime start = DateTime.Now;

            while (distance > Tolerance)
            {
                float currentDistance = distance;

                distance = MathFuncs.GetDistance(WaypointVector3DHelper.LocationToVector3D(CurrentWaypoint), Entity.Location, false);

                Thread.Sleep(50);
                Application.DoEvents();

                DateTime end = DateTime.Now;
                TimeSpan tsTravelTime = end - start;


                // we take as granted that we should move at least 0.1 yards per cycle (might be a good idea to get this routine synchronized so that 
                // we can actually know exactly how much we move "per-tick")
                if (Math.Abs(currentDistance - distance) < 0.1f && Math.Abs(currentDistance - distance) > 0.0001f)
                {
                    //Console.WriteLine(string.Format("Stuck! Distance difference: {0}", Math.Abs(currentDistance - distance)));
                    Entity.Unstuck();
                }

                //repoint at the waypoint if we are getting off course
                //angle = MathFuncs.GetFaceRadian(WaypointVector3DHelper.LocationToVector3D(CurrentWaypoint), Entity.Location);
                //if (Math.Abs(Entity.Rotation - angle) > 0.1f)
                //{
                //    Entity.FaceUsingMemoryWrite(angle, false);
                //}
            }         

            //get next waypoint (may be null)
            CurrentWaypoint = GetNextWayPoint();

            if (CurrentWaypoint == null)
            {
                Finish(Entity);
                Exit(Entity);
                //stop going forward
                Entity.PlayerCM.ArrowKeyUp(key);
            }
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
                if (distance > 3f)
                {
                    break;
                }
            }

            return Next;
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
            return;
        }
    }
}
