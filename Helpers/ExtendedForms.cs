using System.Drawing;
using System.Drawing.Drawing2D;
using System;
using System.Drawing.Imaging;

namespace WinformControls.Helpers {
    public class ExtendedForms {
        public static GraphicsPath RoundedRect( Rectangle bounds, int radius ) {
            int diameter = radius * 2;

            if ( diameter > Math.Min( bounds.Height, bounds.Width ) )
                diameter = Math.Min( bounds.Height, bounds.Width );

            Size size = new Size( diameter, diameter );
            Rectangle arc = new Rectangle( bounds.Location, size );
            GraphicsPath path = new GraphicsPath();

            if ( radius == 0 ) {
                path.AddRectangle( bounds );
                return path;
            }

            // top left arc  
            path.AddArc( arc, 180, 90 );

            // top right arc  
            arc.X = bounds.Right - diameter;
            path.AddArc( arc, 270, 90 );

            // bottom right arc  
            arc.Y = bounds.Bottom - diameter;
            path.AddArc( arc, 0, 90 );

            // bottom left arc 
            arc.X = bounds.Left;
            path.AddArc( arc, 90, 90 );

            path.CloseFigure();
            return path;
        }

        public static GraphicsPath RoundedBottomRect( Rectangle bounds, int radius ) {
            int diameter = radius * 2;
            Size size = new Size( diameter, diameter );
            Rectangle arc = new Rectangle( bounds.Location, size );
            GraphicsPath path = new GraphicsPath();

            if ( radius == 0 ) {
                path.AddRectangle( bounds );
                return path;
            }

            // top right arc  
            arc.X = bounds.Right - diameter;
            //path.AddArc( arc, 270, 90 );

            // bottom right arc  
            arc.Y = bounds.Bottom - diameter;
            path.AddArc( arc, 0, 90 );

            // bottom left arc 
            arc.X = bounds.Left;
            path.AddArc( arc, 90, 90 );

            path.AddLine( bounds.Left, bounds.Top, bounds.Right, bounds.Top );

            path.CloseFigure();
            return path;
        }

        public static GraphicsPath RoundedTopRect( Rectangle bounds, int radius ) {
            int diameter = radius * 2;
            Size size = new Size( diameter, diameter );
            Rectangle arc = new Rectangle( bounds.Location, size );
            GraphicsPath path = new GraphicsPath();

            if ( radius == 0 ) {
                path.AddRectangle( bounds );
                return path;
            }

            // top left arc  
            path.AddArc( arc, 180, 90 );

            // top right arc  
            arc.X = bounds.Right - diameter;
            path.AddArc( arc, 270, 90 );
            
            path.AddLine( bounds.Right, bounds.Bottom, bounds.Left, bounds.Bottom );

            path.CloseFigure();
            return path;
        }

        public static GraphicsPath RoundedRightRect( Rectangle bounds, int radius ) {
            int diameter = radius * 2;
            Size size = new Size( diameter, diameter );
            Rectangle arc = new Rectangle( bounds.Location, size );
            GraphicsPath path = new GraphicsPath();

            if ( radius == 0 ) {
                path.AddRectangle( bounds );
                return path;
            }


            path.AddLine( bounds.Left, bounds.Bottom, bounds.Left, bounds.Top );

            // top left arc  
            //path.AddArc( arc, 180, 90 );

            // top right arc  
            arc.X = bounds.Right - diameter;
            path.AddArc( arc, 270, 90 );

            // bottom right arc  
            arc.Y = bounds.Bottom - diameter;
            path.AddArc( arc, 0, 90 );

            // bottom left arc 
            arc.X = bounds.Left;
            //path.AddArc( arc, 90, 90 );

            path.CloseFigure();
            return path;
        }

        public static GraphicsPath RoundedLeftRect( Rectangle bounds, int radius ) {
            int diameter = radius * 2;
            Size size = new Size( diameter, diameter );
            Rectangle arc = new Rectangle( bounds.Location, size );
            GraphicsPath path = new GraphicsPath();

            if ( radius == 0 ) {
                path.AddRectangle( bounds );
                return path;
            }


            path.AddLine( bounds.Right, bounds.Bottom, bounds.Right, bounds.Top );

            // top left arc  
            path.AddArc( arc, 270, -90 );
            
            // bottom right arc  
            arc.Y = bounds.Bottom - diameter;
            
            // bottom left arc 
            arc.X = bounds.Left;
            path.AddArc( arc, 180, -90 );

            path.CloseFigure();
            return path;
        }

        public static GraphicsPath SkewedRect( Rectangle bounds, int skew ) {
            GraphicsPath path = new GraphicsPath();

            if ( skew == 0 ) {
                path.AddRectangle( bounds );
                return path;
            }
            
            path.AddLine( bounds.Left + skew, bounds.Top, bounds.Width - skew, bounds.Top );
            path.AddLine( bounds.Width - skew, bounds.Top, bounds.Width, bounds.Top + skew );
            path.AddLine( bounds.Width, bounds.Top + skew, bounds.Width, bounds.Height - skew );
            path.AddLine( bounds.Width, bounds.Height - skew, bounds.Width - skew, bounds.Height );
            path.AddLine( bounds.Width - skew, bounds.Height, bounds.Left + skew, bounds.Height );
            path.AddLine( bounds.Left + skew, bounds.Height, bounds.Left, bounds.Height - skew );
            path.AddLine( bounds.Left, bounds.Height - skew, bounds.Left, bounds.Top + skew );
            path.AddLine( bounds.Left, bounds.Top + skew, bounds.Left + skew, bounds.Top );

            path.CloseFigure();
            return path;
        }

        public static Bitmap MakeGrayscale3( Bitmap original ) {
            Bitmap newBitmap = new Bitmap( original.Width, original.Height, PixelFormat.Format32bppArgb );
            Graphics g = Graphics.FromImage( newBitmap );
            
            //ColorMatrix colorMatrix = new ColorMatrix( new float[][] 
            //      {
            //         new float[] { .3f, .3f, .3f, 0, 0 },
            //         new float[] { .59f, .59f, .59f, 0, 0 },
            //         new float[] { .11f, .11f, .11f, 0, 0 },
            //         new float[] { 0, 0, 0, 1, 0 },
            //         new float[] { 0, 0, 0, 0, 1 }
            //      } );

            ColorMatrix colorMatrix = new ColorMatrix( new float[][] 
                  {
                     new float[] { 0.4f, 0.3f, 0.3f, 0, 0 },
                     new float[] { 0.6f, 0.7f, 0.6f, 0, 0 },
                     new float[] { 0.1f, 0.1f, 0.2f, 0, 0 },
                     new float[] { 0, 0, 0, 0.2f, 0 },
                     new float[] { 0, 0, 0, 0, 1 }
                  } );
            
            ImageAttributes attributes = new ImageAttributes();
            attributes.SetColorMatrix( colorMatrix );

            g.DrawImage( original, new Rectangle( 0, 0, original.Width, original.Height ),
               0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes );

            g.Dispose();
            return newBitmap;

            
           
        }
    }
}
