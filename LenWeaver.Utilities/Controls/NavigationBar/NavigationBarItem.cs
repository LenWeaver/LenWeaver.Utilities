using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LenWeaver.Utilities {

    public class NavigationBarItem : Button {

        protected                       NavigationBar?          parentNavigationBar     = null;


        #region Static Members
        public static readonly          DependencyProperty      ExtraIndicatorProperty =
                                        DependencyProperty.Register( nameof(ExtraIndicator), typeof(Brush), typeof(NavigationBarItem),
                                        new PropertyMetadata( null ) );

        public static readonly          DependencyProperty      ImageSideProperty =
                                        DependencyProperty.Register( nameof(ImageSide), typeof(ImageSide), typeof(NavigationBarItem),
                                        new PropertyMetadata( ImageSide.LeftSide, ImageSide_Changed, ImageSide_CoerceValue ) );

        public static readonly          DependencyProperty      MouseOverBackgroundProperty =
                                        DependencyProperty.Register( nameof(MouseOverBackground), typeof(Brush), typeof(NavigationBarItem),
                                        new PropertyMetadata( null ) );

        public static readonly          DependencyProperty      SymbolPathFillProperty =
                                        DependencyProperty.Register( nameof(SymbolPathFill), typeof(Brush), typeof(NavigationBarItem),
                                        new PropertyMetadata( Brushes.White ) );

        public static readonly          DependencyProperty      SelectedIndicatorProperty =
                                        DependencyProperty.Register( nameof(SelectedIndicator), typeof(Brush), typeof(NavigationBarItem),
                                        new PropertyMetadata( Brushes.Transparent ) );

        public static readonly          DependencyProperty      SymbolCharacterProperty =
                                        DependencyProperty.Register( nameof(SymbolCharacter), typeof(string), typeof(NavigationBarItem),
                                        new PropertyMetadata( String.Empty ) );

        public static readonly          DependencyProperty      SymbolFontFamilyProperty =
                                        DependencyProperty.Register( nameof(SymbolFontFamily), typeof(FontFamily), typeof(NavigationBarItem),
                                        new PropertyMetadata( new FontFamily( "Segoe MDL2 Assets") ) );

        public static readonly          DependencyProperty      SymbolForegroundProperty =
                                        DependencyProperty.Register( nameof(SymbolForeground), typeof(Brush), typeof(NavigationBarItem),
                                        new PropertyMetadata( Brushes.Black ) );

        public static readonly          DependencyProperty      SymbolImageSourceProperty =
                                        DependencyProperty.Register( nameof(SymbolImageSource), typeof(ImageSource), typeof(NavigationBarItem),
                                        new PropertyMetadata( null ) );

        public static readonly          DependencyProperty      SymbolPaddingProperty =
                                        DependencyProperty.Register( nameof(SymbolPadding), typeof(Thickness), typeof(NavigationBarItem),
                                        new PropertyMetadata( new Thickness( 0d ) ) );

        public static readonly          DependencyProperty      SymbolPathMarkupProperty =
                                        DependencyProperty.Register( nameof(SymbolPathMarkup), typeof(string), typeof(NavigationBarItem),
                                        new PropertyMetadata( null ) );

        public static readonly          DependencyProperty      SymbolPathStrokeProperty =
                                        DependencyProperty.Register( nameof(SymbolPathStroke), typeof(Brush), typeof(NavigationBarItem),
                                        new PropertyMetadata( null ) );

        public static readonly          DependencyProperty      SymbolPathStrokeThicknessProperty =
                                        DependencyProperty.Register( nameof(SymbolPathStrokeThickness), typeof(double), typeof(NavigationBarItem),
                                        new PropertyMetadata( 1d ) );

        public static readonly          DependencyProperty      TextProperty =
                                        DependencyProperty.Register( nameof(Text), typeof(string), typeof(NavigationBarItem),
                                        new PropertyMetadata( String.Empty ) );


        private static void     ImageSide_Changed       ( DependencyObject d, DependencyPropertyChangedEventArgs e ) {}
        private static object   ImageSide_CoerceValue   ( DependencyObject d, object baseValue ) {
            
            int         asInteger;


            asInteger = (int)baseValue;

            return (ImageSide)asInteger;
        }


        static NavigationBarItem() {

            DefaultStyleKeyProperty.OverrideMetadata( typeof(NavigationBarItem), new FrameworkPropertyMetadata( typeof(NavigationBarItem) ) );
        }
        #endregion


        public NavigationBarItem() {

            Click += NavigationBarItem_Click;
        }
                

        public double           SymbolPathStrokeThickness {
            get { return (double)GetValue( SymbolPathStrokeThicknessProperty ); }
            set { SetValue( SymbolPathStrokeThicknessProperty, value ); }
        }
        public string           SymbolPathMarkup {
            get { return (string)GetValue( SymbolPathMarkupProperty); }
            set { SetValue( SymbolPathMarkupProperty, value ); }
        }
        public string           SymbolCharacter {
            get { return (string)GetValue( SymbolCharacterProperty ); }
            set { SetValue( SymbolCharacterProperty, value ); }
        }
        public string           Text {
            get { return (string)GetValue( TextProperty ); }
            set { SetValue( TextProperty, value ); }
        }
        public ImageSide        ImageSide {
            get { return (ImageSide)GetValue( ImageSideProperty ); }
            set { SetValue( ImageSideProperty, value ); }
        }
        public Thickness        SymbolPadding {
            get { return (Thickness)GetValue( SymbolPaddingProperty ); }
            set { SetValue( SymbolPaddingProperty, value ); }
        }
        public Brush            ExtraIndicator {
            get { return (Brush)GetValue( ExtraIndicatorProperty ); }
            set { SetValue( ExtraIndicatorProperty, value ); }
        }
        public Brush            MouseOverBackground {
            get { return (Brush)GetValue( MouseOverBackgroundProperty ); }
            set { SetValue( MouseOverBackgroundProperty, value ); }
        }
        public Brush            SymbolForeground {
            get { return (Brush)GetValue( SymbolForegroundProperty ); }
            set { SetValue( SymbolForegroundProperty, value ); }
        }
        public Brush            SelectedIndicator {
            get { return (Brush)GetValue( SelectedIndicatorProperty ); }
            set { SetValue( SelectedIndicatorProperty, value ); }
        }
        public Brush            SymbolPathFill {
            get { return (Brush)GetValue( SymbolPathFillProperty ); }
            set { SetValue( SymbolPathFillProperty, value ); }
        }
        public Brush            SymbolPathStroke {
            get { return (Brush)GetValue( SymbolPathStrokeProperty ); }
            set { SetValue( SymbolPathStrokeProperty, value ); }
        }
        public FontFamily       SymbolFontFamily {
            get { return (FontFamily)GetValue( SymbolFontFamilyProperty ); }
            set { SetValue( SymbolFontFamilyProperty, value ); }
        }
        public ImageSource      SymbolImageSource {
            get { return (ImageSource)GetValue( SymbolImageSourceProperty ); }
            set { SetValue( SymbolImageSourceProperty, value ); }
        }
        public NavigationBar?   ParentNavigationBar {
            get { return parentNavigationBar; }
            set { parentNavigationBar = value; }
        }


        public override string ToString() {

            return $"Text: {Text}";
        }


        private void NavigationBarItem_Click( object sender, RoutedEventArgs e ) {
            
            if( parentNavigationBar != null ) {
                parentNavigationBar.SelectedItem = this;
            }
        }
    }
}