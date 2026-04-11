using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Media;
using SWM = System.Windows.Media;

namespace LenWeaver.Utilities {

    public sealed class GeometryDrawingDescriptor {

        private const int RoundingDecimalPlaces = 2;


        public  FillRule?   FillRule            { get; internal set; }  = null;

        public  double?     PenThickness        { get; internal set; }  = null;

        public  string?     GeometryType        { get; internal set; }  = null;

        public  Brush?      Brush               { get; internal set; }  = null;
        public  Brush?      PenBrush            { get; internal set; }  = null;
        public  Geometry?   Geometry            { get; internal set; }  = null;


        public GeometryDrawingDescriptor() {}


        public override string ToString() {

            List<string>        tokens          = new();


            if( Brush is not null )             tokens.Add( $"B={(Brush as SolidColorBrush)!.Color.ToString()}" );
            if( FillRule is not null )          tokens.Add( $"FR={(FillRule.Value == SWM.FillRule.Nonzero ? nameof(SWM.FillRule.Nonzero) : nameof(SWM.FillRule.EvenOdd))}" );
            if( PenThickness is not null )      tokens.Add( $"PT={PenThickness}" );
            if( PenBrush is not null )          tokens.Add( $"PB={(PenBrush as SolidColorBrush)!.Color.ToString()}" );
            if( Geometry is not null )          tokens.Add( $"D={Round( Geometry ).ToPathMarkup()}" );

            return String.Join( '|', tokens );
        }


        private static void             RoundPathGeometry                   ( PathGeometry pg,      int decimals ) {

            foreach( PathFigure figure in pg.Figures ) {
                figure.StartPoint = RoundPoint( figure.StartPoint, decimals );

                foreach( PathSegment segment in figure.Segments ) {
                    switch( segment ) {
                        case LineSegment ls:
                            ls.Point = RoundPoint( ls.Point, decimals );
                            break;

                        case PolyLineSegment pls:
                            pls.Points = RoundPointCollection( pls.Points, decimals );
                            break;

                        case BezierSegment bs:
                            bs.Point1 = RoundPoint( bs.Point1, decimals );
                            bs.Point2 = RoundPoint( bs.Point2, decimals );
                            bs.Point3 = RoundPoint( bs.Point3, decimals );
                            break;

                        case PolyBezierSegment pbs:
                            pbs.Points = RoundPointCollection( pbs.Points, decimals );
                            break;

                        case QuadraticBezierSegment qbs:
                            qbs.Point1 = RoundPoint( qbs.Point1, decimals );
                            qbs.Point2 = RoundPoint( qbs.Point2, decimals );
                            break;

                        case PolyQuadraticBezierSegment pqbs:
                            pqbs.Points = RoundPointCollection( pqbs.Points, decimals );
                            break;

                        case ArcSegment arc:
                            arc.Point = RoundPoint( arc.Point, decimals );
                            arc.Size = new Size(
                                Math.Round( arc.Size.Width, decimals ),
                                Math.Round( arc.Size.Height, decimals )
                            );
                            break;
                    }
                }
            }
        }
        private static void             RoundTransform                      ( Transform t,          int decimals ) {

            switch( t ) {
                case TranslateTransform tt:
                    tt.X = Math.Round( tt.X, decimals );
                    tt.Y = Math.Round( tt.Y, decimals );
                    break;

                case ScaleTransform st:
                    st.ScaleX = Math.Round( st.ScaleX, decimals );
                    st.ScaleY = Math.Round( st.ScaleY, decimals );
                    st.CenterX = Math.Round( st.CenterX, decimals );
                    st.CenterY = Math.Round( st.CenterY, decimals );
                    break;

                case RotateTransform rt:
                    rt.Angle = Math.Round( rt.Angle, decimals );
                    rt.CenterX = Math.Round( rt.CenterX, decimals );
                    rt.CenterY = Math.Round( rt.CenterY, decimals );
                    break;

                case SkewTransform sk:
                    sk.AngleX = Math.Round( sk.AngleX, decimals );
                    sk.AngleY = Math.Round( sk.AngleY, decimals );
                    sk.CenterX = Math.Round( sk.CenterX, decimals );
                    sk.CenterY = Math.Round( sk.CenterY, decimals );
                    break;

                //case MatrixTransform mt:
                //    var m = mt.Matrix;
                //    m.M11 = Math.Round(m.M11, decimals);
                //    m.M12 = Math.Round(m.M12, decimals);
                //    m.M21 = Math.Round(m.M21, decimals);
                //    m.M22 = Math.Round(m.M22, decimals);
                //    m.OffsetX = Math.Round(m.OffsetX, decimals);
                //    m.OffsetY = Math.Round(m.OffsetY, decimals);
                //    mt.Matrix = m;
                //    break;
            }
        }
        private static Point            RoundPoint                          ( Point p,              int decimals ) {
        
                return new Point( Math.Round( p.X, decimals ), Math.Round( p.Y, decimals ) );
        }
        private static Rect             RoundRect                           ( Rect r,               int decimals ) {

            return new Rect( Math.Round( r.X, decimals ),       Math.Round( r.Y, decimals ),
                             Math.Round( r.Width, decimals ),   Math.Round( r.Height, decimals ) );
        }
        private static PointCollection  RoundPointCollection                ( PointCollection pc,   int decimals ) {

            PointCollection result = new PointCollection( pc.Count );


            foreach( Point p in pc ) {
                result.Add( RoundPoint( p, decimals ) );
            }

            return result;
        }
        private static Geometry         Round                               ( Geometry geometry,    int decimals = RoundingDecimalPlaces ) {

            if( geometry == null ) return null;

            geometry = geometry.CloneCurrentValue();

            switch( geometry ) {
                case PathGeometry pg:
                    RoundPathGeometry( pg, decimals );
                    break;

                case GeometryGroup gg:
                    foreach( var child in gg.Children ) {
                        Round( child, decimals );
                    }
                    break;

                case EllipseGeometry eg:
                    eg.Center       = RoundPoint( eg.Center, decimals );
                    eg.RadiusX      = Math.Round( eg.RadiusX, decimals );
                    eg.RadiusY      = Math.Round( eg.RadiusY, decimals );
                    break;

                case RectangleGeometry rg:
                    rg.Rect         = RoundRect( rg.Rect, decimals );
                    break;

                case LineGeometry lg:
                    lg.StartPoint   = RoundPoint( lg.StartPoint, decimals );
                    lg.EndPoint     = RoundPoint( lg.EndPoint, decimals );
                    break;
            }

            if( geometry.Transform is Transform t ) RoundTransform( t, decimals );

            return geometry;
        }


