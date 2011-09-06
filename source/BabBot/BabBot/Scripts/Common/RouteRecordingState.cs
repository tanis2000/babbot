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

        /// <summary>
        /// Is recording done for dead state
        /// </summary>
        bool _dead_state;

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="WpRecordProc">Callback function on recording event</param>
        /// <param name="max_dist">Max distance between waypoints before fire recording event</param>
        public RouteRecordingState(WaypointRecordingHandler WpRecordProc, decimal max_dist, bool dead_state)
        {
            dead_state = _dead_state;
            _rec_dist = (float) max_dist;
            OnWaypointRecording = WpRecordProc;
        }

        /// <summary>
        /// Enter State handler
        /// </summary>
        /// <param name="Entity">Pointer on Player object</param>
        protected override void DoEnter(WowPlayer player)
        {
            // Remember current player coordinates
            _angle = player.Orientation;
            _coord = player.Location;

            // Record first coordinates
            OnWaypointRecording(_coord.CloneVector());
        }

        /// <summary>
        /// Execute State handler
        /// </summary>
        /// <param name="player"></param>
        protected override void DoExecute(WowPlayer player)
        {
            // Ignore dead if in deadstate
            if (!_dead_state && player.IsDead)
            {
                Finish(player);
                return;
            }

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
    }
}
