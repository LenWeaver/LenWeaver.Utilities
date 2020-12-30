using System;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace LenWeaver.Utilities {

    [Obsolete( "Use Typeface and a double for font size instead." )]
    public class FontDescriptor {

        private static              FontFamilyConverter?    fontFamilyConverter     = null;
        private static              FontStretchConverter?   fontStretchConverter    = null;
        private static              FontStyleConverter?     fontStyleConverter      = null;
        private static              FontWeightConverter?    fontWeightConverter     = null;

        public  static  readonly    FontStyle               DefaultFontStyle        = FontStyles.Normal;
        public  static  readonly    FontWeight              DefaultFontWeight       = FontWeights.Normal;
        public  static  readonly    FontStretch             DefaultFontStretch      = FontStretches.Normal;


        public  double              Size        { get; set; }

        public  FontFamily          Family      { get; set; }
        public  FontStretch         Stretch     { get; set; }
        public  FontStyle           Style       { get; set; }
        public  FontWeight          Weight      { get; set; }


        public FontDescriptor( string asString ) {

            string[]                tokens;


            try {
                tokens          = asString.Split( ';' );

                if( tokens.Length == 5 && FontFamilyConverter.CanConvertFrom( typeof(string) ) &&
                    FontStyleConverter.CanConvertFrom( typeof(string) ) && FontWeightConverter.CanConvertFrom( typeof(string) ) &&
                    FontStretchConverter.CanConvertFrom( typeof(string) ) && tokens[0] != null && tokens[1] != null &&
                    tokens[2] != null && tokens[3] != null && tokens[4] != null ) {

                    Family      = (FontFamily)FontFamilyConverter.ConvertFromInvariantString( tokens[0].ToString() );
                    Size        = Double.Parse( tokens[1].ToString() );
                    Style       = (FontStyle)FontStyleConverter.ConvertFromInvariantString( tokens[2].ToString() );
                    Weight      = (FontWeight)FontWeightConverter.ConvertFromInvariantString( tokens[3].ToString() );
                    Stretch     = (FontStretch)FontStretchConverter.ConvertFromInvariantString( tokens[4].ToString() );
                }
                else {
                    throw new ArgumentException( "Argument asString does not contain the necessary elements of a FontDescriptor." );
                }
            }
            catch( Exception ex ) {
                throw new ArgumentException( "Unable to convert string to FontDescriptor.", ex );
            }
        }
        public FontDescriptor( FontFamily family, double size, FontStyle style, FontWeight weight, FontStretch stretch ) {

            Family      = family;
            Size        = size;
            Style       = style;
            Weight      = weight;
            Stretch     = stretch;
        }
        public FontDescriptor( FontFamily family, double size, FontStyle style, FontWeight weight ) : this( family, size, style, weight, DefaultFontStretch ) {}
        public FontDescriptor( FontFamily family, double size, FontStyle style ) : this( family, size, style, DefaultFontWeight, DefaultFontStretch ) {}
        public FontDescriptor( FontFamily family, double size ) : this( family, size, DefaultFontStyle, DefaultFontWeight, DefaultFontStretch ) {}


        #region Static Converter Properties
        public static FontFamilyConverter FontFamilyConverter {
            get {
                if( fontFamilyConverter == null ) {
                    fontFamilyConverter = new FontFamilyConverter();
                }

                return fontFamilyConverter;
            }
        }
        public static FontStretchConverter FontStretchConverter {
            get {
                if( fontStretchConverter == null ) {
                    fontStretchConverter = new FontStretchConverter();
                }

                return fontStretchConverter;
            }
        }
        public static FontStyleConverter FontStyleConverter {
            get {
                if( fontStyleConverter == null ) {
                    fontStyleConverter = new FontStyleConverter();
                }

                return fontStyleConverter;
            }
        }
        public static FontWeightConverter FontWeightConverter {
            get {
                if( fontWeightConverter == null ) {
                    fontWeightConverter = new FontWeightConverter();
                }

                return fontWeightConverter;
            }
        }
        #endregion


        public override string ToString() {

            StringBuilder           sb;


            sb = new StringBuilder();
            sb.Append( FontFamilyConverter.ConvertToInvariantString( Family ) );
            sb.Append( ";" );
            sb.Append( Size.ToString() );
            sb.Append( ";" );
            sb.Append( FontStyleConverter.ConvertToInvariantString( Style ) );
            sb.Append( ";" );
            sb.Append( FontWeightConverter.ConvertToInvariantString( Weight ) );
            sb.Append( ";" );
            sb.Append( FontStretchConverter.ConvertToInvariantString( Stretch ) );
            sb.Append( ";" );

            return sb.ToString();
        }
    }
}