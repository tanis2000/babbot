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
using System.Threading;
using System.Windows.Forms;
using System.Linq;
using BabBot.Bot;
using BabBot.Common;
using BabBot.Manager;
using Pather.Graph;
using BabBot.States;

namespace BabBot.Wow
{
    public class WowPlayer : WowUnit
    {
        public StateMachine<WowPlayer> StateMachine { get; protected set; }

        private const int MAX_REACHTIME = 5000; // Milliseconds to reach the waypoint
        private readonly List<WowUnit> LootableList;
        public CommandManager PlayerCM { get; private set; }
        public float LastDistance;
        public float LastFaceRadian;
        public Vector3D LastLocation;
        private WowUnit LastTargetedMob;
        private bool StopMovement;
        public int TravelTime;
        public readonly List<WowUnit> MobBlackList;
        // The continent id currently player in
        private int _continent = -1;
        // Player class
        string _class;
        // Player race
        string _race;
        // Player sex
        int _sex;
        // Zone text where currently player in
        string _zone;

        /// <summary>
        /// Constructor
        /// </summary>
        public WowPlayer(uint ObjectPointer) : base(ObjectPointer)
        {
            LastLocation = new Vector3D();

            //State = PlayerState.Start;
            PlayerCM = ProcessManager.CommandManager;
            LastDistance = 0.0f;
            LastFaceRadian = 0.0f;
            StopMovement = false;
            TravelTime = 0;
            LootableList = new List<WowUnit>();
            //isMoving = false;
            MobBlackList = new List<WowUnit>();

            //create state machine for player
            StateMachine = new StateMachine<WowPlayer>(this);
        }

        public override string Name
        {
            get
            {
                return ProcessManager.ObjectManager.GetName(ObjectPointer, Guid);
            }
        }

        public int Xp
        {
            get
            {
                return ReadDescriptor<int>(Descriptor.ePlayerFields.PLAYER_XP);
            }
        }

        public int XpToNextLevel
        {
            get
            {
                return ReadDescriptor<int>(Descriptor.ePlayerFields.PLAYER_NEXT_LEVEL_XP);
            }
        }

        public int ContinentID
        {
            get { return _continent; }
        }

        public string CharClass
        {
            get { return _class; }
        }

        public string ZoneText
        {
            get { return _zone; }
        }


        #region Target stats
        public uint TargetHp
        {
            get { return (CurTarget == null) ? 0 : CurTarget.Hp; }
        }

        public uint TargetMaxHp
        {
            get { return (CurTarget == null) ? 0 : CurTarget.MaxHp; }
        }

        public uint TargetMaxMp
        {
            get { return (CurTarget == null) ? 0 : CurTarget.MaxHp; }
        }

        public uint TargetMp
        {
            get { return (CurTarget == null) ? 0 : CurTarget.Mp; }
        }

        public int TargetLevel
        {
            get { return (CurTarget == null) ? 0 : CurTarget.Level; }
        }

        public string CurTargetName
        {
            get { return (CurTarget == null) ? string.Empty : CurTarget.Name; }
        }
        #endregion

        public List<WowObject> GetNearObjects()
        {
            List<WowObject> l = ProcessManager.ObjectManager.GetAllObjectsAroundLocalPlayer();
            return l;
        }

        public List<WowUnit> GetNearMobs()
        {
            var Mobs = new List<WowUnit>();
            List<WowObject> l = ProcessManager.ObjectManager.GetAllObjectsAroundLocalPlayer();

            var d = from c in l where c.Type == Descriptor.eObjType.OT_UNIT select c;

            foreach (WowUnit wo in d)
            {
                Mobs.Add(wo);
            }
                        
            return Mobs;
        }

        public List<WowCorpse> GetNearCorpses()
        {
            var Corpses = new List<WowCorpse>();
            List<WowObject> l = ProcessManager.ObjectManager.GetAllObjectsAroundLocalPlayer();

            var d = from c in l where c.Type == Descriptor.eObjType.OT_CORPSE select c;

            foreach (WowCorpse wo in d)
            {
                Corpses.Add(wo);
            }

            return Corpses;
        }

        public List<WowContainer> GetBags()
        {
            var Bags = new List<WowContainer>();

            for (int i = 0; i < 5; i++)
            {
                uint BagsPointer = ProcessManager.WowProcess.ReadUInt(ObjectPointer + (uint)
                    ((uint) Descriptor.ePlayerFields.PLAYER_FIELD_PACK_SLOT_1 + i * 2) * 0x04);
                Descriptor.eObjType type = ProcessManager.ObjectManager.GetTypeByObject(BagsPointer);
                if (type == Descriptor.eObjType.OT_CONTAINER)
                {
                    var c = new WowContainer(BagsPointer);
                    Bags.Add(c);
                }
            }
            /*
            List<WowObject> l = ProcessManager.ObjectManager.GetAllObjectsAroundLocalPlayer();
            foreach (WowObject o in l)
            {
                Descriptor.eObjType type = ProcessManager.ObjectManager.GetTypeByObject(o.ObjectPointer);
                if (type == Descriptor.eObjType.OT_UNIT)
                {
                    WowUnit u = new WowUnit(o);
                    u.Name = ProcessManager.ObjectManager.GetName(u.ObjectPointer, u.Guid);
                    Mobs.Add(u);
                }
            }*/
            return Bags;
        }

        public Vector3D CorpseLocation
        {
            get
            {
                return new Vector3D(
                    ProcessManager.WowProcess.ReadFloat(ProcessManager.
                                        GlobalOffsets.LocalPlayerCorpseOffset),
                    ProcessManager.WowProcess.ReadFloat(ProcessManager.
                                        GlobalOffsets.LocalPlayerCorpseOffset + 0x04),
                    ProcessManager.WowProcess.ReadFloat(ProcessManager.
                                        GlobalOffsets.LocalPlayerCorpseOffset + 0x08));
            }
        }

        public string NearObjectsAsTextList
        {
            get
            {
                string s = string.Empty;

                List<WowObject> l = GetNearObjects();
                foreach (WowObject obj in l)
                {
                    s += string.Format("GUID:{0:X}|Type:{1:X}\r" + 
                            Environment.NewLine, obj.Guid, obj.Type);
                }
                return s;
            }
        }

