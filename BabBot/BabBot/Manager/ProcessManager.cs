using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security;
using System.Text;
using BabBot.Wow;
using Magic;

namespace BabBot.Manager
{
    public class ProcessManager
    {
        private static BlackMagic wowProcess;
        private static Config config;
        private static Process process;
        public static bool ProcessRunning;
        public static Player Player;
        public static bool InGame;

        public static BlackMagic WowProcess
        {
            get
            {
                if (wowProcess != null)
                {
                    return wowProcess;
                }
                return null;
            }
        }

        public static Config Config
        {
            get { return config;}
        }

        static ProcessManager()
        {
            config = new Config();
            wowProcess = new BlackMagic();
            ProcessRunning = false;
            Player = new Player();
            InGame = false;
        }

        public static void StartWow()
        {
            try
            {
                /// TODO: Read the game path from the registry
                process = Process.Start(Config.WowExePath, "", Config.GuestUsername, new SecureString(), "");
                if (process != null)
                {
                    process.WaitForInputIdle(10000);
                }
            }
            catch (Exception)
            {
                throw new Exception("Cannot run an instance of WoW");
            }

            if (process == null)
            {
                throw new Exception("Cannot obtain process information");
            }

            try
            {
                ProcessRunning = wowProcess.OpenProcessAndThread(process.Id);
            }
            catch (Exception ex)
            {
                throw (ex);
            }

        }

        public static void UpdatePlayerLocation()
        {
            if (!InGame)
            {
                return;
            }

            float x = wowProcess.ReadFloat(Globals.PlayerBaseOffset + Globals.PlayerXOffset);
            float y = wowProcess.ReadFloat(Globals.PlayerBaseOffset + Globals.PlayerYOffset);
            float z = wowProcess.ReadFloat(Globals.PlayerBaseOffset + Globals.PlayerZOffset);

            Player.Location = new Vector3D(x, y, z);

            Player.CurTargetGuid = wowProcess.ReadUInt64(Globals.PlayerCurTargetGuidOffset);
            
        }

        public static void UpdatePlayerStats()
        {
            if (!InGame)
            {
                return;
            }

            Player.Hp = wowProcess.ReadUInt(Globals.PlayerBaseOffset + Globals.PlayerHealthOffset);
            Player.MaxHp = wowProcess.ReadUInt(Globals.PlayerBaseOffset + Globals.PlayerMaxHealthOffset);
            Player.Mp = wowProcess.ReadUInt(Globals.PlayerBaseOffset + Globals.PlayerManaOffset);
            Player.MaxMp = wowProcess.ReadUInt(Globals.PlayerBaseOffset + Globals.PlayerMaxManaOffset);
            Player.Xp = wowProcess.ReadUInt(Globals.PlayerBaseOffset + Globals.PlayerXpOffset);

        }

        public static void UpdatePlayer()
        {
            if (!InGame)
            {
                return;
            }

            UpdatePlayerLocation();
            UpdatePlayerStats();
        }

        public static void CheckInGame()
        {
            try
            {
                WowProcess.ReadUInt(WowProcess.ReadUInt(WowProcess.ReadUInt(Globals.GameOffset) +
                                       Globals.PlayerBaseOffset1) + Globals.PlayerBaseOffset2);
                InGame = true;
            }
            catch
            {
                InGame = false;
            }
        }
    }
}
