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
                if( globalMonospace is null ) {
                    globalMonospace = new FontFamily( "Global Monospace" );

                    if( !Fonts.SystemFontFamilies.Contains( globalMonospace ) ) {
                        throw new ArgumentException( "FontFamily: Global Monospace, does not seem to exist." );
                    }
                }

                return globalMonospace;
            }
        }
        public static FontFamily        GlobalSansSerif {
            get {
                if( globalSansSerif is null ) {
                    globalSansSerif = new FontFamily( "Global Sans Serif" );

                    if( !Fonts.SystemFontFamilies.Contains( globalSansSerif ) ) {
                        throw new ArgumentException( "FontFamily: Global Sans Serif, does not seem to exist." );
                    }
                }

                return globalSansSerif;
            }
        }
        public static FontFamily        GlobalUserInterface {
            get {
                if( globalUserInterface is null ) {
                    globalUserInterface = new FontFamily( "Global User Interface" );

                    if( !Fonts.SystemFontFamilies.Contains( globalUserInterface ) ) {
                        throw new ArgumentException( "FontFamily: Global User Interface, does not seem to exist." );
                    }
                }

                return globalUserInterface;
            }
        }
        public static FontFamily        SegoeFluentIcons {
            get {
                if( segoeFluentIcons is null ) {
                    segoeFluentIcons = new FontFamily( "Segoe Fluent Icons" );

                    if( !Fonts.SystemFontFamilies.Contains( segoeFluentIcons ) ) {
                        throw new ArgumentException( "FontFamily: Segoe Fluent Icons, does not seem to exist." );
                    }
                }

                return segoeFluentIcons;
            }
        }
        public static FontFamily        SegoeMDL2Assets {
            get {
                if( segoeMDL2Assets is null ) {
                    segoeMDL2Assets = new FontFamily( "Segoe MDL2 Assets" );

                    if( !Fonts.SystemFontFamilies.Contains( segoeMDL2Assets ) ) {
                        throw new ArgumentException( "FontFamily: Segoe MDL2 Assets, does not seem to exist." );
                    }
                }

                return segoeMDL2Assets;
            }
        }
        public static FontFamily        SegoeUI {
            get {
                if( segoeUI is null ) {
                    segoeUI = new FontFamily( "Segoe UI" );

                    if( !Fonts.SystemFontFamilies.Contains( segoeUI ) ) {
                        throw new ArgumentException( "FontFamily: Segoe UI, does not seem to exist." );
                    }
                }

                return segoeUI;
            }
        }
        public static FontFamily        SegoeUISymbol {
            get {
                if( segoeUISymbol is null ) {
                    segoeUISymbol = new FontFamily( "Segoe UI Symbol" );

                    if( !Fonts.SystemFontFamilies.Contains( segoeUISymbol ) ) {
                        throw new ArgumentException( "FontFamily: Segoe UI Symbol, does not seem to exist." );
                    }
                }

                return segoeUISymbol;
            }
        }


        public static bool              IsFixedWidth( this FontFamily ff ) {

            bool                result      = false;

            
            foreach( Typeface tf in ff.GetTypefaces() ) {
                result  = tf.IsFixedWidth();

                break;
            }

            return result;
        }
        public static bool              IsFixedWidth( this Typeface tf ) {

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

        public static string            AsString( this SegoeMDL2Assets assetName ) {

            return Char.ConvertFromUtf32( (int)assetName );
        }

        public static FlowDirection     GetFlowDirection( this System.Globalization.CultureInfo culture ) {

            return culture.TextInfo.IsRightToLeft ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;
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
    }
}