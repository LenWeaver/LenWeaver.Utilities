using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Media;


namespace LenWeaver.Utilities {

    public static class ColorExtensions {

        private static Dictionary<string,uint>              knownColors     = new();


        static ColorExtensions() {}


        public static IReadOnlyDictionary<string,uint>      KnownColors {
            get {

                Color?      c;


                if( knownColors.Count == 0 ) {
                    foreach( PropertyInfo pi in typeof(Colors).GetProperties( BindingFlags.Public | BindingFlags.Static ) ) {
                        c       = (Color?)pi.GetValue( null );

                        if( c is not null ) {
                            knownColors.Add( pi.Name, c.Value.ToUInt32() );
                        }
                    }
                }

                return knownColors;
            }
        }


        extension( Color c ) {

            public bool                     IsKnownColor {
                get => KnownColors.Values.Contains( c.ToUInt32() );
            }

            public string?                  Name {
                get {
                    uint        colorUInt   = c.ToUInt32();

                    string?     result      = null;

                    
                    foreach( KeyValuePair<string,uint> kv in KnownColors ) {
                        if( kv.Value == colorUInt ) {
                            result = kv.Key;
                            break;
                        }
                    }

                    return result;
                }
            }


            public static Color?            FromString( string hexOrName ) {

                uint            argb;

                Color?          result      = null;


                hexOrName                   = hexOrName.Trim();

                if( hexOrName.Length > 0 ) {
                    if( hexOrName.StartsWith( '#' ) ) {
                        if( hexOrName.Length == 7 ) {
                            argb            = UInt32.Parse( "FF" + hexOrName.Substring( 1 ), System.Globalization.NumberStyles.AllowHexSpecifier );

                            result          = Color.FromArgb( argb );
                        }
                        else if( hexOrName.Length == 9 ) {
                            argb            = UInt32.Parse( hexOrName.Substring( 1 ), System.Globalization.NumberStyles.AllowHexSpecifier );

                            result          = Color.FromArgb( argb );
                        }
                    }
                    else {
                        if( KnownColors.TryGetValue( hexOrName, out argb ) ) {
                            result          = Color.FromArgb( argb );
                        }
                    }
                }

                return result;
            }

            public static Color             FromArgb( uint argb ) {

                Color   result;


                byte[] colorParts = BitConverter.GetBytes( argb );

                result = Color.FromArgb( colorParts[3], colorParts[2], colorParts[1], colorParts[0] );

                return result;
            }
            public static Color             FromArgb( int argb ) {

                return Color.FromArgb( (uint)argb );
            }

            public static Color             FromRgb( uint rgb ) {

                Color   result;


                byte[] colorParts = BitConverter.GetBytes( rgb );

                result = Color.FromArgb( Byte.MaxValue, colorParts[2], colorParts[1], colorParts[0] );

                return result;
            }
            public static Color             FromRgb( int rgb ) {

                return Color.FromRgb( (uint)rgb );
            }

            public (byte alpha, byte red, byte green, byte blue) ToArgb() {
                
                return (c.A, c.R, c.G, c.B);
            }

            public (byte red, byte green, byte blue) ToRgb() {
                
                return (c.R, c.G, c.B);
            }
        }


        public static uint                  ToUInt32( this Color c ) {
            
            return (uint)((c.A << 24) | (c.R << 16) | (c.G << 8) | c.B);
        }
        public static int                   ToInt32( this Color c ) {

            return (int)c.ToUInt32();
        }

        public static Color                 ToGreyScale( this Color c, byte alphaChannel ) {

            byte        averageRGB;


            averageRGB = (byte)(((int)c.R + (int)c.G + (int)c.B) / 3);

            return Color.FromArgb( alphaChannel, averageRGB, averageRGB, averageRGB );
        }
        public static Color                 ToGreyScale( this Color c ) {

            return c.ToGreyScale( Byte.MaxValue );
        }

        public static Color                 ToGreyScale( this SolidColorBrush scb ) {

            return scb.Color.ToGreyScale();
        }
        public static Color                 ToGreyScale( this GradientBrush gb ) {

            if( gb.GradientStops.Count == 0 ) throw new ArgumentException( "Specified GradientBrush has no gradient stops." );
                
            return gb.GradientStops[0].Color.ToGreyScale();
        }
    }
}