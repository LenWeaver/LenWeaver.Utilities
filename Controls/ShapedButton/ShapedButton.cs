using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace LenWeaver.Utilities {
    
    public class ShapedButton : Button {

        public static readonly string   DefaultMarkup           = "M0,0 100,0 100,100 0,100 Z";
        public static readonly string   ShapeArrowLeft          = "M10,0 0,50 10,100 100,100 100,0 Z";
        public static readonly string   ShapeArrowRight         = "M0,0 0,100 90,100 100,50 90,0 Z";
        public static readonly string   ShapeCircle             = "M0,0 A180,180 180 1 1 1,1 Z";
        public static readonly string   ShapeCanadianMapleLeaf  = "M0,-400 78,-275 150,-302 122,-89 216,-171 249,-127 360,-137 335,-18 372,13 192,176 203,244 12,232 18,406 -18,406 -12,232 -203,244 -192,176 -372,13 -335,-18 -360,-137 -249,-127 -216,-171 -122,-89 -150,-302 -78,-275 Z";
        public static readonly string   ShapeOctagon            = "M20,0 0,20 0,80 20,100 80,100, 100,80 100,20 80,0 Z";
        public static readonly string   PathName                = "buttonPath";


        #region DependencyProperty declaration and static constructor
        public static readonly  DependencyProperty FillBrushProperty
                                = DependencyProperty.Register( nameof(FillBrush), typeof(Brush), typeof(ShapedButton),
                                new FrameworkPropertyMetadata( defaultValue: Brushes.Black,
                                                                      flags: FrameworkPropertyMetadataOptions.AffectsMeasure |
                                                                             FrameworkPropertyMetadataOptions.AffectsRender,
                                                    propertyChangedCallback: ShapeMarkup_Changed ) );

        public static readonly  DependencyProperty StrokeBrushProperty
                                = DependencyProperty.Register( nameof(StrokeBrush), typeof(Brush), typeof(ShapedButton),
                                new FrameworkPropertyMetadata( defaultValue: Brushes.Black,
                                                                      flags: FrameworkPropertyMetadataOptions.AffectsMeasure |
                                                                             FrameworkPropertyMetadataOptions.AffectsRender,
                                                    propertyChangedCallback: ShapeMarkup_Changed ) );

        public static readonly  DependencyProperty ShapeMarkupProperty 
                                = DependencyProperty.Register( nameof(ShapeMarkup), typeof(string), typeof(ShapedButton),
                                new FrameworkPropertyMetadata( defaultValue: ShapedButton.DefaultMarkup,
                                                                      flags: FrameworkPropertyMetadataOptions.AffectsMeasure |
                                                                             FrameworkPropertyMetadataOptions.AffectsRender,
                                                    propertyChangedCallback: ShapeMarkup_Changed ) );
        

        static ShapedButton() {

            DefaultStyleKeyProperty.OverrideMetadata( typeof(ShapedButton), new FrameworkPropertyMetadata( typeof(ShapedButton) ) );
        }


        private static void ApplyPathMarkup( ShapedButton btn, string pathMarkup ) {

            GeometryPresenter   gp;


            if( btn.Template != null ) {
                gp   = (GeometryPresenter)btn.Template.FindName( ShapedButton.PathName, btn );
                if( gp != null ) {
                    gp.PathMarkup = pathMarkup;
                }
            }
        }

        private static void ShapeMarkup_Changed( DependencyObject d, DependencyPropertyChangedEventArgs e ) {

            ShapedButton        btn;


            btn         = (ShapedButton)d;

            ShapedButton.ApplyPathMarkup( btn, e.NewValue?.ToString() ?? ShapedButton.DefaultMarkup );
        }
        #endregion

        
        public ShapedButton() : base() {
        
            base.Loaded += (sender, _) => {
                    ShapedButton    btn = (ShapedButton)sender;

                    btn.ShapeMarkup += String.Empty;
                };
        }


        public string ShapeMarkup {
            get => (string)GetValue( ShapeMarkupProperty );
            set => SetValue( ShapeMarkupProperty, value );
        }

        public Brush FillBrush {
            get => (Brush)GetValue( FillBrushProperty );
            set => SetValue( FillBrushProperty, value );
        }
        public Brush StrokeBrush {
            get => (Brush)GetValue( StrokeBrushProperty );
            set => SetValue( StrokeBrushProperty, value );
        }
    }
}