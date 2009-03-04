using System;
using System.Collections.Generic;
using System.Text;

namespace BabBot.Wow
{
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
        public uint Orientation;

        public Player()
        {
            Location = new Vector3D();
            Hp = 0;
            MaxHp = 0;
            Mp = 0;
            MaxMp = 0;
            Xp = 0;
            CurTargetGuid = 0;
            Orientation = 0;
        }
    }
}
