using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using WinformControls.Helpers;
using WinformControls.Common;
using System.Drawing.Text;
using System.Drawing.Drawing2D;

namespace WinformControls {
    public enum CharacterType {
        All = 0,
        Numbers
    }


    public partial class AxBorderTextBox : UserControl {
        private bool _isSelected = false;
        private bool _error = false;
        private int _borderPadding = 3;
        private int _borderRadius = 2;
        private string _hint;
        private string _caption;
        private Size _captionSize;
        private ContentPosition _captionPosition;

        private StateStyle _normalStyle;
        private StateStyle _hoverStyle;
        private StateStyle _errorStyle;
        private StateStyle _disabledStyle;
        private StateStyle _captionNormalStyle;

        public Image CaptionImage { get; set; }
        public Size CaptionImageSize { get; set; }
        public int CaptionFixedWidth { get; set; }

        public CharacterType CharsType { get; set; }
        public bool ShowUpDown { get; set; }
        public HorizontalAlignment CaptionTextAlignment { get; set; }

        public string Caption {
            get {
                return _caption;
            }
            set {
                _caption = value;
                _captionSize = GetCaptionSize();
                AxBorderTextBox_Resize( this, null );
                this.Refresh();
            }
        }

        public bool CaptionFlatStyle { get; set; }
        public bool CaptionShowBorder { get; set; }

        public ContentPosition CaptionPosition {
            get {
                return _captionPosition;
            }
            set {
                _captionPosition = value;
                _captionSize = GetCaptionSize();
                AxBorderTextBox_Resize( this, null );
                this.Refresh();
            }
        }

        [TypeConverter( typeof( ExpandableObjectConverter ) )]
        [Category("Visual")]
        public StateStyle NormalStyle {
            get {
                return _normalStyle;
            }
            set {
                if( _normalStyle != null )
                    _normalStyle.Dispose();

                _normalStyle = value;
                
                _normalStyle.PropertyChanged += ( propertyName ) => {
                    NormalStyleChanged();
                    Refresh();
                };
                
                NormalStyleChanged();
                Refresh();
            }
        }

        [TypeConverter( typeof( ExpandableObjectConverter ) )]
        [Category( "Visual" )]
        public StateStyle HoverStyle {
            get {
                return _hoverStyle;
            }
            set {
                if ( _hoverStyle != null )
                    _hoverStyle.Dispose();

                _hoverStyle = value;
                Refresh();
            }
        }

        [TypeConverter( typeof( ExpandableObjectConverter ) )]
        [Category( "Visual" )]
        public StateStyle ErrorStyle {
            get {
                return _errorStyle;
            }
            set {
                if ( _errorStyle != null )
                    _errorStyle.Dispose();

                _errorStyle = value;
                Refresh();
            }
        }

        [TypeConverter( typeof( ExpandableObjectConverter ) )]
        [Category( "Visual" )]
        public StateStyle DisabledStyle {
            get {
                return _disabledStyle;
            }
            set {
                if ( _disabledStyle != null )
                    _disabledStyle.Dispose();

                _disabledStyle = value;
                Refresh();
            }
        }

        [Browsable( true )]
        [TypeConverter( typeof( ExpandableObjectConverter ) )]
        [Category( "Visual" )]
        public StateStyle CaptionNormalStyle {
            get {
                return _captionNormalStyle;
            }
            set {
                if ( _captionNormalStyle != null )
                    _captionNormalStyle.Dispose();

                _captionNormalStyle = value;

                if ( value != null )
                    _captionNormalStyle.PropertyChanged += ( propertyName ) => this.Refresh();

                Refresh();
            }
        }

        public string Hint {
            get {
                return _hint;
            }
            set {
                _hint = value;
                if ( string.IsNullOrEmpty( Text ) && !string.IsNullOrEmpty( Hint ) ) {
                    WindowsNative.SendMessage( textBox1.Handle, WindowsNative.EM_SETCUEBANNER, 1, Hint );
                }
                textBox1.Refresh();
            }
        }

        public Color HintColor { get; set; }

        [Browsable( false )]
        public override Color BackColor {
            get {
                return _normalStyle.BackColor;
            }
            set {
                if ( _normalStyle != null )
                    _normalStyle.BackColor = value;
            }
        }

        [Browsable( false )]
        public override Color ForeColor {
            get {
                return _normalStyle.ForeColor;
            }
            set {
                _normalStyle.ForeColor = value;
            }
        }
        
