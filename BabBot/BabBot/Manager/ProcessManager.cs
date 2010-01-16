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

// TODO Check for crush and implement WOW_CRUSHED status

using System;
using System.IO;
using System.Net;
using Pather.Graph;
using System.Threading;
using System.Diagnostics;
using System.Collections;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Collections.Generic;

using BabBot.Bot;
using BabBot.Wow;
using BabBot.States;
using BabBot.Common;
using BabBot.Scripting;

using Magic;

namespace BabBot.Manager
{

    #region Exceptions

    public class ConfigFileChangedException : Exception
    {
        public ConfigFileChangedException() :
            base("Configuration file doesn't match application settings") { }
    }

    public class WoWDataNotFoundException : Exception
    {
        public WoWDataNotFoundException(string version) :
            base("WoWData.xml doesn't contain data for WoW version '" + version + "'") { }
    }

    #endregion

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

        /// <summary>
        /// Updates the status bar element
        /// </summary>
        /// <param name="iStatus">Information for status bar label</param>
        public delegate void UpdateStatusEventHandler(string status);

        /// <summary>
        /// Delegate method to update status bar together with tooltip
        /// </summary>
        /// <param name="status">Information for status bar label</param>
        /// <param name="tooltip">Tooltip for status bar label</param>
        public delegate void UpdateStatusTooltipDelegate(string status, string tooltip);

        /// <summary>
        /// First time run event
        /// </summary>
        public delegate void FirstTimeRunHandler();

        /// <summary>
        /// Configuration file change event
        /// </summary>
        public delegate void ConfigFileChangedHandler();

        /// <summary>
        /// Show Error Message
        /// </summary>
        /// <param name="err"></param>
        public delegate void ShowErrorMessageHandler(string err);

        /// <summary>
        /// Bot Progress Bar start procedure
        /// </summary>
        /// <param name="max">Max number shown in progress bar</param>
        /// <param name="tooltip">Text shown in progress bar tooltip</param>
        public delegate void BotProgressStartHandle(int max, string tooltip);

        /// <summary>
        /// Bot Progress Bar change procedure
        /// </summary>
        /// <param name="value"></param>
        public delegate void BotProgressChangeHandle(int value);

        /// <summary>
        /// Bot progress bar end procedure
        /// </summary>
        public delegate void BotProgressEndHandle();

        /// <summary>
        /// Path visualization procedure
        /// </summary>
        /// <param name="v_arr">Array of waypoints</param>
        public delegate void DisplayPathCalculatedHandle(Vector3D[] v_arr, int retry);

        #endregion

        #region Application Events

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

        /// <summary>
        /// Fired when player Entered to WoW world
        /// </summary>
        public static event PlayerUpdateEventHandler WoWInGame;
        public static event PlayerUpdateEventHandler WoWGameLoaded;

        public static event PlayerUpdateEventHandler PlayerUpdate;
        public static event PlayerWayPointEventHandler PlayerWayPoint;

        public static event UpdateStatusEventHandler UpdateGameStatus;
        public static event UpdateStatusEventHandler UpdateAppStatus;
        public static event UpdateStatusTooltipDelegate UpdateBotStatus;

        public static event FirstTimeRunHandler FirstTimeRun;
        public static event ConfigFileChangedHandler ConfigFileChanged;
        public static event ShowErrorMessageHandler ShowErrorMessage;


        public static event BotProgressStartHandle BotProgressStart;
        public static event BotProgressChangeHandle BotProgressChange;
        public static event BotProgressEndHandle BotProgressEnd;

        /// <summary>
        /// Event raising when bot calculated path and need visualize it
        /// </summary>
        public static event DisplayPathCalculatedHandle DisplayPathCalculated;

        /// <summary>
        /// Event raising when bot completed traveling (finish traveling state)
        /// </summary>
        public static event PlayerUpdateEventHandler TravelCompleted;

        #endregion

        #region Enumerables
        
        /// <summary>
        /// List of application statuses
        /// </summary>
        public enum ProcessStatuses : byte
        {
            IDLE = 0,
            // Starting WoW
            WOW_STARTING = 1,
            // Injecting DLL
            WOW_INJECTING = 2,
            // Initializing internal parameters
            WOW_INITIALIZING = 3,
            // Looking for TLS
            WOW_LOOK_FOR_TLS = 4,
            // WoW started and running
            WOW_RUNNING = 5,
            // Detected that Wow crushed
            WOW_CRUSHED = 254,
            // Detected WoW terminated
            WOW_CLOSED = 255
        }

