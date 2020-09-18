using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;

namespace WinformControls {
    internal class BorderlessDatePicker : DateTimePicker {
        #region RECT struct
        [StructLayout( LayoutKind.Sequential )]
        private struct RECT {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }
        #endregion

        #region ComboBoxInfo Struct
        [StructLayout( LayoutKind.Sequential )]
        private struct ComboBoxInfo {
            public int cbSize;
            public RECT rcItem;
            public RECT rcButton;
            public IntPtr stateButton;
            public IntPtr hwndCombo;
            public IntPtr hwndEdit;
            public IntPtr hwndList;
        }
        #endregion

        [StructLayout( LayoutKind.Sequential )]
        private struct DATETIMEPICKERINFO {
            public int cbSize;
            public RECT rcCheck;
            public IntPtr stateCheck;
            public RECT rcButton;
            public IntPtr stateButton;
            public IntPtr hwndEdit;
            public IntPtr hwndUD;
            public IntPtr hwndDropDown;
        }

        private const int DTM_FIRST = 4096;
        private const int DTM_GETDATETIMEPICKERINFO = DTM_FIRST + 14;

        [DllImport( "user32.dll", EntryPoint = "SendMessageA" )]
        private static extern IntPtr SendMessage( IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam );

        [DllImport( "user32.dll" )]
        private static extern bool GetComboBoxInfo( IntPtr hwndCombo, ref ComboBoxInfo info );

        private bool _mouseInButton = false;
        private bool _droppedDown = false;
        private int _buttonWidth;

        public Color ForeColor { get; set; }
        public Color BackColor { get; set; }

        public Color ButtonBackColor { get; set; }
        public Color ButtonHoverColor { get; set; }

        public Color ButtonBorderColor { get; set; }
        public Color ButtonBorderHoverColor { get; set; }

        public BorderlessDatePicker()
            : base() {
            this.SetStyle( ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true );

            this.CustomFormat = "dd.MM.yyyy";
            this.Format = DateTimePickerFormat.Custom;

            getComboButtonWidth();

            BackColor = Color.White;
            ForeColor = Color.Black;
        }

        protected override void OnPaint( PaintEventArgs e ) {
            int dayWidth = ( int ) Math.Ceiling( e.Graphics.MeasureString( this.Value.ToString( "dd.MM.yyyy" ), this.Font ).Width );
            Rectangle dayRect = new Rectangle( 0, 0, dayWidth, ClientRectangle.Height );

            StringFormat drawFormat = new StringFormat();
            drawFormat.LineAlignment = StringAlignment.Center;

            using ( Brush backgroundBrush = new SolidBrush( BackColor ) ) {
                e.Graphics.FillRectangle( backgroundBrush, ClientRectangle );
            }
            using ( Brush foregroundBrush = new SolidBrush( ForeColor ) ) {
                e.Graphics.DrawString( this.Value.ToString( "dd.MM.yyyy" ), this.Font, foregroundBrush, dayRect, drawFormat );
            }

            Rectangle buttonRect = new Rectangle(
                    ClientRectangle.Width - _buttonWidth,
                    ClientRectangle.Y,
                    _buttonWidth - 1,
                    ClientRectangle.Height - 1 );

            if ( _mouseInButton || _droppedDown ) {
                e.Graphics.FillRectangle( new SolidBrush( ButtonHoverColor ), buttonRect );
                e.Graphics.DrawRectangle( new Pen( ButtonBorderHoverColor ), buttonRect );
            }
            else {
                e.Graphics.FillRectangle( new SolidBrush( ButtonBackColor ), buttonRect );
                e.Graphics.DrawRectangle( new Pen( ButtonBorderColor ), buttonRect );
            }

            Bitmap bitmap = Resource.baseline_date_range_black_18dp;
            e.Graphics.DrawImage( 
                bitmap,
                new Point( ClientRectangle.Width - _buttonWidth / 2 - bitmap.Width / 2, ClientRectangle.Y + ClientRectangle.Height / 2 - bitmap.Height / 2 ) );
        }

        protected override void OnMouseLeave( EventArgs e ) {
            base.OnMouseLeave( e );
            _mouseInButton = false;
        }

        protected override void OnMouseMove( MouseEventArgs e ) {
            base.OnMouseMove( e );

            if ( e.X > ( this.Width - _buttonWidth ) ) {
                if ( _mouseInButton == false ) {
                    _mouseInButton = true;
                    this.Refresh();
                }
            }
            else {
                if ( _mouseInButton == true ) {
                    _mouseInButton = false;
                    this.Refresh();
                }
            }
#if DEBUG
            Console.WriteLine( _mouseInButton );
#endif
        }

        protected override void OnCloseUp( EventArgs eventargs ) {
            base.OnCloseUp( eventargs );
            _droppedDown = false;
#if DEBUG
            Console.WriteLine( "CloseUp" );
#endif
        }
        
        protected override void OnDropDown( EventArgs eventargs ) {
            base.OnDropDown( eventargs );
            _droppedDown = true;
#if DEBUG
            Console.WriteLine( "DropDown" );
#endif
        }

        protected override void OnResize( EventArgs e ) {
            base.OnResize( e );
            getComboButtonWidth();
            Refresh();
        }

