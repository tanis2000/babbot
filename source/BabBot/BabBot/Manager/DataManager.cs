using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml.Serialization;
using BabBot.Data;
using BabBot.Common;
using BabBot.Wow;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
// TODO
// Replace with reflection adding NPC services
namespace BabBot.Manager
{
    #region Exceptions

    public class ZoneNotFoundException : Exception
    {
        public ZoneNotFoundException(string zone_name)
            : base("Zone " + zone_name +
                " not found in continent list. Please configure WoWData.xml") { }
    }

    public class UnknownNpcServiceException : Exception
    {
        public UnknownNpcServiceException(string service)
            : base("New NPC Service " + service +
                " found in GameObjectData.xml but is not defined internally.") { }
    }

    public class DataSynchException : Exception
    {
        public DataSynchException()
            : base("Data between xml source and screen forms non-synchronized." + 
                " Restart bot and try again.") { }
    }

        public class ServiceNotFountException : Exception
    {
        public ServiceNotFountException(string service)
            : base("In toon's area no NPC found for service: " + service) {}
    }

    #endregion

    #region Xml

    public class XmlManager
    {

        /// <summary>
        /// Directory with data files
        /// </summary>
        private static string DataDir = "Data" + Path.DirectorySeparatorChar;

        // Debug
        internal static string DataDirEx =
#if DEBUG
            "..\\..\\" +
#endif
            DataDir;


        /// <summary>
        /// Export Data Directory
        /// </summary>
        public static string ExportDataDir = DataDir + "Export";

        /// <summary>
        /// Export Data Directory with DirectorySeparatorChar attached
        /// </summary>
        public static string ExportDataDirFull = DataDir + "Export" + Path.DirectorySeparatorChar;

        /// <summary>
        /// Current version of GameObjectData and RouteListVersion
        /// </summary>
        internal static readonly int[] Versions = new int[] { 0, 0 };

        /// <summary>
        /// List of datafiles has to be loaded on application startup
        /// </summary>
        internal static string[] DataFiles = new string[2] {DataDirEx + "GameObjectData.xml", 
            DataDirEx + "RouteList.xml" };


        internal static void Init()
        {
            // data first
            int[] versions = new int[2];

            DataManager.wdata = (WoWData) LoadInitData("Data\\WoWData.xml", typeof(WoWData));

            DataManager.gdata = (GameObjectData) LoadInitData(
                        DataManager.GameObjDataFileName, typeof(GameObjectData));
            RouteListManager.rdata = (RouteList)LoadInitData(
                        RouteListManager.RouteListFileName, typeof(RouteList));

            versions[0] = DataManager.gdata.Version;
            versions[1] = RouteListManager.rdata.Version;

            // Check if GameObjData and RouteList versions the same
            for (int i = 0; i < versions.Length; i++)
            {
                if (versions[i] != Versions[i])
                {
                    // TODO Migrate data from old format to new and save
                    // Show message for now
                    ShowErrorMessage(DataFiles[i] + " is in old format. It has version " +
                        versions[i] + " that different from supported " + Versions[i]);
                }
            }
        }

        internal static void AfterInit()
        {
            // Attach GameObj data and Route List to selected WoW version
            DataManager.wversion.GameObjData = DataManager.gdata.
                                FindVersion(DataManager.wversion.Build);
            DataManager.wversion.Routes = RouteListManager.rdata.
                                FindVersion(DataManager.wversion.Build);

            // Index Game Object data for future use
            DataManager.IndexData();

            // Index Routes
            RouteListManager.IndexData();

            // Reset changed flag
            DataManager.ResetChanged();
        }

        internal static void Merge(string wow_version)
        {
            // Auto Merge data from earlier version with latest one
            WoWVersion wprev = (WoWVersion)DataManager.wdata[0];
            int i = 1;
            while ((i < DataManager.wdata.Count) &&
                !wprev.Build.Equals(wow_version))
            {
                WoWVersion wnew = (WoWVersion)DataManager.wdata[i];

                // Merge WoW data
                wnew.MergeWith(wprev);

                // Merge GameObj and RouteList. Ordering is not an issues since it defined in 
                // WoWData already
                // Ignore all exceptions
                try
                {
                    DataManager.gdata.FindVersion(wnew.Build).MergeWith(
                                    DataManager.gdata.FindVersion(wprev.Build));
                    RouteListManager.rdata.FindVersion(wnew.Build).MergeWith(
                                    RouteListManager.rdata.FindVersion(wprev.Build));
                }
                catch { }

                wprev = wnew;
                i++;
            }
        }

        internal static void Clear()
        {
            DataManager.gdata = null;
            DataManager.wdata = null;
            RouteListManager.rdata = null;
        }

        private static object LoadInitData(string fname, Type t)
        {
            object res = null;
            try
            {
                res = Load(fname, t);

            }
            catch (Exception e)
            {
                Output.Instance.LogError(e);
                ProcessManager.TerminateOnInternalBug(
                    ProcessManager.Bugs.XML_ERROR, "Unable load " + fname + 
                    " file. See error log for details");
            }

            return res;
        }

        private static object Load(string fname, Type t)
        {
            object res = null;
            TextReader r = null;
            XmlSerializer s = new XmlSerializer(t);

            try
            {
                r = new StreamReader(fname);
                res = s.Deserialize(r);
            }
            catch (Exception e)
            {
                ProcessManager.TerminateOnInternalBug(ProcessManager.Bugs.XML_ERROR,
                    "Unable load " + fname + " : " +
                    e.Message + Environment.NewLine + e.InnerException);
            }
            finally
            {
                r.Close();
            }

