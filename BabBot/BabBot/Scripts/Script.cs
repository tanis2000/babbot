using System;
using BabBot.Scripting;
using BabBot.Wow;

public class Script : IScript
{
    IHost parent;
    IPlayerWrapper player;
    IHost IScript.Parent { set { parent = value; } }
    IPlayerWrapper IScript.Player { set { player = value; } }

    /// <summary>
    /// Local script initialization. Not much to do here at the moment
    /// </summary>
    void IScript.Init()
    {
        
    }

    /// <summary>
    /// Called at every update of the player data from the main thread of the bot
    /// aka the main routine
    /// </summary>
    void IScript.Update()
    {
		Console.WriteLine("Update() -- Begin");
        Console.WriteLine("Current State: " + player.State());
        switch (player.State())
        {
            case PlayerState.PreMobSelection:
                break;
            case PlayerState.PostMobSelection:
                break;
            case PlayerState.Start:
                OnStart();
                break;
            case PlayerState.WayPointTimeout:
                break;
            case PlayerState.PreRest:
                break;
            case PlayerState.Rest:
                break;
            case PlayerState.PostRest:
                break;
            case PlayerState.Dead:
                OnDead();
                break;
            case PlayerState.Graveyard:
                OnGraveyard();
                break;
            case PlayerState.PreResurrection:
                break;
            case PlayerState.PostResurrection:
                break;
            case PlayerState.PreLoot:
                break;
            case PlayerState.PostLoot:
                break;
            case PlayerState.PreCombat:
                OnPreCombat();
                break;
            case PlayerState.InCombat:
                break;
            case PlayerState.PostCombat:
                break;
            case PlayerState.Sale:
                break;
            case PlayerState.Roaming:
                OnRoaming();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        Console.WriteLine("Update() -- End");
    }

    /// <summary>
    /// Called when we have attached ourselves to WoW's client.
    /// This state happens only once of course
    /// </summary>
    private void OnStart()
    {
        // No ideas for now :)
    }

    /// <summary>
    /// We are either a ghost or we just died
    /// </summary>
    private void OnDead()
    {
        /// We should run back to our corpse
    }

    /// <summary>
    /// We are near the graveyard (aka the spirit healer is in range)
    /// </summary>
    private void OnGraveyard()
    {
        /// If we managed to get to the graveyard and we have already run around
        /// like a crazy chicken without finding our own body, well.. it's time
        /// to resurrect at the spirit healer
    }

    /// <summary>
    /// We are roaming through the waypoints with nothing else to do
    /// </summary>
    private void OnRoaming()
    {
        /// This is where we should walk through the waypoints and 
        /// check what happens around us (like if there's anything to
        /// attack or anything attacking us, or if we run out of mana/health,
        /// or if we should rebuff something)
    }

    private void OnPreCombat()
    {
        
    }
}

