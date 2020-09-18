using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using WinformControls.Helpers;
using System.Runtime.InteropServices;
using System.Drawing.Drawing2D;
using System.ComponentModel;

namespace WinformControls {
    [StructLayout( LayoutKind.Sequential, CharSet = CharSet.Auto )]
    public struct RECT {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }

    [StructLayout( LayoutKind.Sequential, CharSet = CharSet.Auto )]
    public struct PAINTSTRUCT {
        public IntPtr hdc;
        public bool fErase;
        public RECT rcPaint;
        public bool fRestore;
        public bool fIncUpdate;
    }

    public enum ProgressBarFigure {
        Normal,
        Rounded
    }

    public class AxProgressBar : ProgressBar {
        private Timer _timer;
        private int _borderRadius = 1;        
        private int _indicatorWidth = 50;
        private int _indicatorHeight = 50;
        private int _value;
        private DateTime _now;
        private ProgressBarFigure _figure;
        private bool _stopProgress = false;

        int _delta = 1;
        int _iwidth = 1;

        public int BorderThickness { get; set; }
        public Color BorderColor { get; set; }
        public Color ProgressColor { get; set; }

        public int IndicatorBackAlpha { get; set; }

        public int IndicatorWidth {
            get {
                return _indicatorWidth;
            }
            set {
                _indicatorWidth = value;
                Refresh();
            }
        }

        public int IndicatorHeight {
            get {
                return _indicatorHeight;
            }
            set {
                _indicatorHeight = value;
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

        public override string Text {
            get {
                return DateTime.Now.Subtract( _now ).ToString( @"mm\:ss" );
            }
        }

        public new ProgressBarStyle Style {
            get {
                return base.Style;
            }
            set {
                base.Style = value;
                if ( Style == ProgressBarStyle.Marquee )
                    _timer.Start();
                else
                    _timer.Stop();
            }
        }

        public new ProgressBarFigure Figure {
            get {
                return _figure;
            }
            set {
                _figure = value;
                if ( _figure == ProgressBarFigure.Normal )
                    _value = -_indicatorWidth;
                else
                    _value = 0;
                Refresh();
            }
        }

        [Browsable( true ) ]
        public new Font Font {
            get {
                return base.Font;
            }
            set {
                base.Font = value;                
            }
        }

        public bool StopProgress {
            get {
                return _stopProgress;
            }
            set {
                _stopProgress = value;
                if ( _stopProgress )
                    _timer.Stop();
                else
                    _timer.Start();
            }
        }

        public AxProgressBar() {
            this.SetStyle( ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true );
            IndicatorBackAlpha = 50;
            BorderThickness = 2;
            if ( Figure == ProgressBarFigure.Normal )
                _value = -_indicatorWidth;
            else
                _value = 0;
            _now = DateTime.Now;

            _timer = new Timer();
            _timer.Interval = 10;
            _timer.Tick += ( e, f ) => Refresh();
        }

        ~AxProgressBar() {
            if ( _timer != null ) {
                try {
                    _timer.Dispose();
                }
                catch ( Exception ex ) { }
            }
        }

        protected override void OnResize( EventArgs e ) {
            if ( Figure == ProgressBarFigure.Rounded ) {
                Width = Height;
            }
        }

        protected override void OnPaint( PaintEventArgs e ) {
            Graphics g  = e.Graphics;

            if ( BorderRadius > 0 )
                g.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle rect = new Rectangle( ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width - BorderThickness, ClientRectangle.Height - BorderThickness );
            g.FillPath( new SolidBrush( BackColor ), ExtendedForms.RoundedRect( rect, BorderRadius ) );


            if ( Figure == ProgressBarFigure.Normal ) {
                Rectangle progressRect = new Rectangle( _value, rect.Y + 2, IndicatorWidth, rect.Bottom - 3 );

                Brush brush = new LinearGradientBrush( progressRect, BackColor, ProgressColor, 0F ); //new SolidBrush( ProgressColor );

                g.FillPath( brush, ExtendedForms.RoundedRect( progressRect, BorderRadius ) );
                _value++;
                if ( _value > rect.Width )
                    _value = -_indicatorWidth;

                g.DrawPath( new Pen( BorderColor, BorderThickness ), ExtendedForms.RoundedRect( rect, BorderRadius ) );

                
            }
            else {

                Rectangle arcRect = new Rectangle( ClientRectangle.X + IndicatorHeight / 2, ClientRectangle.Y + IndicatorHeight / 2, ClientRectangle.Width - BorderThickness - IndicatorHeight, ClientRectangle.Height - BorderThickness - IndicatorHeight );
                Color color1;
                Color color2;


                //g.FillPie( new SolidBrush( ProgressColor ), rect, 0, Value );
                _value ++;
                if ( _value >= 720 )
                    _value = 360;
                //if ( _value > 360 ) {
                //    _value = 0;
                //    _delta *= -1;
                //}

                //if ( _delta > 0 ) {
                //    color1 = ProgressColor.SetAlpha( IndicatorBackAlpha );
                //    color2 = ProgressColor;
                //}
                //else {
                //    color1 = ProgressColor;
                //    color2 = ProgressColor.SetAlpha( IndicatorBackAlpha );
                //}

                //g.DrawArc( new Pen( color1, IndicatorHeight ), arcRect, 0, 360 );                
                //g.DrawArc( new Pen( color2, IndicatorHeight ), arcRect, -90, _value );

                //Console.WriteLine( _value );

                

                g.DrawArc( new Pen( ProgressColor.SetAlpha( IndicatorBackAlpha ), IndicatorHeight ), arcRect, 0, 360 );
                g.DrawArc( new Pen( ProgressColor, IndicatorHeight ), arcRect, _value, IndicatorWidth );

                g.DrawArc( new Pen( Color.Black.SetAlpha( 50 ), IndicatorHeight ), arcRect, _value * -2, 1 );
                

                //_alpha += _delta;
                //if ( _alpha > 255 ) {
                //    _alpha = 255;
                //    _delta *= -1;
                //}
                //if ( _alpha < 0 ) {
                //    _alpha = 0;
                //    _delta *= -1;
                //}
                
            }

            string text = ( Style == ProgressBarStyle.Marquee ) ? Text : Value.ToString();

            StringFormat stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Center;

            g.DrawString(
                text,
                this.Font,
                new SolidBrush( ForeColor ),
                rect,
                stringFormat );
        }
    }
}