            return res;
        }

        public static bool SaveDataFile(string lfs, bool export, int idx, Type T, object data)
        {
            string fname = XmlManager.DataFiles[idx];

            // Backup old GameObj data before save
            string bf = Path.GetDirectoryName(fname) +
                Path.DirectorySeparatorChar +
                Path.GetFileNameWithoutExtension(fname) + ".bak";

            if (!string.IsNullOrEmpty(lfs))
                Output.Instance.Log(lfs, "Saving " + fname +
                    " before serializing to " + bf);

            try
            {
                File.Copy(fname, bf, true);
            }
            catch (Exception e)
            {
                ShowErrorMessage("Failed update NPC Data. Unable copy file " + fname +
                            "  to " + bf + ". " + e.Message);

                return false;
            }

            if (!Save(fname, T, data))
            {
                if (!string.IsNullOrEmpty(lfs))
                    Output.Instance.Log(lfs, "Recovering " + fname +
                        " after error from " + bf);
                File.Copy(bf, XmlManager.DataFiles[idx]);
                return false;
            }

            if (!string.IsNullOrEmpty(lfs))
                Output.Instance.Log(lfs, "File " + fname + " successfully saved.");

            // Index GameObject Data
            if (idx == 0)
                DataManager.IndexData();
            else if (idx == 1)
                RouteListManager.IndexData();

            // Check all list and generate chanded data for export

            if (export)
                foreach (GameObject obj in DataManager.CurWoWVersion.GameObjData.Values)
                    if (obj.Changed)
                        DataManager.ExportGameObj(obj, lfs);

            // Reset Changed flag
            if (idx == 0)
                DataManager.ResetChanged();

            return true;
        }

        /// <summary>
        /// Serialize object in xml format and write XML document to external file
        /// </summary>
        /// <param name="fname">Output File Name</param>
        /// <param name="t">Type of object</param>
        /// <param name="obj">Object itself</param>
        public static bool Save(string fname, Type t, object obj)
        {
            bool res = false;
            TextWriter w = null;
            try
            {
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("", ""); // Remove  xmlns: parameters

                XmlSerializer s = new XmlSerializer(t);
                w = new StreamWriter(fname);

                s.Serialize(w, obj, ns);
                res = true;
            }
            catch (Exception e)
            {
                ShowErrorMessage("Failed save " + fname + ". " +
                            e.Message + ". " + e.InnerException);
            }
            finally
            {
                if (w != null)
                    w.Close();
            }

            return res;
        }

        private static void ShowErrorMessage(string err)
        {
            ProcessManager.ShowError(err);
        }
    }

    #endregion

    #region Game Data

    public class DataManager
    {
        /// <summary>
        /// Reference the whole WoWData
        /// </summary>
        internal static WoWData wdata;
        
        /// <summary>
        /// Reference the whole GameObject Data
        /// </summary>
        internal static GameObjectData gdata;

        /// <summary>
        /// Reference the current WoW Version from WoWData
        /// </summary>
        internal static WoWVersion wversion;


        /// <summary>
        /// Supported version of GameObject
        /// </summary>
        private static readonly int  GameObjectVersion = XmlManager.Versions[0];

        /// <summary>
        /// Gui mode
        /// </summary>
        private static bool _gui = false;

        /// <summary>
        /// Initialization flag. 
        /// Set to true after internal dataset populated with GameObject data
        /// </summary>
        private static bool _populated = false;

        /// <summary>
        /// Current WoW version loaded
        /// </summary>
        public static WoWVersion CurWoWVersion
        {
            get { return wversion; }
        }

        /// <summary>
        /// Game Objects related to current WoW version
        /// </summary>
        public static GameObjectData GameObjList
        {
            get { return gdata; }
        }

        /// <summary>
        /// GameObjectData xml file name
        /// </summary>
        public static string GameObjDataFileName = XmlManager.DataFiles[0];

        /// <summary>
        /// Application configuration tag from WoWData.xml
        /// </summary>
        internal static AppConfig AppConfig
        {
            get { return wdata.AppConfig; }
        }

        /// <summary>
        /// Global WoW offsets related to current WoW version
        /// Loaded from WoWData.xml
        /// </summary>
        internal static GlobalOffsets Globals
        {
            get { return CurWoWVersion.Globals; }
        }

        /// <summary>
        /// List of all supported WoW versions
        /// </summary>
        internal static WoWVersion[] WoWVersions
        {
            get { return wdata.Versions; }
        }

        /// <summary>
        /// Sequence of Quest Items in Quest local items array
        /// </summary>
        internal static QuestItemTypes[] QuestItemSeq
        {
            get
            {
                return new QuestItemTypes[] {
                QuestItemTypes.REQUIRED,
                QuestItemTypes.REWARD,
                QuestItemTypes.CHOICE };
            }
        }

        /// <summary>
        /// Quest indexed list
        /// </summary>
        internal static SortedDictionary<int, Quest> QuestList =
                        new SortedDictionary<int, Quest>();

        /// <summary>
        /// Zone sorted list with npc servcies as:
        /// taxi, inn, vendor
        /// </summary>
        internal static SortedDictionary<string, ZoneServices> ZoneList =
                new SortedDictionary<string, ZoneServices>();

