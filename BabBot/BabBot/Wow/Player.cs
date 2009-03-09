using System;
using System.Collections.Generic;
using System.Threading;
using BabBot.Common;
using BabBot.Manager;

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
        private readonly CommandManager PlayerCM;
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
        public Player(CommandManager cm)
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
            PlayerCM = cm;
        }

        public string CurTargetName
        {
            get { return Unit.GetCurTargetName(); }
        }

        public string NearObjectsAsTextList
        {
            get
            {
                string s = string.Empty;

                List<WowObject> l = Unit.GetNearObjects();
                foreach (WowObject obj in l)
                {
                    s += string.Format("GUID:{0:X}|Type:{1:X}\r" + Environment.NewLine, obj.Guid, obj.Type);
                }
                return s;
            }
        }

        public string NearMobsAsTextList
        {
            get
            {
                string s = string.Empty;

                List<WowUnit> l = Unit.GetNearMobs();
                foreach (WowUnit obj in l)
                {
                    s += string.Format("{0:X}|{1}" + Environment.NewLine, obj.Guid, obj.Name);
                }
                return s;
            }
        }

        /// <summary>
        /// Creates a new Unit object and attach it to the client's memory so 
        /// that we can therefore call UpdateFromClient() to read the
        /// player's information
        /// </summary>
        /// <param name="ObjectPointer">Pointer to Wow's Player Object</param>
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
            CurTargetGuid = Unit.GetCurTargetGuid();
            Orientation = Unit.GetFacing();
        }

        public bool IsAtGraveyard()
        {
            List<WowUnit> l = Unit.GetNearMobs();
            foreach (WowUnit unit in l)
            {
                if (unit.Name == "Spirit Healer")
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsDead()
        {
            if (Hp == 0)
            {
                return true;
            }
            return false;
        }

        public bool IsGhost()
        {
            // return ( GetKnownField( PLAYER_FLAGS )&0x10 ) != 0;
            // 
            throw new NotImplementedException();
        }

        public bool IsInCombat()
        {
            throw new NotImplementedException();
        }

        public bool HasTarget()
        {
            if (CurTargetGuid != 0)
            {
                return true;
            }
            return false;
        }


        // Movements
        // E' solo un'idea, se ti sembra na cazzata spostiamoli
        // Lo scopo sarebbe di riuscire ad astrarre il più possibile
        // al fine di poter controllare il toon comodamente, ma il sistema
        // di navigazione dovrebbe essere esterno alla classe Player
        // Nella fase di movimento è determinante controllare:
        // 1) se non si è attaccati da qualche mobs
        // 2) se non si è bloccati contro qualche ostacolo

        /// <summary>
        /// Returns the radian that would face the x,y specified
        /// </summary>
        /// <param name="dest">Vector3D current destination points</param>
        /// <returns>radian to face on</returns>
        private float GetFaceRadian(Vector3D dest)
        {
            Vector3D currentPos = Location;
            float dx = (currentPos.X - dest.X);
            float dy = (dest.Y - currentPos.Y);

            float res = 0;
            if (dx > 0.0 && dy >= 0.0)
            {
                res = (float) Math.Atan(dy/dx);
            }
            if (dx < 0.0 && dy >= 0.0)
            {
                res = (float) Math.Atan(dy/dx) + (float) Math.PI;
            }
            if (dx < 0.0 && dy < 0.0)
            {
                res = (float) Math.Atan(dy/dx) + (float) Math.PI;
            }
            if (dx > 0.0 && dy < 0.0)
            {
                res = (float) Math.Atan(dy/dx) + (float) (2*Math.PI);
            }

            // da testare, sembra che nel gioco l'angolo 0 sia puntato ad east 
            res -= (float) 1.57079633;

            return res;
        }

        private float GetDistance(Vector3D dest)
        {
            Vector3D currentPos = Location;

            float dX = currentPos.X - dest.X;
            float dY = currentPos.Y - dest.Y;
            float dZ = (dest.Z != 0 ? currentPos.Z - dest.Z : 0);

            return (float)Math.Sqrt(dX * dX + dY * dY + dZ * dZ);
        }

        public bool GoForward(Vector3D dest)
        {
            float radian = GetFaceRadian(dest);
            float distance = GetDistance(dest);

            Face(radian);


        }

        public bool GoBack(Vector3D dest)
        {
            throw new NotImplementedException();
        }

        private void FaceWithTimer(float facing, CommandManager.ArrowKey key)
        {
            var tm = new GTimer(facing*1000*Math.PI);
            PlayerCM.ArrowKeyDown(key);
            tm.Reset();
            while (!tm.isReady())
            {
                Thread.Sleep(1);
            }
            PlayerCM.ArrowKeyUp(key);
        }

        public void Face(float facing)
        {
            if (facing - Orientation < Math.PI)
            {
                FaceWithTimer(facing, CommandManager.ArrowKey.Left);
            }
            else
            {
                FaceWithTimer(facing, CommandManager.ArrowKey.Right);
            }
        }

        // Actions
        public bool Cast(string SlotBar, string Key)
        {
            throw new NotImplementedException();
        }

        // Messages
        public bool Say(string To, string Message)
        {
            throw new NotImplementedException();
        }
    }
}