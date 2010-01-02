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

// Disable warning about missing GetHashCode override method
#pragma warning disable 0659

using System;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;
using System.Collections;
using System.Xml;
using System.Collections.Generic;

namespace BabBot.Common
{
    public class Serializer<T> where T : new()
    {
        public T Load(string FileName)
        {
            using (var fs = new FileStream(FileName, FileMode.Open))
            {
                var s = new XmlSerializer(typeof(T));
                try
                {
                    fs.Seek(0, SeekOrigin.Begin);
                    return (T)s.Deserialize(fs);
                }
                finally
                {
                    fs.Close();
                }
            }
        }

        public void Save(string FileName, T obj)
        {
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", ""); // Remove  xmlns: parameters

            var s = new XmlSerializer(typeof(T));
            using (TextWriter writer = new StreamWriter(FileName))
            {
                try
                {
                    s.Serialize(writer, obj);
                }
                finally
                {
                    writer.Close();
                }
            }
        }
    }

    #region Interface

    public class MergeException : Exception {
        public MergeException(string type, string value1, string value2) :
            base(string.Format("Error merge {0} instances. Values '{1}'" +
               " == '{2}' are identical", type, value1, value2)) { }
    }

    public interface IMergeable
    {
        bool Changed { get; set; }
        void MergeWith(object obj);
    }

    public static class MergeHelper
    {
        /// <summary>
        /// Check if obj2 can be merged with obj1
        /// </summary>
        /// <param name="obj1">Base object</param>
        /// <param name="obj2">Object with possible new items</param>
        /// <returns>True if obj2 can be merged with obj1</returns>
        public static bool IsMergeable(object obj1, object obj2)
        {
            return (obj2 != null) &&  obj1.GetType().Equals(obj2.GetType());
        }

        /// <summary>
        /// Compare 2 objects that might be null
        /// </summary>
        /// <param name="obj1">Object 1</param>
        /// <param name="obj2">Object 2</param>
        /// <returns>True if both objects null or they equal</returns>
        public static bool Compare(object obj1, object obj2)
        {
            return ((obj1 == null) && (obj2 == null)) ||
                    ((obj1 != null) && (obj2 != null) && obj1.Equals(obj2));
        }
    }

    public interface IExportable
    {
        string Version { get; set; }
        void DoBeforeExport(string version);
        void DoAfterExport();
    }

    #endregion

    #region Common Serialization Classes

    /// <summary>
    /// Common class for collection item that has a unique name 
    /// </summary>
    public abstract class CommonItem : IComparable, IExportable
    {
        /// <summary>
        /// Generic name that can be used as label, reference etc
        /// </summary>
        [XmlAttribute("name")]
        public string Name { get; set; }

        private string _version = null;

        /// <summary>
        /// Version of xml. Used for export
        /// </summary>
        [XmlAttribute("version")]
        public string Version
        {
            get { return _version; }
            set { _version = value; }
        }

        protected CommonItem() { }

        public CommonItem(string name)
            : this()
        {
            Name = name;
        }

        public virtual int CompareTo(object obj)
        {
            return ToString().CompareTo(obj.ToString());
        }

        public override string ToString()
        {
            return Name;
        }

        public override bool Equals(object obj)
        {
            return (obj != null) && 
                (obj.GetType().Equals(GetType())) && 
                    ToString().Equals(obj.ToString());
        }

        public virtual void DoBeforeExport(string version)
        {
            _version = version;
        }

        public virtual void DoAfterExport()
        {
            _version = null;
        }
    }

    /// <summary>
    /// Common class for collection item that has a 
    /// unique label and can be merged with another item 
    /// for ex each item in CommonMergeList below
    /// </summary>
    public abstract class CommonMergeItem : CommonItem, IMergeable
    {
        public CommonMergeItem()
            : base() { }

        public CommonMergeItem(string name)
            : base(name) { }

        private bool _changed = false;

        [XmlIgnore]
        public virtual bool Changed
        {
            get { return _changed; }
            set { _changed = value; }
        }

        // By default do nothing
        public virtual void MergeWith(object obj) { }
    }

    /// <summary>
    /// Class with collection item that includes a mergeable list, for ex WoWVersion
    /// </summary>
    public abstract class CommonMergeListItem : CommonMergeItem
    {
        protected IMergeable[] MergeList;

