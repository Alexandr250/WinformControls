using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using WinformControls.Common;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;

namespace WinformControls.AxControls {
    

    public class AxForm : Form {
        //This gives us the ability to drag the borderless form to a new location
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImport( "user32.dll" )]
        public static extern int SendMessage( IntPtr hWnd, int Msg, int wParam, int lParam );
        [DllImport( "user32.dll" )]
        public static extern bool ReleaseCapture();

        private enum ButtonType {
            Close,
            Maximize,
            Hide
        }
        
        private const int MIN_HEADER_HEIGHT = 30;
        private const int BUTTON_HEIGHT = 24;
        private const int BUTTON_WIDTH = 30;
        private const int BUTTON_SPACE = 3;
        private int _borderWidth = 5;

        private StateStyle _normalStyle;
        private StateStyle _normalHeaderStyle;
        private StateStyle _normalButtonStyle;
        private StateStyle _hoverButtonStyle;

        private int _headerHeight = MIN_HEADER_HEIGHT;
        private StringAlignment _headerHorizontalAlignment = StringAlignment.Near;
        private int _x;
        private int _y;

        //This gives us the drop shadow behind the borderless form
        private const int CS_DROPSHADOW = 0x20000;
        protected override CreateParams CreateParams {
            get {
                CreateParams cp = base.CreateParams;
                cp.ClassStyle |= CS_DROPSHADOW;
                return cp;
            }
        }

        public StringAlignment HeaderHorizontalAlignment {
            get {
                return _headerHorizontalAlignment;
            }
            set {
                if ( _headerHorizontalAlignment != value ) {
                    _headerHorizontalAlignment = value;
                    Refresh();
                }
            }
        }

        public int BorderWidth {
            get {
                return _borderWidth;
            }
            set {
                if ( value != _borderWidth ) {
                    _borderWidth = value;
                    Refresh();
                }
            }
        }

        public int HeaderHeight {
            get {
                return _headerHeight;
            }
            set {
                if ( value < MIN_HEADER_HEIGHT )
                    _headerHeight = MIN_HEADER_HEIGHT;
                else
                    _headerHeight = value;

                Padding = new Padding( this.Padding.Left, _headerHeight, this.Padding.Right, this.Padding.Bottom );

                Refresh();
            }
        }

        [Browsable( true )]
        [TypeConverter( typeof( ExpandableObjectConverter ) )]
        [Category( "Visual" )]
        [DesignerSerializationVisibility( DesignerSerializationVisibility.Content )]
        public StateStyle NormalStyle {
            get {
                return _normalStyle;
            }
            set {
                if ( _normalStyle != null )
                    _normalStyle.Dispose();

                _normalStyle = value;

                if ( value != null )
                    _normalStyle.PropertyChanged += ( propertyName ) => this.Refresh();

                Refresh();
            }
        }

        
        [Browsable( true )]
        [TypeConverter( typeof( ExpandableObjectConverter ) )]
        [DesignerSerializationVisibility( DesignerSerializationVisibility.Content )]
        [Category( "Visual" )]
        [ComVisible( true )]
        public StateStyle HeaderStyle {
            get {
                return _normalHeaderStyle;
            }
            set {
                if ( _normalHeaderStyle != null )
                    _normalHeaderStyle.Dispose();

                _normalHeaderStyle = value;

                if ( value != null )
                    _normalHeaderStyle.PropertyChanged += ( propertyName ) => this.Refresh();

                Refresh();
            }
        }

        [Browsable( true )]
        [TypeConverter( typeof( ExpandableObjectConverter ) )]
        [Category( "Visual" )]
        [DesignerSerializationVisibility( DesignerSerializationVisibility.Content )]
        [ComVisible( true )]
        public StateStyle NormalButtonStyle {
            get {
                return _normalButtonStyle;
            }
            set {
                if ( _normalButtonStyle != null )
                    _normalButtonStyle.Dispose();

                _normalButtonStyle = value;

                if ( value != null )
                    _normalButtonStyle.PropertyChanged += ( propertyName ) => this.Refresh();

                Refresh();
            }
        }

        [Browsable( true )]
        [TypeConverter( typeof( ExpandableObjectConverter ) )]
        [Category( "Visual" )]
        [DesignerSerializationVisibility( DesignerSerializationVisibility.Content )]
        public StateStyle HoverButtonStyle {
            get {
                return _hoverButtonStyle;
            }
            set {
                if ( _hoverButtonStyle != null )
                    _hoverButtonStyle.Dispose();

                _hoverButtonStyle = value;

                if ( value != null )
                    _hoverButtonStyle.PropertyChanged += ( propertyName ) => this.Refresh();

                Refresh();
            }
        }