        public Boolean Error {
            get {
                return _error;
            }
            set {
                if ( _error != value ) {
                    _error = value;
                    if ( !_error ) {
                        textBox1.ForeColor = NormalStyle.ForeColor;
                        textBox1.BackColor = NormalStyle.BackColor;
                    }
                    else {
                        textBox1.BackColor = ErrorStyle.BackColor;
                        textBox1.ForeColor = ErrorStyle.ForeColor;
                    }
                    this.Refresh();
                }
            }
        }
        
        public Boolean Multiline { 
            get {
                return textBox1.Multiline;
            }
            set {
                textBox1.Multiline = value;
            }
        }

        [Browsable( true )]
        public Boolean ReadOnly {
            get {
                return textBox1.ReadOnly;
            }
            set {
                textBox1.ReadOnly = value;
            }
        }

        [Browsable( true )]
        public new string Text {
            get {
                return textBox1.Text;
            }
            set {
                textBox1.Text = value;
            }
        }

        [Browsable( true )]
        public new Font Font {
            get {
                return textBox1.Font;
            }
            set {
                base.Font = value;
                textBox1.Font = value;
                AxBorderTextBox_Resize( this, null );
                Refresh();
            }
        }

        public int BorderPadding {
            get {
                return _borderPadding;
            }
            set {
                _borderPadding = value;
                this.Padding = new Padding( _borderPadding );
                AxBorderTextBox_Resize( this, null );
                Refresh();
            }
        }

        public int BorderRadius {
            get {
                return _borderRadius;
            }
            set {
                _borderRadius = value;
                Refresh();
            }
        } 
        
        public int BorderThickness { get; set; }

        [Browsable( true )]
        public event EventHandler TextChanged {
            add {
                textBox1.TextChanged += value;
            }
            remove {
                textBox1.TextChanged -= value;
            }
        }

        [Browsable( true )]
        public event EventHandler TextBoxEnter {
            add {
                textBox1.Enter += value;
            }
            remove {
                textBox1.Enter -= value;
            }
        }

        [Browsable( true )]
        public event EventHandler TextBoxLeave {
            add {
                textBox1.Leave += value;
            }
            remove {
                textBox1.Leave -= value;
            }
        }

        
        public AxBorderTextBox() {
            this.SetStyle( ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true );            

            InitializeComponent();

            //_captionPosition = ContentPosition.Left;

            CaptionImageSize = new Size( 16, 16 );
            CaptionTextAlignment = HorizontalAlignment.Center;

            _normalStyle = new StateStyle() {
                BackColor = Color.White,
                ForeColor = Color.Black,
                BorderColor = Color.Black
            };
            _normalStyle.PropertyChanged += ( propertyName ) => {
                NormalStyleChanged();
                Refresh();
            };

            _hoverStyle = new StateStyle() {
                BackColor = Color.White,
                ForeColor = Color.Black,
                BorderColor = Color.Black
            };

            _errorStyle = new StateStyle() {
                BackColor = Color.White,
                ForeColor = Color.Black,
                BorderColor = Color.Black
            };

            _disabledStyle = new StateStyle() {
                BackColor = Color.White,
                ForeColor = Color.Black,
                BorderColor = Color.Black
            };

            _captionNormalStyle = new StateStyle() {
                BackColor = Color.White,
                ForeColor = Color.Black,
                BorderColor = Color.Black
            };
            _captionNormalStyle.PropertyChanged += ( propertyName ) => {
                Refresh();
            };

            Error = false;      
            BorderThickness = 1;
            Height = textBox1.Size.Height + BorderPadding * 2;
            textBox1.Resize += ( s, e ) => AxBorderTextBox_Resize( s, e );

            textBox1.GotFocus += ( source, e ) => {
                if ( string.IsNullOrEmpty( Text ) ) {
                    WindowsNative.SendMessage( textBox1.Handle, WindowsNative.EM_SETCUEBANNER, 1, "" );
                }
            };

            textBox1.LostFocus += ( source, e ) => {
                if ( string.IsNullOrEmpty( Text ) && !string.IsNullOrEmpty( Hint ) ) {
                    WindowsNative.SendMessage( textBox1.Handle, WindowsNative.EM_SETCUEBANNER, 1, Hint );
                }
            };

            if ( !Error ) {
                textBox1.ForeColor = NormalStyle.ForeColor;
                textBox1.BackColor = NormalStyle.BackColor;
            }
            else {
                textBox1.BackColor = ErrorStyle.BackColor;
                textBox1.ForeColor = ErrorStyle.ForeColor;
            }

            _captionSize = GetCaptionSize();
        }

