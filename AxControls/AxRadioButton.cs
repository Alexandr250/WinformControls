using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using WinformControls.Helpers;
using WinformControls.Common;
using System.ComponentModel;

namespace WinformControls {
    public class AxRadioButton : RadioButton {

        private static SolidBrush _blackBrush = new SolidBrush( Color.Black );
        private static SolidBrush _whiteBrush = new SolidBrush( Color.White );

        private bool _isSelected = false;
        private bool _roundedThumb = true;
        private bool _useDarckenColorForBorder = true;
        private int _borderRadius = 3;
        private bool _flatStyle = true;

        private StateStyle _normalStyle;

        [TypeConverter( typeof( ExpandableObjectConverter ) )]
        [Category( "Visual" )]
        public StateStyle NormalStyle {
            get {
                return _normalStyle;
            }
            set {
                if ( _normalStyle != null )
                    _normalStyle.Dispose();

                _normalStyle = value;

                _normalStyle.PropertyChanged += ( propertyName ) => {
                    Refresh();
                };

                Refresh();
            }
        }

        public bool FillBackground { get; set; }
        public int Spacing { get; set; }
        public int ThumbRadius { get; set; }

        public Color SelectedColor { get; set; }

        public Color ThumbColor { get; set; }

        #region "Normal style"
        [Browsable( false )]
        public Color BorderColor {
            get { 
                return NormalStyle.BorderColor;
            }
            set {
                NormalStyle.BorderColor = value;
                Refresh();
            }
        }        

        [Browsable( false )]
        public override Color ForeColor {
            get {
                return NormalStyle.ForeColor;
            }
            set {
                NormalStyle.ForeColor = value;
            }
        }

        [Browsable( false )]
        public override Color BackColor {
            get {
                return NormalStyle.BackColor;
            }
            set {
                NormalStyle.BackColor = value;
            }
        }
        #endregion

        public bool RadioButtonFlatStyle {
            get {
                return _flatStyle;
            }
            set {
                _flatStyle = value;
                Refresh();
            }
        }

        public bool UseDarckenColorForBorder {
            get {
                return _useDarckenColorForBorder;
            }
            set {
                _useDarckenColorForBorder = value;
                Refresh();
            }
        }

