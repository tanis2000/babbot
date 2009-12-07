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
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;
using System.Collections;
using System.Xml;

namespace BabBot.Common
{
    public class Serializer<T> where T : new()
    {
        public T Load(string FileName)
        {
            using (var fs = new FileStream(FileName, FileMode.Open))
            {
                var s = new XmlSerializer(typeof (T));
                try
                {
                    fs.Seek(0, SeekOrigin.Begin);
                    return (T) s.Deserialize(fs);
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

            var s = new XmlSerializer(typeof (T));
            using (TextWriter writer = new StreamWriter(FileName))
            {
                try
                {
                    s.Serialize(writer, obj, ns);
                }
                finally
                {
                    writer.Close();
                }
            }
        }
    }

    #region Common Serialization Classes

    /// <summary>
    /// Common class for collection item that has a unique name 
    /// </summary>
    public abstract class CommonItem : IComparable
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
            return ToString().CompareTo(((CommonItem)obj).ToString());
        }

        public override string ToString()
        {
            return Name;
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
    }

    /// <summary>
    /// Common class for collection item that has a unique name and qty
    /// </summary>
    public abstract class CommonQty : CommonItem
    {
        [XmlElement("qty")]
        public int Qty { get; set; }

        public CommonQty() { }

        public CommonQty(string name, int qty) :
            base(name)
        {
            Qty = qty;
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
    }

    /// <summary>
    /// Class with internal hashtable that needs to be serialized
    /// </summary>
    /// <typeparam name="T">Type of elements in the table</typeparam>
    public abstract class CommonTable<T>
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

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public T FindItemByName(string name)
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

        public void Merge(CommonTable<T> t)
        {
            if (t == null)
                return;

            // Check values
            foreach (DictionaryEntry item in t.Table)
            {
                if (!_htable.ContainsKey(item.Key))
                    _htable.Add(item.Key, item.Value);
                else if (!item.Value.Equals(_htable[item.Key]))
                    _htable[item.Key] = item.Value;
            }
        }
    }

    /// <summary>
    /// Class with internal hashtable that needs to be serialized and have a unique name
    /// </summary>
    /// <typeparam name="T">Type of elements in the table</typeparam>
    public abstract class CommonNameTable<T> : CommonTable<T>
    {
        [XmlAttribute("name")]
        public string Name;

        public override string ToString()
        {
            return Name;
        }
    }

    /// <summary>
    /// Class with internal list that needs to be serialized
    /// </summary>
    /// <typeparam name="T">Type of elements in the list</typeparam>
    public abstract class CommonList<T>
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

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public void Add(T item)
        {
            _list.Add(item);
        }

        public void Merge(CommonList<T> l)
        {
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
    }

    #endregion
}