        #region [ Identity ]
        public AxForm() {
            FormBorderStyle = FormBorderStyle.None;
            Padding = new Padding( Padding.Left, _headerHeight, Padding.Right, Padding.Bottom );

            _normalStyle = new StateStyle() {
                BackColor = Color.White,
                ForeColor = Color.Black,
                BorderColor = Color.Gray
            };
            _normalStyle.PropertyChanged += ( propertyName ) => Refresh();

            _normalHeaderStyle = new StateStyle() {
                BackColor = Color.White,
                ForeColor = Color.Black,
                BorderColor = Color.Gray
            };
            _normalHeaderStyle.PropertyChanged += ( propertyName ) => Refresh();

            _normalButtonStyle = new StateStyle() {
                BackColor = Color.White,
                ForeColor = Color.Black,
                BorderColor = Color.Gray
            };
            _normalButtonStyle.PropertyChanged += ( propertyName ) => Refresh();

            _hoverButtonStyle = new StateStyle() {
                BackColor = Color.White,
                ForeColor = Color.Black,
                BorderColor = Color.Gray
            };
            _hoverButtonStyle.PropertyChanged += ( propertyName ) => Refresh();

        }
        #endregion

        private Rectangle GetHeaderRectangle() {
            return new Rectangle( ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width - 1, HeaderHeight );
        }

        private Rectangle GetCloseButtonRectangle() {
            Rectangle headerRect = GetHeaderRectangle();
            return new Rectangle(
                headerRect.Right - Padding.Right - BUTTON_WIDTH,
                headerRect.Bottom / 2 - BUTTON_HEIGHT / 2, 
                BUTTON_WIDTH, 
                BUTTON_HEIGHT 
                );
        }

        private Rectangle GetMaximizeButtonRectangle() {
            Rectangle headerRect = GetHeaderRectangle();
            return new Rectangle(
                headerRect.Right - Padding.Right - BUTTON_WIDTH - BUTTON_SPACE - BUTTON_WIDTH,
                headerRect.Bottom / 2 - BUTTON_HEIGHT / 2,
                BUTTON_WIDTH,
                BUTTON_HEIGHT
                );
        }

        private Rectangle GetHideButtonRectangle() {
            Rectangle headerRect = GetHeaderRectangle();
            return new Rectangle(
                headerRect.Right - Padding.Right - BUTTON_WIDTH - BUTTON_SPACE - BUTTON_WIDTH - BUTTON_SPACE - BUTTON_WIDTH,
                headerRect.Bottom / 2 - BUTTON_HEIGHT / 2,
                BUTTON_WIDTH,
                BUTTON_HEIGHT
                );
        }

        private Rectangle GetIconRectangle() {
            Rectangle headerRect = GetHeaderRectangle();
            return new Rectangle( headerRect.X + Padding.Left, headerRect.Height / 2 - 8, 16, 16 );
        }

        protected override void OnPaint( PaintEventArgs e ) {
            Graphics graphics = e.Graphics;
            Rectangle formRect = new Rectangle( ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width - 1, ClientRectangle.Height - 1 );
            Rectangle headerRect = new Rectangle( ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width - 1, HeaderHeight );

            graphics.Clear( NormalStyle.BackColor );            

            graphics.FillRectangle( new SolidBrush( HeaderStyle.BackColor ), headerRect );

            // иконка формы
            Rectangle iconRectangle = GetIconRectangle();
            graphics.DrawImage( Icon.ToBitmap(), GetIconRectangle() );
            
            
            StringFormat format = new StringFormat();
            format.LineAlignment = StringAlignment.Center;
            format.Alignment = HeaderHorizontalAlignment;

            // заголовок формы
            Rectangle captionRect = new Rectangle( headerRect.X + iconRectangle.Width + Padding.Left, headerRect.Y, ClientRectangle.Width - 1 - iconRectangle.Width - Padding.Horizontal, HeaderHeight );
            graphics.DrawString( Text, Font, new SolidBrush( HeaderStyle.ForeColor ), captionRect, format );

            

            // кнопки закрытия
            DrawButton( graphics, GetCloseButtonRectangle(), ButtonType.Close );
            // кнопки максимизации
            DrawButton( graphics, GetMaximizeButtonRectangle(), ButtonType.Maximize );
            // кнопки сворачивания
            DrawButton( graphics, GetHideButtonRectangle(), ButtonType.Hide );
            
            // обводка заголовка
            graphics.DrawLine( new Pen( HeaderStyle.BorderColor ), headerRect.Left, headerRect.Bottom, headerRect.Right, headerRect.Bottom );

            // обводка формы
            graphics.DrawRectangle( new Pen( NormalStyle.BorderColor ), formRect );
        }

