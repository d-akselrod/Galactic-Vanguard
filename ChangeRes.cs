using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;

namespace YourNamespace
{
    public class Game1 : Game
    {
        // Game1 class definition goes here...

        [DllImport("user32.dll")]
        private static extern int ChangeDisplaySettingsEx(string lpszDeviceName, ref DEVMODE lpDevMode, IntPtr hwnd, uint dwflags, IntPtr lParam);

        [StructLayout(LayoutKind.Sequential)]
        public struct DEVMODE
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string dmDeviceName;
            public short dmSpecVersion;
            public short dmDriverVersion;
            public short dmSize;
            public short dmDriverExtra;
            public int dmFields;
            public short dmOrientation;
            public short dmPaperSize;
            public short dmPaperLength;
            public short dmPaperWidth;
            public short dmScale;
            public short dmCopies;
            public short dmDefaultSource;
            public short dmPrintQuality;
            public short dmColor;
            public short dmDuplex;
            public short dmYResolution;
            public short dmTTOption;
            public short dmCollate;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string dmFormName;
            public short dmLogPixels;
            public int dmBitsPerPel;
            public int dmPelsWidth;
            public int dmPelsHeight;
            public int dmDisplayFlags;
            public int dmDisplayFrequency;
            public int dmICMMethod;
            public int dmICMIntent;
            public int dmMediaType;
            public int dmDitherType;
            public int dmReserved1;
            public int dmReserved2;
            public int dmPanningWidth;
            public int dmPanningHeight;
        }

        // Method to change the screen resolution
        public void ChangeScreenResolution(int width, int height, int colorDepth)
        {
            DEVMODE devmode = new DEVMODE();
            devmode.dmSize = (short)Marshal.SizeOf(typeof(DEVMODE));
            devmode.dmPelsWidth = width;
            devmode.dmPelsHeight = height;
            devmode.dmBitsPerPel = colorDepth;
            devmode.dmFields = 0x10000 | 0x20000 | 0x40000;

            int result = ChangeDisplaySettingsEx(null, ref devmode, IntPtr.Zero, 0x00000004 | 0x00000002, IntPtr.Zero);
            if (result == 0)
            {
                Console.WriteLine("Screen resolution changed successfully.");
            }
            else
            {
                Console.WriteLine("Failed to change screen resolution.");
            }
        }

        // Rest of Game1 class definition goes here...
    }
}