        protected override void OnPaint( PaintEventArgs e ) {
            Graphics g  = e.Graphics;

            StringFormat sf = new StringFormat();
            sf.LineAlignment = StringAlignment.Center;

            switch ( CaptionTextAlignment ) {
                case HorizontalAlignment.Center:
                    sf.Alignment = StringAlignment.Center;
                    break;
                case HorizontalAlignment.Left:
                    sf.Alignment = StringAlignment.Near;
                    break;
                case HorizontalAlignment.Right:
                    sf.Alignment = StringAlignment.Far;
                    break;
            }

            StateStyle style = NormalStyle;

            if ( !Error ) {
                if ( _isSelected )
                    style = HoverStyle;
            }
            else style = ErrorStyle;

            if ( !Enabled )
                style = DisabledStyle;

            if ( Parent != null )
                g.FillRectangle( new SolidBrush( Parent.BackColor ), ClientRectangle );

            g.SmoothingMode = SmoothingMode.HighQuality;
            
            Rectangle textRect = new Rectangle(
                        ClientRectangle.X,
                        ClientRectangle.Y,
                        ClientRectangle.Width - BorderThickness,
                        ClientRectangle.Height - BorderThickness );
            Rectangle captionRect = new Rectangle();

            GraphicsPath textPath = ExtendedForms.RoundedRect( textRect, BorderRadius );
            GraphicsPath captionPath = null;
            
            if ( _captionSize.Width > 0 ) {
                switch ( CaptionPosition ) {
                    case ContentPosition.Left:
                        captionRect = new Rectangle(
                            ClientRectangle.X,
                            ClientRectangle.Y,
                            _captionSize.Width + 2 * BorderPadding,
                            Height - BorderThickness );
                        textRect = new Rectangle(
                            captionRect.Width,
                            ClientRectangle.Y,
                            ClientRectangle.Width - BorderThickness - captionRect.Width,
                            ClientRectangle.Height - BorderThickness );

                        if ( CaptionShowBorder ) {
                            textPath = ExtendedForms.RoundedRightRect( textRect, BorderRadius );
                            captionPath = ExtendedForms.RoundedLeftRect( captionRect, BorderRadius );
                        }
                        else {
                            textPath = ExtendedForms.RoundedRect( textRect, BorderRadius );
                        }
                        break;
                    case ContentPosition.Right:
                        captionRect = new Rectangle(
                            ClientRectangle.Right - ( _captionSize.Width + 2 * BorderPadding ) - BorderThickness,
                            ClientRectangle.Y,
                            _captionSize.Width + 2 * BorderPadding,
                            Height - BorderThickness );
                        textRect = new Rectangle(
                            ClientRectangle.X,
                            ClientRectangle.Y,
                            ClientRectangle.Width - BorderThickness - captionRect.Width,
                            ClientRectangle.Height - BorderThickness );

                        if ( CaptionShowBorder ) {
                            textPath = ExtendedForms.RoundedLeftRect( textRect, BorderRadius );
                            captionPath = ExtendedForms.RoundedRightRect( captionRect, BorderRadius );
                        }
                        else {
                            textPath = ExtendedForms.RoundedRect( textRect, BorderRadius );
                        }

                        break;
                    case ContentPosition.Top:
                        captionRect = new Rectangle(
                            ClientRectangle.X,
                            ClientRectangle.Y,
                            ClientRectangle.Width - BorderThickness,
                            ClientRectangle.Y + _captionSize.Height + BorderPadding );
                        textRect = new Rectangle(
                            ClientRectangle.X,
                            ClientRectangle.Y + captionRect.Height,
                            ClientRectangle.Width - BorderThickness,
                            ClientRectangle.Height - captionRect.Height - BorderThickness );

                        if ( CaptionShowBorder ) {
                            textPath = ExtendedForms.RoundedBottomRect( textRect, BorderRadius );
                            captionPath = ExtendedForms.RoundedTopRect( captionRect, BorderRadius );
                        }
                        else {
                            textPath = ExtendedForms.RoundedRect( textRect, BorderRadius );
                        }

                        break;
                    case ContentPosition.Bottom:
                        captionRect = new Rectangle(
                            ClientRectangle.X,
                            ClientRectangle.Height - ( _captionSize.Height + BorderPadding ) - 1,
                            ClientRectangle.Width - BorderThickness,
                            _captionSize.Height + BorderPadding );
                        textRect = new Rectangle(
                            ClientRectangle.X,
                            ClientRectangle.Y,
                            ClientRectangle.Width - BorderThickness,
                            ClientRectangle.Height - captionRect.Height - BorderThickness );

                        if ( CaptionShowBorder ) {
                            textPath = ExtendedForms.RoundedTopRect( textRect, BorderRadius );
                            captionPath = ExtendedForms.RoundedBottomRect( captionRect, BorderRadius );
                        }
                        else {
                            textPath = ExtendedForms.RoundedRect( textRect, BorderRadius );
                        }
                        break;
                }
            }
            
            g.FillPath( new SolidBrush( style.BackColor ), textPath );

            if ( _captionSize.Width > 0 && captionPath != null && CaptionShowBorder ) {
                if ( CaptionFlatStyle ) {
                    g.FillPath( new SolidBrush( CaptionNormalStyle.BackColor ), captionPath );
                }
                else {
                    g.FillPath( new LinearGradientBrush( captionRect, CaptionNormalStyle.BackColor.Lighten( 20 ), CaptionNormalStyle.BackColor, 90 ), captionPath );
                    switch ( CaptionPosition ) {
                        case ContentPosition.Left:
                            g.DrawLine( new Pen( CaptionNormalStyle.BackColor.Lighten( 40 ) ), captionRect.X + BorderRadius, captionRect.Top + 1, captionRect.Right, captionRect.Top + 1 );
                            break;
                        case ContentPosition.Right:
                            g.DrawLine( new Pen( CaptionNormalStyle.BackColor.Lighten( 40 ) ), captionRect.X, captionRect.Top + 1, captionRect.Right - BorderRadius, captionRect.Top + 1 );
                            break;
                    }
                    
                }
            }

            if ( _captionSize.Width > 0 && captionPath != null )
                g.DrawPath( new Pen( style.BorderColor, BorderThickness ), captionPath );

            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            Rectangle captionTempRect = new Rectangle( captionRect.Location, captionRect.Size );
            if ( CaptionPosition == ContentPosition.Left || CaptionPosition == ContentPosition.Right ) {
                captionTempRect.Inflate( 0, -BorderPadding / 2 - 2 * BorderThickness );
            }
            else {
                captionTempRect.Inflate( -BorderPadding / 2 - 2 * BorderThickness, 0 );
                if ( CaptionImage != null )
                    captionTempRect.Offset( CaptionImageSize.Width + BorderPadding - 1, 0 );
            }

            g.DrawString( Caption, this.Font, new SolidBrush( CaptionNormalStyle.ForeColor ), captionTempRect, sf );
            if ( CaptionImage != null ) {
                int imageY = captionTempRect.Y + ( captionTempRect.Bottom - captionTempRect.Y ) / 2 - CaptionImageSize.Height / 2;
                g.DrawImage( CaptionImage, captionTempRect.X - ( _captionSize.Height + BorderPadding ), imageY, CaptionImageSize.Width, CaptionImageSize.Height );
            }

            if ( _captionSize.Width > 0 && CaptionShowBorder ) {
                switch ( CaptionPosition ) {
                    case ContentPosition.Left:
                        g.DrawLine( new Pen( style.BorderColor, BorderThickness ), captionRect.Right, 0, captionRect.Right, Height );
                        break;
                    case ContentPosition.Right:
                        g.DrawLine( new Pen( style.BorderColor, BorderThickness ), captionRect.Left, 0, captionRect.Left, Height );
                        break;
                }

            }
            
            g.DrawPath( new Pen( style.BorderColor, BorderThickness ), textPath );
            
        }

