using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace BabBot.Wow
{
    [XmlRoot("wow_data")]
    public class WoWData
    {
        private Hashtable _versions;

        public WoWData() 
        {
            Init();
        }

        public WoWData(WoWVersion[] versions)
        {
            Init();
            Versions = versions;
        }

        private void Init()
        {
            _versions = new Hashtable();
        }

        [XmlElement("version")]
        public WoWVersion[] Versions
        {
            get {
                WoWVersion[] res = new WoWVersion[_versions.Count];
                _versions.Values.CopyTo(res,0);
                return res;
            }
            set
            {
                if (value == null) return;
                WoWVersion[] items = (WoWVersion[])value;
                _versions.Clear();
                foreach (WoWVersion item in items)
                    _versions.Add(item.Number, item);
            }
        }

        public WoWVersion FindVersion(string version)
        {
            return (WoWVersion) _versions[version];
        }
    }
    
    [Serializable]
    public class WoWVersion
    {
        
        private ArrayList _tlist;

        public WoWVersion()
        {
            Init();
        }

        private void Init()
        {
            _tlist = new ArrayList();
        }

        [XmlAttribute("num")]
        public string Number;

        [XmlElement("lua")]
        public LuaProc LuaList;
        
        public string FindLuaFunction(string name)
        {
            return LuaList.FindLuaFunction(name);
        }
    }

    public class LuaProc
    {
        private Hashtable _flist;

        public LuaProc() { 
            _flist = new Hashtable(); 
        }

        public LuaProc(LuaFunction[] flist)
        {
            FList = flist;
        }

        [XmlElement("function")]
        public LuaFunction[] FList
        {
            get
            {
                LuaFunction[] res = new LuaFunction[_flist.Count];
                _flist.Values.CopyTo(res, 0);
                return res;
            }

            set
            {
                if (value == null) return;
                LuaFunction[] items = (LuaFunction[])value;
                _flist.Clear();
                foreach (LuaFunction item in items)
                    _flist.Add(item.Name, item);
            }
        }

        public string FindLuaFunction(string name)
        {
            LuaFunction res = (LuaFunction)_flist[name];
            return (res != null) ? res.Code : null;
        }
    }

    public class LuaFunction
    {
        [XmlAttribute("name")] public string Name;
        [XmlElement("text", typeof(XmlCDataSection))]
        public XmlCDataSection Text;

        // [XmlElement("text")] public string Text;

        public LuaFunction() {}

        public LuaFunction(string name, string text)
        {
            Name = name;
            XmlDocument doc = new XmlDocument();
            Text = doc.CreateCDataSection(text);
        }

        [XmlIgnore]
        public string Code
        {
            get { return ((Text != null) ? Text.InnerText : null); }
        }

    }
}
