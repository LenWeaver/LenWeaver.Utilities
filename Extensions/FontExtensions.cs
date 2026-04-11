using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace LenWeaver.Utilities {

    public static class FontExtensions {

        #region Static Property Backing Fields
        private static FontFamily?      globalMonospace         = null;
        private static FontFamily?      globalSansSerif         = null;
        private static FontFamily?      globalUserInterface     = null;
        private static FontFamily?      segoeFluentIcons        = null;
        private static FontFamily?      segoeMDL2Assets         = null;
        private static FontFamily?      segoeUI                 = null;
        private static FontFamily?      segoeUISymbol           = null;
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
            public bool                 IsEmojiFont {
                get {
                    bool                result  = true;


                    foreach( Typeface tf in ff.GetTypefaces() ) {
                        if( !tf.IsEmojiFont ) {
                            result = false;
                            break;
                        }
                    }

                    return result;
                }
            }
            public bool                 IsMonospace {
                get {
                    bool                result      = true;

            
                    foreach( Typeface tf in ff.GetTypefaces() ) {
                        if( !tf.IsMonospaceFont ) {
                            result  = false;
                            break;
                        }
                    }

                    return result;
                }
            }
            public bool                 IsSymbolFont {
                get {
                    bool                result      = true;


                    foreach( Typeface tf in ff.GetTypefaces() ) {
                        if( !tf.IsSymbolFont ) {
                            result  = false;
                            break;
                        }
                    }

                    return result;
                }
            }
        }
        extension( Typeface tf ) {
            public bool                 IsEmojiFont {
                get {
                    bool            result  = false;

                    GlyphTypeface   gtf;

                    if( tf.TryGetGlyphTypeface( out gtf ) ) {
                        foreach( KeyValuePair<int,ushort> k in gtf.CharacterToGlyphMap ) {

                            if( (k.Key >= 0x1F300 && k.Key <= 0x1FAFF) ||
                                (k.Key >= 0x2600  && k.Key <= 0x26FF) ) {

                                result = true;

                                break;
                            }
                        }

                        result = result || tf.FontFamily.Source.Contains( "emoji", StringComparison.OrdinalIgnoreCase );
                    }

                    return result;
                }
            }
            public bool                 IsGlyphBacked {
                get => tf.TryGetGlyphTypeface( out _ );
            }
            public bool                 IsMonospaceFont {
                get {
                    ushort                      iMapIndex;
                    ushort                      wMapIndex;

                    double                      charWidth;
                    double                      iWidth;
                    double                      wWidth;

                    FormattedText               ft;
                    GlyphTypeface               gtf;

                    IDictionary<ushort,double>  advWidths;
                    IDictionary<int,ushort>     chrMap;


                    if( tf.TryGetGlyphTypeface( out gtf ) ) {
                        advWidths               = gtf.AdvanceWidths;
                        chrMap                  = gtf.CharacterToGlyphMap;

                        if( chrMap.TryGetValue( 'i', out iMapIndex ) && chrMap.TryGetValue( 'W', out wMapIndex ) ) {
                            iWidth              = advWidths[iMapIndex];
                            wWidth              = advWidths[wMapIndex];
                        }
                        else {
                            charWidth           = -1d;
                            iWidth              =  0d;
                            wWidth              =  0d;

                            foreach( KeyValuePair<ushort,double> kvp in gtf.AdvanceWidths ) {
                                if( charWidth == -1d ) {
                                    charWidth   = kvp.Value;
                                }
                                else if( Math.Abs( charWidth - kvp.Value ) > Typeface.WidthEqualityTolerance ) {
                                    return false;
                                }
                            }
                        }
                    }
                    else {
                        ft                      = new FormattedText( "i", CultureInfo.CurrentCulture, CultureInfo.CurrentCulture.FlowDirection,
                                                                     tf, 10d, Brushes.Black, 1d );
                        iWidth                  = ft.Width;

                        ft                      = new FormattedText( "W", CultureInfo.CurrentCulture, CultureInfo.CurrentCulture.FlowDirection,
                                                                     tf, 10d, Brushes.Black, 1d );
                        wWidth                  = ft.Width;
                    }

                    return Math.Abs( iWidth - wWidth ) < Typeface.WidthEqualityTolerance;
                }
            }
            public bool                 IsSymbolFont {
                get {
                    bool            result  = false;

                    GlyphTypeface   gtf;

                    if( tf.TryGetGlyphTypeface( out gtf ) ) {
                        result = gtf.Symbol;
                    }

                    return result;
                }
            }

            public static double        WidthEqualityTolerance {
                get => 0.001d;
            }
        }
        extension( CultureInfo ci ) {
            public FlowDirection        FlowDirection {
                get => ci.TextInfo.IsRightToLeft ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;
            }
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
        
        public static TypefaceMetrics   GetMetrics( this Typeface tf ) {

            TypefaceMetrics       result;

            
            result  =   tf.IsMonospaceFont ? TypefaceMetrics.Monospace : TypefaceMetrics.Proportional;

            if( tf.IsSymbolFont )   result |=   TypefaceMetrics.Symbol;
            if( tf.IsEmojiFont )    result |=   TypefaceMetrics.Emoji;

            return result;
        }

        public static string            ToCharacter( this SegoeMDL2Assets assetName ) {

            return Char.ConvertFromUtf32( (int)assetName );
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
        public static Typeface?         GetTypeface( this IReadOnlyFontProperties uf ) {

            return new Typeface( uf.FontFamily, uf.FontStyle, uf.FontWeight, uf.FontStretch );
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

    [Flags()]
    public enum TypefaceMetrics : ushort {
        None            = 0b_0000_0000_0000_0000,

        Monospace       = 0b_0000_0000_0000_0001,
        Proportional    = 0b_0000_0000_0000_0010,

        Symbol          = 0b_0000_0000_0000_0100,
        Emoji           = 0b_0000_0000_0000_1000,

        NotGlyphBacked  = 0b_1000_0000_0000_0000
    }
}