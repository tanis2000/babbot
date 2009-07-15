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
using BabBot.Wow;
using Pather.Graph;

namespace BabBot.Common
{
    public static class WaypointVector3DHelper
    {
        public static Vector3D LocationToVector3D(Location Location)
        {
            return new Vector3D(Location.X, Location.Y, Location.Z);
        }

        public static Location Vector3DToLocation(Vector3D Vector)
        {
            return new Location(Vector.X, Vector.Y, Vector.Z);
        }
    }
}
