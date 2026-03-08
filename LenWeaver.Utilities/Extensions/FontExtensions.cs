using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace LenWeaver.Utilities {

    public static class FontExtensions {

        #region Static Property Backing Fields
        private static FontFamily?              globalMonospace         = null;
        private static FontFamily?              globalSansSerif         = null;
        private static FontFamily?              globalUserInterface     = null;
        private static FontFamily?              segoeFluentIcons        = null;
        private static FontFamily?              segoeMDL2Assets         = null;
        private static FontFamily?              segoeUI                 = null;
        private static FontFamily?              segoeUISymbol           = null;
        #endregion


        static FontExtensions() {}


        //Fonts I often use in my projects.
        public static FontFamily        GlobalMonospace {
            get {
                CreateIfNecessary( ref globalMonospace, "Global Monospace" );

                return globalMonospace!;
            }
        }
        public static FontFamily        GlobalSansSerif {
            get {
                CreateIfNecessary( ref globalSansSerif, "Global Sans Serif" );

                return globalSansSerif!;
            }
        }
        public static FontFamily        GlobalUserInterface {
            get {
                CreateIfNecessary( ref globalUserInterface, "Global User Interface" );

                return globalUserInterface!;
            }
        }
        public static FontFamily        SegoeFluentIcons {
            get {
                CreateIfNecessary( ref segoeFluentIcons, "Segoe Fluent Icons" );

                return segoeFluentIcons!;
            }
        }
        public static FontFamily        SegoeMDL2Assets {
            get {
                CreateIfNecessary( ref segoeMDL2Assets, "Segoe MDL2 Assets" );

                return segoeMDL2Assets!;
            }
        }
        public static FontFamily        SegoeUI {
            get {
                CreateIfNecessary( ref segoeUI, "Segoe UI" );

                return segoeUI!;
            }
        }
        public static FontFamily        SegoeUISymbol {
            get {
                CreateIfNecessary( ref segoeUISymbol, "Segoe UI Symbol" );

                return segoeUI!;
            }
        }


        extension( FontFamily ff ) {
            public bool         IsFixedWidth {
                get {
                    bool                result      = false;

            
                    foreach( Typeface tf in ff.GetTypefaces() ) {
                        result  = tf.IsFixedWidth;

                        break;
                    }

                    return result;
                }
            }
        }
        extension( Typeface tf ) {
            public bool          IsFixedWidth {
                get {

                    double              iWidth;
                    double              wWidth;

                    FormattedText       ft;


                    ft          = new FormattedText( "i", System.Globalization.CultureInfo.CurrentCulture,
                                                     FlowDirection.LeftToRight, tf, 10d, Brushes.Black, 1d );
                    iWidth      = ft.Width;

                    ft          = new FormattedText( "W", System.Globalization.CultureInfo.CurrentCulture,
                                                     FlowDirection.LeftToRight, tf, 10d, Brushes.Black, 1d );
                    wWidth      = ft.Width;


                    return iWidth == wWidth;
                }
            }
        }


        public static string            ToCharacter( this SegoeMDL2Assets assetName ) {

            return Char.ConvertFromUtf32( (int)assetName );
        }

        public static FlowDirection     GetFlowDirection( this System.Globalization.CultureInfo culture ) {

            return culture.TextInfo.IsRightToLeft ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;
        }

        public static int               CompareTo( this FontStretch stretch, FontStretch other ) {

            return stretch.ToOpenTypeStretch().CompareTo( other.ToOpenTypeStretch() );
        }
        public static int               CompareTo( this FontStyle style, FontStyle other ) {

            return style.GetHashCode().CompareTo( other.GetHashCode() );
        }
        public static int               CompareTo( this FontWeight weight, FontWeight other ) {

            return weight.ToOpenTypeWeight().CompareTo( other.ToOpenTypeWeight() );
        }
        public static int               CompareTo( this Typeface tf, Typeface other ) {

            int result;


            result = tf.Stretch.CompareTo( other.Stretch );
            if( result == 0 ) result = tf.Style.CompareTo( other.Style );
            if( result == 0 ) result = tf.Weight.CompareTo( other.Weight );

            return result;
        }

        public static Typeface?         GetTypeface( this FontFamily ff, FontStretch fontStretch, FontStyle fontStyle, FontWeight fontWeight ) {

            Typeface?   result  = null;


            foreach( Typeface tf in ff.GetTypefaces() ) {
                if( tf.Stretch == fontStretch && tf.Style == fontStyle && tf.Weight == fontWeight ) {
                    result = tf;
                    break;
                }
            }

            return result;
        }
        public static Typeface?         GetTypeface( this FontFamily ff, Control c ) {

            return ff.GetTypeface( c.FontStretch, c.FontStyle, c.FontWeight );
        }
        public static Typeface?         GetTypeface( this Control c ) {

            return c.FontFamily.GetTypeface( c );
        }


        private static void             CreateIfNecessary( ref FontFamily? ff, string fontFamilyName ) {

            if( ff is null ) {
                ff = new FontFamily( fontFamilyName );

                if( !Fonts.SystemFontFamilies.Contains( ff ) ) {
                    throw new ArgumentException( $"FontFamily: {fontFamilyName}, does not seem to exist." );
                }
            }
        }
    }
}