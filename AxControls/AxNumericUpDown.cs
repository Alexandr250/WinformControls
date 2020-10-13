using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using WinformControls.Helpers;
using System.ComponentModel;
using WinformControls.Common;

namespace WinformControls.AxControls {
    public enum NumericButtonPosition {
        InlineRight,
        InlineLeft,
        InlineBothSide
    }

    public class AxNumericUpDown : Control {
        AxButton _buttonUp = new AxButton();
        AxButton _buttonDown = new AxButton();
        TextBox _textBox1 = new TextBox();
        private decimal _value;
        private decimal _minimum = 0;
        private decimal _maximum = 100;
        private bool _keyboardEnter = false;
        
        Timer _timer;
        private  int _borderPadding = 3;
        private  bool _isDecrementing;
        private  int _borderRadius = 3;
        private  StateStyle _normalStyle;
        private  StateStyle _hoverStyle;
        private  bool _isSelected;

        public event EventHandler ValueChanged;

        public decimal Minimum {
            get {
                return _minimum;
            }
            set {
                _minimum = value;
            }
        }

        public decimal Maximum {
            get {
                return _maximum;
            }
            set {
                _maximum = value;
            }
        }

        public string Postfix { get; set; }

        public NumericButtonPosition ButtonPosition { get; set; }

        [Browsable( true )]
        [TypeConverter( typeof( ExpandableObjectConverter ) )]
        [Category( "Visual" )]
        public StateStyle HoverStyle {
            get {
                return _hoverStyle;
            }
            set {
                if ( _hoverStyle != null )
                    _hoverStyle.Dispose();

                _hoverStyle = value;

                if ( value != null )
                    _hoverStyle.PropertyChanged += ( propertyName ) => this.Refresh();

                Refresh();
            }
        }

        [Browsable( true )]
        [TypeConverter( typeof( ExpandableObjectConverter ) )]
        [Category( "Visual" )]
        public StateStyle NormalStyle {
            get {
                return _normalStyle;
            }
            set {
                if ( _normalStyle != null )
                    _normalStyle.Dispose();

                _normalStyle = value;

                if ( value != null )
                    _normalStyle.PropertyChanged += ( propertyName ) => this.Refresh();

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
                OnResize( null );
                Refresh();
            }
        }

        public Color ButtonBorderColor {
            get {
                return _buttonDown.NormalStyle.BorderColor;
            }
            set {
                _buttonDown.NormalStyle.BorderColor = _buttonUp.NormalStyle.BorderColor = value;
                OnResize( null );
                Refresh();
            }
        }

        public Color ButtonBackColor {
            get {
                return _buttonDown.NormalStyle.BackColor;
            }
            set {
                _buttonDown.NormalStyle.BackColor = _buttonUp.NormalStyle.BackColor = value;
                OnResize( null );
                Refresh();
            }
        }

        public bool DrawButtonBorder {
            get {
                return _buttonDown.DrawBorder;
            }
            set {
                _buttonDown.DrawBorder = _buttonUp.DrawBorder = value;
                OnResize( null );
                Refresh();
            }
        }

        public Color ButtonHoverBorderColor {
            get {
                return _buttonDown.HoverStyle.BorderColor;
            }
            set {
                _buttonDown.HoverStyle.BorderColor = _buttonUp.HoverStyle.BorderColor = value;
                OnResize( null );
                Refresh();
            }
        }

