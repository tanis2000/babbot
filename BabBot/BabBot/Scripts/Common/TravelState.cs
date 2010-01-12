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
        public string Lfs;

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
        public List<Vector3D> Vlist = null;

        /// <summary>
        /// Last destination vector that was submit to 
        /// NavigateTo state
        /// </summary>
        Vector3D _last_dest;

        /// <summary>
        /// Destination check class
        /// </summary>
        AbstractCheck _check;

        public WowPlayer Player;

        public TravelState(GameObject obj, string lfs, string tooltip_text)
        {
            _check = new GameObjCheck(this,  obj);
            Init(obj, lfs, tooltip_text);
        }

        public TravelState(Endpoint point, string lfs, string tooltip_text)
        {
            _check = new EndpointCheck(this, point);
            Init(point, lfs, tooltip_text);
        }

        void Init(object dest, string lfs, string tooltip_text)
        {
            Lfs = lfs;
            _dest = dest;
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
            Player = player;

            // Locate route in Endpoints table
            string name = _dest.ToString();
            Vector3D cur_loc = player.Location;

            try
            {
                if (!_check.DoBeforeRouteCheck())
                {
                    // We already at dest or something wrong
                    Finish(player);
                    return;
                }
            }
            catch
            {
                Finish(player);
                return;
            }

            // Find all avail routes for dest
            List<Route> lr = RouteListManager.Endpoints[name];
            bool calc_route = true;

            // Min dist to Endpoint B (if any)
            float min_dist_nb = float.MaxValue;
            foreach (Route r in lr)
            {
                // It's either A or B
                Endpoint[] eps = r.GetEndpoints(name);
                if (eps == null)
                    return;

                if (CheckEndpoint(eps[1], r, player, cur_loc))
                    return;

                // Exact route not found
                // Check distance to endpoint vs distance to destination
                float dist_b = eps[1].Waypoint.GetDistanceTo(cur_loc);
                float dist_a = eps[0].Waypoint.GetDistanceTo(cur_loc);

                // Calc new min dist to B endpoint and
                // set calc flag if it shorter than direct path
                // to Endpoint A
                min_dist_nb = Math.Min(min_dist_nb, dist_b);
                calc_route = calc_route && (dist_a < dist_b);
            }
            
            // Check among undef routes
            // r = RouteListManager.FindRoute(
        }

        private bool CheckEndpoint(Endpoint ep, Route r, 
                        WowPlayer player, Vector3D cur_loc)
        {
            if (ep.Waypoint.IsClose(cur_loc))
            {
                // Found exact route
                ActivateMoveState(r, player);
                return true;
            }

            return false;
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

        private void ActivateMoveState(Route r, WowPlayer player)
        {
            // Load waypoints and launch NavigationState
            Waypoints wp = RouteListManager.LoadWaypoints(r.WaypointFileName);
            if (wp != null)
            {
                if (r.Reversible)
                    wp.List.Reverse();

                CallChangeStateEvent(player, new NavigationState(wp, 
                                Lfs, "Traveling to " + _dest.ToString()));
            }
        }
       
    }

    abstract class AbstractCheck {
        protected TravelState Parent;
        protected string Lfs;

        protected AbstractCheck(TravelState parent)
        {
            Parent = parent;
        }

        public virtual bool DoBeforeRouteCheck() { return true; }
    }

    class GameObjCheck : AbstractCheck
    {
        GameObject _obj;

        internal GameObjCheck(TravelState parent, GameObject obj)
            : base(parent)
        {
            _obj = obj;
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

        public override bool DoBeforeRouteCheck()
        {
            // Check if obj close
            if (LookForGameObjClose(_obj))
                // We already at destination
                return false;

            // Check if obj has coordinates
            Vector3D v = NpcHelper.GetGameObjCoord(_obj, Parent.Lfs);
            if (v == null)
                throw new GameObjectCoordNotFound(_obj.Name);

            // Check for other coordinates
            if (_obj.GetType().IsSubclassOf(typeof(NPC)))
            {
                NPC npc = (NPC)_obj;

                // Use current zone as a key
                if (npc.Coordinates.Count > 0)
                {
                    ZoneWp zwp = npc.Coordinates[Parent.Player.ZoneText];
                    if (zwp != null)
                        Parent.Vlist = zwp.List;
                }
            }

            return true;
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
