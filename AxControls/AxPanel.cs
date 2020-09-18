using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using WinformControls.Common;
using System.Drawing;
using WinformControls.Helpers;
using System.Drawing.Drawing2D;

namespace WinformControls.AxControls {
    public class AxPanel : Panel {
        private StateStyle _normalStyle;
        private int _borderRadius = 5;

        /// <summary>
        /// Стиль контрола
        /// </summary>
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
                _normalStyle.PropertyChanged += ( property ) => this.Refresh();                 
                Refresh();
            }
        }

        /// <summary>
        /// Радиус границы
        /// </summary>
        [Browsable( true )]
        public int BorderRadius {
            get {
                return _borderRadius;
            }
            set {
                if ( _borderRadius == value )
                    return;

                int biggestSide = this.Height;
                if ( this.Width < this.Height )
                    biggestSide = this.Width;

                if ( value < 0 ) {
                    _borderRadius = 0;
                    this.Refresh();
                    return;
                }

                if ( value > biggestSide / 2 ) {
                    _borderRadius = biggestSide / 2;
                    this.Refresh();
                    return;
                }

                _borderRadius = value;
                this.Refresh();
            }
        }

        /// <summary>
        /// Просто скрывает одноименное свойство
        /// </summary>
        [Browsable( false )]
        public new BorderStyle BorderStyle { get; set; }

        /// <summary>
        /// Перенаправляет стандартное свойство на NormalStyle
        /// </summary>
        [Browsable( true )]
        public override Color BackColor {
            get {
                if ( NormalStyle != null )
                    return NormalStyle.BackColor;
                
                return Color.White;
            }
            set {
                if ( NormalStyle != null )
                    NormalStyle.BackColor = value;
            }
        }

        #region Instance
        /// <summary>
        /// Конструктор
        /// </summary>
        public AxPanel() {
            this.SetStyle( ControlStyles.AllPaintingInWmPaint
                | ControlStyles.OptimizedDoubleBuffer
                | ControlStyles.ResizeRedraw
                | ControlStyles.UserPaint, true );

            base.BorderStyle = BorderStyle.None;

            _normalStyle = new StateStyle() {
                BackColor = Color.White,
                ForeColor = Color.Black,
                BorderColor = Color.Black
            };

            _normalStyle.PropertyChanged += ( property ) => this.Refresh(); 
        }
        #endregion

        protected override void OnPaint( PaintEventArgs e ) {
            Graphics g  = e.Graphics;

            Rectangle clientRectangle = new Rectangle( 0, 0, this.Width - 1, this.Height - 1 );

            if ( Parent != null )
                g.Clear( Parent.BackColor );

            if ( BorderRadius > 0 )
                g.SmoothingMode = SmoothingMode.AntiAlias;

            g.FillPath( NormalStyle.BackBrush, ExtendedForms.RoundedRect( clientRectangle, BorderRadius ) );
            g.DrawPath( NormalStyle.BorderPen, ExtendedForms.RoundedRect( clientRectangle, BorderRadius ) );
        }
    }
}
