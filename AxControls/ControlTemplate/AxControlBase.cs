using System.Windows.Forms;
using System.Drawing;
using WinformControls.Common;
using System.ComponentModel;

namespace WinformControls.AxControls.ControlTemplate {
    public abstract class AxControlBase : Control {
        private AxControlPalette _palette = null;

        private StateStyle _normalStyle;
        private StateStyle _hoverStyle;
        private StateStyle _disabledStyle;

        private Font _font;
        private int _borderRadius = 2;
        private bool _allCaps;
        private ContentAlignment _textAlign = ContentAlignment.MiddleCenter;
        

        [Category( "Visual" )]
        public bool AllCaps {
            get {
                return _allCaps;
            }
            set {
                _allCaps = value;
                Refresh();
            }
        }

        [Category( "Visual" )]
        public int BorderRadius {
            get {
                return _borderRadius;
            }
            set {
                if ( _borderRadius != value ) {
                    _borderRadius = value;
                    Refresh();
                }
            }
        }

        [Browsable( true )]
        [Category( "Visual" )]
        public new Font Font {
            get {
                if ( _font == null )
                    return DefaultFont;
                else
                    return _font;
            }
            set {
                _font = value;
                Size = GetPreferredSize( this.Size );
                Refresh();
            }
        }

        [Browsable( true )]
        public ContentAlignment TextAlign {
            get {
                return _textAlign;
            }
            set {
                if ( _textAlign != value ) {
                    _textAlign = value;
                    Refresh();
                }
            }
        }

        [Category( "Visual" )]
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

        [Browsable( false )]
        [Category( "Visual" )]
        public Color BorderColor {
            get {
                if ( Palette == null )
                    return NormalStyle.BorderColor;
                else
                    return Palette.NormalStyle.BorderColor;
            }
            set {
                if ( Palette == null )
                    NormalStyle.BorderColor = value;
                else
                    Palette.NormalStyle.BorderColor = value;

                Refresh();
            }
        }

        public AxControlBase() {
            //this.SetStyle( ControlStyles.AllPaintingInWmPaint
            //    | ControlStyles.OptimizedDoubleBuffer
            //    | ControlStyles.ResizeRedraw
            //    | ControlStyles.UserPaint, true );

            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.SupportsTransparentBackColor |
                ControlStyles.UserPaint,
                true );

            if ( Palette == null ) {
                _hoverStyle = new StateStyle() {
                    BackColor = Color.White,
                    ForeColor = Color.Black,
                    BorderColor = Color.Gray
                };

                _disabledStyle = new StateStyle() {
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
            }
        }
    }
}
