using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace WinformControls.Helpers {
    public static class WindowsNative {
        public const int WM_ERASEBKGND = 0x14;
        public const int WM_PAINT = 0xF;
        public const int WM_NCPAINT = 0x85;

        public const int NM_FIRST = 0;
        public const int NM_CUSTOMDRAW = NM_FIRST - 12;

        public const int WM_NOTIFY = 0x4E;

        public const int EM_SETCUEBANNER = 0x1501;

        public const int WM_CHANGEUISTATE = 0x0127;
        public const int WM_QUERYUISTATE = 0x0129;
        public const int WM_UPDATEUISTATE = 0x0128;

        public const int UIS_SET = 1;
        public const int UISF_HIDEFOCUS = 0x1;

        [DllImport( "user32.dll", CharSet = CharSet.Auto )]
        public static extern IntPtr SendMessage( IntPtr hWnd, int Msg, int wParam, [MarshalAs( UnmanagedType.LPWStr )] string lParam );

        [DllImport( "user32.dll", CharSet = CharSet.Auto )]
        public static extern IntPtr SendMessage( IntPtr hWnd, int Msg, int wParam, int lParam );

        public static int MakeLong( int wLow, int wHigh ) {
            int low = ( int ) IntLoWord( wLow );
            short high = IntLoWord( wHigh );
            int product = 0x10000 * ( int ) high;
            int mkLong = ( int ) ( low | product );
            return mkLong;
        }

        private static short IntLoWord( int word ) {
            return ( short ) ( word & short.MaxValue );
        }

    }
}
