using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    }

    public class PlayerWrapper : IPlayerWrapper
    {
        // Reference to the Player object
        private Player player;

        public PlayerWrapper(Player iPlayer)
        {
            player = iPlayer;
        }

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
    }
}
