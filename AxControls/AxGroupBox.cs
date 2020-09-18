using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;
using WinformControls.Helpers;
using WinformControls.Common;

namespace WinformControls {
    public class AxGroupBox : GroupBox {        
        private int _borderRadius = 0;
        private int _headerPadding = 3;
        private int _headerGradientLightenValue = 20;
        private Color _headerBackColor;
        private Color _backColor;
        private Color _borderColor;
        private Color _borderHoverColor;
        private ContentPosition _headerPosition = ContentPosition.Top;
        private bool _needRepaintBackground;
        private bool _needRepaintHeaders;
        private bool _isSelected = false;
        private bool _headerFlatStyle = true;
        private bool _showHeader = true;

        private Bitmap _normalHeader;
        private Bitmap _hoverHeader;
        private SizeF _headerSize;
        private Rectangle _backgroundRectangle;
        private GraphicsPath _backgroundPath;
        private Brush _backgroundBrush;
        private Pen _borderPen;
        private Pen _borderHoverPen;


        public ContentPosition HeaderPosition {
            get {
                return _headerPosition;
            }
            set {
                _headerPosition = value;
                _needRepaintBackground = true;
                _needRepaintHeaders = true;
                Refresh();
            }
        }

        public int HeaderGradientLightenValue {
            get {
                return _headerGradientLightenValue;
            }
            set {
                if ( value < 0 )
                    value = 0;
                else if ( value > 255 )
                    value = 255;

                _headerGradientLightenValue = value;
                _needRepaintBackground = true;
                _needRepaintHeaders = true;
                Refresh();
            }
        }

        public int BorderRadius {
            get {
                return _borderRadius;
            }
            set {
                // Значение пока не устанавливается тк лениво перерисовывать Background  в заголовке при BorderRadius > 0
                //_borderRadius = value;
                Refresh();
            }
        }

        public bool HeaderFlatStyle {
            get {
                return _headerFlatStyle;
            }
            set {
                _headerFlatStyle = value;
                _needRepaintBackground = true;
                _needRepaintHeaders = true;
                Refresh();
            }
        }

        public bool ShowHeader {
            get {
                return _showHeader;
            }
            set {
                _showHeader = value;
                _needRepaintBackground = true;
                _needRepaintHeaders = true;
                Refresh();
            }
        }
        
        public int HeaderPadding {
            get {
                return _headerPadding;
            }

            set {
                _headerPadding = value;

                Graphics g = Graphics.FromHwnd( this.Handle );
                SizeF headerSize = g.MeasureString(
                    Text,
                    this.Font,
                    ClientRectangle.Width + BorderThickness * 2 + HeaderPadding * 2 );

                this.Padding = new Padding( this.Padding.Left, ( int ) headerSize.Height + BorderThickness * 2 + HeaderPadding * 2, this.Padding.Right, this.Padding.Bottom );

                Refresh();
            }
        }

        public int BorderThickness { get; set; }

        public Color BorderColor {
            get {
                return _borderColor;
            }
            set {
                if ( _borderPen != null )
                    _borderPen.Dispose();

                _borderColor = value;

                _borderPen = new Pen( _borderColor, BorderThickness );
                _needRepaintBackground = true;

                Refresh();
            }
        }
        public Color BorderHoverColor {
            get {
                return _borderHoverColor;
            }
            set {
                if ( _borderHoverPen != null )
                    _borderHoverPen.Dispose();

                _borderHoverColor = value;

                _borderHoverPen = new Pen( _borderHoverColor, BorderThickness );
                _needRepaintBackground = true;

                Refresh();
            }
        }

        public Color HeaderBackColor {
            get {
                return _headerBackColor;
            }
            set {
                _headerBackColor = value;
                _needRepaintBackground = true;
                _needRepaintHeaders = true;
                Refresh();
            }
        }
        public Color HeaderHoverColor { get; set; }

        public bool ShowBottomBorder { get; set; }

        public override Color BackColor {
            get {
                return _backColor;
            }
            set {
                if ( _backgroundBrush != null )
                    _backgroundBrush.Dispose();

                _backColor = value;

                _backgroundBrush = new SolidBrush( _backColor );
                _needRepaintBackground = true;

                Refresh();
            }
        }

        public bool Hovered { get; set; }

