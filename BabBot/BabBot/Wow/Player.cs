using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
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
        /// We are fighting
        /// </summary>
        InCombat,
        /// <summary>
        /// After Combat
        /// </summary>
        PostCombat,
        /// <summary>
        /// At the vendor/repair guy
        /// </summary>
        Sale,
        /// <summary>
        /// We are walking through the waypoints
        /// </summary>
        Roaming
    }

    /// <summary>
    /// Stores all the information about the player
    /// </summary>
    public class Player
    {
        private readonly CommandManager PlayerCM;
        public UInt64 CurTargetGuid;
        public uint Hp;
        public float LastDistance;
        public float LastFaceRadian;
        public Vector3D LastLocation;
        public Vector3D Location;
        public uint MaxHp;
        public uint MaxMp;
        public uint Mp;
        public float Orientation;
        public PlayerState State;
        private bool StopMovement;
        public Unit Unit; // The corresponding Unit in Wow's ObjectManager
        public uint Xp;
        public UInt64 Guid; // Our own GUID

        /// <summary>
        /// Constructor
        /// </summary>
        public Player(CommandManager cm)
        {
            Location = new Vector3D();
            LastLocation = new Vector3D();
            Hp = 0;
            MaxHp = 0;
            Mp = 0;
            MaxMp = 0;
            Xp = 0;
            CurTargetGuid = 0;
            Orientation = 0.0f;
            State = PlayerState.Start;
            PlayerCM = cm;
            LastDistance = 0.0f;
            LastFaceRadian = 0.0f;
            StopMovement = false;
            Guid = 0;
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

            Guid = ProcessManager.ObjectManager.GetLocalGUID();
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
            if (Hp <= 0)
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

        public bool IsAggro()
        {
            return Unit.IsAggro();
        }

        public bool IsSitting()
        {
            return Unit.IsSitting();
        }

        public bool IsBeingAttacked()
        {
            /// We check the list of mobs around us and if any of them has us as target and has aggro
            /// it means that we are being attacked by that mob. It can be more than one mob
            /// but we don't care. As long as one is attacking us, this will return true.
            List<WowUnit> l = Unit.GetNearMobs();
            foreach (WowUnit obj in l)
            {
                if (obj.HasTarget())
                {
                    if (obj.CurTargetGuid ==  this.Guid)
                    {
                        if (obj.IsAggro())
                        {
                            // Ok this really means that this mob is attacking us
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public bool IsTargetDead()
        {
            if (HasTarget())
            {
                WowUnit target = Unit.GetCurTarget();
                if (target.IsDead())
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsInCombat()
        {
            /// We should check if we are in combat (aka the resting buttons are greyed out)
            /// We could as well make a method called CanRest() that calls this one
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

        public Vector3D GetTargetLocation()
        {
            if (HasTarget())
            {
                WowUnit target = Unit.GetCurTarget();
                return target.Location;
            }

            return new Vector3D(0, 0, 0);
        }

        /// <summary>
        /// Returns the angle that would face the x,y specified
        /// </summary>
        /// <param name="dest">Vector3D current destination points</param>
        /// <returns>radian to face on</returns>
        private float GetFaceRadian(Vector3D dest)
        {
            Vector3D currentPos = Location;
            float wowFacing = negativeAngle((float) Math.Atan2((dest.Y - currentPos.Y), (dest.X - currentPos.X)));
            LastFaceRadian = wowFacing;
            return wowFacing;
        }

        private static float negativeAngle(float angle)
        {
            if (angle < 0)
            {
                angle += (float) (Math.PI*2);
            }
            return angle;
        }

        public float HGetDistance(Vector3D dest, bool UseZ)
        {
            Vector3D currentPos = Location;

            float dX = currentPos.X - dest.X;
            float dY = currentPos.Y - dest.Y;
            float dZ = (dest.Z != 0 ? currentPos.Z - dest.Z : 0);

            float res = 0;
            if (UseZ)
            {
                res = (float) Math.Sqrt(dX*dX + dY*dY + dZ*dZ);
            }
            else
            {
                res = (float) Math.Sqrt(dX*dX + dY*dY);
            }

            LastDistance = res;

            return res;
        }

        private int LGetDistance(Vector3D dest, bool UseZ)
        {
            Vector3D currentPos = Location;
            var dX = (int) (currentPos.X - dest.X);
            var dY = (int) (currentPos.Y - dest.Y);
            int res = 0;

            res = (int) Math.Sqrt(dX*dX + dY*dY);

            LastDistance = res;
            return res;
        }

        private static int RandomNumber(int min, int max)
        {
            var random = new Random();
            return random.Next(min, max);
        }

        public void MoveTo(Vector3D dest)
        {
            if (!StopMovement)
            {
                const CommandManager.ArrowKey key = CommandManager.ArrowKey.Up;

                float angle = GetFaceRadian(dest);
                Face(angle);

                // Random jump
                int rndJmp = RandomNumber(1, 8);
                bool doJmp = false;
                if (rndJmp == 1 || rndJmp == 3)
                {
                    doJmp = true;
                }

                float distance = HGetDistance(dest, false);

                Vector3D currentPos = Location;

                PlayerCM.ArrowKeyDown(key);
                while ((int) distance > 1)
                {
                    if (currentPos.Equals(Location))
                    {
                        //break;
                    }

                    if (StopMovement)
                    {
                        break;
                    }

                    if (doJmp)
                    {
                        doJmp = false;
                        PlayerCM.SendKeys(" ");
                    }

                    currentPos = Location;
                    distance = HGetDistance(dest, false);
                    Thread.Sleep(50);
                    Application.DoEvents();
                }
                PlayerCM.ArrowKeyUp(key);
            }
        }

        public void Stop()
        {
            StopMovement = true;
        }

        public bool GoBack(Vector3D dest)
        {
            throw new NotImplementedException();
        }

        private void FaceWithTimer(float radius, CommandManager.ArrowKey key)
        {
            var tm = new GTimer(radius*1000*Math.PI);
            PlayerCM.ArrowKeyDown(key);
            tm.Reset();
            while (!tm.isReady())
            {
                Thread.Sleep(1);
            }
            PlayerCM.ArrowKeyUp(key);
        }

        public void Face(float angle)
        {
            float face;
            if (negativeAngle(angle - Orientation) < Math.PI)
            {
                face = negativeAngle(angle - Orientation);
                FaceWithTimer(face, CommandManager.ArrowKey.Left);
            }
            else
            {
                face = negativeAngle(Orientation - angle);
                FaceWithTimer(face, CommandManager.ArrowKey.Right);
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