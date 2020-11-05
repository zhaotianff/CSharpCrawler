using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace CSharpCrawler.Util
{
    public class WinAPI
    {
        public const int WM_MOVING = 0x0216;
        public const int WM_SIZE = 0x0005;

        private static readonly IntPtr HWND_BOTTOM = new IntPtr(1);
        private static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);
        private static readonly IntPtr HWND_TOP = new IntPtr(0);
        private static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);

        private static uint SWP_NOMOVE = 0x0002;
        private static uint SWP_NOSIZE = 0x0001;

        private static int SW_SHOW = 5;

        [DllImport("User32.dll")]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        [DllImport("User32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("User32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd,int nCmdShow);

        public static void SetWindowOrder(IntPtr top,IntPtr bottom)
        {
            SetWindowPos(top, HWND_TOP, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE);
            SetWindowPos(bottom, HWND_BOTTOM, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE);
        }

        public static void SetTopWindow(IntPtr hwnd)
        {
            SetWindowPos(hwnd, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE);
            SetWindowPos(hwnd, HWND_NOTOPMOST, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE);
        }

        public static ushort HIWORD(uint num)
        {
            return (ushort)(num >> 16);
            //下面这种方式也可
            //return BitConverter.ToInt16(BitConverter.GetBytes(value), 2);
        }

        public static ushort LOWORD(uint num)
        {
            return (ushort)(num & 0xFFFF);
            //return BitConverter.ToInt16(BitConverter.GetBytes(value), 0);
        }
    }

    public struct RECT
    {
        public int left;
        public int top;
        public int right;
        public int bottom;
    }
}
