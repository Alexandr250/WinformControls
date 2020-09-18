using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Security.Permissions;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using WinformControls.AxControls;
using WinformControls.AxControls.Designer;
using WinformControls.Common;
using WinformControls.Helpers;

namespace WinformControls {
    public enum FigureType {
        None,
        Rounded,
        Skewed
    }

    public class AxButton : Button {        
        private AxControlPalette _palette = null;

        private Point _mouseLocation;        

        private bool _isSelected = false;
        private bool _isPushed = false;
        private FigureType _figureType = FigureType.None;
        private int _borderRadius = 5;
        
        private StateStyle _hoverStyle;
        private StateStyle _normalStyle;
        private StateStyle _disabledStyle;

        public AxControlPalette Palette {
            get {
                return _palette;
            }
            set {
                _palette = value;

                if ( _palette == null ) {
                    if ( _hoverStyle == null )
                        _hoverStyle = new StateStyle() {
                            BackColor = Color.White,
                            ForeColor = Color.Black,
                            BorderColor = Color.Gray
                        };

                    if ( _normalStyle == null ) {
                        _normalStyle = new StateStyle() {
                            BackColor = Color.White,
                            ForeColor = Color.Black,
                            BorderColor = Color.Gray
                        };
                        _normalStyle.PropertyChanged += ( propertyName ) => this.Refresh();
                    }

                    if ( _disabledStyle == null )
                        _disabledStyle = new StateStyle() {
                            BackColor = Color.White,
                            ForeColor = Color.Black,
                            BorderColor = Color.Gray
                        };
                }
                else {
                    if ( _hoverStyle != null )
                        _hoverStyle.Dispose();
                    if ( _normalStyle != null )
                        _normalStyle.Dispose();
                    if ( _disabledStyle != null )
                        _disabledStyle.Dispose();
                }

                Refresh();
            }
        }

        [Browsable( true )]
        [TypeConverter( typeof( ExpandableObjectConverter ) )]
        [Category( "Visual" )]
        public StateStyle HoverStyle {
            get {
                if ( Palette == null )
                    return _hoverStyle;
                else
                    return Palette.HoverStyle;
            }
            set {
                if ( Palette == null ) {
                    if ( _hoverStyle != null )
                        _hoverStyle.Dispose();

                    _hoverStyle = value;

                    if ( value != null )
                        _hoverStyle.PropertyChanged += ( propertyName ) => this.Refresh();
                }

                Refresh();
            }
        }

        [Browsable( true )]
        [TypeConverter( typeof( ExpandableObjectConverter ) )]
        [Category( "Visual" )]
        public StateStyle NormalStyle {
            get {
                if ( Palette == null )
                    return _normalStyle;
                else
                    return Palette.NormalStyle;
            }
            set {
                if ( Palette == null ) {
                    if ( _normalStyle != null )
                        _normalStyle.Dispose();

                    _normalStyle = value;

                    if ( value != null )
                        _normalStyle.PropertyChanged += ( propertyName ) => this.Refresh();
                }

                Refresh();
            }
        }

        [Browsable( true )]
        [TypeConverter( typeof( ExpandableObjectConverter ) )]
        [Category( "Visual" )]
        public StateStyle DisabledStyle {
            get {
                if ( Palette == null )
                    return _disabledStyle;
                else
                    return Palette.DisabledStyle;
            }
            set {
                if ( Palette == null ) {
                    if ( _disabledStyle != null )
                        _disabledStyle.Dispose();

                    _disabledStyle = value;

                    if ( value != null )
                        _disabledStyle.PropertyChanged += ( propertyName ) => this.Refresh();
                }

                Refresh();
            }
        }

        [Browsable( false )]
        public override Color ForeColor {
            get {
                if ( Palette == null )
                    return NormalStyle.ForeColor;
                else
                    return Palette.NormalStyle.ForeColor;
            }
            set {
                if ( Palette == null )
                    NormalStyle.ForeColor = value;
                else
                    Palette.NormalStyle.ForeColor = value;

                Refresh();
            }
        }

        [Browsable( false )]
        public override Color BackColor {
            get {
                if ( Palette == null )
                    return NormalStyle.BackColor;
                else
                    return Palette.NormalStyle.BackColor;
            }
            set {
                if ( Palette == null )
                    NormalStyle.BackColor = value;
                else
                    Palette.NormalStyle.BackColor = value;

                Refresh();
            }
        }

        [Browsable( true )]
        [Editor( typeof( IntEditor ), typeof( UITypeEditor ) )]
        public int BorderRadius {
            get {
                int borderRadius = _borderRadius;

                int bigSide = this.Height;
                if ( this.Width < this.Height )
                    bigSide = this.Width;

                if ( borderRadius < 0 )
                    borderRadius = 0;

                if ( borderRadius > bigSide / 2 )
                    borderRadius = bigSide / 2;

                return borderRadius; 
            }
            set {
                if ( _borderRadius == value )
                    return;

                //int bigSide = this.Height;
                //if ( this.Width < this.Height )
                //    bigSide = this.Width;

                _borderRadius = value;

                //if ( value < 0 )
                //    _borderRadius = 0;

                //if ( value > bigSide / 2 )
                //    _borderRadius = bigSide / 2;
                
                if ( FigureType == FigureType.Rounded || FigureType == FigureType.Skewed )
                    Refresh();
            }
        }

