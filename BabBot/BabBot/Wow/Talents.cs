using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BabBot.Wow
{
    // Talents list which will be serialized
    [XmlRoot("talents")]
    public class Talents
    {
        // List with talent build
        private ArrayList _list;
        // Full path to file
        private string _path;
        private string _fname;

        public Talents()
        {
            Init();
        }

        public Talents(string pattern, string url, string descr, Level[] levels)
        {
            Init();

            Pattern = pattern;
            URL = url;
            Description = descr;
            LevelList = levels;
        }

        private void Init()
        {
            _list = new ArrayList();
        }

        [XmlAttribute("pattern")]
        public string Pattern = null;

        [XmlAttribute("url")]
        public string URL = null;

        [XmlAttribute("description")]
        public string Description;

        [XmlAttribute("class")]
        public string Class;

        [XmlIgnore]
        public string FileName
        {
            get { return _fname; }
        }

        [XmlIgnore]
        public string FullPath
        {
            get { return _path; }
            set {
                _path = value;
                _fname = System.IO.Path.GetFileName(_path);
            }
        }

        [XmlElement("level")]
        public Level[] LevelList
        {
            get {
                Level[] tlist = new Level[_list.Count];
                _list.CopyTo( tlist );
                return tlist;
            }
            set {
                if( value == null ) return;
                Level[] tlist = (Level[])value;
                _list.Clear();
                foreach (Level t in tlist)
                    _list.Add( t );
            }
        }

        public int AddLevel(Level t)
        {
            return _list.Add( t );
        }

        public void RemoveLevel(Level l)
        {
            _list.Remove(l);
        }

        public override string ToString()
        {
            return FileName;
        }
        
    }
    
    // Items in the shopping list
    public class Level
    {
        [XmlAttribute("num")] public byte Num;
        [XmlAttribute("tab_id")] public byte TabId;
        [XmlAttribute("talent_id")] public int TalentId;
        [XmlAttribute("rank")] public byte Rank;

        public Level() {}
        
        public Level(byte num, byte tab, int tid, byte rank) {
            Update(num, tab, tid, rank);
        }

        public void Update(byte tab, int tid, byte rank)
        {
            Update(Num, tab, tid, rank);
        }

        public void Update(byte num, byte tab, int tid, byte rank) {
            Num = num;
            TabId = tab;
            TalentId = tid;
            Rank = rank;
        }

        public override string ToString()
        {
            return string.Format("Level: {0} {1}/{2}/{3}",
                                    Num, TabId, TalentId, Rank);
        }

    }
/*
Talents tlist = new Talents();
tlist.AddItem( new Talent( 10, 1, 1 ) );
tlist.AddItem( new Talent( 11, 1, 2 ) );
tlist.AddItem( new Talent( 12, 1, 3 ) );

// Serialization
XmlSerializer s = new XmlSerializer( typeof( Talents ) );
TextWriter w = new StreamWriter( "Profiles\Talents\HunterBM.txt" );
s.Serialize( w, Talents );
w.Close();

// Deserialization
Talents talents;
TextReader r = new StreamReader( "Profiles\Talents\HunterBM.txt" );
Talents = (Talents)s.Deserialize( r );
r.Close();
*/
}
