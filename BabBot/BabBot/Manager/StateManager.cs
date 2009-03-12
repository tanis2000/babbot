using BabBot.Wow;

namespace BabBot.Manager
{
    public class StateManager
    {
        private PlayerState CurrentState;
        private PlayerState LastState;

        public StateManager()
        {
            CurrentState = LastState = PlayerState.Start;
        }

        public void UpdateState()
        {
            LastState = CurrentState;

            if (CurrentState == PlayerState.Start)
            {
                CurrentState = PlayerState.Roaming;
            }

            if (CurrentState == PlayerState.Roaming)
            {
                if (ProcessManager.Player.IsBeingAttacked())
                {
                    CurrentState = PlayerState.PreCombat;
                }
            }

            if (CurrentState == PlayerState.PreCombat)
            {
                CurrentState = PlayerState.InCombat;
            }

            if (CurrentState == PlayerState.InCombat)
            {
                /// We should check if our target died and
                /// in that case go to PostCombat
                //CurrentState = PlayerState.PostCombat;
            }

            if (CurrentState == PlayerState.PostCombat)
            {
                /// We should check if we need to rest
                //CurrentState = PlayerState.PreRest;
            }

            if (CurrentState == PlayerState.PreRest)
            {
                /// We should check if we finished resting
                CurrentState = PlayerState.Rest;
            }

            if (CurrentState == PlayerState.Rest)
            {
                /// We should check if we finished resting
                //CurrentState = PlayerState.PostRest;
            }

            if (CurrentState == PlayerState.PostRest)
            {
                /// We finished resting, go back to roaming
                CurrentState = PlayerState.Roaming;
            }

            if (ProcessManager.Player.IsDead())
            {
                CurrentState = PlayerState.Dead;
            }

            if (ProcessManager.Player.IsGhost())
            {
                CurrentState = PlayerState.Dead;
            }

            if (ProcessManager.Player.IsAtGraveyard())
            {
                CurrentState = PlayerState.Graveyard;
            }
        }
    }
}