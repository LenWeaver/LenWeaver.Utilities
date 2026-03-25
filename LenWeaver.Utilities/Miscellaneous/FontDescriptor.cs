using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;


namespace LenWeaver.Utilities {

    /// <summary>Encapsulates the five font properties: Family, Size, Stretch, Style and Weight.</summary>
    /// <remarks>The FontDescriptor class describes a set of font related data, primarily for storage.</remarks>
    public sealed class FontDescriptor : IComparable, IComparable<FontDescriptor>, IReadOnlyFontProperties {

        public static readonly string           EllipsisChar        = "…";
        public static readonly string           EllipsisString      = "...";

        private bool?                           isGlyphBacked       = null;
        private bool?                           isSymbol            = null;

        private DpiScale?                       dpiScale            = null;

        private GlyphTypeface?                  glyphTypeface       = null;
        private Typeface?                       typeface            = null;

        private IDictionary<ushort,double>?     advancedWidths      = null;
        private IDictionary<int,ushort>?        characterMap        = null;

        public  double                          Size                { get; init; }

        public  FontStretch                     Stretch             { get; init; }
        public  FontStyle                       Style               { get; init; }
        public  FontWeight                      Weight              { get; init; }

        public  FontFamily                      Family              { get; init; }


        /// <summary>Initializes a new instance of the FontDescriptor class using the specified font descriptor string.</summary>
        /// <remarks>The constructor parses the input string to extract the font family, size, stretch,
        /// style, and weight. The size must be specified in points and end with 'pt'.</remarks>
        /// <param name="s">The font descriptor string in the format '{Family.Source} {Size}pt - {Stretch}, {Style}, {Weight}'. The
        /// family and size must be specified, with the size ending in 'pt'. The stretch, style, and weight must be
        /// valid enumeration values separated by commas.</param>
        /// <exception cref="FormatException">Thrown if the format of the font descriptor string is invalid, including incorrect family/size or
        /// stretch/style/weight formats.</exception>
        public FontDescriptor( string s ) {
            // Expected format: "{Family.Source} {Size}pt - {Stretch}, {Style}, {Weight}"

            string      token;

            string[]    tokens;
            string[]    parts;


            parts = s.Split( " - ", StringSplitOptions.TrimEntries );
            if( parts.Length != 2 ) throw new FormatException( "Invalid font descriptor format." );

            tokens = parts[0].Split( ' ', StringSplitOptions.RemoveEmptyEntries );
            if( tokens.Length < 2 ) throw new FormatException( "Invalid family/size format." );

            token = tokens[^1];
            if( !token.EndsWith( "pt" ) ) throw new FormatException( "Font size must end with 'pt'." );

            token = token[..^2];
            if( !Double.TryParse( token, out var size ) ) throw new FormatException( "Invalid font size." );

            Size = size;

            // FontFamily is everything before the size token
            token = String.Join( ' ', tokens[..^1] );
            Family = new FontFamily( token );

            // Typeface format: "{Stretch}, {Style}, {Weight}"
            tokens = parts[1].Split( ',', StringSplitOptions.TrimEntries );
            if( tokens.Length != 3 ) throw new FormatException( "Invalid stretch/style/weight format." );

            if( !Enum.TryParse<FontStretch>( tokens[0], out FontStretch stretch ) ) throw new FormatException( "Invalid FontStretch value." );
            Stretch = stretch;

            if( !Enum.TryParse<FontStyle>( tokens[1], out FontStyle style ) ) throw new FormatException( "Invalid FontStyle value." );
            Style = style;

            if( !Enum.TryParse<FontWeight>( tokens[2], out FontWeight weight ) ) throw new FormatException( "Invalid FontWeight value." );
            Weight = weight;
        }
        public FontDescriptor( FontFamily ff, double fs, FontStretch stretch, FontStyle style, FontWeight weight ) {

            Family          = ff;
            Size            = fs;
            Stretch         = stretch;
            Style           = style;
            Weight          = weight;
        }
        public FontDescriptor( FontFamily ff, double fs, Typeface tf ) : this( ff, fs, tf.Stretch, tf.Style, tf.Weight ) {}
        public FontDescriptor( Control c )                  : this(  c.FontFamily,  c.FontSize,  c.FontStretch,  c.FontStyle,  c.FontWeight ) {}
        public FontDescriptor( TextBlock tb )               : this( tb.FontFamily, tb.FontSize, tb.FontStretch, tb.FontStyle, tb.FontWeight ) {}
        public FontDescriptor( TextElement te )             : this( te.FontFamily, te.FontSize, te.FontStretch, te.FontStyle, te.FontWeight ) {}
        public FontDescriptor( FlowDocument fd )            : this( fd.FontFamily, fd.FontSize, fd.FontStretch, fd.FontStyle, fd.FontWeight ) {}
        public FontDescriptor( IReadOnlyFontProperties fp ) : this( fp.FontFamily, fp.FontSize, fp.FontStretch, fp.FontStyle, fp.FontWeight ) {}