        public FigureType FigureType {
            get { 
                return _figureType; 
            }
            set {
                _figureType = value;
                Refresh();
            }
        }

        [Category( "Flash" )]
        public bool DrawFlash { get; set; }
        [Category( "Flash" )]
        public byte FlashAlpha { get; set; }

        [Category( "Gradient" )]
        public bool DrawGradient { get; set; }
        public byte GradientTopLighten { get; set; }
        public byte GradientMiddleDelta { get; set; }
        public byte GradientBottomLighten { get; set; }        
        
        public bool DrawBorder { get; set; }
        public bool DrawInnerBorder { get; set; }
        public bool DrawPushAnimation { get; set; }

        #region "Эти 2 свойства просто скрывают из редактора свойств одноименные свойства предка"
        [Browsable( false )]
        public new FlatStyle FlatStyle { get; set; }

        [Browsable( false )]
        public new FlatButtonAppearance FlatAppearance {
            get {
                return base.FlatAppearance;
            }
        }
        #endregion
               
        public AxButton() {
            this.SetStyle( ControlStyles.AllPaintingInWmPaint 
                | ControlStyles.OptimizedDoubleBuffer 
                | ControlStyles.ResizeRedraw 
                | ControlStyles.UserPaint, true );

            if ( Palette == null ) {
                _hoverStyle = new StateStyle() {
                    BackColor = Color.White,
                    ForeColor = Color.Black,
                    BorderColor = Color.Gray
                };

                _normalStyle = new StateStyle() {
                    BackColor = Color.White,
                    ForeColor = Color.Black,
                    BorderColor = Color.Gray
                };
                _normalStyle.PropertyChanged += ( propertyName ) => this.Refresh();

                _disabledStyle = new StateStyle() {
                    BackColor = Color.White,
                    ForeColor = Color.Black,
                    BorderColor = Color.Gray
                };
            }
        }

