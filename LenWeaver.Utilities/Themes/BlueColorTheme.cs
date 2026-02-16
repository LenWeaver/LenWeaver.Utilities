using System;
using System.Windows.Media;

namespace LenWeaver.Utilities {

    public class BlueColorTheme : IColorTheme {

        public string   Name                            => "Blue Color Theme";

        public Brush    LightPanel                      => new SolidColorBrush( Color.FromRgb( 0x69, 0xAF, 0xD1 ) );
        public Brush    DarkPanel                       => new SolidColorBrush( Color.FromRgb( 0x22, 0x67, 0x8C ) );
        public Brush    ButtonBackground                => new SolidColorBrush( Color.FromRgb( 0x32, 0x32, 0x32 ) );
        public Brush    ButtonForeground                => new SolidColorBrush( Color.FromRgb( 0xFF, 0xFF, 0xFF ) );
        public Brush    ControlBackground               => new SolidColorBrush( Color.FromRgb( 0xD3, 0xD3, 0xD3 ) );
        public Brush    ControlForeground               => new SolidColorBrush( Color.FromRgb( 0xFF, 0xFF, 0xFF ) );
        public Brush    HighlightBackground             => new SolidColorBrush( Color.FromRgb( 0xA9, 0xA9, 0xA9 ) );
        public Brush    HighlightForeground             => new SolidColorBrush( Color.FromRgb( 0xFF, 0xFF, 0xFF ) );
        public Brush    LabelForeground                 => new SolidColorBrush( Color.FromRgb( 0x00, 0x00, 0x00 ) );


        public int      CompareTo( IColorTheme? other ) {

            Color       otherColor;
            Color       thisColor;


            if( other is null ) throw new ArgumentNullException();

            if( LightPanel is SolidColorBrush scb ) {
                thisColor = scb.ToGreyScale();
            }
            else if( LightPanel is GradientBrush gb ) {
                thisColor = gb.ToGreyScale();
            }
            else {
                throw new ArgumentException( "Comparison target must be of type SolidColorBrush or one of the GradientBrush types." );
            }

            if( other.LightPanel is SolidColorBrush otherScb ) {
                otherColor = otherScb.ToGreyScale();
            }
            else if( LightPanel is GradientBrush otherGb ) {
                otherColor = otherGb.ToGreyScale();
            }
            else {
                throw new ArgumentException( "Comparison target must be of type SolidColorBrush or one of the GradientBrush types." );
            }

            return thisColor.R.CompareTo( otherColor.R );
        }

        public override string ToString() => Name;
    }
}