        // This variable is for binding
        internal static Quest[] QuestDataSource
        {
            get
            {
                Quest[] ret = new Quest[QuestList.Count];
                QuestList.Values.CopyTo(ret, 0);
                return ret;
            }
        }

        #region Bot DataSet

        /// <summary>
        /// BotDataSet filled with data
        /// </summary>
        public static BotDataSet GameData { get; private set; }

        #region Nested: Enumerates

        /// <summary>
        /// List of services provided by NPC 
        /// </summary>
        public enum ServiceTypes : byte
        {
            BINDER = 0,
            BANKER = 1,
            BATTLEMASTER = 2,
            CLASS_TRAINER = 3,
            TAXI = 4,
            TRADE_SKILL_TRAINER = 5,
            VENDOR_REGULAR = 6,
            VENDOR_GROSSERY = 7,
            VENDOR_REPAIR = 8,
            WEP_SKILL_TRAINER = 9,
        }

        /// <summary>
        /// Game object types that can own quest
        /// </summary>
        public enum GameObjectTypes : byte
        {
            ITEM = 0,
            NPC = 1
        }

        /// <summary>
        /// Enumerates for Quest Item Types
        /// </summary>
        public enum QuestItemTypes
        {
            REQUIRED = 0,
            REWARD = 1,
            CHOICE = 2,
            OBJECTIVES = 3
        }

        #endregion

        #endregion

        #region Routes

        /// <summary>
        /// Sorted list with existing set of EndpointTypes
        /// </summary>
        public static SortedDictionary<string, EndpointTypes> EndpointsSet =
                                new SortedDictionary<string, EndpointTypes>();

        #endregion

        /// <summary>
        /// Data Manager class
        /// </summary>
        static DataManager()
        {
            foreach (EndpointTypes ept in Enum.GetValues(typeof(EndpointTypes)))
                EndpointsSet.Add(Enum.GetName(typeof(EndpointTypes), 
                                                    ept).ToLower(), ept);

            // TEST
            /*
            FileStream fs = new FileStream("test.bin", FileMode.Create);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(fs, ServiceTypeTable);
            fs.Close();
             */
        }

        /// <summary>
        /// Initialize all DataSet tables required for binding on GUI forms
        /// When this method calls means bot running in GUI mode
        /// </summary>
        public static void Initialize()
        {
            // We running gui
            _gui = true;

            GameData = new BotDataSet();

            // Add game object types
            foreach (GameObjectTypes id in Enum.GetValues(typeof(GameObjectTypes)))
                GameData.GameObjectTypes.AddGameObjectTypesRow((int) id, Enum.GetName(
                                            typeof(GameObjectTypes), id).ToLower());

            // Add service types
            foreach (ServiceTypes id in Enum.GetValues(typeof(ServiceTypes)))
                GameData.ServiceTypes.AddServiceTypesRow((int)id, Enum.GetName(
                                            typeof(ServiceTypes), id).ToLower());

            // Add Quest Item Types
            foreach (QuestItemTypes id in Enum.GetValues(typeof(QuestItemTypes)))
                GameData.QuestItemType.AddQuestItemTypeRow((int)id, Enum.GetName(
                                            typeof(QuestItemTypes), id).ToLower());

            // Add coordinate types
            foreach (VectorTypes id in Enum.GetValues(typeof(VectorTypes)))
                GameData.CoordTypes.AddCoordTypesRow((int)id, Enum.GetName(
                                            typeof(VectorTypes), id).ToLower());

            // Create other empty tables
            GameData.AcceptChanges();

            // Populate DataSet with data
            PopulateDataSet();
        }

        private static void ShowErrorMessage(string err)
        {
            ProcessManager.ShowError(err);
        }
        
        internal static WoWVersion FindWoWVersionByName(string version)
        {
            return wdata[version];
        }

        public static void SetWowVersion(string version)
        {
            wversion = FindWoWVersionByName(version);
            if (wversion == null)
                throw new WoWDataNotFoundException(version);
        }

        public static Quest FindQuestById(int id)
        {
            // Possible exception
            try
            {
                return QuestList[id];
            }
            catch
            {
                return null;
            }
        }

        public static void IndexData()
        {
            ZoneList.Clear();
            QuestList.Clear();

            foreach (Continent cid in CurWoWVersion.Continents.ContinentList)
                    foreach (Zone z in cid.List)
                        ZoneList.Add(z.Name, new ZoneServices(cid.Id));

            foreach (GameObject g_obj in CurWoWVersion.GameObjData.Values)
            {
                if (!string.IsNullOrEmpty(g_obj.ZoneText) && 
                                            g_obj.GetType().Equals(typeof(NPC)))
                {
                    NPC npc = (NPC)g_obj;

                    if (ZoneList.ContainsKey(g_obj.ZoneText))
                        ZoneList[g_obj.ZoneText].AddService(npc);
                    else
                        throw new Exception("Zone '" + g_obj.ZoneText + 
                                            "' not defined in WoWData.xml");
                }
                
                // Index quest
                Quests ql = g_obj.QuestList;
                if (ql != null)
                {
                    foreach (Quest q in ql.Values)
                    {
                        q.Src = g_obj;
                        if (!string.IsNullOrEmpty(q.DestName))
                            q.Dest = CurWoWVersion.GameObjData[q.DestName];
                        QuestList.Add(q.Id, q);
                    }
                }

                // Fill dataset if we running in GUI mode
                PopulateDataSet();
            }
        }

