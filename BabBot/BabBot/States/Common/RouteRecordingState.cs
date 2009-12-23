using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BabBot.Wow;
using BabBot.Manager;

namespace BabBot.States.Common
{
    /// <summary>
    /// Trace toon movementd and fires waypoing recording event 
    /// when it changes orientation or when it go certain distance
    /// based on recording preferences
    /// </summary>
    class RouteRecordingState : State<WowPlayer>
    {
        public delegate void WaypointRecordingHandler(Vector3D v);

        public event WaypointRecordingHandler OnWaypointRecording;

        /// <summary>
        /// Last toon orientation
        /// </summary>
        private float _angle;
        /// <summary>
        /// Last toon coordinates
        /// </summary>
        private Vector3D _coord;
        /// <summary>
        /// Recording distance. Fire Recording event if toon go more than this parameters
        /// </summary>
        private float _rec_dist;
        /// <summary>
        /// Minimal recording distance. If toon rotating than do not record.
        /// </summary>
        private float _min_dist = 1;

        public RouteRecordingState(WaypointRecordingHandler WpRecordProc, decimal max_dist)
        {
            _rec_dist = (float) max_dist;
            OnWaypointRecording = WpRecordProc;
        }

        protected override void DoEnter(WowPlayer Entity)
        {
            // Remember current player coordinates
            _angle = ProcessManager.Player.Orientation;
            _coord = (Vector3D)ProcessManager.Player.Location.Clone();
        }

        protected override void DoExecute(WowPlayer player)
        {
            Vector3D cur_loc = player.Location;

            if (player.Orientation != _angle)
            {
                // Remember new angle and record coord
                _angle = player.Orientation;
                // Check if we not rotating
                if (cur_loc.GetDistanceTo(_coord) >= _min_dist)
                {
                    _coord = cur_loc;
                    OnWaypointRecording(_coord.CloneVector());
                }
            }
            else if (player.Location.GetDistanceTo(_coord) >= _rec_dist)
            {
                _coord = cur_loc;
                OnWaypointRecording(_coord.CloneVector());
            }
        }

        protected override void DoExit(WowPlayer Entity)
        {
            //on exit we will do nothing
            return;
        }

        protected override void DoFinish(WowPlayer Entity)
        {
            //on finish we will do nothing
            return;
        }
    }
}
