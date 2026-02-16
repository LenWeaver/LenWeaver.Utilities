using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace LenWeaver.Utilities {

    public static class GridExtensions {

        static GridExtensions() {}


        public static ColumnDefinition  Add( this ColumnDefinitionCollection cols, double value, GridUnitType gut ) {
            
            ColumnDefinition    result  = new();


            result.Width        = new GridLength( value, gut );

            cols.Add( result );

            return result;
        }
        public static ColumnDefinition  Add( this ColumnDefinitionCollection cols, double pixels ) {

            return cols.Add( pixels, GridUnitType.Pixel );
        }
        public static ColumnDefinition  AddAuto( this ColumnDefinitionCollection cols ) {

            return cols.Add( 1d, GridUnitType.Auto );
        }
        public static ColumnDefinition  AddStar( this ColumnDefinitionCollection cols, int numberOfStars ) {

            return cols.Add( (double)numberOfStars, GridUnitType.Star );
        }
        public static ColumnDefinition  AddStar( this ColumnDefinitionCollection cols ) {

            return cols.Add( 1d, GridUnitType.Star );
        }

        public static RowDefinition     Add( this RowDefinitionCollection rows, double value, GridUnitType gut ) {

            RowDefinition       result  = new();


            result.Height       = new GridLength( value, gut );

            rows.Add( result );

            return result;
        }
        public static RowDefinition     Add( this RowDefinitionCollection rows, double pixels ) {

            return rows.Add( pixels, GridUnitType.Pixel );
        }
        public static RowDefinition     AddAuto( this RowDefinitionCollection rows ) {

            return rows.Add( 1d, GridUnitType.Auto );
        }
        public static RowDefinition     AddStar( this RowDefinitionCollection rows ) {

            return rows.Add( 1d, GridUnitType.Star );
        }
        public static RowDefinition     AddStar( this RowDefinitionCollection rows, int numberOfStars ) {

            return rows.Add( (double)numberOfStars, GridUnitType.Star );
        }

        public static UIElement         AddChild( this Grid grd, string textBlockText, int row = 0, int column = 0, int rowSpan = 1, int colSpan = 1,
                                                  Brush? foreground = null, HorizontalAlignment horizontalAlignment = HorizontalAlignment.Left ) {

            TextBlock   tb          = new();


            tb.Text                 = textBlockText;
            tb.Foreground           = foreground ?? Brushes.Black;
            tb.HorizontalAlignment  = horizontalAlignment;
            tb.VerticalAlignment    = VerticalAlignment.Center;

            return AddChild( grd, tb, row, column, rowSpan, colSpan );
        }
        public static UIElement         AddChild( this Grid grd, UIElement control, int row = 0, int column = 0, int rowSpan = 1, int colSpan = 1 ) {

            grd.Children.Add( control );

            Grid.SetRow( control, row );
            Grid.SetColumn( control, column );
            Grid.SetRowSpan( control, rowSpan );
            Grid.SetColumnSpan( control, colSpan );

            return control;
        }
    }
}