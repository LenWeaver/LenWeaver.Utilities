using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using win = System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;

namespace LenWeaver.Utilities {

    public static class WPFExtensions {

        static WPFExtensions() {}


        #region ButtonBase Extensions
        extension( ButtonBase btn ) {
            public CornerRadius CornerRadius {
                get {
                    Border?      brd;


                    brd                 = btn?.Template.FindName( "border", btn ) as Border;
                    if( brd == null ) throw new ArgumentException( $"Specified button control does not seem to have a border." );

                    return brd.CornerRadius;
                }
                set {
                    Border?      brd;


                    brd                 = btn?.Template.FindName( "border", btn ) as Border;
                    if( brd == null ) throw new ArgumentException( $"Specified button control does not seem to have a border." );

                    brd.CornerRadius    = value;
                }
            }
        }

        public static void PerformClick( this ButtonBase btn ) {

            btn.RaiseEvent( new RoutedEventArgs( Button.ClickEvent ) );
        }
        #endregion
        #region Control Extensions
        // extension( Control ctrl ) {
        //    public CornerRadius CornerRadius {
        //        get {
        //            Border?      brd;


        //            brd                 = ctrl?.Template.FindName( "border", ctrl ) as Border;
        //            if( brd == null ) throw new ArgumentException( $"Specified control does not seem to have a border." );

        //            return brd.CornerRadius;
        //        }
        //        set {
        //            Border?      brd;


        //            brd                 = ctrl?.Template.FindName( "border", ctrl ) as Border;
        //            if( brd == null ) throw new ArgumentException( $"Specified control does not seem to have a border." );

        //            brd.CornerRadius    = value;
        //        }
        //    }
        //}
        #endregion
        #region ComboBox Extensions
        extension( ComboBox cbo ) {
            public int MaxInputLength {
                get {
                    TextBox         txt;


                    txt             = (TextBox)cbo.Template.FindName( "PART_EditableTextBox", cbo );

                    return txt.MaxLength;
                }
                set {
                    TextBox         txt;


                    txt             = (TextBox)cbo.Template.FindName( "PART_EditableTextBox", cbo );
                    txt.MaxLength   = value;
                }
            }
        }
        #endregion
        #region Drawing And DrawingGroup Extensions
        extension( DrawingGroup group ) {

            public Rect RenderBounds {
                get {
                    Rect    result  = Rect.Empty;
                    

                    foreach( Drawing d in group.Children ) {
                        if( d is GeometryDrawing gd ) {
                            result.Union( gd.Geometry.GetRenderBounds( gd.Pen ) );
                        }
                    }

                    return result;
                }
            }
        }

        public static void Normalize( this DrawingGroup group, double adjustX, double adjustY ) {

            TranslateTransform  normalize   = new TranslateTransform( adjustX, adjustY );

            foreach( Drawing d in group.Children ) {
                if( d is GeometryDrawing gd ) {
                    gd.Geometry                 = gd.Geometry.Clone();
                    gd.Geometry.Transform       = normalize;
                }
            }
        }
        #endregion
        #region Geometry Extensions
        extension( Geometry geo ) {
            public FillRule? FillRule {
                get {
                    FillRule?   result  = null;
                    
                    if( geo is PathGeometry pg ) {
                        result = pg.FillRule;
                    }

                    return result;
                }
            }
        }

        public static string ToPathMarkup( this Geometry geo ) {

            string  result  = geo.ToString();


            if( result.StartsWith( "F0", StringComparison.InvariantCultureIgnoreCase ) || result.StartsWith( "F1", StringComparison.InvariantCultureIgnoreCase ) ) {
                result = result.Substring( 2 );
            }

            return result.Trim();
        }
        #endregion
        #region Grid Extensions
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

        public static UIElement         AddText( this win.Grid grd, string textBlockText, int row = 0, int column = 0, int rowSpan = 1, int colSpan = 1,
                                                  Brush? foreground = null, HorizontalAlignment horizontalAlignment = HorizontalAlignment.Left ) {

            TextBlock   tb          = new();


            tb.Text                 = textBlockText;
            tb.Foreground           = foreground ?? Brushes.Black;
            tb.HorizontalAlignment  = horizontalAlignment;
            tb.VerticalAlignment    = VerticalAlignment.Center;

            return AddChild( grd, tb, row, column, rowSpan, colSpan );
        }
        public static UIElement         AddChild( this win.Grid grd, UIElement control, int row = 0, int column = 0, int rowSpan = 1, int colSpan = 1 ) {

            grd.Children.Add( control );

            Grid.SetRow( control, row );
            Grid.SetColumn( control, column );
            Grid.SetRowSpan( control, rowSpan );
            Grid.SetColumnSpan( control, colSpan );

            return control;
        }
        public static Control           AddChild( this win.Grid grd, Control control, int row = 0, int column = 0, int rowSpan = 1, int colSpan = 1 ) {

            grd.Children.Add( control );

            Grid.SetRow( control, row );
            Grid.SetColumn( control, column );
            Grid.SetRowSpan( control, rowSpan );
            Grid.SetColumnSpan( control, colSpan );

            return control;
        }
        #endregion
        #region ListView Extensions
        /// <summary>Populates a ListView control with columns and items based on the structure and data of the specified
        /// DataTable, using a GridView view.</summary>
        /// <remarks>This method clears any existing columns and items in the ListView before adding new
        /// ones based on the DataTable. Each DataTable column becomes a GridViewColumn, and each DataRow is added as an
        /// item. The method requires that the ListView is configured to use a GridView; otherwise, an exception is
        /// thrown.</remarks>
        /// <param name="lvw">The ListView control to populate. Must have its View property set to a GridView.</param>
        /// <param name="dt">The DataTable whose columns and rows are used to create the ListView's columns and items. Cannot be null.</param>
        /// <exception cref="InvalidOperationException">Thrown if the ListView's View property is not set to a GridView.</exception>
        public static void FromDataTable( this ListView lvw, DataTable dt ) {

            //FUTURE: Use styles to have some columns right justified depending upon data type.

            GridViewColumn      gvc;


            ArgumentNullException.ThrowIfNull( lvw, nameof(lvw) );
            ArgumentNullException.ThrowIfNull(  dt, nameof(dt) );

            if( lvw.View is GridView gv ) {
                gv.Columns.Clear();
                lvw.Items.Clear();

                for( int index = 0; index < dt.Columns.Count; index++ ) {
                    gvc                         = new GridViewColumn();
                    gvc.Header                  = dt.Columns[index].ColumnName;
                    gvc.DisplayMemberBinding    = new Binding( $"[{index}]" );

                    gv.Columns.Add( gvc );
                }

                foreach( DataRow dr in dt.Rows ) {
                    lvw.Items.Add( dr.ItemArray );
                }
            }
            else {
                throw new InvalidOperationException( $"ListView {lvw.Name}'s view is not set to GridView." );
            }
        }
        #endregion
    }
}