        protected override void OnPaint( PaintEventArgs e ) {
            Graphics g  = e.Graphics;

            StringFormat sf = new StringFormat {
                LineAlignment = StringAlignment.Center,
                Alignment = StringAlignment.Center,
                FormatFlags = StringFormatFlags.NoWrap
            };

            if ( Parent != null && FigureType != FigureType.None )
                g.FillRectangle( new SolidBrush( Parent.BackColor ), ClientRectangle );

            Rectangle rect;

            if ( DrawBorder ) {
                rect = new Rectangle(
                ClientRectangle.X,
                ClientRectangle.Y,
                ClientRectangle.Width - 1,
                ClientRectangle.Height - 1 );
            }
            else {
                rect = ClientRectangle;
            }

            GraphicsPath path = null;
            switch ( FigureType ) {
                case FigureType.None:
                    path = ExtendedForms.RoundedRect( rect, 0 );
                    break;
                case FigureType.Rounded:
                    path = ExtendedForms.RoundedRect( rect, BorderRadius );
                    break;
                case FigureType.Skewed:
                    path = ExtendedForms.SkewedRect( rect, BorderRadius );
                    break;
                default:
                    path = ExtendedForms.RoundedRect( rect, 0 );
                    break;
            }

            if ( FigureType != FigureType.None )
                g.SmoothingMode = SmoothingMode.HighQuality;

            
            Brush backBrush;
            Pen borderPen;
            Pen innerBorderPen;
            SolidBrush foreBrush;
            
            if ( Enabled ) {
                if ( _isSelected && !_isPushed && !NormalStyle.Equals( HoverStyle ) ) {
                    if ( Palette == null ) {
                        backBrush = HoverStyle.BackBrush;
                        borderPen = HoverStyle.BorderPen;
                        innerBorderPen = HoverStyle.InnerBorderPen;
                        foreBrush = HoverStyle.ForeBrush;
                    }
                    else {
                        backBrush = Palette.HoverStyle.BackBrush;
                        borderPen = Palette.HoverStyle.BorderPen;
                        innerBorderPen = Palette.HoverStyle.InnerBorderPen;
                        foreBrush = Palette.HoverStyle.ForeBrush;
                    }
                }
                else if ( _isSelected && _isPushed ) {
                    if ( Palette == null ) {
                        if ( !ButtonAnimation.Animated || !DrawPushAnimation ) 
                            backBrush = new SolidBrush( HoverStyle.BackColor.Darken( 30 ) );
                        else
                            backBrush = HoverStyle.BackBrush;
                        borderPen = HoverStyle.BorderPen;
                        innerBorderPen = HoverStyle.InnerBorderPen;
                        foreBrush = HoverStyle.ForeBrush;
                    }
                    else {
                        backBrush = Palette.HoverStyle.BackBrush;
                        borderPen = Palette.HoverStyle.BorderPen;
                        innerBorderPen = Palette.HoverStyle.InnerBorderPen;
                        foreBrush = Palette.HoverStyle.ForeBrush;
                    }
                }
                else {
                    if ( Palette == null ) {
                        backBrush = NormalStyle.BackBrush;
                        borderPen = NormalStyle.BorderPen;
                        innerBorderPen = NormalStyle.InnerBorderPen;
                        foreBrush = NormalStyle.ForeBrush;
                    }
                    else {
                        backBrush = Palette.NormalStyle.BackBrush;
                        borderPen = Palette.NormalStyle.BorderPen;
                        innerBorderPen = Palette.NormalStyle.InnerBorderPen;
                        foreBrush = Palette.NormalStyle.ForeBrush;
                    }
                }
            }
            else {
                if ( Palette == null ) {
                    backBrush = DisabledStyle.BackBrush;
                    borderPen = DisabledStyle.BorderPen;
                    innerBorderPen = DisabledStyle.InnerBorderPen;
                    foreBrush = DisabledStyle.ForeBrush;
                }
                else {
                    backBrush = Palette.DisabledStyle.BackBrush;
                    borderPen = Palette.DisabledStyle.BorderPen;
                    innerBorderPen = Palette.DisabledStyle.InnerBorderPen;
                    foreBrush = Palette.DisabledStyle.ForeBrush;
                }
            }

            g.FillPath( backBrush, path );


            if ( DrawPushAnimation && ButtonAnimation.Animated ) {
                GraphicsPath gp = new GraphicsPath();

                int length = ButtonAnimation.CurrentValue;

                gp.AddEllipse( new Rectangle( ButtonAnimation.MousePos.X - length, ButtonAnimation.MousePos.Y - length, length * 2, length * 2 ) );

                Region region = new Region();
                region.Intersect( path );
                region.Intersect( gp );
                
                e.Graphics.FillRegion( new SolidBrush( HoverStyle.BackColor.Darken( 20 ) ), region );
            }


            if ( _isSelected && DrawFlash ) { //mouse blick
                GraphicsPath gp = new GraphicsPath();

                int length = ( ClientRectangle.Width + ClientRectangle.Height ) / 4;

                gp.AddEllipse( new Rectangle( _mouseLocation.X - length, _mouseLocation.Y - length, length * 2, length * 2 ) );

                PathGradientBrush pgb = new PathGradientBrush( gp );

                pgb.CenterPoint = _mouseLocation;
                pgb.CenterColor = Color.FromArgb( FlashAlpha, Color.White );
                pgb.SurroundColors = new Color[] { Color.FromArgb( 0, Color.White ) };

                e.Graphics.FillPath( pgb, gp );
            }


            if ( DrawBorder )
                g.DrawPath( borderPen, path );

            if ( DrawInnerBorder ) {
                Rectangle innerRect = new Rectangle( rect.Location, rect.Size );
                innerRect.Inflate( -1, -1 );
                
                int radius = 0;
                switch ( FigureType ) {
                    case FigureType.Rounded:
                        radius = BorderRadius;
                        break;
                    case FigureType.Skewed:
                        radius = BorderRadius;
                        break;
                }
                if ( FigureType == WinformControls.FigureType.Skewed )
                    g.DrawPath( innerBorderPen, ExtendedForms.SkewedRect( innerRect, radius ) );
                else
                    g.DrawPath( innerBorderPen, ExtendedForms.RoundedRect( innerRect, radius ) );
            }

            if ( _isSelected && _isPushed )
                rect.Offset( 0, 1 );

            g.SmoothingMode = SmoothingMode.HighSpeed;

            g.DrawString( Text, Font, foreBrush, rect, sf );
        }

        #region "Mouse events"
        protected override void OnMouseEnter( EventArgs e ) {
            base.OnMouseEnter( e );
            _isSelected = true;
            Refresh();
        }

        protected override void OnMouseLeave( EventArgs e ) {
            base.OnMouseLeave( e );
            _isSelected = false;
            if ( DrawPushAnimation ) {
                ButtonAnimation.Animated = false;
                ButtonAnimation.TargetButton = null;
            }
            Refresh();
        }

        protected override void OnMouseDown( MouseEventArgs mevent ) {
            base.OnMouseDown( mevent );
            _isPushed = true;
            if ( DrawPushAnimation ) {
                ButtonAnimation.CurrentValue = 0;
                ButtonAnimation.Animated = true;
                ButtonAnimation.TargetButton = this;
                ButtonAnimation.MousePos = mevent.Location;
                ButtonAnimation.Restart();
            }
            Refresh();
        }

        protected override void OnMouseUp( MouseEventArgs mevent ) {
            base.OnMouseUp( mevent );
            _isPushed = false;
            Refresh();
        }

        protected override void OnMouseMove( MouseEventArgs mevent ) {
            base.OnMouseMove( mevent );
            _mouseLocation = mevent.Location;
            Refresh();
        }
        #endregion
    }
}
