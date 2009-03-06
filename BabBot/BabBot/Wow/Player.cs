using System;

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
        /// After Combat
        /// </summary>
        PostCombat,
        /// <summary>
        /// At the vendor/repair guy
        /// </summary>
        Sale
    }

    /// <summary>
    /// Stores all the information about the player
    /// </summary>
    public class Player
    {
        public UInt64 CurTargetGuid;
        public uint Hp;
        public Vector3D Location;
        public uint MaxHp;
        public uint MaxMp;
        public uint Mp;
        public float Orientation;
        public PlayerState State;
        public Unit Unit; // The corresponding Unit in Wow's ObjectManager
        public uint Xp;

        /// <summary>
        /// Constructor
        /// </summary>
        public Player()
        {
            Location = new Vector3D();
            Hp = 0;
            MaxHp = 0;
            Mp = 0;
            MaxMp = 0;
            Xp = 0;
            CurTargetGuid = 0;
            Orientation = 0.0f;
            State = PlayerState.Start;
        }

        /// <summary>
        /// Creates a new Unit object and attach it to the client's memory so 
        /// that we can therefore call UpdateFromClient() to read the
        /// player's information
        /// </summary>
        /// <param name="ObjectPointer">Pointer to Wow's ObjectManager</param>
        public void AttachUnit(uint ObjectPointer)
        {
            Unit = new Unit(ObjectPointer);
        }

        /// <summary>
        /// Reads the player information from Wow's ObjectManager
        /// </summary>
        public void UpdateFromClient()
        {
            if (Unit == null)
            {
                return;
            }

            Location = Unit.GetPosition();
            Hp = Unit.GetHp();
            MaxHp = Unit.GetMaxHp();
            Mp = Unit.GetMp();
            MaxMp = Unit.GetMaxMp();
            Xp = Unit.GetXp();
            //CurTargetGuid = Unit.GetCurTargetGuid();
            Orientation = Unit.GetFacing();
        }

        public bool IsAtGraveyard()
        {
            throw new NotImplementedException();
        }

        public bool IsDead()
        {
            throw new NotImplementedException();
        }

        public bool IsGhost()
        {
            throw new NotImplementedException();
        }

        public bool IsInCombat()
        {
            throw new NotImplementedException();
        }
    }
}