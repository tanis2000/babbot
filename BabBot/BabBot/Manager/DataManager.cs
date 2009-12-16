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

namespace BabBot.Manager
{
    public class DataManager
    {
        // Reference the whole WoWData
        private static WoWData wdata;
        
        // Reference the whole NPCData
        private static NPCData ndata;

        // Reference the current WoW Version from WoWData
        private static WoWVersion wversion;

        public enum ServiceTypes : byte
        {
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

        // Current version of NPCData
        private static readonly int NPCDataVersion = 0;

        public static WoWVersion CurWoWVersion
        {
            get { return wversion; }
        }

        public static NPCData AllNpcList
        {
            get { return ndata; }
        }

        // NPCData file name
        private static string NPCDataFileName =
#if DEBUG
            "..\\..\\Data\\" +
#endif
            "NPCData.xml";

        internal static AppConfig AppConfig
        {
            get { return wdata.AppConfig; }
        }

        internal static GlobalOffsets Globals
        {
            get { return CurWoWVersion.Globals; }
        }

        internal static WoWVersion[] WoWVersions
        {
            get { return wdata.Versions; }
        }

        // Quest indexed list
        internal static SortedDictionary<int, Quest> QuestList =
                        new SortedDictionary<int, Quest>();

        // Zone sorted list with taxi and bind servcies
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

        // Table with all npc service types
        internal static BotDataSet.ServiceTypesDataTable ServiceTypeTable;

        static DataManager()
        {
            // Populate local data storage

            // Add service types
            ServiceTypeTable = new BotDataSet.ServiceTypesDataTable();
            foreach (ServiceTypes z in Enum.GetValues(typeof(ServiceTypes)))
                ServiceTypeTable.Rows.Add(z, Enum.GetName(typeof(ServiceTypes), z).ToLower());
            // ServiceTypeTable.AcceptChanges();
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

            foreach (NPC npc in CurWoWVersion.NPCData.STable.Values)
            {
                // Index zone
                foreach (string z in npc.Continents.ZoneList)
                    if (!ZoneList.ContainsKey(z))
                        ZoneList.Add(z, new ZoneServices());

                // Check if NPC provide taxi/inn
                // If taxi or inn than npc cannot be in multiple zones
                if (npc.HasTaxi)
                    ZoneList[npc.Continents.ContinentList[0].
                            ZList[0].Name].AddTaxiService(npc.GetSimpleFormat());
                else if (npc.HasInn)
                    ZoneList[npc.Continents.ContinentList[0].
                            ZList[0].Name].AddInnService(npc.GetSimpleFormat());

                // Index quest
                Quests ql = npc.QuestList;
                if (ql != null)
                {
                    foreach (Quest q in ql.Table.Values)
                    {
                        q.SrcNpc = npc;
                        if (!string.IsNullOrEmpty(q.DestNpcName))
                            q.DestNpc = CurWoWVersion.NPCData.STable[q.DestNpcName];
                        QuestList.Add(q.Id, q);
                    }
                }

                // Update zone list as well

            }
        }

        internal static void ClearXml()
        {
            ndata = null;
            wdata = null;
        }

        private static object LoadXmlData(string fname, Type t)
        {
            object res = null;

            XmlSerializer s = new XmlSerializer(t);
            TextReader r = new StreamReader(fname);

            try
            {
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
            ndata = (NPCData)LoadXmlData(NPCDataFileName, typeof(NPCData));

            // Check if NPC data version the same
            if (ndata.Version != NPCDataVersion)
            {
                // TODO Migrate data from old format to new and save
                // Show message for now
                ShowErrorMessage("NPCData.xml is in old format. It has version " +
                    ndata.Version + " that different from supported " + NPCDataVersion);
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

                // Merge NPCData. Ordering is not an issues since it defined in WoWData already
                // Ignore all exceptions
                try
                {
                    ndata.FindVersion(wnew.Build).MergeWith(ndata.FindVersion(wprev.Build));
                }
                catch { }

                wprev = wnew;
                i++;
            }
        }

        internal static void AfterXmlInit()
        {
            // Attach NPC data to selected WoW version
            wversion.NPCData = ndata.FindVersion(wversion.Build);

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
            foreach (NPC npc in CurWoWVersion.NPCData.STable.Values)
                npc.Changed = false;
        }

        #region Load/Save npc file

        public static NPC LoadXml(string fname)
        {
            Serializer<NPC> s = new Serializer<NPC>();
            return s.Load(fname);
        }

        public static void SaveXml(string fname, NPC npc)
        {
            Serializer<NPC> s = new Serializer<NPC>();
            s.Save(fname, npc);
        }

        /// <summary>
        /// Save NPC data in xml format
        /// </summary>
        public static bool SaveNpcData()
        {
            // Backup old NPC data before save
            string bf = System.IO.Path.GetDirectoryName(NPCDataFileName) +
                System.IO.Path.DirectorySeparatorChar +
                System.IO.Path.GetFileNameWithoutExtension(NPCDataFileName) + ".bak";
            Output.Instance.Log("npc", "Saving " + NPCDataFileName +
                " before serializing to " + bf);

            try
            {
                File.Copy(NPCDataFileName, bf, true);
            }
            catch (Exception e)
            {
                ShowErrorMessage("Failed update NPC Data. Unable copy file " + NPCDataFileName +
                            "  to " + bf + ". " + e.Message);

                return false;
            }

            if (!SaveXmlData(NPCDataFileName, typeof(NPCData), ndata))
            {
                Output.Instance.Log("npc", "Recovering " + NPCDataFileName +
                    " after error from " + bf);
                File.Copy(bf, NPCDataFileName);
                return false;
            }
            else
                Output.Instance.Log("npc", "File " + NPCDataFileName +
                                                            " successfully saved.");

            // Index NPC Data
            IndexData();

            // Check all list and generate chanded data for export

            foreach (NPC npc in CurWoWVersion.NPCData.STable.Values)
                if (npc.Changed)
                    ExportNPC(npc);

            // Reset Changed flag
            ResetChanged();

            return true;
        }

        /// <summary>
        /// Serialize the given NPC in Export directory
        /// </summary>
        /// <param name="npc"></param>
        private static void ExportNPC(NPC npc)
        {
            try
            {
                Output.Instance.Log("npc", "Exporting NPC: " + npc.Name +
                                                " to Data\\Export subdirectory");
                SaveXmlData("Data" + System.IO.Path.DirectorySeparatorChar +
                    "Export" + System.IO.Path.DirectorySeparatorChar + npc.Name + ".npc",
                    typeof(NPC), npc);
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
}
