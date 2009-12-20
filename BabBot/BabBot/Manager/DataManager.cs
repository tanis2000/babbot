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

namespace BabBot.Manager
{
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

    public class DataManager
    {
        /// <summary>
        /// Reference the whole WoWData
        /// </summary>
        private static WoWData wdata;
        
        /// <summary>
        /// Reference the whole GameObject Data
        /// </summary>
        private static GameObjectData gdata;

        /// <summary>
        /// Reference the current WoW Version from WoWData
        /// </summary>
        private static WoWVersion wversion;

        /// <summary>
        /// Current version of GameObjectData
        /// </summary>
        private static readonly int GameObjDataVersion = 0;

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
        private static string GameObjDataFileName =
#if DEBUG
            "..\\..\\Data\\" +
#endif
            "GameObjectData.xml";

        /// <summary>
        /// Waypoints xml file name
        /// </summary>
        private static string RoutesFileName =
#if DEBUG
 "..\\..\\Data\\" +
#endif
 "Routes.xml";

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

        public static BotDataSet GameData { get; private set; }

        #region Nested: Enumerates
        /// <summary>
        /// List of services provided by NPC 
        /// </summary>
        public enum ServiceTypes : byte
        {
            INN = 0,
            BANKER = 1,
            BATTLEMASTER = 2,
            CLASS_TRAINER = 3,
            TAXI = 4,
            TRADE_SKILL_TRAINER = 5,
            VENDOR_REGULAR = 6,
            VENDOR_GROSSERY = 7,
            VENDOR_REPEAR = 8,
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
            foreach (GameObjectTypes z in Enum.GetValues(typeof(GameObjectTypes)))
                GameData.GameObjectTypes.AddGameObjectTypesRow((int) z, Enum.GetName(
                                            typeof(GameObjectTypes), z).ToLower());

            // Add service types
            foreach (ServiceTypes z in Enum.GetValues(typeof(ServiceTypes)))
                GameData.ServiceTypes.Rows.Add(z, Enum.GetName(
                                            typeof(ServiceTypes), z).ToLower());

            // Add Quest Item Types
            foreach (QuestItemTypes qi in Enum.GetValues(typeof(QuestItemTypes)))
                GameData.QuestItemType.Rows.Add(qi, Enum.GetName(
                                            typeof(QuestItemTypes), qi).ToLower());

            // Create other empty tables
            GameData.AcceptChanges();

            //\\ TEST
            PopulateDataSet();

        }

        private static void ShowErrorMessage(string err)
        {
            ProcessManager.ShowError(err);
        }
        