        private static void PopulateDataSet()
        {
            if (!_gui || _populated)
                return;

            // Should invoke cascading deleting but raise FK exception
            // GameData.GameObjects.Clear();
            // GameData.ContinentList.Clear();

            foreach (Continent c in CurWoWVersion.Continents.Values)
            {
                GameData.ContinentList.Rows.Add(c.Id, c.Name);
                foreach (Zone z in c.List)
                    GameData.ZoneList.AddZoneListRow(z.Name, c.Id);
            }

            foreach (GameObject g in CurWoWVersion.GameObjData.Values)
                AddGameObject(g);

            GameData.AcceptChanges();
            _populated = true;
        }

        /// <summary>
        /// Add new game object to internal dataset
        /// </summary>
        /// <param name="g"></param>
        /// <returns></returns>
        public static BotDataSet.GameObjectsRow AddGameObject(GameObject g)
        {
            BotDataSet.ZoneListRow z = GameData.ZoneList.FindByNAME(g.ZoneText);

            if (z == null)
                throw new ZoneNotFoundException(g.ZoneText);

            string faction = "";
            if (g.ObjType == GameObjectTypes.NPC)
                faction = ((NPC) g).Faction;

            BotDataSet.GameObjectsRow obj_row = GameData.
                        GameObjects.AddGameObjectsRow(
                            GameData.GameObjectTypes.FindByID((int)
                                    g.ObjType), g.Name, z, faction);

            // Add base coordinates
            BotDataSet.CoordinatesZoneRow cz_row = GameData.CoordinatesZone.
                AddCoordinatesZoneRow(obj_row, g.ZoneText);

            GameData.Coordinates.AddCoordinatesRow(cz_row, 
                                g.X, g.Y, g.Z, g.BasePosition.Type);

            // Add quests
            foreach (Quest q in g.QuestList.Values)
            {
                BotDataSet.QuestListRow qrow = GameData.QuestList.
                    AddQuestListRow(q.Id, obj_row, q.Title,
                    q.GreetingText, q.ObjectivesText, g.Name, 
                    q.DestName, q.Level, q.Link, q.BonusSpell, 
                    q.Objectives.ObjList.Count);

                // Add quest items
                for (int i = 0; i < q.QuestItems.Length; i++)
                {
                    if (q.QuestItems[i] == null)
                        continue;

                    for (int j = 0; j < q.QuestItems[i].Count; j++)
                    {
                        CommonQty qi = q.QuestItems[i][j];
                        // Locate type
                        GameData.QuestItems.AddQuestItemsRow(qrow, j,
                            (int)DataManager.QuestItemSeq[i],qi.Name, qi.Qty, qi.Name);
                    }
                }

                if (q.Objectives.ObjList != null)
                {
                    // Add quest objectives
                    for (int i = 0; i < q.Objectives.ObjList.Count; i++)
                    {
                        AbstractQuestObjective qobj = q.Objectives.ObjList[i];

                        int qty = 0;
                        if (qobj.HasQty)
                            qty = ((AbstractQtyQuestObjective)qobj).ReqQty;

                        GameData.QuestItems.AddQuestItemsRow(qrow, i,
                                (int)DataManager.QuestItemTypes.OBJECTIVES,
                                qobj.Name, qty, qobj.FullName);
                    }
                }
            }


            if (g.ObjType == GameObjectTypes.NPC)
            {
                NPC npc = (NPC)g;

                // Add other coordinates
                foreach (ZoneWp coord in npc.Coordinates.Values)
                {
                    // Check if row exists
                    DataRow[] cz_check = GameData.CoordinatesZone.
                            Select("ZONE_NAME='" + coord.Name + "' AND GID=" + obj_row.ID);
                    if (cz_check.Length > 0)
                        cz_row = (BotDataSet.CoordinatesZoneRow) cz_check[0];
                    else
                        cz_row = GameData.CoordinatesZone.
                            AddCoordinatesZoneRow(obj_row, coord.Name);

                    foreach (Vector3D v in coord.List)
                        GameData.Coordinates.AddCoordinatesRow(cz_row, 
                                                    v.X, v.Y, v.Z, v.Type);
                }

                // Add Services
                foreach (NPCService srv in npc.Services.Values)
                {
                    // Locate service
                    BotDataSet.ServiceTypesRow srv_row =
                        GameData.ServiceTypes.FindByID((int)srv.SrvType);

                    GameData.NpcServices.AddNpcServicesRow(obj_row, srv_row, srv_row.NAME, srv.Descr);
                }
            }

            return obj_row;
        }

        /// <summary>
        /// Reset Changed flag on the whole NPC database
        /// </summary>
        internal static void ResetChanged()
        {
            foreach (GameObject obj in CurWoWVersion.GameObjData.Values)
                obj.Changed = false;
        }

        public static void DeleteGameObjRow(string name)
        {
            CurWoWVersion.GameObjData.Remove(name);
        }

        /// <summary>
        /// Add new GameObject from DataSet information
        /// </summary>
        /// <param name="row">row from GameObject table</param>
        public static void AddGameObjectRow(BotDataSet.GameObjectsRow row)
        {
            CurWoWVersion.GameObjData.Add(MakeGameObj(row));
        }