        private void getComboButtonWidth() {
            if ( !this.IsHandleCreated )
                return;

            DATETIMEPICKERINFO pickerInfo = new DATETIMEPICKERINFO() { 
                cbSize = Marshal.SizeOf( typeof( DATETIMEPICKERINFO ) ) 
            };
            
            IntPtr pickerInfoPointer = Marshal.AllocHGlobal( Marshal.SizeOf( pickerInfo ) );
            Marshal.StructureToPtr( pickerInfo, pickerInfoPointer, false );

            SendMessage( this.Handle, DTM_GETDATETIMEPICKERINFO, ( IntPtr ) 0, pickerInfoPointer );

            pickerInfo = ( DATETIMEPICKERINFO ) Marshal.PtrToStructure( pickerInfoPointer, typeof( DATETIMEPICKERINFO ) );

            int buttonWidth = pickerInfo.rcButton.Right - pickerInfo.rcButton.Left;
            _buttonWidth = buttonWidth;
            
#if DEBUG
            Console.WriteLine( "_buttonWidth = " + _buttonWidth );
#endif
        }

        //public BorderlessDatePicker() : base() {
        //    this.SetStyle( ControlStyles.DoubleBuffer, true );
        //    this.SetStyle( ControlStyles.AllPaintingInWmPaint, true );
        //}

        //[DllImport( "user32.dll" )]
        //static extern int ReleaseDC( IntPtr hWnd, IntPtr hDC );

        //[DllImport( "user32.dll" )]
        //private static extern IntPtr GetWindowDC( IntPtr hWnd );

        //[DllImport( "user32.dll" )]
        //public static extern bool RedrawWindow( IntPtr hWnd, IntPtr lprcUpdate, IntPtr hrgnUpdate, RedrawWindowFlags flags );

        //[DllImport( "user32.dll", EntryPoint = "SendMessageA" )]
        //public static extern IntPtr SendMessage( IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam );

        //[DllImport( "gdi32.dll" )]
        //[return: MarshalAs( UnmanagedType.Bool )]
        //public static extern bool DeleteObject( IntPtr hObject );

        

        //[Flags()]
        //public enum RedrawWindowFlags : uint {
        //    Invalidate = 0x1,
        //    InternalPaint = 0x2,
        //    Erase = 0x4,
        //    Validate = 0x8,
        //    NoInternalPaint = 0x10,
        //    NoErase = 0x20,
        //    NoChildren = 0x40,
        //    AllChildren = 0x80,
        //    UpdateNow = 0x100,
        //    EraseNow = 0x200,
        //    Frame = 0x400,
        //    NoFrame = 0x800
        //}

        
        //const int WM_FONTCHANGE = 0x1D;
        //const int WM_SETFONT = 0x30;
        //const int WM_ERASEBKGND = 0x14;
        //const int WM_PAINT = 0xF;
        //const int WM_NC_HITTEST = 0x84;
        //const int WM_NC_PAINT = 0x85;
        //const int WM_PRINTCLIENT = 0x318;
        //const int WM_SETCURSOR = 0x20;
        //const int HTBORDER = 18;

        //protected override CreateParams CreateParams {
        //    get {
        //        if ( DesignMode ) {
        //            return base.CreateParams;
        //        }
        //        CreateParams cp = base.CreateParams;
        //        cp.ExStyle &= ( ~0x00000200 ); // WS_EX_CLIENTEDGE
        //        cp.Style |= 0x00800000; // WS_BORDER
        //        return cp;
        //    }
        //}

        //// During OnResize, call RedrawWindow with Frame|UpdateNow|Invalidate so that the frame is always redrawn accordingly
        //protected override void OnResize( EventArgs e ) {
        //    base.OnResize( e );
        //    if ( DesignMode ) {
        //        RecreateHandle();
        //    }
        //    RedrawWindow( this.Handle, IntPtr.Zero, IntPtr.Zero, RedrawWindowFlags.Frame | RedrawWindowFlags.UpdateNow | RedrawWindowFlags.Invalidate );
        //}

        const int WM_CREATE = 1;
        const int WM_MOVE = 3;
        const int WM_SIZE = 5;
        const int WM_GETTEXT = 13;
        const int WM_GETTEXTLENGTH = 14;
        const int WM_PAINT = 15;
        const int WM_ERASEBKGND = 20;
        const int WM_SHOWWINDOW = 24;
        
        const int WM_STYLECHANGING = 124;
        const int WM_STYLECHANGED = 125;
        const int WM_NCCREATE = 129;
        const int WM_NCPAINT = 133;
        const int WM_NCCALCSIZE = 131;
        const int OCM_PARENTNOTIFY = 8720;

        const int LVM_GETIMAGELIST = 4098;
        const int LVM_GETITEMCOUNT = 4100;
        const int LVM_SETITEMA = 4102;
        const int LVM_DELETEALLITEMS = 4105;
        const int LVM_GETSELECTEDCOUNT = 4146;
        
        const int WM_WINDOWPOSCHANGING = 70;
        const int WM_WINDOWPOSCHANGED = 71;

        const int WM_NCHITTEST = 132;
        const int WM_SETCURSOR = 32;

        const int WM_TIMER = 0x0113; //275


        protected override void WndProc( ref Message m ) {
            switch ( m.Msg ) {
                case WM_CREATE:
                case WM_MOVE:
                case WM_SIZE:
                case WM_GETTEXT:
                case WM_GETTEXTLENGTH:
                case WM_PAINT:
                case WM_ERASEBKGND:
                case WM_SHOWWINDOW:
        
                case WM_STYLECHANGING:
                case WM_STYLECHANGED:
                case WM_NCCREATE:
                case WM_NCPAINT:
                case WM_NCCALCSIZE:
                case LVM_GETIMAGELIST:
                case LVM_GETITEMCOUNT:
                case LVM_SETITEMA:
                case LVM_DELETEALLITEMS:
                case LVM_GETSELECTEDCOUNT:
        
                case WM_WINDOWPOSCHANGING:
                case WM_WINDOWPOSCHANGED:
                case OCM_PARENTNOTIFY:
                    base.WndProc( ref m );
                    return;
            }

            Console.WriteLine( m.Msg );
            base.WndProc( ref m );
        }
    }
}
