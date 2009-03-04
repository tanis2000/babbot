using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace BabBot.Manager
{
    public static class CommandManager
    {
        #region BasicMovement enum

        public enum BasicMovement
        {
            Up,
            Left,
            Right,
            Down,
            Jump,
            Stop
        }

        #endregion

        private const int WM_KEYDOWN = 0x100;
        private const int WM_KEYUP = 0x101;

        [DllImport("user32.dll", EntryPoint = "PostMessage")]
        private static extern bool _PostMessage(IntPtr hWnd, uint msg, uint wParam, uint lParam);

        public static bool PostMessage(IntPtr hWnd, uint msg, uint wParam, uint lParam)
        {
            return _PostMessage(hWnd, msg, wParam, lParam);
        }

        #region Key Down/Up

        public static bool BasicMovementDown(int hWnd, BasicMovement bm)
        {
            uint wParam, lParam;

            switch (bm)
            {
                case BasicMovement.Up:
                    wParam = (int) Keys.Up;
                    break;

                case BasicMovement.Left:
                    wParam = (int) Keys.Left;
                    break;

                case BasicMovement.Right:
                    wParam = (int) Keys.Right;
                    break;

                case BasicMovement.Down:
                    wParam = (int) Keys.Down;
                    break;

                case BasicMovement.Jump:
                    wParam = (int) Keys.Space;
                    break;

                    // per interrompere un'azione...forse meglio spostarla
                case BasicMovement.Stop:
                    wParam = (int) Keys.Escape;
                    break;

                default:
                    return false;
            }

            return PostMessage(new IntPtr(hWnd), WM_KEYDOWN, wParam, 0);
        }

        public static bool BasicMovementUp(int hWnd, BasicMovement bm)
        {
            uint wParam, lParam;

            switch (bm)
            {
                case BasicMovement.Up:
                    wParam = (int) Keys.Up;
                    break;

                case BasicMovement.Left:
                    wParam = (int) Keys.Left;
                    break;

                case BasicMovement.Right:
                    wParam = (int) Keys.Right;
                    break;

                case BasicMovement.Down:
                    wParam = (int) Keys.Down;
                    break;

                case BasicMovement.Jump:
                    wParam = (int) Keys.Space;
                    break;

                case BasicMovement.Stop:
                    wParam = (int) Keys.Escape;
                    break;

                default:
                    return false;
            }

            return PostMessage(new IntPtr(hWnd), WM_KEYUP, wParam, 0);
        }

        #endregion


    }
}