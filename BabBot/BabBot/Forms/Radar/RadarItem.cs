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

namespace BabBot.Forms.Radar
{
    public abstract class RadarItem : IComparable
    {
        private GraphicsPath _gp;

        protected RadarItem Link;

        protected void AddPolygon(Graphics g, double [] angles)
        {
            int len = angles.Length;
            PointF[] points = new PointF[angles.Length];
            for (int i = 0; i < len; i++)
                points[i] = new PointF(P.X - (float)(R * Math.Sin(angles[i])), 
                                                    P.Y - (float) (R * Math.Cos(angles[i])));
            _gp = new GraphicsPath(FillMode.Winding);
            _gp.AddPolygon(points);

            g.FillPath(new SolidBrush(C), _gp);
        }

        protected double GetAngle(double angle)
        {
            double d = D + angle;
            if (d >= Math.PI * 2)
                return d - Math.PI * 2;
            else if (d < 0)
                return Math.PI * 2 + d;
            else
                return d;
        }

        public readonly ulong ID;

        public readonly PointF P;

        public float X
        {
            get
            {
                return P.X;
            }
        }

        public float Y
        {
            get
            {
                return P.Y;
            }
        }

        public readonly float Z;

        public readonly int R;
        
        public readonly double D;

        public readonly Color C;

        protected readonly bool H;

        public readonly DateTime Created;

        public RadarItem(ulong id, int r, PointF p, float z, double dir, Color c)
            : this(id, r, p, z, dir, c, false) { }

        public RadarItem(ulong id, int r, PointF p, float z, double dir, Color c, RadarItem link)
            : this(id, r, p, z, dir, c, false, link) { }

        public RadarItem(ulong id, int r, PointF p, float z, double dir, Color c, bool hollow)
            : this(id, r, p, z, dir, c, hollow, null) { }

        public RadarItem(ulong id, int r, PointF p, 
                float z, double dir, Color c, bool hollow, RadarItem link)
        {
            ID = id;
            R = r;
            P = p;
            Z = z;
            D = dir;
            C = c;
            H = hollow;

            Created = DateTime.Now;
            Link = link;
        }

        // Implement IComparable CompareTo method - provide default sort order.
        public int CompareTo(object obj)
        {
            RadarItem r = obj as RadarItem;
            return (Z < r.Z) ? -1 : ((Z > r.Z) ? 1 : 0);
        }

        /// <summary>
        /// Draw link between items if they linked
        /// for ex linked mobs or calculated path
        /// </summary>
        /// <param name="radar">Pointer on radar object</param>
        /// <param name="g">Current graph canvas</param>
        public virtual void DrawItem(Radar radar, Graphics g)
        {
            if (Link != null)
                g.DrawLine(new Pen(C, 1F), new PointF(X, Y), new PointF(Link.X, Link.Y));
        }
    }
}


