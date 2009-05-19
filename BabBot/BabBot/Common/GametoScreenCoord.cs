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
using System.Runtime.InteropServices;
using System.Linq;
using System.Text;
using System.Drawing;
using BabBot.Manager;
using BabBot.Wow;

namespace BabBot.Common
{
    class IngameTOScreen
    {
        public struct Rect
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }
        [DllImport("user32.dll")]
        private static extern bool GetClientRect(IntPtr hWnd, ref Rect rect);

        public const float Deg2Rad = 0.01745329251f;

        struct CameraInfo
        {
            public Vector Pos;
            public Matrix ViewMat;
            public float Foc;
        };

        public static bool MoveMouseToWoWCoords(float x, float y, float z)
        {
            Vector pseudoVec = new Vector(x, y, z);     //not really a vector. its the location we want to click
            IntPtr hwnd = ProcessManager.WowProcess.WindowHandle;         //windowhandle for getting size
            CameraInfo camera = new CameraInfo();
            //Read information
            uint pAddr2 = ProcessManager.WowProcess.ReadUInt((ProcessManager.WowProcess.ReadUInt((uint)Globals.cameraPointer)) + (uint)Globals.cameraOffset);
            byte[] bCamera = new byte[68];
            bCamera = ProcessManager.WowProcess.ReadBytes(pAddr2, 68);

            //Convert bytes to usable data
            camera.Pos = new Vector(BitConverter.ToSingle(bCamera, 8), BitConverter.ToSingle(bCamera, 12), BitConverter.ToSingle(bCamera, 16));
            camera.ViewMat = new Matrix(BitConverter.ToSingle(bCamera, 20), BitConverter.ToSingle(bCamera, 24), BitConverter.ToSingle(bCamera, 28),
                BitConverter.ToSingle(bCamera, 32), BitConverter.ToSingle(bCamera, 36), BitConverter.ToSingle(bCamera, 40),
                BitConverter.ToSingle(bCamera, 44), BitConverter.ToSingle(bCamera, 48), BitConverter.ToSingle(bCamera, 52));
            camera.Foc = BitConverter.ToSingle(bCamera, 64);
            //Get windoesize
            Rect rc = new Rect();
            GetClientRect(hwnd, ref rc);

            //Vector camera -> object
            Vector Diff = pseudoVec - camera.Pos;

            if ((Diff * camera.ViewMat.getFirstColumn) < 0)
                return false;

            Vector View = Diff * camera.ViewMat.inverse();
            Vector Cam = new Vector( -View.Y, -View.Z, View.X);

            float fScreenX = (rc.right - rc.left) / 2.0f;
            float fScreenY = (rc.bottom - rc.top) / 2.0f;
            //Aspect ratio
            float fTmpX = fScreenX / (float)Math.Tan(((camera.Foc * 44.0f) / 2.0f) * Deg2Rad);
            float fTmpY = fScreenY / (float)Math.Tan(((camera.Foc * 35.0f) / 2.0f) * Deg2Rad);

            Point pctMouse = new Point();
            pctMouse.X = (int)(fScreenX + Cam.X * fTmpX / Cam.Z);
            pctMouse.Y = (int)(fScreenY + Cam.Y * fTmpY / Cam.Z);
            
            if (pctMouse.X < 0 || pctMouse.Y < 0 || pctMouse.X > rc.right || pctMouse.Y > rc.bottom)
                return false;

            CommandManager.MoveMouse(pctMouse.X, pctMouse.Y);
            return true;
        }
    }
}
