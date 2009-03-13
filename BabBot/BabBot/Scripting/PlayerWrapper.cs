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
    }

    public class PlayerWrapper : IPlayerWrapper
    {
        // Reference to the Player object
        private readonly Player player;

        public PlayerWrapper(Player iPlayer)
        {
            player = iPlayer;
        }

        #region IPlayerWrapper Members

        public string Test()
        {
            return "test da player";
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

        #endregion
    }
}