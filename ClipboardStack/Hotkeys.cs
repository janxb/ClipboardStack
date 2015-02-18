using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace ClipboardStack
{
    static class Hotkeys
    {
        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);

        [DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        public const int MOD_ALT = 0x1;
        public const int MOD_CTRL = 0x2;
        public const int MOD_SHIFT = 0x4;
        public const int MOD_WIN = 0x8;
        public const int WM_HOTKEY = 0x0312;
    }
}
