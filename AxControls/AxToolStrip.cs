using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WinformControls.Helpers;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace WinformControls.AxControls {
    public class AxToolStrip : ToolStrip {
        private AxToolStripRenderer _renderer;

        public int ToolStripGradientAngle {
            get {
                return _renderer.ToolStripGradientAngle;
            }
            set {
                _renderer.ToolStripGradientAngle = value;
            }
        }

        public int ToolStripBackgroundAlpha {
            get {
                return _renderer.ToolStripBackgroundAlpha;
            }
            set {
                _renderer.ToolStripBackgroundAlpha = value;
            }
        }

        public int ToolStripButtonBackgroundAlpha {
            get {
                return _renderer.ToolStripButtonBackgroundAlpha;
            }
            set {
                _renderer.ToolStripButtonBackgroundAlpha = value;
                this.Refresh();
            }
        }

        public int ToolStripButtonBorderAlpha {
            get {
                return _renderer.ToolStripButtonBorderAlpha;
            }
            set {
                _renderer.ToolStripButtonBorderAlpha = value;
                this.Refresh();
            }
        }

        public Color ToolStripTopGradienColor {
            get {
                return _renderer.ToolStripTopGradienColor;
            }
            set {
                _renderer.ToolStripTopGradienColor = value;
            }
        }

        public Color ToolStripBottomGradienColor {
            get {
                return _renderer.ToolStripBottomGradienColor;
            }
            set {
                _renderer.ToolStripBottomGradienColor = value;
            }
        }

        public Color ToolStripBorderColor {
            get {
                return _renderer.ToolStripBorderColor;
            }
            set {
                _renderer.ToolStripBorderColor = value;
                this.Refresh();
            }
        }

        public Color ToolStripButtonHoverColor {
            get {
                return _renderer.ToolStripButtonHoverColor;
            }
            set {
                _renderer.ToolStripButtonHoverColor = value;
                this.Refresh();
            }
        }

        public Color ToolStripButtonBorderHoverColor {
            get {
                return _renderer.ToolStripButtonBorderHoverColor;
            }
            set {
                _renderer.ToolStripButtonBorderHoverColor = value;
                this.Refresh();
            }
        }

        public bool ToolStripDrawTopShadow {
            get {
                return _renderer.ToolStripDrawTopShadow;
            }
            set {
                _renderer.ToolStripDrawTopShadow = value;
            }
        }

        public bool ToolStripDrawBottomShadow {
            get {
                return _renderer.ToolStripDrawBottomShadow;
            }
            set {
                _renderer.ToolStripDrawBottomShadow = value;
            }
        }

        public bool ToolStripDrawTopBorderPart {
            get {
                return _renderer.ToolStripDrawTopBorderPart;
            }
            set {
                _renderer.ToolStripDrawTopBorderPart = value;
            }
        }

        public AxToolStrip() {
            this.SetStyle( ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true );

            _renderer = new AxToolStripRenderer();
            this.Renderer = _renderer;

            //this.Paint += ( s, e ) => {
            //    foreach ( ToolStripItem item in Items ) {
            //        if ( !item.Selected && item is ToolStripComboBox ) {
            //            ToolStripComboBox combo = item as ToolStripComboBox;
            //            Rectangle r = new Rectangle(
            //                combo.ComboBox.Location.X - 1,
            //                combo.ComboBox.Location.Y - 1,
            //                combo.ComboBox.Size.Width + 1,
            //                combo.ComboBox.Size.Height + 1 );

            //            e.Graphics.DrawRectangle( new Pen( ToolStripBorderColor.Lighten( 15 ) ), r );
            //        }
            //    }
            //};
        }
    }



    internal class AxToolStripRenderer : ToolStripRenderer {
        private Color _itemForeColor = Color.Black;

        public Color ItemForeColor {
            get { 
                return _itemForeColor; 
            }
            set { 
                _itemForeColor = value; 
            }
        }



        public int ToolStripGradientAngle { get; set; }

        public Color ToolStripTopGradienColor { get; set; }
        public Color ToolStripBottomGradienColor { get; set; }

        public Color ToolStripBorderColor { get; set; }

        public bool ToolStripDrawBorder { get; set; }

        public bool ToolStripDrawTopShadow { get; set; }
        public bool ToolStripDrawBottomShadow { get; set; }

        public bool ToolStripDrawTopBorderPart { get; set; }
        public int ToolStripBackgroundAlpha { get; set; }

        public Color ToolStripButtonHoverColor { get; set; }
        public Color ToolStripButtonBorderHoverColor { get; set; }

        public Color ToolStripButtonPressedColor { get; set; }
        public Color ToolStripButtonBorderPressedColor { get; set; }

        public int ToolStripButtonBackgroundAlpha { get; set; }
        public int ToolStripButtonBorderAlpha { get; set; }

        public AxToolStripRenderer() {
            ToolStripTopGradienColor = Color.White;
            ToolStripBottomGradienColor = Color.Black;
            ToolStripBorderColor = Color.Black;
            ToolStripButtonHoverColor = Color.Gray;
            ToolStripButtonBorderHoverColor = Color.Black;
            ToolStripButtonPressedColor = Color.Gray;
            ToolStripButtonBorderPressedColor = Color.Black;

            ToolStripGradientAngle = 90;
            ToolStripDrawTopShadow = false;
            ToolStripDrawBottomShadow = false;
            ToolStripDrawTopBorderPart = true;
            ToolStripDrawBorder = true;
            ToolStripBackgroundAlpha = 255;
            ToolStripButtonBackgroundAlpha = 255;
            ToolStripButtonBorderAlpha = 255;
        }

        protected override void OnRenderToolStripBackground( ToolStripRenderEventArgs e ) {
            Graphics g =  e.Graphics;
            Color toolStripTopGradienColor = ToolStripTopGradienColor.SetAlpha( ToolStripBackgroundAlpha );
            Color toolStripBottomGradienColor = ToolStripBottomGradienColor.SetAlpha( ToolStripBackgroundAlpha );

            using ( Brush brush = new LinearGradientBrush( e.AffectedBounds, toolStripTopGradienColor, toolStripBottomGradienColor, ToolStripGradientAngle ) ) {
                g.FillRectangle( brush, e.AffectedBounds );

                if ( ToolStripDrawBorder ) {
                    if ( ToolStripDrawTopShadow ) {
                        int borderOffset = 1;
                        if ( !ToolStripDrawTopBorderPart )
                            borderOffset = 0;

                        g.DrawLine(
                            new Pen( toolStripTopGradienColor.Lighten( 25 ) ),
                            e.AffectedBounds.X + 1,
                            e.AffectedBounds.Top + borderOffset,
                            e.AffectedBounds.Right - 1,
                            e.AffectedBounds.Top + borderOffset );
                    }

                    if ( ToolStripDrawBottomShadow )
                        g.DrawLine(
                            new Pen( toolStripBottomGradienColor.Darken( 20 ) ),
                            e.AffectedBounds.X + 1,
                            e.AffectedBounds.Bottom - 2,
                            e.AffectedBounds.Right - 1,
                            e.AffectedBounds.Bottom - 2 );
                }
            }
        }

        protected override void OnRenderToolStripBorder( ToolStripRenderEventArgs e ) {
            if ( !ToolStripDrawBorder )
                return;

            Graphics g =  e.Graphics;
            using ( Pen pen = new Pen( ToolStripBorderColor ) ) {
                int borderOffset = 0;
                if ( !ToolStripDrawTopBorderPart )
                    borderOffset = 1;

                Rectangle rect = new Rectangle( e.AffectedBounds.X, e.AffectedBounds.Y - borderOffset, e.AffectedBounds.Width - 1, e.AffectedBounds.Height - 1 + borderOffset );
                g.DrawRectangle( pen, rect );
            }
        }

        protected override void OnRenderGrip( ToolStripGripRenderEventArgs e ) {
            Graphics g =  e.Graphics;
            using ( Pen pen = new Pen( ToolStripBorderColor.Lighten( 10 ) ) ) {
                //pen.DashStyle = DashStyle.Dot;
                g.DrawLine( pen, e.GripBounds.X, e.GripBounds.Y + 1, e.GripBounds.X, e.GripBounds.Bottom );
                g.DrawLine( pen, e.GripBounds.X + 2, e.GripBounds.Y + 1, e.GripBounds.X + 2, e.GripBounds.Bottom );
            }
        }

        protected override void OnRenderButtonBackground( ToolStripItemRenderEventArgs e ) {
            Graphics g =  e.Graphics;
            Color toolStripButtonHoverColor = ToolStripButtonHoverColor.SetAlpha( ToolStripButtonBackgroundAlpha );
            Color toolStripButtonBorderHoverColor = ToolStripButtonBorderHoverColor.SetAlpha( ToolStripButtonBorderAlpha );
            Color toolStripButtonPressedColor = ToolStripButtonPressedColor.SetAlpha( ToolStripButtonBackgroundAlpha );
            Color toolStripButtonBorderPressedColor = ToolStripButtonBorderPressedColor.SetAlpha( ToolStripButtonBorderAlpha );
            Rectangle rect = new Rectangle( 1, 1, e.Item.Bounds.Width - 2, e.Item.Bounds.Height - 3 );
            if ( !ToolStripDrawBorder ) {
                rect = new Rectangle( 0, 1, e.Item.Bounds.Width - 1, e.Item.Bounds.Height - 3 );
            }

            if ( e.Item.Selected ) {
                using ( Brush brush = new SolidBrush( toolStripButtonHoverColor ) ) {
                    g.FillRectangle( brush, rect );
                }
                g.DrawRectangle( new Pen( toolStripButtonBorderHoverColor ), rect );

                Rectangle innerRect = rect;
                innerRect.Inflate( -1, -1 );

                g.DrawRectangle( new Pen( toolStripButtonHoverColor.Lighten( 100 ) ), innerRect );
            }

            if ( e.Item.Pressed ) {
                using ( Brush brush = new SolidBrush( toolStripButtonPressedColor ) ) {
                    g.FillRectangle( brush, rect );
                }
                g.DrawRectangle( new Pen( toolStripButtonBorderPressedColor ), rect );

                Rectangle innerRect = rect;
                innerRect.Inflate( -1, -1 );

                g.DrawRectangle( new Pen( toolStripButtonPressedColor.Lighten( 100 ) ), innerRect );
            }
        }

        protected override void OnRenderDropDownButtonBackground( ToolStripItemRenderEventArgs e ) {
            OnRenderButtonBackground( e );
        }

        protected override void OnRenderSplitButtonBackground( ToolStripItemRenderEventArgs e ) {
            OnRenderButtonBackground( e );
        }

        protected override void OnRenderMenuItemBackground( ToolStripItemRenderEventArgs e ) {
            e.Item.ForeColor = Color.Black;
            OnRenderButtonBackground( e );
        }

        protected override void OnRenderSeparator( ToolStripSeparatorRenderEventArgs e ) {
            Graphics g =  e.Graphics;
            using ( Pen pen = new Pen( ToolStripBorderColor.Lighten( 15 ) ) ) {
                int center = ( int ) ( e.Item.Bounds.Width / 2 );
                g.DrawLine( pen, center, 1, center, e.Item.Bounds.Height - 2 );
            }
        }

        protected override void OnRenderItemImage( ToolStripItemImageRenderEventArgs e ) {
            if ( e.Image != null ) {
                Rectangle imageRect = new Rectangle(
                    e.Item.Padding.Left,
                    e.Item.Size.Height / 2 - e.Image.Size.Height / 2,
                    e.Image.Size.Width,
                    e.Image.Size.Height );
                Graphics g =  e.Graphics;

                if ( e.Item.Enabled ) {
                    g.DrawImage( e.Image, imageRect );
                }
                else {
                    Bitmap b = ExtendedForms.MakeGrayscale3( new Bitmap( e.Image ) );
                    g.DrawImage( b, imageRect );
                }
            }
        }

        protected override void OnRenderItemText( ToolStripItemTextRenderEventArgs e ) {
            Rectangle textRect = new Rectangle(
                e.Item.Padding.Left,
                e.Item.Padding.Top,
                e.Item.Size.Width - e.Item.Padding.Left - e.Item.Padding.Right,
                e.Item.Size.Height - e.Item.Padding.Top - e.Item.Padding.Bottom );
            
            if ( e.Item.Image != null )
                textRect = new Rectangle(
                e.Item.Padding.Left + e.Item.Image.Size.Width,
                e.Item.Padding.Top,
                e.Item.Size.Width - e.Item.Padding.Left - e.Item.Image.Size.Width - e.Item.Padding.Right - 1,
                e.Item.Size.Height - e.Item.Padding.Top - e.Item.Padding.Bottom - 1 );

            Graphics g =  e.Graphics;

            StringFormat format = new StringFormat();
            format.LineAlignment = StringAlignment.Center;
            format.FormatFlags = StringFormatFlags.NoWrap;

            if ( e.Item.TextAlign == ContentAlignment.BottomCenter || e.Item.TextAlign == ContentAlignment.TopCenter || e.Item.TextAlign == ContentAlignment.MiddleCenter )
                format.Alignment = StringAlignment.Center;
            if ( e.Item.TextAlign == ContentAlignment.BottomLeft || e.Item.TextAlign == ContentAlignment.TopLeft || e.Item.TextAlign == ContentAlignment.MiddleLeft )
                format.Alignment = StringAlignment.Near;
            if ( e.Item.TextAlign == ContentAlignment.BottomRight || e.Item.TextAlign == ContentAlignment.TopRight || e.Item.TextAlign == ContentAlignment.MiddleRight )
                format.Alignment = StringAlignment.Far;

            Brush brush = new SolidBrush( ItemForeColor );
            if ( !e.Item.Enabled )
                brush = new SolidBrush( ItemForeColor.SetAlpha( 100 ) );

            g.DrawString( e.Text, e.TextFont, brush, textRect, format );

            //Console.WriteLine( e.Text + " " + e.Item.Alignment );
        }
    }



}
