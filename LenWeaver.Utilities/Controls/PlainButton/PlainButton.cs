using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LenWeaver.Utilities {

    public class PlainButton : Button {

        protected internal  Brush?                  beforeMouseEnter        = null;
        protected internal  ContentPresenter?       partContent             = null;


        #region Static Members
        public static readonly      DependencyProperty MouseOverBackgroundProperty =
                                    DependencyProperty.Register( nameof(MouseOverBackground), typeof(Brush), typeof(PlainButton),
                                                                 new PropertyMetadata( Brushes.Transparent ) );

        public static readonly      DependencyProperty MouseOverBorderBrushProperty =
                                    DependencyProperty.Register( nameof(MouseOverBorderBrush), typeof(Brush), typeof(PlainButton),
                                                                 new PropertyMetadata( null ) );

        public static readonly      DependencyProperty MouseOverForegroundProperty =
                                    DependencyProperty.Register( nameof(MouseOverForeground), typeof(Brush), typeof(PlainButton),
                                                                 new PropertyMetadata( null ) );

        public static readonly      DependencyProperty SymbolCharacterProperty =
                                    DependencyProperty.Register( nameof(SymbolCharacter), typeof(string), typeof(PlainButton),
                                                                 new PropertyMetadata( null ) );
        
        public static readonly      DependencyProperty SymbolPathFillProperty =
                                    DependencyProperty.Register( nameof(SymbolPathFill), typeof(Brush), typeof(PlainButton),
                                                                 new PropertyMetadata( Brushes.Transparent ) );

        public static readonly      DependencyProperty SymbolPathMarkupProperty =
                                    DependencyProperty.Register( nameof(SymbolPathMarkup), typeof(string), typeof(PlainButton),
                                                                 new PropertyMetadata( null ) );

        public static readonly      DependencyProperty SymbolPathStrokeProperty =
                                    DependencyProperty.Register( nameof(SymbolPathStroke), typeof(Brush), typeof(PlainButton),
                                                                 new PropertyMetadata( Brushes.Black ) );

        public static readonly      DependencyProperty SymbolPathStrokeThicknessProperty =
                                    DependencyProperty.Register( nameof(SymbolPathStrokeThickness), typeof(double), typeof(PlainButton),
                                                                 new PropertyMetadata( 1d ) );

        public static readonly      DependencyProperty SymbolImageSourceProperty =
                                    DependencyProperty.Register( nameof(SymbolImageSource), typeof(ImageSource), typeof(PlainButton),
                                                                 new PropertyMetadata( null ) );
        #endregion


        static PlainButton() {
            
            DefaultStyleKeyProperty.OverrideMetadata( typeof(PlainButton), new FrameworkPropertyMetadata( typeof(PlainButton) ) );
            PlainButton.ContentProperty.OverrideMetadata( typeof(PlainButton), new FrameworkPropertyMetadata( Content_Changed ) );
        }

        public PlainButton() : base() {

            MouseEnter              += PlainButton_MouseEnter;
            MouseLeave              += PlainButton_MouseLeave;

            Loaded += (s,e) => {
                partContent         = (ContentPresenter?)this.Template?.FindName( "PART_Content", this );
            };
        }
        

        public double       SymbolPathStrokeThickness {
            get { return (double)GetValue( SymbolPathStrokeThicknessProperty ); }
            set { SetValue( SymbolPathStrokeThicknessProperty, value ); }
        }
        public string       SymbolCharacter {
            get { return (string)GetValue( SymbolCharacterProperty ); }
            set { SetValue( SymbolCharacterProperty, value ); }
        }
        public string       SymbolPathMarkup {
            get { return (string)GetValue( SymbolPathMarkupProperty ); }
            set { SetValue( SymbolPathMarkupProperty, value ); }
        }
        public Brush        MouseOverBackground {
            get { return (Brush)GetValue( MouseOverBackgroundProperty ); }
            set { SetValue( MouseOverBackgroundProperty, value ); }
        }
        public Brush        MouseOverBorderBrush {
            get { return (Brush)GetValue( MouseOverBorderBrushProperty ); }
            set { SetValue( MouseOverBorderBrushProperty, value ); }
        }
        public Brush        MouseOverForeground {
            get { return (Brush)GetValue( MouseOverForegroundProperty ); }
            set { SetValue( MouseOverForegroundProperty, value ); }
        }
        public Brush        SymbolPathFill {
            get { return (Brush)GetValue( SymbolPathFillProperty ); }
            set { SetValue( SymbolPathFillProperty, value ); }
        }
        public Brush        SymbolPathStroke {
            get { return (Brush)GetValue( SymbolPathStrokeProperty ); }
            set { SetValue( SymbolPathStrokeProperty, value ); }
        }
        public ImageSource  SymbolImageSource {
            get { return (ImageSource)GetValue( SymbolImageSourceProperty ); }
            set { SetValue( SymbolImageSourceProperty, value ); }
        }


        private void PlainButton_MouseEnter( object sender, MouseEventArgs e ) {

            if( MouseOverForeground != null ) {
                beforeMouseEnter    = Foreground;
                Foreground          = MouseOverForeground;
            }
        }
        private void PlainButton_MouseLeave( object sender, MouseEventArgs e ) {

            if( beforeMouseEnter != null ) Foreground = beforeMouseEnter;
        }

        private static void Content_Changed( DependencyObject d, DependencyPropertyChangedEventArgs e ) {

            TextBlock       tb;


            if( d is PlainButton btn && e.NewValue is string s && btn.partContent != null ) {
                tb = new TextBlock();
                tb.Text = s;

                btn.partContent.Content = tb;
            }
            else {
                d.SetValue( e.Property, e.NewValue );
            }
        }
    }
}