        public AxGroupBox() {
            this.SetStyle( ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true );

            Hovered = true;
            BorderThickness = 1;

            _backColor = Color.White;
            _backgroundBrush = new SolidBrush( BackColor );

            _borderColor = Color.Black;
            _borderPen = new Pen( _borderColor, BorderThickness );

            _borderHoverColor = Color.Black;
            _borderHoverPen = new Pen( _borderHoverColor, BorderThickness );

            _needRepaintBackground = true;            
            
            ShowBottomBorder = true;
        }

        protected override void OnPaint( PaintEventArgs e ) {
#if DEBUG
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
#endif
            RepaintGroupBox( e.Graphics );

#if DEBUG
            stopwatch.Stop();
            Console.WriteLine( string.Format( "GroupBox repaint : {0} (ticks)", stopwatch.ElapsedTicks ) );
#endif
        }        

        private void RepaintGroupBox( Graphics g ) {
            if ( BorderRadius > 0 )
                g.SmoothingMode = SmoothingMode.AntiAlias;

            // Заливка фона с закрашиванием углов
            // Отключена тк отключен BorderRadius
            //if ( Parent != null || ( BackColor == null || BackColor == Color.Transparent ) )
            //    g.FillRectangle( new SolidBrush( Parent.BackColor ), ClientRectangle );

            // Заливка фона
            if ( _needRepaintBackground ) {
                RepaintBackground( g );
            }

            // Рисование заголовка
            RepaintHeader( g );

            // Рисование границы
            if ( BorderThickness > 0 && ( ( _isSelected ) ? _borderHoverPen : _borderPen ) != null )
                g.DrawPath( ( _isSelected ) ? _borderHoverPen : _borderPen, _backgroundPath );
        }

        private void RepaintBackground( Graphics g ) {
            _needRepaintBackground = false;

            _backgroundRectangle = new Rectangle( ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width - BorderThickness, ClientRectangle.Height - BorderThickness );
            _backgroundPath = ExtendedForms.RoundedRect( _backgroundRectangle, BorderRadius );
            g.FillPath( _backgroundBrush, _backgroundPath );
        }

        private void RepaintHeader( Graphics g ) {
            if ( _normalHeader == null || _hoverHeader == null || _needRepaintHeaders )
                UpdateHeaders( g );

            _needRepaintHeaders = false;
            g.DrawImage( ( _isSelected ) ? _hoverHeader : _normalHeader, 0, 0 );
        }

        private void UpdateHeaders( Graphics g ) {
            _headerSize = g.MeasureString( Text, this.Font, ClientRectangle.Width + BorderThickness * 2 + HeaderPadding * 2 );

            CreateHeader( g, false );
            CreateHeader( g, true );        
        }

        private void CreateHeader( Graphics g, bool hovered ) {
            Rectangle headerRectangle = GetHeaderRectangle( ( int ) _headerSize.Height );
            if ( headerRectangle.Width <= 0 || headerRectangle.Height <= 0 )
                return;

            Bitmap header = new Bitmap( headerRectangle.Width, headerRectangle.Height, PixelFormat.Format24bppRgb );

            DrawHeader( header, BorderColor, ( hovered ) ? HeaderHoverColor : HeaderBackColor, headerRectangle );

            switch ( HeaderPosition ) {
                case ContentPosition.Top:
                    break;
                case ContentPosition.Bottom:
                    headerRectangle.Y = ClientRectangle.Height - headerRectangle.Height;
                    break;
                case ContentPosition.Left:
                    _normalHeader = header;
                    _normalHeader.RotateFlip( RotateFlipType.Rotate270FlipNone );
                    break;
                case ContentPosition.Right:
                    _normalHeader.RotateFlip( RotateFlipType.Rotate90FlipNone );
                    headerRectangle.X = ClientRectangle.Width - headerRectangle.Height;
                    break;
                default:
                    break;
            }

            if ( hovered ) {
                if ( _hoverHeader != null )
                    _hoverHeader.Dispose();
                _hoverHeader = header;

            }
            else {
                if ( _normalHeader != null )
                    _normalHeader.Dispose(); 
                _normalHeader = header;
            }
        }

