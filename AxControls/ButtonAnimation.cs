using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinformControls.AxControls {
    internal static class ButtonAnimation {
        private static Timer _timer;
        private static AxButton _targetButton;
        private static Point _mousePos;

        public static bool Animated { get; set; }
        public static int CurrentValue { get; set; }
        public static Point MousePos {
            get {
                return _mousePos;
            }
            set {
                _mousePos = value;
                if ( _targetButton != null ) {
                    int length1 = ( int ) Math.Sqrt( Math.Pow( MousePos.X, 2 ) + Math.Pow( MousePos.Y, 2 ) );
                    int length2 = ( int ) Math.Sqrt( Math.Pow( _targetButton.Width - MousePos.X, 2 ) + Math.Pow( MousePos.Y, 2 ) );
                    int length3 = ( int ) Math.Sqrt( Math.Pow( MousePos.X, 2 ) + Math.Pow( _targetButton.Height - MousePos.Y, 2 ) );
                    int length4 = ( int ) Math.Sqrt( Math.Pow( _targetButton.Width - MousePos.X, 2 ) + Math.Pow( _targetButton.Height - MousePos.Y, 2 ) );
                    int length = Math.Max( Math.Max( length1, length2 ), Math.Max( length3, length4 ) );

                    MaxValue = length;
                }
                else
                    MaxValue = 0;
            }
        }
        private static int MaxValue { get; set; }

        public static AxButton TargetButton {
            get {
                return _targetButton;
            }
            set {
                if ( _targetButton != null && _targetButton.Equals( value ) )
                    return;

                if ( _targetButton != null ) {
                    _timer.Stop();
                    Animated = false;
                    _targetButton.Refresh();
                }

                _targetButton = value;
                Animated = true;
                CurrentValue = 0;
                if ( _targetButton != null ) {
                    int length1 = ( int ) Math.Sqrt( Math.Pow( MousePos.X, 2 ) + Math.Pow( MousePos.Y, 2 ) );
                    int length2 = ( int ) Math.Sqrt( Math.Pow( _targetButton.Width - MousePos.X, 2 ) + Math.Pow( MousePos.Y, 2 ) );
                    int length3 = ( int ) Math.Sqrt( Math.Pow( MousePos.X, 2 ) + Math.Pow( _targetButton.Height - MousePos.Y, 2 ) );
                    int length4 = ( int ) Math.Sqrt( Math.Pow( _targetButton.Width - MousePos.X, 2 ) + Math.Pow( _targetButton.Height - MousePos.Y, 2 ) );
                    int length = Math.Max( Math.Max( length1, length2 ), Math.Max( length3, length4 ) );

                    MaxValue = length;
                }
                else
                    MaxValue = 0;

                _timer.Start();
            }
        }

        static ButtonAnimation() {
            _timer = new Timer();
            _timer.Interval = 10;
            _timer.Tick += ( s, e ) => {
                CurrentValue += 2;
                if ( CurrentValue > MaxValue ) {
                    Animated = false;
                    _timer.Stop();
                }

                if ( _targetButton != null ) {
                    _targetButton.Refresh();
                }
            };
        }

        public static void Restart() {
            if ( _targetButton != null )
                _timer.Start();
        }

    }
}