        public Color ButtonHoverBackColor {
            get {
                return _buttonDown.HoverStyle.BackColor;
            }
            set {
                _buttonDown.HoverStyle.BackColor = _buttonUp.HoverStyle.BackColor = value;
                OnResize( null );
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

        public int ButtonBorderRadius {
            get {
                return _buttonDown.BorderRadius;
            }
            set {
                _buttonDown.BorderRadius = _buttonUp.BorderRadius = value;
                Refresh();
            }
        }

        public decimal Value {
            get {
                return _value;
            }
            set {
                if ( value < Minimum || value > Maximum )
                    return;

                this._value = value;
                PrintValue();
                if ( ValueChanged != null )
                    ValueChanged( this, new EventArgs() );
                Invalidate();
            }
        }

        private void PrintValue() {
            if ( _keyboardEnter )
                _textBox1.Text = string.Format( "{0}", Value.ToString() );
            else
                _textBox1.Text = string.Format( "{0} {1}", Value.ToString(), Postfix );
        }

        public HorizontalAlignment TextAlign {
            get {
                return _textBox1.TextAlign;
            }
            set {
                _textBox1.TextAlign = value;
            }
        }

        public AxNumericUpDown() {
            SetStyle( ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor | ControlStyles.UserPaint, true );
            Width = 60;
            //Height = 20;

            _value = 0;
            _textBox1.Text = _value.ToString();

            _textBox1.KeyDown += textBox1_KeyDown;
            _textBox1.BorderStyle = BorderStyle.None;
            //_textBox.BackColor = Color.Red;
            _textBox1.Font = this.Font;
            _textBox1.KeyPress += textBox1_KeyPress;
            _textBox1.Parent = this;


            _buttonDown.Width = _buttonUp.Width = 18;

            _buttonUp.Text = "+";
            _buttonUp.Click += ButtonUp_Click;
            _buttonUp.MouseDown += ButtonUp_MouseDown;
            _buttonUp.MouseUp += ButtonUp_MouseUp;

            _buttonDown.Top = 0;
            _buttonDown.Text = "-";
            _buttonDown.MouseDown += ButtonDown_MouseDown;
            _buttonDown.MouseUp += ButtonDown_MouseUp;
            _buttonDown.Click += ButtonDown_Click;

            _buttonDown.FigureType = _buttonUp.FigureType = FigureType.Rounded;
            _buttonDown.DrawPushAnimation = _buttonUp.DrawPushAnimation = true;
            //_buttonDown.FlashAlpha = _buttonUp.FlashAlpha = 40;
            _buttonDown.Parent = _buttonUp.Parent = this;


            _normalStyle = new StateStyle() {
                BackColor = Color.White,
                ForeColor = Color.Black,
                BorderColor = Color.Black
            };

            _hoverStyle = new StateStyle() {
                BackColor = Color.White,
                ForeColor = Color.Black,
                BorderColor = Color.Black
            };


            this.Invalidate();
            this.OnResize( null );

            _timer = new Timer() {
                Interval = 400
            };
            _timer.Tick += timer_Tick;


            MouseEnter += AxNumericUpDown_MouseEnter;
            MouseLeave += AxNumericUpDown_MouseLeave;
            _textBox1.MouseEnter += textBox1_MouseEnter;
            _textBox1.MouseLeave += textBox1_MouseLeave;
            _textBox1.Enter += textBox1_Enter;
            _textBox1.Leave += textBox1_Leave;
            _buttonUp.MouseEnter += axButton_MouseEnter;
            _buttonUp.MouseLeave += axButton_MouseLeave;
            _buttonDown.MouseEnter += axButton_MouseEnter;
            _buttonDown.MouseLeave += axButton_MouseLeave;
        }

        //void box_KeyDown( object sender, KeyEventArgs e ) {
        //    if ( e.KeyCode == Keys.Enter ) {
        //        e.SuppressKeyPress = true;
        //        System.Media.SystemSounds.Asterisk.Play();
        //    }
        //}

        void timer_Tick( object sender, EventArgs e ) {
            Value = _isDecrementing ? Value - 1 : Value + 1;
            //_textBox1.Text = _value.ToString();
            if ( _timer.Interval >= 50 )
                _timer.Interval /= 2;

        }

        void ButtonDown_MouseUp( object sender, MouseEventArgs e ) {
            _timer.Interval = 400;
            _timer.Stop();
            _isDecrementing = false;
        }

        void ButtonDown_MouseDown( object sender, MouseEventArgs e ) {
            _buttonDown.Focus();
            _isDecrementing = true;
            //_value = decimal.Parse( _textBox1.Text );
            _timer.Start();
        }

        void ButtonUp_MouseUp( object sender, MouseEventArgs e ) {
            _timer.Interval = 400;
            _timer.Stop();
            _isDecrementing = false;
        }

        void ButtonUp_MouseDown( object sender, MouseEventArgs e ) {
            _buttonUp.Focus();
            _isDecrementing = false;
            //_value = decimal.Parse( _textBox1.Text );
            _timer.Start();
        }


        void ButtonDown_Click( object sender, EventArgs e ) {
            Value--;
        }

        void ButtonUp_Click( object sender, EventArgs e ) {
            Value++;
        }

        private void textBox1_KeyDown( object sender, KeyEventArgs e ) {
            if ( e.KeyCode == Keys.Up )
                Value++;
            if ( e.KeyCode == Keys.Down )
                Value--;
        }

        void textBox1_KeyPress( object sender, KeyPressEventArgs e ) {

            //if ( _textBox1.SelectedText.Length >= _textBox1.Text.Length )
            //    _textBox1.Text = "";
            //if ( !char.IsControl( e.KeyChar ) && ( !char.IsDigit( e.KeyChar ) ) && ( e.KeyChar != '.' ) && ( e.KeyChar != '-' ) )
            //    e.Handled = true;

            //if ( e.KeyChar == '.' && ( sender as TextBox ).Text.IndexOf( '.' ) > -1 )
            //    e.Handled = true;

            //if ( e.KeyChar == '-' && ( sender as TextBox ).Text.Length > 0 )
            //    e.Handled = true;

            

            if ( !char.IsControl( e.KeyChar ) && ( !char.IsDigit( e.KeyChar ) ) )
                e.Handled = true;
        }

        void textBox1_Enter( object sender, EventArgs e ) {
            _keyboardEnter = true;
            PrintValue();
        }

        void textBox1_Leave( object sender, EventArgs e ) {
            _keyboardEnter = false;

            decimal value = _value;

            if ( decimal.TryParse( _textBox1.Text, out value ) )
                Value = value;

            PrintValue();
        }


        protected override void OnResize( EventArgs e ) {
            base.OnResize( e );
            this.Height = _textBox1.Height + 2 * BorderPadding;

            switch ( ButtonPosition ) {
                case NumericButtonPosition.InlineRight:
                    _buttonDown.Left = this.Width - _buttonDown.Width - 2;// BorderPadding;
                    _buttonUp.Left = _buttonDown.Left - _buttonUp.Width - 1;
                    _textBox1.Left = BorderPadding;
                    _textBox1.Width = this.Width - 2 * BorderPadding - _buttonDown.Width - _buttonUp.Width - 1;
                    break;
                case NumericButtonPosition.InlineLeft:
                    _buttonUp.Left = 2;// BorderPadding;
                    _buttonDown.Left = _buttonUp.Left + _buttonUp.Width + 1;
                    _textBox1.Left = _buttonDown.Left + _buttonDown.Width + BorderPadding;
                    _textBox1.Width = this.Width - 3 * BorderPadding - _buttonDown.Width - _buttonUp.Width - 1;
                    break;
                case NumericButtonPosition.InlineBothSide:
                    _buttonUp.Left = 2;// BorderPadding;
                    _buttonDown.Left = this.Width - _buttonDown.Width - 2;;
                    _textBox1.Left = _buttonUp.Left + _buttonUp.Width + BorderPadding;
                    _textBox1.Width = this.Width - 3 * BorderPadding - _buttonDown.Width - _buttonUp.Width - 1;
                    break;
            }

            _buttonUp.Top = 2;
            _buttonDown.Top = 2;

            _buttonUp.Height = this.Height - 2 * 2;
            _buttonDown.Height = this.Height - 2 * 2;
            
            
            _textBox1.Top = this.Height / 2 - _textBox1.Height / 2;
        }

        protected override void OnPaint( PaintEventArgs e ) {

            if ( BorderRadius > 0 )
                e.Graphics.FillRectangle( Brushes.Transparent, 0, 0, Width, Height );

            Rectangle controlRect = new Rectangle(
                        ClientRectangle.X,
                        ClientRectangle.Y,
                        ClientRectangle.Width - 1,
                        ClientRectangle.Height - 1 );

            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;

            if ( _isSelected ) {
                e.Graphics.FillPath( HoverStyle.BackBrush, ExtendedForms.RoundedRect( controlRect, BorderRadius ) );
                e.Graphics.DrawPath( HoverStyle.BorderPen, ExtendedForms.RoundedRect( controlRect, BorderRadius ) );
                _textBox1.BackColor = HoverStyle.BackBrush.Color;
            }
            else {
                e.Graphics.FillPath( NormalStyle.BackBrush, ExtendedForms.RoundedRect( controlRect, BorderRadius ) );
                e.Graphics.DrawPath( NormalStyle.BorderPen, ExtendedForms.RoundedRect( controlRect, BorderRadius ) );
                _textBox1.BackColor = NormalStyle.BackBrush.Color;
            }
        }

        protected override void OnFontChanged( EventArgs e ) {
            _textBox1.Font = Font;
            base.OnFontChanged( e );
            OnResize( null );
        }

        protected override void OnMouseWheel( MouseEventArgs e ) {
            if ( e.Delta > 0 )
                Value++;
            else
                Value--;
        }

        private void AxNumericUpDown_MouseEnter( object sender, EventArgs e ) {
            if ( !_isSelected ) {
                _isSelected = true;

                _textBox1.ForeColor = HoverStyle.ForeColor;
                _textBox1.BackColor = HoverStyle.BackColor;

                Refresh();
            }
        }

        private void AxNumericUpDown_MouseLeave( object sender, EventArgs e ) {
            if ( _isSelected ) {
                _isSelected = false;

                _textBox1.ForeColor = NormalStyle.ForeColor;
                _textBox1.BackColor = NormalStyle.BackColor;

                Refresh();
            }
        }

        private void textBox1_MouseEnter( object sender, EventArgs e ) {
            if ( !_isSelected ) {
                _isSelected = true;

                _textBox1.ForeColor = HoverStyle.ForeColor;
                _textBox1.BackColor = HoverStyle.BackColor;

                Refresh();
            }
        }

        private void textBox1_MouseLeave( object sender, EventArgs e ) {
            if ( _isSelected ) {
                _isSelected = false;

                _textBox1.ForeColor = NormalStyle.ForeColor;
                _textBox1.BackColor = NormalStyle.BackColor;

                Refresh();
            }
        }

        private void axButton_MouseEnter( object sender, EventArgs e ) {
            if ( !_isSelected ) {
                _isSelected = true;
                Refresh();
            }
        }

        private void axButton_MouseLeave( object sender, EventArgs e ) {
            if ( _isSelected ) {
                _isSelected = false;
                Refresh();
            }
        }
    }
}
