using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using Shapes        = System.Windows.Shapes;


namespace LenWeaver.Utilities {

    public static class WPFHelpers {

        static WPFHelpers() {}


        public static void                      SetCornerRadius( this ButtonBase btn, CornerRadius cornerRadius ) {

            Border?      brd;


            brd                 = btn?.Template.FindName( "border", btn ) as Border;
            if( brd == null ) throw new ArgumentException( $"Specified button control does not seem to have a border." );

            brd.CornerRadius    = cornerRadius;
        }
        public static void                      SetCornerRadius( Control control, string buttonName, CornerRadius cornerRadius ) {

            Button? btn;


            if( control == null )  throw new ArgumentNullException( $"{nameof(control)} parameter must not be null." );

            btn                     = control.Template?.FindName( buttonName, control ) as Button;
            if( btn != null ) {
                SetCornerRadius( btn, cornerRadius );
            }
            else {
                throw new ArgumentException( $"A button named {buttonName} was not found on the specified Window." );
            }
        }
        public static void                      SetMaxLength( this ComboBox cbo, int newMaxLength ) {

            TextBox         txt;

            
            txt             = (TextBox)cbo.Template.FindName( "PART_EditableTextBox", cbo );
            txt.MaxLength   = newMaxLength;
        }
        public static void                      SetSelectedBackground( this TabItem ti, Brush b ) {

            var border = ti.Template.FindName( "innerBorder", ti ) as Border;

            if( border is not null ) border.Background = b;
        }

        public static int                       Add( this CommandBindingCollection bindings, ICommand command,
                                                     ExecutedRoutedEventHandler executedHandler, CanExecuteRoutedEventHandler canExecuteHandler ) {

            return bindings.Add( new CommandBinding( command, executedHandler, canExecuteHandler ) );
        }
        public static int                       Add( this CommandBindingCollection bindings, ICommand command, ExecutedRoutedEventHandler executedHandler ) {

            return bindings.Add( new CommandBinding( command, executedHandler ) );
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
                                case "FILL":            p.Fill              = (Brush?)bc?.ConvertFromInvariantString( nameValues[1] );  break;
                                case "STROKE":          p.Stroke            = (Brush?)bc?.ConvertFromInvariantString( nameValues[1] );  break;
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

        public static T                         FindInContent<T>( this ContentControl cc, string name ) where T : FrameworkElement {

            ContentControl?     con         = cc;

            T?                  result      = null;


            while( result == null ) {
                if( con?.Content is T && String.CompareOrdinal( ((FrameworkElement)con.Content).Name, name ) == 0 ) {
                    result = con.Content as T;
                }
                else if( con?.Content != null && con.Content is ContentControl ) {
                    con = con.Content as ContentControl;
                }
            }

            return result;
        }
        public static T?                        FindInContent<T>( this ContentControl cc ) where T : class {

            ContentControl?     con         = cc;

            T?                  result      = null;


            while( result == null ) {
                if( con?.Content is T ) {
                    result = con.Content as T;
                }
                else if( con?.Content != null && con.Content is ContentControl ) {
                    con = con.Content as ContentControl;
                }
            }

            return result;
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