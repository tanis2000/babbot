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
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Windows.Forms;
using BabBot.Forms.Radar.Items;
using BabBot.Wow;

namespace BabBot.Forms.Radar
{
    
    public class Radar
    {
        #region Private Fields

        // the diameter of the radar
        int _size;

        // the base image of the radar
        // this saves time/cycles, b/c we only have to draw the
        // background of the radar once (and update it as necessary)
        Image _baseImage;

        // the azimuth of the radar
        int _az = 0;

        // some internally used points for drawing the fade
        // of the radar scanline
        PointF _pt = new PointF(0F, 0F);
        PointF _pt2 = new PointF(1F, 1F);
        PointF _pt3 = new PointF(2F, 2F);
        
        // do draw the scanline or not to draw the scanline?
        bool _scanLine = false;

        // colors to use in drawing the image
        Color _topColor = Color.FromArgb(0, 120, 0);
        Color _bottomColor = Color.FromArgb(0, 40, 0);
        Color _lineColor = Color.FromArgb(0, 255, 0);

        // list of items to draw on the radar
        List<RadarItem> _items = new List<RadarItem>();

        // Center item
        RadarItem _center;

        // Coordinates of center item
        PointF _ccoord = new PointF();

        // Parent picture box allocated for radar
        PictureBox _img;

        // Default item radius
        byte _r = 8;

        // Zoom
        float _zoom = 1F;

        #endregion

        #region Constructor

        public Radar(PictureBox img)
        {
            // set the diameter of the control
            // this influences the size of the output image
            _img = img;
            _size = _img.Width;

            // create the base image
            CreateBaseImage();
            
            // create the export image
            Draw();
        }

        #endregion

        #region Public Field Accessor/Mutators

        public bool DrawScanLine
        {
            get
            {
                return _scanLine;
            }
            set
            {
                bool old = _scanLine;
                _scanLine = value;
                // if the user turns off/on the scanline, redraw the image
                if (old != _scanLine)
                    Draw();
            }
        }

        public Color CustomGradientColorTop
        {
            get
            {
                return _topColor;
            }
            set
            {
                _topColor = value;
                // update the base image
                CreateBaseImage();
                // update the output image
                Draw();
            }
        }

        public Color CustomGradientColorBottom
        {
            get
            {
                return _bottomColor;
            }
            set
            {
                _bottomColor = value;
                // update the base image
                CreateBaseImage();
                // update the output image
                Draw();
            }
        }

        public Color CustomLineColor
        {
            get
            {
                return _lineColor;
            }
            set
            {
                _lineColor = value;
                // update the base image
                CreateBaseImage();
                // update the output image
                Draw();
            }
        }

        public byte R
        {
            get { return _r; }
            set { _r = value; }
        }

        public float Zoom
        {
            get { return _zoom; }
            set
            {
                _zoom = value;
                // TODO
                // Recalculate coordinates and ReDraw
            }
        }

        #endregion

        #region Private Functions

        /// <summary>
        /// Creates the base radar image
        /// </summary>
        void CreateBaseImage()
        {
            _baseImage = new Bitmap(_size, _size);

            // create the drawing objects
            Graphics g = Graphics.FromImage(_baseImage);
            Pen p = new Pen(_lineColor);
            // set a couple of graphics properties to make the
            // output image look nice
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.InterpolationMode = InterpolationMode.Bicubic;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.AntiAlias;
            // draw the background of the radar
            g.FillEllipse(new LinearGradientBrush(new Point((int)(_size / 2), 0), new Point((int)(_size / 2), _size - 1), _topColor, _bottomColor), 0, 0, _size - 1, _size - 1);
            // draw the outer ring (0° elevation)
            g.DrawEllipse(p, 0, 0, _size - 1, _size - 1);
            // draw the inner ring (60° elevation)
            int interval = _size / 3;
            g.DrawEllipse(p, (_size - interval) / 2, (_size - interval) / 2, interval, interval);
            // draw the middle ring (30° elevation)
            interval *= 2;
            g.DrawEllipse(p, (_size - interval) / 2, (_size - interval) / 2, interval, interval);
            // draw the x and y axis lines
            g.DrawLine(p, new Point(0, (int)(_size / 2)), new Point(_size - 1, (int)(_size / 2)));
            g.DrawLine(p, new Point((int)(_size / 2), 0), new Point((int)(_size / 2), _size - 1));
            // release the graphics object
            g.Dispose();
        }

