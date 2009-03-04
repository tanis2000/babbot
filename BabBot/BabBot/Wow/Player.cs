using System;
using System.Collections.Generic;
using System.Text;

namespace BabBot.Wow
{
    public enum PlayerState : int
    {
        PreMobSelection, // Before selecting a mob
        PostMobSelection, // After selecting a mob
        Start, // We just started 
        WayPointTimeout, // Cannot reach a waypoint in time
        PreRest, // Before resting
        Rest, // During rest
        PostRest, // After resting
        Dead, // Died
        Graveyard, // Spawned at the graveyard
        PreResurrection, // Before resurrecting
        PostResurrection, // After resurrecting
        PreLoot, // Before looting
        PostLoot, // After looting
        PreCombat, // Before combat
        PostCombat, // After Combat
        Sale // At the vendor/repair guy
    }

    public class Player
    {
        public uint Hp;
        public uint MaxHp;
        public uint Mp;
        public uint MaxMp;
        public uint Xp;
        public Vector3D Location;
        public UInt64 CurTargetGuid;
        public Unit Unit; // The corresponding Unit in Wow's ObjectManager
        public float Orientation;
        public PlayerState State;

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

        public void AttachUnit(uint ObjectPointer)
        {
            Unit = new Unit(ObjectPointer);
        }

        public void UpdateFromClient()
        {
            if (Unit == null) return;

            Location = Unit.GetPosition();
            Hp = Unit.GetHp();
            MaxHp = Unit.GetMaxHp();
            Mp = Unit.GetMp();
            MaxMp = Unit.GetMaxMp();
            Xp = Unit.GetXp();
            //CurTargetGuid = Unit.GetCurTargetGuid();
            Orientation = Unit.GetFacing();
        }
    }
}
