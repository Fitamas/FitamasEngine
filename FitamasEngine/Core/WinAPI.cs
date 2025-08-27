using System;
using System.Runtime.InteropServices;

namespace Fitamas.Core
{
    public class WinAPI
    {
        public static float GetDpi() //TODO fix
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                IntPtr hwnd = GetActiveWindow();
                uint dpi = GetDpiForWindow(hwnd);
                return dpi > 0 ? dpi : 96f;
            }
            return 96f;
        }

        [DllImport("user32.dll")]
        private static extern IntPtr GetActiveWindow();

        [DllImport("user32.dll")]
        private static extern uint GetDpiForWindow(IntPtr hwnd);
    }
}
