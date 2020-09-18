using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
using WinformControls.Common;
using System.Drawing.Drawing2D;
using WinformControls.Helpers;
using System.Drawing.Text;

namespace WinformControls.AxControls.ControlTemplate {
    public abstract class AxCaptionControlBase<T> : AxControlBase where T : Control, new() {
        private T _innerControl;
        private string _caption;
        private StateStyle _captionNormalStyle;

        [Browsable( true )]
        [TypeConverter( typeof( ExpandableObjectConverter ) )]
        [Category( "Visual" )]
        public StateStyle CaptionNormalStyle {
            get {
                return _captionNormalStyle;
            }
            set {
                if ( _captionNormalStyle != null )
                    _captionNormalStyle.Dispose();

                _captionNormalStyle = value;

                if ( value != null )
                    _captionNormalStyle.PropertyChanged += ( propertyName ) => this.Refresh();

                Refresh();
            }
        }

        public bool DrawCaptionBackground { get; set; }

        public string Caption {
            get {
                return _caption;
            }
            set {
                _caption = value;
                ResizeInnerControl();
            }
        }

        public ContentAlignment TextAlign { get; set; }

        public override string Text {
            get {
                return _innerControl.Text;
            }
            set {
                _innerControl.Text = value;
            }
        }

        internal T Control {
            get {
                return _innerControl;
            }
        }

        public AxCaptionControlBase() {
            this.Width = 150;
            this.Height = 60;

            Caption = string.Empty;
            DrawCaptionBackground = false;

            _captionNormalStyle = new StateStyle() {
                BackColor = Color.White,
                ForeColor = Color.Black,
                BorderColor = Color.Gray
            };
            _captionNormalStyle.PropertyChanged += ( propertyName ) => this.Refresh();

            _innerControl = new T();
            _innerControl.Parent = this;
            ResizeInnerControl();
        }

        //public AxCaptionControlBase( T control ) {
        //    this.Width = 150;
        //    this.Height = 60;

        //    Caption = string.Empty;
        //    DrawCaptionBackground = false;

        //    _captionNormalStyle = new StateStyle() {
        //        BackColor = Color.White,
        //        ForeColor = Color.Black,
        //        BorderColor = Color.Gray
        //    };
        //    _captionNormalStyle.PropertyChanged += ( propertyName ) => this.Refresh();

        //    _innerControl = control;
        //    _innerControl.Parent = this;
        //    ResizeInnerControl();
        //}

        public override Size GetPreferredSize( Size proposedSize ) {
            Graphics g = CreateGraphics();
            float width = 0;
            float height = 0;

            if ( !string.IsNullOrEmpty( Caption ) ) {
                SizeF size = g.MeasureString( Caption, Font );
                width += g.MeasureString( Caption, Font ).Width;
                height = size.Height;
                width += ( Padding.Left + Padding.Right ) ;
            }
            if ( !string.IsNullOrEmpty( Text ) ) {
                SizeF size = g.MeasureString( Text, Font );
                width += g.MeasureString( Text, Font ).Width;
                width += ( Padding.Left + Padding.Right );
            }
            if ( width == 0 ) {
                SizeF size = g.MeasureString( "0", Font );
                width = ( Padding.Left + Padding.Right );
                height = size.Height;
            }

            height += ( Padding.Top + Padding.Bottom );

            Size labelSize = new Size( ( int ) width, ( int ) height );

            return labelSize;
        }

        private Size GetCaptionSize() {
            Graphics g = CreateGraphics();
            float width = 0;
            float height = 0;

            if ( !string.IsNullOrEmpty( Caption ) ) {
                SizeF size = g.MeasureString( Caption, Font );
                width = size.Width;
                height = size.Height;
                height += ( Padding.Top + Padding.Bottom );
                width += ( Padding.Left + Padding.Right );
            }

            Size labelSize = new Size( ( int ) width, ( int ) height );

            return labelSize;
        }

        private void ResizeInnerControl() {
            if ( _innerControl != null ) {
                _innerControl.Left = Padding.Left;
                _innerControl.Width = this.Width - Padding.Left - Padding.Right;

                Size captionSize = GetCaptionSize();

                _innerControl.Top = Padding.Top + captionSize.Height;
                _innerControl.Height = this.Height - Padding.Top - Padding.Bottom - captionSize.Height;
            }
        }

        protected override void OnResize( EventArgs e ) {
            ResizeInnerControl();
        }

