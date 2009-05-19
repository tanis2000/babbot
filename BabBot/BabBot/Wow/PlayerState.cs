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
namespace BabBot.Wow
{
    /// <summary>
    /// Possible states of the player in game
    /// </summary>
    public enum PlayerState
    {
        ///<summary>
        /// Before selecting a mob
        ///</summary>
        PreMobSelection,
        /// <summary>
        /// After selecting a mob
        /// </summary>
        PostMobSelection,
        /// <summary>
        /// We just started
        /// </summary>
        Start,
        /// <summary>
        /// Cannot reach a waypoint in time
        /// </summary>
        WayPointTimeout,
        /// <summary>
        /// Before resting
        /// </summary>
        PreRest,
        /// <summary>
        /// During rest
        /// </summary>
        Rest,
        /// <summary>
        /// After resting
        /// </summary>
        PostRest,
        /// <summary>
        /// Died
        /// </summary>
        Dead,
        /// <summary>
        /// Spawned at the graveyard
        /// </summary>
        Graveyard,
        /// <summary>
        /// Before resurrecting
        /// </summary>
        PreResurrection,
        /// <summary>
        /// After resurrecting
        /// </summary>
        PostResurrection,
        /// <summary>
        /// Before looting
        /// </summary>
        PreLoot,
        /// <summary>
        /// After looting
        /// </summary>
        PostLoot,
        /// <summary>
        /// Before combat
        /// </summary>
        PreCombat,
        /// <summary>
        /// We are fighting
        /// </summary>
        InCombat,
        /// <summary>
        /// After Combat
        /// </summary>
        PostCombat,
        /// <summary>
        /// At the vendor/repair guy
        /// </summary>
        Sale,
        /// <summary>
        /// We are walking through the waypoints
        /// </summary>
        Roaming,
        /// <summary>
        /// We are ready for the next action
        /// </summary>
        Ready,
        /// <summary>
        /// We stopped botting
        /// </summary>
        Stop
    }
}