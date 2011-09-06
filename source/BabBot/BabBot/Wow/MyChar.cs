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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BabBot.Manager;
using BabBot.Common;
using System.Xml.Serialization;

namespace BabBot.Wow
{

    /// <summary>
    /// Argument for MyChar snapshot
    /// </summary>
    public class Snapshot : CommonList<WowObject>
    {
        /// <summary>
        /// DateTime Stamp
        /// </summary>
        [XmlAttribute("dts")]
        public readonly DateTime DTS;

        /// <summary>
        /// List of all objects around MyChar taken with DTS
        /// </summary>
        [XmlElement("objects")]
        public WowObject[] Objects
        {
            get { return Items; }
            set { Items = value; }
        }

        public Snapshot()
            :base() {}

        public Snapshot(List<WowObject> snapshot)
            : base(snapshot)
        {
            DTS = DateTime.Now;
        }
    }

    /// <summary>
    /// EventHandler wrapper around Snapshot class
    /// </summary>
    public class SnapshotArg : EventArgs
    {
        public Snapshot LastSnapshot;

        public SnapshotArg(Snapshot snapshot)
        {
            LastSnapshot = snapshot;
        }
    }

    /// <summary>
    /// In-Game character controlled by bot/player
    /// Also char and also toon :)
    /// </summary>
    public class InGameChar : WowPlayer
    {
        /// <summary>
        /// Snapshot lock
        /// </summary>
        private object _lock = new object();

        /// <summary>
        /// Most recent snapshot taking from objects around in-game character
        /// </summary>
        public Snapshot CurrentSnapshot { get; private set; }
        
        /// <summary>
        /// On Snapshot Update event handler
        /// </summary>
        public event EventHandler<SnapshotArg> OnUpdate;

        public InGameChar(uint ObjectPointer) :
            base(ObjectPointer) { }

        /// <summary>
        /// Find OT_UNIT type of object around in-game character
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public WowUnit FindUnitByName(string name)
        {
            // Temp
            Update();

            WowUnit res = null;

            foreach(WowObject wo in CurrentSnapshot.List)
            {
                if (wo.Type == Descriptor.eObjType.OT_UNIT &&
                    wo.Name.Equals(name))
                {
                    res = (WowUnit)wo;
                    break;
                }
            }

            return res;
        }

        /// <summary>
        /// Snapshot update method
        /// </summary>
        public void Update()
        {
            lock (_lock)
            {
                // TODO
                // Location = 
                CurrentSnapshot = new Snapshot(ProcessManager.
                    ObjectManager.GetAllObjectsAroundLocalPlayer());
                if (OnUpdate != null)
                    OnUpdate(this, new SnapshotArg(CurrentSnapshot));
            }
        }
    }
}
