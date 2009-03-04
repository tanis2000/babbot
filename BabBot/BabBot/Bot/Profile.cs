using System;
using System.Collections.Generic;
using System.Text;

namespace BabBot.Bot
{
    [Serializable]
    public class Profile
    {
        [NonSerialized]
        public string FileName;

        public string Name;
        public string Description;
        public WayPointCollection NormalWayPoints;
        public WayPointCollection GhostWayPoints;
        public WayPointCollection VendorWayPoints;
        public WayPointCollection RepairWayPoints;

        public Profile()
        {
            Name = "";
            Description = "";
            NormalWayPoints = new WayPointCollection();
            GhostWayPoints = new WayPointCollection();
            VendorWayPoints = new WayPointCollection();
            RepairWayPoints = new WayPointCollection();
        }

        public void Load()
        {
            // TODO: Implementare il caricamento del profilo xml
            throw new NotImplementedException();
        }

        public void Save()
        {
            // TODO: Implementare il salvataggio del profilo xml
            throw new NotImplementedException();
        }
    }
}