        public bool                             IsGlyphBacked {
            get {
                if( isGlyphBacked is null ) {
                    isGlyphBacked = Typeface.TryGetGlyphTypeface( out glyphTypeface );
                }

                return isGlyphBacked.Value;
            }
        }
        public bool                             IsSymbol {
            get {
                if( isSymbol is null ) {
                    isSymbol    = IsGlyphBacked && GlyphTypeface!.Symbol;
                }

                return isSymbol.Value;
            }
        }
        public DpiScale                         DpiScale {
            get {
                dpiScale ??= VisualTreeHelper.GetDpi( new DrawingVisual() );

                return dpiScale.Value;
            }
        }
        public GlyphTypeface?                   GlyphTypeface {
            get {
                if( glyphTypeface is null ) {
                    isGlyphBacked   = Typeface.TryGetGlyphTypeface( out glyphTypeface );
                }

                return glyphTypeface;
            }
        }
        public Typeface                         Typeface {
            get {
                typeface ??= new Typeface( Family, Style, Weight, Stretch );

                return typeface;
            }
        }
        public IDictionary<ushort,double>?      AdvancedWidths {
            get {
                advancedWidths ??= GlyphTypeface?.AdvanceWidths;

                return advancedWidths;
            }
        }
        public IDictionary<int,ushort>?         CharacterMap {
            get {
                characterMap ??= GlyphTypeface?.CharacterToGlyphMap;

                return characterMap;
            }
        }


        public double MeasureText               ( ReadOnlySpan<char> s ) {

            double              result;

            FormattedText       ft;


            if( IsGlyphBacked ) {
                result  = 0;

                foreach( char c in s ) {
                    result += AdvancedWidths![CharacterMap![c]];
                }

                result *= Size;
            }
            else {
                ft      = new FormattedText( s.ToString(), CultureInfo.CurrentCulture, CultureInfo.CurrentCulture.FlowDirection,
                                             Typeface, Size, Brushes.Black, DpiScale.PixelsPerDip );

                result  = ft.WidthIncludingTrailingWhitespace;
            }

            return result;
        }
        public double MeasureText               ( string s ) {

            return MeasureText( s.AsSpan() );
        }

        public string TruncateWithEllipsis      ( ReadOnlySpan<char> s, double maxPixels, string ellipsisChar, EllipsisLocation location ) {

            double                  ellipsisWidth;

            string                  result;


            maxPixels           -= 4;
            
            if( s.Length == 0 || maxPixels <= 0 ) {
                result          = String.Empty;
            }
            else {
                ellipsisWidth   = MeasureText( ellipsisChar );

                if( MeasureText( s ) + ellipsisWidth <= maxPixels ) {
                    result      = s.ToString();
                }
                else {
                    result      = location switch {
                        EllipsisLocation.Beginning  => TruncateBeginning( s, maxPixels - 4, ellipsisChar, ellipsisWidth ),
                        EllipsisLocation.Middle     => TruncateMiddle( s, maxPixels - 4, ellipsisChar, ellipsisWidth ),
                        EllipsisLocation.End        => TruncateEnd( s, maxPixels - 4, ellipsisChar, ellipsisWidth ),
                        _                           => throw new System.ComponentModel.InvalidEnumArgumentException( nameof(location), (int)location, typeof(EllipsisLocation) )
                    };
                }
            }

            return result;
        }
        public string TruncateWithEllipsis      ( string s, double maxPixels, string ellipsisChar, EllipsisLocation location ) {

            return TruncateWithEllipsis( s.AsSpan(), maxPixels, ellipsisChar, location );
        }