        /// <summary>
        /// In-Game statuses
        /// </summary>
        public enum GameStatuses : byte
        {
            // self explaining
            NOT_STARTED = 0,
            // wow started and we on login page
            INIT = 1,
            // on login page (but no auto-login set or auto-login failed)
            // on disconnect page with reason above
            IDLE = 2,
            // Executing auto-login
            // Next state IDLE or ENTERING_WORLD
            LOGGING = 3,
            // After Auto-Login successfull we assume that world is loading
            ENTERING_WORLD = 4,
            // Connection manager found and character initialized with zone & continent
            INITIALIZED = 5,
            // Happened ..
            DISCONNECTED = 255
        }

        /// <summary>
        /// Keep list of internal bugs that cause application termination
        /// </summary>
        public enum Bugs : byte {
          CONFIG_NOT_FOUND = 1,
          WRONG_CONFIG_VERSION = 2,
          WRONG_WOW_VERSION = 3,
          WOWDATA_NOT_FOUND= 4,
          ERROR_LOAD_CONFIG = 5,
          XML_ERROR = 6,
          LUA_NOT_FOUND = 7,
          WRONG_LUA_RESULT_LIST = 8,
          WRONG_LUA_PARAMETER_LIST = 9,
          NULL_LUA_RETURN = 10,
          WRONG_LUA_RETURN_LIST = 11,
        }

        #endregion
        
        #region Public Properties

        private static readonly BlackMagic wowProcess;
        public static BotManager BotManager;
        public static Caronte.Caronte Caronte;
        public static CommandManager CommandManager;
        public static InjectionManager Injector;
        public static DataManager DataManager;
        private static Config config;
        public static ObjectManager ObjectManager;
        public static WowPlayer Player;
        public static InGameChar MyChar;
        private static Process process;
        public static Profile Profile;
        public static Host ScriptHost;
        public static uint TLS;
        public static int WowHWND;
        private static bool arun = false;
        // Config file name
        private static string ConfigFileName = "config.xml";

        // Current version of config file
        private static readonly int ConfigVersion = 3;


        private static GameStatuses _gstatus = GameStatuses.NOT_STARTED;
        private static ProcessStatuses _pstatus = ProcessStatuses.IDLE;

        internal static AppConfig AppConfig
        {
            get { return DataManager.AppConfig; }
        }

        internal static GlobalOffsets GlobalOffsets
        {
            get { return DataManager.Globals; }
        }

        /// <summary>
        /// Return Process Status
        /// </summary>
        public static GameStatuses GameStatus
        {
            get { return _gstatus; }
            set 
            {
                if (_gstatus != value)
                {
                    OnUpdateGameStatus(Enum.GetName(typeof(GameStatuses), value));
                    _gstatus = value;
                }
            }
        }

        public static ProcessStatuses ProcessStatus
        {
            get { return _pstatus; }
            private set
            {
                if (_pstatus != value)
                {
                    OnUpdateAppStatus(Enum.GetName(typeof(ProcessStatuses), value));
                    _pstatus = value;
                }
            }
        }

        /// <summary>
        /// Get the object for the manipulation of memory and general "hacking"
        /// of another process
        /// </summary>
        public static BlackMagic WowProcess
        {
            get { return wowProcess; }
        }

        /// <summary>
        /// Get the basic configuration for babBot: deprecated
        /// </summary>
        public static Config Config
        {
            get { return config; }
            set { config = value; }
        }

        
        public static ArrayList TalentTemplateList
        {
            get
            {
                string[] dir;
                ArrayList res = new ArrayList(); ;

                // Scan Profiles/Talents for list
                string wdir = config.ProfilesDir +
                        System.IO.Path.DirectorySeparatorChar + "Talents";
                try
                {
                    dir = Directory.GetFiles(wdir, "*.xml");
                }
                catch (System.IO.DirectoryNotFoundException)
                {
                    if (WoWProcessFailed != null)
                    ShowError("Directory '" + wdir + "' not found");
                    return res;
                }

                // Check each file
                foreach (string fname in dir)
                {
                    try
                    {
                        Talents tlist = ProcessManager.ReadTalents(fname);
                        if ((tlist != null) && (tlist.Description != null))
                            res.Add(tlist);
                    }
                    catch
                    {
                        // Continue 
                    }
                }

                return res;
            }
        }

