/*
    This file is part of BabBot.

    BabBot is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    BabBot is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with BabBot.  If not, see <http://www.gnu.org/licenses/>.
  
    Copyright 2009 BabBot Team
*/
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using BabBot.Bot;
using BabBot.Common;
using BabBot.Manager;
using Pather.Graph;

namespace BabBot.Wow
{
    public class WowPlayer : WowObject
    {
        private const int MAX_REACHTIME = 5000; // Milliseconds to reach the waypoint
        private readonly CommandManager PlayerCM;
        public float LastDistance;
        public float LastFaceRadian;
        public Vector3D LastLocation;
        private bool StopMovement;
        public int TravelTime;

        private WowUnit LastTargetedMob;
        private List<WowUnit> LootableList;

        private readonly Unit unit; // The corresponding Unit in Wow's ObjectManager

        /// <summary>
        /// Constructor
        /// </summary>
        public WowPlayer(WowObject wo, CommandManager cm)
        {
            Guid = wo.Guid;
            ObjectPointer = wo.ObjectPointer;
            Type = wo.Type;
            unit = new Unit(ObjectPointer);

            LastLocation = new Vector3D();

            //State = PlayerState.Start;
            PlayerCM = cm;
            LastDistance = 0.0f;
            LastFaceRadian = 0.0f;
            StopMovement = false;
            TravelTime = 0;
            LootableList = new List<WowUnit>();
        }



        #region Stats
        
        public uint Hp
        {
            get { return unit.GetHp(); }
        }

        public uint MaxHp
        {
            get { return unit.GetMaxHp(); }
        }

        public uint MaxMp
        {
            get { return unit.GetMaxHp(); }
        }

        public uint Mp
        {
            get { return unit.GetMp(); }
        }

        public uint Xp
        {
            get { return unit.GetXp(); }
        }

        #endregion

        public Vector3D Location
        {
            get { return unit.GetPosition(); }
        }

        public float Orientation
        {
            get { return unit.GetFacing(); }
        }

        public UInt64 CurTargetGuid
        {
            get { return unit.GetCurTargetGuid(); }
        }

        public string CurTargetName
        {
            get { return unit.GetCurTargetName(); }
        }

        public string NearObjectsAsTextList
        {
            get
            {
                string s = string.Empty;

                List<WowObject> l = unit.GetNearObjects();
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

                List<WowUnit> l = unit.GetNearMobs();
                foreach (WowUnit obj in l)
                {
                    s += string.Format("{0:X}|{1}|Lootable:{2}" + Environment.NewLine, obj.Guid, obj.Name, obj.IsLootable());
                }
                return s;
            }
        }

        public string BagsAsTextList
        {
            get
            {
                string s = string.Empty;
                try
                {
                    List<WowContainer> l = unit.GetBags();
                    foreach (WowContainer obj in l)
                    {
                        s += string.Format("{0:X}|Slots:{1}|Empty:{2}" + Environment.NewLine, obj.Guid, obj.GetSlots(),
                                           obj.GetEmptySlots());
                    }
                } catch (Exception ex)
                {
                    throw (ex);
                }
                return s;
            }
        }

        public PlayerState State
        {
            get { return ProcessManager.BotManager.State; }
        }


