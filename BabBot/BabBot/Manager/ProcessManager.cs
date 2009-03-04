using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Security;
using BabBot.Wow;
using Magic;

namespace BabBot.Manager
{
    public class ProcessManager
    {
        private static readonly Config config;
        private static readonly BlackMagic wowProcess;
        public static bool InGame;
        public static ObjectManager ObjectManager;
        public static Player Player;
        private static Process process;
        public static bool ProcessRunning;
        public static uint TLS;

        static ProcessManager()
        {
            config = new Config();
            wowProcess = new BlackMagic();
            ProcessRunning = false;
            Player = new Player();
            InGame = false;
            TLS = 0x0;
        }

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
            get { return config; }
        }

        public static void StartWow()
        {
            try
            {
                /// TODO: Read the game path from the registry:  Common.GetWowInstallationPath()
                /// TODO: In VISTA cosi non funziona manco per il cazzo (virtualization di merda) 
                /// Process.Start(Common.GetWowInstallationPath());
                /// http://www.bokebb.com/dev/english/2045/posts/204519378.shtml
                
                process = !Common.IsVista()
                              ? Process.Start(Common.GetWowInstallationPath(), "", Config.GuestUsername,
                                              new SecureString(), "")
                              : Process.Start(Common.GetWowInstallationPath());

                if (process != null)
                {
                    process.WaitForInputIdle(10000);
                }
            }
            catch (Win32Exception ex)
            {
                if (ex.NativeErrorCode == 267)
                {
                    throw new Exception(string.Format("{0} has no permissions to run an instance of WoW",
                                                      Config.GuestUsername));
                }
                throw;
            }
            catch 
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

        // TODO: This should actually be converted to the initialization function once we are into the game
        public static void FindTLS()
        {
            //search for the code pattern that we want (in this case, WoW TLS)
            TLS = SPattern.FindPattern(process.Handle, process.MainModule,
                                       "EB 02 33 C0 8B D 00 00 00 00 64 8B 15 00 00 00 00 8B 34 8A 8B D 00 00 00 00 89 81 00 00 00 00",
                                       "xxxxxx????xxx????xxxxx????xx????");

            Globals.ClientConnectionPointer = wowProcess.ReadUInt(TLS + 0x16);
            Globals.ClientConnection = wowProcess.ReadUInt(Globals.ClientConnectionPointer);
            Globals.ClientConnectionOffset = wowProcess.ReadUInt(TLS + 0x1C);
            Globals.CurMgr = wowProcess.ReadUInt(Globals.ClientConnection + Globals.ClientConnectionOffset);
            ObjectManager = new ObjectManager();


            if (TLS == uint.MaxValue)
            {
                throw new Exception("Could not find WoW's Object Manager.\nPlease press [ENTER] to continue...");
            }
        }
    }
}