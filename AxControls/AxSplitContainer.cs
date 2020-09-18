using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using WinformControls.Helpers;
using System.ComponentModel;

namespace WinformControls.AxControls {
    public class AxSplitContainer : SplitContainer {
        [DefaultValue( 10 )]
        public int SplitterButtonLength { get; set; }

        public Color SplitterForeColor { get; set; }

        public DashStyle SplitterButtonDashStyle { get; set; }
        public DashStyle SplitterDashStyle { get; set; }

        public bool ShowSplitterBorder { get; set; }

        public override bool Focused {
            get { return false; }
        }

        public AxSplitContainer() {
            SplitterButtonDashStyle = DashStyle.Solid;
            SplitterDashStyle = DashStyle.Solid;
            SplitterForeColor = Color.Black;
        }
        
        protected override void OnPaint( PaintEventArgs e ) {
            base.OnPaint( e );
            if ( this.Orientation == System.Windows.Forms.Orientation.Vertical ) {
                int splitterStart = this.Height / 2 - SplitterButtonLength * 2;
                if ( splitterStart < 0 )
                    splitterStart = 0;

                int splitterEnd = SplitterButtonLength * 2;
                if ( splitterEnd > this.Height )
                    splitterEnd = this.Height;

                for ( int i = 0 ; i < splitterEnd ; i++ ) {
                    e.Graphics.DrawLine( new Pen( SplitterForeColor ) { DashStyle = SplitterButtonDashStyle }, SplitterDistance, splitterStart + 2 * i, SplitterDistance + SplitterWidth, splitterStart + 2 * i );
                }

                //Rectangle r = new Rectangle( SplitterDistance, 1, SplitterWidth - 1, SplitterWidth - 1 );
                //e.Graphics.DrawRectangle( new Pen( ForeColor ), r );

                //r = new Rectangle( SplitterDistance, this.Height - 2 - ( SplitterWidth - 1 ), SplitterWidth - 1, SplitterWidth - 1 );
                //e.Graphics.DrawRectangle( new Pen( ForeColor ), r );

                if ( ShowSplitterBorder ) {
                    Rectangle r = new Rectangle( SplitterDistance, 1, SplitterWidth - 1, this.Height - 2 );
                    e.Graphics.DrawRectangle( new Pen( SplitterForeColor ) { DashStyle = DashStyle.Dot }, r );
                }

                //r = new Rectangle( SplitterDistance, this.Height - 2 - ( SplitterWidth - 1 ), SplitterWidth - 1, SplitterWidth - 1 );
                //e.Graphics.DrawRectangle( new Pen( ForeColor ), r );
            }
            else {

                int splitterStart = this.Width / 2 - SplitterButtonLength * 2;
                if ( splitterStart < 0 )
                    splitterStart = 0;

                int splitterEnd = SplitterButtonLength * 2;
                if ( splitterEnd > this.Width )
                    splitterEnd = this.Width;

                for ( int i = 0 ; i < splitterEnd ; i++ ) {
                    e.Graphics.DrawLine( new Pen( SplitterForeColor ) { DashStyle = SplitterButtonDashStyle }, splitterStart + 2 * i, SplitterDistance, splitterStart + 2 * i, SplitterDistance + SplitterWidth );
                }
                if ( ShowSplitterBorder ) {
                    Rectangle r = new Rectangle( 1, SplitterDistance, this.Width - 2, SplitterWidth - 1 );
                    e.Graphics.DrawRectangle( new Pen( SplitterForeColor ) { DashStyle = DashStyle.Dot }, r );
                }
            }
        }
    }
}
