using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

            if (ProcessManager.Player.IsAtGraveyard())
            {
                CurrentState = PlayerState.Graveyard;
            }
        }
    }
}
