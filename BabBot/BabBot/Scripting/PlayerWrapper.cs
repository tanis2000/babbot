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
using BabBot.Bot;
using BabBot.Wow;

namespace BabBot.Scripting
{
    public interface IPlayerWrapper
    {
        string Test();
        bool IsSitting();
        bool IsDead();
        bool IsGhost();
        PlayerState State();
        bool IsBeingAttacked();
        bool SelectWhoIsAttackingUs();
        void FaceTarget();
        float DistanceFromTarget();
        float Facing();
        float TargetFacing();
        float AngleToTarget();
        float FacingDegrees();
        float TargetFacingDegrees();
        float AngleToTargetDegrees();
        void WalkToNextWayPoint(Bot.WayPointType wpType);
        void Stop();
        void PlayAction(PlayerAction action, bool toggle);
        void PlayAction(PlayerAction action);
    }

    public class PlayerWrapper : IPlayerWrapper
    {
        // Reference to the Player object
        private readonly WowPlayer player;

        public PlayerWrapper(WowPlayer iPlayer)
        {
            player = iPlayer;
        }

        #region IPlayerWrapper Members

        public string Test()
        {
            return "player.Test()";
        }

        public bool IsSitting()
        {
            return player.IsSitting();
        }

        public bool IsDead()
        {
            return player.IsDead();
        }

        public bool IsGhost()
        {
            return player.IsGhost();
        }

        public PlayerState State()
        {
            return player.State;
        }

        public bool IsBeingAttacked()
        {
            return player.IsBeingAttacked();
        }

        public bool SelectWhoIsAttackingUs()
        {
            return player.SelectWhoIsAttackingUs();
        }

        public void FaceTarget()
        {
            player.FaceTarget();
        }

        public float DistanceFromTarget()
        {
            return player.DistanceFromTarget();
        }

        public float Facing()
        {
            return player.Facing();
        }

        public float TargetFacing()
        {
            return player.TargetFacing();
        }

        public float AngleToTarget()
        {
            return player.AngleToTarget();
        }

        public float FacingDegrees()
        {
            return player.FacingDegrees();
        }

        public float TargetFacingDegrees()
        {
            return player.TargetFacingDegrees();
        }

        public float AngleToTargetDegrees()
        {
            return player.AngleToTargetDegrees();
        }

        public void WalkToNextWayPoint(Bot.WayPointType wpType)
        {
            player.WalkToNextWayPoint(wpType);
        }

        public void Stop()
        {
            player.Stop();
        }

        public void PlayAction(PlayerAction action, bool toggle)
        {
            player.PlayAction(action, toggle);
        }

        public void PlayAction(PlayerAction action)
        {
            player.PlayAction(action);
        }

        #endregion
    }
}