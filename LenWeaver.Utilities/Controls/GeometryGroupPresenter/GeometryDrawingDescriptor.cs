using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace LenWeaver.Utilities {

    public sealed class GeometryDrawingDescriptor {

        public  FillRule    FillRule            { get; internal set; }  = FillRule.Nonzero;

        public  double      PenThickness        { get; internal set; }  = 1d;

        public  Brush       Brush               { get; internal set; }  = Brushes.Black;
        public  Brush       PenBrush            { get; internal set; }  = Brushes.Black;
        public  Geometry    Geometry            { get; internal set; }  = Geometry.Empty;


        internal GeometryDrawingDescriptor() {}


        public void FreezeAll() {

            Brush.Freeze();
            PenBrush.Freeze();
            Geometry.Freeze();
        }


        public static IEnumerable<GeometryDrawingDescriptor> Parse( string extendedPathMarkup ) {
            //PB=#F12398|FR=NonZero|B=#12A456|PT=1|D=M0,0 10,0 10,25 Z@PB=#F12398|FR=NonZero|B=#12A456|PT=1|D=M0,0 10,0 10,25 Z

            string                              name;
            string                              value;

            string[]                            nameValue;

            Color?                              c;

            GeometryDrawingDescriptor           gdd;

            List<GeometryDrawingDescriptor>     result  = new();


            foreach( string s in extendedPathMarkup.Split( '@', StringSplitOptions.TrimEntries ) ) {
                gdd                 = new GeometryDrawingDescriptor();

                foreach( string token in s.Split( '|', StringSplitOptions.TrimEntries ) ) {
                    nameValue       = token.Split( '=', StringSplitOptions.TrimEntries );

                    if( nameValue.Length == 1 && gdd.Geometry.IsEmpty() ) {
                        name        = "d";
                        value       = nameValue[0];
                    }
                    else if( nameValue.Length == 2 ) {
                        name        = nameValue[0];
                        value       = nameValue[1];
                    }
                    else {
                        throw new FormatException( $"Formatting error: Name Value = '{token}'." );
                    }


                    if( name.Length < 1 || value.Length < 1 ) throw new FormatException( $"Either the name or value is too short: name = {name}, value = {value}." );

                    switch( name.ToLower() ) {
                        case "b":
                        case "brush":
                            c = Color.FromString( value );
                            if( c is not null ) gdd.Brush = new SolidColorBrush( c.Value );
                            break;

                        case "d":
                        case "data":
                            gdd.Geometry = Geometry.Parse( value );
                            break;

                        case "fr":
                        case "fillrule":
                            gdd.FillRule = value.Equals( "nonzero", StringComparison.InvariantCultureIgnoreCase ) ? FillRule.Nonzero : FillRule.EvenOdd;
                            break;

                        case "pb":
                        case "penbrush":
                            c = Color.FromString( value );
                            if( c is not null ) gdd.PenBrush = new SolidColorBrush( c.Value );
                            break;

                        case "pt":
                        case "penthickness":
                            gdd.PenThickness = Convert.ToDouble( value );
                            break;

                        default:
                            throw new FormatException( "Unknown extension token." );
                    }
                }

                if( gdd.Geometry.IsEmpty() ) throw new FormatException( "Data section of Extended Path Markup is empty." );

                result.Add( gdd );
            }


            return result.ToArray();
        }
    }
}