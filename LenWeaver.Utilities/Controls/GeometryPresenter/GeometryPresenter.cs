using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Ink;
using System.Windows.Media;

namespace LenWeaver.Utilities {

    public class GeometryPresenter : FrameworkElement {

        private Geometry?       normalizedGeometry      = null;


        #region DependencyProperty Declarations
        public static readonly  DependencyProperty StrokeThicknessProperty
                                = DependencyProperty.Register( nameof(StrokeThickness),
                                                               typeof(double),
                                                               typeof(GeometryPresenter),
                                                               new FrameworkPropertyMetadata( 1d,
                                                                                              FrameworkPropertyMetadataOptions.AffectsRender,
                                                                                              PathMarkup_Changed ) );

        public static readonly  DependencyProperty FillBrushProperty
                                = DependencyProperty.Register( nameof(FillBrush),
                                                               typeof(Brush),
                                                               typeof(GeometryPresenter),
                                                               new FrameworkPropertyMetadata( Brushes.Transparent,
                                                                                              FrameworkPropertyMetadataOptions.AffectsRender,
                                                                                              PathMarkup_Changed ) );

        public static readonly  DependencyProperty StrokeBrushProperty
                                = DependencyProperty.Register( nameof(StrokeBrush),
                                                               typeof(Brush),
                                                               typeof(GeometryPresenter),
                                                               new FrameworkPropertyMetadata( Brushes.Transparent,
                                                                                              FrameworkPropertyMetadataOptions.AffectsRender,
                                                                                              PathMarkup_Changed ) );

        public static readonly  DependencyProperty PathMarkupProperty
                                = DependencyProperty.Register( nameof(PathMarkup),
                                                              typeof(string),
                                                              typeof(GeometryPresenter),
                                                              new FrameworkPropertyMetadata( null, 
                                                                                             FrameworkPropertyMetadataOptions.AffectsRender |
                                                                                             FrameworkPropertyMetadataOptions.AffectsMeasure,
                                                                                             PathMarkup_Changed ) );
        #endregion


        public double StrokeThickness {
            get => (double)GetValue( StrokeThicknessProperty );
            set => SetValue( StrokeThicknessProperty, value );
        }

        public string PathMarkup {
            get => (string)GetValue( PathMarkupProperty );
            set => SetValue( PathMarkupProperty, value );
        }

        public Brush FillBrush {
            get => (Brush)GetValue( FillBrushProperty );
            set => SetValue( FillBrushProperty, value );
        }
        public Brush StrokeBrush {
            get => (Brush)GetValue( StrokeBrushProperty );
            set => SetValue( StrokeBrushProperty, value );
        }

        
        protected override void OnRender( DrawingContext dc ) {

            if( normalizedGeometry != null ) {
                dc.DrawGeometry( FillBrush, new Pen( StrokeBrush, StrokeThickness ), normalizedGeometry );
            }
        }
        protected override void OnRenderSizeChanged( SizeChangedInfo sizeInfo ) {

            base.OnRenderSizeChanged( sizeInfo );

            UpdateGeometry();
        }


        private void UpdateGeometry() {

            double          sx;
            double          sy;

            Rect            bounds;

            Geometry        g;
            TransformGroup  tg;


            if( String.IsNullOrWhiteSpace( PathMarkup ) ) {
                normalizedGeometry              = null;
            }
            else {
                g                               = Geometry.Parse( PathMarkup );
                bounds                          = g.Bounds;

                sx                              = ActualWidth / bounds.Width;
                sy                              = ActualHeight / bounds.Height;

                tg                              = new TransformGroup();
                tg.Children.Add( new TranslateTransform( -bounds.X, -bounds.Y ) );
                tg.Children.Add( new ScaleTransform( sx, sy ) );

                normalizedGeometry              = g.GetFlattenedPathGeometry().Clone();
                normalizedGeometry.Transform    = tg;
            }

            InvalidateVisual();
        }


        private static void PathMarkup_Changed( DependencyObject d, DependencyPropertyChangedEventArgs e ) {

            ((GeometryPresenter)d).UpdateGeometry();
        }
    }
}