        public static GeometryDrawingDescriptor ParseSingle                 ( string extendedPathMarkup ) {

            string                      name;
            string                      value;

            Color?                      c;

            string[]                    nameValue;

            GeometryDrawingDescriptor   result;


            result                  = new GeometryDrawingDescriptor();

            foreach( string token in extendedPathMarkup.Split( '|', StringSplitOptions.TrimEntries ) ) {
                nameValue           = token.Split( '=', StringSplitOptions.TrimEntries );

                if( nameValue.Length == 1 && (result.Geometry is null || result!.Geometry!.IsEmpty() ) ) {
                    name            = "d";
                    value           = nameValue[0];
                }
                else if( nameValue.Length == 2 ) {
                    name            = nameValue[0];
                    value           = nameValue[1];
                }
                else {
                    throw new FormatException( $"Formatting error: Name Value = '{token}'." );
                }


                if( name.Length < 1 || value.Length < 1 ) throw new FormatException( $"Either the name or value is too short: name = {name}, value = {value}." );

                switch( name.ToLower() ) {
                    case "b":
                    case "brush":
                        c           = Color.FromString( value );
                        if( c is not null ) result.Brush = new SolidColorBrush( c.Value );
                        break;

                    case "d":
                    case "data":
                        result.Geometry = Geometry.Parse( value );
                        break;

                    case "fr":
                    case "fillrule":
                        result.FillRule = value.Equals( nameof(SWM.FillRule.Nonzero), StringComparison.InvariantCultureIgnoreCase ) ? SWM.FillRule.Nonzero : SWM.FillRule.EvenOdd;
                        break;

                    case "pb":
                    case "penbrush":
                        c = Color.FromString( value );
                        if( c is not null ) result.PenBrush = new SolidColorBrush( c.Value );
                        break;

                    case "pt":
                    case "penthickness":
                        result.PenThickness = Convert.ToDouble( value );
                        break;

                    default:
                        throw new FormatException( "Unknown extension token." );
                }
            }

            return result;
        }

