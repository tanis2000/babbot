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
        public WayPointCollection BranchWayPoints;
        public string Description;
        public EnemyCollection Enemies;
        [XmlIgnore] public string FileName;
        public WayPointCollection GhostWayPoints;

        public string Name;
        public WayPointCollection NormalWayPoints;
        public WayPointCollection RepairWayPoints;
        public WayPointCollection VendorWayPoints;

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