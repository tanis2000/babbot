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
using System.Drawing;
using System.Runtime.InteropServices;
using BabBot.Manager;
using BabBot.Wow;

namespace BabBot.Common
{
    internal class IngameTOScreen
    {
        public const float Deg2Rad = 0.01745329251f;

        [DllImport("user32.dll")]
        private static extern bool GetClientRect(IntPtr hWnd, ref Rect rect);

        public static bool MoveMouseToWoWCoords(float x, float y, float z)
        {
            var pseudoVec = new Vector3D(x, y, z); //not really a vector. its the location we want to click
            IntPtr hwnd = ProcessManager.WowProcess.WindowHandle; //windowhandle for getting size
            var camera = new CameraInfo();
            //Read information
            uint pAddr2 =
                ProcessManager.WowProcess.ReadUInt((ProcessManager.WowProcess.ReadUInt(ProcessManager.CurWoWVersion.Globals.cameraPointer)) +
                                                   ProcessManager.CurWoWVersion.Globals.cameraOffset);
            var bCamera = new byte[68];
            bCamera = ProcessManager.WowProcess.ReadBytes(pAddr2, 68);

            //Convert bytes to usable data
            camera.Pos = new Vector3D(BitConverter.ToSingle(bCamera, 8), BitConverter.ToSingle(bCamera, 12),
                                    BitConverter.ToSingle(bCamera, 16));
            camera.ViewMat = new Matrix(BitConverter.ToSingle(bCamera, 20), BitConverter.ToSingle(bCamera, 24),
                                        BitConverter.ToSingle(bCamera, 28),
                                        BitConverter.ToSingle(bCamera, 32), BitConverter.ToSingle(bCamera, 36),
                                        BitConverter.ToSingle(bCamera, 40),
                                        BitConverter.ToSingle(bCamera, 44), BitConverter.ToSingle(bCamera, 48),
                                        BitConverter.ToSingle(bCamera, 52));
            camera.Foc = BitConverter.ToSingle(bCamera, 64);
            //Get windoesize
            var rc = new Rect();
            GetClientRect(hwnd, ref rc);

            //Vector camera -> object
            Vector3D Diff = pseudoVec - camera.Pos;

            if ((Diff*camera.ViewMat.getFirstColumn) < 0)
            {
                return false;
            }

            Vector3D View = Diff * camera.ViewMat.inverse();
            var Cam = new Vector3D(-View.Y, -View.Z, View.X);

            float fScreenX = (rc.right - rc.left)/2.0f;
            float fScreenY = (rc.bottom - rc.top)/2.0f;
            //Aspect ratio
            float fTmpX = fScreenX/(float) Math.Tan(((camera.Foc*44.0f)/2.0f)*Deg2Rad);
            float fTmpY = fScreenY/(float) Math.Tan(((camera.Foc*35.0f)/2.0f)*Deg2Rad);

            var pctMouse = new Point();
            pctMouse.X = (int) (fScreenX + Cam.X*fTmpX/Cam.Z);
            pctMouse.Y = (int) (fScreenY + Cam.Y*fTmpY/Cam.Z);

            if (pctMouse.X < 0 || pctMouse.Y < 0 || pctMouse.X > rc.right || pctMouse.Y > rc.bottom)
            {
                return false;
            }

            ProcessManager.CommandManager.MoveMouse(pctMouse.X, pctMouse.Y);
            return true;
        }

        #region Nested type: CameraInfo

        private struct CameraInfo
        {
            public float Foc;
            public Vector3D Pos;
            public Matrix ViewMat;
        } ;

        #endregion

        #region Nested type: Rect

        public struct Rect
        {
            public int bottom;
            public int left;
            public int right;
            public int top;
        }

        #endregion
    }
}