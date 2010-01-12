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
using System.Text;
using System.Threading;
using System.Windows.Forms;
using BabBot.Common;

namespace BabBot.Manager
{
    /// <summary>
    /// Main class for toon movements and actions 
    /// </summary>
    public class CommandManager
    {
        #region ArrowKey enum

        /// <summary>
        /// Enumerator for arrow keys
        /// </summary>
        public enum ArrowKey
        {
            /// <summary>
            /// ArrowKey UP 
            /// </summary>
            Up = 0x26,
            /// <summary>
            /// ArrowKey DOWN 
            /// </summary>
            Down = 0x28,
            /// <summary>
            /// ArrowKey LEFT 
            /// </summary>
            Left = 0x25,
            /// <summary>
            /// ArrowKey RIGHT 
            /// </summary>
            Right = 0x27
        }

        #endregion

        #region Special Constant Keys

        public const string SK_ENTER = "{ENTER}";
        public const string SK_ESC = "{ESC}";
        public const string SK_F12 = "{F12}";
        public const string SK_F5 = "{F5}";
        public const string SK_SHIFT_DOWN = "{SHIFTD}";
        public const string SK_SHIFT_UP = "{SHIFTU}";
        public const string SK_TAB = "{TAB}";
        public const string SK_CTRL_TAB = "{CTRLTAB}";
        public const string SK_SPACE = "{SPACE}";
        public const string SK_Q = "Q";
        public const string SK_E = "E";

        #endregion

        #region External declarations

        [DllImport("user32.dll", EntryPoint = "PostMessage")]
        private static extern int _PostMessage(int hWnd, int msg, int wParam, uint lParam);

        [DllImport("user32.dll", EntryPoint = "MapVirtualKey")]
        private static extern int _MapVirtualKey(int uCode, int uMapType);

        [DllImport("user32.dll")]
        private static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        #endregion

        #region Private util functions

        /// <summary>
        /// Create the lParam for PostMessage
        /// </summary>
        /// <param name="a">HiWord</param>
        /// <param name="b">LoWord</param>
        /// <returns>Returns the long value</returns>
        private static uint MakeLong(int a, int b)
        {
            return (((ushort) (a)) | ((uint) ((ushort) (b) << 16)));
        }

        #endregion

        public int WowHWND;

        public CommandManager(int hwnd)
        {
            WowHWND = hwnd;
        }

        public CommandManager()
        {
            WowHWND = 0;
        }

        #region Keyboard Manager

        public int SendKeys(string keys)
        {
            return SendKeys(keys, true);
        }
        /// <summary>
        /// Sends keystrokes to the specified window
        /// </summary>
        /// <param name="keys">String of keys to send</param>
        /// <param name="release">True to send release event, False if button keep pressed</param>
        /// <returns>Returns number of keystrokes sent, -1 if an error occurs</returns>
        public int SendKeys(string keys, bool release)
        {
            if (WowHWND <= 0 || keys.Length == 0)
            {
                return -1;
            }
            int ret, i = 0;

            var str = new StringBuilder(keys.ToUpper());

            str.Replace(Convert.ToChar("`"), Convert.ToChar(0xC0));
            str.Replace(Convert.ToChar("~"), Convert.ToChar(0xC0));
            str.Replace(Convert.ToChar("-"), Convert.ToChar(0xBD));
            str.Replace(Convert.ToChar("="), Convert.ToChar(0xBB));
            str.Replace(SK_TAB, Convert.ToChar(0x9).ToString());
            str.Replace(SK_CTRL_TAB, Convert.ToChar(0x3B).ToString()); // ??
            str.Replace(SK_ENTER, Convert.ToChar(0xD).ToString());
            str.Replace(SK_ESC, Convert.ToChar(0x1B).ToString());
            str.Replace(SK_F5, Convert.ToChar(0x74).ToString());
            str.Replace(SK_F12, Convert.ToChar(0x7B).ToString());
            str.Replace(SK_SHIFT_DOWN, Convert.ToChar(0xC1).ToString());
            str.Replace(SK_SHIFT_UP, Convert.ToChar(0xC2).ToString());
            str.Replace(SK_SPACE, Convert.ToChar(0x20).ToString());
            //str.Replace(SK_Q, Convert.ToChar(0x71).ToString());
            //str.Replace(SK_E, Convert.ToChar(0x65).ToString());

            for (int ix = 1; ix <= str.Length; ++ix)
            {
                char chr = str[i];

                if (Convert.ToInt32(chr) == 0xC1)
                {
                    _PostMessage(WowHWND, 0x100, 0x10, 0x002A0001);
                    _PostMessage(WowHWND, 0x100, 0x10, 0x402A0001);
                    Thread.Sleep(1);
                }
                else if (Convert.ToInt32(chr) == 0xC2)
                {
                    _PostMessage(WowHWND, 0x101, 0x10, 0xC02A0001);
                    Thread.Sleep(1);
                }
                else
                {
                    ret = _MapVirtualKey(Convert.ToInt32(chr), 0);
                    if (_PostMessage(WowHWND, 0x100, Convert.ToInt32(chr), MakeLong(1, ret)) == 0)
                    {
                        return -1;
                    }

                    Thread.Sleep(1);

                    if (release)
                    {
                        //Post the WM_KEYUP message, return false if unsuccessful
                        if (_PostMessage(WowHWND, 0x101, Convert.ToInt32(chr), 
                                                (MakeLong(1, ret) + 0xC0000000)) == 0)
                        {
                            return -1;
                        }
                    }
                }
                i++;
            }
            return i;
        }