        public bool RoundedThumb {
            get {
                return _roundedThumb;
            }
            set {
                _roundedThumb = value;
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

        public AxRadioButton() {
            this.SetStyle( ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true );

            _normalStyle = new StateStyle() {
                BackColor = Color.White,
                ForeColor = Color.Black,
                BorderColor = Color.Black
            };
            _normalStyle.PropertyChanged += ( propertyName ) => {
                Refresh();
            };

            //BorderColor = Color.Black;
            ThumbColor = Color.Gray;
            ThumbRadius = 10;
            Spacing = 5;
            FillBackground = true;
        }

        protected override void OnPaint( PaintEventArgs e ) {
            Graphics g  = e.Graphics;

            if ( Parent != null )
                g.FillRectangle( new SolidBrush( Parent.BackColor ), ClientRectangle );

            g.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle backgroundRect = new Rectangle( 
                ClientRectangle.X, 
                ClientRectangle.Y, 
                ClientRectangle.Width - 1, 
                ClientRectangle.Height - 1 );

            if ( FillBackground ) {
                if ( !_isSelected ) {
                    if ( RadioButtonFlatStyle ) {
                        LinearGradientBrush gradient = new LinearGradientBrush( backgroundRect, BackColor.Lighten( 5 ), BackColor.Darken( 25 ), LinearGradientMode.Vertical );
                        g.FillPath( gradient, ExtendedForms.RoundedRect( backgroundRect, BorderRadius ) );
                    }
                    else {
                        g.FillPath( new SolidBrush( BackColor ), ExtendedForms.RoundedRect( backgroundRect, BorderRadius ) );
                    }

                    if ( _useDarckenColorForBorder )
                        g.DrawPath( new Pen( BackColor.Darken( 80 ), 1 ), ExtendedForms.RoundedRect( backgroundRect, BorderRadius ) );
                    else
                        g.DrawPath( new Pen( BorderColor, 1 ), ExtendedForms.RoundedRect( backgroundRect, BorderRadius ) );
                }
                else {
                    if ( RadioButtonFlatStyle ) {
                        LinearGradientBrush gradient = new LinearGradientBrush( backgroundRect, BackColor.Lighten( 30 ), BackColor.Lighten( 15 ), LinearGradientMode.Vertical );
                        g.FillPath( gradient, ExtendedForms.RoundedRect( backgroundRect, BorderRadius ) );
                    }
                    else {
                        g.FillPath( new SolidBrush( BackColor.Lighten( 30 ) ), ExtendedForms.RoundedRect( backgroundRect, BorderRadius ) );
                    }

                    if ( _useDarckenColorForBorder )
                        g.DrawPath( new Pen( BorderColor.Lighten( 20 ), 1 ), ExtendedForms.RoundedRect( backgroundRect, BorderRadius ) );
                    else
                        g.DrawPath( new Pen( BorderColor, 1 ), ExtendedForms.RoundedRect( backgroundRect, BorderRadius ) );
                }
            }

            int dotDiameter = ThumbRadius;
            int thumbY = ClientRectangle.Height / 2 - dotDiameter / 2 - 1;
            RectangleF innerRect = new RectangleF( Padding.Left + Spacing, thumbY, dotDiameter, dotDiameter );
            int textX = ( int ) innerRect.Left + ( int ) innerRect.Width + Spacing;


            SolidBrush thumbBackBrush = null;
            Pen thumbBorderPen = new Pen( ThumbColor, 2 );
            SolidBrush thumbDotBrush = new SolidBrush( ThumbColor );

            if ( _isSelected ) {
                thumbBackBrush = _whiteBrush;
                thumbBorderPen = ( RadioButtonFlatStyle ) ?
                    thumbBorderPen = new Pen( BackColor.Darken( 120 ), 2 ) :
                    thumbBorderPen = new Pen( ThumbColor, 2 );
            }
            else {
                thumbBackBrush = _whiteBrush;
                thumbBorderPen = ( RadioButtonFlatStyle ) ?
                    thumbBorderPen = new Pen( BackColor.Darken( 130 ), 2 ) :
                    thumbBorderPen = new Pen( ThumbColor, 2 );
            }

            int count = ( Checked || _isSelected ) ? 2 : 1;

            for ( int i = 0 ; i < count ; i++ ) {
                if ( RoundedThumb ) {
                    g.FillEllipse( ( ( Checked || _isSelected ) && i > 0 ) ? thumbDotBrush : thumbBackBrush, innerRect );
                    g.DrawEllipse( thumbBorderPen, innerRect );
                }
                else {
                    g.FillRectangle( thumbBackBrush, innerRect );
                    g.DrawRectangle( thumbBorderPen, innerRect.X, innerRect.Y, innerRect.Width, innerRect.Height );
                }

                innerRect.Inflate( -4, -4 );
            }


            StringFormat sf = new StringFormat();
            sf.LineAlignment = StringAlignment.Center;
            sf.Alignment = StringAlignment.Near;
            sf.FormatFlags = StringFormatFlags.NoWrap;

            innerRect = new RectangleF( textX, 1, ClientRectangle.Width - 1 - dotDiameter - 2, ClientRectangle.Height - 3 );

            SolidBrush textBrush = ( ForeColor == Color.Black ) ? _blackBrush : new SolidBrush( ForeColor ) ;
            g.DrawString( Text, Font, textBrush, innerRect, sf );
        }

        protected override void OnMouseEnter( EventArgs eventargs ) {
            if ( !_isSelected ) {
                _isSelected = true;
                this.Refresh();
            }
        }

        protected override void OnMouseLeave( EventArgs eventargs ) {
            if ( _isSelected ) {
                _isSelected = false;
                this.Refresh();
            }
        }
    }
}
