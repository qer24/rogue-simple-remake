﻿using RogueProject.Utils;

namespace ProjektFB.Utils.Libraries;

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

public static class WindowUtility
{
    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

    private const uint SWP_NOSIZE = 0x0001;
    private const uint SWP_NOZORDER = 0x0004;

    private static Size GetScreenSize() => new Size(GetSystemMetrics(0), GetSystemMetrics(1));

    private struct Size(int width, int height)
    {
        public int Width { get; set; } = width;
        public int Height { get; set; } = height;
    }

    [DllImport("User32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
    private static extern int GetSystemMetrics(int nIndex);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool GetWindowRect(HandleRef hWnd, out Rect lpRect);

    [StructLayout(LayoutKind.Sequential)]
    private struct Rect
    {
        public int Left;        // x position of upper-left corner
        public int Top;         // y position of upper-left corner
        public int Right;       // x position of lower-right corner
        public int Bottom;      // y position of lower-right corner
    }

    private static Size GetWindowSize(IntPtr window)
    {
        if (!GetWindowRect(new HandleRef(null, window), out Rect rect))
            throw new Exception("Unable to get window rect!");

        int width = rect.Right - rect.Left;
        int height = rect.Bottom - rect.Top;

        return new Size(width, height);
    }

    public static void MoveWindowToCenter()
    {
        IntPtr window = Process.GetCurrentProcess().MainWindowHandle;

        if (window == IntPtr.Zero)
        {
            Logger.Log("Couldn't find a window to center!");
            return;
        }

        var screenSize = GetScreenSize();
        var windowSize = GetWindowSize(window);

        int x = (screenSize.Width - windowSize.Width) / 2;
        int y = (screenSize.Height - windowSize.Height) / 2;

        SetWindowPos(window, IntPtr.Zero, x, y, 0, 0, SWP_NOSIZE | SWP_NOZORDER);
    }
}
