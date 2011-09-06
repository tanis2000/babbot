using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glider.Common.Objects;

namespace Pather.Helpers
{
    public class EasyItem
    {
        public string RealName = null;
        public GItem GItem = null;
        public Item Item = null;
        public long GUID = 0;
        public EasyItem(GItem gi, Item ii, long g)
        {
            GItem = gi;
            Item = ii;
            GUID = g;
        }

        public EasyItem(GItem gi, Item ii, long g, string rn)
        {
            RealName = rn;
            GItem = gi;
            Item = ii;
            GUID = g;
        }

        public EasyItem(Item i)
        {
            GItem gi = Inventory.GetItem(i.Name);
            if (gi == null)
                gi = Inventory.GetItem(GetBaseName(i.Name));
            GItem = gi;
            Item = i;
            if(gi  != null)
            {
                GUID = gi.GUID;
            }
        }

        public EasyItem Create(Item i)
        {
            GItem gi = Inventory.GetItem(i.Name);
            if (gi == null) return null;
            EasyItem E = new EasyItem(gi, i, gi.GUID);
            return E;
        }

        public EasyItem Create(Item i, string rn)
        {
            GItem gi = Inventory.GetItem(i.Name);
            if (gi == null) return null;
            EasyItem E = new EasyItem(gi, i, gi.GUID, rn);
            return E;
        }

        public string GetBaseName(string name)
        {
            if (name.Contains(" of"))
                return name = name.Substring(0, name.IndexOf(" of")).TrimEnd();
            else
                return name;
        }
    }
}
