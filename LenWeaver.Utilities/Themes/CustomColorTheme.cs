using System;
using System.Windows.Media;

namespace LenWeaver.Utilities {

    public class CustomColorTheme : IColorTheme {

        public string   Name                            { get; set; }   = "Custom Color Theme";

        public Brush    LightPanel                      { get; set; }   = Brushes.White;
        public Brush    DarkPanel                       { get; set; }   = Brushes.LightGray;
        public Brush    ButtonBackground                { get; set; }   = Brushes.White;
        public Brush    ButtonForeground                { get; set; }   = Brushes.Black;
        public Brush    ControlBackground               { get; set; }   = Brushes.White;
        public Brush    ControlForeground               { get; set; }   = Brushes.Black;
        public Brush    HighlightBackground             { get; set; }   = Brushes.White;
        public Brush    HighlightForeground             { get; set; }   = Brushes.Black;
        public Brush    LabelForeground                 { get; set; }   = Brushes.Black;


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