        private void AxBorderTextBox_Resize( object sender, EventArgs e ) {
            GetCaptionSize();

            if ( CaptionPosition == ContentPosition.Left || CaptionPosition == ContentPosition.Right || CaptionPosition == ContentPosition.Bottom ) {
                textBox1.Top = BorderPadding;
            }
            else if ( CaptionPosition == ContentPosition.Top )  {
                if ( _captionSize.Width > 0 ) {
                    textBox1.Top = BorderPadding * 2 + _captionSize.Height;
                }
                else {
                    textBox1.Top = BorderPadding;
                }
            }

            int captionPadding = ( _captionSize.Width > 0 ) ? BorderPadding * 3 : BorderPadding;

            if ( CaptionPosition == ContentPosition.Left )
                textBox1.Left = _captionSize.Width + captionPadding;
            if ( CaptionPosition == ContentPosition.Right || CaptionPosition == ContentPosition.Bottom || CaptionPosition == ContentPosition.Top )
                textBox1.Left = BorderPadding;

            if ( CaptionPosition == ContentPosition.Left || CaptionPosition == ContentPosition.Right ) {
                textBox1.Width = Width - ( _captionSize.Width + captionPadding ) - BorderPadding;
            }
            else {
                textBox1.Width = Width - 2 * BorderPadding;
            }

            if ( CaptionPosition == ContentPosition.Left || CaptionPosition == ContentPosition.Right ) {
                Height = textBox1.Size.Height + BorderPadding * 2;
            }
            else {
                if ( _captionSize.Width > 0 ) {
                    Height = textBox1.Size.Height + BorderPadding * 3 + _captionSize.Height;
                }
                else {
                    Height = textBox1.Size.Height + BorderPadding * 2;
                }
            }

            this.Refresh();
        }

