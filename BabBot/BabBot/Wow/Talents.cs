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
        
        [XmlElement("levels")]
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
        
        // [XmlIgnore]

        public int AddTalent(Level t)
        {
            return _list.Add( t );
        }

        public override string ToString()
        {
            return Description;
        }
        
    }
    
    // Items in the shopping list
    public class Level
    {
        [XmlAttribute("level")] public byte Num;
        [XmlAttribute("tab")] public byte TabId;
        [XmlAttribute("id")] public int TalentId;

        public Level() {}
        
        public Level(byte num, byte tab, int tid) {
            Num = num;
            TabId = tab;
            TalentId = tid;
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
