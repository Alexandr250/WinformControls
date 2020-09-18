using System.Drawing;
using System.ComponentModel;
using System;
using System.Globalization;
using System.Drawing.Design;
using WinformControls.AxControls.Designer;

namespace WinformControls.Common {
    public class StateStyle : IDisposable {
        private Color _foreColor;
        private Color _backColor;
        private Color _borderColor;

        private bool _disposed = false;

        private byte _innerBorderPenLightenValue = 0;

        public Color ForeColor {
            get {
                return _foreColor;
            }
            set {
                if ( _foreColor == value )
                    return;

                _foreColor = value;

                if ( ForePen != null )
                    ForePen.Dispose();

                ForePen = new Pen( _foreColor );

                if ( ForeBrush != null )
                    ForeBrush.Dispose();

                ForeBrush = new SolidBrush( _foreColor );

                if ( PropertyChanged != null )
                    PropertyChanged( "ForeColor" );
            }
        }

        [Browsable( false )]
        public Pen ForePen { get; private set; }

        [Browsable( false )]
        public SolidBrush ForeBrush { get; private set; }

        public Color BackColor {
            get {
                return _backColor;
            }
            set {
                if ( _backColor == value )
                    return;

                _backColor = value;

                if ( BackPen != null )
                    BackPen.Dispose();

                BackPen = new Pen( _backColor );

                if ( BackBrush != null )
                    BackBrush.Dispose();

                BackBrush = new SolidBrush( _backColor );

                if ( InnerBorderPen != null )
                    InnerBorderPen.Dispose();

                InnerBorderPen = new Pen( _backColor.Lighten( _innerBorderPenLightenValue ) );

                if ( PropertyChanged != null )
                    PropertyChanged( "BackColor" );
            }
        }

        [Browsable( false )]
        public Pen BackPen { get; private set; }

        [Browsable( false )]
        public SolidBrush BackBrush { get; private set; }

        public Color BorderColor {
            get {
                return _borderColor;
            }
            set {
                if ( _borderColor == value )
                    return;

                _borderColor = value;

                if ( BorderPen != null )
                    BorderPen.Dispose();

                BorderPen = new Pen( _borderColor );

                if ( BorderBrush != null )
                    BorderBrush.Dispose();

                BorderBrush = new SolidBrush( _borderColor );

                if ( PropertyChanged != null )
                    PropertyChanged( "BorderColor" );
            }
        }

        [Browsable( false )]
        public Pen BorderPen { get; private set; }

        [Browsable( false )]
        public Pen InnerBorderPen { get; private set; }

        [Browsable( false )]
        public SolidBrush BorderBrush { get; private set; }

        [BrowsableAttribute( true )]
        [EditorAttribute( typeof( IntEditor ), typeof( UITypeEditor ) )]
        public byte InnerBorderPenLightenValue {
            get {
                return _innerBorderPenLightenValue;
            }
            set {
                if ( InnerBorderPen != null )
                    InnerBorderPen.Dispose();

                _innerBorderPenLightenValue = value;

                InnerBorderPen = new Pen( _backColor.Lighten( _innerBorderPenLightenValue ) );
            }
        }
        /// <summary>
        /// Срабатывает когда меняется какое-либо свойство класса StateStyle
        /// </summary>
        public event Action<string> PropertyChanged;        

        public override string ToString() {
            return ForeColor.ToString() + ", " + BackColor.ToString() + ", " + BorderColor.ToString();
        }

        public override bool Equals( object obj ) {
            if ( obj == null )
                return false;
            if ( obj == this )
                return true;

            if ( !( obj is StateStyle ) )
                return false;

            StateStyle targetStyle = ( StateStyle ) obj;

            if ( targetStyle.BackColor == this.BackColor && targetStyle.ForeColor == this.ForeColor && targetStyle.BorderColor == this.BorderColor )
                return true;

            return false;
        }

        public void Dispose() {
            if ( !_disposed ) {
                _disposed = true;
                ResetHandlers();
            }
        }

        private void ResetHandlers() {
            PropertyChanged = null;
        }
    }
}
