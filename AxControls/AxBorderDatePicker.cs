using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WinformControls.Helpers;

namespace WinformControls.AxControls {
    public partial class AxBorderDatePicker : UserControl {
        private bool _isSelected = false;
        private bool _error = false;
        private int _borderPadding = 3;
        private int _borderRadius = 2;

        public event EventHandler ValueChanged;

        public Color BorderColor { get; set; }
        public Color BorderErrorColor { get; set; }
        public Color BorderHoverColor { get; set; }

        public Color ButtonBackColor {
            get {
                return borderlessDatePicker1.ButtonBackColor;
            }
            set {
                borderlessDatePicker1.ButtonBackColor = value;
            }
        }
        public Color ButtonBorderColor {
            get {
                return borderlessDatePicker1.ButtonBorderColor;
            }
            set {
                borderlessDatePicker1.ButtonBorderColor = value;
            }
        }

        public Color ButtonHoverColor {
            get {
                return borderlessDatePicker1.ButtonHoverColor;
            }
            set {
                borderlessDatePicker1.ButtonHoverColor = value;
            }
        }

        public Color ButtonBorderHoverColor {
            get {
                return borderlessDatePicker1.ButtonBorderHoverColor;
            }
            set {
                borderlessDatePicker1.ButtonBorderHoverColor = value;
            }
        }

        [Browsable( true )]
        public DateTime Value {
            get {
                return borderlessDatePicker1.Value;
            }
            set {
                borderlessDatePicker1.Value = value;
            }
        }

        public DateTime MaxDate {
            get {
                return borderlessDatePicker1.MaxDate;
            }
            set {
                borderlessDatePicker1.MaxDate = value;
            }
        }

        public DateTime MinDate {
            get {
                return borderlessDatePicker1.MinDate;
            }
            set {
                borderlessDatePicker1.MinDate = value;
            }
        }

        public Boolean Error {
            get {
                return _error;
            }
            set {
                _error = value;
                this.Refresh();
            }
        }        

        [Browsable( true )]
        public new Font Font {
            get {
                return borderlessDatePicker1.Font;
            }
            set {
                base.Font = value;
                borderlessDatePicker1.Font = value;
                AxBorderDatePicker_Resize( this, null );
                Refresh();
            }
        }

        public new Color ForeColor {
            get {
                return borderlessDatePicker1.ForeColor;
            }
            set {
                borderlessDatePicker1.ForeColor = value;
                AxBorderDatePicker_Resize( this, null );
                Refresh();
            }
        }

        public int BorderPadding {
            get {
                return _borderPadding;
            }
            set {
                _borderPadding = value;
                this.Padding = new Padding( _borderPadding );
                AxBorderDatePicker_Resize( this, null );
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
        public int BorderThickness { get; set; }

        public AxBorderDatePicker() {
            this.SetStyle( ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true );
            InitializeComponent();

            Error = false;
            BorderColor = Color.Black;
            BorderErrorColor = Color.Red;
            BorderHoverColor = Color.Blue;
            BorderThickness = 1;
            Height = borderlessDatePicker1.Size.Height + BorderPadding * 2;
            borderlessDatePicker1.Resize += ( s, e ) => AxBorderDatePicker_Resize( s, e );
        }

        private void AxBorderDatePicker_Resize( object sender, EventArgs e ) {
            Height = borderlessDatePicker1.Size.Height + BorderPadding * 2;
            borderlessDatePicker1.Left = BorderPadding;
            borderlessDatePicker1.Width = this.Width - BorderPadding * 2;
            borderlessDatePicker1.Top = this.Height / 2 - borderlessDatePicker1.Height / 2;
        }

        protected override void OnPaint( PaintEventArgs e ) {
            Graphics g  = e.Graphics;

            if ( Parent != null )
                g.FillRectangle( new SolidBrush( Parent.BackColor ), ClientRectangle );

            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            Rectangle rect = new Rectangle( ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width - BorderThickness, ClientRectangle.Height - BorderThickness );

            g.FillPath( new SolidBrush( Color.White ), ExtendedForms.RoundedRect( rect, BorderRadius ) );

            if ( !Error ) {
                if ( _isSelected ) {
                    g.DrawPath( new Pen( BorderHoverColor, BorderThickness ), ExtendedForms.RoundedRect( rect, BorderRadius ) );
                }
                else {
                    g.DrawPath( new Pen( BorderColor, BorderThickness ), ExtendedForms.RoundedRect( rect, BorderRadius ) );
                }
            }
            else {
                g.DrawPath( new Pen( BorderErrorColor, BorderThickness ), ExtendedForms.RoundedRect( rect, BorderRadius ) );
            }
        }
        
        private void AxBorderDatePicker_MouseEnter( object sender, EventArgs e ) {
            _isSelected = true;
            Refresh();
        }

        private void AxBorderDatePicker_MouseLeave( object sender, EventArgs e ) {
            _isSelected = false;
            Refresh();
        }

        private void borderlessDatePicker1_MouseEnter( object sender, EventArgs e ) {
            if ( !_isSelected ) {
                _isSelected = true;
                Refresh();
            }
        }

        private void borderlessDatePicker1_MouseLeave( object sender, EventArgs e ) {
            if ( _isSelected ) {
                _isSelected = false;
                Refresh();
            }
        }

        private void borderlessDatePicker1_ValueChanged( object sender, EventArgs e ) {
            if ( ValueChanged != null )
                ValueChanged( sender, e );
        }

    }
}
