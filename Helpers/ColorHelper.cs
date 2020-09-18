using System.Drawing;
using System;

namespace WinformControls {
    public static class ColorHelper {
        public static Color Lighten( this Color baseColor, int value ) {
            int red = baseColor.R;
            int green = baseColor.G;
            int blue = baseColor.B;

            red += value;
            if ( red > 255 )
                red = 255;

            green += value;
            if ( green > 255 )
                green = 255;

            blue += value;
            if ( blue > 255 )
                blue = 255;

            return Color.FromArgb( baseColor.A, red, green, blue );
        }

        public static Color Darken( this Color baseColor, int value ) {
            int red = baseColor.R;
            int green = baseColor.G;
            int blue = baseColor.B;

            red -= value;
            if ( red < 0 )
                red = 0;

            green -= value;
            if ( green < 0 )
                green = 0;

            blue -= value;
            if ( blue < 0 )
                blue = 0;

            return Color.FromArgb( baseColor.A, red, green, blue );
        }

        public static Color SetAlpha( this Color baseColor, int value ) {
            int alpha = value;
            if ( alpha < 0 )
                alpha = 0;

            if ( alpha > 255 )
                alpha = 255;

            return Color.FromArgb( alpha, baseColor.R, baseColor.G, baseColor.B );
        }

        public static string HexRgb( this Color color ) {
            return string.Format( "#{0}{1}{2}", color.R.ToString( "X2" ), color.G.ToString( "X2" ), color.B.ToString( "X2" ) );
        }
    }
}
