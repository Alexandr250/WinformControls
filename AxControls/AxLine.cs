using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.ComponentModel;

namespace WinformControls.AxControls {
    public class AxLine : Control {
        private Color _lineColor = Color.Black;
        private int _lineWidth = 1;
        private DashStyle _dashStyle = DashStyle.Solid;
        private Orientation _orientation = Orientation.Horizontal;

        public Orientation Orientation {
            get {
                return _orientation;
            }
            set {
                _orientation = value;
                OnResize( null );
                Refresh();
            }
        }

        public Color LineColor {
            get {
                return _lineColor;
            }
            set {
                _lineColor = value;
                OnResize( null );
                Refresh();
            }
        }

        public int LineWidth {
            get {
                return _lineWidth;
            }
            set {
                _lineWidth = value;
                OnResize( null );
                Refresh();
            }
        }

        public DashStyle LineDashStyle {
            get {
                return _dashStyle;
            }
            set {
                _dashStyle = value;
                OnResize( null );
                Refresh();
            }
        }

        /// <summary>
        /// Скрывает одноименное свойство
        /// </summary>
        [Browsable( false )]
        public override Color BackColor {
            get {
                return base.BackColor;
            }
            set {
                base.BackColor = value;
            }
        }

        /// <summary>
        /// Скрывает одноименное свойство
        /// </summary>
        [Browsable( false )]
        public override Color ForeColor {
            get {
                return base.ForeColor;
            }
            set {
                base.ForeColor = value;
            }
        }

        /// <summary>
        /// Скрывает одноименное свойство
        /// </summary>
        [Browsable( false )]
        public override Image BackgroundImage {
            get {
                return base.BackgroundImage;
            }
            set {
                base.BackgroundImage = value;
            }
        }

        public AxLine() {
            SetStyle( 
                ControlStyles.AllPaintingInWmPaint | 
                ControlStyles.OptimizedDoubleBuffer | 
                ControlStyles.ResizeRedraw | 
                ControlStyles.SupportsTransparentBackColor | 
                ControlStyles.UserPaint, 
                true );
        }

        protected override void OnPaint( PaintEventArgs e ) {
            Graphics g  = e.Graphics;

            if ( Parent != null )
                g.Clear( Parent.BackColor );

            using ( Pen linePen = new Pen( LineColor, LineWidth ) ) {
                linePen.DashStyle = LineDashStyle;

                if ( Orientation == Orientation.Horizontal ) {                    
                    g.DrawLine( linePen, 0, 1, Width, 1 );
                }
                else {
                    g.DrawLine( linePen, 1, 0, 1, Height );
                }
            }
        }

        protected override void OnResize( EventArgs e ) {
            base.OnResize( e );

            if ( Orientation == Orientation.Horizontal )
                this.Height = LineWidth + 1;
            else
                this.Width = LineWidth + 1;
        }
    }
}
