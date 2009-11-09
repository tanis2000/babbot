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
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace BabBot.Forms.Radar.Items
{
    public class TriangleRadarItem : RadarItem
    {
        private static double angle = Math.PI / 6; // 30 degree
        private static double b1 = Math.PI - angle;
        private static double b2 = Math.PI + angle;
 
        public TriangleRadarItem(ulong id, int r, PointF p, float z, double dir, Color c)
            : base(id, r, p, z, dir, c) { }

        public override void DrawItem(Radar radar, Graphics g)
        {
            double[] a = {D, GetAngle(b1), GetAngle(b2) };
            AddPolygon(g, a);
        }
    }
}
