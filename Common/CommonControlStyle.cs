using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace WinformControls.Common {
    public static class CommonControlStyle {
        private static StateStyle _normalStyle;
        private static StateStyle _hoverStyle;
        private static StateStyle _disabledStyle;

        public static StateStyle NormalStyle {
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
        public static StateStyle HoverStyle {
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
        public static StateStyle DisabledStyle {
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

        static CommonControlStyle() {
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