        protected override void OnPaint( PaintEventArgs e ) {
            Graphics g  = e.Graphics;

            if ( Parent != null )
                g.FillRectangle( new SolidBrush( Parent.BackColor ), ClientRectangle );

            Rectangle clientRect = new Rectangle( ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width - 1, ClientRectangle.Height - 1 );

            if ( DrawCaptionBackground ) {
                g.FillPath( new SolidBrush( NormalStyle.BackColor ), ExtendedForms.RoundedLeftRect( clientRect, BorderRadius ) );
                g.DrawPath( new Pen( NormalStyle.BorderColor ), ExtendedForms.RoundedLeftRect( clientRect, BorderRadius ) );
            }

            StringFormat sf = new StringFormat();
            sf.LineAlignment = StringAlignment.Center;
            switch ( TextAlign ) {
                case ContentAlignment.BottomLeft:
                    sf.Alignment = StringAlignment.Near;
                    break;
                case ContentAlignment.MiddleLeft:
                    sf.Alignment = StringAlignment.Near;
                    break;
                case ContentAlignment.TopLeft:
                    sf.Alignment = StringAlignment.Near;
                    break;
                case ContentAlignment.BottomCenter:
                    sf.Alignment = StringAlignment.Center;
                    break;
                case ContentAlignment.MiddleCenter:
                    sf.Alignment = StringAlignment.Center;
                    break;
                case ContentAlignment.TopCenter:
                    sf.Alignment = StringAlignment.Center;
                    break;
                case ContentAlignment.BottomRight:
                    sf.Alignment = StringAlignment.Far;
                    break;
                case ContentAlignment.MiddleRight:
                    sf.Alignment = StringAlignment.Far;
                    break;
                case ContentAlignment.TopRight:
                    sf.Alignment = StringAlignment.Far;
                    break;
            }

            g.SmoothingMode = SmoothingMode.AntiAlias;

            int captionHeight = GetCaptionSize().Height;// ( int ) g.MeasureString( Caption, Font ).Width;// +2 * _spacing;

            Rectangle captionRect = new Rectangle( ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width - 1, captionHeight ); 

            if ( DrawCaptionBackground ) {
                g.FillPath( new SolidBrush( CaptionNormalStyle.BackColor ), ExtendedForms.RoundedLeftRect( captionRect, BorderRadius ) );
                g.DrawLine( new Pen( NormalStyle.BorderColor ), captionRect.X, captionRect.Bottom, captionRect.Right, captionRect.Bottom );
            }
            else {
                //g.FillPath( new SolidBrush( Parent.BackColor ), ExtendedForms.RoundedLeftRect( captionRect, BorderRadius ) );
                //g.DrawLine( new Pen( NormalStyle.BorderColor ), captionRect.X, captionRect.Bottom, captionRect.Right, captionRect.Bottom );
            }
            //g.FillPath( new SolidBrush( BackColor ), ExtendedForms.RoundedRightRect( textRect, BorderRadius ) );

            //g.DrawPath( new Pen( BorderColor ), ExtendedForms.RoundedLeftRect( captionRect, BorderRadius ) );
            //g.DrawPath( new Pen( BorderColor ), ExtendedForms.RoundedRightRect( textRect, BorderRadius ) );

            //Rectangle innerCaptionRect = captionRect;
            //innerCaptionRect.Inflate( -1, -1 );
            //g.DrawPath( new Pen( CaptionNormalStyle.BackColor.Lighten( CaptionNormalStyle.InnerBorderPenLightenValue ) ), ExtendedForms.RoundedLeftRect( innerCaptionRect, BorderRadius ) );

            g.SmoothingMode = SmoothingMode.HighSpeed;

            //captionRect.Offset( _spacing, 0 );
            //textRect.Offset( _spacing, 0 );

            //Rectangle captionRect = new Rectangle( ClientRectangle.Location, new Size( captionWidth, ClientRectangle.Size.Height ) );

            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            captionRect = new Rectangle( 
                ClientRectangle.X + Padding.Left, 
                ClientRectangle.Y + Padding.Top,
                ClientRectangle.Width - 1 - Padding.Left - Padding.Right, 
                captionHeight - Padding.Top - Padding.Bottom ); 

            g.DrawString( ( AllCaps ) ? Caption.ToUpper() : Caption, Font, new SolidBrush( CaptionNormalStyle.ForeColor ), captionRect, sf );

            //Rectangle textRect = new Rectangle( new Point( captionWidth, ClientRectangle.Location.Y ), ClientRectangle.Size );

            //g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            //g.DrawString( ( AllCaps ) ? Text.ToUpper() : Text, Font, new SolidBrush( ForeColor ), textRect, sf );
        }
    }
}