        private void AxBorderTextBox_MouseEnter( object sender, EventArgs e ) {
            if ( !_isSelected ) {
                _isSelected = true;
                if ( !Error ) {
                    textBox1.ForeColor = HoverStyle.ForeColor;
                    textBox1.BackColor = HoverStyle.BackColor;
                }
                else {
                    textBox1.BackColor = ErrorStyle.BackColor;
                    textBox1.ForeColor = ErrorStyle.ForeColor;
                }
                Refresh();
            }
        }

        private void AxBorderTextBox_MouseLeave( object sender, EventArgs e ) {
            if ( _isSelected ) {
                _isSelected = false;
                if ( !Error ) {
                    textBox1.ForeColor = NormalStyle.ForeColor;
                    textBox1.BackColor = NormalStyle.BackColor;
                }
                else {
                    textBox1.BackColor = ErrorStyle.BackColor;
                    textBox1.ForeColor = ErrorStyle.ForeColor;
                }
                Refresh();
            }
        }

        private void textBox1_MouseEnter( object sender, EventArgs e ) {
            if ( !_isSelected ) {
                _isSelected = true;
                if ( !Error ) {
                    textBox1.ForeColor = HoverStyle.ForeColor;
                    textBox1.BackColor = HoverStyle.BackColor;
                }
                else {
                    textBox1.BackColor = ErrorStyle.BackColor;
                    textBox1.ForeColor = ErrorStyle.ForeColor;
                }
                Refresh();
            }
        }

        private void textBox1_MouseLeave( object sender, EventArgs e ) {
            if ( _isSelected ) {
                _isSelected = false;
                if ( !Error ) {
                    textBox1.ForeColor = NormalStyle.ForeColor;
                    textBox1.BackColor = NormalStyle.BackColor;
                }
                else {
                    textBox1.BackColor = ErrorStyle.BackColor;
                    textBox1.ForeColor = ErrorStyle.ForeColor;
                }
                Refresh();
            }
        }

        private void NormalStyleChanged() {
            if ( !Error ) {
                textBox1.BackColor = NormalStyle.BackColor;
                textBox1.ForeColor = NormalStyle.ForeColor;
            }
            else {
                textBox1.BackColor = ErrorStyle.BackColor;
                textBox1.ForeColor = ErrorStyle.ForeColor;
            }
        }

        private Size GetCaptionSize() {
            Graphics g = CreateGraphics();
            float width = 0;
            float height = 0;

            if ( !string.IsNullOrEmpty( Caption ) ) {
                SizeF size = g.MeasureString( Caption, Font );
                width += g.MeasureString( Caption, Font ).Width;
                height = size.Height;
            }
            else {
                SizeF size = g.MeasureString( "0", Font );
                width = 0;
                height = size.Height;
            }

            if ( CaptionFixedWidth > 0 )
                width = CaptionFixedWidth;

            Size labelSize = new Size( ( int ) width, ( int ) height );

            return labelSize;
        }

        private void textBox1_KeyPress( object sender, KeyPressEventArgs e ) {
            if ( !char.IsControl( e.KeyChar ) && !char.IsDigit( e.KeyChar ) && CharsType == CharacterType.Numbers ) {
                e.Handled = true;
            }
        }

    }
}
