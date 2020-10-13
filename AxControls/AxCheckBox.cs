using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using WinformControls.Helpers;

namespace WinformControls {
    public class AxCheckBox : CheckBox {
        private bool _selected = false;
        private int _borderRadius = 2;

        public string TextChecked { get; set; }

        public Color ToggleBackColor { get; set; }
        public Color ToggleHoverBackColor { get; set; }
        public Color ToggleBorderColor { get; set; }
        public Color ToggleBorderHoverColor { get; set; }
        public Color ToggleCheckedBackColor { get; set; }
        public Color ToggleCheckedBorderColor { get; set; }
        public Color CheckedForeColor { get; set; }
        
        public Color ToggleColor { get; set; }

        public int BorderRadius {
            get {
                return _borderRadius;
            }
            set {
                _borderRadius = value;
                Refresh();
            }
        }

        public AxCheckBox() {
            this.SetStyle( ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true );

            ToggleBackColor = Color.White;
            ToggleHoverBackColor = Color.White;
            ToggleBorderColor = Color.Black;
            ToggleBorderHoverColor = Color.Black;
            ToggleColor = Color.Black;

            ToggleCheckedBackColor = Color.White;
            ToggleCheckedBorderColor = Color.Black;

            CheckedForeColor = Color.Black;
        }

        protected override void OnPaint( PaintEventArgs e ) {
            Graphics g  = e.Graphics;

            if ( Parent != null )
                g.FillRectangle( new SolidBrush( Parent.BackColor ), ClientRectangle );

            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            
            Rectangle toggleRect = new Rectangle( ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Height - 1, ClientRectangle.Height - 1 );

            int toggleWidth = toggleRect.Width;

            if ( Appearance == Appearance.Normal ) {
                int rectInflate = 3; //( toggleRect.Height - Font.Height );
                if ( Font.Height < 15 )
                    rectInflate = 3;

                toggleRect.Offset( -rectInflate, 0 );
                toggleRect.Inflate( -rectInflate * 1, -rectInflate * 1 );

                if ( _selected ) {
                    g.FillPath( new SolidBrush( ToggleHoverBackColor ), ExtendedForms.RoundedRect( toggleRect, BorderRadius ) );
                    g.DrawPath( new Pen( ToggleBorderHoverColor ), ExtendedForms.RoundedRect( toggleRect, BorderRadius ) );
                }
                else {
                    g.FillPath( new SolidBrush( ToggleBackColor ), ExtendedForms.RoundedRect( toggleRect, BorderRadius ) );
                    g.DrawPath( new Pen( ToggleBorderColor ), ExtendedForms.RoundedRect( toggleRect, BorderRadius ) );
                }

                int offset = 3;
                if ( BorderRadius > offset )
                    offset = BorderRadius;

                if ( Checked ) {
                    g.DrawLine( new Pen( ToggleColor, 2 ), toggleRect.X + offset, toggleRect.Y + offset, toggleRect.Right - offset, toggleRect.Bottom - offset );
                    g.DrawLine( new Pen( ToggleColor, 2 ), toggleRect.Right - offset, toggleRect.Y + offset, toggleRect.X + offset, toggleRect.Bottom - offset );
                }


                StringFormat sf = new StringFormat();
                sf.LineAlignment = StringAlignment.Center;
                sf.Alignment = StringAlignment.Near;
                sf.FormatFlags = StringFormatFlags.NoWrap;

                Rectangle innerRect = new Rectangle(
                    ClientRectangle.X + toggleWidth,
                    ClientRectangle.Y,
                    ClientRectangle.Width - ClientRectangle.X - toggleWidth,
                    ClientRectangle.Height );
                g.DrawString( Checked ? TextChecked : Text, Font, new SolidBrush( Checked ? CheckedForeColor : ForeColor ), innerRect, sf );
            }
            else {
                Rectangle checkBoxRect = new Rectangle( ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width - 1, ClientRectangle.Height - 1 );

                if ( _selected ) {
                    g.FillPath( new SolidBrush( ToggleHoverBackColor ), ExtendedForms.RoundedRect( checkBoxRect, BorderRadius ) );
                    g.DrawPath( new Pen( ToggleBorderHoverColor ), ExtendedForms.RoundedRect( checkBoxRect, BorderRadius ) );
                }
                else {
                    if ( Checked ) {
                        g.FillPath( new SolidBrush( ToggleCheckedBackColor ), ExtendedForms.RoundedRect( checkBoxRect, BorderRadius ) );
                        g.DrawPath( new Pen( ToggleCheckedBorderColor ), ExtendedForms.RoundedRect( checkBoxRect, BorderRadius ) );
                    }
                    else {
                        g.FillPath( new SolidBrush( ToggleBackColor ), ExtendedForms.RoundedRect( checkBoxRect, BorderRadius ) );
                        g.DrawPath( new Pen( ToggleBorderColor ), ExtendedForms.RoundedRect( checkBoxRect, BorderRadius ) );
                    }
                }

                StringFormat sf = new StringFormat();
                sf.LineAlignment = StringAlignment.Center;
                sf.Alignment = StringAlignment.Center;
                sf.FormatFlags = StringFormatFlags.NoWrap;
                                
                g.DrawString( Checked ? TextChecked : Text, Font, new SolidBrush( Checked ? CheckedForeColor : ForeColor ), checkBoxRect, sf );
            }
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
    }
}