        public static void SaveGameObjRow(BotDataSet.GameObjectsRow row)
        {
            // Remember name
            string name = row.NAME;

            // Delete Game Object data
            if (!CurWoWVersion.GameObjData.ContainsKey(name))
                throw new DataSynchException();

            
            // Make new Game Object from dataset
            GameObject obj = MakeGameObj(row);

            DeleteGameObjRow(name);
            CurWoWVersion.GameObjData.Add(obj);
        }

        private static GameObject MakeGameObj(BotDataSet.GameObjectsRow row)
        {
            // Pull coordinates
            Dictionary<string, DataRow[]> zone_wp = null;
            Vector3D vfirst = null;

            DataRow[] zrows = DataManager.GameData.CoordinatesZone.Select("GID=" + row.ID);
            foreach (BotDataSet.CoordinatesZoneRow zrow in zrows)
            {
                DataRow[] coords = DataManager.GameData.Coordinates.Select("ZONE_ID=" + zrow.ID);
                if (coords == null)
                    continue;

                if (vfirst == null)
                {
                    BotDataSet.CoordinatesRow first_coord = (BotDataSet.CoordinatesRow)coords[0];
                    vfirst = new Vector3D((float)first_coord.X ,
                           (float)first_coord.Y, (float)first_coord.Z);

                    zone_wp = new Dictionary<string, DataRow[]>();
                }

                zone_wp.Add(zrow.ZONE_NAME, coords);
            }

            if (vfirst == null)
                throw new Exception("Game object required at least one base coordinates");

            
            GameObject obj = null;

            switch(row.TYPE_ID)
            {
                case (int)GameObjectTypes.ITEM :
                    obj = new GameObject(row.NAME, row.ZONE_NAME, vfirst);
                    break;

                case (int)GameObjectTypes.NPC:
                    obj = new NPC(row.NAME, row.ZONE_NAME, vfirst, row.FACTION);
                    break;

                default:
                    throw new Exception("Unknown Game Object Type :" + row.TYPE_ID);
            }

            // Pull quest list with items
            DataRow[] qrows = GameData.QuestList.Select("GID=" + row.ID);
            foreach (BotDataSet.QuestListRow qrow in qrows)
            {
                Quest q = new Quest(qrow.ID, qrow.TITLE, qrow.GREETING_TEXT,
                        qrow.OBJECTIVES_TEXT, qrow.LEVEL, qrow.BONUS_SPELL, qrow.LINK);

                for (int i = 0; i < DataManager.QuestItemSeq.Length; i++)
                {
                    QuestItemTypes qi = DataManager.QuestItemSeq[i];

                    DataRow[] item_rows = GameData.QuestItems.
                        Select("QID=" + qrow.ID + " AND ITEM_TYPE_ID=" + (int)qi);
                    foreach (BotDataSet.QuestItemsRow item_row in item_rows)
                    {
                        // Quest item by default null
                        if (q.QuestItems[i] == null)
                            q.QuestItems[i] = new QuestItem();
                        q.QuestItems[i].Add(new CommonQty(item_row.NAME, item_row.QTY));
                    }
                }

                DataRow[] rows_objective = GameData.QuestItems.Select("QID=" + qrow.ID +
                    " AND ITEM_TYPE_ID=" + (int)QuestItemTypes.OBJECTIVES);
            }

            if (row.TYPE_ID == (int) (int)GameObjectTypes.NPC)
            {
                NPC npc = (NPC) obj;

                npc.Faction = row.FACTION;

                // NPC can have extra coordinates and services
                // Add other coordinates
                if (zone_wp != null)
                {
                    foreach (KeyValuePair<string, DataRow[]> item in zone_wp)
                    {
                        ZoneWp zone = new ZoneWp(item.Key);
                        foreach (BotDataSet.CoordinatesRow crow in item.Value)
                        {
                            Vector3D v = new Vector3D((float)crow.X, 
                                        (float)crow.Y, (float)crow.Z);
                            if (!v.Equals(vfirst)) 
                                zone.Add(v);
                        }

                        if (zone.Count > 0)
                            npc.Coordinates.Add(zone);
                    }
                }

                // Extra services
                DataRow[] srows = GameData.NpcServices.Select("GID=" + row.ID);
                foreach (BotDataSet.NpcServicesRow srow in srows)
                {
                    // TODO Replace with reflection
                    NPCService srv = null;

                    switch (srow.SERVICE_NAME)
                    {
                        case "inn":
                        case "taxi":
                            srv = new ZoneNpcService(srow.SERVICE_NAME, srow.DESCR);
                            break;

                        case "banker":
                        case "battlemaster":
                            srv = new NPCService(srow.SERVICE_NAME);
                            break;

                        case "class_trainer":
                            srv = new ClassTrainingService(srow.DESCR.ToUpper());
                            break;

                        case "trade_skill_trainer":
                            srv = new TradeSkillTrainingService(srow.DESCR);
                            break;

                        case "vendor_regular":
                        case "vendor_grossery":
                        case "vendor_repair":
                            bool has_grossery = srow.SERVICE_NAME.Equals("vendor_grossery");
                            srv = new VendorService(srow.SERVICE_NAME.
                                Equals("vendor_repair"), has_grossery, has_grossery);
                            break;

                        case "wep_skill_trainer":
                            srv = new WepSkillService(srow.DESCR);
                            break;

                        default:
                            throw new ServiceNotFountException(srow.SERVICE_NAME);
                    }

                    npc.Services.Add(srv);
                }
            }


            return obj;
        }

        #region Load/Save single Game Object

