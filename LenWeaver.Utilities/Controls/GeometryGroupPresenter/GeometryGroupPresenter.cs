using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Media;


namespace LenWeaver.Utilities {

    public class GeometryGroupPresenter : FrameworkElement {

        private string                                  pathCache       = String.Empty;

        private Rect                                    drawingRect     = Rect.Empty;
        private Size                                    renderSize      = Size.Empty;

        private readonly        DrawingGroup            drawingGroup    = new DrawingGroup();
        private readonly        PlacementDescriptor     placement       = new PlacementDescriptor();
        private readonly        TransformGroup          transform       = new TransformGroup();


        #region Dependency Property Declarations
        public static readonly  DependencyProperty  ExtendedPathMarkupProperty =
                                DependencyProperty.Register( nameof(ExtendedPathMarkup),
                                                             typeof(string),
                                                             typeof(GeometryGroupPresenter),
                                                             new FrameworkPropertyMetadata( String.Empty,
                                                                                            FrameworkPropertyMetadataOptions.AffectsMeasure |
                                                                                            FrameworkPropertyMetadataOptions.AffectsArrange |
                                                                                            FrameworkPropertyMetadataOptions.AffectsRender,
                                                                                            ExtendedPathMarkup_Changed ) );

        public static readonly  DependencyProperty  StretchProperty =
                                DependencyProperty.Register( nameof(Stretch),
                                                             typeof(Stretch),
                                                             typeof(GeometryGroupPresenter),
                                                             new FrameworkPropertyMetadata( Stretch.None,
                                                                                            FrameworkPropertyMetadataOptions.AffectsArrange |
                                                                                            FrameworkPropertyMetadataOptions.AffectsRender ) );
        #endregion


        static GeometryGroupPresenter() {
        
            HorizontalAlignmentProperty.OverrideMetadata( typeof(GeometryGroupPresenter),
                                                          new FrameworkPropertyMetadata( HorizontalAlignment.Left,
                                                                                         FrameworkPropertyMetadataOptions.AffectsArrange |
                                                                                         FrameworkPropertyMetadataOptions.AffectsRender ) );

            VerticalAlignmentProperty.OverrideMetadata(   typeof(GeometryGroupPresenter),
                                                          new FrameworkPropertyMetadata( VerticalAlignment.Top,
                                                                                         FrameworkPropertyMetadataOptions.AffectsArrange |
                                                                                         FrameworkPropertyMetadataOptions.AffectsRender ) );
        }

        public GeometryGroupPresenter() {}


        public string ExtendedPathMarkup {
            get => (string)GetValue( ExtendedPathMarkupProperty );
            set => SetValue( ExtendedPathMarkupProperty, value?.Trim() ?? String.Empty );
        }
        public Stretch Stretch {
            get => (Stretch)GetValue( StretchProperty );
            set => SetValue( StretchProperty, value );
        }


        protected void InvalidateGeometry() {

            string              extendedMarkup;

            GeometryDrawing     gd;


            try {
                extendedMarkup              = ExtendedPathMarkup;

                if( !extendedMarkup.Equals( pathCache, StringComparison.Ordinal ) ) {
                    drawingGroup.Children.Clear();

                    if( !String.IsNullOrWhiteSpace( extendedMarkup ) ) {
                        foreach( GeometryDrawingDescriptor gdd in GeometryDrawingDescriptor.Parse( extendedMarkup ) ) {
                            gd              = new GeometryDrawing();
                            
                            gd.Brush        = gdd.Brush;
                            gd.Pen          = new Pen( gdd.PenBrush, gdd.PenThickness );
                            gd.Geometry     = gdd.Geometry.Clone();

                            drawingGroup.Children.Add( gd );
                        }

                        drawingRect         = drawingGroup.RenderBounds;

                        if( drawingRect.Left != 0 || drawingRect.Top != 0 ) {
                            drawingGroup.Normalize( -drawingRect.Left, -drawingRect.Top );
                        }

                        pathCache           = extendedMarkup;
                        drawingRect         = new Rect( 0d, 0d, drawingRect.Width, drawingRect.Height );
                    }
                }

                InvalidateMeasure();
                InvalidateArrange();
            }
            catch( Exception ex ) {
                throw new ApplicationException( $"{nameof(InvalidateGeometry)} method failed.", ex );
            }
        }


        protected override void OnRender( DrawingContext drawingContext ) {

            if( drawingGroup.Children.Count > 0 ) {
                transform.Children.Clear();
                transform.Children.Add( new ScaleTransform( placement.ScaleX, placement.ScaleY ) );
                transform.Children.Add( new TranslateTransform( placement.Left, placement.Top ) );

                drawingGroup.Transform = transform;
                drawingContext.DrawDrawing( drawingGroup );
            }
        }

        protected override Size ArrangeOverride( Size finalSize ) {

            double      naturalHeight;
            double      naturalWidth;
            double      scaledHeight;
            double      scaledWidth;


            renderSize = finalSize;

            if (!renderSize.IsEmpty && !drawingRect.IsEmpty) {
                naturalWidth        = drawingRect.Width;
                naturalHeight       = drawingRect.Height;

                placement.ScaleX    = HorizontalAlignment == HorizontalAlignment.Stretch || naturalWidth > finalSize.Width
                                                           ? finalSize.Width / naturalWidth
                                                           : 1.0;

                placement.ScaleY    = VerticalAlignment == VerticalAlignment.Stretch || naturalHeight > finalSize.Height
                                                           ? finalSize.Height / naturalHeight
                                                           : 1.0;

                scaledWidth         = naturalWidth * placement.ScaleX;
                scaledHeight        = naturalHeight * placement.ScaleY;

                placement.Left = HorizontalAlignment switch {
                    HorizontalAlignment.Center  => (finalSize.Width - scaledWidth) / 2,
                    HorizontalAlignment.Right   => finalSize.Width - scaledWidth,
                    _ => 0
                };

                placement.Top = VerticalAlignment switch {
                    VerticalAlignment.Center    => (finalSize.Height - scaledHeight) / 2,
                    VerticalAlignment.Bottom    => finalSize.Height - scaledHeight,
                    _ => 0
                };
            }

            return finalSize;
        }
        protected override Size MeasureOverride( Size availableSize ) {

            double      height;
            double      width;

            Size        result;

            
            if( !drawingRect.IsEmpty ) {
                result              = new Size( 0d, 0d );
            }
            else {
                width               = Math.Min( availableSize.Width,    drawingRect.Width );
                height              = Math.Min( availableSize.Height,   drawingRect.Height );

                result              = new Size( width, height );
            }

            return result;
        }


        private static void ExtendedPathMarkup_Changed( DependencyObject d, DependencyPropertyChangedEventArgs e ) {
            
            ((GeometryGroupPresenter)d).InvalidateGeometry();
        }
    }
}