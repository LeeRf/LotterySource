using System;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;

namespace DoubleBalls.Style.Helper
{
    public class WinApi
    {
        [DllImport("user32.dll", EntryPoint = "GetSystemMetrics")]
        public static extern int GetSystemMetrics(int which);

        [DllImport("user32.dll")]
        public static extern void
            SetWindowPos(IntPtr hwnd, IntPtr hwndInsertAfter,
                         int X, int Y, int width, int height, uint flags);

        private const int SM_CXSCREEN = 0;
        private const int SM_CYSCREEN = 1;
        private static IntPtr HWND_TOP = IntPtr.Zero;
        private const int SWP_SHOWWINDOW = 64; // 0x0040

        public static int ScreenX {
            get { return GetSystemMetrics(SM_CXSCREEN); }
        }

        public static int ScreenY {
            get { return GetSystemMetrics(SM_CYSCREEN); }
        }

        public static void SetWinFullScreen(IntPtr hwnd) {
            SetWindowPos(hwnd, HWND_TOP, 0, 0, ScreenX, ScreenY, SWP_SHOWWINDOW);
        }
    }
    /// <summary>
    /// Class used to preserve / restore state of the form
    /// </summary>
    public static class FormState
    {
        private static FormWindowState winState;
        private static FormBorderStyle brdStyle;
        private static bool topMost;
        private static Rectangle bounds;

        private static bool isMaximized = false;

        public static bool IsMaximized
        {
            get { return isMaximized; }
        }

        public static void Maximize(Form targetForm)
        {
            if (!isMaximized)
            {
                targetForm.SuspendLayout();
                isMaximized = true;
                Save(targetForm);
                targetForm.WindowState = FormWindowState.Normal;
                targetForm.FormBorderStyle = FormBorderStyle.None;
                targetForm.TopMost = false;
                WinApi.SetWinFullScreen(targetForm.Handle);
                targetForm.ResumeLayout(true);
            }
        }

        public static void Save(Form targetForm)
        {
            targetForm.SuspendLayout();
            winState = targetForm.WindowState;
            brdStyle = targetForm.FormBorderStyle;
            topMost = targetForm.TopMost;
            bounds = targetForm.Bounds;
            targetForm.ResumeLayout(true);
        }

        public static void Restore(Form targetForm)
        {
            targetForm.WindowState = winState;
            targetForm.FormBorderStyle = brdStyle;
            targetForm.TopMost = topMost;
            targetForm.Bounds = bounds;
            isMaximized = false;
        }
    }
}