        [XmlIgnore]
        public override bool Changed
        {
            get 
            {
                if (base.Changed)
                    return true;

                foreach (IMergeable m in MergeList)
                    if (m.Changed)
                        return true;

                return false;
            }
            set
            {
                base.Changed = value;

                // On reset do reset all subcomponents as well
                if (value)
                    return;
                
                foreach (IMergeable m in MergeList)
                    m.Changed = false;
            }
        }

        public CommonMergeListItem()
            : base() { }

        public CommonMergeListItem(string name)
            : base(name) { }

        public override void MergeWith(object obj)
        {
            if (!MergeHelper.IsMergeable(this, obj))
                return;

            CommonMergeListItem item = (CommonMergeListItem) obj;

            for (int i = 0; i < MergeList.Length; i++)
                if (MergeList[i] == null)
                    MergeList[i] = item.MergeList[i];
                else
                    MergeList[i].MergeWith(item.MergeList[i]);
        }
    }

    /// <summary>
    /// Common class for collection item that has a unique name and CDATA text
    /// </summary>
    public abstract class CommonText : CommonItem
    {
        [XmlElement("text", typeof(XmlCDataSection))]
        public XmlCDataSection Text { get; set; }

        [XmlIgnore]
        public string TextData
        {
            get { return ((Text != null) ? Text.InnerText : null); }
        }

        public CommonText() { }

