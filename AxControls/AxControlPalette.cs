using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using WinformControls.Common;
using System.Drawing;

namespace WinformControls.AxControls {
    public class AxControlPalette : Component {
        private StateStyle _normalStyle;
        private StateStyle _hoverStyle;
        private StateStyle _disabledStyle;

        [Browsable( true )]
        [TypeConverter( typeof( ExpandableObjectConverter ) )]
        public StateStyle NormalStyle {
            get {
                return _normalStyle;
            }
            set {
                if ( _normalStyle != null && _normalStyle.Equals( value )  )
                    return;

                _normalStyle = value;
                Console.WriteLine( "CommonControlStyle.NormalStyle.Set." );
            }
        }

        [Browsable( true )]
        [TypeConverter( typeof( ExpandableObjectConverter ) )]
        public StateStyle HoverStyle {
            get {
                return _hoverStyle;
            }
            set {
                if ( _hoverStyle != null && _hoverStyle.Equals( value ) )
                    return;

                _hoverStyle = value;
                Console.WriteLine( "CommonControlStyle.HoverStyle.Set." );
            }
        }

        [Browsable( true )]
        [TypeConverter( typeof( ExpandableObjectConverter ) )]
        public StateStyle DisabledStyle {
            get {
                return _disabledStyle;
            }
            set {
                if ( _disabledStyle != null && _disabledStyle.Equals( value ) )
                    return;

                _disabledStyle = value;
                Console.WriteLine( "CommonControlStyle.DisabledStyle.Set." );
            }
        }

        public AxControlPalette() {
            _normalStyle = new StateStyle() {
                BackColor = Color.White,
                ForeColor = Color.Black,
                BorderColor = Color.Gray
            };

            _hoverStyle = new StateStyle() {
                BackColor = Color.White,
                ForeColor = Color.Black,
                BorderColor = Color.Gray
            };

            _disabledStyle = new StateStyle() {
                BackColor = Color.White,
                ForeColor = Color.Black,
                BorderColor = Color.Gray
            };
        }
    }
}
