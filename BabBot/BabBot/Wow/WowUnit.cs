using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BabBot.Wow
{
    public class WowUnit : WowObject
    {
        public string Name;

        public WowUnit()
        {
        }

        public WowUnit(WowObject o)
        {
            Guid = o.Guid;
            ObjectPointer = o.ObjectPointer;
            Type = o.Type;
        }
    }
}
