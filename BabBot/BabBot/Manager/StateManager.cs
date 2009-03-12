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