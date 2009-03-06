using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace BabBot.Manager
{
    /// <summary>
    /// Main class for toon movements and actions 
    /// </summary>
    public static class CommandManager
    {
        #region ArrowKey enum

        /// <summary>
        /// Enumerator for arrow keys
        /// </summary>
        public enum ArrowKey
        {
            Up = 0x26,
            Down = 0x28,
            Left = 0x25,
            Right = 0x27
        }

        #endregion

        #region External declarations

        [DllImport("user32.dll", EntryPoint = "PostMessage")]
        private static extern int _PostMessage(int hWnd, int msg, int wParam, uint lParam);

        [DllImport("user32.dll", EntryPoint = "MapVirtualKey")]
        private static extern int _MapVirtualKey(int uCode, int uMapType);

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

        public static int WowHWND;

        #region Send keys methods

        /// <summary>
        /// Sends keystrokes to the specified window
        /// </summary>
        /// <param name="keys">String of keys to send</param>
        /// <returns>Returns number of keystrokes sent, -1 if an error occurs</returns>
        public static int SendKeys(string keys)
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
            str.Replace("{TAB}", Convert.ToChar(0x9).ToString());
            str.Replace("{ENTER}", Convert.ToChar(0xD).ToString());
            str.Replace("{ESC}", Convert.ToChar(0x1B).ToString());
            str.Replace("{F5}", Convert.ToChar(0x74).ToString());
            str.Replace("{F12}", Convert.ToChar(0x7B).ToString());
            str.Replace("{SHIFTD}", Convert.ToChar(0xC1).ToString());
            str.Replace("{SHIFTU}", Convert.ToChar(0xC2).ToString());

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

                    if (_PostMessage(WowHWND, 0x101, Convert.ToInt32(chr), (MakeLong(1, ret) + 0xC0000000)) == 0)
                    {
                        return -1;
                    }
                }
                i++;
            }
            return i;
        }

        /// <summary>
        /// Taps the specified arrow key
        /// </summary>
        /// <param name="key">The arrow key to be tapped ("left", "up", "right", or "down")</param>
        /// <returns>Returns true if successful, false if not</returns>
        public static bool SendArrowKey(ArrowKey key)
        {
            //If hWnd is 0 return false
            if (WowHWND <= 0)
            {
                return false;
            }


            var wParam = (int) key;
            uint lParam;

            //Set up lParam based upon which button needs pressing
            switch (key)
            {
                case ArrowKey.Left:
                    lParam = 0x14B0001;
                    break;

                case ArrowKey.Up:
                    lParam = 0x1480001;
                    break;

                case ArrowKey.Right:
                    lParam = 0x14D0001;
                    break;

                case ArrowKey.Down:
                    lParam = 0x1500001;
                    break;

                default:
                    return false;
            }

            //Post the WM_KEYDOWN message, return false if unsuccessful
            if (_PostMessage(WowHWND, 0x100, wParam, lParam) == 0)
            {
                return false;
            }

            //Sleep to let the window process the message
            Thread.Sleep(5);

            //Post the WM_KEYUP message, return false if unsuccessful
            if (_PostMessage(WowHWND, 0x101, wParam, (lParam + 0xC0000000)) == 0)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Holds down an arrow key for the specified time
        /// </summary>
        /// <param name="key">The arrow key to be tapped ("left", "up", "right", or "down")</param>
        /// <param name="holdDelay">Number of milliseconds to hold down key</param>
        /// <returns>Returns true if successful, false if not</returns>
        public static bool SendArrowKey(ArrowKey key, int holdDelay)
        {
            //If hWnd is 0 return false
            if (WowHWND <= 0)
            {
                return false;
            }


            var wParam = (int) key;
            uint lParam;

            //Set up lParam based upon which button needs pressing
            switch (key)
            {
                case ArrowKey.Left:
                    lParam = 0x14B0001;
                    break;

                case ArrowKey.Up:
                    lParam = 0x1480001;
                    break;

                case ArrowKey.Right:
                    lParam = 0x14D0001;
                    break;

                case ArrowKey.Down:
                    lParam = 0x1500001;
                    break;

                default:
                    return false;
            }

            //Post the WM_KEYDOWN message, return false if unsuccessful
            if (_PostMessage(WowHWND, 0x100, wParam, lParam) == 0)
            {
                return false;
            }

            //Sleep for half a second to emulate the delay you get when you hold a key down on your keyboard
            Thread.Sleep(500);

            //Loop until i >= delay specified in parameter 3
            for (int i = 0; i < holdDelay; i += 50)
            {
                //Post the WM_KEYDOWN message with the repeat flag turned on, return false if unsuccessful
                if (_PostMessage(WowHWND, 0x100, wParam, (lParam + 0x40000000)) == 0)
                {
                    return false;
                }

                //Sleep for 1/20th of a second between posting the message
                Thread.Sleep(50);
            }

            //Post the WM_KEYUP message, return false if unsuccessful
            if (_PostMessage(WowHWND, 0x101, wParam, (lParam + 0xC0000000)) == 0)
            {
                return false;
            }

            return true;
        }

        #endregion

    }
}