using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Shapes = System.Windows.Shapes;
using SysDrawing = System.Drawing;

namespace LenWeaver.Utilities {

    public static class WPFHelpers {

        static WPFHelpers() {}
        

        public static void                      SetButtonCornerRadius( this ButtonBase btn, CornerRadius cornerRadius ) {

            Border?      brd;


            brd                 = btn?.Template.FindName( "border", btn ) as Border;
            if( brd == null ) throw new ArgumentException( $"Specified button control does not seem to have a border." );

            brd.CornerRadius    = cornerRadius;
        }
        public static void                      SetButtonCornerRadius( Control control, string buttonName, CornerRadius cornerRadius ) {

            Button? btn;


            if( control == null )  throw new ArgumentNullException( $"{nameof(control)} parameter must not be null." );

            btn                     = control.Template?.FindName( buttonName, control ) as Button;
            if( btn != null ) {
                SetButtonCornerRadius( btn, cornerRadius );
            }
            else {
                throw new ArgumentException( $"A button named {buttonName} was not found on the specified Window." );
            }
        }
        public static void                      SetComboBoxMaxLength( this ComboBox cbo, int newMaxLength ) {

            TextBox         txt;

            
            txt             = (TextBox)cbo.Template.FindName( "PART_EditableTextBox", cbo );
            txt.MaxLength   = newMaxLength;
        }

        public static bool                      IsFixedWidth( this FontFamily ff ) {

            bool                result      = false;

            
            foreach( Typeface tf in ff.GetTypefaces() ) {
                result  = tf.IsFixedWidth();

                break;
            }

            return result;
        }
        public static bool                      IsFixedWidth( this Typeface tf ) {

            double              iWidth;
            double              wWidth;

            FormattedText       ft;


            ft          = new FormattedText( "i", System.Globalization.CultureInfo.CurrentCulture,
                                             FlowDirection.LeftToRight, tf, 10d, Brushes.Black, 1.25d );
            iWidth      = ft.Width;

            ft          = new FormattedText( "W", System.Globalization.CultureInfo.CurrentCulture,
                                             FlowDirection.LeftToRight, tf, 10d, Brushes.Black, 1.25d );
            wWidth      = ft.Width;


            return iWidth == wWidth;
        }

        public static Binding                   CreateBinding( object bindingSource, string propertySource ) {
            
            Binding         result;


            result              = new Binding( propertySource );
            result.Source       = bindingSource;

            return result;
        }

        public static Shapes.Path               ExtendedPathMarkupToPath( Shapes.Path p, string markup ) {

            int                 index;

            string              data;
            string              extended;

            string[]            nameValues;

            BrushConverter      bc;


            // The 'extended' part of the markup is used to set the Fill, Stroke
            // and StrokeThickness properties.  Usage is as follows:
            //
            //  "{Fill=Red,Stroke=#FFFF0000,StrokeThickness=1}M0,0 0,100, 100,100, 100,0 Z"
            //
            //TODO: Allow the user to specify brush type for both Fill and Stroke.
            if( markup.StartsWith( "{" ) ) {
                index               = markup.IndexOf( '}' );
                if( index > 0 && index < markup.Length - 1 ) {
                    extended        = markup.Substring( 1, index - 1);
                    data            = markup.Substring( index + 1 );

                    bc              = new BrushConverter();

                    foreach( string token in extended.Trim().ToUpper().Split( ',' ) ) {
                        nameValues  = token.Split( '=' );
                        if( nameValues.Length == 2 ) {
                            switch( nameValues[0] ) {
                                case "FILL":            p.Fill              = (Brush)bc.ConvertFromInvariantString( nameValues[1] );    break;
                                case "STROKE":          p.Stroke            = (Brush)bc.ConvertFromInvariantString( nameValues[1] );    break;
                                case "STROKETHICKNESS": p.StrokeThickness   = Double.Parse( nameValues[1] );                            break;
                            }
                        }
                    }
                }
                else {
                    throw new ArgumentException( "Malformed extended path markup string." );
                }
            }
            else {
                data                = markup;
            }

            p.Data                  = Geometry.Parse( data );

            return p;
        }
        public static Shapes.Path               ExtendedPathMarkupToPath( string markup ) {

            return ExtendedPathMarkupToPath( new Shapes.Path(), markup );
        }

        public static BitmapImage               ToBitmapImage( this SysDrawing.Bitmap bmp ) {

            BitmapImage     result;

            MemoryStream?   ms          = null;;


            try {
                ms                  = new MemoryStream();

                bmp.Save( ms, ImageFormat.Png );

                ms.Position         = 0;

                result              = new BitmapImage();
                result.BeginInit();
                result.StreamSource = ms;
                result.CacheOption  = BitmapCacheOption.OnLoad;
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
        public static BitmapImage               ToBitmapImage( this SysDrawing.Icon ico ) {

            return ToBitmapImage( ico.ToBitmap() );
        }

        public static T?                        FindInTreeView<T>( this ItemCollection items, Predicate<T> found ) where T : TreeViewItem {

            T?      result  = null;


            for( int index = 0; index < items.Count; index++ ) {
                if( found( (T)items[index] ) ) {
                    result = (T)items[index];
                    break;
                }
            }

            return result;
        }

        public static IEnumerable<T>            ForEach<T>( this ItemCollection items ) where T : ItemsControl {

            foreach( T? item in items ) {
                if( item != null ) { 
                    yield return item;

                    if( item.HasItems ) {
                        foreach( T subItem in ForEach<T>( item.Items ) ) {
                            yield return subItem;
                        }
                    }
                }
            }
        }
        public static IEnumerable<T>            ForEach<T>( this TreeView tvw ) where T : TreeViewItem {

            return ForEach<T>( tvw.Items );
        }

        public static IEnumerable<T>            ForEachChild<T>( this Visual parent ) where T : Visual {

            int         childCount;

            Visual      child;


            for( int index = 0; index < VisualTreeHelper.GetChildrenCount( parent ); index++ ) {
                child           = (Visual)VisualTreeHelper.GetChild( parent, index );

                if( child is T ) yield return (T)child;

                childCount      = VisualTreeHelper.GetChildrenCount( child );
                if( childCount > 0 ) {
                    foreach( Visual v in ForEachChild<T>( child ) ) {
                        if( v is T ) yield return (T)v;
                    }
                }
            }
        }
        public static IEnumerable<Visual>       ForEachChild( this Visual parent ) {

            return ForEachChild<Visual>( parent );
        }
    }
}