        public static GameObject LoadGameObj(string fname)
        {
            Serializer<GameObject> s = new Serializer<GameObject>();
            return s.Load(fname);
        }

        public static bool MergeGameObjData(List<GameObject> list, bool export, string lfs)
        {
            bool f = false;
            GameObject check = null;
            foreach (GameObject obj in list)
            {
                // Check if GameObject already exists

                try
                {
                    // If null it produces exception
                    check = DataManager.CurWoWVersion.
                        GameObjData[obj.Name];
                }
                catch { }

                if ((check != null))
                {
                    if (obj.Equals(check))
                    {
                        Output.Instance.LogError(lfs, "GameObject '" + obj.Name +
                            "' already added with identicall parameters");
                        continue;
                    }
                    else
                    {
                        // NPC in database but with different parameters
                        Output.Instance.Debug(lfs, "'" + obj.Name +
                            "' data merged with currently configured");
                        check.MergeWith(obj);

                        f = true;
                    }
                }
                else
                {
                    Output.Instance.Debug(lfs, "Adding new GameObject '" + obj.Name +
                            "' into " + GameObjDataFileName);

                    obj.Changed = true;
                    DataManager.CurWoWVersion.GameObjData.Add(obj);

                    f = true;
                }

            }

            return (!f || (f && (DataManager.SaveGameObjData(lfs, export))));
        }

        /// <summary>
        /// Save All Game Object data in xml format
        /// </summary>
        /// <param name="lfs">Name of logging facility</param>
        /// <returns>True if data successfully saved and False if not</returns>
        public static bool SaveGameObjData(string lfs)
        {
            return SaveGameObjData(lfs, true);
        }

        /// <summary>
        /// Save All Game Object data in xml format
        /// </summary>
        /// <param name="lfs">Name of logging facility</param>
        /// <param name="export">Is export required for changed object</param>
        /// <returns>True if data successfully saved and False if not</returns>
        public static bool SaveGameObjData(string lfs, bool export)
        {
            return XmlManager.SaveDataFile(lfs, export, 0, typeof(GameObjectData), gdata);
        }

        

        /// <summary>
        /// Serialize the given NPC in Export directory
        /// </summary>
        /// <param name="npc"></param>
        public static string ExportGameObj(GameObject obj, string lfs)
        {
            string name = obj.Name;
            string fname = XmlManager.ExportDataDir + Path.DirectorySeparatorChar + name + ".obj";
            try
            {
                Output.Instance.Log(lfs, "Exporting Game Object: " + name +
                                                " to " + XmlManager.ExportDataDir + " subdirectory");

                obj.DoBeforeExport(GameObjectVersion.ToString());
                bool res = XmlManager.Save(fname, typeof(GameObject), obj);
                obj.DoAfterExport();

                if (res)
                    Output.Instance.Log(lfs, 
                        "Export successfull!!! Don't forget upload updated data to " +
                                        "BabBot forum https://sourceforge.net/apps/phpbb/babbot/");
            }
            catch (Exception e)
            {
                ShowErrorMessage("Unable generate export data. " + e.Message);
                return null;
            }

            return fname;
        }


        #endregion
    }

    public class ZoneServices
    {
        public readonly int ContinentId;

        /// <summary>
        /// List of all local (zone) services
        /// </summary>
        public readonly Dictionary<string, List<NPC>> LocalServices =
                                new Dictionary<string, List<NPC>>();
        public readonly List<NPC> TaxiServices;
        public readonly List<NPC> InnServices;
        public readonly List<NPC> RepairServices;
        public readonly List<NPC> GrosseryServices;
        public readonly List<NPC> VendorServices;

        public ZoneServices(int cid)
        {
            ContinentId = cid;

            TaxiServices = new List<NPC>();
            InnServices = new List<NPC>();

            RepairServices = new List<NPC>();
            GrosseryServices = new List<NPC>();
            VendorServices = new List<NPC>();

            LocalServices.Add(Enum.GetName(typeof(DataManager.ServiceTypes), 
                DataManager.ServiceTypes.TAXI).ToLower(), TaxiServices);
            LocalServices.Add(Enum.GetName(typeof(DataManager.ServiceTypes),
                DataManager.ServiceTypes.BINDER).ToLower(), InnServices);
            LocalServices.Add(Enum.GetName(typeof(DataManager.ServiceTypes),
                 DataManager.ServiceTypes.VENDOR_REPAIR).ToLower(), RepairServices);
            LocalServices.Add(Enum.GetName(typeof(DataManager.ServiceTypes),
                DataManager.ServiceTypes.VENDOR_GROSSERY).ToLower(), GrosseryServices);
            LocalServices.Add(Enum.GetName(typeof(DataManager.ServiceTypes),
                DataManager.ServiceTypes.VENDOR_REGULAR).ToLower(), VendorServices);
        }

        public void AddService(NPC npc)
        {
            if (npc.HasInn)
                InnServices.Add(npc);
            
            if (npc.HasTaxi)
                TaxiServices.Add(npc);
                
            if (npc.IsVendor)
            {
                VendorServices.Add(npc);
                VendorService vs = (VendorService)npc.Services["vendor"];
                
                // Check for other services
                if (vs.CanRepair)
                    RepairServices.Add(npc);
                else if (vs.HasFood)
                    GrosseryServices.Add(npc);
            }
        }

    }

    #endregion

    #region Routes

    public class WaypointsNotFoundException : Exception
    {
        public WaypointsNotFoundException(string fname)
            : base("File with waypoints '" + fname + " not found.") {}
    }

