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

namespace BabBot.Common
{
    internal static class WindowSize
    {
        [DllImport("user32.dll")]
        private static extern bool GetWindowInfo(IntPtr hwnd, ref tagWINDOWINFO pwi);

        [DllImport(@"user32.dll", EntryPoint = "SetWindowPos", 
            CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        public static extern int SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, 
                                            int X, int Y, int cx, int cy, uint uFlags);

        public static Rectangle GetSize(IntPtr hwnd)
        {
            var info = new tagWINDOWINFO();
            info.cbSize = (uint) Marshal.SizeOf(info);
            var returnRectangle = new Rectangle();
            GetWindowInfo(hwnd, ref info);
            returnRectangle.X = info.rcClient.left;
            returnRectangle.Y = info.rcClient.top;
            returnRectangle.Width = info.rcClient.right - returnRectangle.X;
            returnRectangle.Height = info.rcClient.bottom - returnRectangle.Y;
            return returnRectangle;
        }

        public static void SetPositionSize(IntPtr hwnd, int x, int y, int width, int height)
        {
            SetWindowPos(hwnd, (IntPtr)null, x, y, width, height, 0u);
        }

        public static Rectangle GetPositionSize(IntPtr hwnd)
        {
            var info = new tagWINDOWINFO();
            info.cbSize = (uint)Marshal.SizeOf(info);
            GetWindowInfo(hwnd, ref info);

            var rect = new Rectangle();
            rect.X = info.rcWindow.left;
            rect.Y = info.rcWindow.top;
            rect.Width = info.rcWindow.right - info.rcWindow.left;
            rect.Height = info.rcWindow.bottom - info.rcWindow.top;
            return rect;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct tagWINDOWINFO
    {
        /// DWORD->unsigned int
        public uint cbSize;

        /// RECT->tagRECT
        public tagRECT rcWindow;

        /// RECT->tagRECT
        public tagRECT rcClient;

        /// DWORD->unsigned int
        public uint dwStyle;

        /// DWORD->unsigned int
        public uint dwExStyle;

        /// DWORD->unsigned int
        public uint dwWindowStatus;

        /// UINT->unsigned int
        public uint cxWindowBorders;

        /// UINT->unsigned int
        public uint cyWindowBorders;

        /// ATOM->WORD->unsigned short
        public ushort atomWindowType;

        /// WORD->unsigned short
        public ushort wCreatorVersion;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct tagRECT
    {
        /// LONG->int
        public int left;

        /// LONG->int
        public int top;

        /// LONG->int
        public int right;

        /// LONG->int
        public int bottom;
    }
}