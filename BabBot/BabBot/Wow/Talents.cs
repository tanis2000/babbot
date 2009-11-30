using System;
using System.IO;
using System.Collections;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using BabBot.Manager;

namespace BabBot.Wow
{
    public class TalentLearnException : Exception
    {
        public TalentLearnException(string err) : base(err) { }

        public TalentLearnException(string err, string talent, int id, int rank) :
            base(string.Format("Failed learn talent '{0}; id: {1}; rank {2}. {3}",
                talent, id, rank, err)) { }
    }

    public struct TalentInfo
    {
        public string Name;
        public int Rank;
        public bool Meets;

        public TalentInfo(string name, int rank, bool meets)
        {
            Name = name;
            Rank = rank;
            Meets = meets;
        }
    }

    // Talents list which will be serialized
    [XmlRoot("talents")]
    public class Talents
    {
        // List with talent build
        private ArrayList _list;
        // Full path to file
        private string _path;
        private string _fname;

        [XmlIgnore]
        public ArrayList Levels
        {
            get { return _list; }
        }

        public Talents()
        {
            _list = new ArrayList();
        }

        public Talents(string fname, string url, string descr) : 
                                         this("", url, descr, null) {
            _path = ProcessManager.Config.ProfilesDir +
                            Path.DirectorySeparatorChar + 
                            "Talents" + Path.DirectorySeparatorChar + fname;
        }

        public Talents(string pattern, string url, string descr, 
                                                    Level[] levels) : this()
        {
            Pattern = pattern;
            URL = url;
            Description = descr;
            LevelList = levels;
        }

        [XmlAttribute("pattern")]
        public string Pattern = null;

        [XmlAttribute("url")]
        public string URL = null;

        [XmlAttribute("learn_order")]
        public string LearningOrder = null;

        [XmlAttribute("description")]
        public string Description;

        [XmlAttribute("class")]
        public string Class;

        [XmlAttribute("wow_version")]
        public string WoWVersion;

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
            get
            {
                Level[] tlist = new Level[_list.Count];
                _list.CopyTo(tlist);
                return tlist;
            }
            set
            {
                if (value == null) return;
                Level[] tlist = (Level[])value;
                _list.Clear();
                foreach (Level t in tlist)
                    _list.Add(t);
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

        public Level GetLevel(int num)
        {
            return (Level) _list[num];
        }

        public override string ToString()
        {
            // Strip extension
            return Path.GetFileNameWithoutExtension(FileName);
        }
        
    }
    
    // Items in the shopping list
    public class Level: ICloneable
    {
        [XmlAttribute("num")] 
        public int Num;
        
        [XmlAttribute("tab_id")] 
        public int TabId;
        
        [XmlAttribute("talent_id")] 
        public int TalentId;
        
        [XmlAttribute("rank")] 
        public int Rank;

        public Level() {}

        public Level(int num, int tab, int tid, int rank)
        {
            Update(num, tab, tid, rank);
        }

        public void Update(int tab, int tid, int rank)
        {
            Update(Num, tab, tid, rank);
        }

        public void Update(int num, int tab, int tid, int rank)
        {
            Num = num;
            TabId = tab;
            TalentId = tid;
            Rank = rank;
        }

        public string InfoStr
        {
            get { return ToString(); }
        }

        public override string ToString()
        {
            return string.Format("Level {0}: {1}/{2}/{3}",
                                    Num, TabId, TalentId, Rank);
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public string TalentToString()
        {
            return string.Format("{0}/{1}/{2}", TabId, TalentId, Rank);
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