        /// <summary>
        /// Retrieve lparam value for arrow key
        /// </summary>
        /// <param name="key">Arrow key</param>
        /// <returns>lparam value or 0 if key is unknown</returns>
        private uint GetArrowKeyCode(ArrowKey key)
        {
            //Set up lParam based upon which button needs pressing
            switch (key)
            {
                case ArrowKey.Left: return 0x14B0001;
                case ArrowKey.Up: return 0x1480001;
                case ArrowKey.Right: return 0x14D0001;
                case ArrowKey.Down: return 0x1500001;
                default: return 0;
            }

        }

        /// <summary>
        /// Retrieve wparam and lparam for arrow key
        /// </summary>
        /// <param name="key">Arrow key</param>
        /// <param name="wparam">wParam value</param>
        /// <param name="lparam">lParam value</param>
        /// <returns>True if key valid and False if key invalid (unknown)</returns>
        private bool CheckArrowKey(ArrowKey key, out int wparam, out uint lparam)
        {
            wparam = (int)key;
            lparam = GetArrowKeyCode(key);
            if (lparam == 0)
                    return false;

            //If hWnd is 0 return false
            if (WowHWND <= 0)
                return false;

            return true;
        }

        /// <summary>
        /// Taps the specified arrow key. Send both UP and DOWN events
        /// with delay of 5 msec
        /// </summary>
        /// <param name="key">The arrow key to be send</param>
        /// <param name="release">True if send WM_KEYUP message as well</param>
        /// <returns>Returns true if successful, false if not</returns>
        public bool SendArrowKey(ArrowKey key)
        {
            if (!ArrowKeyDown(key))
                return false;

            //Sleep to let the window process the message
            Thread.Sleep(5);

            return ArrowKeyUp(key);
        }

        /// <summary>
        /// Holds down an arrow key for the specified time
        /// </summary>
        /// <param name="key">The arrow key to be send</param>
        /// <param name="holdDelay">Number of milliseconds to hold down key</param>
        /// <returns>Returns true if successful, false if not</returns>
        public bool SendArrowKey(ArrowKey key, int holdDelay)
        {
            int wParam;
            uint lParam;

            if (!CheckArrowKey(key, out wParam, out lParam))
                return false;

            //Post the WM_KEYDOWN message, return false if unsuccessful
            if (_PostMessage(WowHWND, 0x100, wParam, lParam) == 0)
                return false;

            //Sleep to emulate the delay you get when you hold a key down on your keyboard
            Thread.Sleep(50);

            //Loop until i >= delay specified in parameter 2
            for (int i = 1; i < holdDelay; i += 50)
            {
                //Post the WM_KEYDOWN message with the repeat flag turned on, return false if unsuccessful
                if (_PostMessage(WowHWND, 0x100, wParam, (lParam + 0x40000000)) == 0)
                    return false;

                //Sleep for 1/20th of a second between posting the message
                Thread.Sleep(50);
            }

            //Post the WM_KEYUP message, return false if unsuccessful
            if (_PostMessage(WowHWND, 0x101, wParam, (lParam + 0xC0000000)) == 0)
                return false;

            return true;
        }

        /// <summary>
        /// Send WM_KEYDOWN event for arrow key
        /// </summary>
        /// <param name="key">Arrow key</param>
        /// <returns>True if WM_KEYDOWN message successfully sent and
        /// False if not</returns>
        public bool ArrowKeyDown(ArrowKey key)
        {
            uint lparam;
            int wparam;

            if (!CheckArrowKey(key, out wparam, out lparam))
                return false;

            //Post the WM_KEYDOWN message, return false if unsuccessful
            return (_PostMessage(WowHWND, 0x100, wparam, lparam) == 0);
        }

        /// <summary>
        /// Send WM_KEYUP event for arrow key
        /// </summary>
        /// <param name="key">Arrow key</param>
        /// <returns>True if WM_KEYUP message successfully sent and
        /// False if not</returns>
        public bool ArrowKeyUp(ArrowKey key)
        {
            uint lparam;
            int wparam;

            if (!CheckArrowKey(key, out wparam, out lparam))
                return false;

            //Post the WM_KEYUP message, return false if unsuccessful
            return (_PostMessage(WowHWND, 0x101,
                wparam, (lparam + 0xC0000000)) == 0);
        }

        #endregion

        #region Mouse Manager

        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;
        private Rectangle _WowWindowRect;

        public int CenterX, CenterY;

