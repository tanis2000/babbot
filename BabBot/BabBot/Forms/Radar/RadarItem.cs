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
        private ulong _id;
        private PointF _p;
        private float _z;
        private int _r;
        private double _d;
        private Color _color;

        private DateTime _created = DateTime.Now;
        private GraphicsPath _gp;

        protected void AddPolygon(Graphics g, double [] angles)
        {
            int len = angles.Length;
            PointF[] points = new PointF[angles.Length];
            for (int i = 0; i < len; i++)
                points[i] = new PointF(_p.X - (float)(R * Math.Sin(angles[i])), 
                                                    _p.Y - (float) (R * Math.Cos(angles[i])));
            _gp = new GraphicsPath(FillMode.Winding);
            _gp.AddPolygon(points);

            g.FillPath(new SolidBrush(_color), _gp);
        }

        protected double GetAngle(double angle)
        {
            double d = _d + angle;
            if (d >= Math.PI * 2)
                return d - Math.PI * 2;
            else if (d < 0)
                return Math.PI * 2 + d;
            else
                return d;
        }

        public ulong ID
        {
            get
            {
                return _id;
            }
        }

        public PointF P
        {
            get
            {
                return _p;
            }
        }

        public float X
        {
            get
            {
                return _p.X;
            }
        }

        public float Y
        {
            get
            {
                return _p.Y;
            }
        }

        public float Z
        {
            get
            {
                return _z;
            }
        }

        public int R
        {
            get
            {
                return _r;
            }
        }
        
        public double D
        {
            get
            {
                return _d;
            }
        }
        
        public Color Color
        {
            get
            {
                return _color;
            }
        }
        
        public DateTime Created
        {
            get
            {
                return _created;
            }
        }

        public RadarItem(ulong id, int r, PointF p, float z, double dir, Color c)
        {
            _id = id;
            _r = r;
            _p = p;
            _z = z;
            _d = dir;
            _color = c;
        }

        // Implement IComparable CompareTo method - provide default sort order.
        public int CompareTo(object obj)
        {
            RadarItem r = obj as RadarItem;
            return (_z < r.Z) ? -1 : ((_z > r.Z) ? 1 : 0);
        }

        // Abstract
        public abstract void DrawItem(Radar radar, Graphics g);    
    }
}


