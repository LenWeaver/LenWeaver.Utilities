using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;


namespace LenWeaver.Utilities {

    [ValueConversion( typeof(Geometry), typeof(string) )]
    public class PathMarkupConverter : IValueConverter {

        //<Path x:Name="pPath" Fill="{TemplateBinding PathFill}" Stroke="{TemplateBinding PathStroke}"
        //                              StrokeThickness="{TemplateBinding PathStrokeThickness}" ToolTip="pPath"
        //                              Data="{Binding RelativeSource={RelativeSource TemplatedParent},
        //                                                       Path=PathMarkup,Converter={StaticResource PathConverter}}"/>

        // Fill                 Brush
        // Stroke               Brush
        // StrokeThickness      decimal
        // Data                 M0,0 0,100 100,100 100,0 Z

        // {Fill=Black,Stroke=Black,StrokeThickness=1}M0,0 0,100 100,100 100,0 Z

        // M0,0 0,100 100,100 100,0 Z
        public object Convert( object value, Type targetType, object parameter, CultureInfo culture ) {
            
            var a = PathGeometry.Parse( value?.ToString() ?? String.Empty );
            
            return a;
        }

        public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture ) {
            
            var a = PathGeometry.CreateFromGeometry( (Geometry)value ).ToString();

            return a;
        }
    }
}