using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BabBot.Wow
{
    // Talents list which will be serialized
    [XmlRoot("shoppingList")]
    class Talents
    {
        // List with talent build
        private ArrayList _list;

        public Talents()
        {
            _list = new ArrayList();
        }
        
        [XmlElement("item")]
        public Talent[] TalentList {
        get {
            Talent[] tlist = new Talent[ _list.Count ];
            _list.CopyTo( tlist );
            return tlist;
        }
        set {
            if( value == null ) return;
            Talent[] tlist = (Talent[])value;
            _list.Clear();
            foreach( Talent t in tlist )
                _list.Add( t );
            }
        }
        
        // [XmlIgnore]
        
        public int AddTalent( Talent t ) {
            return _list.Add( t );
        }
    }
    
    // Items in the shopping list
    public class Talent
    {
        [XmlAttribute("level")] public byte Level;
        [XmlAttribute("tab")] public byte TabId;
        [XmlAttribute("id")] public int TalentId;

        public Talent() {}

        public Talent(byte level, byte tab, int id)
        {
        Level = level;
        TabId = tab;
        TalentId = id;
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