        private string TruncateEnd              ( ReadOnlySpan<char> span, double maxPixels, string ellipsis, double ellipsisWidth ) {

            int                     left        = 0;
            int                     mid;
            int                     right       = span.Length;

            double                  width;

            ReadOnlySpan<char>      finalSpan;


            while( left < right ) {
                mid         = (left + right) / 2;
                width       = MeasureText( span[..mid].ToString() );

                if( width + ellipsisWidth > maxPixels ) {
                    right   = mid;
                }
                else { 
                    left    = mid + 1;
                }
            }

            finalSpan       = span[..Math.Max( 0, right - 1 )];

            return finalSpan.ToString() + ellipsis;
        }
        private string TruncateBeginning        ( ReadOnlySpan<char> span, double maxPixels, string ellipsis, double ellipsisWidth ) {

            int left        = 0;
            int mid;
            int right       = span.Length;

            double  width;

            ReadOnlySpan<char>  finalSpan;


            while( left < right ) {
                mid         = (left + right) / 2;
                width       = MeasureText(span[mid..].ToString());

                if( width + ellipsisWidth > maxPixels ) {
                    left    = mid + 1;
                }
                else {
                    right   = mid;
                }
            }

            finalSpan       = span[Math.Min(span.Length, left)..];

            return ellipsis + finalSpan.ToString();
        }
        private string TruncateMiddle           ( ReadOnlySpan<char> span, double maxPixels, string ellipsis, double ellipsisWidth ) {

            int keep;

            int finalKeep;
            int finalLeft;
            int finalRight;
            int left;
            int leftCount;
            int right;
            int rightCount;
            int total                           = span.Length;

            double  width;

            ReadOnlySpan<char>  leftSpan;
            ReadOnlySpan<char>  rightSpan;

            left            = 1;
            right           = total - 1;

            while( left < right ) {
                keep = (left + right + 1) / 2; // bias upward

                leftCount   = keep / 2;
                rightCount  = keep - leftCount;

                leftSpan    = span[..leftCount];
                rightSpan   = span[^rightCount..];

                width       = MeasureText(leftSpan.ToString() + rightSpan.ToString());

                if( width + ellipsisWidth > maxPixels ) {
                    right   = keep - 1;
                }
                else {
                    left    = keep;
                }
            }

            finalKeep       = Math.Max( 1, left );
            finalLeft       = finalKeep / 2;
            finalRight      = finalKeep - finalLeft;

            return span[..finalLeft].ToString()
                 + ellipsis
                 + span[^finalRight..].ToString();
        }


        public override string ToString() => $"{Family.Source} {Size.ToString( CultureInfo.InvariantCulture )}pt - {Stretch}, {Style}, {Weight}";


        #region IComparable and IComparable<FontDescriptor?> Implementation
        public int CompareTo( object? obj ) => CompareTo( obj as FontDescriptor );
        public int CompareTo( FontDescriptor? other ) {

            int result;


            result                      = other is null ? 1 : 0;
            if( result == 0 ) result    = Family.Source.CompareTo( other!.Family.Source, StringComparison.Ordinal );
            if( result == 0 ) result    = Size.CompareTo( other!.Size );
            if( result == 0 ) result    = Stretch.CompareTo( other!.Stretch );
            if( result == 0 ) result    = Style.CompareTo( other!.Style );
            if( result == 0 ) result    = Weight.CompareTo( other!.Weight );

            return result;
        }
        #endregion
        #region IReadOnlyFontProperties Implementation
        FontFamily  IReadOnlyFontProperties.FontFamily  { get => Family;  }
        FontStretch IReadOnlyFontProperties.FontStretch { get => Stretch; }
        FontStyle   IReadOnlyFontProperties.FontStyle   { get => Style;   }
        FontWeight  IReadOnlyFontProperties.FontWeight  { get => Weight;  }
        double      IReadOnlyFontProperties.FontSize    { get => Size;    }
        #endregion
    }
}