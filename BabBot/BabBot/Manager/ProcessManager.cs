using System;
using System.ComponentModel;
using System.Diagnostics;
using BabBot.Bot;
using BabBot.Common;
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

        #endregion

        private static readonly Config config;
        private static readonly BlackMagic wowProcess;
        public static bool InGame;
        public static ObjectManager ObjectManager;
        public static Player Player;
        private static Process process;
        public static bool ProcessRunning;
        public static uint TLS;
        public static bool Initialized;
        public static Profile Profile;
        public static int WowHWND;
        public static BotManager BotManager;
        public static CommandManager CommandManager;

        static ProcessManager()
        {
            config = new Config();
            wowProcess = new BlackMagic();
            ProcessRunning = false;
            Player = new Player(CommandManager);
            InGame = false;
            TLS = 0x0;
            Initialized = false;
            Profile = new Profile();
            WowHWND = 0;
            BotManager = new BotManager();
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
                // process.WaitForInputIdle(15000);
                if (process != null)
                {
                    WowHWND = AppHelper.WaitForWowWindow();
                    // Set before to use BlackMagic methods
                    process.EnableRaisingEvents = true;
                    process.Exited += exitProcess;
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
                WoWProcessEnded(((Process)sender).Id);
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
                string wowPath = AppHelper.GetWowInstallationPath();
                if (!string.IsNullOrEmpty(wowPath))
                {
                    process = AppHelper.RunAs(Config.GuestUsername, "", null, wowPath);
                    afterProcessStart();
                }
                else
                {
                    throw new Exception("Wow is not installed or the registry key is missed.");
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

        /// <summary>
        /// Update all player informations: hp,mana,xp,position,etc...
        /// </summary>
        public static void UpdatePlayer()
        {
            if (!Initialized)
            {
                return;
            }

            Player.UpdateFromClient();
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
            Player.AttachUnit(ObjectManager.GetLocalPlayerObject());
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
                Scripting.Host.Go();
                Initialized = true;
            }
            catch
            {
                throw new Exception("Initialize failed!");
            }
        }
    }
}