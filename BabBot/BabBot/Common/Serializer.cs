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
                catch
                {
                    return new T();
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

    #region IMergeable interface

    public interface IMergeable
    {
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
    }

    #endregion

    #region Common Serialization Classes

    /// <summary>
    /// Common class for collection item that has a unique name 
    /// </summary>
    public abstract class CommonItem : IComparable, IMergeable
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        public CommonItem() { }

        public CommonItem(string name)
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
            return (obj != null) && ToString().Equals(obj.ToString());
        }

        // By default do nothing
        public virtual void MergeWith(object obj) {}
    }

    public abstract class CommonMergeListItem : CommonItem
    {
        protected IMergeable[] MergeList;

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
            return Convert.ToString(Id);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj) && (((CommonItemEx) obj).Id == Id);
        }
    }

    /// <summary>
    /// Common class for collection items with id -> name parameters
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
            return Convert.ToString(Id);
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
    public abstract class CommonTable<T> : IMergeable where T : IMergeable
    {
        private readonly Hashtable _htable;

        [XmlIgnore]
        internal T[] Items
        {
            get
            {
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
                    _htable.Add(item.ToString(), item);
            }
        }

        public CommonTable()
        {
            _htable = new Hashtable();
        }

        [XmlIgnore]
        public Hashtable Table
        {
            get { return _htable; }
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
            foreach (DictionaryEntry item1 in _htable)
            {
                T item2 = (T)t.Table[item1.Key];
                if ((item2 == null) || !item1.Value.Equals(item2))
                    return false;
            }

            // No differences found
            return true;
        }

        protected T FindItemByName(string name)
        {
            return (T)_htable[name];
        }

        public void Add(T item)
        {
            _htable.Add(item.ToString(), item);
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
            foreach (DictionaryEntry item in t.Table)
            {
                if (!_htable.ContainsKey(item.Key))
                    _htable.Add(item.Key, item.Value);
                else 
                    ((T)_htable[item.Key]).MergeWith(item.Value);
            }
        }
    }

    /// <summary>
    /// Class with internal sorted table that needs to be serialized
    /// </summary>
    /// <typeparam name="T">Type of elements in the table</typeparam>
    public abstract class CommonSortedList<T> where T : CommonItem
    {
        private readonly SortedList _slist;

        [XmlIgnore]
        internal T[] Items
        {
            get
            {
                T[] res = new T[_slist.Count];
                _slist.Values.CopyTo(res, 0);
                return res;
            }
            set
            {
                if (value == null) return;
                T[] items = (T[])value;
                _slist.Clear();
                foreach (T item in items)
                    _slist.Add(item.ToString(), item);
            }
        }

        public CommonSortedList()
        {
            _slist = new SortedList();
        }

        [XmlIgnore]
        public SortedList SList
        {
            get { return _slist; }
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            CommonSortedList<T> t = (CommonSortedList<T>)obj;

            // Check size first
            int cnt = _slist.Count;
            if (cnt != t.SList.Count)
                return false;

            // Check keys and values. List is sorted
            for ( int i = 0; i < cnt; i++)
            {
                T item2 = (T) t.SList.GetByIndex(i);

                if ((item2 == null) || !_slist.GetByIndex(i).Equals(item2))
                    return false;
            }

            // No differences found
            return true;
        }

        public T FindItemByName(string name)
        {
            return (T)_slist[name];
        }

        public T FindItemByIndex(int idx)
        {
            return ((idx < 0) || (idx >= _slist.Count)) ? 
                    default(T) :  (T) _slist.GetByIndex(idx);
        }

        public void Add(T item)
        {
            _slist.Add(item.ToString(), item);
        }

        public int ItemCount()
        {
            return _slist.Count;
        }

        public void Merge(CommonTable<T> t)
        {
            if (t == null)
                return;

            // Check values
            foreach (DictionaryEntry de in t.Table)
            {
                T item1 = (T) _slist[de.Key];
                T item2 = (T) de.Value;

                if (item1 == null)
                    _slist[de.Key] = item2;
                else if (!item1.Equals(item2))
                    item1.MergeWith(item2);
            }
        }
    }

    /// <summary>
    /// Class with internal hashtable that needs to be serialized and have a unique name
    /// </summary>
    /// <typeparam name="T">Type of elements in the table</typeparam>
    public abstract class CommonNameTable<T> : CommonTable<T> where T : IMergeable
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
    /// Class with internal hashtable that needs to be serialized and have a unique id
    /// </summary>
    /// <typeparam name="T">Type of elements in the table</typeparam>
    public abstract class CommonIdTable<T> : CommonTable<T> where T : IMergeable
    {
        [XmlAttribute("id")]
        public int Id;

        public CommonIdTable() { }

        public CommonIdTable(int id) : base()
        {
            Id = id;
        }

        public override string ToString()
        {
            return Convert.ToString(Id);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj) && (((CommonIdTable<T>)obj).Id == Id);
        }
    }

    /// <summary>
    /// Class with internal list that needs to be serialized
    /// Used for elements like list of waypoints where each element is not meargeable
    /// </summary>
    /// <typeparam name="T">Type of elements in the list</typeparam>
    public abstract class CommonList<T> : IMergeable
    {
        internal readonly ArrayList _list;

        [XmlIgnore]
        internal T[] Items
        {
            get
            {
                T[] res = new T[_list.Count];
                _list.CopyTo(res, 0);
                return res;
            }
            set
            {
                if (value == null) return;
                T[] items = (T[])value;
                _list.Clear();
                foreach (T item in items)
                    _list.Add(item);
            }
        }

        [XmlIgnore]
        public ArrayList List
        {
            get { return _list; }
        }

        public CommonList()
        {
            _list = new ArrayList();
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
            _list.Add(item);
        }

        public void MergeWith(object obj)
        {
            if (!MergeHelper.IsMergeable(this, obj))
                return;

            CommonList<T> l = (CommonList<T>)obj;

            foreach (T item1 in l.List)
            {
                foreach (T item2 in _list)
                    if (!item1.Equals(item2))
                    {
                        _list.Add(item1);
                        break;
                    }
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

        public CommonNameList() {}

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