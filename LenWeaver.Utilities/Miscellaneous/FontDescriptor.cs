using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;


namespace LenWeaver.Utilities {

    public sealed class FontDescriptor : IComparable<FontDescriptor> {

        public  double              Size            { get; init; }

        public  FontStretch         Stretch         { get; init; }
        public  FontStyle           Style           { get; init; }
        public  FontWeight          Weight          { get; init; }

        public  FontFamily          Family          { get; init; }


        public FontDescriptor( string s ) {

            // Expected format:
            // "{Family.Source} {Size}pt - {Stretch}, {Style}, {Weight}"

            // Split at " - "
            var parts = s.Split( " - ", StringSplitOptions.TrimEntries );
            if( parts.Length != 2 )
                throw new FormatException( "Invalid font descriptor format." );

            // --- LEFT SIDE: "FamilyName 12pt" ---
            var left = parts[0];

            // Size is always the last token before "pt"
            var leftTokens = left.Split( ' ', StringSplitOptions.RemoveEmptyEntries );
            if( leftTokens.Length < 2 ) throw new FormatException( "Invalid family/size format." );

            // Parse size
            var sizeToken = leftTokens[^1];
            if( !sizeToken.EndsWith( "pt" ) ) throw new FormatException( "Font size must end with 'pt'." );

            var sizeString = sizeToken[..^2]; // remove "pt"
            if( !double.TryParse( sizeString, out var size ) ) throw new FormatException( "Invalid font size." );

            Size = size;

            // Family is everything before the size token
            var familyName = string.Join( ' ', leftTokens[..^1] );
            Family = new FontFamily( familyName );

            // --- RIGHT SIDE: "{Stretch}, {Style}, {Weight}" ---
            var rightTokens = parts[1].Split( ',', StringSplitOptions.TrimEntries );
            if( rightTokens.Length != 3 ) throw new FormatException( "Invalid stretch/style/weight format." );

            // Parse Stretch
            if( !Enum.TryParse<FontStretch>( rightTokens[0], out var stretch ) ) throw new FormatException( "Invalid FontStretch value." );
            Stretch = stretch;

            // Parse Style
            if( !Enum.TryParse<FontStyle>( rightTokens[1], out var style ) ) throw new FormatException( "Invalid FontStyle value." );
            Style = style;

            // Parse Weight
            if( !Enum.TryParse<FontWeight>( rightTokens[2], out var weight ) ) throw new FormatException( "Invalid FontWeight value." );
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


        public Typeface Typeface => new Typeface( Family, Style, Weight, Stretch );

        public override string ToString() => $"{Family.Source} {Size}pt - {Stretch}, {Style}, {Weight}";


        public int CompareTo( FontDescriptor? other ) {

            int result;


            if( other is null ) ArgumentNullException.ThrowIfNull( other, nameof( other) );

            result = Family.Source.CompareTo( other.Family.Source );
            if( result == 0 ) result = Size.CompareTo( other.Size );
            if( result == 0 ) result = Typeface.CompareTo( other.Typeface );

            return result;
        }
    }
}