        private void DrawHeader( Bitmap headerBitmap, Color headerBorderColor, Color headerBackgroundColor, Rectangle headerRectangle ) {
            Graphics innerGraphics = Graphics.FromImage( headerBitmap );

            if ( HeaderFlatStyle ) {
                innerGraphics.FillPath( new SolidBrush( headerBackgroundColor ), ExtendedForms.RoundedRect( headerRectangle, BorderRadius ) );
            }
            else {
                LinearGradientBrush brush = new LinearGradientBrush( headerRectangle, headerBackgroundColor.Lighten( HeaderGradientLightenValue ), headerBackgroundColor, LinearGradientMode.Vertical );
                innerGraphics.FillPath( brush, ExtendedForms.RoundedRect( headerRectangle, BorderRadius ) );
            }
            innerGraphics.DrawPath( new Pen( headerBorderColor, BorderThickness ), ExtendedForms.RoundedRect( headerRectangle, BorderRadius ) );

            DrawString( innerGraphics, headerRectangle );

            if ( ShowBottomBorder ) {
                innerGraphics.DrawLine( 
                    new Pen( headerBackgroundColor.Lighten( 30 ) ), 
                    headerRectangle.X, 
                    headerRectangle.Y + 1, 
                    headerRectangle.Right, 
                    headerRectangle.Y + 1 );
                innerGraphics.DrawLine( 
                    new Pen( headerBackgroundColor.Darken( 30 ) ), 
                    headerRectangle.X, 
                    headerRectangle.Bottom - 1, 
                    headerRectangle.Right, 
                    headerRectangle.Bottom - 1 );
            }

            Graphics g = Graphics.FromHwnd( this.Handle );
            g.DrawImage( headerBitmap, headerRectangle.Location );
        }

        private void DrawString( Graphics g, Rectangle headerRectangle ) {
            StringFormat stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Center;
            
            g.DrawString(
                Text,
                this.Font,
                new SolidBrush( ForeColor ),
                headerRectangle,
                stringFormat );
        }

        private Rectangle GetHeaderRectangle( int textHeight ) {
            Rectangle headerRectangle;

            switch ( HeaderPosition ) {
                case ContentPosition.Top:
                    headerRectangle = new Rectangle( 0, 0, ClientRectangle.Width - BorderThickness, textHeight + BorderThickness * 2 + HeaderPadding * 2 );
                    break;
                case ContentPosition.Bottom:
                    headerRectangle = new Rectangle( 0, 0, ClientRectangle.Width, textHeight + BorderThickness * 2 + HeaderPadding * 2 );
                    break;
                case ContentPosition.Left:
                    headerRectangle = new Rectangle( 0, 0, ClientRectangle.Height, textHeight + BorderThickness * 2 + HeaderPadding * 2 );
                    break;
                case ContentPosition.Right:
                    headerRectangle = new Rectangle( 0, 0, ClientRectangle.Height - BorderThickness, textHeight + BorderThickness * 2 + HeaderPadding * 2 );
                    break;
                default:
                    headerRectangle = new Rectangle( 0, 0, ClientRectangle.Width - BorderThickness, textHeight + BorderThickness * 2 + HeaderPadding * 2 );
                    break;
            }

            return headerRectangle;
        }
        
        private void InitializeComponent() {
            this.SuspendLayout();
            this.ResumeLayout(false);
        }

        private void Activate() {
            if ( !Hovered )
                return;

            _isSelected = true;
            Refresh();
        }

        private void Deactivate() {
            if ( !Hovered )
                return;

            if ( !ClientRectangle.Contains( PointToClient( Cursor.Position ) ) )
                _isSelected = false;
            Refresh();
        }

        protected override void OnMouseEnter( EventArgs e ) {
            base.OnMouseEnter( e );
            Activate();
        }

        protected override void OnMouseLeave( EventArgs e ) {
            base.OnMouseLeave( e );
            Deactivate();
        }

        protected override void OnControlAdded( ControlEventArgs e ) {
            base.OnControlAdded( e );
            e.Control.MouseEnter += ( g, f ) => Activate();
            e.Control.MouseLeave += ( g, f ) => Deactivate();
        }

        protected override void OnControlRemoved( ControlEventArgs e ) {
            base.OnControlRemoved( e );
            e.Control.MouseEnter -= ( g, f ) => Activate();
            e.Control.MouseLeave -= ( g, f ) => Deactivate();
        }

        protected override void OnResize( EventArgs e ) {
            base.OnResize( e );
            _needRepaintBackground = true;
            _needRepaintHeaders = true;
            Refresh();
        }
    }
}
