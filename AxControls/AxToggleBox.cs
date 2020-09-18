using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using WinformControls.Helpers;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using WinformControls.Common;
namespace WinformControls {
    
    public class AxToggleBox : CheckBox {
        private bool _selected = false;
        private int _borderRadius = 2;
        private Rectangle _toggleRect;

        private StateStyle _toggleButtonStyle;

        [Browsable( true )]
        [TypeConverter( typeof( ExpandableObjectConverter ) )]
        public StateStyle ToggleButtonStyle {
            get {
                return _toggleButtonStyle;
            }
            set {
                _toggleButtonStyle = value;
                Refresh();
            }
        }
        
        public Color ToggleCheckedBackColor { get; set; }
        public Color ToggleCheckedHoverBackColor { get; set; }

        public Color ToggleUncheckedBackColor { get; set; }
        public Color ToggleUncheckedHoverBackColor { get; set; }

        
        public Color ToggleButtonHoverBackColor { get; set; }
        
        
        public Color ToggleBorderHoverColor { get; set; }

        public Color ForeChechedColor { get; set; }
        public Color ForeUnchechedColor { get; set; }
        public Color ForeHoverColor { get; set; }

        public string ToggleCheckedText { get; set; }
        public string ToggleUncheckedText { get; set; }

        public bool ToggleFlatStyle { get; set; }
        public bool ToggleShowIcon { get; set; }

        public int BorderRadius {
            get {
                return _borderRadius;
            }
            set {
                _borderRadius = value;
                Refresh();
            }
        }

        public int LineHalfLength { get; set; }

        public AxToggleBox() {
            this.SetStyle( ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true );

            _toggleButtonStyle = new StateStyle() {
                BackColor = Color.White,
                BorderColor = Color.Black,
                ForeColor = Color.Empty
            };

            ToggleCheckedBackColor = Color.White;
            ToggleCheckedHoverBackColor = Color.White;
            ToggleUncheckedBackColor = Color.White;
            ToggleUncheckedHoverBackColor = Color.White;

            //ToggleButtonBackColor = Color.White;
            ToggleButtonHoverBackColor = Color.White;
            
            //ToggleBorderColor = Color.Black;
            ToggleBorderHoverColor = Color.Black;

            ForeHoverColor = Color.Black;
            ForeChechedColor = Color.Black;
            ForeUnchechedColor = Color.Black;

            ToggleCheckedText = "on";

            ToggleFlatStyle = true;

            LineHalfLength = 4;

            ToggleShowIcon = false;

            _toggleRect = new Rectangle( ClientRectangle.X, ClientRectangle.Y, ( ClientRectangle.Height - 1 ) * 2, ClientRectangle.Height - 1 );
        }

