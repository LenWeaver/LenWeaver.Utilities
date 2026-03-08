using System;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;


namespace LenWeaver.Utilities {

    public static class ImageExtensions {

        static ImageExtensions() {}
        

        public static BitmapImage       CreateBitmapImage( Uri uri, int pixelHeight, int pixelWidth ) {

            BitmapImage     result      = new BitmapImage();


            result.BeginInit();
            result.UriSource            = uri;
            result.DecodePixelHeight    = pixelHeight;
            result.DecodePixelWidth     = pixelWidth;
            result.EndInit();
            
            return result;
        }
        public static BitmapImage       CreateBitmapImage( string uriString, int pixelHeight, int pixelWidth ) {

            return CreateBitmapImage( new Uri( uriString, UriKind.RelativeOrAbsolute ), pixelHeight, pixelWidth );
        }

        public static BitmapImage       ToBitmapImage( this System.Drawing.Bitmap bmp ) {

            BitmapImage     result;

            MemoryStream?   ms          = null;

            
            try {
                ms                      = new MemoryStream();

                bmp.Save( ms, ImageFormat.Png );

                ms.Position             = 0;

                result                  = new BitmapImage();
                result.BeginInit();
                result.StreamSource     = ms;
                result.CacheOption      = BitmapCacheOption.OnLoad;
                result.EndInit();
            }
            catch( Exception ex ) {
                throw new ApplicationException( "Unable to convert Bitmap to BitmapImage.", ex );
            }
            finally {
                if( ms != null ) ms.Dispose();
            }

            return result;
        }
        public static BitmapImage       ToBitmapImage( this System.Drawing.Icon ico ) {

            return ToBitmapImage( ico.ToBitmap() );
        }

        public static ImageSource       CharacterToBitmap( this FontFamily ff, int unicodePoint, int pixelHeight, int pixelWidth, Brush foreground, Brush background ) {

            GeometryDrawing         gd;
            TextBlock               tb;
            VisualBrush             vb;


            tb                      = new TextBlock();
            tb.Background           = background;
            tb.Foreground           = foreground;
            tb.FontFamily           = ff;
            tb.Text                 = Char.ConvertFromUtf32( unicodePoint );

            vb                      = new VisualBrush();
            vb.Visual               = tb;
            vb.Stretch              = Stretch.Uniform;

            gd                      = new GeometryDrawing();
            gd.Brush                = vb;
            gd.Geometry             = new RectangleGeometry( new Rect( 0d, 0d, (double)pixelHeight, (double)pixelWidth ) );

            return new DrawingImage( gd );
        }
        public static ImageSource       CharacterToBitmap( this FontFamily ff, int unicodePoint, int pixelHeight, int pixelWidth ) {

            return CharacterToBitmap( ff, unicodePoint, pixelHeight, pixelWidth, Brushes.Black, Brushes.White );
        }
    }
}