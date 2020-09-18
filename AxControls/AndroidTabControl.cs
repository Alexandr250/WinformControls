using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace WinformControls {
    public class AndroidTabControl : TabControl {
        private Brush inactiveTabBrush = Brushes.DimGray;
        private Brush activeTabBrush = Brushes.Gray;
        private Brush selectedTabBrush = Brushes.White;

        private Pen inactiveTabPen;
        private Pen selectedTabPen = Pens.White;
        private Pen activeTabBorder = Pens.DimGray;

        private int _hotTabIndex = -1;

        private Color headerColor = Color.DimGray;
        private bool allCaps;
        

        public Color HeaderColor {
            get {
                return headerColor;
            }
            set {
                headerColor = value;
                Refresh();
            }
        }

        public bool AllCaps {
            get {
                return allCaps;
            }
            set {
                allCaps = value;
                Refresh();
            }
        }

        
        private int HotTabIndex {
            get { return _hotTabIndex; }
            set {
                if ( _hotTabIndex != value ) {
                    _hotTabIndex = value;
                    this.Invalidate();
                }
            }
        }

        private int CloseButtonHeight {
            get { return FontHeight; }
        }

        

        public AndroidTabControl() {
            this.SetStyle( ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true );
            inactiveTabPen = new Pen( Color.FromArgb( 150, selectedTabPen.Color ) );
        }

        protected override void OnPaint( PaintEventArgs e ) {
            base.OnPaint( e );

            for ( int id = 0 ; id < this.TabCount ; id++ )
                DrawTabContent( e.Graphics, id );
        }


        protected override void OnPaintBackground( PaintEventArgs e ) {

            base.OnPaintBackground( e );
            //pevent.Graphics.DrawLine( Pens.White, 0, 0, ClientRectangle.Right, ClientRectangle.Top );
            //e.Graphics.DrawLine( activeTabBorder, 0, ItemSize.Height + 2, ClientRectangle.Right - 1, ItemSize.Height + 2 );
            e.Graphics.FillRectangle( new SolidBrush( headerColor ), e.Graphics.ClipBounds );
            for ( int id = 0 ; id < this.TabCount ; id++ )
                DrawTabBackground( e.Graphics, id );
        }

        protected override void OnCreateControl() {
            base.OnCreateControl();
            this.OnFontChanged( EventArgs.Empty );
        }

        protected override void OnFontChanged( EventArgs e ) {
            base.OnFontChanged( e );
            IntPtr hFont = this.Font.ToHfont();
            SendMessage( this.Handle, WM_SETFONT, hFont, new IntPtr( -1 ) );
            SendMessage( this.Handle, WM_FONTCHANGE, IntPtr.Zero, IntPtr.Zero );
            this.UpdateStyles();
        }


        private void DrawTabContent( Graphics graphics, int id ) {
            bool selectedOrHot = id == this.SelectedIndex || id == this.HotTabIndex;
            bool vertical = this.Alignment >= TabAlignment.Left;

            Image tabImage = null;

            if ( this.ImageList != null ) {
                TabPage page = this.TabPages[ id ];
                if ( page.ImageIndex > -1 && page.ImageIndex < this.ImageList.Images.Count )
                    tabImage = this.ImageList.Images[ page.ImageIndex ];

                if ( page.ImageKey.Length > 0 && this.ImageList.Images.ContainsKey( page.ImageKey ) )
                    tabImage = this.ImageList.Images[ page.ImageKey ];
            }

            Rectangle tabRect = GetTabRect( id );
            Rectangle contentRect = vertical ? new Rectangle( 0, 0, tabRect.Height, tabRect.Width ) : new Rectangle( Point.Empty, tabRect.Size );

            contentRect = new Rectangle( ClientRectangle.X, ClientRectangle.Y, /*ItemSize.Width*/ tabRect.Width, ItemSize.Height );
            Rectangle textrect = contentRect;

            //textrect.Width -= FontHeight;

            if ( tabImage != null ) {
                textrect.Width -= tabImage.Width;
                textrect.X += tabImage.Width;
            }


            Color frColor = id == SelectedIndex ? selectedTabPen.Color : inactiveTabPen.Color;// this.ForeColor;
            Color bkColor = id == SelectedIndex ? inactiveTabPen.Color : activeTabBorder.Color;//FindForm().BackColor;

            using ( Bitmap bm = new Bitmap( contentRect.Width, contentRect.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb ) ) {
                using ( Graphics bmGraphics = Graphics.FromImage( bm ) ) {
                    //TextRenderer.DrawText( bmGraphics, this.TabPages[ id ].Text, this.Font, textrect, frColor, bkColor );
                    //StringFormat sf = new StringFormat();
                    //sf.LineAlignment = StringAlignment.Center;
                    //sf.Alignment = StringAlignment.Center;
                    //sf.FormatFlags = StringFormatFlags.NoWrap;

                    //bmGraphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                    //bmGraphics.DrawString( this.TabPages[ id ].Text, this.Font, new SolidBrush( frColor ), textrect, sf );
                    //if (selectedOrHot)
                    //{
                    //Rectangle closeRect = new Rectangle( contentRect.Right - CloseButtonHeight, 0, CloseButtonHeight, CloseButtonHeight );
                    //closeRect.Offset( -2, ( contentRect.Height - closeRect.Height ) / 2 );
                    //DrawCloseButton( bmGraphics, closeRect );
                    //}
                    if ( tabImage != null ) {
                        Rectangle imageRect = new Rectangle( Padding.X, 0, tabImage.Width, tabImage.Height );
                        imageRect.Offset( 0, ( contentRect.Height - imageRect.Height ) / 2 );
                        bmGraphics.DrawImage( tabImage, imageRect );
                    }
                }

                if ( vertical ) {
                    if ( this.Alignment == TabAlignment.Left )
                        bm.RotateFlip( RotateFlipType.Rotate270FlipNone );
                    else
                        bm.RotateFlip( RotateFlipType.Rotate90FlipNone );
                }
                graphics.DrawImage( bm, tabRect );

            }
        }

        private void DrawCloseButton( Graphics graphics, Rectangle bounds ) {
            //using (Font closeFont = new Font("Tahoma", Font.Size, FontStyle.Bold))
            //    TextRenderer.DrawText(graphics, "x", closeFont, bounds, Color.Gray, Color.Transparent, TextFormatFlags.HorizontalCenter | TextFormatFlags.NoPadding | TextFormatFlags.SingleLine | TextFormatFlags.VerticalCenter);

        }



        private void DrawTabBackground( Graphics graphics, int id ) {
            //graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            Color frColor = id == SelectedIndex ? selectedTabPen.Color : inactiveTabPen.Color;
            Rectangle rect = GetTabRect( id );

            if ( id == SelectedIndex ) {
                graphics.DrawLine( new Pen( selectedTabPen.Color, 2 ), rect.Left, rect.Bottom - 0, rect.Right, rect.Bottom - 0 );
            }
            else {  }

            StringFormat sf = new StringFormat();
            sf.LineAlignment = StringAlignment.Center;
            sf.Alignment = StringAlignment.Center;
            //sf.FormatFlags = StringFormatFlags.NoWrap;


            graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            graphics.DrawString( allCaps ? this.TabPages[ id ].Text.ToUpper() : this.TabPages[ id ].Text, this.Font, new SolidBrush( frColor ), rect, sf );

            if ( id == HotTabIndex && ( id != SelectedIndex ) ) { }


        }

        public static GraphicsPath RoundedRect( Rectangle bounds, int radius ) {
            int diameter = radius * 2;
            Size size = new Size( diameter, diameter );
            Rectangle arc = new Rectangle( bounds.Location, size );
            GraphicsPath path = new GraphicsPath();

            if ( radius == 0 ) {
                path.AddRectangle( bounds );
                return path;
            }

            path.AddLine( bounds.Left, bounds.Bottom, bounds.Left, bounds.Height / 2 );
            // top left arc  
            path.AddArc( arc, 180, 90 );

            // top right arc  
            arc.X = bounds.Right - diameter;
            path.AddArc( arc, 270, 90 );

            //// bottom right arc  
            //arc.Y = bounds.Bottom - diameter;
            //path.AddArc( arc, 0, 90 );

            //// bottom left arc 
            //arc.X = bounds.Left;
            //path.AddArc( arc, 90, 90 );

            path.AddLine( bounds.Right, bounds.Height / 2, bounds.Right, bounds.Bottom );

            path.CloseFigure();
            return path;
        }


        private const int TCM_ADJUSTRECT = 0x1328;//(TCM_FIRST + 40);
        //protected override void WndProc( ref Message m ) {
        //    if ( m.Msg == TCM_SETPADDING ) {
        //        m.LParam = MAKELPARAM( this.Padding.X /*+ CloseButtonHeight / 2*/, this.Padding.Y );
        //    }

        //    if ( m.Msg == WM_MOUSEDOWN && !this.DesignMode ) {
        //        Point pt = this.PointToClient( Cursor.Position );
        //        //Rectangle closeRect = GetCloseButtonRect( HotTabIndex );
        //        //if ( closeRect.Contains( pt ) && HotTabIndex != 0 ) {
        //        //    TabPages.RemoveAt( HotTabIndex );
        //        //    m.Msg = WM_NULL;
        //        //}
        //    }

        //    if ( m.Msg == TCM_ADJUSTRECT && !this.DesignMode ) {
        //        RECT rc = new RECT();
        //        rc.Left -= 3;
        //        rc.Right += 1;
        //        rc.Top -= 1;
        //        rc.Bottom += 1;
        //        Marshal.StructureToPtr( rc, m.LParam, true );
        //    }

        //    base.WndProc( ref m );
        //}
        private struct RECT {
            public int Left, Top, Right, Bottom;
        }

        protected override void OnMouseMove( MouseEventArgs e ) {
            base.OnMouseMove( e );
            TCHITTESTINFO HTI = new TCHITTESTINFO( e.X, e.Y );
            HotTabIndex = SendMessage( this.Handle, TCM_HITTEST, IntPtr.Zero, ref HTI );
        }

        protected override void OnMouseLeave( EventArgs e ) {
            base.OnMouseLeave( e );
            HotTabIndex = -1;
        }

        private IntPtr MAKELPARAM( int lo, int hi ) {
            return new IntPtr( ( hi << 16 ) | ( lo & 0xFFFF ) );
        }

        private Rectangle GetCloseButtonRect( int id ) {

            Rectangle tabRect = GetTabRect( id );
            Rectangle closeRect = new Rectangle( tabRect.Left, tabRect.Top, CloseButtonHeight, CloseButtonHeight );

            switch ( Alignment ) {
                case TabAlignment.Left:
                    closeRect.Offset( ( tabRect.Width - closeRect.Width ) / 2, 0 );
                    break;
                case TabAlignment.Right:
                    closeRect.Offset( ( tabRect.Width - closeRect.Width ) / 2, tabRect.Height - closeRect.Height );
                    break;
                default:
                    closeRect.Offset( tabRect.Width - closeRect.Width, ( tabRect.Height - closeRect.Height ) / 2 );
                    break;
            }

            closeRect = new Rectangle( 0, 0, 0, 0 );
            return closeRect;
        }

        #region Interop

        [DllImport( "user32.dll" )]
        private static extern IntPtr SendMessage( IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam );

        [DllImport( "user32.dll" )]
        private static extern int SendMessage( IntPtr hwnd, int msg, IntPtr wParam, ref TCHITTESTINFO lParam );

        [StructLayout( LayoutKind.Sequential )]
        private struct TCHITTESTINFO {
            public Point pt;
            public TCHITTESTFLAGS flags;
            public TCHITTESTINFO( int x, int y ) {
                pt = new Point( x, y );
                flags = TCHITTESTFLAGS.TCHT_NOWHERE;
            }
        }

        [Flags()]
        private enum TCHITTESTFLAGS {
            TCHT_NOWHERE = 1,
            TCHT_ONITEMICON = 2,
            TCHT_ONITEMLABEL = 4,
            TCHT_ONITEM = TCHT_ONITEMICON | TCHT_ONITEMLABEL
        }

        private const int WM_NULL = 0x0;
        private const int WM_SETFONT = 0x30;
        private const int WM_FONTCHANGE = 0x1D;
        private const int WM_MOUSEDOWN = 0x201;

        private const int TCM_FIRST = 0x1300;
        private const int TCM_HITTEST = TCM_FIRST + 13;
        private const int TCM_SETPADDING = TCM_FIRST + 43;
        

        #endregion
    }
}