        public string NearMobsAsTextList
        {
            get
            {
                string s = string.Empty;

                List<WowUnit> l = GetNearMobs();
                foreach (WowUnit obj in l)
                {
                    s += string.Format("{0:X}|{1}|Lootable:{2}" + 
                        Environment.NewLine, obj.Guid, obj.Name, obj.IsLootable);
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
                    List<WowContainer> l = GetBags();
                    foreach (WowContainer obj in l)
                    {
                        s += string.Format("{0:X}|Slots:{1}|Empty:{2}" + 
                            Environment.NewLine, obj.Guid, obj.GetSlots(),
                                           obj.GetEmptySlots());
                    }
                }
                catch (Exception ex)
                {
                    throw (ex);
                }
                return s;
            }
        }

        //public PlayerState State
        //{
        //    get { return ProcessManager.BotManager.State; }
        //}
        
        public bool IsAtGraveyard()
        {
            List<WowUnit> l = GetNearMobs();

            var d = from c in l where c.Name == "Spirit Healer" select c;

            if (d.Count<WowUnit>() > 0) return true;
            else return false;
        }

        public bool IsBeingAttacked()
        {
            /// We check the list of mobs around us and if any of them has us as target and has aggro
            /// it means that we are being attacked by that mob. It can be more than one mob
            /// but we don't care. As long as one is attacking us, this will return true.
            if (GetCurAttacker() == null) return false;
            return true;
        }

        public WowUnit GetCurAttacker()
        {
            List<WowUnit> l = GetNearMobs();
            var d = from c in l where c.HasTarget && c.CurTargetGuid == Guid && c.IsAggro select c;

            if (d.Count<WowUnit>() > 0) return d.First<WowUnit>();
            else return null;
        }

        public WowUnit GetCurTarget()
        {
            if (HasTarget)
            {
                WowUnit u = CurTarget;
                return u;
            }
            return null;
        }

