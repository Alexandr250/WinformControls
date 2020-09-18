using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
using WinformControls.AxControls.ControlTemplate;
using WinformControls.Helpers;

namespace WinformControls {
    public class AxLabel : AxControlBase {
        #region [ Fields ]

        private bool _autoWidth = false;
        private Color _shadowColor;
        private Point _shadowOffset;
        private Control _target;
        private bool _drawBorder;

        #endregion

        #region [ Properties ]

        [Browsable( true )]
        public LinearGradientBrush Grad { get; set; }

        /// <summary>
        /// Указывает контрол к которому прикрепляется метка
        /// </summary>
        [Browsable( true )]
        [Description( "Указывает контрол к которому прикрепляется метка" )]
        public Control Target {
            get {
                return _target;
            }
            set {
                if ( _target != null ) {
                    _target.MouseMove -= OnTargetMove;
                }
                _target = value;
                if ( _target != null ) {
                    Location = new Point( _target.Left, _target.Top - Height );
                    _target.Move += OnTargetMove;
                }
            }
        }

        /// <summary>
        /// Цвет тени от текста
        /// </summary>
        [Browsable( true )]
        [Category( "Visual" )]
        [Description( "Цвет тени от текста" )]
        public Color ShadowColor {
            get {
                return _shadowColor;
            }
            set {
                _shadowColor = value;
                Refresh();
            }
        }

        /// <summary>
        /// Смещение тени от текста
        /// </summary>
        [Browsable( true )]
        [Category( "Visual" )]
        [Description( "Смещение тени от текста" )]
        public Point ShadowOffset {
            get {
                return _shadowOffset;
            }
            set {
                _shadowOffset = value;
                Refresh();
            }
        }

        /// <summary>
        /// Указывает нужно ли рисовать тень от текста
        /// </summary>
        [Browsable( true )]
        [Category( "Visual" )]
        [Description( "Указывает нужно ли рисовать тень от текста" )]
        public bool DrawShadow { get; set; }
                

        [Browsable( true )]
        [Category( "Visual" )]
        public override bool AutoSize {
            get {
                return base.AutoSize;
            }
            set {
                base.AutoSize = value;
                OnResize( null );
            }
        }

        [Browsable( true )]
        [Category( "Visual" )]
        public bool AutoWidth {
            get {
                return _autoWidth;
            }
            set {
                _autoWidth = value;
                ResizeWidth();
                Refresh();
            }
        }

        public new string Text {
            get {
                return base.Text;
            }
            set {
                base.Text = value;
                if ( AutoWidth )
                    ResizeWidth();
                Refresh();
            }
        }

        public new Size Size {
            get {
                return base.Size;
            }
            set {
                base.Size = GetPreferredSize( value );
                Refresh();
            }
        }

        /// <summary>
        /// Определяет нужно ли рисовать границу метки
        /// </summary>
        [Browsable( true )]
        [Category( "Visual" )]
        public bool DrawBorder {
            get {
                return _drawBorder;
            }
            set {
                if ( _drawBorder != value ) {
                    _drawBorder = value;
                    Refresh();
                }
            }
        }

        #endregion

        #region [ Instance ]

        public AxLabel() {
            FontChanged += ( s, e ) => GetPreferredSize( Size );
            TextAlign = ContentAlignment.MiddleCenter;
        }

        #endregion
        
        #region [ Override ]

        protected override void OnPaint( PaintEventArgs e ) {
            Graphics controlGraphics = e.Graphics;

            if ( Parent != null )
                controlGraphics.Clear( Parent.BackColor );

            StringFormat stringFormat = new StringFormat() {
                LineAlignment = TextAlign.GetVerticalAlignment(),
                Alignment = TextAlign.GetHorizontalAlignment()
            };            

            controlGraphics.SmoothingMode = SmoothingMode.AntiAlias;

            SolidBrush foreBrush = NormalStyle.ForeBrush;

            if ( DrawBorder ) {
                Rectangle rect = new Rectangle(
                    ClientRectangle.X,
                    ClientRectangle.Y,
                    ClientRectangle.Width - 1,
                    ClientRectangle.Height - 1 );

                SolidBrush backBrush;
                Pen borderPen;

                if ( Enabled ) {
                    if ( Palette == null ) {
                        backBrush = NormalStyle.BackBrush;
                        borderPen = NormalStyle.BorderPen;
                        foreBrush = NormalStyle.ForeBrush;
                    }
                    else {
                        backBrush = Palette.NormalStyle.BackBrush;
                        borderPen = Palette.NormalStyle.BorderPen;
                        foreBrush = Palette.NormalStyle.ForeBrush;
                    }
                }
                else {
                    if ( Palette == null ) {
                        backBrush = DisabledStyle.BackBrush;
                        borderPen = DisabledStyle.BorderPen;
                        foreBrush = DisabledStyle.ForeBrush;
                    }
                    else {
                        backBrush = Palette.DisabledStyle.BackBrush;
                        borderPen = Palette.DisabledStyle.BorderPen;
                        foreBrush = Palette.DisabledStyle.ForeBrush;
                    }
                }

                controlGraphics.FillPath( backBrush, ExtendedForms.RoundedRect( rect, BorderRadius ) );
                controlGraphics.DrawPath( borderPen, ExtendedForms.RoundedRect( rect, BorderRadius ) );
            }

            Rectangle captionRect = new Rectangle(
                ClientRectangle.X + Padding.Left,
                ClientRectangle.Y + Padding.Top,
                ClientRectangle.Width - Padding.Horizontal,
                ClientRectangle.Height - Padding.Vertical );

            controlGraphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            if ( DrawShadow && ShadowColor != null ) {
                Rectangle shadowRect = captionRect;
                shadowRect.Offset( ShadowOffset );
                using ( SolidBrush shadowBrush = new SolidBrush( ShadowColor ) ) {
                    controlGraphics.DrawString( ( AllCaps ) ? Text.ToUpper() : Text, Font, shadowBrush, shadowRect, stringFormat );
                }
            }

            controlGraphics.DrawString( ( AllCaps ) ? Text.ToUpper() : Text, Font, foreBrush, captionRect, stringFormat );
        }

        protected override void OnResize( EventArgs e ) {
            base.OnResize( e );
            ResizeWidth();
            if ( Target != null ) {
                Target.Location = new Point( Left, Top + Height );
            }
        }

        public override Size GetPreferredSize( Size proposedSize ) {
            if ( AutoSize )
                return GetPreferedSize();

            if ( AutoWidth )
                return new Size( GetPreferedSize().Width, proposedSize.Height );

            return proposedSize;
        }

        protected override void OnMove( EventArgs e ) {
            base.OnMove( e );
            if ( Target != null ) {
                Target.Location = new Point( Left, Top + Height );
            }
        }

        #endregion

        #region [ Private ]

        private void OnTargetMove( object sender, EventArgs e ) {
            this.Location = new Point( _target.Left, _target.Top - Height );
        }

        private void ResizeWidth() {
            base.Size = GetPreferredSize( new Size( Width, Height ) );
        }

        private Size GetPreferedSize() {
            Graphics g = Graphics.FromHwnd( Handle );
            SizeF size = g.MeasureString( ( AllCaps ) ? Text.ToUpper() : Text, Font );

            return new Size(
                ( int )size.Width + Padding.Horizontal + 2,
                ( int )size.Height + Padding.Horizontal + 2
                );
        }

        #endregion
    }
}
