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
using BabBot.Bot;
using BabBot.Manager;
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
        void FaceTarget();
        float DistanceFromTarget();
        float Facing();
        float TargetFacing();
        float AngleToTarget();
        float FacingDegrees();
        float TargetFacingDegrees();
        float AngleToTargetDegrees();
        void WalkToNextWayPoint(WayPointType wpType);
        void Stop();
        void PlayAction(PlayerAction action, bool toggle);
        void PlayAction(PlayerAction action);
        bool EnemyInSight();
        void FaceClosestEnemy();
        bool HasTarget();
        void MoveForward(int iTime);
        void MoveBackward(int iTime);
        void MoveToTarget(float tolerance);
        void MoveToClosestLootableMob();
        void FaceClosestLootableMob();
        void LootClosestLootableMob();
        void AddLastTargetToLootList();
        void CastSpellByName(string name, bool onSelf);
        void CastSpellByName(string name);
        int Hp();
        int Mp();
        int HpPct();
        int MpPct();
        bool IsTargetDead();
        void AttackTarget();
        void SpellStopCasting();
        bool IsMoving();
        bool IsAttacking();
        bool CanCast(string iName);
        int TargetHp();
        int TargetMp();
        int TargetHpPct();
        int TargetMpPct();
        void DoString(string iCommand);
        void Wait(int iTime);
        int Level();
        bool HasItem(Item i);
        bool HasItem(string i);
        void UseItem(Item i);
        void UseItem(string i);
        bool HasBuff(string name);
        bool HasDebuff(string name);
        void MoveToCorpse();
        void MoveToCorpse(float tolerance);
        float DistanceFromCorpse();
        void RetrieveCorpse();
        void RepopMe();
        float TargetBoundingRadius();
        bool IsCasting();
        bool IsCasting(string spellName);
        Item GetMerchantItemInfo(int idx);
        void BuyMerchantItem(int idx, int quantity);
        void TargetMe();
        bool IsInCombat();
    }

    public class PlayerWrapper : IPlayerWrapper
    {
        // Reference to the Player object
        private readonly WowPlayer player;

        public PlayerWrapper(WowPlayer iPlayer)
        {
            player = iPlayer;
        }

        #region IPlayerWrapper Members

        public string Test()
        {
            return "player.Test()";
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

        public void FaceTarget()
        {
            player.FaceTarget();
        }

        public float DistanceFromTarget()
        {
            return player.DistanceFromTarget();
        }

        public float Facing()
        {
            return player.Facing();
        }

        public float TargetFacing()
        {
            return player.TargetFacing();
        }

        public float AngleToTarget()
        {
            return player.AngleToTarget();
        }

        public float FacingDegrees()
        {
            return player.FacingDegrees();
        }

        public float TargetFacingDegrees()
        {
            return player.TargetFacingDegrees();
        }

        public float AngleToTargetDegrees()
        {
            return player.AngleToTargetDegrees();
        }

        public void WalkToNextWayPoint(WayPointType wpType)
        {
            player.WalkToNextWayPoint(wpType);
        }

        public void Stop()
        {
            player.Stop();
        }

        public void PlayAction(PlayerAction action, bool toggle)
        {
            player.PlayAction(action, toggle);
        }

        public void PlayAction(PlayerAction action)
        {
            player.PlayAction(action);
        }

        public bool EnemyInSight()
        {
            return player.EnemyInSight();
        }

        public void FaceClosestEnemy()
        {
            player.FaceClosestEnemy();
        }

        public bool HasTarget()
        {
            return player.HasTarget();
        }

        public void MoveForward(int iTime)
        {
            player.MoveForward(iTime);
        }

        public void MoveToTarget(float tolerance)
        {
            player.MoveToTarget(tolerance);
        }

        public void FaceClosestLootableMob()
        {
            player.FaceClosestLootableMob();
        }

        public void MoveToClosestLootableMob()
        {
            player.MoveToClosestLootableMob();
        }

        public void LootClosestLootableMob()
        {
            player.LootClosestLootableMob();
        }

        public void AddLastTargetToLootList()
        {
            player.AddLastTargetToLootList();
        }

        public void CastSpellByName(string name)
        {
            CastSpellByName(name, false);
        }

        public void CastSpellByName(string name, bool onSelf)
        {
            ProcessManager.Injector.CastSpellByName(name, onSelf);
        }

        public int Hp()
        {
            return (int)player.Hp;
        }

        public int Mp()
        {
            return (int)player.Mp;
        }

        public int HpPct()
        {
            return (int)((player.Hp/player.MaxHp)*100);
        }

        public int MpPct()
        {
            return (int)((player.Mp / player.MaxMp) * 100);
        }

        public bool IsTargetDead()
        {
            return player.IsTargetDead();
        }

        public void AttackTarget()
        {
            player.AttackTarget();
        }

        public void SpellStopCasting()
        {
            player.SpellStopCasting();
        }

        public bool IsMoving()
        {
            return player.IsMoving();
        }

        public void MoveBackward(int iTime)
        {
            player.MoveBackward(iTime);
        }

        public bool IsAttacking()
        {
            return player.IsAttacking();
        }

        public bool CanCast(string iName)
        {
            return player.CanCast(iName);
        }

        public int TargetHp()
        {
            return (int)player.TargetHp;
        }

        public int TargetMp()
        {
            return (int)player.TargetMp;
        }

        public int TargetHpPct()
        {
            return (int)((player.TargetHp / player.TargetMaxHp) * 100);
        }

        public int TargetMpPct()
        {
            return (int)((player.TargetMp / player.TargetMaxMp) * 100);
        }

        public void DoString(string iCommand)
        {
            ProcessManager.Injector.Lua_DoString(iCommand);
        }

        public void Wait(int iTime)
        {
            player.Wait(iTime);
        }

        public int Level()
        {
            return (int)player.Level;
        }
        
        public bool HasItem(Item i)
        {
            return player.HasItem(i);
        }

        public bool HasItem(string i)
        {
            return player.HasItem(i);
        }

        public void UseItem(Item i)
        {
            player.UseItem(i);
        }

        public void UseItem(string i)
        {
            player.UseItem(i);
        }

        public bool HasBuff(string name)
        {
            return player.HasBuff(name);
        }

        public bool HasDebuff(string name)
        {
            return player.HasDebuff(name);
        }

        public void MoveToCorpse()
        {
            player.MoveToCorpse();
        }

        public void MoveToCorpse(float tolerance)
        {
            player.MoveToCorpse(tolerance);
        }

        public float DistanceFromCorpse()
        {
            return player.DistanceFromCorpse();
        }
        
        public void RetrieveCorpse()
        {
            player.RetrieveCorpse();
        }

        public void RepopMe()
        {
            player.RepopMe();
        }

        public float TargetBoundingRadius()
        {
            return player.TargetBoundingRadius();
        }

        public bool IsCasting()
        {
            return player.IsCasting();
        }

        public bool IsCasting(string spellName)
        {
            return player.IsCasting(spellName);
        }

        public Item GetMerchantItemInfo(int idx)
        {
            return player.GetMerchantItemInfo(idx);
        }

        public void BuyMerchantItem(int idx, int quantity)
        {
            player.BuyMerchantItem(idx, quantity);
        }

        public void TargetMe()
        {
            player.TargetMe();
        }

        public bool IsInCombat()
        {
            return player.IsInCombat();
        }
        #endregion
    }
}