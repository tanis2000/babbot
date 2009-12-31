using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using BabBot.Wow;
using BabBot.Common;
using BabBot.Manager;
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

        public TravelState(GameObject obj, string lfs, string tooltip_text)
        {
            Init(obj, lfs, tooltip_text);
        }

        public TravelState(Endpoint point, string lfs, string tooltip_text)
        {
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
            Route r = RouteListManager.FindRoute(name);
        }

        protected override void DoExecute(WowPlayer player)
        {
            // Finish(player);
        }
    }
}