    public static class RouteListManager
    {
        /// <summary>
        /// Supported version of RouteList
        /// </summary>
        private static readonly int RouteListVersion = XmlManager.Versions[1];

        /// <summary>
        /// Directory with waypoints
        /// </summary>
        public static string RoutesDirFull = XmlManager.DataDirEx + "Routes" + Path.DirectorySeparatorChar;

        /// <summary>
        /// Reference on all Routes
        /// </summary>
        public static RouteList rdata;

        /// <summary>
        /// Waypoints xml file name
        /// </summary>
        public static string RouteListFileName = XmlManager.DataFiles[1];

        /// <summary>
        /// Indexed table of all undef endpoints A and endpoints B for reversible routes
        /// Used to quickly locate waypoints available from current one
        /// Include only defined endpoints
        /// </summary>
        public static Dictionary<string, List<Route>> Endpoints = 
                                new Dictionary<string, List<Route>>();

        /// <summary>
        /// Indexed table with available routes. 
        /// Include only route that has defined points A and B
        /// </summary>
        public static Dictionary<string, List<Route>> Routes =
                                new Dictionary<string, List<Route>>();

        /// <summary>
        /// Similar to Endpoints but include actual vector length as a key
        /// Include all waypoints (include undefined)
        /// </summary>
        public static SortedList<double, List<Route>> Waypoints =
                                new SortedList<double, List<Route>>(new Vector3DComparer());

        /// <summary>
        /// Save All Routes data in xml format
        /// </summary>
        /// <param name="lfs">Name of logging facility</param>
        /// <param name="export">Is export required for changed object</param>
        /// <returns>True if data successfully saved and False if not</returns>
        public static bool SaveRouteList(string lfs)
        {
            return XmlManager.SaveDataFile(lfs, false, 1, typeof(RouteList), rdata);
        }

        /// <summary>
        /// Save route with associated waypoints
        /// and export saved file
        /// </summary>
        /// <param name="route">Route object</param>
        /// <param name="waypoints">Waypoint object</param>
        /// <returns></returns>
        public static bool SaveRoute(Route route, Waypoints waypoints, string lfs)
        {
            return SaveRoute(route, waypoints, true, lfs);
        }

        /// <summary>
        /// Save route with associated waypoints
        /// </summary>
        /// <param name="route">Route object</param>
        /// <param name="waypoints">Waypoint object</param>
        /// <param name="export">Is export required</param>
        /// <returns></returns>
        private static bool SaveRoute(Route route, 
            Waypoints waypoints, bool export, string lfs)
        {
            bool is_new = route.WaypointFileName == null;
            // Make sure file name for new route waypoints is unique
            if (is_new)
            {
                route.MakeWaypointFileName();

                while (DataManager.CurWoWVersion.Routes.ContainsKey(route.WaypointFileName))
                    route.MakeWaypointFileName();
            }

            // Save waypoints first
            waypoints.Name = route.WaypointFileName;
            if (!XmlManager.Save(RouteListManager.RoutesDirFull + 
                    waypoints.Name + ".wp", typeof(Waypoints), waypoints))
                return false;

            return SaveRoute(route, waypoints, is_new, export, lfs);
        }

        /// <summary>
        /// Save existing route
        /// </summary>
        /// <param name="route">Route object</param>
        public static bool SaveRoute(Route route, string lfs)
        {
            return SaveRoute(route, null, false, false, lfs);
        }

        /// <summary>
        /// Save just the route and index route list after all
        /// </summary>
        /// <param name="route">Route object</param>
        /// <param name="is_new">Is route new</param>
        /// <param name="export">Is export required</param>
        private static bool SaveRoute(Route route, Waypoints waypoints, 
                                    bool is_new, bool export, string lfs)
        {
             if (is_new)
                DataManager.CurWoWVersion.Routes.Add(route);
            else
                DataManager.CurWoWVersion.Routes[route.WaypointFileName] = route;

            SaveRouteList(lfs);

            if (export)
                ExportRoute(route, waypoints);
            
            // Check if Quest/Gameobjects needs to be updated
            bool f1 = route.PointA.UpdateDependedObj();
            bool f2 = route.PointB.UpdateDependedObj();
            if (f1 || f2)
                // Index GameObject data as well
                DataManager.SaveGameObjData(lfs);

            return true;
        }

        public static bool ExportRoute(Route route, Waypoints waypoints)
        {
            bool res = false;
            string fname = route.MakeFileName();

            if (fname != null)
            {
                route.FileName = fname + ".route";

                // Attach waypoints and version
                route.DoBeforeExport(RouteListVersion.ToString(), waypoints);

                // Create Export file
                res = XmlManager.Save(XmlManager.ExportDataDirFull +
                                route.FileName, typeof(Route), route);
                if (!res)
                    route.FileName = null;

                // Detach waypoints
                route.DoAfterExport();
            }
            else
                route.FileName = null;

            return res;
        }

        /// <summary>
        /// Look up route in Endpoints table
        /// </summary>
        /// <param name="name">Endpoint name. Can be GameObject QuestItem name</param>
        /// <returns>Route object or null if not found</returns>
        public static Route FindRoute(string name)
        {
            return FindRoute(name, ProcessManager.MyChar.Location);
        }