        public static GeometryDrawingDescriptor[] Parse                     ( string extendedPathMarkup ) {

            GeometryDrawingDescriptor           gdd;

            List<GeometryDrawingDescriptor>     result  = new();


            foreach( string s in extendedPathMarkup.Split( '@', StringSplitOptions.TrimEntries ) ) {
                gdd = ParseSingle( s );

                if( gdd.Geometry is null || gdd.Geometry.IsEmpty() ) throw new FormatException( "Data section of Extended Path Markup is empty." );

                result.Add( gdd );
            }


            return result.ToArray();
        }
        public static GeometryDrawingDescriptor[] Parse                     ( ResourceDictionary rd ) {

            List<GeometryDrawingDescriptor>     result  = new();


            foreach( object o in rd.Values ) {
                if( o is DrawingGroup dg && dg.Children.Count > 0 ) {
                    result.AddRange( IterateResourceDictionary( dg ) );
                }
            }

            return result.ToArray();
        }

        private static GeometryDrawingDescriptor ConvertEllipseGeometry     ( EllipseGeometry   eg, Brush? brush, Brush? penBrush, double? penThickness ) {

            GeometryDrawingDescriptor   result  = new();


            result.Brush        = brush;
            result.PenBrush     = penBrush;
            result.PenThickness = penThickness;
            result.FillRule     = eg.FillRule;
            result.Geometry     = Round( eg.GetOutlinedPathGeometry() );
            result.GeometryType = "Ellipse";

            return result;
        }
        private static GeometryDrawingDescriptor ConvertLineGeometry        ( LineGeometry      lg, Brush? brush, Brush? penBrush, double? penThickness ) {

            GeometryDrawingDescriptor   result  = new();


            result.Brush        = brush;
            result.PenBrush     = penBrush;
            result.PenThickness = penThickness;
            result.FillRule     = lg.FillRule;
            result.Geometry     = Round( lg.GetFlattenedPathGeometry() );
            result.GeometryType = "Line";

            return result;
        }
        private static GeometryDrawingDescriptor ConvertPathGeometry        ( PathGeometry      pg, Brush? brush, Brush? penBrush, double? penThickness ) {

            GeometryDrawingDescriptor   result  = new();


            result.Brush        = brush;
            result.PenBrush     = penBrush;
            result.PenThickness = penThickness;
            result.FillRule     = pg.FillRule;
            result.Geometry     = Round( pg );
            result.GeometryType = "Path";

            return result;
        }
        private static GeometryDrawingDescriptor ConvertRectangleGeometry   ( RectangleGeometry rg, Brush? brush, Brush? penBrush, double? penThickness ) {

            GeometryDrawingDescriptor   result  = new();


            result.Brush        = brush;
            result.PenBrush     = penBrush;
            result.PenThickness = penThickness;
            result.FillRule     = rg.FillRule;
            result.Geometry     = Round( rg.GetFlattenedPathGeometry() );
            result.GeometryType = "Rectangle";

            return result;
        }

        private static GeometryDrawingDescriptor[] IterateResourceDictionary( DrawingGroup dg ) {

            double?                             penThickness;

            Brush?                              brush;
            Brush?                              penBrush;

            List<GeometryDrawingDescriptor>     result          = new();


            foreach( object o in dg.Children ) {
                if( o is GeometryDrawing gd ) {
                    brush           = gd.Brush;
                    penBrush        = gd.Pen?.Brush;
                    penThickness    = gd.Pen?.Thickness;

                    if( gd.Geometry is EllipseGeometry eg ) {
                        result.Add( ConvertEllipseGeometry( eg, brush, penBrush, penThickness ) );
                    }
                    else if( gd.Geometry is LineGeometry lg ) {
                        result.Add( ConvertLineGeometry( lg, brush, penBrush, penThickness ) );
                    }
                    else if( gd.Geometry is PathGeometry pg ) {
                        result.Add( ConvertPathGeometry( pg, brush, penBrush, penThickness ) );
                    }
                    else if( gd.Geometry is RectangleGeometry rg ) {
                        result.Add( ConvertRectangleGeometry( rg, brush, penBrush, penThickness ) );
                    }
                }
                else if( o is DrawingGroup group && group.Children.Count > 0 ) {
                    result.AddRange( IterateResourceDictionary( group ) );
                }
            }

            return result.ToArray();
        }
    }
}