        public static bool InGame
        {
            get { return (_gstatus == GameStatuses.INITIALIZED); }
        }

        public static bool InWorld
        {
            get { return ((_gstatus == GameStatuses.INITIALIZED) || 
                    (_gstatus == GameStatuses.ENTERING_WORLD)); }
        }

        #endregion

        #region Private Methods

        private static void AfterProcessStart()
        {
            Debug("char", "Executing AfterStart ...");

            if (WoWProcessStarted != null)
                WoWProcessStarted(process.Id);

            try
            {
                // Set before using any BlackMagic methods
                process.EnableRaisingEvents = true;
                process.Exited += exitProcess;

                IntPtr hnd = process.MainWindowHandle;
                WowHWND = AppHelper.WaitForWowWindow((uint) process.Id);
                CommandManager.WowHWND = WowHWND;

                //verify we haven't already opened it, like when we do the injection
                if (!wowProcess.IsProcessOpen)
                    wowProcess.OpenProcessAndThread(process.Id);

                // We don't let anyone do anything until wow has finished launching
                // We look for the TLS first
                ProcessStatus = ProcessStatuses.WOW_LOOK_FOR_TLS;
                while (!FindTLS())
                {
                   Debug("char", "Looking for the TLS ...");
                   Thread.Sleep(250);
                }

                if (config.WowPos != null)
                    BabBot.Common.WindowSize.SetPositionSize((IntPtr)WowHWND,
                        config.WowPos.Pos.X, config.WowPos.Pos.Y,
                        config.WowPos.Pos.Width, config.WowPos.Pos.Height);
                else if (config.WoWInfo.Resize)
                    BabBot.Common.WindowSize.SetPositionSize(
                                    (IntPtr)WowHWND, 0, 0, 328, 274);


                // At this point it should be safe to do any LUA calls
                if (WoWGameLoaded != null)
                    WoWGameLoaded();

                ProcessStatus = ProcessStatuses.WOW_RUNNING;
                GameStatus = GameStatuses.INIT;

                Debug("char", "AfterStart completed.");
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
            // Prevent from recursion
            if (ProcessStatus == ProcessStatuses.WOW_CLOSED)
                return;

            // Do it first
            ProcessStatus = ProcessStatuses.WOW_CLOSED;
            GameStatus = GameStatuses.NOT_STARTED;

            // Blah blah blah after
            Log("char", "WoW termination detected");
            Debug("char", "Executing After WoW termination ...");

            // Cleaning
            WowHWND = 0;
            Injector.IsLuaRegistered = false;
            Injector.ClearLuaInjection();
            wowProcess.CloseProcess();

            // TODO Check for crush

            if (WoWProcessEnded != null)
                WoWProcessEnded(((Process) sender).Id);

            Debug("char", "WoW termination completed");
            
            // Check for crush
            uint err_pid = AppHelper.FindPidWindowByTitle(AppHelper.WOWERROR_APP_NAME);

            if (err_pid != 0 && config.Account.ReStart)
            {
                Log("char", "Crush detected. Clearing task list ... ");

                Process err_proc = null;

                try
                {
                    err_proc = System.
                        Diagnostics.Process.GetProcessById((int)err_pid);
                }
                catch
                {
                    // Nothing wrong. Might be closed already by user.
                }

                do
                {
                    try
                    {
                        if (process != null)
                            process.Kill();
                    }
                    catch
                    {
                        // Already killed
                        process = null;
                    }

                    try
                    {
                        if (err_proc != null)
                        {
                            err_proc.Kill();
                            err_proc = System.Diagnostics.Process.GetProcessById((int)err_pid);
                        }
                    }
                    catch
                    {
                        // Already killed or doesn't exists on the list
                        err_proc = null;

                    }

                    Thread.Sleep(1000);
                } while (process != null || err_proc != null);
                    
                Log("char", "Cleaing completed. Restarting bot ... ");

                StartBot();
            }
            else
                ProcessStatus = ProcessStatuses.IDLE;
        }

        private static void Log(string facility, string msg)
        {
            Output.Instance.Log(facility, msg);
        }

        private static void Debug(string facility, string msg)
        {
            Output.Instance.Debug(facility, msg);
        }

        private static void Error(string facility, string msg)
        {
            Output.Instance.LogError(facility, msg);
        }

        private static void Error(string facility, string msg, Exception e)
        {
            Output.Instance.LogError(facility, msg, e);
        }

        /// <summary>
        /// Read URL and return result. Saved for future use
        /// </summary>
        /// <param name="url">URL to retrieve data from</param>
        /// <returns>HTTP response</returns>
        private string ReadURL(string url)
        {
            // Create a request for the URL.         
            WebRequest request = WebRequest.Create(url);
            // If required by the server, set the credentials.
            request.Credentials = CredentialCache.DefaultCredentials;
            // Get the response.
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            // Display the status.
            // response.StatusDescription;
            // Get the stream containing content returned by the server.
            Stream stream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(stream);
            // Read the content.
            string res = reader.ReadToEnd();
            // Cleanup the streams and the response.
            reader.Close();
            stream.Close();
            response.Close();

            return res;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Class constructor
        /// </summary>
        static ProcessManager()
        {
            config = new Config();
            wowProcess = new BlackMagic();
            CommandManager = new CommandManager();
            Injector = new InjectionManager();
            TLS = 0x0;
            Profile = new Profile();
            WowHWND = 0;
            BotManager = new BotManager();
            ScriptHost = new Host();
            WayPointManager.Instance.Init();
            Caronte = new Caronte.Caronte();
        }

        /// <summary>
        /// Initialize configuration parameters. 
        /// This method must be first called on application start
        /// For GUI mode from Main Form constructor
        /// For CMD mode from main thread
        /// The rest of events can be bind later but those 3 are mandatory
        /// </summary>
        /// <param name="OnFirstTimeRun">OnFirstTimeRun event handler</param>
        /// <param name="OnConfigFileChanged">OnConfigFileChanged event handler</param>
        /// <param name="OnShowErrorMessage">OnShowErrorMessage event handler</param>
        public static void Initialize(FirstTimeRunHandler OnFirstTimeRun,
            ConfigFileChangedHandler OnConfigFileChanged,
            ShowErrorMessageHandler OnShowErrorMessage)
        {
            Process.EnterDebugMode();

            // Load the configuration file
            // Configuration must be loaded first of all. 
            FirstTimeRun += OnFirstTimeRun;
            ConfigFileChanged += OnConfigFileChanged;
            ShowErrorMessage += OnShowErrorMessage;

            XmlManager.Init();
            
            //\\ TEST
            // SaveNpcData();

            // Everything else after
            LoadConfig();
            XmlManager.Merge(config.WoWInfo.Version);
            XmlManager.AfterInit();

#if DEBUG
            List<Route> lr = new List<Route>();
            Route r1 = new Route(new Endpoint(EndpointTypes.UNDEF, "", new Vector3D(1, 1, 1)),
                new Endpoint(EndpointTypes.UNDEF, "", new Vector3D(100, 100, 100)), "", true);
            lr.Add(r1);
            RouteListManager.Waypoints.Add(1F, lr);

            Route r2 = RouteListManager.FindRoute(new Vector3D(2, 2, 2));
            if (!ReferenceEquals(r1, r2))
                throw new Exception("Waypoints access failed");

            //\\ Test
            // Quest merge test
            Quest q1 = new Quest(1, "Test 1", "Test", "", 1, new int[6], new string[3], "", "", "");
            Quest q2 = new Quest(1, "Test 1", "Test", "", 1, new int[6], new string[3], "", "", "");

            if (!q1.Equals(q2))
                 throw new Exception("Quest Test 1 failed");

            // Add link  and dest 
            q2.Relations.Add(11);
            q2.DestName = "npc";

            // Merge
            q1.MergeWith(q2);

            // check
            if (!q1.DestName.Equals("npc"))
                throw new Exception("Quest Test 2 failed");

            if (!q1.RelatedTo.Equals("11"))
                throw new Exception("Quest Test 3 failed");

            // Add extra relation
            q2.Relations.Add("10");
            q1.MergeWith(q2);
            if (!q1.RelatedTo.Equals("10,11"))
                throw new Exception("Quest Test 4 failed");

            if (!typeof(IMergeable).IsAssignableFrom(typeof(Quest)))
                throw new Exception("Quest Test 5 failed");
            /*
            NPC npc1 = ndata.Items[0].Items[2];
            NPC npc2 = ndata.Items[0].Items[2];

            if (!npc1.Equals(npc2))
                Environment.Exit(1);
            */
            /*
            LuaFunction lf = CurWoWVersion.FindLuaFunction("GetNpcDialogInfo");
            string c = lf.Code;
            // string c = "{0} {{}} {{'dd','xx'}}";
            string s = string.Format(c, 1);
            */
            /*
            NPC npc = new NPC();

            npc.Name = "Test XXX";
            npc.WPList.Add(new Vector3D(1, 2, 3));
            npc.AddService(new ClassTrainingService("HUNTER"));
            npc.AddQuest(new QuestHeader("Catch if u can", 1));

            SaveNpcData(); */
#endif
        }

        /// <summary>
        /// Redirect error message to ShowErrorMessage handler
        /// </summary>
        /// <param name="err"></param>
        /// <returns></returns>
        public static bool ShowError(string err)
        {
            bool res = (ShowErrorMessage != null);
            if (res)
                ShowErrorMessage(err);
            return res;
        }

        /// <summary>
        /// Start Bot Thread
        /// </summary>
        public static void StartBot()
        {
            // Reset process state so bot will pick it up
            ProcessStatus = ProcessStatuses.WOW_STARTING;

            // Start bot thread
            ProcessManager.BotManager.Start();
        }

        /// <summary>
        /// Try to run the WoW process 
        /// </summary>
        public static bool StartWow()
        {

            try
            {
                // Perry style = paranoid ;-)
                string wowPath = Config.WoWInfo.ExePath;

                // Ok, no one configured the path, let's try to find it on our own
                if (string.IsNullOrEmpty(wowPath))
                {
                    wowPath = AppHelper.GetWowInstallationPath();
                }

                if (!string.IsNullOrEmpty(wowPath))
                {
                    Log("char", "Checking WoW version ...");
                    // Get WoW.exe version and convert to form x.x.x.x
                    string version = 
                        FileVersionInfo.GetVersionInfo(wowPath).FileVersion.
                            Replace(",", ".").Replace(" ", ""); ;

                    if (!DataManager.CurWoWVersion.ToString().Equals(version))
                        TerminateOnInternalBug(ProcessManager.Bugs.WRONG_WOW_VERSION, 
                            "Version of loading WoW.exe '" + version +
                                "' is not equal version from config.xml '" + 
                                    DataManager.CurWoWVersion + "'");

                    else
                        Log("char", "Continuing with WoW.exe version '" + version + "'");
                        
                    Log("char", "Starting WoW ...");

                    // Guest account might not be enabled
                    try
                    {
                        // Process startup options
                        if (config.WoWInfo.NoSound)
                            wowPath += " -nosound";
                        if (config.WoWInfo.Windowed)
                            wowPath += " -windowed";

                        process = AppHelper.RunAs(Config.WoWInfo.GuestUsername,
                                            Config.WoWInfo.GuestPassword, null, wowPath);
                    }
                    catch (Win32Exception w32e)
                    {
                        if (WoWProcessFailed != null)
                        {
                            WoWProcessFailed("Unable to start '" + wowPath + 
                              "' with the Guest account (" + w32e.Message + ").\n" + 
                              "Check that the path is correct and the Guest account is enabled." );
                        }

                        return false;

                    }
                    // The process is now being started as suspended. We should actually
                    // create our own IDirect3DDevice and get the pointer to the EndScene function
                    // and then save it, resume wow, do all our stuff and use that pointer when we 
                    // inject the LUA DLL

                    //Inject!!!!
                    ProcessStatus = ProcessStatuses.WOW_INJECTING;
                    Injector.InjectLua(process.Id);

                    // If we're not using autologin we make sure that the LUA hook is off the way until we are logged in
                    // Injector.Lua_RegisterInputHandler();

                    // resume
                    ProcessStatus = ProcessStatuses.WOW_INITIALIZING;
                    ResumeMainWowThread();

                    if (process != null)
                    {
                        AfterProcessStart();
                        Log("char", "WoW started.");
                        return true;
                    }
                    else
                    {
                        Log("char", "Failed start WoW.");
                        return false;
                    }
                }
                else
                {
                    ShowError("Wow is not installed or the registry key is missing.");
                    return false;
                }
            }
            catch (Win32Exception w32e)
            {
                if (WoWProcessFailed != null)
                    WoWProcessFailed(w32e.Message);
                return false;
            }
            catch (Exception ex)
            {
                if (WoWProcessAccessFailed != null)
                    WoWProcessAccessFailed(ex.Message);
                return false;
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
                Player.LastLocation = new Vector3D(0, 0, 0);
            }
        }

        /// <summary>
        /// Update all player informations: hp,mana,xp,position,etc...
        /// </summary>
        public static void UpdatePlayer()
        {
            if (_gstatus != GameStatuses.INITIALIZED)
                return;
            
            if (PlayerUpdate != null)
            {
                PlayerUpdate();
            }

            //update last player location
            Player.LastLocation = Player.Location;

            if (PlayerWayPoint != null)
            {
                Vector3D current = Player.Location;
                WayPoint wpLast = (WayPointManager.Instance.NormalNodeCount > 0) ? 
                    WayPointManager.Instance.NormalPath[WayPointManager.Instance.NormalPath.Count - 1] : null;

                if (wpLast != null && MathFuncs.GetDistance(current, wpLast.Location, false) > 5 || wpLast == null)
                {
                    PlayerWayPoint(current);
                }
            }

        }

        /// <summary>
        /// Check if you are logged into the WoW game
        /// </summary>
        public static bool CheckInGame()
        {
            try
            {
                WowProcess.ReadUInt(WowProcess.ReadUInt(WowProcess.ReadUInt(
                    DataManager.CurWoWVersion.Globals.GameOffset) +
                        DataManager.CurWoWVersion.Globals.PlayerBaseOffset1) +
                            DataManager.CurWoWVersion.Globals.PlayerBaseOffset2);
                // Read successful. Check if we need initialize
                if ((_gstatus != GameStatuses.INITIALIZED) && 
                        (_gstatus != GameStatuses.DISCONNECTED))
                {
                    GameStatus = GameStatuses.ENTERING_WORLD;
                    InitializeBot();
                }

                // Means no exception, we might be actually not in game yet or already initialized
                return true;
            }
            catch(Exception ex)
            {
                if (InGame)
                {
                    Error("char", "CheckInGame() - caugth exception. See error log for details.");
                    Error("errors", "CheckInGame() - ", ex);

                    GameStatus = GameStatuses.DISCONNECTED;
                }

                return false;
                
            }
        }

        /// <summary>
        /// Try to find the Thread Local Storage, aka WoW must be running
        /// </summary>
        public static bool FindTLS()
        {
            try
            {
                // search for the code pattern that we want (in this case, WoW TLS)
                TLS = SPattern.FindPattern(process.Handle, process.MainModule,
      "EB 02 33 C0 8B D 00 00 00 00 64 8B 15 00 00 00 00 8B 34 8A 8B D 00 00 00 00 89 81 00 00 00 00",
                                           "xxxxxx????xxx????xxxxx????xx????");


                if (TLS == uint.MaxValue)
                {
                    Debug("char", "Looking for the TLS returned an invalid value");
                    return false;
                }

                Debug("char", "TLS found");

                return true;

            } catch (Exception)
            {
                Debug("char", "Cannot find the TLS");
                return false;
            }
        }

        private static bool InitializeConnectionManager()
        {
            try
            {
                Globals.ClientConnectionPointer = wowProcess.ReadUInt(TLS + 0x16);
                Globals.ClientConnection = wowProcess.ReadUInt(Globals.ClientConnectionPointer);
                if (Globals.ClientConnection == 0)
                {
                    Debug("char", "ClientConnection not yet available");
                    return false;
                }
                Globals.ClientConnectionOffset = wowProcess.ReadUInt(TLS + 0x1C);
                if (Globals.ClientConnectionOffset == 0)
                {
                    Debug("char", "ClientConnectionOffset not yet available");
                    return false;
                }
                Globals.CurMgr = wowProcess.ReadUInt(Globals.ClientConnection + Globals.ClientConnectionOffset);
                if (Globals.CurMgr == 0)
                {
                    Debug("char", "ConnectionManager not yet available");
                    return false;
                }
                //ObjectManager = new ObjectManager();
                //Player = new WowPlayer(ObjectManager.GetLocalPlayerObject());
                Debug("char", "Found ConnectionManager");
                return true;
            }
            catch(Exception)
            {
                Debug("char", "ConnectionManager not found");
                return false;
            }
        }

        private static void InitializePlayer()
        {
            ObjectManager = new ObjectManager();
            Player = new WowPlayer(ObjectManager.GetLocalPlayerObject());
            Player.SetCurrentMapInfo();
            Player.SetCharInfo();

            if (WoWInGame != null)
                WoWInGame();
        }

        /// <summary>
        /// Search for the TLS and Initialize the bot once the user is logged in
        /// </summary>
        public static void InitializeBot()
        {
            try
            {
                Log("char", "Initializing Character ...");

                while (!InitializeConnectionManager())
                {
                    if (!InWorld)
                        return;

                    Debug("char", "ConnectionManager not found. Keep looking ...");
                    Thread.Sleep(250);
                }
 
                // This might have already been done, but since we could have autologin disabled
                // we do this again (there are no issues if you call this more than once anyway)
                Injector.Lua_RegisterInputHandler();

                InitializePlayer();
                InitializeCaronte();
                //ScriptHost.Start();
                //StateManager.Instance.Stop();

                Log("char", "Character initialized");
                GameStatus = GameStatuses.INITIALIZED;
            }
            catch (Exception ex)
            {
                throw new Exception("Initialize failed! - " + ex.ToString());
            }
        }

        public static void InitializeCaronte()
        {
            //string continent = "Azeroth";  // temporary fix to get things running while debugging LUA
            
            string continent = DataManager.CurWoWVersion.Continents.
                                            FindContinentNameById(Player.ContinentID);
            if (continent == null)
            {
                // Stopping the bot
                BotManager.Stop();
                throw new Exception("Continent not found for id: " + Player.ContinentID);
            }

            Caronte.Init(continent);
            Log("char", string.Format(
                "Caronte initialized with continent '{0}'", continent));

            // Caronte.Init("Azeroth"); 

            // We generate a fake path once to initialize the chunk loader stuff
            //Pather.Graph.Path path = ProcessManager.Caronte.CalculatePath(new Pather.Graph.Location(Player.Location.X, Player.Location.Y, Player.Location.Z), 
            //    new Pather.Graph.Location(Player.Location.X+5, Player.Location.Y+5, Player.Location.Z));
        }

        public static void SuspendMainWowThread()
        {
            ProcessThread wowMainThread = SThread.GetMainThread(process.Id);
            IntPtr hThread = SThread.OpenThread(wowMainThread.Id);
            SThread.SuspendThread(hThread);
        }

        public static void ResumeMainWowThread()
        {
            ProcessThread wowMainThread = SThread.GetMainThread(process.Id);
            IntPtr hThread = SThread.OpenThread(wowMainThread.Id);
            SThread.ResumeThread(hThread);
        }

        /// <summary>
        /// Get AutoRun mode
        /// </summary>
        public static bool AutoRun
        {
            get { return arun; }
        }

        public static void SetAutoRun()
        {
            arun = true;
        }

        /// <summary>
        /// Return true when WoW.exe successfully started & dante injected
        /// </summary>
        public static bool ProcessRunning
        {
            get { return _pstatus == ProcessStatuses.WOW_RUNNING; }
        }
        
        
        /// <summary>
        /// Reset game status after disconnect or exception
        /// </summary>
        public static void ResetGameStatus()
        {
            GameStatus = GameStatuses.DISCONNECTED;
        }

        public static void TerminateOnInternalBug(Bugs bug_id, string err)
        {
            ShowError("Internal bug " + Enum.GetName(typeof(Bugs), bug_id) + 
                ". " + err + ". Terminating application");
            Environment.Exit((int) bug_id);
        }

        public static void SetGameIdle(int sleep_time)
        {
            GameStatus = GameStatuses.IDLE;
            Thread.Sleep(sleep_time);
        }

        #endregion

        #region XML

        public static Talents ReadTalents(string fname)
        {
            XmlSerializer s = new XmlSerializer(typeof(Talents));
            TextReader r = new StreamReader(fname);
            Talents talents = (Talents)s.Deserialize(r);
            talents.FullPath = fname;
            r.Close();

            return talents;
        }

        /// <summary>
        /// Load Application config file
        /// </summary>
        public static void LoadConfig()
        {
            var serializer = new Serializer<Config>();
            
            try
            {
                config = serializer.Load(ConfigFileName);

                // Check version of config file
                if (ProcessManager.ConfigVersion != config.Version)
                    throw new ConfigFileChangedException();


                // Decrypt auto-login password
                if (!string.IsNullOrEmpty(config.Account.LoginPassword))
                {
                    try
                    {
                        config.Account.DecryptPassword(
                                config.Account.LoginPassword);
                    }
                    catch (Exception)
                    {
                        // We couldn't decrypt the password for some reason. We reset it to blank.
                        config.Account.LoginPassword = "";
                    }
                }

                OnConfigurationChanged();
            }
            catch (FileNotFoundException)
            {
                // Show App configuration window for the first time run
                if (FirstTimeRun != null)
                    FirstTimeRun();
                else
                    TerminateOnInternalBug(Bugs.CONFIG_NOT_FOUND,
                        "Config file config.xml not found");
            }
            catch (ConfigFileChangedException)
            {
                if (ConfigFileChanged != null)
                    ConfigFileChanged();
                else
                    TerminateOnInternalBug(Bugs.WRONG_CONFIG_VERSION,
                        "Config file config.xml has version " +
                        config.Version + " that different from application version " +
                        ProcessManager.ConfigVersion);
            }
            catch (WoWDataNotFoundException ex)
            {
                TerminateOnInternalBug(Bugs.WOWDATA_NOT_FOUND, ex.Message);
            }
            catch (Exception e)
            {
                TerminateOnInternalBug(Bugs.ERROR_LOAD_CONFIG, 
                            "Unable load config.xml : " + e.Message);
            }
        }

        /// <summary>
        /// Save application config file
        /// </summary>
        public static void SaveConfig()
        {
            /*
             XmlWriterSettings settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true; // Remove the <?xml version="1.0" encoding="utf-8"?>
             */

            var serializer = new Serializer<Config>();

            try
            {
                // Remember current config version
                config.Version = ConfigVersion;
                XmlManager.Save(ConfigFileName, typeof(Config), config);
                // serializer.Save(ConfigFileName, config);
                OnConfigurationChanged();

            }
            catch (Exception ex)
            {
                ShowError("Failed save configuration file. " + 
                    ex.Message + ". " + ex.InnerException);
            }
        }

        #endregion

        #region Event Processing

        private static void OnConfigurationChanged()
        {
            // Check  mandatory directories
            string[] dirs = {config.LogParams.Dir,
                config.ProfilesDir + System.IO.Path.DirectorySeparatorChar + "Accounts",
                config.ProfilesDir + System.IO.Path.DirectorySeparatorChar + "Characters" };

            foreach (string dir in dirs)
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

            DataManager.SetWowVersion(config.WoWInfo.Version);
        }

        private static void OnUpdateAppStatus(string new_status)
        {
            if (UpdateAppStatus != null)
                UpdateAppStatus(new_status);
        }

        private static void OnUpdateGameStatus(string new_status)
        {
            if (UpdateGameStatus != null)
                UpdateGameStatus(new_status);
        }

        public static void OnUpdateBotStatus(string new_status, string queue)
        {
            if (UpdateBotStatus != null)
                UpdateBotStatus(new_status, queue);
        }

        public static void OnBotProgressStart(int max, string tooltip)
        {
            if (BotProgressStart != null)
                BotProgressStart(max, tooltip);
        }

        public static void OnBotProgressChange(int value)
        {
            if (BotProgressChange != null)
                BotProgressChange(value);
        }

        public static void OnBotProgressEnd()
        {
            if (BotProgressEnd != null)
                BotProgressEnd();
        }

        public static void StopStateMachine(object sm, EventArgs arg)
        {
            Player.StateMachine.IsRunning = false;
        }

        public static void OnPathCalculated(Vector3D[] v_arr, int retry)
        {
            if (DisplayPathCalculated != null)
                DisplayPathCalculated(v_arr, retry);
        }

        public static void OnTravelCompleted()
        {
            if (TravelCompleted != null)
                TravelCompleted();
        }

        #endregion

    }
}