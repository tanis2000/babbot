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
using System.ComponentModel;
using System.Diagnostics;
using BabBot.Bot;
using BabBot.Common;
using BabBot.Scripting;
using BabBot.Wow;
using Magic;

namespace BabBot.Manager
{
    /// <summary>
    /// Main class for reading, writing and gathering process information 
    /// </summary>
    public class ProcessManager
    {
        #region Delegates

        /// <summary>
        /// Update notification on player fields
        /// </summary>
        public delegate void PlayerUpdateEventHandler();

        public delegate void PlayerWayPointEventHandler(Vector3D waypoint);

        /// <summary>
        /// Error is the Win32Exception.Message thrown.
        /// </summary>
        public delegate void WoWProcessAccessFailedEventHandler(string error);

        /// <summary>
        /// Process is the ID of the process that exited.
        /// </summary>
        public delegate void WoWProcessEndedEventHandler(int process);

        /// <summary>
        /// Error is the Win32Exception.Message thrown.
        /// </summary>
        public delegate void WoWProcessFailedEventHandler(string error);

        /// <summary>
        /// Process is the ID of the process that started.
        /// </summary>
        public delegate void WoWProcessStartedEventHandler(int process);

        #endregion

        #region WOWApplication Events

        /// <summary>
        /// ProcessFailed is fired if an exception is thrown when attempting to start the
        ///  process.
        /// </summary>
        public static event WoWProcessFailedEventHandler WoWProcessFailed;

        /// <summary>
        /// ProcessEnded is fired where the started process exits.
        /// </summary>
        public static event WoWProcessEndedEventHandler WoWProcessEnded;

        /// <summary>
        /// ProcessStarted is fired if the process successfully starts.
        /// </summary>
        public static event WoWProcessStartedEventHandler WoWProcessStarted;

        /// <summary>
        /// ProcessAccessFailed is fired if the current user does not have permission to
        ///  access the new process.
        /// </summary>
        public static event WoWProcessAccessFailedEventHandler WoWProcessAccessFailed;

        public static event PlayerUpdateEventHandler PlayerUpdate;
        public static event PlayerWayPointEventHandler PlayerWayPoint;

        #endregion

        #region Player Events

        #endregion

        private static Config config;
        private static readonly BlackMagic wowProcess;
        public static BotManager BotManager;
        public static CommandManager CommandManager;
        public static bool InGame;
        public static bool Initialized;
        public static ObjectManager ObjectManager;
        public static WowPlayer Player;
        private static Process process;
        public static bool ProcessRunning;
        public static Profile Profile;
        public static uint TLS;
        public static int WowHWND;
        public static Host ScriptHost;
        //public static WayPointManager WayPointManager;

        static ProcessManager()
        {
            config = new Config();
            wowProcess = new BlackMagic();
            ProcessRunning = false;
            CommandManager = new CommandManager();
            InGame = false;
            TLS = 0x0;
            Initialized = false;
            Profile = new Profile();
            WowHWND = 0;
            BotManager = new BotManager();
            ScriptHost = new Host();
            //WayPointManager = WayPointManager.Instance;
            WayPointManager.Instance.Init();
        }

        /// <summary>
        /// Get the object for the manipulation of memory and general "hacking"
        /// of another process
        /// </summary>
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

        /// <summary>
        /// Get the basic configuration for babBot: deprecated
        /// </summary>
        public static Config Config
        {
            get { return config; }
            set { config = value; }
        }

        #region Private Methods

        private static void afterProcessStart()
        {
            if (WoWProcessStarted != null && process != null)
            {
                WoWProcessStarted(process.Id);
            }
            try
            {
                ProcessRunning = false;
                // TODO: è un po sporca ogni tanto scazza, meglio usare una FindWindow, per
                // sapere esattamente quando aprire il processo con BlackMagic
                //process.WaitForInputIdle(15000);
                if (process != null)
                {
                    // Set before to use BlackMagic methods
                    process.EnableRaisingEvents = true;
                    process.Exited += exitProcess;

                    WowHWND = AppHelper.WaitForWowWindow();
                    CommandManager.WowHWND = WowHWND;

                    ProcessRunning = wowProcess.OpenProcessAndThread(process.Id);
                }
            }
            catch (Exception e)
            {
                if (WoWProcessAccessFailed != null)
                {
                    WoWProcessAccessFailed(e.Message);
                }
            }
        }

        private static void exitProcess(object sender, EventArgs e)
        {
            // TODO: clean everything here...
            ProcessRunning = false;
            WowHWND = 0;

            if (WoWProcessEnded != null)
            {
                WoWProcessEnded(((Process) sender).Id);
            }
        }

        #endregion