        private void DrawButton( Graphics graphics, Rectangle buttonRectangle, ButtonType buttonType ) {

            graphics.FillRectangle( new SolidBrush( NormalButtonStyle.BackColor ), buttonRectangle );

            graphics.SmoothingMode = SmoothingMode.HighQuality;

            if ( buttonType == ButtonType.Close ) {
                graphics.DrawLine( new Pen( NormalButtonStyle.ForeColor ),
                    buttonRectangle.Left + buttonRectangle.Width / 2 - 4,
                    buttonRectangle.Top + buttonRectangle.Height / 2 - 4,
                    buttonRectangle.Left + buttonRectangle.Width / 2 + 4,
                    buttonRectangle.Top + buttonRectangle.Height / 2 + 4 );
                graphics.DrawLine( new Pen( NormalButtonStyle.ForeColor ),
                    buttonRectangle.Left + buttonRectangle.Width / 2 + 4,
                    buttonRectangle.Top + buttonRectangle.Height / 2 - 4,
                    buttonRectangle.Left + buttonRectangle.Width / 2 - 4,
                    buttonRectangle.Top + buttonRectangle.Height / 2 + 4 );
            }
            if ( buttonType == ButtonType.Maximize ) {
                graphics.DrawRectangle( new Pen( NormalButtonStyle.ForeColor ),
                    buttonRectangle.Left + buttonRectangle.Width / 2 - 4,
                    buttonRectangle.Top + buttonRectangle.Height / 2 - 4,
                    8,
                    8 );
            }

            if ( buttonType == ButtonType.Hide ) {
                graphics.DrawLine( new Pen( NormalButtonStyle.ForeColor ),
                    buttonRectangle.Left + buttonRectangle.Width / 2 - 4,
                    buttonRectangle.Top + buttonRectangle.Height / 2 + 4,
                    buttonRectangle.Left + buttonRectangle.Width / 2 + 4,
                    buttonRectangle.Top + buttonRectangle.Height / 2 + 4 );
            }

            graphics.SmoothingMode = SmoothingMode.HighSpeed;

            graphics.DrawRectangle( new Pen( NormalButtonStyle.BorderColor ), buttonRectangle );
        }

        protected override void OnMouseDown( MouseEventArgs e ) {            
            base.OnMouseDown( e );
            //_x = e.X;
            //_y = e.Y;
            //if ( e.Button == MouseButtons.Left && ModifierKeys == Keys.Control ) {
            //    ReleaseCapture();
            //    SendMessage( Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0 );
            //}

            if ( GetCloseButtonRectangle().Contains( e.Location ) )
                this.Close();
        }
        
        //protected virtual void OnMouseEnter( EventArgs e );
        
        //protected virtual void OnMouseHover( EventArgs e );
        
        //protected virtual void OnMouseLeave( EventArgs e );

        protected override void OnMouseMove( MouseEventArgs e ) {
            base.OnMouseMove( e );
            //if ( e.Button == MouseButtons.Left && GetHeaderRectangle().Contains( _x, _y ) ) {                
            //    Left = Left + e.X - _x;
            //    Top = Top + e.Y - _y;
            //}
            //if ( e.Button == MouseButtons.Left && e.X > ClientRectangle.Width - 10 ) {
            //    Width = Width + e.X - _x;
            //    _x = e.X;
            //    Refresh();
            //}            
        }

        protected override void OnResize( EventArgs e ) {
            base.OnResize(e);

            Refresh();
        }

        protected override void WndProc( ref Message m ) {
            //const int RESIZE_HANDLE_SIZE = 10;

            switch ( m.Msg ) {
                case 0x0084/*NCHITTEST*/ :
                    base.WndProc( ref m );

                    if ( ( int )m.Result == 0x01/*HTCLIENT*/) {
                        Point screenPoint = new Point( m.LParam.ToInt32() );
                        Point clientPoint = PointToClient( screenPoint );


                        if ( clientPoint.Y <= BorderWidth ) {
                            if ( clientPoint.X <= BorderWidth )
                                m.Result = ( IntPtr )13/*HTTOPLEFT*/ ;
                            else if ( clientPoint.X < ( Size.Width - BorderWidth ) )
                                m.Result = ( IntPtr )12/*HTTOP*/ ;
                            else
                                m.Result = ( IntPtr )14/*HTTOPRIGHT*/ ;
                        }
                        else if ( clientPoint.Y <= ( Size.Height - BorderWidth ) ) {
                            if ( clientPoint.X <= BorderWidth )
                                m.Result = ( IntPtr )10/*HTLEFT*/ ;
                            else if ( clientPoint.X < ( Size.Width - BorderWidth ) )
                                m.Result = ( IntPtr )2/*HTCAPTION*/ ;
                            else
                                m.Result = ( IntPtr )11/*HTRIGHT*/ ;
                        }
                        else {
                            if ( clientPoint.X <= BorderWidth )
                                m.Result = ( IntPtr )16/*HTBOTTOMLEFT*/ ;
                            else if ( clientPoint.X < ( Size.Width - BorderWidth ) )
                                m.Result = ( IntPtr )15/*HTBOTTOM*/ ;
                            else
                                m.Result = ( IntPtr )17/*HTBOTTOMRIGHT*/ ;
                        }
                    }
                    return;
            }
            base.WndProc( ref m );
        }        
    }
}