        protected override void OnPaint( PaintEventArgs e ) {
            Rectangle toggleRect = _toggleRect;
            int _toggleWidth = toggleRect.Width;

            Graphics g  = e.Graphics;
            

            if ( Parent != null )
                g.FillRectangle( new SolidBrush( Parent.BackColor ), ClientRectangle );

            g.SmoothingMode = SmoothingMode.HighQuality;

            int rectInflate = ( toggleRect.Height - Font.Height );
            if ( Font.Height < 15 )
                rectInflate = 2;

            toggleRect.Offset( -1, 0 );
            //toggleRect.Inflate( rectInflate * -1, rectInflate * -1 );

            toggleRect.Inflate( -1, -1 );

            #region "Toggle drawing"
            Color toggleBackColor = ToggleCheckedBackColor;
            Color toggleBorderColor = ToggleButtonStyle.BorderColor;// ToggleBorderColor;

            if ( _selected ) {
                toggleBackColor = ( Checked ) ? ToggleCheckedHoverBackColor : ToggleUncheckedHoverBackColor;
                toggleBorderColor = ToggleBorderHoverColor;
            }
            else {
                toggleBackColor = ( Checked ) ? ToggleCheckedBackColor : ToggleUncheckedBackColor;
            }

            Brush backBrush = null;
            if ( ToggleFlatStyle ) {
                backBrush = new SolidBrush( toggleBackColor );
            }
            else {
                backBrush = new LinearGradientBrush( toggleRect, toggleBackColor, toggleBackColor.Lighten( 20 ), 90 );
            }

            g.FillPath( backBrush, ExtendedForms.RoundedRect( toggleRect, BorderRadius ) );
            g.DrawPath( new Pen( toggleBorderColor ), ExtendedForms.RoundedRect( toggleRect, BorderRadius ) );


            Rectangle tempRect = toggleRect;
            tempRect.Inflate( -1, -1 );
            g.DrawPath( new Pen( toggleBackColor.Lighten( 20 ) ), ExtendedForms.RoundedRect( tempRect, BorderRadius ) );

            //g.DrawLine( new Pen( Color.FromArgb( 80, 255, 255, 255 ) ), toggleRect.X + BorderRadius, toggleRect.Y + 1, toggleRect.Right - BorderRadius, toggleRect.Y + 1 );
            #endregion

            
            StringFormat sf = new StringFormat();
            sf.LineAlignment = StringAlignment.Center;
            sf.Alignment = StringAlignment.Near;
            sf.FormatFlags = StringFormatFlags.NoWrap;


            int offset = 3;
            if ( BorderRadius > offset )
                offset = BorderRadius;

            Rectangle checkedRect = new Rectangle( toggleRect.X + toggleRect.Width / 2, toggleRect.Y, toggleRect.Width / 2, toggleRect.Height );
            Rectangle unCheckedRect = new Rectangle( toggleRect.X, toggleRect.Y, toggleRect.Width / 2, toggleRect.Height );
            if ( Checked ) {
                Brush brush = null;

                if ( ToggleFlatStyle ) {
                    brush = new SolidBrush( ToggleButtonStyle.BackColor );
                }
                else {
                    brush = new LinearGradientBrush( checkedRect, ToggleButtonStyle.BackColor.Lighten( 30 ), ToggleButtonStyle.BackColor, 90 );
                }

                //g.FillPath( brush, ExtendedForms.RoundedLeftRect( checkedRect, BorderRadius ) );
                //g.DrawPath( new Pen( ToggleButtonStyle.BorderColor ), ExtendedForms.RoundedLeftRect( checkedRect, BorderRadius ) );
                checkedRect.Inflate( -2, -2 );
                g.FillPath( brush, ExtendedForms.RoundedRect( checkedRect, BorderRadius ) );
                g.DrawPath( new Pen( ToggleButtonStyle.BorderColor ), ExtendedForms.RoundedRect( checkedRect, BorderRadius ) );

                tempRect = checkedRect;
                tempRect.Inflate( -1, -1 );
                g.DrawPath( new Pen( ToggleButtonStyle.BackColor.Lighten( 20 ) ), ExtendedForms.RoundedRect( tempRect, BorderRadius ) );
            }
            else {
                Brush brush = null;

                if ( ToggleFlatStyle ) {
                    brush = new SolidBrush( ToggleButtonStyle.BackColor );
                }
                else {
                    brush = new LinearGradientBrush( checkedRect, ToggleButtonStyle.BackColor.Lighten( 30 ), ToggleButtonStyle.BackColor, 90 );
                }

                //g.FillPath( brush, ExtendedForms.RoundedRightRect( unCheckedRect, BorderRadius ) );
                //g.DrawPath( new Pen( ToggleButtonStyle.BorderColor ), ExtendedForms.RoundedRightRect( unCheckedRect, BorderRadius ) );

                unCheckedRect.Inflate( -2, -2 );
                g.FillPath( brush, ExtendedForms.RoundedRect( unCheckedRect, BorderRadius ) );
                g.DrawPath( new Pen( ToggleButtonStyle.BorderColor ), ExtendedForms.RoundedRect( unCheckedRect, BorderRadius ) );


                tempRect = unCheckedRect;
                tempRect.Inflate( -1, -1 );
                g.DrawPath( new Pen( ToggleButtonStyle.BackColor.Lighten( 20 ) ), ExtendedForms.RoundedRect( tempRect, BorderRadius ) );
            }

            

            //unCheckedRect.Inflate( -( 4 ), -( 4 ) );

            g.SmoothingMode = SmoothingMode.HighSpeed;

            if ( ToggleShowIcon ) {
                if ( Checked ) {
                    int centerY = ( unCheckedRect.Bottom - unCheckedRect.Top ) / 2 + 2;
                    int centerX = unCheckedRect.Left + ( unCheckedRect.Right - unCheckedRect.Left ) / 2 + 1;

                    g.DrawLine( new Pen( ForeChechedColor, 2 ), centerX - LineHalfLength, centerY, centerX + LineHalfLength, centerY );
                    g.DrawLine( new Pen( ForeChechedColor, 2 ), centerX, centerY - LineHalfLength, centerX, centerY + LineHalfLength );
                }
                else {
                    int centerY = ( checkedRect.Bottom - checkedRect.Top ) / 2 + 2;
                    int centerX = ( checkedRect.Right + ( checkedRect.Right - checkedRect.Left ) ) / 2 + 1;

                    g.DrawLine( new Pen( ForeUnchechedColor, 2 ), centerX - LineHalfLength, centerY, centerX + LineHalfLength, centerY );
                }
            }

            Rectangle innerRect = new Rectangle(
                ClientRectangle.X + _toggleWidth + 2,
                ClientRectangle.Y,
                ClientRectangle.Width - ClientRectangle.X - _toggleWidth,
                ClientRectangle.Height );

            Color foreColor = ( _selected ) ? ForeHoverColor : ForeColor;

            g.DrawString( Text, Font, new SolidBrush( foreColor ), innerRect, sf );
        }

        protected override void OnMouseEnter( EventArgs eventargs ) {
            if ( !_selected ) {
                _selected = true;
                this.Refresh();
            }
        }

        protected override void OnMouseLeave( EventArgs eventargs ) {
            if ( _selected ) {
                _selected = false;
                this.Refresh();
            }
        }

        protected override void OnResize( EventArgs e ) {
            _toggleRect = new Rectangle( ClientRectangle.X, ClientRectangle.Y, ( ClientRectangle.Height - 1 ) * 2, ClientRectangle.Height - 1 );
            //this.Refresh();
        }


        public override Size GetPreferredSize( Size proposedSize ) {
            Size s = base.GetPreferredSize( proposedSize );
            if ( AutoSize ) {
                s.Width += _toggleRect.Width / 2 + 1;
            }
            return s;
        }
    }
}
