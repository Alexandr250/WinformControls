using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using WinformControls.Helpers;

namespace WinformControls {
    public class AxStatusStrip : StatusStrip {
        private int _borderRadius = 2;

        public int BorderThickness { get; set; }
        public Color BorderColor { get; set; }

        public int BorderRadius {
            get {
                return _borderRadius;
            }
            set {
                _borderRadius = value;
                Refresh();
            }
        }

        public AxStatusStrip() {
            this.SetStyle( ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true );
            BorderThickness = 1;
        }

        protected override void OnPaint( PaintEventArgs e ) {
            Graphics g  = e.Graphics;

            Rectangle rect = new Rectangle( ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width - BorderThickness, ClientRectangle.Height - BorderThickness );
            g.FillPath( new SolidBrush( BackColor ), ExtendedForms.RoundedRect( rect, BorderRadius ) );
            g.DrawLine( new Pen( BorderColor, BorderThickness ), rect.X, rect.Y, rect.Width, rect.Y );

            foreach ( ToolStripItem control in this.Items ) {
                //control.Paint();
            }

            //g.DrawPath( new Pen( BorderColor, BorderThickness ), ExtendedForms.RoundedRect( rect, BorderRadius ) );
        }
    }
}
