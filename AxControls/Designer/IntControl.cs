using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace WinformControls.AxControls.Designer {
    internal class IntControl : UserControl {
        private int _value;
        private Font _font;
        private int _maxValue = byte.MaxValue;

        public int MaxValue {
            get {
                return _maxValue;
            }
            set {
                _maxValue = value;
            }
        }

        public int Value {
            get {
                return _value;
            }
            set {
                _value = value;
            }
        }

        public IntControl( int value ) {
            this.SetStyle( ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true );

            _value = value;
            this.Height = 30;
            _font = new Font( this.Font.FontFamily, this.Font.Size - 2, FontStyle.Regular );
        }

        protected override void OnPaint( PaintEventArgs e ) {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            e.Graphics.FillRectangle( SystemBrushes.Control, 0, 0, this.Width, this.Height );

            e.Graphics.DrawLine( SystemPens.ControlDark, 0, this.Height / 2, this.Width, this.Height / 2 );
            e.Graphics.DrawLine( SystemPens.ControlText, getPosByVal( _value ), this.Height / 2 - 5, getPosByVal( _value ), this.Height / 2 + 5 );



            e.Graphics.DrawString( _value.ToString(), _font, SystemBrushes.GrayText, 0, 0 );


            var size = e.Graphics.MeasureString( MaxValue.ToString(), _font );

            e.Graphics.DrawString( "0", _font, SystemBrushes.GrayText, 0, this.Height - size.Height );
            e.Graphics.DrawString( MaxValue.ToString(), _font, SystemBrushes.GrayText, this.Width - size.Width, this.Height - size.Height );
        }

        private Single getPosByVal( int value ) {
            float delta = ( ( float ) value ) / ( ( float ) _maxValue );
            return ( int ) ( ( float ) this.Width * delta );
        }

        protected override void OnMouseDown( MouseEventArgs e ) {
            if ( e.Button == MouseButtons.Left ) {
                int mouseX = e.X;
                if ( mouseX < 0 )
                    mouseX = 0;

                if ( mouseX > this.Width )
                    mouseX = this.Width;

                float delta = ( ( float ) mouseX ) / ( ( float ) this.Width );
                try {
                    _value = ( int ) ( ( float ) _maxValue * delta );
                }
                catch ( Exception ex ) { }
                this.Refresh();
            }
        }

        protected override void OnMouseMove( MouseEventArgs e ) {
            if ( e.Button == MouseButtons.Left ) {
                int mouseX = e.X;
                if ( mouseX < 0 )
                    mouseX = 0;

                if ( mouseX > this.Width )
                    mouseX = this.Width;

                float delta = ( ( float ) mouseX ) / ( ( float ) this.Width );
                try {
                    _value = ( int ) ( ( float ) _maxValue * delta );
                }
                catch ( Exception ex ) { }
                this.Refresh();
            }
        }
    }
}
