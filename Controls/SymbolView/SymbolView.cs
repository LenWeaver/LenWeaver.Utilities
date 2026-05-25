using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace LenWeaver.Utilities {

    [TemplatePart( Name = "PART_Character",             Type = typeof(TextBlock) )]
    [TemplatePart( Name = "PART_Image",                 Type = typeof(Image) )]
    [TemplatePart( Name = "PART_PathMarkup",            Type = typeof(System.Windows.Shapes.Path) )]
    [TemplatePart( Name = "PART_ExtendedPathMarkup",    Type = typeof(GeometryGroupPresenter) )]
    public class SymbolView : Control {

        protected                       SymbolViewSource        symbolSource                = SymbolViewSource.None;

        protected                       Viewbox?                vbCharacter                 = null;
        protected                       Viewbox?                vbExtendedPathMarkup        = null;
        protected                       Viewbox?                vbImage                     = null;
        protected                       Viewbox?                vbPath                      = null;


        #region Static Members
        private static readonly         double                  DefaultPathStrokeThickness  = 1d;
        private static readonly         string?                 DefaultCharacter            = null;
        private static readonly         string?                 DefaultExtendedPathMarkup   = String.Empty;
        private static readonly         string?                 DefaultPathMarkup           = null;
        private static readonly         Brush                   DefaultPathFill             = Brushes.Red;
        private static readonly         Brush                   DefaultPathStroke           = Brushes.Black;
        private static readonly         ImageSource?            DefaultImageSource          = null;


        public static readonly          DependencyProperty      CharacterProperty =
                                        DependencyProperty.Register( nameof(Character), typeof(string), typeof(SymbolView),
                                        new PropertyMetadata( defaultValue: DefaultCharacter,
                                                   propertyChangedCallback: Character_Changed) );

        public static readonly          DependencyProperty      ExtendedPathMarkupProperty =
                                        DependencyProperty.Register( nameof(ExtendedPathMarkup), typeof(string), typeof(SymbolView),
                                        new PropertyMetadata( defaultValue: DefaultExtendedPathMarkup,
                                                   propertyChangedCallback: ExtendedPathMarkup_Changed ) );

        public static readonly          DependencyProperty      PathFillProperty =
                                        DependencyProperty.Register( nameof(PathFill), typeof(Brush), typeof(SymbolView),
                                        new PropertyMetadata( defaultValue: DefaultPathFill,
                                                   propertyChangedCallback: PathFill_Changed ) );

        public static readonly          DependencyProperty      PathMarkupProperty =
                                        DependencyProperty.Register( nameof(PathMarkup), typeof(string), typeof(SymbolView),
                                        new PropertyMetadata( defaultValue: DefaultPathMarkup,
                                                   propertyChangedCallback: PathMarkup_Changed ) );

        public static readonly          DependencyProperty      PathStrokeProperty = 
                                        DependencyProperty.Register( nameof(PathStroke), typeof(Brush), typeof(SymbolView),
                                        new PropertyMetadata( defaultValue: DefaultPathStroke,
                                                   propertyChangedCallback: PathStroke_Changed ) );

        public static readonly          DependencyProperty      PathStrokeThicknessProperty = 
                                        DependencyProperty.Register( nameof(PathStrokeThickness), typeof(double), typeof(SymbolView),
                                        new PropertyMetadata( defaultValue: DefaultPathStrokeThickness,
                                                   propertyChangedCallback: PathStrokeThickess_Changed ) );

        public static readonly          DependencyProperty      ImageSourceProperty = 
                                        DependencyProperty.Register( nameof(ImageSource), typeof(ImageSource), typeof(SymbolView),
                                        new PropertyMetadata( defaultValue: DefaultImageSource,
                                                   propertyChangedCallback: ImageSource_Changed ) );
        

        private static void Character_Changed                   ( DependencyObject d, DependencyPropertyChangedEventArgs e ) {
            
            if( d is SymbolView sv && e.NewValue != null ) {
                sv.RevealViewbox( SymbolViewSource.Character );
            }
        }
        private static void ExtendedPathMarkup_Changed          ( DependencyObject d, DependencyPropertyChangedEventArgs e ) {

            if( d is SymbolView sv && e.NewValue != null ) {
                sv.RevealViewbox( SymbolViewSource.ExtendedPath );
            }
        }
        private static void ImageSource_Changed                 ( DependencyObject d, DependencyPropertyChangedEventArgs e ) {
        
            if( d is SymbolView sv && e.NewValue != null ) {
                sv.RevealViewbox( SymbolViewSource.Image );
            }
        }
        private static void PathFill_Changed                    ( DependencyObject d, DependencyPropertyChangedEventArgs e ) {}
        private static void PathMarkup_Changed                  ( DependencyObject d, DependencyPropertyChangedEventArgs e ) {
            
            if( d is SymbolView sv && e.NewValue != null ) {
                sv.RevealViewbox( SymbolViewSource.Path );
            }
        }
        private static void PathStroke_Changed                  ( DependencyObject d, DependencyPropertyChangedEventArgs e ) {}
        private static void PathStrokeThickess_Changed          ( DependencyObject d, DependencyPropertyChangedEventArgs e ) {}
        #endregion


        static SymbolView() {

            DefaultStyleKeyProperty.OverrideMetadata( typeof(SymbolView), new FrameworkPropertyMetadata( typeof(SymbolView) ) );
        }

        public SymbolView() : base() {

            Loaded += (sender, e) => {

                vbCharacter                     = this.Template.FindName( nameof(vbCharacter),          this ) as Viewbox;
                vbImage                         = this.Template.FindName( nameof(vbImage),              this ) as Viewbox;
                vbPath                          = this.Template.FindName( nameof(vbPath),               this ) as Viewbox;
                vbExtendedPathMarkup            = this.Template.FindName( nameof(vbExtendedPathMarkup), this ) as Viewbox;

                RevealViewbox( symbolSource );
            };
        }


        protected void RevealViewbox( SymbolViewSource svs ) {

            if( vbCharacter != null && vbImage != null && vbPath != null && vbExtendedPathMarkup != null ) { 
                vbCharacter.Visibility          = svs   == SymbolViewSource.Character       ? Visibility.Visible    : Visibility.Collapsed;
                vbImage.Visibility              = svs   == SymbolViewSource.Image           ? Visibility.Visible    : Visibility.Collapsed;
                vbPath.Visibility               = svs   == SymbolViewSource.Path            ? Visibility.Visible    : Visibility.Collapsed;
                vbExtendedPathMarkup.Visibility = svs   == SymbolViewSource.ExtendedPath    ? Visibility.Visible    : Visibility.Collapsed;
            }
            else {
                symbolSource                    = svs;
            }
        }


        public      double          PathStrokeThickness {
            get => (double)GetValue( PathStrokeThicknessProperty );
            set => SetValue( PathStrokeThicknessProperty, value );
        }
        public      string          Character           {
            get => (string)GetValue( CharacterProperty );
            set => SetValue( CharacterProperty, value );
        }
        public      string          ExtendedPathMarkup  {
            get => (string)GetValue( ExtendedPathMarkupProperty );
            set => SetValue( ExtendedPathMarkupProperty, value );
        }
        public      string          PathMarkup          {
            get => (string)GetValue( PathMarkupProperty );
            set => SetValue( PathMarkupProperty, value );
        }
        public      Brush           PathFill            {
            get => (Brush)GetValue( PathFillProperty );
            set => SetValue( PathFillProperty, value );
        }
        public      Brush           PathStroke          {
            get => (Brush)GetValue( PathStrokeProperty );
            set => SetValue( PathStrokeProperty, value );
        }
        public      ImageSource     ImageSource         {
            get => (ImageSource)GetValue( ImageSourceProperty );
            set => SetValue( ImageSourceProperty, value );
        }
    }
}