        public bool IsTargetDead()
        {
            if (HasTarget)
            {
                WowUnit target = GetCurTarget();
                if (target.IsDead)
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsInCombat()
        {
            ProcessManager.Injector.Lua_DoString(@"(function()
	local incombat = InCombatLockdown()
	return incombat
end)()");
            string incombat = ProcessManager.Injector.Lua_GetLocalizedText(0);
            if (incombat == "1") return true; else return false;
        }

        public void AddLastTargetToLootList()
        {
            //Thread.Sleep(2000);
            Output.Instance.Debug("AddLastTargetToLootList() - " + 
                "Adding our last target to the list of lootable mobs", this);
            ProcessManager.Injector.Lua_DoString("TargetLastTarget()");
            Thread.Sleep(2000);
            if (HasTarget)
            {
                if (CurTarget.IsLootable)
                {
                    LootableList.Add(CurTarget);
                    Output.Instance.Debug("AddLastTargetToLootList() - " + 
                                "Mob added to the list of llotable mobs", this);
                }
                else
                    Output.Instance.Debug("AddLastTargetToLootList() - " +
                        "It looks like our last kill is not lootable for some reason", this);
            }
            else
            {
                Output.Instance.Debug("AddLastTargetToLootList() - " + 
                    "Something bad happened. We couldn't target our last target", this);
            }
        }

        public Vector3D GetTargetLocation()
        {
            if (HasTarget)
            {
                WowUnit target = GetCurTarget();
                return target.Location;
            }

            return new Vector3D(0, 0, 0);
        }

        public bool SelectWhoIsAttackingUsWithCTM()
        {
            WowUnit u = GetCurAttacker();
            if (u != null)
            {
                // if we already have a target we'd better check if it's the one we want so
                // we avoid tabbing around uselessly
                if (HasTarget)
                {
                    if (CurTargetGuid == u.Guid)
                    {
                        // we are already on the target, that's fine
                        LastTargetedMob = u;
                        return true;
                    }
                }
                ClickToMoveAttack(u.Guid);
            }
            return false;
        }

        public bool SelectWhoIsAttackingUs()
        {
            WowUnit u = GetCurAttacker();
            if (u != null)
            {
                // if we already have a target we'd better check if it's the one we want so
                // we avoid tabbing around uselessly
                if (HasTarget)
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

        public bool SelectNpc(WowUnit u)
        {
            return SelectUnit(u, CommandManager.SK_CTRL_TAB);
        }

        public bool SelectMob(WowUnit u)
        {
            return SelectUnit(u, CommandManager.SK_TAB);
        }

        public bool SelectUnit(WowUnit u, string key)
        {
            // We face the target otherwise the tabbing won't work as it might be out of our scope
            // NOTE: we are using the timed one, but it's a bit shaky
            //FaceUsingMemoryWrite(GetFaceAngle(u.Location), true);
            Face(u.Location);

            // We tab till we have found our target or till we come back to the first GUID we met
            return FindUnit(u, key);
        }

        public bool FindUnit(WowUnit u, string key)
        {
            DateTime start = DateTime.Now;
            TimeSpan tsTimeSpent;
            PlayerCM.SendKeys(key);
            Thread.Sleep(250);
            ulong firstGuid = CurTargetGuid;

            if (firstGuid == 0)
            {
                Output.Instance.Debug("We tried targeting but " + 
                    "the first TAB didn't land on anything", this);
                return false;
            }

            do
            {
                DateTime end = DateTime.Now;
                tsTimeSpent = end - start;

                PlayerCM.SendKeys(CommandManager.SK_TAB);
                Thread.Sleep(250);
                if (CurTargetGuid == u.Guid)
                {
                    LastTargetedMob = u;
                    Output.Instance.Debug("We found a target while TABbing", this);
                    return true;
                }
            } while ((CurTargetGuid != u.Guid) && 
                (CurTargetGuid != firstGuid) && 
                (tsTimeSpent.TotalMilliseconds < 10000));

            if (tsTimeSpent.TotalMilliseconds >= 10000)
            {
                // If we couldn't find the target we were looking for we clear the target
                ProcessManager.Injector.Lua_DoString(@"ClearTarget()");
            }
            Output.Instance.Debug("We cycled through all the targets " + 
                                            "but nothing good could be found", this);
            return false;
        }

        public void FaceTarget()
        {
            if (HasTarget)
            {
                WowUnit target = GetCurTarget();
                Face(target.Location);
            }
        }

        public float DistanceFromTarget()
        {
            if (HasTarget)
            {
                WowUnit target = GetCurTarget();
                return MathFuncs.GetDistance(target.Location, Location, false);
            }
            return 0.0f;
        }

        public float DistanceFromCorpse()
        {
            if (IsDead || IsGhost)
            {
                return MathFuncs.GetDistance(CorpseLocation, Location, false);
            }
            return 0.0f;
        }

        public float TargetFacing()
        {
            if (HasTarget)
            {
                WowUnit target = GetCurTarget();
                return target.Orientation;
            }
            return 0.0f;
        }

        public float AngleToTarget()
        {
            if (HasTarget)
            {
                WowUnit target = GetCurTarget();
                return MathFuncs.GetFaceRadian(target.Location, Location);
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
            List<WowUnit> l = GetNearMobs();

            foreach (Enemy enemy in ProcessManager.Profile.Enemies)
            {
                foreach (WowUnit obj in l)
                {
                    if (obj.Name == enemy.Name)
                    {
                        // We have something. But if it's too far away we ignore it
                        if (MathFuncs.GetDistance(obj.Location, Location, false) < 20.0f)
                        {
                            // We have him in sight
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public WowUnit GetClosestEnemyInSight()
        {
            List<WowUnit> l = GetNearMobs();
            WowUnit closest = null;

            foreach (Enemy enemy in ProcessManager.Profile.Enemies)
            {
                foreach (WowUnit obj in l)
                {
                    if ((obj.Name == enemy.Name) && (!obj.IsDead) && !IsInMobBlackList(obj))
                    {
                        if (closest == null)
                            closest = obj;
                        else
                            if (MathFuncs.GetDistance(closest.Location, Location, false) > 
                                        MathFuncs.GetDistance(obj.Location, Location, false))
                                closest = obj;
                    }
                }
            }

            return closest;
        }

        public bool IsTargetInEnemyList()
        {
            if (!HasTarget) return false;
            if (IsTargetDead()) return false;

            foreach (Enemy enemy in ProcessManager.Profile.Enemies)
            {
                if (CurTarget.Name == enemy.Name)
                    return true;
            }
            return false;
        }

        public bool IsInMobBlackList(WowUnit mob)
        {
            foreach (var ent in MobBlackList)
            {
                if (ent.Guid == mob.Guid)
                    return true;
            }
            return false;
        }

        public void AttackClosestEnemyWithCTM()
        {
            List<WowUnit> l = GetNearMobs();
            WowUnit closest = null;

            foreach (Enemy enemy in ProcessManager.Profile.Enemies)
            {
                foreach (WowUnit obj in l)
                {
                    if ((obj.Name == enemy.Name) && (!obj.IsDead) && !IsInMobBlackList(obj))
                    {
                        if (closest == null)
                            closest = obj;
                        else
                            if (MathFuncs.GetDistance(closest.Location, Location, false) > 
                                        MathFuncs.GetDistance(obj.Location, Location, false))
                                closest = obj;
                    }
                }
            }

            if (closest != null)
            {
                // We have him somewhere around us
                // Let's turn to face him so that we can tab-search
                Face(closest.Location);

                // Attack it with CTM
                ClickToMoveAttack(closest.Guid);
            }
        }

        public void AttackMobWithCTM(WowUnit mob)
        {
            if (mob == null) return;

            Face(mob.Location);
            ClickToMoveAttack(mob.Guid);
        }

        public void MoveToClosestEnemy()
        {
            List<WowUnit> l = GetNearMobs();
            WowUnit closest = null;

            foreach (Enemy enemy in ProcessManager.Profile.Enemies)
            {
                foreach (WowUnit obj in l)
                {
                    if ((obj.Name == enemy.Name) && (!obj.IsDead) && !IsInMobBlackList(obj))
                    {
                        if (closest == null)
                            closest = obj;
                        else
                            if (MathFuncs.GetDistance(closest.Location, Location, false) > 
                                        MathFuncs.GetDistance(obj.Location, Location, false))
                                closest = obj;
                    }
                }
            }

            if (closest != null)
            {
                // We have him somewhere around us
                // Let's turn to face him so that we can tab-search
                Face(closest.Location);

                // Move to it with CTM
                ClickToMove(closest.Location);
            }
        }

        public void FaceClosestEnemy()
        {
            List<WowUnit> l = GetNearMobs();
            WowUnit closest = null;

            foreach (Enemy enemy in ProcessManager.Profile.Enemies)
            {
                foreach (WowUnit obj in l)
                {
                    if ((obj.Name == enemy.Name) && (!obj.IsDead))
                    {
                        if (closest == null)
                            closest = obj;
                        else
                            if (MathFuncs.GetDistance(closest.Location, Location, false) > 
                                        MathFuncs.GetDistance(obj.Location, Location, false))
                                closest = obj;
                    }
                }
            }

            if (closest != null)
            {
                // We have him somewhere around us
                // Let's turn to face him so that we can tab-search
                Face(closest.Location);

                // Tab-target it
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
                    closest = lootable;
                else
                    if (MathFuncs.GetDistance(closest.Location, Location, false) > 
                            MathFuncs.GetDistance(lootable.Location, Location, false))
                        closest = lootable;
            }

            if (closest != null)
            {
                Output.Instance.Debug("We have a lootable mob", this);
                return closest;
            }
            Output.Instance.Debug("No lootable mob found", this);
            return null;
        }

        public void FaceClosestLootableMob()
        {
            WowUnit closest = FindClosestLootableMob();

            if (closest != null)
            {
                Output.Instance.Debug("Facing closest lootable mob", this);
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
            Output.Instance.Debug("===Lootable List===", this);
            foreach (WowUnit wowUnit in LootableList)
                if (wowUnit != null)
                    Output.Instance.Debug(wowUnit.Guid + " - " + wowUnit.Name, this);

            Output.Instance.Debug("LootClosestLootableMob() - Finding closest lootable mob", this);
            WowUnit mob = FindClosestLootableMob();
            if (mob != null)
            {
                Output.Instance.Debug("LootClosestLootableMob() - Lootable mob found", this);
                Face(mob.Location);
                if (MathFuncs.GetDistance(mob.Location, Location, false) > 5.0f)
                {
                    Output.Instance.Debug("LootClosestLootableMob() - We are too far. Getting closer.", this);
                    MoveTo(mob.Location, 5.0f);
                }
                Output.Instance.Debug("LootClosestLootableMob() - Looting the mob", this);
                Loot(mob);
            }
        }
                
        /// <summary>
        /// Loots a mob and remove it from the lootable list. We must already be near the corpse
        /// </summary>
        /// <param name="mob">Corpse that we want to loot</param>
        public void Loot(WowUnit mob)
        {
            Output.Instance.Debug("Loot() - Interacting with the corpse mob", this);
            //mob.Interact();
            ProcessManager.Injector.Lua_DoString(@"InteractUnit(""target"")");
            Thread.Sleep(2000);
            Output.Instance.Debug("Loot() - Removing this corpse from the list", this);
            LootableList.Remove(mob);
        }
        
        public void MoveToTarget(float tolerance)
        {
            if (HasTarget)
            {
                WowUnit target = GetCurTarget();
                MoveTo(target, tolerance);
            }
        }

        public void ClickToMove(Vector3D dest)
        {
            ProcessManager.WowProcess.SuspendThread();
            ProcessManager.WowProcess.WriteFloat(ProcessManager.
                                        GlobalOffsets.ClickToMoveUnknown, 9.0f);
            ProcessManager.WowProcess.WriteFloat(ProcessManager.
                                        GlobalOffsets.ClickToMoveTurnScale, 13.962634f);
            ProcessManager.WowProcess.WriteFloat(ProcessManager.
                                        GlobalOffsets.ClickToMoveUnknown2, 14.0f);
            ProcessManager.WowProcess.WriteFloat(ProcessManager.
                                        GlobalOffsets.ClickToMoveInteractDistance, 0.5f);
            ProcessManager.WowProcess.WriteUInt(ProcessManager.
                                        GlobalOffsets.ClickToMoveActionType, 
                                    (uint)Descriptor.eClickToMoveActionType.WalkTo);
            ProcessManager.WowProcess.WriteFloat(ProcessManager.
                                        GlobalOffsets.ClickToMoveDestX, dest.X);
            ProcessManager.WowProcess.WriteFloat(ProcessManager.
                                        GlobalOffsets.ClickToMoveDestY, dest.Y);
            ProcessManager.WowProcess.WriteFloat(ProcessManager.
                                        GlobalOffsets.ClickToMoveDestZ, dest.Z);
            ProcessManager.WowProcess.ResumeThread();
        }

        public void ClickToMoveAttack(ulong guid)
        {
            ClickToMoveUnit(guid, Descriptor.eClickToMoveActionType.AttackGuid);
        }

        public void ClickToMoveInteractNpc(ulong guid)
        {
            ClickToMoveUnit(guid, Descriptor.eClickToMoveActionType.InteractNpc);
        }

        private void ClickToMoveUnit(ulong guid, 
                Descriptor.eClickToMoveActionType action_type)
        {
            ProcessManager.WowProcess.SuspendThread();
            ProcessManager.WowProcess.WriteFloat(ProcessManager.
                                        GlobalOffsets.ClickToMoveUnknown, 9.0f);
            ProcessManager.WowProcess.WriteFloat(ProcessManager.
                                        GlobalOffsets.ClickToMoveTurnScale, 13.962634f);
            ProcessManager.WowProcess.WriteFloat(ProcessManager.
                                        GlobalOffsets.ClickToMoveUnknown2, 14.0f);
            ProcessManager.WowProcess.WriteFloat(ProcessManager.
                                        GlobalOffsets.ClickToMoveInteractDistance, 0.366f);
            ProcessManager.WowProcess.WriteUInt64(ProcessManager.
                                        GlobalOffsets.ClickToMoveTarget, guid);
            ProcessManager.WowProcess.WriteUInt(ProcessManager.
                                    GlobalOffsets.ClickToMoveActionType, (uint)action_type);
            ProcessManager.WowProcess.ResumeThread();
        }

        /// <summary>
        /// Makes the player walk from the current location to the location of "dest" with 
        /// a default tolerance of 1 yard.
        /// </summary>
        /// <param name="dest">Destination</param>
        /// <returns>A NavigationState indicating what the result of the movement was</returns>
        public NavigationState MoveTo(Vector3D dest)
        {
            return MoveTo(dest, 1.0f);
        }


        /// <summary>
        /// Makes the player walk from the current location to the location of "dest"
        /// </summary>
        /// <param name="dest">Destination</param>
        /// <param name="tolerance">A value indicating the tolerance. i.e. if we pass
        /// 3.0f as tolerance, the bot will stop moving when reaching 3 yards from the
        /// destination.</param>
        /// <returns>A NavigationState indicating what the result of the movement was</returns>
        public NavigationState MoveTo(Vector3D dest, float tolerance)
        {
            // TODO: add PPather usage here as well
            const CommandManager.ArrowKey key = CommandManager.ArrowKey.Up;

            //isMoving = true;

            if (!dest.IsValid())
            {
                //isMoving = false;
                return NavigationState.Ready;
            }

            // Random jump
            // TODO: make this configurable
            int rndJmp = MathFuncs.RandomNumber(1, 8);
            bool doJmp = false;
            if (rndJmp == 1 || rndJmp == 3)
            {
                doJmp = true;
            }

            // Move on...
            float distance = MathFuncs.GetDistance(dest, Location, false);
            PlayerCM.ArrowKeyDown(key);

            /// We face our destination waypoint while we are already moving, so that it looks 
            /// more human-like
            float angle = MathFuncs.GetFaceRadian(dest, Location);
            Face(angle);

            // Start profiler for WayPointTimeOut
            DateTime start = DateTime.Now;

            NavigationState res = NavigationState.Roaming;

            while (distance > tolerance)
            {
                float currentDistance = distance;

                if (doJmp)
                {
                    doJmp = false;
                    PlayerCM.SendKeys(" ");
                }

                if (StopMovement)
                {
                    res = NavigationState.Ready;
                    StopMovement = false;
                    break;
                }

                distance = MathFuncs.GetDistance(dest, Location, false);

                Thread.Sleep(50);
                Application.DoEvents();

                DateTime end = DateTime.Now;
                TimeSpan tsTravelTime = end - start;

                TravelTime = tsTravelTime.Milliseconds + tsTravelTime.Seconds*1000;

                // we take as granted that we should move at least 0.1 yards 
                // per cycle (might be a good idea to get this routine synchronized so that 
                // we can actually know exactly how much we move "per-tick")
                if (Math.Abs(currentDistance - distance) < 0.1f && 
                            Math.Abs(currentDistance - distance) > 0.0001f) 
                {
                    Output.Instance.Debug(string.Format("Stuck! Distance difference: {0}", 
                                                Math.Abs(currentDistance - distance)), this);
                    Unstuck();
                }
                else if (TravelTime >= MAX_REACHTIME)
                {
                    TravelTime = 0;
                    res = NavigationState.WayPointTimeout;
                    break;
                }
                Output.Instance.Debug(string.Format("Tolerance: {0} - Distance: {1}", 
                                                            tolerance, distance), this);
            }

            PlayerCM.ArrowKeyUp(key);
            //isMoving = false;
            return res;
        }

        /// <summary>
        /// Chase a mob given its location and a distance tolerance
        /// </summary>
        /// <param name="target">Unit we want to reach</param>
        /// <param name="tolerance">Distance at which we can stop</param>
        /// <returns>A NavigationState indicating what the result of the movement was</returns>
        public NavigationState MoveTo(WowUnit target, float tolerance)
        {
            const CommandManager.ArrowKey key = CommandManager.ArrowKey.Up;
            float angle = 0;
            float distance = 0;
            int steps = 0;
            int currentStep = 0;
            float distanceFromStep = 0;

            //isMoving = true;

            if (!target.Location.IsValid())
            {
                //isMoving = false;
                return NavigationState.Ready;
            }

            distance = MathFuncs.GetDistance(target.Location, Location, false);

            Path path =
                ProcessManager.Caronte.CalculatePath(
                    new Location(Location.X, Location.Y, Location.Z),
                    new Location(target.Location.X, target.Location.Y, target.Location.Z));

            if (path == null)
            {
                Output.Instance.Debug(string.Format(
                    "Cannot find a path from X:{0} Y:{1} Z:{2} to X:{3} Y:{4} Z:{5}", 
                    Location.X, Location.Y, Location.Z, target.Location.X, target.Location.Y, 
                        target.Location.Z), this);
                return NavigationState.Ready;
            }

            foreach (Location loc in path.locations)
                Output.Instance.Debug(string.Format(
                    "X: {0}  Y: {1}   Z: {2}", loc.X, loc.Y, loc.Z), this); 


            steps = path.locations.Count;
            if (steps > 0)
            {
                currentStep = 1;
                angle = MathFuncs.GetFaceRadian(
                    new Vector3D(path.locations[currentStep].X, 
                        path.locations[currentStep].Y, 
                        path.locations[currentStep].Z), Location);
            }
            else
            {
                angle = MathFuncs.GetFaceRadian(target.Location, Location);
            }

            Face(angle);

            // Random jump
            int rndJmp = MathFuncs.RandomNumber(1, 8);
            bool doJmp = false;
            if (rndJmp == 1 || rndJmp == 3)
                doJmp = true;

            // Move on...

            PlayerCM.ArrowKeyDown(key);

            // Start profiler for WayPointTimeOut
            DateTime start = DateTime.Now;

            NavigationState res = NavigationState.Roaming;

            while (distance > tolerance)
            {
                float currentDistance = distance;


                if (doJmp)
                {
                    doJmp = false;
                    PlayerCM.SendKeys(" ");
                }

                if (StopMovement)
                {
                    res = NavigationState.Ready;
                    StopMovement = false;
                    break;
                }


                distance = MathFuncs.GetDistance(target.Location, Location, false);

                if (steps > 0)
                {
                    distanceFromStep =
                        MathFuncs.GetDistance(
                            new Vector3D(path.locations[currentStep].X, path.locations[currentStep].Y,
                                         path.locations[currentStep].Z), Location, false);
                    Output.Instance.Debug("Step: " + currentStep, this);
                    Output.Instance.Debug("Location: " + Location, this);
                    Output.Instance.Debug("Distance from step: " + distanceFromStep, this);
                    Output.Instance.Debug("Distance from target: " + distance, this);
                    if (distanceFromStep < 10.0f)
                    {
                        if (currentStep < steps - 1)
                        {
                            currentStep++;

                            if ((steps > 0) && (currentStep < steps - 1))
                            {
                                angle =
                                    MathFuncs.GetFaceRadian(new Vector3D(path.locations[currentStep].X,
                                                               path.locations[currentStep].Y,
                                                               path.locations[currentStep].Z), Location);
                            }
                            else
                            {
                                angle = MathFuncs.GetFaceRadian(target.Location, Location);
                            }
                            // NOTE: tanis - I've switched this to the memory write one. 
                            // I dunno if it's better yet
                            //Face(angle);
                            angle = MathFuncs.GetFaceRadian(
                                WaypointVector3DHelper.LocationToVector3D(
                                        path.locations[currentStep]),Location);
                            FaceUsingMemoryWrite(angle, true);
                            // NOTE: tanis - end
                        }
                    }
                }

                Thread.Sleep(50);
                Application.DoEvents();

                DateTime end = DateTime.Now;
                TimeSpan tsTravelTime = end - start;

                TravelTime = tsTravelTime.Milliseconds + tsTravelTime.Seconds*1000;

                if (currentDistance == distance)
                {
                    /// We should come up with a routine to get us unstuck.
                    /// Of course we should also add some tolerance here as it is unlikely that
                    /// we will be exactly in the same position when we get stuck
                }
                else if (TravelTime >= MAX_REACHTIME)
                {
                    TravelTime = 0;
                    res = NavigationState.WayPointTimeout;
                    break;
                }
                Output.Instance.Debug(string.Format(
                    "Tolerance: {0} - Distance: {1}", tolerance, distance), this);
            }

            PlayerCM.ArrowKeyUp(key);
            //isMoving = false;
            return res;
        }

        /// <summary>
        /// Called by MoveTo to try and unstuck us if at all possible. It assumes
        /// we are actually moving (thus up arrow key pressed)
        /// </summary>
        public void Unstuck()
        {
            // The first thing we try is jumping
            PlayerCM.SendKeys(" ");
            Thread.Sleep(250);
            PlayerCM.SendKeys(" ");
            Thread.Sleep(250);
            PlayerCM.SendKeys(" ");
            Thread.Sleep(250);

            // Then we strafe right a bit
            StrafeRight(2000);
            PlayerCM.SendKeys(" ");
            Thread.Sleep(250);

            // And then left a bit
            StrafeLeft(2000);
            PlayerCM.SendKeys(" ");
            Thread.Sleep(250);
        }

        /// <summary>
        /// When called, this will stop the bot if it's navigating through waypoints
        /// or chasing a mob
        /// </summary>
        public void Stop()
        {
            StopMovement = true;
            const CommandManager.ArrowKey key = CommandManager.ArrowKey.Up;
            PlayerCM.ArrowKeyUp(key);
        }

        /// <summary>
        /// Moves forward for iTime milliseconds
        /// </summary>
        /// <param name="iTime">how long we should move (milliseconds)</param>
        public void MoveForward(int iTime)
        {
            const CommandManager.ArrowKey key = CommandManager.ArrowKey.Up;
            PlayerCM.ArrowKeyDown(key);
            Thread.Sleep(iTime);
            PlayerCM.ArrowKeyUp(key);
        }

        public void MoveForward()
        {
            const CommandManager.ArrowKey key = CommandManager.ArrowKey.Up;
            PlayerCM.ArrowKeyDown(key);
            //isMoving = true;
        }

        public void StopMoveForward()
        {
            const CommandManager.ArrowKey key = CommandManager.ArrowKey.Up;
            PlayerCM.ArrowKeyUp(key);
            //isMoving = false;
        }

        /// <summary>
        /// Moves backward for iTime milliseconds
        /// </summary>
        /// <param name="iTime">how long we should move (milliseconds)</param>
        public void MoveBackward(int iTime)
        {
            const CommandManager.ArrowKey key = CommandManager.ArrowKey.Down;
            PlayerCM.ArrowKeyDown(key);
            Thread.Sleep(iTime);
            PlayerCM.ArrowKeyUp(key);
        }

        /// <summary>
        /// Strafes right for iTime milliseconds
        /// </summary>
        /// <param name="iTime">how long we should move (milliseconds)</param>
        public void StrafeRight(int iTime)
        {
            for (int i = 0; i < iTime/100; i++)
            {
                PlayerCM.SendKeys("e");
                Thread.Sleep(100);
            }
        }

        /// <summary>
        /// Strafes left for iTime milliseconds
        /// </summary>
        /// <param name="iTime">how long we should move (milliseconds)</param>
        public void StrafeLeft(int iTime)
        {
            for (int i = 0; i < iTime/100; i++)
            {
                PlayerCM.SendKeys("q");
                Thread.Sleep(100);
            }
        }

        /// <summary>
        /// Iterates through the waypoints and makes us walk to the next one in the list
        /// </summary>
        /// <param name="wpType">The type of waypoint we're going to (Normal, Vendor, GraveYard, etc..)</param>
        public void WalkToNextWayPoint(WayPointType wpType)
        {
            WayPoint wp = WayPointManager.Instance.GetNextWayPoint(wpType);
            if (wp != null)
            {
                MoveTo(wp.Location);
            }
        }

        public void FaceUsingMemoryWrite(float angle, bool Timed)
        {
            Output.Instance.Debug(string.Format("FaceUsingMemoryWrite() - angle:{0}", angle));
            //if angle is smaller then error margin then don't bother to fix
            if (Math.Abs(angle) < 0.04f)
                return;

            //if timed is false then do memory write immediately and exit
            if (!Timed)
            {
                ProcessManager.WowProcess.SuspendThread();
                ProcessManager.WowProcess.WriteFloat(ObjectPointer + ProcessManager.
                                                    GlobalOffsets.PlayerRotationOffset, angle);
                ProcessManager.WowProcess.ResumeThread();

                Thread.Sleep(50);

                PlayerCM.ArrowKeyDown(CommandManager.ArrowKey.Left);
                PlayerCM.ArrowKeyUp(CommandManager.ArrowKey.Left);

                Thread.Sleep(50);
                
                return;
            }

            //the max distance we need to turn is Pi.  
            // To turn that distance in 1 second we need to turn Pi/10 every 100 miliseconds
            float dPerCycle = (float)Math.PI / 10f;

            //determine which direction to turn
            if (MathFuncs.negativeAngle(angle - Orientation) > Math.PI)
            {
                dPerCycle *= -1;
            }

            CommandManager.ArrowKey nextKey = CommandManager.ArrowKey.Left;

            //while the distance left to turn is greater then that covered in a single cycle
            while (Math.Abs(angle - (Orientation % (2 * System.Math.PI))) > Math.Abs(dPerCycle))
            {
                //get new angle by adding the positive or negative dpercycle value to our 
                // current orientation and setting it as our players angle
                float newOrientation = (float)(Orientation % (2 * System.Math.PI)) + dPerCycle;

                if (newOrientation < 0f)
                {
                    newOrientation += (float)((double)2 * Math.PI);
                }

                ProcessManager.WowProcess.SuspendThread();
                ProcessManager.WowProcess.WriteFloat(ObjectPointer + ProcessManager.
                                            GlobalOffsets.PlayerRotationOffset, newOrientation);
                ProcessManager.WowProcess.ResumeThread();

                PlayerCM.ArrowKeyDown(nextKey);
                PlayerCM.ArrowKeyUp(nextKey);

                if (nextKey == CommandManager.ArrowKey.Left)
                    nextKey = CommandManager.ArrowKey.Right;
                else
                    nextKey = CommandManager.ArrowKey.Left;

                //then wait 100 miliseconds
                Thread.Sleep(50);
            }

            //write the new location memory
            ProcessManager.WowProcess.SuspendThread();
            ProcessManager.WowProcess.WriteFloat(ObjectPointer + ProcessManager.
                                            GlobalOffsets.PlayerRotationOffset, angle);
            ProcessManager.WowProcess.ResumeThread();
            Thread.Sleep(50);

            PlayerCM.ArrowKeyDown(nextKey);
            PlayerCM.ArrowKeyUp(nextKey);            
        }

        private void FaceWithTimer(float radius, CommandManager.ArrowKey key)
        {
            //if radius is smaller then error margin then don't bother to fix
            if (radius < 0.04f)
                return;

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

            if (MathFuncs.negativeAngle(angle - Orientation) < Math.PI)
            {
                face = MathFuncs.negativeAngle(angle - Orientation);
                FaceWithTimer(face, CommandManager.ArrowKey.Left);
            }
            else
            {
                face = MathFuncs.negativeAngle(Orientation - angle);
                FaceWithTimer(face, CommandManager.ArrowKey.Right);
            }
            Thread.Sleep(500);
        }

        public void Face(Vector3D dest)
        {
            // NOTE: tanis - begin test
            //float angle = GetFaceAngle(dest);
            //Face(angle);
            float angle = MathFuncs.GetFaceRadian(dest, Location);
            FaceUsingMemoryWrite(angle, false);
            // NOTE: tanis - end
        }

        protected float GetFaceAngle(Vector3D dest)
        {
            float angle = MathFuncs.GetFaceRadian(dest, Location);

            return GetFaceAngle(angle);
        }

        protected float GetFaceAngle(float angle)
        {
            float face;

            if (MathFuncs.negativeAngle(angle - Orientation) < Math.PI)
            {
                face = MathFuncs.negativeAngle(angle - Orientation);
            }
            else
            {
                face = MathFuncs.negativeAngle(Orientation - angle);
            }

            return face;
        }

        public void PlayAction(PlayerAction act, bool? toggle)
        {
            if (toggle != null)
            {
                if (act.Toggle)
                {
                    if ((bool) toggle)
                    {
                        if (!act.Active)
                        {
                            // attiva
                            if (act.Binding.Bar > 0) //support for keys which are not bound to any action bar
                            {
                                PlayerCM.SendKeys(CommandManager.SK_SHIFT_DOWN + act.Binding.Bar +
                                                  CommandManager.SK_SHIFT_UP);
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
                                PlayerCM.SendKeys(CommandManager.SK_SHIFT_DOWN + act.Binding.Bar +
                                                  CommandManager.SK_SHIFT_UP);
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
                    PlayerCM.SendKeys(CommandManager.SK_SHIFT_DOWN + 
                        act.Binding.Bar + CommandManager.SK_SHIFT_UP);
                PlayerCM.SendKeys(act.Binding.Key);
            }
        }

        public void PlayAction(PlayerAction act)
        {
            PlayAction(act, null);
        }

        /// <summary>
        /// Sets the id of the continent and text of the zone we're in
        /// </summary>
        public void setCurrentMapInfo()
        {
            string[] lret = ProcessManager.
                    Injector.Lua_ExecByName("GetCurrentMapInfo");
            try
            {
                _continent = Convert.ToInt32(lret[0]);
                _zone = lret[1];
            }
            catch
            {
                _continent = -1;
            }
        }

        public void SetCharInfo()
        {
            string[] s = ProcessManager.Injector.Lua_ExecByName("GetUnitInfo",
                new object[] {"player"});

            if ((s == null) || (s.Length < 5) || (s[1] == null) || (s[3] == null))
                throw new Exception("Unable retrieve character's class");

            _class = s[1];
            _race = s[3];
            _sex = Convert.ToInt32(s[4]);
        }

        public void AttackTarget()
        {
            ProcessManager.Injector.Lua_DoString("AttackTarget()");
        }

        public void SpellStopCasting()
        {
            ProcessManager.Injector.Lua_DoString("SpellStopCasting()");
        }

        public bool IsMoving()
        {
            return (MathFuncs.GetDistance(Location, LastLocation, false) > .1f) ? true : false;
        }

        /// <summary>
        /// Checks whether we are attacking something or not
        /// Note that this supposes that we have the Attack action in the
        /// first slot of the first action bar
        /// </summary>
        /// <returns>true if we are attacking</returns>
        public bool IsAttacking()
        {
            // TODO: find a way to read if we are attacking without checking the action bar
            ProcessManager.Injector.Lua_DoString(@"(function()
	local action = IsCurrentAction(1)
	return action
end)()");
            string action = ProcessManager.Injector.Lua_GetLocalizedText(0);
            if (action == "1") return true; else return false;
        }

        public bool CanCast(string iName)
        {
            ProcessManager.Injector.Lua_DoString(string.Format(@"(function()
	local name, rank, icon, cost, isFunnel, powerType, castTime, minRange, maxRange = GetSpellInfo(""{0}"")
	return name, rank, icon, cost, isFunnel, powerType, castTime, minRange, maxRange
end)()", iName));
            string cost = ProcessManager.Injector.Lua_GetLocalizedText(3);

            // If we cannot get the info it means we don't know this spell
            if (cost == "") return false;

            Int32 realCost = Convert.ToInt32(cost);

            // TODO: We should do all the checks based on our class. 
            // For now we take for granted we're talking about mana

            if (realCost <= Mp)
                return true;

            return false;
        }

        public void Wait(int iTime)
        {
            Thread.Sleep(iTime);
        }

        public bool HasItem(Item item)
        {
            return HasItem(item.Name);
        }

        public bool HasItem(string item) 
        {
            if (item == "") return false;

            const string luaScript =
            @"
              (function()
                  for bag = 0,4 do
                    for slot = 1,GetContainerNumSlots(bag) do
                      babbot_item = GetContainerItemLink(bag,slot);
                      if babbot_item and babbot_item:find(""{0}"") then
                        return true
                      end
                    end
                  end
                  return false
              end)()
            ";

            ProcessManager.Injector.Lua_DoString(string.Format(luaScript, item));
            string found = ProcessManager.Injector.Lua_GetLocalizedText(0);
            if (found == "")
            {
                return false;
            } else
            {
                return true;
            }


            /*
            for (int i = 0; i <5 ; i++) {
                Bag bag = new Bag(i);
                bag.UpdateBagItems();
                if (bag.Find(item) != null)
                {
                    return true;
                }
            }
             
            return false;
            */
        }

        public void UseItem(Item item)
        {
            UseItem(item.Name);
        }

        public void UseItem(string item)
        {
            if (item == "") return;

            const string luaScript = 
            @"
              (function()
              for bag = 0,4 do
                for slot = 1,GetContainerNumSlots(bag) do
                  local item = GetContainerItemLink(bag,slot)
                  if item and item:find(""{0}"") then
                    UseContainerItem(bag,slot)
                  end
                end
              end)()
            ";

            ProcessManager.Injector.Lua_DoString(string.Format(luaScript, item));
            /*
            for (int i = 0; i < 5; i++)
            {
                Bag bag = new Bag(i);
                bag.UpdateBagItems();
                ItemLink il = bag.Find(item);
                if (il != null)
                {
                    ProcessManager.Injector.Lua_DoString(string.Format(
                            "UseContainerItem({0}, {1});", bag.BagID, il.Slot));
                }
            }
             */
        }

        public bool HasBuff(string iName)
        {
            ProcessManager.Injector.Lua_DoString(string.Format(@"(function()
	local name, rank, icon, count, debuffType, duration, expirationTime, source, isStealable = UnitBuff(""player"", ""{0}"")
	return name, rank, icon, count, debuffType, duration, expirationTime, source, isStealable
end)()", iName));
            string name = ProcessManager.Injector.Lua_GetLocalizedText(0);
            if (string.IsNullOrEmpty(name)) return false; else return true;
        }

        public bool HasDebuff(string iName)
        {
            ProcessManager.Injector.Lua_DoString(string.Format(@"(function()
	local name, rank, icon, count, debuffType, duration, expirationTime, source, isStealable = UnitDebuff(""player"", ""{0}"")
	return name, rank, icon, count, debuffType, duration, expirationTime, source, isStealable
end)()", iName));
            string name = ProcessManager.Injector.Lua_GetLocalizedText(0);
            if (string.IsNullOrEmpty(name)) return false; else return true;
        }

        public void MoveToCorpse(float tolerance)
        {
            MoveTo(CorpseLocation, tolerance);
        }

        public void MoveToCorpse()
        {
            MoveTo(CorpseLocation);
        }

        public void RetrieveCorpse()
        {
            ProcessManager.Injector.Lua_DoString(string.Format("RetrieveCorpse()"));
        }

        public void RepopMe()
        {
            ProcessManager.Injector.Lua_DoString(string.Format("RepopMe()"));
        }

        public float TargetBoundingRadius()
        {
            if (!HasTarget)
            {
                return 0.0f;
            }

            return CurTarget.BoundingRadius;
        }

        public bool IsCasting()
        {
            ProcessManager.Injector.Lua_DoString(string.Format(@"(function()
            local spell, rank, displayName, icon, startTime, endTime, isTradeSkill, castID, notInterruptible = UnitCastingInfo(""player"")
            return spell, rank, displayName, icon, startTime, endTime, isTradeSkill, castID, notInterruptible
end)()"));
            string spell = ProcessManager.Injector.Lua_GetLocalizedText(0);

            ProcessManager.Injector.Lua_DoString(string.Format(@"(function()
            local spell, rank, displayName, icon, startTime, endTime, isTradeSkill, notInterruptible = UnitChannelInfo(""player"")
            return spell, rank, displayName, icon, startTime, endTime, isTradeSkill, notInterruptible
end)()"));
            string channel = ProcessManager.Injector.Lua_GetLocalizedText(0);

            if (channel == "" && spell == "") return false; else return true;
        }

        public bool IsCasting(string spellName)
        {
            ProcessManager.Injector.Lua_DoString(string.Format(@"(function()
	local spell, rank, displayName, icon, startTime, endTime, isTradeSkill, castID, notInterruptible = UnitCastingInfo(""player"")
	return spell, rank, displayName, icon, startTime, endTime, isTradeSkill, castID, notInterruptible
end)()"));
            string spell = ProcessManager.Injector.Lua_GetLocalizedText(0);

            ProcessManager.Injector.Lua_DoString(string.Format(@"(function()
	local spell, rank, displayName, icon, startTime, endTime, isTradeSkill, notInterruptible = UnitChannelInfo(""player"")
	return spell, rank, displayName, icon, startTime, endTime, isTradeSkill, notInterruptible
end)()"));
            string channel = ProcessManager.Injector.Lua_GetLocalizedText(0);

            if (channel == spellName ||  spell == spellName) return true; else return false;
        }

        public Item GetMerchantItemInfo(int idx)
        {
            ProcessManager.Injector.Lua_DoString(string.Format(@"(function()
	local name, texture, price, quantity, numAvailable, isUsable, extendedCost = GetMerchantItemInfo({0})
	return name, texture, price, quantity, numAvailable, isUsable, extendedCost
end)()", idx));
            string name = ProcessManager.Injector.Lua_GetLocalizedText(0);
            string price = ProcessManager.Injector.Lua_GetLocalizedText(2);
            string quantity = ProcessManager.Injector.Lua_GetLocalizedText(3);
            Item item = new Item(name, 0, 0);
            item.Price = Convert.ToInt32(price);
            item.Quantity = Convert.ToInt32(quantity);
            return item;
        }

        public void BuyMerchantItem(int idx)
        {
            BuyMerchantItem(idx, 0);
        }

        public void BuyMerchantItem(int idx, int quantity)
        {
            if (quantity > 0)
            {
                ProcessManager.Injector.Lua_DoString(string.Format("BuyMerchantItem({0}, {1})", idx, quantity));
            }
            else
            {
                ProcessManager.Injector.Lua_DoString(string.Format("BuyMerchantItem({0})", idx));
            }
        }

        public void TargetMe()
        {
            ProcessManager.Injector.Lua_DoString(string.Format("TargetUnit(\"player\")"));
        }

        public void CastSpellByName(string name)
        {
            CastSpellByName(name, false);
        }

        public void CastSpellByName(string name, bool onSelf)
        {
            ProcessManager.Injector.CastSpellByName(name, onSelf);
        }

        public int HpPct
        {
            get { return (int) ((float) ((float) Hp/(float) MaxHp)*100); }
        }

        public int MpPct
        {
            get { return (int) ((float) ((float) Mp/(float) MaxMp)*100); }
        }

        public int TargetHpPct
        {
            get
            {
                if (TargetMaxHp == 0) return 0;
                return (int) ((TargetHp/TargetMaxHp)*100);
            }
        }

        public int TargetMpPct
        {
            get
            {
                if (TargetMaxMp == 0) return 0;
                return (int) ((TargetMp/TargetMaxMp)*100);
            }
        }
        
        public void DoString(string iCommand)
        {
            ProcessManager.Injector.Lua_DoString(iCommand);
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
            return (float) (Rotation*(180.0f/Math.PI));
        }

        public float TargetFacingDegrees()
        {
            return (float) (TargetFacing()*(180.0f/Math.PI));
        }

        public float AngleToTargetDegrees()
        {
            return (float) (AngleToTarget()*(180.0f/Math.PI));
        }

        #endregion
    }
}