        /// <summary>
        /// Try to run the WoW process 
        /// </summary>
        public static void StartWow()
        {
            try
            {
                // Perry style = paranoid ;-)
                string wowPath = Config.WowExePath;

                // Ok, no one configured the path, let's try to find it on our own
                if (string.IsNullOrEmpty(wowPath))
                {
                    wowPath = AppHelper.GetWowInstallationPath();
                }

                if (!string.IsNullOrEmpty(wowPath))
                {
                    process = AppHelper.RunAs(Config.GuestUsername, Config.GuestPassword, null, wowPath);
                    afterProcessStart();
                }
                else
                {
                    throw new Exception("Wow is not installed or the registry key is missing.");
                }
            }
            catch (Win32Exception w32e)
            {
                if (WoWProcessFailed != null)
                {
                    WoWProcessFailed(w32e.Message);
                }
            }
            catch (Exception ex)
            {
                if (WoWProcessAccessFailed != null)
                {
                    WoWProcessAccessFailed(ex.Message);
                }
            }
        }

        /// <summary>
        /// Try to attach on the WoW process 
        /// </summary>
        public static void AttachToWow()
        {
            try
            {
                process = AppHelper.GetRunningWoWProcess(Config.GuestUsername);
                if (process != null)
                {
                    afterProcessStart();
                }
                else
                {
                    throw new Exception("Wow is not running in the current context.");
                }
            }
            catch (Win32Exception w32e)
            {
                if (WoWProcessFailed != null)
                {
                    WoWProcessFailed(w32e.Message);
                }
            }
            catch (Exception ex)
            {
                if (WoWProcessAccessFailed != null)
                {
                    WoWProcessAccessFailed(ex.Message);
                }
            }
        }

        public static void ResetWayPoint()
        {
            if (PlayerWayPoint != null)
            {
                if (Player == null)
                {
                    throw new Exception("Cannot reset waypoints. No Player object found.");
                }
                Player.LastLocation.X = 0;
                Player.LastLocation.Y = 0;
                Player.LastLocation.Z = 0;
            }
        }

        /// <summary>
        /// Update all player informations: hp,mana,xp,position,etc...
        /// </summary>
        public static void UpdatePlayer()
        {
            if (!Initialized)
            {
                return;
            }

            if (PlayerUpdate != null)
            {
                PlayerUpdate();
            }

            if (PlayerWayPoint != null)
            {
                Vector3D last = Player.LastLocation;
                if (last.X == 0 && last.Y == 0 && last.Z == 0)
                {
                    Player.LastLocation = Player.Location;
                    last = Player.LastLocation;
                }

                if ((int) Player.HGetDistance(last, false) > 5 && (int) Player.HGetDistance(last, false) < 7)
                {
                    Player.LastLocation = Player.Location;
                    PlayerWayPoint(Player.LastLocation);
                }
            }
        }

        /// <summary>
        /// Check if you are logged into the WoW game
        /// </summary>
        public static void CheckInGame()
        {
            try
            {
                WowProcess.ReadUInt(WowProcess.ReadUInt(WowProcess.ReadUInt(Globals.GameOffset) +
                                                        Globals.PlayerBaseOffset1) + Globals.PlayerBaseOffset2);
                InGame = true;
                if (!Initialized)
                {
                    Initialize();
                }
            }
            catch
            {
                InGame = false;
                Initialized = false;
            }
        }

        /// <summary>
        /// Try to find the Thread Local Storage: must be logged in WoW
        /// </summary>
        public static void FindTLS()
        {
            // search for the code pattern that we want (in this case, WoW TLS)
            TLS = SPattern.FindPattern(process.Handle, process.MainModule,
                                       "EB 02 33 C0 8B D 00 00 00 00 64 8B 15 00 00 00 00 8B 34 8A 8B D 00 00 00 00 89 81 00 00 00 00",
                                       "xxxxxx????xxx????xxxxx????xx????");


            if (TLS == uint.MaxValue)
            {
                throw new Exception("Could not find WoW's Object Manager.");
            }
        }

        private static void InitializeObjectManager()
        {
            Globals.ClientConnectionPointer = wowProcess.ReadUInt(TLS + 0x16);
            Globals.ClientConnection = wowProcess.ReadUInt(Globals.ClientConnectionPointer);
            Globals.ClientConnectionOffset = wowProcess.ReadUInt(TLS + 0x1C);
            Globals.CurMgr = wowProcess.ReadUInt(Globals.ClientConnection + Globals.ClientConnectionOffset);
            ObjectManager = new ObjectManager();
            WowObject wo = new WowObject();
            wo.ObjectPointer = ObjectManager.GetLocalPlayerObject();
            wo.Guid = ObjectManager.GetGUIDByObject(wo.ObjectPointer);
            wo.Type = ObjectManager.GetTypeByObject(wo.ObjectPointer);
            Player = new WowPlayer(wo, CommandManager);
        }

        /// <summary>
        /// Search for the TLS and Initialize the bot once the user is logged in
        /// </summary>
        public static void Initialize()
        {
            try
            {
                FindTLS();
                InitializeObjectManager();
                ScriptHost.Start();
                Initialized = true;
            }
            catch (Exception ex)
            {
                throw new Exception("Initialize failed!");
            }
        }
    }
}