        /// <summary>
        /// Draws the output image and fire the event caller for ImageUpdate
        /// </summary>
        void Draw()
        {
            // create a copy of the base image to draw on
            Image i = (Image)_baseImage.Clone();
            // create the circular path for clipping the output
            Graphics g = Graphics.FromImage(i);
            GraphicsPath path = new GraphicsPath();
            path.FillMode = FillMode.Winding;
            path.AddEllipse(-1F, -1F, (float)(_size + 1), (float)(_size + 1));
            // clip the output image to the circular shape
            g.Clip = new Region(path);
            // set a couple of graphics properties to make the
            // output image look nice
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.InterpolationMode = InterpolationMode.Bicubic;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.AntiAlias;

            // Sort by Z coord b4 draw
            _items.Sort();

            // draw each RadarItem in the list
            foreach (RadarItem item in _items)
            {
                item.DrawItem(this, g);
            }
            // if the scanline is on, draw it
            if (_scanLine)
            {
                // create the fade path and gradient
                GraphicsPath gp = new GraphicsPath(FillMode.Winding);
                gp.AddLine(new PointF((float)(_size / 2), (float)(_size / 2)), _pt2);
                gp.AddCurve(new PointF[] { _pt2, _pt3, _pt });
                gp.AddLine(_pt, new PointF((float)(_size / 2), (float)(_size / 2)));
                PathGradientBrush pgb = new PathGradientBrush(gp);
                pgb.CenterPoint = _pt;
                pgb.CenterColor = Color.FromArgb(128, _lineColor);
                pgb.SurroundColors = new Color[] { Color.Empty };
                // draw the fade path
                g.FillPath(pgb, gp);
                // draw the scanline
                g.DrawLine(new Pen(_lineColor), new PointF((float)(_size / 2), (float)(_size / 2)), _pt);
            }
            // draw the outer ring once more
            // this gives the control a clearly defined edge, good for the UI
            g.DrawEllipse(new Pen(_lineColor), 0, 0, _size - 1, _size - 1);

            // release the graphics object
            g.Dispose();

            _img.Image = i;
        }

        #endregion

        #region Public Functions

        public PointF GetXY(float x, float y)
        {
            // Everything upside down in WoW
            return new PointF((_center.X - (y - _ccoord.Y) * _zoom), 
                                    (_center.Y - (x - _ccoord.X) * _zoom));
        }

        public PointF AzEl2XY(int azimuth, int elevation)
        {
            // rotate coords... 90deg W = 180deg trig
            double angle = (270d + (double)azimuth);

            // turn into radians
            angle *= 0.0174532925d;

            double r, x, y;

            // determine the lngth of the radius
            r = (double)_size * 0.5d;
            r -= (r * (double)elevation / 90);

            x = (((double)_size * 0.5d) + (r * Math.Cos(angle)));
            y = (((double)_size * 0.5d) + (r * Math.Sin(angle)));

            return new PointF((float)x, (float)y);
        }

        public void AddCenter(ulong id, Vector3D coord, double dir)
        {
            _ccoord = new PointF(coord.X, coord.Y);
            _items.Clear();

            _center = new TriangleRadarItem(id, _r, new PointF((float)(_size / 2),
                                            (float)(_size / 2)), coord.Z, dir, CustomLineColor);
            _items.Add(_center);
        }

        public void AddItem(ulong id, Vector3D coord, double dir, Color color)
        {
            PointF p = GetXY(coord.X, coord.Y);
            _items.Add(new TriangleRadarItem(id, _r, p, coord.Z, dir, color));
        }

        public void AddItem(ulong id, Vector3D coord, Color color)
        {
            PointF p = GetXY(coord.X, coord.Y);
            _items.Add(new CircleRadarItem(id, _r, p, coord.Z, color));
        }

        public void Update()
        {
            // increment the azimuth
            _az++;

            // reset the azimuth if needed
            // if not, this will cause an OverflowException
            if (_az >= 360)
                _az = 0;

            // update the fade path coordinates
            _pt = AzEl2XY(_az, 0);
            _pt2 = AzEl2XY(_az - 20, 0);
            _pt3 = AzEl2XY(_az - 10, -10);

            // redraw the output image
            Draw();
        }

        #endregion
    }

}
