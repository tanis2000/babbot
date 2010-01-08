using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using BabBot.Wow;
using BabBot.Common;
using BabBot.Manager;
using BabBot.Wow.Helpers;
using Pather.Graph;

namespace BabBot.States.Common
{
    /// <summary>
    /// Class used to set travel destination for GameObject or Endpoint
    /// Aggro ignored in this state
    /// </summary>
    class TravelState : State<WowPlayer>
    {
        /// <summary>
        /// Logging facility
        /// </summary>
        private string _lfs;

        /// <summary>
        /// Text shown in bot progress bar tooltip
        /// </summary>
        private string _tooltip;

        /// <summary>
        /// Endpoint or GameObject i.e Destination
        /// </summary>
        private object _dest;

        /// <summary>
        /// List of coordinates if more than 1 (for NPC)
        /// </summary>
        List<Vector3D> _vlist = null;

        /// <summary>
        /// Last destination vector that was submit to 
        /// NavigateTo state
        /// </summary>
        Vector3D _last_dest;

        /// <summary>
        /// Destination check class
        /// </summary>
        AbstractCheck _check;

        public TravelState(GameObject obj, string lfs, string tooltip_text)
        {
            _check = new GameObjCheck(this, obj);
            Init(obj, lfs, tooltip_text);
        }

        public TravelState(Endpoint point, string lfs, string tooltip_text)
        {
            _check = new EndpointCheck(this, point);
            Init(point, lfs, tooltip_text);
        }

        void Init(object dest, string lfs, string tooltip_text)
        {
            _dest = dest;
            _lfs = lfs;
            _tooltip = tooltip_text;
        }

        /// <summary>
        /// Enter state event handler
        /// Try locate route or locate destination waypoint and 
        /// launch NavigationState
        /// </summary>
        /// <param name="player"></param>
        protected override void DoEnter(WowPlayer player)
        {
            // Locate route in Endpoints table
            string name = _dest.ToString();

            if (_dest.GetType().IsSubclassOf(typeof(GameObject)))
            {
                GameObject obj = (GameObject)_dest;

                // Check if obj close
                if (LookForGameObjClose(obj))
                {
                    // We already at destination
                    Finish(player);
                    return;
                }
                
                // Check if obj has coordinates
                Vector3D v = NpcHelper.GetGameObjCoord(obj, _lfs);
                if (v == null)
                {
                    // Can't travel
                    Finish(player);
                    return;
                }

                // Check for other coordinates
                if (obj.GetType().IsSubclassOf(typeof(NPC)))
                {
                    NPC npc = (NPC) obj;

                    // Use current zone as a key
                    if (npc.Coordinates.Count > 0)
                    {
                        ZoneWp zwp = npc.Coordinates[player.ZoneText];
                        if (zwp != null)
                            _vlist = zwp.List;
                    }
                }
            }

            Route r = RouteListManager.FindRoute(name);
            if (r != null)
            {
                // Load waypoints and launch NavigationState
                Waypoints wp = RouteListManager.LoadWaypoints(r.WaypointFileName);
                if (wp != null)
                    CallChangeStateEvent(player,
                        new NavigationState(wp, _lfs, "Traveling to " + _dest.ToString()));

                return;
            }

            // Check undef routes
            // r = RouteListManager
        }

        protected override void DoExecute(WowPlayer player)
        {
            // Check if we arrived
            if (_dest.GetType().IsSubclassOf(typeof(GameObject)))
            {
                // Check for another NPC location
                // if (_vlist != null)
            

            }
            else
            {
                if (_last_dest.IsClose(player.Location))
                {
                    Finish(player);
                    return;
                }
            }
        }

        /// <summary>
        /// Check if GameObject can be found around
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool LookForGameObjClose(GameObject obj)
        {
            WowObject wo = ProcessManager.ObjectManager.LookForGameObj(obj.Name);
            if (wo == null)
                return false;

            return wo.Location.IsClose(ProcessManager.Player.Location);
        }
    }

    abstract class AbstractCheck {
        protected TravelState Parent;

        protected AbstractCheck(TravelState parent)
        {
            Parent = parent;
        }

        // abstract bool DoCheck(Vector3D coord);
    }

    class GameObjCheck : AbstractCheck
    {
        GameObject _obj;

        internal GameObjCheck(TravelState parent, GameObject obj)
            : base(parent)
        {
            _obj = obj;
        }
    }

    class EndpointCheck : AbstractCheck
    {
        Endpoint _ep;

        internal EndpointCheck(TravelState parent, Endpoint ep)
            : base(parent)
        {
            _ep = ep;
        }
    }
}
