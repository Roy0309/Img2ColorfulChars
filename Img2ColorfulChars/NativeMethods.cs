using System;
using System.Runtime.InteropServices;

namespace Img2ColorfulChars
{
    internal class NativeMethods
    {
        // Source: visual studio - Custom text color in C# console application? - Stack Overflow
        // Answered by Alexei Shcherbakov & Olivier Jacot-Descombes
        // https://stackoverflow.com/questions/7937256/custom-text-color-in-c-sharp-console-application
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool SetConsoleMode(IntPtr hConsoleHandle, int mode);
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool GetConsoleMode(IntPtr handle, out int mode);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr GetStdHandle(int handle);
    }
}