        public CommonText(string name, string text) :
            base(name)
        {
            XmlDocument doc = new XmlDocument();
            Text = doc.CreateCDataSection(text);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj) && (((CommonText) obj).TextData.Equals(TextData));
        }
    }

    /// <summary>
    /// Common class for collection item that has a unique name and qty
    /// </summary>
    public class CommonQty : CommonItem
    {
        [XmlAttribute("qty")]
        public int Qty { get; set; }

        public CommonQty() { }

        public CommonQty(string name, int qty) :
            base(name)
        {
            Qty = qty;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj) && (((CommonQty) obj).Qty == Qty);
        }
    }

    /// <summary>
    /// Common class for collection items with id -> name parameters
    /// </summary>
    public abstract class CommonItemEx : CommonItem
    {
        [XmlAttribute("id")]
        public int Id;

        public CommonItemEx() { }

        public CommonItemEx(int id, string name) : 
            base(name)
        {
            Id = id;
        }

        public override int CompareTo(object obj)
        {
            CommonItemEx c = (CommonItemEx) obj;
            return ((Id > c.Id) ? 1 : ((Id < c.Id) ? -1 : 1));
        }

        public override string ToString()
        {
            return Id.ToString();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj) && (((CommonItemEx) obj).Id == Id);
        }
    }

    /// <summary>
    /// Common class for collection items with id parameter
    /// </summary>
    public abstract class CommonItemId 
    {
        [XmlAttribute("id")]
        public int Id;

        public CommonItemId() { }

        public CommonItemId(int id)
        {
            Id = id;
        }

        public override string ToString()
        {
            return Id.ToString();
        }

        public override bool Equals(object obj)
        {
            return (obj != null) && (((CommonItemId) obj).Id == Id);
        }
    }

    /// <summary>
    /// Class with internal hashtable that needs to be serialized
    /// </summary>
    /// <typeparam name="T">Type of elements in the table</typeparam>
    public abstract class CommonTable<T> : IMergeable
    {
        private readonly Dictionary <string, T> _htable;

        internal Dictionary<string, T> Table
        {
            get { return _htable; }
        }

        private bool _changed = false;

        [XmlIgnore]
        public bool Changed
        {
            get { 
                if (_changed)
                    return true;

                if (typeof(IMergeable).IsAssignableFrom(typeof(T)))
                {
                    // Scan list for changes

                    foreach(T item in _htable.Values)
                        if (((IMergeable)item).Changed)
                            return true;
                }

                return false;
            }
            set
            {
                _changed = value;

                if (value)
                    return;

                // Reset list as well
                if (typeof(IMergeable).IsAssignableFrom(typeof(T)))
                    foreach (T item in _htable.Values)
                        ((IMergeable)item).Changed = false;
            }
        }

        internal T[] Items
        {
            get
            {
                if (_htable.Count == 0)
                    return null;
                
                T[] res = new T[_htable.Count];
                _htable.Values.CopyTo(res, 0);
                return res;
            }
            set
            {
                if (value == null) return;
                T[] items = (T[])value;
                _htable.Clear();
                foreach (T item in items)
                    Add(item);
            }
        }

        public CommonTable()
        {
            _htable = new Dictionary<string, T> ();
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            CommonTable<T> t = (CommonTable<T>)obj;

            // Check size first
            if (_htable.Count != t.Table.Count)
                return false;

            // Check values
            foreach (KeyValuePair<string, T> item1 in _htable)
            {
                // Stupid exception if not found
                try { 
                    T item2 = t.Table[item1.Key];
                    if (!item1.Value.Equals(item2))
                        return false;

                } catch {
                    return false;
                }
            }

            // No differences found
            return true;
        }

        protected T FindItemByName(string name)
        {
            return _htable[name];
        }

        public void Add(T item)
        {
            _changed = true;
            _htable.Add(item.ToString(), item);
        }

        public void Remove(string key)
        {
            _changed = true;
            _htable.Remove(key);
        }

        public int ItemCount()
        {
            return _htable.Count;
        }

        public void MergeWith(object obj)
        {
            if (!MergeHelper.IsMergeable(this, obj))
                return;

            CommonTable<T> t = (CommonTable<T>)obj;

            // Check values
            foreach (KeyValuePair<string, T> item in t.Table)
            {
                if (!_htable.ContainsKey(item.Key))
                    Add(item.Value);
                else
                {
                    T value = _htable[item.Key];
                    if (typeof(IMergeable).IsAssignableFrom(value.GetType()))
                        ((IMergeable)value).MergeWith(item.Value);
                }
            }
        }
    }

    /// <summary>
    /// Class with internal hashtable that needs to be serialized
    /// and have version number for saved metadata
    /// and have internal data split by wow versions
    /// Used by: RoutesList, GameObjectData
    /// </summary>
    /// <typeparam name="T">Type of elements in the table</typeparam>
    public abstract class CommonVersionTable<T> : CommonTable<T>
    {
        [XmlAttribute("version")]
        public int Version;

        [XmlElement("wow_version")]
        public T[] Versions
        {
            get { return (T[])Items; }
            set { Items = value; }
        }

        public T FindVersion(string version)
        {
            return FindItemByName(version);
        }
    }

    /// <summary>
    /// Class with internal sorted table that needs to be serialized
    /// Used by: WoWData
    /// </summary>
    /// <typeparam name="T">Type of elements in the table</typeparam>
    public abstract class CommonSortedTable<T> : IMergeable
    {
        private readonly SortedList <string, T> _stable;

        private bool _changed = false;

        [XmlIgnore]
        public bool Changed
        {
            get { return _changed; }
            set { _changed = value; }
        }

        internal int Count
        {
            get { return _stable.Count; }
        }

        public T this[int idx]
        {
            get
            {
                return ((idx < 0) || (idx >= _stable.Count)) ?
                    default(T) : _stable.Values[idx];
            }
        }

        internal T[] Items
        {
            get
            {
                if (_stable.Count == 0)
                    return null;

                T[] res = new T[_stable.Count];
                _stable.Values.CopyTo(res, 0);
                return res;
            }
            set
            {
                if (value == null) 
                    return;

                T[] items = (T[])value;
                _stable.Clear();
                foreach (T item in items)
                    Add(item);
            }
        }

        internal SortedList<string, T> STable
        {
            get { return _stable; }
        }

        public CommonSortedTable()
        {
            _stable = new SortedList<string, T>();
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            CommonSortedTable<T> t = (CommonSortedTable<T>)obj;

            // Check size first
            int cnt = _stable.Count;
            if (cnt != t.STable.Count)
                return false;

            // Check keys and values. List is sorted
            for ( int i = 0; i < cnt; i++)
            {
                T item2 = t.STable.Values[i];

                if ((item2 == null) || !_stable.Values[i].Equals(item2))
                    return false;
            }

            // No differences found
            return true;
        }

        public T FindItemByName(string name)
        {
            return _stable[name];
        }

        /*
        public T GetItemByIndex(int idx)
        {
            return ((idx < 0) || (idx >= _stable.Count)) ?
                    default(T) : _stable.Values[idx];
        }
        */

        public void Add(T item)
        {
            _changed = true;
            _stable.Add(item.ToString(), item);
        }

        public int ItemCount()
        {
            return _stable.Count;
        }
        
        public void MergeWith(object obj)
        {
            // Same as CommonTable.MergeWith

            if (!MergeHelper.IsMergeable(this, obj))
                return;

            CommonTable<T> t = (CommonTable<T>)obj;

            // Check values
            foreach (KeyValuePair<string, T> item in t.Table)
            {
                if (!_stable.ContainsKey(item.Key))
                    Add(item.Value);
                else
                {
                    T value = _stable[item.Key];
                    if (typeof(IMergeable).IsAssignableFrom(value.GetType()))
                        ((IMergeable)value).MergeWith(item.Value);
                }
            }
        }
    }

    /// <summary>
    /// Class with internal sorted table that needs to be serialized and have a unique name
    /// Used by: NPCVersion
    /// </summary>
    /// <typeparam name="T">Type of elements in the table</typeparam>
    public abstract class CommonNameTable<T> : CommonSortedTable<T>
    {
        [XmlAttribute("name")]
        public string Name;

        public override string ToString()
        {
            return Name;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj) && 
                (((CommonNameTable<T>)obj).Name.Equals(Name));
        }
    }

    /// <summary>
    /// Class with internal list (sorted or not) that needs to be serialized
    /// Used for elements like list of zones or waypoints 
    /// Used by: Continent, Zone
    /// </summary>
    /// <typeparam name="T">Type of elements in the list</typeparam>
    public abstract class CommonList<T> : IMergeable
    {
        internal readonly List<T> _list;

        internal T[] Items
        {
            get
            {
                if (_list.Count == 0)
                    return null;

                T[] res = new T[_list.Count];
                _list.CopyTo(res, 0);
                return res;
            }
            set
            {
                if (value == null) 
                    return;

                T[] items = (T[])value;
                _list.Clear();
                foreach (T item in items)
                    Add(item);
            }
        }

        internal List<T> List
        {
            get { return _list; }
        }

        private bool _changed = false;

        [XmlIgnore]
        public bool Changed
        {
            get { return _changed; }
            set { _changed = value; }
        }

        internal bool IsSorted = false;

        public CommonList()
        {
            _list = new List<T>();

            // By default not sorted
            IsSorted = false;
        }

        public CommonList(bool sorted)
            : this()
        {
            IsSorted = sorted;
        }

        public CommonList(T[] list)
        {
            _list = new List<T>(list);
            IsSorted = false;
        }

        public CommonList(T[] list, bool sorted)
            : this(list)
        {
            IsSorted = sorted;
            if (IsSorted)
                _list.Sort();
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            CommonList<T> l = (CommonList<T>)obj;

            // Check size first
            if (_list.Count != l.List.Count)
                return false;

            // Check values
            for (int i = 0; i < _list.Count; i++)
            {
                T item1 = (T)_list[i];
                T item2 = (T)l.List[i];

                if ((item2 == null) || !item1.Equals(item2))
                    return false;
            }

            // No differences found
            return true;
        }

        public void Add(T item)
        {
            _changed = true;
            _list.Add(item);
            if (IsSorted)
                _list.Sort();
        }

        public void MergeWith(object obj)
        {
            if (!MergeHelper.IsMergeable(this, obj))
                return;

            CommonList<T> l = (CommonList<T>)obj;

            foreach (T item1 in l.List)
            {
                bool f = false;
                foreach (T item2 in _list)
                {
                    if (item1.Equals(item2))
                    {
                        if (typeof(IMergeable).IsAssignableFrom(item1.GetType()))
                            ((IMergeable)item1).MergeWith(item2);

                        f = true;
                        break;
                    }
                }
                if (!f)
                    Add(item1);
            }
        }
    }

    /// <summary>
    /// Class with internal list that needs to be serialized and have a unique name
    /// </summary>
    /// <typeparam name="T">Type of elements in the list</typeparam>
    public abstract class CommonNameList<T> : CommonList<T>
    {
        [XmlAttribute("name")]
        public string Name;

        public CommonNameList() : base() {}

        public CommonNameList(string name) :
            base()
        {
            Name = name;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj) && ((CommonNameList<T>) obj).Name.Equals(Name);
        }

        public override string ToString()
        {
            return Name;
        }
    }

    #endregion
}