        public bool IsAtGraveyard()
        {
            List<WowUnit> l = unit.GetNearMobs();
            foreach (WowUnit u in l)
            {
                if (u.Name == "Spirit Healer")
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
            return unit.IsGhost();
        }

        public bool IsAggro()
        {
            return unit.IsAggro();
        }

        public bool IsSitting()
        {
            return unit.IsSitting();
        }

        public bool IsBeingAttacked()
        {
            /// We check the list of mobs around us and if any of them has us as target and has aggro
            /// it means that we are being attacked by that mob. It can be more than one mob
            /// but we don't care. As long as one is attacking us, this will return true.
            List<WowUnit> l = unit.GetNearMobs();
            foreach (WowUnit obj in l)
            {
                if (obj.HasTarget())
                {
                    if (obj.CurTargetGuid == Guid)
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

        public WowUnit GetCurAttacker()
        {
            List<WowUnit> l = unit.GetNearMobs();
            foreach (WowUnit obj in l)
            {
                if (obj.HasTarget())
                {
                    if (obj.CurTargetGuid == Guid)
                    {
                        if (obj.IsAggro())
                        {
                            // Ok this really means that this mob is attacking us
                            return obj;
                        }
                    }
                }
            }
            return null;
        }

        public bool IsTargetDead()
        {
            if (HasTarget())
            {
                WowUnit target = unit.GetCurTarget();
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
            throw new NotImplementedException("IsInCombat");
        }

        public bool HasTarget()
        {
            if (CurTargetGuid != 0)
            {
                return true;
            }
            return false;
        }

        public void AddLastTargetToLootList()
        {
            if (LastTargetedMob != null)
            {
                LootableList.Add(LastTargetedMob);
            }
        }

        public Vector3D GetTargetLocation()
        {
            if (HasTarget())
            {
                WowUnit target = unit.GetCurTarget();
                return target.Location;
            }

            return new Vector3D(0, 0, 0);
        }

        public bool SelectWhoIsAttackingUs()
        {
            WowUnit u = GetCurAttacker();
            if (u != null)
            {
                // if we already have a target we'd better check if it's the one we want so
                // we avoid tabbing around uselessly
                if (HasTarget())
                {
                    if (CurTargetGuid == u.Guid)
                    {
                        // we are already on the target, that's fine
                        LastTargetedMob = u;
                        return true;
                    }
                }
                return SelectMob(u);
            }
            return false;
        }

        public bool SelectMob(WowUnit u)
        {
            // We face the target otherwise the tabbing won't work as it might be out of our scope
            Face(u.Location);

            // We tab till we have found our target or till we come back to the first GUID we met
            return FindMob(u);
        }

        public bool FindMob(WowUnit u)
        {
            PlayerCM.SendKeys(CommandManager.SK_TAB);
            Thread.Sleep(1000);
            ulong firstGuid = CurTargetGuid;

            if (firstGuid == 0) return false;

            do
            {
                PlayerCM.SendKeys(CommandManager.SK_TAB);
                Thread.Sleep(1000);
                if (CurTargetGuid == u.Guid)
                {
                    LastTargetedMob = u;
                    return true;
                }
            } while ((CurTargetGuid != u.Guid) && (CurTargetGuid != firstGuid));


            return false;
        }

        public void FaceTarget()
        {
            if (HasTarget())
            {
                WowUnit target = unit.GetCurTarget();
                Face(target.Location);
            }
        }

        public float DistanceFromTarget()
        {
            if (HasTarget())
            {
                WowUnit target = unit.GetCurTarget();
                return GetDistance(target.Location);
            }
            return 0.0f;
        }

        public float Facing()
        {
            return Orientation;
        }

        public float TargetFacing()
        {
            if (HasTarget())
            {
                WowUnit target = unit.GetCurTarget();
                return target.Orientation;
            }
            return 0.0f;
        }

        public float AngleToTarget()
        {
            if (HasTarget())
            {
                WowUnit target = unit.GetCurTarget();
                return GetFaceRadian(target.Location);
            }
            return 0.0f;
        }

        /// <summary>
        /// Selects the first mob with that name that we can target by tabbing
        /// </summary>
        /// <returns></returns>
        public bool SelectMobByName(string name)
        {
            throw new NotImplementedException("SelectMobByName");
        }


        public bool EnemyInSight()
        {
            List<WowUnit> l = unit.GetNearMobs();

            foreach (var enemy in ProcessManager.Profile.Enemies)
            {
                foreach (WowUnit obj in l)
                {
                    if (obj.Name == enemy.Name)
                    {
                        // We have something. But if it's too far away we ignore it
                        if (GetDistance(obj.Location, false) < 20.0f)
                        {
                            // We have him in sight
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public void FaceClosestEnemy()
        {
            List<WowUnit> l = unit.GetNearMobs();
            WowUnit closest = null;

            foreach (var enemy in ProcessManager.Profile.Enemies)
            {
                foreach (WowUnit obj in l)
                {
                    if (obj.Name == enemy.Name)
                    {
                        if (closest == null)
                        {
                            closest = obj;
                        } else
                        {
                            if (GetDistance(closest.Location, false) > GetDistance(obj.Location, false))
                            {
                                closest = obj;
                            }
                        }
                    }
                }
            }

            if (closest != null)
            {
                // We have him somewhere around us
                // Let's turn to face him so that we can tab-search
                if (SelectMob(closest))
                {
                    // We managed to tab-target it
                    FaceTarget();
                }
            }
        }

        public WowUnit FindClosestLootableMob()
        {
            WowUnit closest = null;

            foreach (WowUnit lootable in LootableList)
            {
                if (closest == null)
                {
                    closest = lootable;
                }
                else
                {
                    if (GetDistance(closest.Location, false) > GetDistance(lootable.Location, false))
                    {
                        closest = lootable;
                    }
                }
            }

            if (closest != null)
            {
                Console.WriteLine("We have a lootable mob");
                return closest;
            }
            Console.WriteLine("No lootable mob found");
            return null;
        }

        public void FaceClosestLootableMob()
        {
            WowUnit closest = FindClosestLootableMob();

            if (closest != null)
            {
                Console.WriteLine("Facing closest lootable mob");
                Face(closest.Location);
            }
        }

        public void MoveToClosestLootableMob()
        {
            WowUnit mob = FindClosestLootableMob();
            if (mob != null)
            {
                Face(mob.Location);
                MoveTo(mob.Location);
            }
        }

        public void LootClosestLootableMob()
        {
            Console.WriteLine("===Lootable List===");
            foreach (var wowUnit in LootableList)
            {
                Console.WriteLine(wowUnit.Guid + " - " + wowUnit.Name);
            }
            WowUnit mob = FindClosestLootableMob();
            if (mob != null)
            {
                Face(mob.Location);
                MoveTo(mob.Location, 3.0f);
                Loot(mob);
            }
        }

        public void Loot(WowUnit mob)
        {
            PlayerCM.MoveMouseToCenter();
            ulong guid = ProcessManager.ObjectManager.GetMouseOverGUID();

            if (guid == mob.Guid)
            {
                PlayerCM.RightClickOnCenter();
                return;
            }

            /// We build a grid and go through it until we find the
            /// corpse we want to loot or until we run out of points to
            /// scan
            //List<Point> grid = new List<Point>();
            int startX = PlayerCM.WowWindowRect.Width/2 - 10*5;
            int startY = PlayerCM.WowWindowRect.Height / 2 - 10 * 5;
            for (int i = 0; i < 20; i++)
            {
                for (int j = 0 ; j < 20 ; j++)
                {
                    PlayerCM.MoveMouseRelative(startX + j*5, startY+i*5);
                    Thread.Sleep(50);
                    guid = ProcessManager.ObjectManager.GetMouseOverGUID();

                    if (guid == mob.Guid)
                    {
                        PlayerCM.SingleClick("right");
                        Thread.Sleep(250);
                        LootableList.Remove(mob);
                        return;
                    }
                }
            }
        }

        public void MoveToTarget(float tolerance)
        {
            if (HasTarget())
            {
                WowUnit target = unit.GetCurTarget();
                MoveTo(target, tolerance);
            }
        }

        private static int RandomNumber(int min, int max)
        {
            var random = new Random();
            return random.Next(min, max);
        }

        public PlayerState MoveTo(Vector3D dest)
        {
            return MoveTo(dest, 1.0f);
        }


        public PlayerState MoveTo(Vector3D dest, float tolerance)
        {
            const CommandManager.ArrowKey key = CommandManager.ArrowKey.Up;

            if (!dest.IsValid())
            {
                return PlayerState.Ready;
            }

            float angle = GetFaceRadian(dest);
            Face(angle);

            // Random jump
            int rndJmp = RandomNumber(1, 8);
            bool doJmp = false;
            if (rndJmp == 1 || rndJmp == 3)
            {
                doJmp = true;
            }

            // Move on...
            float distance = HGetDistance(dest, false);
            PlayerCM.ArrowKeyDown(key);

            // Start profiler for WayPointTimeOut
            DateTime start = DateTime.Now;

            PlayerState res = PlayerState.Roaming;

            while (distance > tolerance)
            {
                var currentDistance = distance;

                if (doJmp)
                {
                    doJmp = false;
                    PlayerCM.SendKeys(" ");
                }

                if (StopMovement)
                {
                    res = PlayerState.Ready;
                    StopMovement = false;
                    break;
                }

                distance = HGetDistance(dest, false);

                Thread.Sleep(50);
                Application.DoEvents();

                DateTime end = DateTime.Now;
                TimeSpan tsTravelTime = end - start;

                TravelTime = tsTravelTime.Milliseconds + tsTravelTime.Seconds*1000;

                if (currentDistance == distance)
                {
                    // sono bloccato? non mi sono mosso di una distanza significativa?
                    // anche qui da verificare, mi sa che bisogna lavorare in float con
                    // un po di tolleranza
                }
                else if (TravelTime >= MAX_REACHTIME)
                {
                    TravelTime = 0;
                    res = PlayerState.WayPointTimeout;
                    break;
                }
            }

            PlayerCM.ArrowKeyUp(key);
            return res;
        }

        /// <summary>
        /// Chase a mob given its location and a distance tolerance
        /// </summary>
        /// <param name="target">Unit we want to reach</param>
        /// <param name="tolerance">Distance at which we can stop</param>
        /// <returns></returns>
        public PlayerState MoveTo(WowUnit target, float tolerance)
        {
            const CommandManager.ArrowKey key = CommandManager.ArrowKey.Up;
            float angle = 0;
            float distance = 0;
            int steps = 0;
            int currentStep = 0;
            float distanceFromStep = 0;
            Path path = new Path();

            if (!target.Location.IsValid())
            {
                return PlayerState.Ready;
            }

            distance = HGetDistance(target.Location, false);

            path =
                ProcessManager.Caronte.CalculatePath(
                    new Location(this.Location.X, this.Location.Y, this.Location.Z),
                    new Location(target.Location.X, target.Location.Y, target.Location.Z));

            foreach (Pather.Graph.Location loc in path.locations)
            {
                Console.WriteLine("X: {0}  Y: {1}   Z: {2}", loc.X, loc.Y, loc.Z);
            }
            

            steps = path.locations.Count;
            if (steps > 0)
            {
                currentStep = 1;
                angle = GetFaceRadian(new Vector3D(path.locations[currentStep].X, path.locations[currentStep].Y, path.locations[currentStep].Z));
            } else
            {
                angle = GetFaceRadian(target.Location);
            }
            
            Face(angle);

            // Random jump
            int rndJmp = RandomNumber(1, 8);
            bool doJmp = false;
            if (rndJmp == 1 || rndJmp == 3)
            {
                doJmp = true;
            }

            // Move on...
            
            PlayerCM.ArrowKeyDown(key);

            // Start profiler for WayPointTimeOut
            DateTime start = DateTime.Now;

            PlayerState res = PlayerState.Roaming;

            while (distance > tolerance)
            {
                var currentDistance = distance;
                


                if (doJmp)
                {
                    doJmp = false;
                    PlayerCM.SendKeys(" ");
                }

                if (StopMovement)
                {
                    res = PlayerState.Ready;
                    StopMovement = false;
                    break;
                }



                distance = HGetDistance(target.Location, false);

                if (steps > 0)
                {
                    distanceFromStep = HGetDistance(new Vector3D(path.locations[currentStep].X, path.locations[currentStep].Y, path.locations[currentStep].Z), false);
                    Console.WriteLine("Step: " + currentStep);
                    Console.WriteLine("Location: " + Location);
                    Console.WriteLine("Distance from step: " + distanceFromStep);
                    Console.WriteLine("Distance from target: " + distance);
                    if (distanceFromStep < 10.0f)
                    {
                        if (currentStep < steps - 1)
                        {
                            currentStep++;

                            if ((steps > 0) && (currentStep < steps - 1))
                            {
                                angle = GetFaceRadian(new Vector3D(path.locations[currentStep].X, path.locations[currentStep].Y, path.locations[currentStep].Z));
                            }
                            else
                            {
                                angle = GetFaceRadian(target.Location);
                            }
                            Face(angle);
                        }
                    }
                }

                Thread.Sleep(50);
                Application.DoEvents();

                DateTime end = DateTime.Now;
                TimeSpan tsTravelTime = end - start;

                TravelTime = tsTravelTime.Milliseconds + tsTravelTime.Seconds * 1000;

                if (currentDistance == distance)
                {
                    // sono bloccato? non mi sono mosso di una distanza significativa?
                    // anche qui da verificare, mi sa che bisogna lavorare in float con
                    // un po di tolleranza
                }
                else if (TravelTime >= MAX_REACHTIME)
                {
                    TravelTime = 0;
                    res = PlayerState.WayPointTimeout;
                    break;
                }
            }

            PlayerCM.ArrowKeyUp(key);
            return res;
        }

        public void Stop()
        {
            StopMovement = true;
        }

        public void MoveForward()
        {
            Vector3D dest = Location + (Location.Normalize()*3.0f);
            MoveTo(dest);
        }

        public bool GoBack(Vector3D dest)
        {
            throw new NotImplementedException("GoBack");
        }

        public void WalkToNextWayPoint(WayPointType wpType)
        {
            WayPoint wp = WayPointManager.Instance.GetNextWayPoint(wpType);
            if (wp != null)
            {
                MoveTo(wp.Location);
            }
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
            Thread.Sleep(500);
        }

        public void Face(Vector3D dest)
        {
            float angle = GetFaceRadian(dest);
            Face(angle);
        }


        public void PlayAction(PlayerAction act, bool? toggle)
        {
            if (toggle != null)
            {
                if (act.Toggle)
                {
                    if ((bool)toggle)
                    {
                        if (!act.Active)
                        {
                            // attiva
                            if (act.Binding.Bar > 0) //support for keys which are not bound to any action bar
                            {
                                PlayerCM.SendKeys(CommandManager.SK_SHIFT_DOWN + act.Binding.Bar + CommandManager.SK_SHIFT_UP);
                            }
                            PlayerCM.SendKeys(act.Binding.Key);
                            act.Active = true;
                        }
                    }
                    else
                    {
                        if (act.Active)
                        {
                            // disattiva
                            if (act.Binding.Bar > 0)
                            {
                                PlayerCM.SendKeys(CommandManager.SK_SHIFT_DOWN + act.Binding.Bar + CommandManager.SK_SHIFT_UP);
                            }
                            PlayerCM.SendKeys(act.Binding.Key);
                            act.Active = true;
                        }
                    }
                }
            } 
            else
            {
                if (act.Binding.Bar > 0)
                {
                    PlayerCM.SendKeys(CommandManager.SK_SHIFT_DOWN + act.Binding.Bar + CommandManager.SK_SHIFT_UP);
                }
                PlayerCM.SendKeys(act.Binding.Key);
            }
        }

        public void PlayAction(PlayerAction act)
        {
            PlayAction(act, null);
        }

        // Actions
        public bool Cast(string SlotBar, string Key)
        {
            throw new NotImplementedException("Cast");
        }

        // Messages
        public bool Say(string To, string Message)
        {
            throw new NotImplementedException("Say");
        }

        #region Mathematics functions

        public float FacingDegrees()
        {
            return (float) (Facing()*(180.0f/Math.PI));
        }

        public float TargetFacingDegrees()
        {
            return (float) (TargetFacing()*(180.0f/Math.PI));
        }

        public float AngleToTargetDegrees()
        {
            return (float) (AngleToTarget()*(180.0f/Math.PI));
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

            float res;
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

        /// <summary>
        /// Generic distance calculation. It does not update the LastDistance property
        /// </summary>
        /// <param name="dest"></param>
        /// <param name="UseZ"></param>
        /// <returns></returns>
        private float GetDistance(Vector3D dest, bool UseZ)
        {
            Vector3D currentPos = Location;

            float dX = currentPos.X - dest.X;
            float dY = currentPos.Y - dest.Y;

            float res;
            if (UseZ)
            {
                float dZ = (dest.Z != 0 ? currentPos.Z - dest.Z : 0);
                res = (float) Math.Sqrt(dX*dX + dY*dY + dZ*dZ);
            }
            else
            {
                res = (float) Math.Sqrt(dX*dX + dY*dY);
            }

            return res;
        }

        private float GetDistance(Vector3D dest)
        {
            return GetDistance(dest, false);
        }

        #endregion
    }
}
