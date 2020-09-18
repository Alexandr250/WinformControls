using System.Drawing;

namespace WinformControls.Helpers {
    public static class AlignmentHelper {
        public static StringAlignment GetHorizontalAlignment( this ContentAlignment contentAlignment ) {
            if ( contentAlignment == ContentAlignment.BottomLeft ||
                contentAlignment == ContentAlignment.MiddleLeft ||
                contentAlignment == ContentAlignment.TopLeft ) {
                return StringAlignment.Near;
            }
            if ( contentAlignment == ContentAlignment.BottomCenter ||
                contentAlignment == ContentAlignment.MiddleCenter ||
                contentAlignment == ContentAlignment.TopCenter ) {
                return StringAlignment.Center;
            }
            if ( contentAlignment == ContentAlignment.BottomRight ||
                contentAlignment == ContentAlignment.MiddleRight ||
                contentAlignment == ContentAlignment.TopRight ) {
                return StringAlignment.Far;
            }

            return StringAlignment.Center;            
        }

        public static StringAlignment GetVerticalAlignment( this ContentAlignment contentAlignment ) {
            if ( contentAlignment == ContentAlignment.BottomLeft ||
                contentAlignment == ContentAlignment.BottomCenter ||
                contentAlignment == ContentAlignment.BottomRight ) {
                return StringAlignment.Far;
            }
            if ( contentAlignment == ContentAlignment.MiddleLeft ||
                contentAlignment == ContentAlignment.MiddleCenter ||
                contentAlignment == ContentAlignment.MiddleRight ) {
                return StringAlignment.Center;
            }
            if ( contentAlignment == ContentAlignment.TopLeft ||
                contentAlignment == ContentAlignment.TopCenter ||
                contentAlignment == ContentAlignment.TopRight ) {
                return StringAlignment.Near;
            }

            return StringAlignment.Center;
        }
    }
}
