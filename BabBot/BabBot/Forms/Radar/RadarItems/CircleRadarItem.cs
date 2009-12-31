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

namespace BabBot.Forms.Radar.Items
{
    public class CircleRadarItem : RadarItem
    {
        public CircleRadarItem(ulong id, int r, PointF p, float z, Color c)
            : base(id, r, p, z, 0, c) { }

        public CircleRadarItem(ulong id, int r, 
                PointF p, float z, Color c, bool hollow)
            : base(id, r, p, z, 0, c, hollow) { }

        public CircleRadarItem(ulong id, int r, 
                PointF p, float z, Color c, bool hollow, RadarItem link)
            : base(id, r, p, z, 0, c, hollow, link) { }

        public override void DrawItem(Radar radar, Graphics g)
        {
            RectangleF r = new RectangleF(new PointF(X - R/2, Y - R/2), new SizeF(R, R));

            if (H)
                g.DrawEllipse(new Pen(C, radar.PenWidth), r);
            else
                g.FillEllipse(new SolidBrush(C), r);

            // Draw link if required
            base.DrawItem(radar, g);
        }
    }
}
