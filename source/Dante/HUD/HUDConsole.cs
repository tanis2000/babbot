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
  
    Copyright 2009 BabBot Team -
*/

using System;
using System.Drawing;
using System.Runtime.InteropServices;
using SlimDX;
using SlimDX.Direct3D9;
using Font=SlimDX.Direct3D9.Font;

namespace Dante.HUD
{
    [StructLayout(LayoutKind.Sequential)]
    struct Vertex
    {
        public Vector4 PositionRhw;
        public int Color;
    }


    public sealed class HUDConsole : IResource
    {
        private Device device;
        private bool dirty;
        private Font font;
        private string fontName;
        private int fontSize;
        private FontWeight fontWeight;
        private Sprite sprite;

        #region Properties

        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        /// <value>The location.</value>
        public Point Location { get; set; }

        /// <summary>
        /// Gets or sets the color of the foreground.
        /// </summary>
        /// <value>The color of the foreground.</value>
        public Color4 ForegroundColor { get; set; }

        /// <summary>
        /// Gets or sets the height of a line.
        /// </summary>
        /// <value>The height of a line.</value>
        public int LineHeight { get; set; }

        /// <summary>
        /// Gets or sets the name of the font.
        /// </summary>
        /// <value>The name of the font.</value>
        public string FontName
        {
            get { return fontName; }
            set
            {
                if (fontName == value)
                {
                    return;
                }

                fontName = value;
                dirty = true;
            }
        }

        /// <summary>
        /// Gets or sets the size of the font.
        /// </summary>
        /// <value>The size of the font.</value>
        public int FontSize
        {
            get { return fontSize; }
            set
            {
                if (fontSize == value)
                {
                    return;
                }

                fontSize = value;
                dirty = true;
            }
        }

        /// <summary>
        /// Gets or sets the font weight.
        /// </summary>
        /// <value>The font weight.</value>
        public FontWeight FontWeight
        {
            get { return fontWeight; }
            set
            {
                if (fontWeight == value)
                {
                    return;
                }

                fontWeight = value;
                dirty = true;
            }
        }

        #endregion

        #region Implementation of IDisposable

        public void Dispose()
        {
            font.Dispose();
            sprite.Dispose();
            font = null;

            GC.SuppressFinalize(this);
        }

        #endregion

        #region Implementation of IResource

        public void Initialize(Device managedDevice)
        {
            device = managedDevice;
            CreateFont();
            sprite = new Sprite(device);
        }

        public void LoadContent()
        {
            font.OnResetDevice();
            sprite.OnResetDevice();
        }

        public void UnloadContent()
        {
            font.OnLostDevice();
            sprite.OnLostDevice();
        }

        #endregion

        public HUDConsole()
        {
            FontSize = 15;
            LineHeight = 15;
            FontWeight = FontWeight.Bold;
            FontName = "Arial";
            ForegroundColor = Color.White;
        }

        public void Write(string text)
        {
            if (dirty)
                CreateFont();

            device.DrawPrimitives(PrimitiveType.LineList, 0, 1);
            font.DrawString(sprite, text, Location.X, Location.Y, ForegroundColor);
        }

        public void WriteLine()
        {
            Location = new Point(Location.X, Location.Y + LineHeight);
        }

        public void WriteLine(string text)
        {
            Write(text);
            WriteLine();
        }

        public void Begin()
        {
            sprite.Begin(SpriteFlags.AlphaBlend | SpriteFlags.SortTexture);
        }

        public void End()
        {
            sprite.End();
        }

        private void CreateFont()
        {
            if (font != null)
            {
                font.Dispose();
            }

            dirty = false;
            font = new Font(device, FontSize, 0, FontWeight, 0, false,
                            CharacterSet.Default, Precision.TrueType, FontQuality.ClearTypeNatural,
                            PitchAndFamily.Default | PitchAndFamily.DontCare, FontName);
        }
    }
}