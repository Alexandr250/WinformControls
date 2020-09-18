using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using WinformControls.Helpers;
using System.Drawing.Text;
using System.ComponentModel;
using WinformControls.Common;

namespace WinformControls.AxControls {
    public class AxCaptionLabel : Label {
        private int _spacing = 2;

        private Color _borderColor;
        private bool _allCaps;
        private  int _borderRadius;
        private StateStyle _captionNormalStyle;

        public bool AllCaps {
            get {
                return _allCaps;
            }
            set {
                _allCaps = value;
                Refresh();
            }
        }

        public Color BorderColor {
            get {
                return _borderColor;
            }
            set {
                _borderColor = value;
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


        public bool CaptionFlatStyle { get; set; }

        public int BorderRadius {
            get {
                return _borderRadius;
            }
            set {
                _borderRadius = value;
                Refresh();
            }
        }

        public override BorderStyle BorderStyle {
            get {
                return BorderStyle.None;
            }
            set { } 
        }

        public string Caption { get; set; }

        public AxCaptionLabel() {
            _captionNormalStyle = new StateStyle() {
                BackColor = Color.White,
                ForeColor = Color.Black,
                BorderColor = Color.Gray
            };
            _captionNormalStyle.PropertyChanged += ( propertyName ) => this.Refresh();

            Caption = string.Empty;
            CaptionFlatStyle = true;
        }

        protected override void OnPaint( PaintEventArgs e ) {
            Graphics g  = e.Graphics;

            if ( Parent != null )
                g.FillRectangle( new SolidBrush( Parent.BackColor ), ClientRectangle );

            StringFormat sf = new StringFormat();
            sf.LineAlignment = StringAlignment.Center;
            switch ( TextAlign ) {
                case ContentAlignment.BottomLeft:
                    sf.Alignment = StringAlignment.Near;
                    break;
                case ContentAlignment.MiddleLeft:
                    sf.Alignment = StringAlignment.Near;
                    break;
                case ContentAlignment.TopLeft:
                    sf.Alignment = StringAlignment.Near;
                    break;
                case ContentAlignment.BottomCenter:
                    sf.Alignment = StringAlignment.Center;
                    break;
                case ContentAlignment.MiddleCenter:
                    sf.Alignment = StringAlignment.Center;
                    break;
                case ContentAlignment.TopCenter:
                    sf.Alignment = StringAlignment.Center;
                    break;
                case ContentAlignment.BottomRight:
                    sf.Alignment = StringAlignment.Far;
                    break;
                case ContentAlignment.MiddleRight:
                    sf.Alignment = StringAlignment.Far;
                    break;
                case ContentAlignment.TopRight:
                    sf.Alignment = StringAlignment.Far;
                    break;
            }

            g.SmoothingMode = SmoothingMode.AntiAlias;

            int captionWidth = ( int ) g.MeasureString( Caption, Font ).Width + 2 * _spacing;

            Rectangle captionRect = new Rectangle( ClientRectangle.X, ClientRectangle.Y, captionWidth, ClientRectangle.Height - 1 );
            Rectangle textRect = new Rectangle( ClientRectangle.X + captionWidth, ClientRectangle.Y, ClientRectangle.Width - 1 - captionWidth, ClientRectangle.Height - 1 );


            if ( CaptionFlatStyle ) {
                g.FillPath( new SolidBrush( CaptionNormalStyle.BackColor ), ExtendedForms.RoundedLeftRect( captionRect, BorderRadius ) );
                
            }
            else {
                g.FillPath( new LinearGradientBrush( captionRect, CaptionNormalStyle.BackColor.Lighten( 20 ), CaptionNormalStyle.BackColor, 90 ), ExtendedForms.RoundedLeftRect( captionRect, BorderRadius ) );

                g.DrawLine( new Pen( CaptionNormalStyle.BackColor.Lighten( 40 ) ), captionRect.X + BorderRadius, captionRect.Top + 1, captionRect.Right, captionRect.Top + 1 );
            }
            g.FillPath( new SolidBrush( BackColor ), ExtendedForms.RoundedRightRect( textRect, BorderRadius ) );

            g.DrawPath( new Pen( BorderColor ), ExtendedForms.RoundedLeftRect( captionRect, BorderRadius ) );
            g.DrawPath( new Pen( BorderColor ), ExtendedForms.RoundedRightRect( textRect, BorderRadius ) );

            Rectangle innerCaptionRect = captionRect;
            innerCaptionRect.Inflate( -1, -1 );
            g.DrawPath( new Pen( CaptionNormalStyle.BackColor.Lighten( CaptionNormalStyle.InnerBorderPenLightenValue ) ), ExtendedForms.RoundedLeftRect( innerCaptionRect, BorderRadius ) );

            g.SmoothingMode = SmoothingMode.HighSpeed;

            //captionRect.Offset( _spacing, 0 );
            //textRect.Offset( _spacing, 0 );
            
            //Rectangle captionRect = new Rectangle( ClientRectangle.Location, new Size( captionWidth, ClientRectangle.Size.Height ) );

            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            g.DrawString( ( _allCaps ) ? Caption.ToUpper() : Caption, Font, new SolidBrush( CaptionNormalStyle.ForeColor ), captionRect, sf );

            //Rectangle textRect = new Rectangle( new Point( captionWidth, ClientRectangle.Location.Y ), ClientRectangle.Size );

            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            g.DrawString( ( _allCaps ) ? Text.ToUpper() : Text, Font, new SolidBrush( ForeColor ), textRect, sf );
        }

        public override Size GetPreferredSize( Size proposedSize ) {
            Graphics g = CreateGraphics();
            float width = 0;
            float height = 0;
            
            if ( !string.IsNullOrEmpty( Caption ) ) {
                SizeF size = g.MeasureString( Caption, Font );
                width += g.MeasureString( Caption, Font ).Width;
                height = size.Height;
                width += 2 * _spacing;
            }
            if ( !string.IsNullOrEmpty( Text ) ) {
                SizeF size = g.MeasureString( Text, Font );
                width += g.MeasureString( Text, Font ).Width;
                width += 2 * _spacing;
            }
            if ( width == 0 ) {
                SizeF size = g.MeasureString( "0", Font );
                width = _spacing;
                height = size.Height;
            }

            height += _spacing;

            Size labelSize = new Size( ( int ) width, ( int ) height );

            return labelSize;
        }
    }
}