        public Rectangle WowWindowRect
        {
            get
            {
                if (_WowWindowRect.Width == 0 && _WowWindowRect.Height == 0)
                {
                    SetWindowSize();
                }
                return _WowWindowRect;
            }
            set { _WowWindowRect = value; }
        }

        public int CursorX()
        {
            return Cursor.Position.X;
        }

        public int CursorY()
        {
            return Cursor.Position.Y;
        }


        public void SetWindowSize()
        {
            WowWindowRect = WindowSize.GetSize((IntPtr) WowHWND);
            CenterX = WowWindowRect.Width/2 + WowWindowRect.X;
            CenterY = WowWindowRect.Height/2 + WowWindowRect.Y;
        }

        public void MoveMouseRelative(int X, int Y)
        {
            X += WowWindowRect.X;
            Y += WowWindowRect.Y;
            Cursor.Position = new Point(X, Y);
        }

        public void MoveMouse(int X, int Y)
        {
            Cursor.Position = new Point(X, Y);
        }

        public void MoveMouseToCenter()
        {
            MoveMouse(WowWindowRect.Width/2, WowWindowRect.Height/2);
        }

        public void RightClickOnCenter()
        {
            SetWindowSize();
            SingleClick(WowWindowRect.Width/2, WowWindowRect.Height/2, "right");
        }

        public void LeftClickOnCenter()
        {
            SetWindowSize();
            SingleClick(WowWindowRect.Width / 2, WowWindowRect.Height / 2, "left");
        }

        public void SingleClick(string MouseButton)
        {
            MouseButton = MouseButton.ToLower();

            if (MouseButton == "left")
            {
                LeftMouseDown();
                LeftMouseUp();
            }
            if (MouseButton == "right")
            {
                RightMouseDown();
                RightMouseUp();
            }
        }

        public void SingleClick(int X, int Y, string MouseButton)
        {
            MoveMouse(X, Y);

            MouseButton = MouseButton.ToLower();

            if (MouseButton == "left")
            {
                LeftMouseDown();
                LeftMouseUp();
            }
            if (MouseButton == "right")
            {
                RightMouseDown();
                RightMouseUp();
            }
        }

        public void DoubleClick(String MouseButton)
        {
            MouseButton = MouseButton.ToLower();

            if (MouseButton == "left")
            {
                LeftMouseDown();
                LeftMouseUp();
                LeftMouseDown();
                LeftMouseUp();
            }
            if (MouseButton == "right")
            {
                RightMouseDown();
                RightMouseUp();
                RightMouseDown();
                RightMouseUp();
            }
        }

        public void DoubleClick(int X, int Y, String MouseButton)
        {
            MoveMouse(X, Y);

            MouseButton = MouseButton.ToLower();

            if (MouseButton == "left")
            {
                LeftMouseDown();
                LeftMouseUp();
                LeftMouseDown();
                LeftMouseUp();
            }
            if (MouseButton == "right")
            {
                RightMouseDown();
                RightMouseUp();
                RightMouseDown();
                RightMouseUp();
            }
        }

        public void DragMouse(int FinishX, int FinishY, String MouseButton)
        {
            MouseButton = MouseButton.ToLower();

            if (MouseButton == "left")
            {
                LeftMouseDown();
            }
            if (MouseButton == "right")
            {
                RightMouseDown();
            }


            if (MouseButton == "left")
            {
                LeftMouseUp();
            }
            if (MouseButton == "right")
            {
                RightMouseUp();
            }
        }

        public void DragMouse(int StartX, int StartY, int FinishX, int FinishY, String MouseButton)
        {
            MoveMouse(StartX, StartY);

            MouseButton = MouseButton.ToLower();

            if (MouseButton == "left")
            {
                LeftMouseDown();
            }
            if (MouseButton == "right")
            {
                RightMouseDown();
            }

            Thread.Sleep(50);

            MoveMouse(FinishX, FinishY);

            Thread.Sleep(50);

            if (MouseButton == "left")
            {
                LeftMouseUp();
            }
            if (MouseButton == "right")
            {
                RightMouseUp();
            }
        }

        public void LeftMouseDown()
        {
            Point Coordinates = Cursor.Position;
            mouse_event(MOUSEEVENTF_LEFTDOWN, Coordinates.X, Coordinates.Y, 0, 0);
        }

        public void LeftMouseUp()
        {
            Point Coordinates = Cursor.Position;
            mouse_event(MOUSEEVENTF_LEFTUP, Coordinates.X, Coordinates.Y, 0, 0);
        }

        public void RightMouseDown()
        {
            Point Coordinates = Cursor.Position;
            mouse_event(MOUSEEVENTF_RIGHTDOWN, Coordinates.X, Coordinates.Y, 0, 0);
        }

        public void RightMouseUp()
        {
            Point Coordinates = Cursor.Position;
            mouse_event(MOUSEEVENTF_RIGHTUP, Coordinates.X, Coordinates.Y, 0, 0);
        }

        #endregion
    }
}