        internal static WoWVersion FindWoWVersionByName(string version)
        {
            return wdata.FindVersion(version);
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

            foreach (GameObject g_obj in CurWoWVersion.GameObjData.STable.Values)
            {
                ZoneServices zs = null;

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
                    foreach (Quest q in ql.Table.Values)
                    {
                        q.Src = g_obj;
                        if (!string.IsNullOrEmpty(q.DestName))
                            q.Dest = CurWoWVersion.GameObjData.STable[q.DestName];
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

            foreach (Continent c in CurWoWVersion.Continents.Table.Values)
            {
                GameData.ContinentList.Rows.Add(c.Id, c.Name);
                foreach (Zone z in c.List)
                    GameData.ZoneList.AddZoneListRow(z.Name, c.Id);
            }

            foreach (GameObject g in CurWoWVersion.GameObjData.STable.Values)
                AddGameObject(g);

            GameData.AcceptChanges();
            _populated = true;
        }

        public static BotDataSet.GameObjectsRow AddGameObject(GameObject g)
        {
            BotDataSet.ZoneListRow z = GameData.ZoneList.FindByNAME(g.ZoneText);

            if (z == null)
                throw new ZoneNotFoundException(g.ZoneText);

            string faction = null;
            if (g.GetObjType() == GameObjectTypes.NPC)
                faction = ((NPC) g).Faction;

            BotDataSet.GameObjectsRow gobj_row = GameData.
                        GameObjects.AddGameObjectsRow(
                            GameData.GameObjectTypes.FindByID((int)
                                    g.GetObjType()), g.Name, z, faction);

            // Add base coordinates
            GameData.Coordinates.Rows.Add(gobj_row.ID, g.X, g.Y, g.Z);

            // Add quests
            foreach (Quest q in g.QuestList.Table.Values)
            {
                BotDataSet.QuestListRow qrow = GameData.QuestList.
                    AddQuestListRow(q.Id, gobj_row, q.Name,
                    q.GreetingText, q.ObjectivesText, g.Name, 
                    q.DestName, q.Level, q.Link, q.BonusSpell);

                // Add quest items
                for (int i = 0; i < q.QuestItems.Length; i++)
                {
                    if (q.QuestItems[i] == null)
                        continue;

                    for (int j = 0; j < q.QuestItems[i].List.Count; j++)
                    {
                        CommonQty qi = q.QuestItems[i].List[j];
                        // Locate type
                        GameData.QuestItems.AddQuestItemsRow(qrow, j,
                            (int)q.QuestItemSeq[i],qi.Name, qi.Qty, qi.Name);
                    }
                }

                if (q.ObjList != null)
                {
                    // Add quest objectives
                    for (int i = 0; i < q.ObjList.List.Count; i++)
                    {
                        AbstractQuestObjective qobj = q.ObjList.List[i];

                        int qty = 0;
                        if (qobj.HasQty)
                            qty = ((AbstractQtyQuestObjective)qobj).ReqQty;

                        GameData.QuestItems.AddQuestItemsRow(qrow, i,
                                (int)DataManager.QuestItemTypes.OBJECTIVES,
                                qobj.Name, qty, qobj.FullName);
                    }
                }
            }


            if (g.GetObjType() == GameObjectTypes.NPC)
            {
                NPC npc = (NPC)g;

                // Add other coordinates
                foreach (ZoneWp coord in npc.Coordinates.Table.Values)
                    foreach (Vector3D v in coord.List)
                        GameData.Coordinates.Rows.Add(gobj_row.ID, v.X, v.Y, v.Z);

                // Add Services
                foreach (NPCService srv in npc.Services.Table.Values)
                {
                    // Locate service
                    BotDataSet.ServiceTypesRow srv_row =
                        GameData.ServiceTypes.FindByID((int)srv.SrvType);

                    GameData.NpcServices.AddNpcServicesRow(gobj_row, srv_row, srv_row.NAME);
                }
            }

            return gobj_row;
        }

        internal static void ClearXml()
        {
            gdata = null;
            wdata = null;
        }

        private static object LoadXmlData(string fname, Type t)
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

        internal static void InitXmlData()
        {
            // data first
            wdata = (WoWData)LoadXmlData("Data\\WoWData.xml", typeof(WoWData));
            gdata = (GameObjectData)LoadXmlData(
                                GameObjDataFileName, typeof(GameObjectData));

            // Check if NPC data version the same
            if (gdata.Version != GameObjDataVersion)
            {
                // TODO Migrate data from old format to new and save
                // Show message for now
                ShowErrorMessage("NPCData.xml is in old format. It has version " +
                    gdata.Version + " that different from supported " + 
                                        GameObjDataVersion);
            }
        }

        internal static void MergeXml(string wow_version)
        {
            // Auto Merge data from earlier version with latest one
            WoWVersion wprev = (WoWVersion)wdata.STable.Values[0];
            int i = 1;
            while ((i < wdata.STable.Count) &&
                !wprev.Build.Equals(wow_version))
            {
                WoWVersion wnew = (WoWVersion)wdata.STable.Values[i];

                // Merge WoW data
                wnew.MergeWith(wprev);

                // Merge NPCData. Ordering is not an issues since it defined in 
                // WoWData already
                // Ignore all exceptions
                try
                {
                    gdata.FindVersion(wnew.Build).MergeWith(
                                    gdata.FindVersion(wprev.Build));
                }
                catch { }

                wprev = wnew;
                i++;
            }
        }

        internal static void AfterXmlInit()
        {
            // Attach NPC data to selected WoW version
            wversion.GameObjData = gdata.FindVersion(wversion.Build);

            // Index NPC data for future use
            DataManager.IndexData();

            // Reset changed flag
            ResetChanged();
        }

        /// <summary>
        /// Reset Changed flag on the whole NPC database
        /// </summary>
        private static void ResetChanged()
        {
            foreach (NPC npc in CurWoWVersion.GameObjData.STable.Values)
                npc.Changed = false;
        }

        public static void SaveGameObjRow(BotDataSet.GameObjectsRow row)
        {
            // Delete Game Object data
            GameObject obj_old = CurWoWVersion.GameObjData.STable[row.NAME];
            if (obj_old == null)
                throw new DataSynchException();

            // Mak new Game Object from dataset
            GameObject obj_new = GetGameObj(row);

            CurWoWVersion.GameObjData.STable.Remove(row.NAME);
            CurWoWVersion.GameObjData.STable.Add(row.NAME, obj_new);

            // Finally Save data
            SaveGameObjData();
        }

        private static GameObject GetGameObj(BotDataSet.GameObjectsRow row)
        {
            // Pull coordinates
            DataRow[] coords = GameData.Coordinates.Select("GID=" + row.ID);
            if (coords.Length == 0)
                throw new Exception("Game object required at least one base coordinates");

            BotDataSet.CoordinatesRow first_coord = (BotDataSet.CoordinatesRow) coords[0];
            GameObject obj = null;

            Vector3D v = new Vector3D((float)first_coord.X, 
                            (float)first_coord.Y, (float)first_coord.Z);

            switch(row.TYPE_ID)
            {
                case (int)GameObjectTypes.ITEM :
                    obj = new GameObject(row.NAME, row.ZONE_NAME, v);
                    break;

                case (int)GameObjectTypes.NPC:
                    obj = new NPC(row.NAME, row.ZONE_NAME, v, row.FACTION);
                    break;

                default:
                    throw new Exception("Unknown Game Object Type :" + row.TYPE_ID);
            }

            if (row.TYPE_ID == (int) (int)GameObjectTypes.NPC)
            {
                NPC npc = (NPC) obj;

                // NPC can have extra coordinates and services
                // Add other coordinates
                for (int i = 1; i < coords.Length; i++)
                {
                    // TODO
                    // BotDataSet.CoordinatesRow coord = (BotDataSet.CoordinatesRow) coords[i];
                    // npc.Coordinates.Table[npc.ZoneText];
                }
            }

            // Pull quest list with items
            DataRow[] qrows = GameData.QuestList.Select("GID=" + row.ID);
            foreach (BotDataSet.QuestListRow qrow in qrows)
            {
                Quest q = new Quest(qrow.ID, qrow.NAME, qrow.GREETING_TEXT,
                        qrow.OBJECTIVES_TEXT, qrow.LEVEL, qrow.BONUS_SPELL, qrow.LINK);

                for (int i = 0; i < q.QuestItemSeq.Length; i++)
                {
                    QuestItemTypes qi = q.QuestItemSeq[i];

                    DataRow[] item_rows = GameData.QuestItems.
                        Select("QID=" + qrow.ID + " AND ITEM_TYPE_ID=" + (int)qi);
                    foreach (BotDataSet.QuestItemsRow item_row in item_rows)
                        q.QuestItems[i].List.Add(new CommonQty(item_row.NAME, item_row.QTY));
                }

                DataRow[] rows_objective = GameData.QuestItems.Select("QID=" + qrow.ID +
                    " AND ITEM_TYPE_ID=" + (int)QuestItemTypes.OBJECTIVES);
            }

            return obj;
        }

        #region Load/Save single Game Object

        public static GameObject LoadXml(string fname)
        {
            Serializer<GameObject> s = new Serializer<GameObject>();
            return s.Load(fname);
        }

        public static void SaveXml(string fname, GameObject obj)
        {
            Serializer<GameObject> s = new Serializer<GameObject>();
            s.Save(fname, obj);
        }

        /// <summary>
        /// Save All Game Object data in xml format
        /// </summary>
        public static bool SaveGameObjData()
        {
            // Backup old NPC data before save
            string bf = System.IO.Path.GetDirectoryName(GameObjDataFileName) +
                System.IO.Path.DirectorySeparatorChar +
                System.IO.Path.GetFileNameWithoutExtension(GameObjDataFileName) + ".bak";
            Output.Instance.Log("npc", "Saving " + GameObjDataFileName +
                " before serializing to " + bf);

            try
            {
                File.Copy(GameObjDataFileName, bf, true);
            }
            catch (Exception e)
            {
                ShowErrorMessage("Failed update NPC Data. Unable copy file " + GameObjDataFileName +
                            "  to " + bf + ". " + e.Message);

                return false;
            }

            if (!SaveXmlData(GameObjDataFileName, typeof(GameObjectData), gdata))
            {
                Output.Instance.Log("npc", "Recovering " + GameObjDataFileName +
                    " after error from " + bf);
                File.Copy(bf, GameObjDataFileName);
                return false;
            }
            else
                Output.Instance.Log("npc", "File " + GameObjDataFileName +
                                                            " successfully saved.");

            // Index GameObject Data
            IndexData();

            // Check all list and generate chanded data for export

            foreach (GameObject obj in CurWoWVersion.GameObjData.STable.Values)
                if (obj.Changed)
                    ExportGameObj(obj);

            // Reset Changed flag
            ResetChanged();

            return true;
        }

        /// <summary>
        /// Serialize the given NPC in Export directory
        /// </summary>
        /// <param name="npc"></param>
        public static void ExportGameObj(GameObject obj)
        {
            string name = obj.Name;
            try
            {
                Output.Instance.Log("npc", "Exporting Game Object: " + name +
                                                " to Data\\Export subdirectory");
                SaveXmlData("Data" + System.IO.Path.DirectorySeparatorChar +
                    "Export" + System.IO.Path.DirectorySeparatorChar + name + ".obj",
                    typeof(GameObject), obj);
                Output.Instance.Log("npc", "Export successfull!!! Don't forget upload updated data to " +
                                        "BabBot forum https://sourceforge.net/apps/phpbb/babbot/");
            }
            catch (Exception e)
            {
                ShowErrorMessage("Unable generate export data. " + e.Message);
            }
        }

        /// <summary>
        /// Serialize object in xml format
        /// </summary>
        /// <param name="fname">Output File Name</param>
        /// <param name="t">Type of object</param>
        /// <param name="obj">Object itself</param>
        public static bool SaveXmlData(string fname, Type t, object obj)
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

        #endregion
    }

    public class ZoneServices
    {
        public readonly int ContinentId;

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
                VendorService vs = (VendorService)npc.Services.Table["vendor"];
                
                // Check for other services
                if (vs.CanRepair)
                    RepairServices.Add(npc);
                else if (vs.HasFood)
                    GrosseryServices.Add(npc);
            }
        }
    }
}
