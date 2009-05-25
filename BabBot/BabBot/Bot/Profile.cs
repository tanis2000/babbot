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
using System.Xml.Serialization;

namespace BabBot.Bot
{
    [Serializable]
    public class Profile
    {
        [XmlIgnore] 
        public string FileName;

        public string Name;
        public string Description;
        public WayPointCollection NormalWayPoints;
        public WayPointCollection GhostWayPoints;
        public WayPointCollection VendorWayPoints;
        public WayPointCollection RepairWayPoints;
        public WayPointCollection BranchWayPoints;
        public EnemyCollection Enemies;

        public Profile()
        {
            Name = "";
            Description = "";
            NormalWayPoints = new WayPointCollection();
            GhostWayPoints = new WayPointCollection();
            VendorWayPoints = new WayPointCollection();
            RepairWayPoints = new WayPointCollection();
            BranchWayPoints = new WayPointCollection();
            Enemies = new EnemyCollection();
        }

        
    }
}