        public static Route FindRoute(string name, Vector3D cur_pos)
        {
            Route r = null;

            try 
            { 
                List<Route> lr = Endpoints[name];
                if (lr == null || lr.Count == 0)
                    return null;

                // Find one close to cur pos
                // For multiple routes return random
                foreach (Route r1 in lr)
                {
                    // TODO
                }
            }  
            catch 
            {
                return null;
            }

            return r;
        }

        /// <summary>
        /// Look up route by destination waypoint in Waypoints table
        /// </summary>
        /// <param name="v">Destination waypoint</param>
        /// <returns>Route object or null if not found</returns>
        public static Route FindRoute(Vector3D v)
        {
            double len = v.Length;

            List<Route> lr;

            if (!Waypoints.TryGetValue(len, out lr))
                return null;

            // Check which route exactly have started or ended waypoint
            foreach (Route r in lr)
            {
                if (r.PointA.Waypoint.IsClose(v) ||
                        r.Reversible && r.PointB.Waypoint.IsClose(v))
                    return r;
            }

            // If nothing found than nothing found
            return null;
        }

        /// <summary>
        /// Load waypoints from external file
        /// </summary>
        /// <param name="id">Waypoint file name </param>
        /// <returns></returns>
        public static Waypoints LoadWaypoints(string id)
        {
            // Check if file exists
            string fname = RoutesDirFull + id + ".wp";
            if (!File.Exists(fname))
                throw new WaypointsNotFoundException(fname);

            Serializer<Waypoints> s = new Serializer<Waypoints>();
            return s.Load(fname);
        }

        /// <summary>
        /// Load route from external file
        /// </summary>
        /// <param name="fname">File Name</param>
        /// <returns></returns>
        public static Route ImportRoute(string fname, string lfs)
        {
            if (!File.Exists(fname))
            {
                ProcessManager.ShowError("Route file '" + fname + "' not found");
                return null;
            }

            Route r = null;
            try
            {
                Serializer<Route> s = new Serializer<Route>();
                r = s.Load(fname);
            }
            catch (Exception e)
            {
                ProcessManager.ShowError("Failed import route from file '" +
                    fname + "'. " + e.Message);
                return null;
            }

            // Check version
            if (r.Version != RouteListVersion.ToString())
            {
                ProcessManager.ShowError("Imported route version " + r.Version +
                    " different from supported " + RouteListVersion);
                return null;
            }

            // Extract waypoints and save in external file
            Waypoints wp = r.WpList;
            r.WpList = null;
            r.Version = null;

            if (!SaveRoute(r, wp, false, lfs))
                return null;

            return r;
        }

        public static void IndexData()
        {
            Routes.Clear();
            Endpoints.Clear();
            Waypoints.Clear();

            foreach (Route r in DataManager.CurWoWVersion.Routes.Values)
            {
                r.FileName = r.MakeFileName();

                if (r.FileName != null)
                {
                    if (Routes.ContainsKey(r.FileName))
                        Routes[r.FileName].Add(r);
                    else
                    {
                        List<Route> l = new List<Route>();
                        l.Add(r);
                        Routes.Add(r.FileName, l);
                    }
                }

                foreach (char c in new char[] { 'a', 'b' })
                {
                    Endpoint e = r[c];

                    // Lookup endpoint object
                    if (e.HasLinkedObj)
                    {
                        AbstractDefEndpoint de = (AbstractDefEndpoint)e;
                        switch (e.PType)
                        {
                            case EndpointTypes.GAME_OBJ:
                                de.Obj = DataManager.
                                    CurWoWVersion.GameObjData[de.Name];
                                break;

                            case EndpointTypes.QUEST_OBJ:
                                QuestObjEndpoint qoe = (QuestObjEndpoint) e;
                                Quest q = null;
                                DataManager.QuestList.
                                    TryGetValue(Convert.ToInt32(qoe.Name), out q); 
                                if (q != null && q.Objectives != null)
                                    de.Obj = q.Objectives.ObjList[qoe.ObjId];
                                break;
                        }
                    }

                    if (c.Equals('a') ||
                        c.Equals('b') && r.Reversible)
                    {
                        // Add to Endpoint waypoint
                        double len = e.Waypoint.Length;
                        List<Route> l = null;
                        try
                        {
                            l = Waypoints[len];
                        }
                        catch
                        {
                            l = new List<Route>();
                            Waypoints.Add(len, l);
                        }

                        l.Add(r);
                    }
                    else
                        continue;

                    // Add endpoint
                    if (e.PType == EndpointTypes.UNDEF)
                        continue;

                    string ename = e.ToString();
                    if (!Endpoints.ContainsKey(ename))
                    {
                        List<Route> rlist = new List<Route>();
                        rlist.Add(r);
                        Endpoints.Add(ename, rlist);
                    }
                    else
                    {
                        List<Route> rlist = Endpoints[ename];
                        rlist.Add(r);
                    }
                }
            }
        }

        public static void DeleteRoute(Route route, string lfs)
        {
            // Remove file from list
            DataManager.CurWoWVersion.Routes.Remove(route.WaypointFileName);

            // Save & Index route
            SaveRouteList(lfs);

            // Delete waypoint file
            File.Delete(RoutesDirFull + route.WaypointFileName+ ".wp");
        }

    }

    public class Vector3DComparer : IComparer<double>
    {
        public int Compare(double v1, double v2)
        {
            return (Math.Abs(v1 - v2) <= 5F) ? 0 :
                                ((v1 < v2) ? -1 : 1);
        }
    }

    #endregion
}

