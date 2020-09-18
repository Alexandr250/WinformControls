using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinformControls.Helpers;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace WinformControls.AxControls {
    //internal interface AnimatedControl {
    //}

    internal class AnimationManager {
        private Timer _timer;
        public int StartValue { get; set; }
        public int EndValue { get; set; }
        public int Interval { get; set; }
        public int CurrentValue { get; set; }

        public void Start() {
            _timer.Stop();
            CurrentValue = StartValue;
            _timer.Enabled = true;
            _timer.Start();
        }
        public void Stop() {
            _timer.Enabled = false;
            _timer.Stop();
        }

        public event Action OnAnimate;

        public AnimationManager() {
            Interval = 5;
            _timer = new Timer() { Interval = 10, Enabled = false };
            _timer.Tick += ( s, e ) => {
                CurrentValue += Interval;
                Console.WriteLine( CurrentValue );

                if ( CurrentValue > EndValue )
                    _timer.Enabled = false;

                if ( OnAnimate != null )
                    OnAnimate();                
            };
        }
    }


    public class AxAnimatedButton : Button {
        private bool _isSelected = false;
        private bool _isPushed = false;
        private FigureType _figureType = FigureType.None;
        private int _borderRadius = 5;
        private int _borderSkew = 5;
        private Color _borderColor = Color.White;
        private Color _selectedColor = Color.White;
        private Color _selectedBorderColor = Color.White;
        private Color _selectedForeColor = Color.Black;
        private AnimationManager _manager = new AnimationManager() { StartValue = 1, EndValue = 255 };

        public Color BorderColor {
            get {
                return _borderColor;
            }
            set {
                _borderColor = value;
                Refresh();
            }
        }

        public Color SelectedBorderColor {
            get { 
                return _selectedBorderColor; 
            }
            set { 
                _selectedBorderColor = value;
                Refresh();
            }
        }

        public Color SelectedForeColor {
            get {
                return _selectedForeColor;
            }
            set {
                _selectedForeColor = value;
                Refresh();
            }
        }

        public override Color BackColor {
            get {
                return base.BackColor;
            }
            set {
                base.BackColor = value;
                Refresh();
            }
        }

        public Color SelectedColor {
            get { 
                return _selectedColor; 
            }
            set { 
                _selectedColor = value;
                Refresh();
            }
        }

        public int BorderRadius {
            get { 
                return _borderRadius; 
            }
            set { 
                _borderRadius = value;
                if ( FigureType == FigureType.Rounded )
                    Refresh();
            }
        }        

        public int BorderSkew {
            get { 
                return _borderSkew; 
            }
            set { 
                _borderSkew = value;
                if ( FigureType == FigureType.Skewed )
                    Refresh();
            }
        }

        public FigureType FigureType {
            get { 
                return _figureType; 
            }
            set {
                _figureType = value;
                Refresh();
            }
        }

        public AxAnimatedButton() {
            this.SetStyle( ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true );
            _manager.OnAnimate += () => this.Refresh();
        }

        private bool backgroundPainted = false;

        protected override void OnPaint( PaintEventArgs e ) {
            //base.OnPaint( e );
            Graphics g  = e.Graphics;
            //g.TextRenderingHint = TextRenderingHint.AntiAlias;

            StringFormat sf = new StringFormat();
            sf.LineAlignment = StringAlignment.Center;
            sf.Alignment = StringAlignment.Center;
            sf.FormatFlags = StringFormatFlags.NoWrap;


            if ( Parent != null && !backgroundPainted ) {
                g.FillRectangle( new SolidBrush( Parent.BackColor ), ClientRectangle );
                backgroundPainted = true;
            }

            Rectangle rect = new Rectangle( ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width - 1, ClientRectangle.Height - 1 );

            GraphicsPath path = null;
            switch ( FigureType ) {
                case FigureType.None:
                    path = ExtendedForms.RoundedRect( rect, 0 );
                    break;
                case FigureType.Rounded:
                    path = ExtendedForms.RoundedRect( rect, BorderRadius );
                    break;
                case FigureType.Skewed:
                    path = ExtendedForms.SkewedRect( rect, BorderSkew );
                    break;
                default:
                    path = ExtendedForms.RoundedRect( rect, 0 );
                    break;
            }


            g.SmoothingMode = SmoothingMode.HighQuality;

            if ( _isSelected && !_isPushed ) {                
                g.FillPath( new SolidBrush( SelectedColor.SetAlpha( _manager.CurrentValue ) ), path );
                g.DrawPath( new Pen( SelectedBorderColor ), path );

                g.SmoothingMode = SmoothingMode.HighSpeed;

                g.DrawString( Text, Font, new SolidBrush( SelectedForeColor ), rect, sf );
            }
            else if ( _isSelected && _isPushed ) {
                g.FillPath( new SolidBrush( SelectedColor.Darken( 10 ) ), path );
                g.DrawPath( new Pen( SelectedBorderColor.Darken( 10 ) ), path );

                g.SmoothingMode = SmoothingMode.HighSpeed;

                rect.Offset( 0, 2 );
                g.DrawString( Text, Font, new SolidBrush( SelectedForeColor ), rect, sf );
            }
            else {
                g.FillPath( new SolidBrush( BackColor.SetAlpha( _manager.CurrentValue ) ), path );
                g.DrawPath( new Pen( BorderColor ), path );

                g.SmoothingMode = SmoothingMode.HighSpeed;

                g.DrawString( Text, Font, new SolidBrush( ForeColor ), rect, sf );
            }
        }

        protected override void OnPaintBackground( PaintEventArgs e ) {
            e.Graphics.FillRectangle( Brushes.Red, Bounds );
        }

        protected override void OnMouseEnter( EventArgs e ) {
            base.OnMouseEnter( e );
            _isSelected = true;
            _manager.Start();
            //Refresh();
        }

        protected override void OnMouseLeave( EventArgs e ) {
            base.OnMouseLeave( e );
            _isSelected = false;
            _manager.Start();
            //Refresh();
        }

        protected override void OnMouseDown( MouseEventArgs mevent ) {
            base.OnMouseDown( mevent );
            _isPushed = true;
            Refresh();
        }

        protected override void OnMouseUp( MouseEventArgs mevent ) {
            base.OnMouseUp( mevent );
            _isPushed = false;
            Refresh();
        }
    }
}
